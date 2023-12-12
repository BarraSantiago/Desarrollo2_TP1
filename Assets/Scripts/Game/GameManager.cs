using System;
using Menu;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// GameManager manages the game win or lose state.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static Action OnWinEvent;

        [SerializeField] private GameObject gameOverMenu;
        [SerializeField] private Button restartLevel;

        private int targets;

        private void OnEnable()
        {
            PlayerUI.OnNoTargets += PlayerWin;
            PlayerController.OnDefeatEvent += GameOver;
        }

        private void OnDestroy()
        {
            PlayerUI.OnNoTargets -= PlayerWin;
            PlayerController.OnDefeatEvent -= GameOver;
        }

        /// <summary>
        /// in case of win, disables this object to stop unecesary calculation and invokes win event.
        /// </summary>
        private void PlayerWin()
        {
            gameObject.SetActive(false);
            OnWinEvent?.Invoke();
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