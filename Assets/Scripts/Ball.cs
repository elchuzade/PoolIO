using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Ball : MonoBehaviourPunCallbacks
{
    FloatingJoystick joystick;

    [SerializeField] GameObject playerCamera;

    GameplayStatus gameplayStatus;
    [SerializeField] Rigidbody2D rb;
    // Ball Child collider
    [SerializeField] CircleCollider2D colTrigger;

    // Opposite to mouse drag direction
    Vector3 direction;

    [SerializeField] GameObject arrows;
    [SerializeField] GameObject arrowOne;
    [SerializeField] GameObject arrowTwo;
    [SerializeField] GameObject arrowThree;

    float baseSpeed = 100000;
    float speed = 0;
    // When score increases, the multiplier increases too
    float baseSpeedStep = 10000;
    float massStep = 50;

    // This is not to wait until the full stop
    float almostStopped = 100f;

    // When you are sucked in to the hole disable controls
    bool disabled = false;

    // To allow aiming
    bool idle = true;
    // To determine who touched you the last
    string touchedUserId;
    int score = 0;

    [SerializeField] GameObject canvasName;
    [SerializeField] GameObject canvasScore;

    // Position of the hole to suck the ball into
    Vector3 holePosition;
    float holeSuckSpeed = 2;
    // Distance to which the player needs to approach the hole center to reappear
    float holeCenterMargin = 10;
    bool mine = false;

    public string userId;
    public string nickname;

    void Awake()
    {
        gameplayStatus = FindObjectOfType<GameplayStatus>();
    }

    void Start()
    {
        SetCanvasScore();

        if (photonView.IsMine)
        {
            mine = true;
            // Hide what is not mine for every player
            joystick = GameObject.Find("PlayerJoystick").GetComponent<FloatingJoystick>();

            userId = PhotonNetwork.LocalPlayer.UserId;
            nickname = PhotonNetwork.LocalPlayer.NickName;

            GetComponent<BallInfo>().SetNickname(nickname);
            GetComponent<BallInfo>().SetUserId(userId);

            GetComponent<PhotonView>().RPC("NewPlayerJoined", RpcTarget.AllBuffered, userId, nickname, 0);
        } else
        {
            playerCamera.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (mine)
        {
            if (idle)
            {
                ShowArrows();
            }
        }
    }

    void Update()
    {
        if (mine)
        {
            if (disabled)
            {
                if (Vector2.Distance(transform.position, holePosition) > holeCenterMargin)
                {
                    Vector3 movePos = Vector3.MoveTowards(transform.position, holePosition, holeSuckSpeed);
                    transform.position = movePos;
                    transform.localScale *= 0.99f;
                }
                else
                {
                    GetComponent<PhotonView>().RPC("AddScore", RpcTarget.AllBuffered, touchedUserId);
                    Reappear();
                }
            }
            else
            {
                if (rb.velocity.magnitude < almostStopped)
                {
                    idle = true;
                } else
                {
                    idle = false;
                }
                if (idle)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        HideArrows();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            touchedUserId = collision.transform.parent.GetComponent<BallInfo>().GetUserId();
        } else if (collision.gameObject.tag == "Hole")
        {
            holePosition = collision.transform.position;
            disabled = true;
            //GetComponent<CircleCollider2D>().enabled = false;
            rb.velocity = Vector3.zero;
        }
    }

    private void SetCanvasName()
    {
        canvasName.GetComponent<Text>().text = photonView.Owner.NickName;
    }

    private void SetCanvasScore()
    {
        canvasScore.GetComponent<Text>().text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int _score)
    {
        score = _score;
    }

    private void Reappear()
    {
        touchedUserId = "";
        disabled = false;
        //GetComponent<CircleCollider2D>().enabled = true;
        idle = true;
        transform.position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50));
        transform.localScale = Vector3.one;
    }

    private void ShowArrows()
    {
        direction = Vector3.up * joystick.Vertical + Vector3.right * joystick.Horizontal;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        arrows.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        float value = direction.magnitude;

        if (value < 0.25f)
        {
            arrowOne.SetActive(false);
            arrowTwo.SetActive(false);
            arrowThree.SetActive(false);
            speed = baseSpeed * 0;
        } else if (value >= 0.25f && value < 0.5f)
        {
            arrowOne.SetActive(true);
            arrowTwo.SetActive(false);
            arrowThree.SetActive(false);
            speed = baseSpeed * 1;
        } else if (value >= 0.5f && value < 0.75f)
        {
            arrowOne.SetActive(true);
            arrowTwo.SetActive(true);
            arrowThree.SetActive(false);
            speed = baseSpeed * 2;
        } else
        {
            arrowOne.SetActive(true);
            arrowTwo.SetActive(true);
            arrowThree.SetActive(true);
            speed = baseSpeed * 3;
        }
    }

    private void HideArrows()
    {
        arrowOne.SetActive(false);
        arrowTwo.SetActive(false);
        arrowThree.SetActive(false);

        rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    [PunRPC]
    public void NewPlayerJoined(string _id, string _name, int _score)
    {
        GetComponent<BallInfo>().SetNickname(_name);
        GetComponent<BallInfo>().SetUserId(_id);
        SetCanvasName();
        gameplayStatus.AddPlayer(_id, _name, _score);
    }

    public override void OnLeftRoom()
    {
        GetComponent<PhotonView>().RPC("RemovePlayer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.UserId);
    }

    [PunRPC]
    public void RemovePlayer(string _id)
    {
        gameplayStatus.RemovePlayer(_id);
    }

    [PunRPC]
    public void AddScore(string touchedId)
    {
        // Incase the ball you hit is heavy you get more points
        int _score = 1;
        gameplayStatus.AddScore(touchedId, _score);
        SetCanvasScore();
    }

    public void IncreaseScore(int _score)
    {
        touchedUserId = "";
        score += _score;
        baseSpeed += baseSpeedStep * _score;
        rb.mass += massStep * _score;
        SetCanvasScore();
    }
}
