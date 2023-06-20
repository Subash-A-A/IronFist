using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] TMP_InputField emailFieldLogin;
    [SerializeField] TMP_InputField passwordFieldLogin;
    [SerializeField] TMP_InputField emailFieldReg;
    [SerializeField] TMP_InputField passwordFieldReg;
    [SerializeField] TMP_Text errTextLogin;
    [SerializeField] TMP_Text errTextReg;
    [SerializeField] TMP_Text playerNames;
    [SerializeField] TMP_Text playerScores;
    [SerializeField] TMP_Text playerRank;
    [SerializeField] Toggle rememberMeToggle;
    [SerializeField] TMP_InputField nickNameField;

    private void Start()
    {
        CheckPreviousSessions();
    }

    public void CheckPreviousSessions()
    {
        LootLockerSDKManager.CheckWhiteLabelSession(response =>
        {
            if (response)
            {
                // Start a new session
                Debug.Log("session is valid, you can start a game session");
                LootLockerSDKManager.StartWhiteLabelSession(response =>
                {
                    if (response.success)
                    {
                        PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                        MenuCanvas.Instance.CloseAllPanel();
                        RetrieveLeaderboard();
                    }
                    else
                    {
                        Debug.Log("Could not retrieve leaderboard");
                    }
                });
            }
            else
            {
                // Show login form here
                Debug.Log("session is NOT valid, we should show the login form");
                MenuCanvas.Instance.OpenLoginPage();
            }
        });
    }

    public void SignUp()
    {
        string email = emailFieldReg.text;
        string password = passwordFieldReg.text;
        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while creating user");
                errTextReg.text = "Error while creating user\npassword should have atleast 8 characters";
                return;
            }

            Debug.Log("user created successfully");

            MenuCanvas.Instance.OpenLoginPage();
        });
    }

    public void SignIn()
    {
        // This code should be placed in a handler when user clicks the login button.
        string email = emailFieldLogin.text;
        string password = passwordFieldLogin.text;
        bool rememberMe = rememberMeToggle.isOn;

        LootLockerSDKManager.WhiteLabelLoginAndStartSession(email, password, rememberMe, response =>
        {
            if (!response.success)
            {
                if (!response.LoginResponse.success)
                {
                    errTextLogin.text = "error while logging in";
                }
                else if (!response.SessionResponse.success)
                {
                    errTextLogin.text = "error while starting session";
                }
                return;
            }

            // Login Success
            PlayerPrefs.SetString("PlayerID", response.SessionResponse.player_id.ToString());
            MenuCanvas.Instance.CloseAllPanel();

            RetrieveLeaderboard();
        });
    }

    public void GuestSignIn()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
            // Login Success
            PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
            MenuCanvas.Instance.CloseAllPanel();

            RetrieveLeaderboard();
        });

    }

    public void ResetPassword()
    {
        string email = "user@lootlocker.io";
        LootLockerSDKManager.WhiteLabelRequestPassword(email, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error requesting password reset");

                return;
            }

            Debug.Log("requested password reset successfully");
        });
    }

    public void RetrieveLeaderboard()
    {
        string leaderboardKey = "globalGameScore";
        int count = 10;

        LootLockerSDKManager.GetScoreList(leaderboardKey, count, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");

                LootLockerLeaderboardMember[] members = response.items;

                string tempRank = "Rank\n";
                string tempNames = "Name\n";
                string tempScores = "Score\n";

                for(int i = 0; i < members.Length; i++)
                {
                    tempRank += members[i].rank + "\n";

                    if (members[i].player.name != "")
                    {
                        tempNames += members[i].player.name;
                    }
                    else
                    {
                        tempNames += members[i].player.id;
                    }

                    tempScores += members[i].score + "\n";
                    tempNames += "\n";
                }
                playerNames.text = tempNames;
                playerScores.text = tempScores;
                playerRank.text = tempRank;
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

    public void ChangeNickname()
    {
        LootLockerSDKManager.SetPlayerName(nickNameField.text, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Changed !");
            }
            else
            {
                Debug.Log("Error " + response.Error);
            }
        });
    }
}

