using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private SoundSlider[] soundSliders = new SoundSlider[1];
    [SerializeField] private PrefSlider[] otherSliders;
    [SerializeField] private PrefInputField[] inputFields = new PrefInputField[1];

    private void Start()
    {
        LoadToggle();
        LoadVolume();
        LoadOtherSliders();
        LoadInput();
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

    private void LoadOtherSliders()
    {
        for (int i = 0; i < otherSliders.Length; i++)
        {
            PrefSlider slider = otherSliders[i];
            if (PlayerPrefs.HasKey(slider.name))
            {
                float value = PlayerPrefs.GetFloat(slider.name);
                slider.slider.value = value;
            }
        }

    }

    private void LoadInput()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            PrefInputField field = inputFields[i];
            if (PlayerPrefs.HasKey(field.name))
            {
                float value = PlayerPrefs.GetFloat(field.name);
                field.field.text = value + "";
            }
        }
    }

    private void LoadToggle()
    {
        if (PlayerPrefs.HasKey("AutoPlay"))
        {
            _toggle.isOn = PlayerPrefs.GetInt("AutoPlay") != 0 ? true : false;
        }
    }

    public void changeVolume(int targetSlider)
    {
        SoundSlider volumeSlider = soundSliders[targetSlider];

        volumeSlider.group.audioMixer.SetFloat(volumeSlider.name, volumeSlider.slider.value);
        PlayerPrefs.SetFloat(volumeSlider.name, volumeSlider.slider.value);
    }

    public void changeValue(int targetSlider)
    {
        PrefSlider slider = otherSliders[targetSlider];
        PlayerPrefs.SetFloat(slider.name, slider.slider.value);
    }

    public void changeFieldValue(int target)
    {
        PrefInputField field = inputFields[target];
        PlayerPrefs.SetFloat(field.name, float.Parse(field.field.text));
    }
}

[System.Serializable]
class SoundSlider : PrefSlider
{
    public AudioMixerGroup group;
}

[System.Serializable]
class PrefSlider
{
    public string name;
    public Slider slider;
}

[System.Serializable]
class PrefInputField
{
    public string name;
    public TMP_InputField field;
}