using System;
using Game;
using Player;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviourSingleton<AudioManager>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip winSound;
        [SerializeField] private AudioClip loseSound;

        private void Start()
        {
            GameManager.OnWinEvent += PlayWinSound;
            PlayerController.OnDefeatEvent += PlayLoseSound;
        }

        private void OnDestroy()
        {
            GameManager.OnWinEvent -= PlayWinSound;
            PlayerController.OnDefeatEvent -= PlayLoseSound;   
        }

        /// <summary>
        /// Plays an audioclip in the audio source
        /// </summary>
        /// <param name="audioClip"> Audio source in player </param>
        public void PlaySound(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
        
        /// <summary>
        /// Play loseSound when LoseEvent gets invoked.
        /// </summary>
        private void PlayLoseSound()
        {
            audioSource.PlayOneShot(loseSound);
        }
        
        /// <summary>
        /// Play winSound when WinEvent gets invoked.
        /// </summary>
        private void PlayWinSound()
        {
            audioSource.PlayOneShot(winSound);
        }
    }
}