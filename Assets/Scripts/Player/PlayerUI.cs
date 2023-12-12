using System;
using System.Collections;
using Enemy;
using TMPro;
using UnityEngine;
using Weapons;

namespace Player
{
    /// <summary>
    /// The PlayerUI class manages the user interface for the player.
    /// </summary>
    public class PlayerUI : MonoBehaviour
    {
        public static Action OnNoTargets;

        [SerializeField] private TMP_Text timer;
        [SerializeField] private TMP_Text targetsRemaining;
        [SerializeField] private TMP_Text bulletsCounter;
        [SerializeField] private GameObject flyingTextPrefab;
        [SerializeField] private GameObject flyingTextSpawnPoint;
        [SerializeField] private PlayerController player;

        private int targetAmount;
        private bool isPlayerNull;
        private WeaponContainer weaponContainer;
        private GameObject[] flyingTextPool;
        private const int textPoolSize = 10;

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
            PlayerController.OnRecieveDamageEvent += SpawnFlyingText;

            targetAmount = GameObject.FindGameObjectsWithTag("Target").Length;
            ShowTargetsRemaining();
            flyingTextPool = new GameObject[textPoolSize];

            for (int i = 0; i < flyingTextPool.Length; i++)
            {
                flyingTextPool[i] = Instantiate(flyingTextPrefab);
                flyingTextPool[i].SetActive(false);
            }
        }

        private void OnDestroy()
        {
            Target.OnTargetDeath -= UpdateRemainingTargets;
            InputManager.OnShootEvent -= ShowBullets;
            InputManager.OnReloadEvent -= ShowBullets;
            InputManager.OnPickUpEvent -= ShowBullets;
            InputManager.OnDropEvent -= ShowBullets;
            InputManager.OnSwapWeaponEvent -= ShowBullets;
            PlayerController.OnRecieveDamageEvent -= SpawnFlyingText;
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
            if (isPlayerNull) return;
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

        /// <summary>
        /// This method spawns a text object that flies up when receiving damage.
        /// </summary>
        /// <param name="text"> The amount of damage received. </param>
        private void SpawnFlyingText(float text)
        {
            foreach (GameObject flyingText in flyingTextPool)
            {
                if (flyingText.activeSelf) continue;

                flyingText.SetActive(true);
                flyingText.transform.SetParent(gameObject.transform);
                flyingText.transform.position = flyingTextSpawnPoint.transform.position;
                flyingText.GetComponent<TMP_Text>().text = "-" + text.ToString("N1");
                
                StartCoroutine(DeactivateText(flyingText));
                break;
            }
        }

        /// <summary>
        /// This coroutine deactivates the flying text object after a delay.
        /// </summary>
        /// <param name="text"> The text object to deactivate. </param>
        private IEnumerator DeactivateText(GameObject text)
        {
            const float delay = 2;

            yield return new WaitForSeconds(delay);
            
            text.SetActive(false);
        }
    }
}