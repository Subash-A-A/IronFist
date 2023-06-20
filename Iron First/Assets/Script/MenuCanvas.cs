using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject registPanel;
    [SerializeField] GameObject afterLoginPanels;
    [SerializeField] GameObject tutorial;

    public static MenuCanvas Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        loginPanel.SetActive(true);
        registPanel.SetActive(false);
        afterLoginPanels.SetActive(false);
    }

    public void OpenLoginPage()
    {
        loginPanel.SetActive(true);
        registPanel.SetActive(false);
        afterLoginPanels.SetActive(false);
    }
    public void OpenRegisterPage()
    {
        loginPanel.SetActive(false);
        registPanel.SetActive(true);
        afterLoginPanels.SetActive(false);
    }

    public void CloseAllPanel()
    {
        loginPanel.SetActive(false);
        registPanel.SetActive(false);
        afterLoginPanels.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowTutorial()
    {
        tutorial.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorial.SetActive(false);
    }
}
