using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    GameObject leftTopHole;
    GameObject middleTopHole;
    GameObject rightTopHole;
    GameObject leftBottomHole;
    GameObject middleBottomHole;
    GameObject rightBottomHole;

    public Vector2 closestHoleVector;
    public string closestHoleName = "MiddleTop";

    void Start()
    {
        leftTopHole = GameObject.Find("LeftTopHole");
        middleTopHole = GameObject.Find("MiddleTopHole");
        rightTopHole = GameObject.Find("RightTopHole");
        leftBottomHole = GameObject.Find("LeftBottomHole");
        middleBottomHole = GameObject.Find("MiddleBottomHole");
        rightBottomHole = GameObject.Find("RightBottomHole");
    }

    void Update()
    {
        GetAllHoleDistances();
    }

    private void GetAllHoleDistances()
    {
        Vector2 leftTop = transform.position - leftTopHole.transform.position;
        Vector2 middleTop = transform.position - middleTopHole.transform.position;
        Vector2 rightTop = transform.position - rightTopHole.transform.position;
        Vector2 leftBottom = transform.position - leftBottomHole.transform.position;
        Vector2 middleBottom = transform.position - middleBottomHole.transform.position;
        Vector2 rightBottom = transform.position - rightBottomHole.transform.position;

        float closestDistance = leftTop.magnitude;
        Vector2 closestVector = leftTop;
        string closestName = "LeftTop";

        if (middleTop.magnitude < closestDistance)
        {
            closestDistance = middleTop.magnitude;
            closestVector = middleTop;
            closestName = "MiddleTop";
        }
        if (rightTop.magnitude < closestDistance)
        {
            closestDistance = rightTop.magnitude;
            closestVector = rightTop;
            closestName = "RightTop";
        }
        if (leftBottom.magnitude < closestDistance)
        {
            closestDistance = leftBottom.magnitude;
            closestVector = leftBottom;
            closestName = "LeftBottom";
        }
        if (middleBottom.magnitude < closestDistance)
        {
            closestDistance = middleBottom.magnitude;
            closestVector = middleBottom;
            closestName = "MiddleBottom";
        }
        if (rightBottom.magnitude < closestDistance)
        {
            closestDistance = rightBottom.magnitude;
            closestVector = rightBottom;
            closestName = "RightBottom";
        }

        closestHoleVector = closestVector;
        closestHoleName = closestName;
    }
}
