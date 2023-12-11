using System;
using Enemy;
using Game;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        public static Action OnNoTargets;
        
        [SerializeField] private TMP_Text timer;
        [SerializeField] private TMP_Text targetsRemaining;
        [SerializeField] private TMP_Text bulletsCounter;
        [SerializeField] private PlayerController player;

        private int targetAmount;
        private bool isPlayerNull;
        private WeaponContainer weaponContainer;

        private void Start()
        {
            weaponContainer = FindObjectOfType<WeaponContainer>();
            isPlayerNull = player == null;

            Target.OnTargetDeath += UpdateRemainingTargets;
            InputManager.OnShootEvent += ShowBullets;
            InputManager.OnReloadEvent += ShowBullets;
            InputManager.OnPickUpEvent += ShowBullets;
            InputManager.OnDropEvent += ShowBullets;
            InputManager.OnSwapWeaponEvent += ShowBullets;

            targetAmount = GameObject.FindGameObjectsWithTag("Target").Length;
            ShowTargetsRemaining();
        }

        private void OnDestroy()
        {
            Target.OnTargetDeath -= UpdateRemainingTargets;
            InputManager.OnShootEvent -= ShowBullets;
            InputManager.OnReloadEvent -= ShowBullets;
            InputManager.OnPickUpEvent -= ShowBullets;
            InputManager.OnDropEvent -= ShowBullets;
            InputManager.OnSwapWeaponEvent -= ShowBullets;
        }

        private void Update()
        {
            ShowTimer();
        }

        /// <summary>
        /// Show time remaining
        /// </summary>
        private void ShowTimer()
        {
            if(isPlayerNull) return;
            if (player.Timer < 0) timer.text = "";
            timer.text = player.Timer.ToString("0.#");
        }

        private void UpdateRemainingTargets()
        {
            targetAmount--;
            ShowTargetsRemaining();
            if (targetAmount <= 0) OnNoTargets?.Invoke();
        }

        /// <summary>
        /// OnTargetDeath event, update targets remaining in ui
        /// </summary>
        private void ShowTargetsRemaining()
        {
            targetsRemaining.text = "Targets remaining: " + targetAmount;
        }

        /// <summary>
        /// Shows currently equipped weapon's remaing bullets
        /// </summary>
        private void ShowBullets()
        {
            if (weaponContainer.GetWeapon() == null)
            {
                bulletsCounter.text = "";
                return;
            }
            bulletsCounter.text = weaponContainer.GetWeapon()?.Bullets + "/" + weaponContainer.GetWeapon()?.MaxBullets;
        }
    }
}