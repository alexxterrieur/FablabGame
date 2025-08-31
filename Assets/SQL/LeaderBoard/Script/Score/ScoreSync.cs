using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreSync : MonoBehaviour
{
    private readonly string serverUrl = "http://127.0.0.1/eurekagame/UpdateScore.php";

    public IEnumerator SyncScore(int score)
    {
        if (string.IsNullOrEmpty(PlayerSession.Instance?.Email))
        {
            Debug.LogWarning("ScoreSync: No logged in user!");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("email", PlayerSession.Instance.Email);
        form.AddField("score", score);

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("ScoreSync: Server error - " + www.error);
            }
            else
            {
                Debug.Log("ScoreSync: Score updated successfully: " + www.downloadHandler.text);
            }
        }
    }

}
