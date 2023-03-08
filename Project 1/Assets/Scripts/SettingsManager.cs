using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private SoundSlider[] soundSliders = new SoundSlider[1];

    private void Start()
    {
        LoadVolume();
    }
    public void ToggleAutoPlay()
    {
        if (_toggle.isOn)
            PlayerPrefs.SetInt("AutoPlay", 1);
        else
            PlayerPrefs.SetInt("AutoPlay", 0);
    }
    private void LoadVolume()
    {
        for (int i = 0; i < soundSliders.Length; i++)
        {

            SoundSlider volumeSlider = soundSliders[i];
            //Debug.Log("Slider " + volumeSlider.name + " is being adjusted");
            if (PlayerPrefs.HasKey(volumeSlider.name))
            {
                float volumeLevel = PlayerPrefs.GetFloat(volumeSlider.name);
                volumeSlider.group.audioMixer.SetFloat(volumeSlider.name, volumeLevel);
                volumeSlider.slider.value = volumeLevel;
            }
        }
    }

    public void changeVolume(int targetSlider)
    {
        SoundSlider volumeSlider = soundSliders[targetSlider];

        volumeSlider.group.audioMixer.SetFloat(volumeSlider.name, volumeSlider.slider.value);
        PlayerPrefs.SetFloat(volumeSlider.name, volumeSlider.slider.value);
    }

    [System.Serializable]
    struct SoundSlider
    {
        public string name;
        public AudioMixerGroup group;
        public Slider slider;
    }

}

