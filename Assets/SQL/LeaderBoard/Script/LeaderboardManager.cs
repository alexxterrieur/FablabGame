using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI Prefabs")]
    public GameObject leaderboardEntryPrefab;

    [Header("Current User")]
    public string currentEmail;
    public string currentUsername;

    private void OnEnable()
    {
        StartCoroutine(GetLeaderboard());
    }

    IEnumerator GetLeaderboard()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", currentEmail);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/eurekagame/GetLeaderboard.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur connexion serveur : " + www.error);
            }
            else
            {
                Debug.Log("Réponse serveur: " + www.downloadHandler.text);
                ParseLeaderboard(www.downloadHandler.text);
            }
        }
    }

    void ParseLeaderboard(string data)
    {
        // Effacer l’ancien leaderboard
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        string[] lines = data.Split('\n');

        // --- Ligne TOP5 ---
        if (lines.Length > 0 && lines[0].StartsWith("TOP5"))
        {
            string[] parts = lines[0].Split('|');
            for (int i = 1; i < parts.Length; i++)
            {
                string[] userData = parts[i].Split(':');
                if (userData.Length == 2)
                {
                    string username = userData[0].Trim();
                    if (!int.TryParse(userData[1].Trim(), out int score))
                    {
                        Debug.LogWarning("Score invalide pour " + username + ": " + userData[1]);
                        score = 0;
                    }

                    if (leaderboardEntryPrefab != null)
                    {
                        GameObject entry = Instantiate(leaderboardEntryPrefab, transform);
                        var comp = entry.GetComponent<LeaderboardEntry>();
                        if (comp != null)
                            comp.Setup(i, username, score);
                    }
                }
            }
        }

        // --- Ligne PLAYER ---
        if (lines.Length > 1 && lines[1].StartsWith("PLAYER"))
        {
            string[] parts = lines[1].Split('|');
            if (parts.Length == 4)
            {
                string rankStr = parts[1].Trim();
                string username = parts[2].Trim();
                if (!int.TryParse(parts[3].Trim(), out int score))
                {
                    Debug.LogWarning("Score invalide pour joueur courant: " + parts[3]);
                    score = 0;
                }

                int rank = rankStr == "NA" ? -1 : int.TryParse(rankStr, out int r) ? r : -1;

                if (leaderboardEntryPrefab != null)
                {
                    GameObject entry = Instantiate(leaderboardEntryPrefab, transform);
                    var comp = entry.GetComponent<LeaderboardEntry>();
                    if (comp != null)
                        comp.Setup(rank, username, score, true);
                }
            }
        }
    }
}
