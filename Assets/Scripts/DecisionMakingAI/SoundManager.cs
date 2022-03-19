using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

namespace DecisionMakingAI
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public GameSoundParameters soundParameters;

        public AudioMixerSnapshot paused;
        public AudioMixerSnapshot unpaused;
        public AudioMixer masterMixer;
        
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
            paused.TransitionTo(0.01f);
        }

        private void OnResumeGame()
        {
            unpaused.TransitionTo(0.01f);
        }

        private void OnUpdateMusicVolume(object data)
        {
            float volume = (float)data;
            masterMixer.SetFloat("musicVol", volume);
        }

        private void OnUpdateSfxVolume(object data)
        {
            float volume = (float)data;
            masterMixer.SetFloat("sfxVol", volume);
        }
    }
}
