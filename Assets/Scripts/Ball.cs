using UnityEngine;

public class Ball : MonoBehaviour
{
    GameplayStatus gameplayStatus;

    [SerializeField] Rigidbody2D rb;
    // Ball Self collider
    [SerializeField] CircleCollider2D col;
    // Ball Child collider
    [SerializeField] CircleCollider2D colTrigger;

    // Opposite to mouse drag direction
    Vector3 direction;

    [SerializeField] GameObject arrows;
    [SerializeField] GameObject arrowOne;
    [SerializeField] GameObject arrowTwo;
    [SerializeField] GameObject arrowThree;

    [Header("Mouse Stuff")]
    [SerializeField] GameObject mouseCircle;
    [SerializeField] GameObject mouseArrow;
    [SerializeField] LineRenderer mouseLine;
    private Vector3[] mouseCoords = { new Vector3(0,0,0), new Vector3(0,0,0) };

    // Where the tap started
    Vector3 mouseDownPosition;

    float baseSpeed = 5000000;
    float speed = 0;

    // This is not to wait until the full stop
    float almostStopped = 100f;

    // When you are sucked in to the hole disable controls
    bool disabled = false;

    // To allow aiming
    bool idle = true;
    // To determine who touched you the last
    int touchedHash;
    int score;

    string nickname = "Orkhan";

    // Position of the hole to suck the ball into
    Vector3 holePosition;
    float holeSuckSpeed = 2;

    void Awake()
    {
        gameplayStatus = FindObjectOfType<GameplayStatus>();
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
                Reappear();
            }
        } else
        {
            // If the ball is moving then it is not idle even if someone else hit you
            if (rb.velocity.magnitude < almostStopped)
            {
                SetIdlePosition();
            }
            else
            {
                idle = false;
            }

            if (Input.GetMouseButtonDown(0) && idle)
            {
                GetMouseDirection();
            }
            if (Input.GetMouseButtonUp(0) && idle)
            {
                SetMouseDirection();
            }
            if (Input.GetMouseButton(0) && idle)
            {
                GetMouseDragging();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            touchedHash = collision.GetHashCode();
        } else if (collision.gameObject.tag == "Hole")
        {
            holePosition = collision.transform.position;
            disabled = true;
            rb.velocity = Vector3.zero;
        }
    }

    public string GetNickname()
    {
        return nickname;
    }

    public int GetScore()
    {
        return score;
    }

    private void Reappear()
    {
        disabled = false;
        idle = true;
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    private void SetIdlePosition()
    {
        idle = true;
        touchedHash = 0;
    }

    private void GetMouseDirection()
    {
        mouseDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ShowMouse(mouseDownPosition);
    }

    private void GetMouseDragging()
    {
        rb.velocity = Vector3.zero;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = mouseDownPosition - mousePosition;

        ShowMouse(mousePosition);

        ShowArrows();
    }

    private void SetMouseDirection()
    {
        rb.AddForce(direction.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        HideArrows();
        HideMouse();

        idle = false;
    }

    private void ShowMouse(Vector3 mousePosition)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        mouseArrow.SetActive(true);
        mouseArrow.transform.position = new Vector3(mouseDownPosition.x, mouseDownPosition.y, 0);
        mouseArrow.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        mouseCoords[0] = new Vector3(mouseDownPosition.x, mouseDownPosition.y, 0);

        mouseCircle.SetActive(true);
        mouseCircle.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        mouseCoords[1] = new Vector3(mousePosition.x, mousePosition.y, 0);

        mouseLine.gameObject.SetActive(true);
        mouseLine.positionCount = 2;
        mouseLine.SetPositions(mouseCoords);
    }

    private void ShowArrows()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        arrows.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        int value = (int)direction.magnitude / 20;
        value = Mathf.Abs(value);

        if (value > 3)
        {
            value = 3;
        }

        switch (value)
        {
            case 0:
                arrowOne.SetActive(false);
                arrowTwo.SetActive(false);
                arrowThree.SetActive(false);
                speed = baseSpeed * 0;
                break;
            case 1:
                arrowOne.SetActive(true);
                arrowTwo.SetActive(false);
                arrowThree.SetActive(false);
                speed = baseSpeed * 1;
                break;
            case 2:
                arrowOne.SetActive(true);
                arrowTwo.SetActive(true);
                arrowThree.SetActive(false);
                speed = baseSpeed * 2;
                break;
            case 3:
                arrowOne.SetActive(true);
                arrowTwo.SetActive(true);
                arrowThree.SetActive(true);
                speed = baseSpeed * 3;
                break;
        }
    }

    private void HideMouse()
    {
        mouseCircle.SetActive(false);
        mouseArrow.SetActive(false);
        mouseLine.gameObject.SetActive(false);
    }

    private void HideArrows()
    {
        arrowOne.SetActive(false);
        arrowTwo.SetActive(false);
        arrowThree.SetActive(false);
    }

    public void AddScore()
    {
        score++;
    }
}
