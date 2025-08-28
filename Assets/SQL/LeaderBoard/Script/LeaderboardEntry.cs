using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text usernameText;
    public TMP_Text scoreText;

    public void Setup(int rank, string username, int score, bool isPlayer = false)
    {
        rankText.text = rank > 0 ? rank.ToString() : "-";
        usernameText.text = username;
        scoreText.text = score.ToString();

        if (isPlayer)
        {
            usernameText.color = Color.yellow;
        }
    }
}
