using System;
using UI;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static Action OnWinEvent;

        private int targets;

        private void OnEnable()
        {
            PlayerUI.OnNoTargets += PlayerWin;
        }

        private void OnDestroy()
        {
            PlayerUI.OnNoTargets -= PlayerWin;
        }
        
        /// <summary>
        /// in case of win, disables this object to stop unecesary calculation and invokes win event.
        /// </summary>
        private void PlayerWin()
        {
            gameObject.SetActive(false);
            OnWinEvent?.Invoke();
        }
    }
}