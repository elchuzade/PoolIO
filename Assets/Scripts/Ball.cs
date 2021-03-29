using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    // Opposite to mouse drag direction
    Vector3 direction;

    // 0, 1, 2, 3 deterines arrows and speed of the ball launch
    float stretchMagnitude = 0;

    [SerializeField] GameObject arrows;
    [SerializeField] GameObject arrowOne;
    [SerializeField] GameObject arrowTwo;
    [SerializeField] GameObject arrowThree;

    // Where the tap started
    Vector3 mouseDownPosition;

    float baseSpeed = 5000000;
    float speed = 0;
    // Drag force
    float drag;
    // This is not to wait until the full stop
    float almostStopped = 100f;

    bool idle = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && idle)
        {
            GetMouseDirection();
        }
        if (Input.GetMouseButtonUp(0) && idle)
        {
            SetMouseDirection();
        }

        if (rb.velocity.magnitude < almostStopped && !idle)
        {
            idle = true;
        }
        if (Input.GetMouseButton(0) && idle)
        {
            GetMouseDragging();
        }
    }

    private void GetMouseDirection()
    {
        mouseDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void GetMouseDragging()
    {
        direction = mouseDownPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ShowArrows();
    }

    private void SetMouseDirection()
    {
        rb.AddForce(direction.normalized * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        HideArrows();

        idle = false;
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

    private void HideArrows()
    {
        arrowOne.SetActive(false);
        arrowTwo.SetActive(false);
        arrowThree.SetActive(false);
    }
}
