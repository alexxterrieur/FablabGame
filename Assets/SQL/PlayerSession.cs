using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    public static PlayerSession Instance { get; private set; }

    public string Email { get; private set; }
    public string Username { get; private set; }
    public bool IsLoggedIn { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSession(string email, string username)
    {
        Email = email;
        Username = username;
        IsLoggedIn = true;
    }

    public void ClearSession()
    {
        Email = "";
        Username = "";
        IsLoggedIn = false;
    }
}
