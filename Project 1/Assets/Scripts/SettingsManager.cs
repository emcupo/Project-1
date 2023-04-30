using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Toggle _vsyncToggle;
    [SerializeField] private SoundSlider[] soundSliders = new SoundSlider[1];

    [SerializeField] private TextMeshProUGUI resolutionLabel;
    [SerializeField] private List<Resolution> resolutions = new List<Resolution>();
    private int selectedResolution = 0;

    private void Awake()
    {
        if (resolutions.Count <= 0)
        {
            resolutions.Add(new Resolution(1920, 1080));
            resolutions.Add(new Resolution(1600, 900));
            resolutions.Add(new Resolution(1280, 720));
            resolutions.Add(new Resolution(854, 480));
        }
    }
    private void Start()
    {
        LoadVolume();
        LoadGraphics();
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

    private void LoadGraphics()
    {
        _fullScreenToggle.isOn = Screen.fullScreen;
        _vsyncToggle.isOn = QualitySettings.vSyncCount > 0 ? true : false;

        bool foundRes = false;
        Resolution selectedRes = resolutions[selectedResolution];
        if (Screen.width != selectedRes.width || Screen.height != selectedRes.height)
        {
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    selectedResolution = i;
                    foundRes = true;
                    break;
                }
            }
        }

        if (!foundRes)
        {
            resolutions.Add(new Resolution(Screen.width, Screen.height));
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResolution();
    }

    public void changeVolume(int targetSlider)
    {
        SoundSlider volumeSlider = soundSliders[targetSlider];

        volumeSlider.group.audioMixer.SetFloat(volumeSlider.name, volumeSlider.slider.value);
        PlayerPrefs.SetFloat(volumeSlider.name, volumeSlider.slider.value);
    }

    public void ResolutionLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
            selectedResolution = resolutions.Count - 1;
        UpdateResolution();
    }

    public void ResolutionRight()
    {
        selectedResolution++;
        if (selectedResolution >= resolutions.Count)
            selectedResolution = 0;
        UpdateResolution();
    }

    public void UpdateResolution()
    {
        resolutionLabel.text = resolutions[selectedResolution].ToString();
    }
    public void ApplyGraphics()
    {
        if (_vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        Resolution res = resolutions[selectedResolution];
        Screen.SetResolution(res.width, res.height, _fullScreenToggle.isOn);
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
class Resolution
{
    public int width;
    public int height;

    public Resolution(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
    public override string ToString()
    {
        return width + "x" + height;
    }
}