using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class LoadNextLevel : MonoBehaviour
    {
        private readonly int sceneId = 2;
        
        private void OnDestroy()
        {
            SceneManager.LoadScene(sceneId);
        }
    }
}