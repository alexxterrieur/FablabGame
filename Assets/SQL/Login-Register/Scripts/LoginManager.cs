using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    [Header("Register")]
    [SerializeField] TMP_InputField Reg_Email;
    [SerializeField] TMP_InputField Reg_UserName;
    [SerializeField] TMP_InputField Reg_Password;

    [Header("Login")]
    [SerializeField] TMP_InputField Log_Email;
    [SerializeField] TMP_InputField Log_Password;

    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private LeaderboardManager leaderboardManager;

    [SerializeField] private GameObject leaderboardPanel;


    public async void OnRegisterPressed()
    {
        if(!registerPanel.activeSelf)
        {
            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            return;
        }

        if(string.IsNullOrWhiteSpace(Reg_Email.text) || Reg_Email.text.Length < 5)
        {
            Debug.LogError("Please enter a valid email");
            return;
        }

        if (string.IsNullOrWhiteSpace(Reg_Password.text) || Reg_Password.text.Length < 5)
        {
            Debug.LogError("Please enter a valid password");
            return;
        }

        if (string.IsNullOrWhiteSpace(Reg_UserName.text) || Reg_UserName.text.Length < 3)
        {
            Debug.LogError("Please enter a valid user name");
            return;
        }

        //if (await MySqlManager.RegisterUser(Reg_Email.text, Reg_Password.text, Reg_UserName.text))
        //    Debug.Log("Successfully Registered");        
        //else 
        //    Debug.Log("Failed to Register user");

        (bool success, string message) = await MySqlManager.RegisterUser(Reg_Email.text, Reg_Password.text, Reg_UserName.text);

        if (success)
        {
            Debug.Log("Successfully Registered: " + message);
        }
        else
            Debug.Log("Failed to Register user: " + message);

    }

    public async void OnLoginPressed()
    {
        if (!loginPanel.activeSelf)
        {
            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
            return;
        }

        if (string.IsNullOrWhiteSpace(Log_Email.text) || Log_Email.text.Length < 5)
        {
            Debug.LogError("Please enter a valid email");
            return;
        }

        if (string.IsNullOrWhiteSpace(Log_Password.text) || Log_Password.text.Length < 5)
        {
            Debug.LogError("Please enter a valid password");
            return;
        }

        (bool success, string userName, string userEmail) = await MySqlManager.LoginUser(Log_Email.text, Log_Password.text);
        if (success)
        {
            Debug.Log("Successfully Logged in user: '" + userName + "'  email: '" + userEmail + "'");

            leaderboardManager.currentEmail = userEmail;
            leaderboardManager.currentUsername = userName;

            //close register
            gameObject.SetActive(false);
            leaderboardPanel.SetActive(true);

            //menu de fin afficher leaderBoard + menu start
        }
        else
        {
            Debug.Log("Failed to Log user: " + userName);
        }
    }
}
