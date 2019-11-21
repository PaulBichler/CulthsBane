using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static CombatSystem CombatSystem;

    public Button muteButton;
    public Sprite muteIcon;
    public Sprite unmuteIcon;
    public Button pauseButton;
    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button exitButton;
    public Button exitButton2;
    public Button startNew;
    
    
    private void Start()
    {
        if (muteButton) muteButton.onClick.AddListener(muteToggle);
        if (pauseButton) pauseButton.onClick.AddListener(displayPauseMenu);
        if (resumeButton) resumeButton.onClick.AddListener(resumeGame);
        if (exitButton) exitButton.onClick.AddListener(exitGame);    
        if(exitButton2) exitButton2.onClick.AddListener(exitGame);
        if(startNew) startNew.onClick.AddListener(startNewGame);
    }

    public void muteToggle()
    {
        if(AudioListener.volume > 0)
        {
            AudioListener.volume = 0;
            muteButton.transform.GetComponentInParent<Image>().sprite = muteIcon;
        } else
        {
            AudioListener.volume = 1;
            muteButton.transform.GetComponentInParent<Image>().sprite = unmuteIcon;
        }
        
    }

    public void displayPauseMenu()
    {
        if (pauseMenuPanel)
        {
            GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

            foreach(GameObject obj in sceneObjects)
            {
                foreach(Collider2D coll in obj.GetComponentsInChildren<Collider2D>())
                {
                    coll.enabled = false;
                }
            }

            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void resumeGame()
    {
        if (pauseMenuPanel)
        {
            Time.timeScale = 1;
            GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in sceneObjects)
            {
                foreach (Collider2D coll in obj.GetComponentsInChildren<Collider2D>())
                {
                    coll.enabled = true;
                }
            }

            pauseMenuPanel.SetActive(false);
        }
    }

    public void startNewGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Map");
    }

    public void exitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}