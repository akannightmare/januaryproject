using UnityEngine;
using UnityEngine.SceneManagement;

namespace Peykarimeh.PlatformerToolkit
{
    public class PauseManager : MonoBehaviour
    {
        bool isPaused;
        [SerializeField] GameObject pausePanel;

        private void Start()
        {
            isPaused = false;
            Time.timeScale = 1.0f;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { PauseGame(); }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("MainMenu");
            }
        }

        void PauseGame()
        {
            isPaused = !isPaused;
            pausePanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0.0f : 1.0f;

        }
    }
}