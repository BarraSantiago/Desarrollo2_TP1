﻿using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class WinManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameWonMenu;
        [SerializeField] private Button selectButton;
        [SerializeField] private bool canWin = true;
        private void Start()
        {
            GameManager.OnWinEvent += GameWin;
        }

        private void OnDestroy()
        {
            GameManager.OnWinEvent -= GameWin;
        }

        /// <summary>
        /// in case of win event, activates the game won menu
        /// </summary>
        private void GameWin()
        {
            if(!canWin) return;
            PauseManager.ChangeMouseState(true);
            selectButton.Select();
            gameWonMenu.SetActive(true);
        }
    }
}