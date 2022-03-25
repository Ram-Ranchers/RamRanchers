using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

namespace DecisionMakingAI
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public GameSoundParameters soundParameters;
        
        public AudioMixer masterMixer;

        private void Start()
        {
            masterMixer.SetFloat("musicVol", soundParameters.musicVolume);
            masterMixer.SetFloat("sfxVol", soundParameters.sfxVolume);
        }

        private void OnEnable()
        {
            EventManager.AddListener("PlaySoundByName", OnPlaySoundByName);
            EventManager.AddListener("PauseGame", OnPauseGame);
            EventManager.AddListener("ResumeGame", OnResumeGame);
            EventManager.AddListener("UpdateGameParameter:musicVolume", OnUpdateMusicVolume);
            EventManager.AddListener("UpdateGameParameter:sfxVolume", OnUpdateSfxVolume);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener("PlaySoundByName", OnPlaySoundByName);
            EventManager.RemoveListener("PauseGame", OnPauseGame);
            EventManager.RemoveListener("ResumeGame", OnResumeGame);
            EventManager.RemoveListener("UpdateGameParameter:musicVolume", OnUpdateMusicVolume);
            EventManager.RemoveListener("UpdateGameParameter:sfxVolume", OnUpdateSfxVolume);
        }

        private void OnPlaySoundByName(object data)
        {
            string clipName = (string)data;

            FieldInfo[] fields = typeof(GameSoundParameters).GetFields();
            AudioClip clip = null;
            foreach (FieldInfo field in fields)
            {
                if (field.Name == clipName)
                {
                    clip = (AudioClip)field.GetValue(soundParameters);
                    break;
                }
            }
            
            if (clip == null)
            {
                Debug.LogWarning($"Unknown clip name: '{clipName}'");
                return;
            }
            
            audioSource.PlayOneShot(clip);
        }

        private void OnPauseGame()
        {
            StartCoroutine(TransitioningVolume("musicVol", soundParameters.musicVolume, soundParameters.musicVolume - 6,
                0.5f));
            StartCoroutine(TransitioningVolume("sfxVol", soundParameters.sfxVolume,  -80,
                0.5f));
        }

        private void OnResumeGame()
        {
            StartCoroutine(TransitioningVolume("musicVol", soundParameters.musicVolume - 6, soundParameters.musicVolume,
                0.5f));
            StartCoroutine(TransitioningVolume("sfxVol", -80,  soundParameters.sfxVolume,
                0.5f));        }

        private void OnUpdateMusicVolume(object data)
        {
            float volume = (float)data;
            masterMixer.SetFloat("musicVol", volume);
        }

        private void OnUpdateSfxVolume(object data)
        {
            if (GameManager.instance.gameIsPaused)
            {
                return;
            }
            
            float volume = (float)data;
            masterMixer.SetFloat("sfxVol", volume);
        }

        private IEnumerator TransitioningVolume(string volumeParameter, float from, float to, float delay)
        {
            float t = 0;
            while (t < delay)
            {
                masterMixer.SetFloat(volumeParameter, Mathf.Lerp(from, to, t/ delay));
                t += Time.deltaTime;
                yield return null;
            }

            masterMixer.SetFloat(volumeParameter, to);
        }
    }
}
