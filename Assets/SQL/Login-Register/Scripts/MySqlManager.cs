using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class MySqlManager
{
    readonly static string SERVER_URL = "http://127.0.0.1/eurekagame";

    // ---------- REGISTER ----------
    public static async Task<(bool success, string message)> RegisterUser(string email, string password, string username)
    {
        string REGISTER_USER_URL = $"{SERVER_URL}/RegisterUser.php";

        return await SendPostRequest(REGISTER_USER_URL, new Dictionary<string, string>()
        {
            {"email", email},
            {"username", username},
            {"password", password}
        });
    }

    // ---------- LOGIN ----------
    public static async Task<(bool success, string userName, string userEmail)> LoginUser(string email, string password)
    {
        string LOGIN_USER_URL = $"{SERVER_URL}/Login.php";

        var (success, message) = await SendPostRequest(LOGIN_USER_URL, new Dictionary<string, string>()
        {
            {"email", email},
            {"password", password}
        });

        if (!success)
            return (false, "", "");

        message = message.Trim(); // Supprime \n ou espaces
        string[] parts = message.Split('|');
        if (parts.Length < 2)
        {
            Debug.LogWarning("LoginUser: message mal formé -> " + message);
            return (false, "", "");
        }

        return (true, parts[0].Trim(), parts[1].Trim());
    }

    // ---------- GENERIC POST ----------
    static async Task<(bool success, string returnMessage)> SendPostRequest(string url, Dictionary<string, string> data)
    {
        using (UnityWebRequest req = UnityWebRequest.Post(url, data))
        {
            req.SendWebRequest();
            while (!req.isDone) await Task.Delay(100);

            string response = req.downloadHandler.text.Trim();
            Debug.Log("Server response: '" + response + "'");

            if (!string.IsNullOrEmpty(req.error))
                return (false, req.error);

            if (int.TryParse(response, out _)) // Codes d'erreur numériques
                return (false, response);

            if (response.StartsWith("Success"))
            {
                string msg = response.Substring("Success|".Length); // Récupère tout après Success|
                return (true, msg);
            }

            return (false, response);
        }
    }
}


public class DatabaseUser
{
    public string Email;
    public string Password;
    public string Username;
}
