using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    /// <summary>
    /// MenuManager class provides methods to load the game scene, quit the application, and load scenes by index or name.
    /// </summary>
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

        /// <summary>
        /// Loads a scene by the index introduced
        /// </summary>
        /// <param name="index"> Index of scene to load </param>
        public void LoadSceneByIndex(int index)
        { 
            const float normalTimeScale = 1;
            
            if(index > SceneManager.sceneCountInBuildSettings || index < 0) return;
            
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