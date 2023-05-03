using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    public static Action advancedUpdated;

    [Header("Sound")]
    [SerializeField] private SoundSlider[] soundSliders = new SoundSlider[1];

    [Header("Video")]
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Toggle _vsyncToggle;

    [SerializeField] private TextMeshProUGUI resolutionLabel;
    [SerializeField] private List<Resolution> resolutions = new List<Resolution>();
    private int selectedResolution = 0;

    [Header("Advanced")]
    [SerializeField] private TextMeshProUGUI capLabel;
    [SerializeField] private List<Cap> caps = new List<Cap>(4);
    [SerializeField] private int selectedCap = 2;

    [SerializeField] private Image displayImage;
    private float selectedTransparency = 0.75f;

    private void Awake()
    {
        if (resolutions.Count <= 0)
        {
            resolutions.Add(new Resolution(1920, 1080));
            resolutions.Add(new Resolution(1600, 900));
            resolutions.Add(new Resolution(1280, 720));
            resolutions.Add(new Resolution(854, 480));
        }

        if (caps.Count <= 0)
        {
            caps.Add(new Cap(5, "Low"));
            caps.Add(new Cap(25, "Medium"));
            caps.Add(new Cap(100, "High"));
        }
    }
    private void Start()
    {
        LoadVolume();
        LoadGraphics();
        LoadPreferences();
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

    private void LoadPreferences()
    {
        if (PlayerPrefs.HasKey("remainAlpha"))
            selectedTransparency = PlayerPrefs.GetFloat("remainAlpha");
        if (PlayerPrefs.HasKey("remainMax"))
        {
            int stored = PlayerPrefs.GetInt("remainMax");
            for (int i = 0; i < caps.Count; i++)
            {
                if (stored == caps[i].value)
                    selectedCap = i;
            }
        }
        updateAlphaDisplay();
        UpdateCap();
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

    public void AlphaAdd(float value)
    {
        selectedTransparency += value;
        if (selectedTransparency < 0)
            selectedTransparency = 1f;
        else if (selectedTransparency > 1)
            selectedTransparency = 0;
        PlayerPrefs.SetFloat("remainAlpha", selectedTransparency);
        updateAlphaDisplay();
        advancedUpdated?.Invoke();
    }

    public void CapAdd(int value)
    {
        selectedCap += value;
        if (selectedCap < 0)
            selectedCap = caps.Count - 1;
        else if (selectedCap >= caps.Count)
            selectedCap = 0;

        PlayerPrefs.SetInt("remainMax", caps[selectedCap].value);
        UpdateCap();
        advancedUpdated?.Invoke();
    }
    public void UpdateCap()
    {
        capLabel.text = caps[selectedCap].name;
    }

    private void updateAlphaDisplay()
    {
        if (displayImage != null)
            displayImage.color = new Color(displayImage.color.r, displayImage.color.g, displayImage.color.b, selectedTransparency);
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

class Cap
{
    public int value;
    public string name;

    public Cap(int value, string name)
    {
        this.value = value;
        this.name = name;
    }
}