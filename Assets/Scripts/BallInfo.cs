using UnityEngine;

public class BallInfo : MonoBehaviour
{
    public string nickname;
    public string userId;

    public void SetNickname(string name)
    {
        nickname = name;
    }

    public string GetNickname()
    {
        return nickname;
    }

    public void SetUserId(string id)
    {
        userId = id;
    }

    public string GetUserId()
    {
        return userId;
    }
}
