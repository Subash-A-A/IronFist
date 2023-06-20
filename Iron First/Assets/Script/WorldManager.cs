using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [SerializeField] Punch playerPunch;
    [SerializeField] PlayerHealth player;
    [SerializeField] Animator camAnim;
    [SerializeField] KeyCode[] dodgeKeys;
    [SerializeField] Text keyDisplayText;
    [SerializeField] GameObject bossFeintSign;
    [SerializeField] GameObject[] Enemies;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] TMP_Text pauseScoreText;

    private bool isSwiping = false;
    private int swipeDirection = 0;
    private bool playerDodged = false;
    private KeyCode dodgeKey = KeyCode.None;
    
    public int currentScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        currentScore = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.None))
        {
            Debug.Log("None");
        }
        if (isSwiping)
        {
            if (Input.GetKeyDown(dodgeKey))
            {
                EndSwipe(true);
            }
            else if (Input.anyKeyDown)
            {
                EndSwipe(false);
            }
        }
    }
    public void InitiateSwipe(int swipeDir)
    {
        PlayerWaitBegin();
        swipeDirection = swipeDir;
        playerDodged = false;
        dodgeKey = GenerateRandomKeyCode();
        keyDisplayText.transform.parent.gameObject.SetActive(true);
        isSwiping = true;
    }
    public void EndSwipe(bool dodged)
    {
        keyDisplayText.transform.parent.gameObject.SetActive(false);
        isSwiping = false;
        TimeManager.Instance.StopSlowMotion();
        playerDodged = dodged;
        if (dodged)
        {
            player.Dodge(swipeDirection);
            PlayerWaitEnd();
        }
    }
    private KeyCode GenerateRandomKeyCode()
    {
        KeyCode key = dodgeKeys[Random.Range(0, dodgeKeys.Length)];
        keyDisplayText.text = key.ToString();
        return key;
    }

    public EnemyBoss GetActiveEnemyBoss()
    {
        return FindObjectOfType<EnemyBoss>();
    }

    public PlayerHealth GetPlayer()
    {
        return player;
    }

    public GameObject GetPlayerGameObject()
    {
        return player.gameObject;
    }

    public void CameraShake()
    {
        camAnim.SetTrigger("Shake");
    }

    public void PlayerWaitBegin()
    {
        playerPunch.playerWait = true;
    }

    public void PlayerWaitEnd()
    {
        playerPunch.playerWait = false;
    }

    public bool DidPlayerDodge()
    {
        return playerDodged;
    }

    public GameObject GetBossFeintSign()
    {
        return bossFeintSign;
    }

    public void SpawnRandomEnemy()
    {
        UpdateScore();
        PlayerWaitBegin();
        IncreaseWorldDifficulty();
        Instantiate(Enemies[Random.Range(0, Enemies.Length)]);
    }

    private void IncreaseWorldDifficulty()
    {
        TimeManager.Instance.IncrementSlowDownFactor(0.05f);
    }
    public void GameOver()
    {
        SubmitScore();
        PlayerWaitBegin();
        GetActiveEnemyBoss().GameOver();
        pauseMenu.SetActive(true);
    }

    public void UpdateScore()
    {
        currentScore++;
        scoreText.text = "x" + currentScore;
        pauseScoreText.text = "x" + currentScore;
    }

    public void SubmitScore()
    {
        string memberID = PlayerPrefs.GetString("PlayerID");
        string leaderboardKey = "globalGameScore";
        int score = currentScore;

        LootLockerSDKManager.SubmitScore(memberID, score, leaderboardKey, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }
}
