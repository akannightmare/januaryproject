using UnityEngine;
using UnityEngine.SceneManagement;

namespace Peykarimeh.PlatformerToolkit
{
    public class ButtonController : MonoBehaviour
    {
        private void Start()
        {
            if (RaceTimer.instance != null)
            {
                Destroy(RaceTimer.instance.gameObject);
            }
        }
        public void LoadScene(string SceneName)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}