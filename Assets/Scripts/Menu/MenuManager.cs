using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        private const string SceneName = "Tutorial";

        /// <summary>
        /// Loads the game scene
        /// </summary>
        public void LoadGame()
        {
            LoadSceneByName();
        }

        /// <summary>
        /// Quits the application
        /// </summary>
        public void OnExitButtonClick()
        {
            Application.Quit();
        }

        public void LoadSceneByIndex(int index)
        { 
            const float normalTimeScale = 1;
            
            Time.timeScale = normalTimeScale;
            
            SceneManager.LoadScene(index);
        }

        /// <summary>
        /// Loads the required scene from the current scene.
        /// </summary>
        private void LoadSceneByName()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}