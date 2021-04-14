using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NormalBot : MonoBehaviour
{
    GameObject leftTopHole;
    GameObject middleTopHole;
    GameObject rightTopHole;
    GameObject leftBottomHole;
    GameObject middleBottomHole;
    GameObject rightBottomHole;
    GameObject[] players;

    List<(string, Vector2, Vector2)> playerDistances = new List<(string, Vector2, Vector2)>();

    [SerializeField] Rigidbody2D rb;

    // When you are sucked in to the hole disable controls
    bool disabled = false;

    // This is not to wait until the full stop
    float almostStopped = 100f;

    bool idle = true;

    Vector3 direction;
    float baseSpeed = 100000;
    float speed = 0;
    // When score increases, the multiplier increases too
    float baseSpeedStep = 10000;
    float massStep = 50;

    // To determine who touched you the last
    string touchedUserId;
    int score = 0;

    string nickname;
    string userId;

    [SerializeField] GameObject canvasName;
    [SerializeField] GameObject canvasScore;

    // Position of the hole to suck the ball into
    Vector3 holePosition;
    float holeSuckSpeed = 2;

    void Start()
    {
        SetCanvasScore();

        leftTopHole = GameObject.Find("LeftTopHole");
        middleTopHole = GameObject.Find("MiddleTopHole");
        rightTopHole = GameObject.Find("RightTopHole");
        leftBottomHole = GameObject.Find("LeftBottomHole");
        middleBottomHole = GameObject.Find("MiddleBottomHole");
        rightBottomHole = GameObject.Find("RightBottomHole");

        StartCoroutine(getPlayers());
    }

    void Update()
    {
        if (disabled)
        {
            if (transform.position != holePosition)
            {
                Vector3 movePos = Vector3.MoveTowards(transform.position, holePosition, holeSuckSpeed);
                transform.position = movePos;
                transform.localScale *= 0.99f;
            }
            else
            {
                //GetComponent<PhotonView>().RPC("AddScore", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.UserId);
                Reappear();
            }
        } else
        {
            if (rb.velocity.magnitude < almostStopped)
            {
                idle = true;
            }
            else
            {
                idle = false;
            }

            if (idle)
            {
                Attack();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            touchedUserId = collision.transform.parent.GetComponent<BallInfo>().GetUserId();
            canvasName.GetComponent<Text>().text = touchedUserId;
        }
        else if (collision.gameObject.tag == "Hole")
        {
            holePosition = collision.transform.position;
            disabled = true;
            GetComponent<CircleCollider2D>().enabled = false;
            rb.velocity = Vector3.zero;
        }
    }

    public void SetTouchedUserId(string id)
    {
        touchedUserId = id;
        canvasName.GetComponent<Text>().text = touchedUserId;
    }

    void Reappear()
    {
        disabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        idle = true;
        transform.position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50));
        transform.localScale = Vector3.one;
    }

    void SetCanvasName()
    {
        canvasName.GetComponent<Text>().text = nickname;
    }

    void SetCanvasScore()
    {
        canvasScore.GetComponent<Text>().text = score.ToString();
    }

    void Attack()
    {
        int arrowsCount = Random.Range(1, 4);
        speed = baseSpeed * arrowsCount;

        // Find closes to the hole player and charge towards him with a random shift
        int minIndex = 0;

        for (int i = 1; i < playerDistances.Count; i++)
        {
            if (playerDistances[i].Item2.magnitude < playerDistances[0].Item2.magnitude)
            {
                minIndex = i;
            }
        }

        int random = Random.Range(0, 5);

        if (random == 0)
        {
            // Strike in a random direction
            direction += new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
        } else
        {
            direction = playerDistances[minIndex].Item3 - (Vector2)transform.position;
        }
        rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    IEnumerator getPlayers()
    {
        while(true) {
            players = GameObject.FindGameObjectsWithTag("Player");
            playerDistances.Clear();
            foreach (GameObject p in players)
            {
                if (p != null && p.GetHashCode() != gameObject.GetHashCode())
                {
                    playerDistances.Add((
                        p.GetComponent<DistanceTracker>().closestHoleName,
                        p.GetComponent<DistanceTracker>().closestHoleVector,
                        p.transform.position
                    ));
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    #region Public Methods

    public void SetNameAndUserId(string name)
    {
        nickname = name;
        userId = "12345";

        GetComponent<BallInfo>().SetUserId(userId);
        GetComponent<BallInfo>().SetNickname(nickname);

        SetCanvasName();
    }

    //[PunRPC]
    //public void AddScore(string id)
    //{
    //    score++;
    //    baseSpeed += baseSpeedStep;
    //    rb.mass += massStep;
    //    SetCanvasScore();
    //    gameplayStatus.AddScore(id, score);
    //}

    #endregion
}
