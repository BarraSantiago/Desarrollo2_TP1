using Game;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverMenu;
        [SerializeField] private Button restartLevel;
        private void Start()
        {
            PlayerController.OnDefeatEvent += GameOver;
        }

        private void OnDestroy()
        {
            PlayerController.OnDefeatEvent -= GameOver;
        }

        /// <summary>
        /// in case of defeat event, activates the game over menu
        /// </summary>
        private void GameOver()
        {
            PauseManager.ChangeMouseState(true);
            restartLevel.Select();
            gameOverMenu.SetActive(true);
        }
    }
}