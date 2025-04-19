using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public GameObject panel;
    private LogicManager logicManager;
    private LayoutUI layoutUI;
    public Toggle soundToggle;
    public Toggle cameraRotationToggle;
    public Slider volumeSlider;
    public GameObject confirmationPopup;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] availableResolutions;

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        logicManager = FindFirstObjectByType<LogicManager>();
        layoutUI = FindFirstObjectByType<LayoutUI>();

        if (panel != null)
        {
            panel.SetActive(false);
        }
        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(false);
        }

        LoadSettings();

        volumeSlider.interactable = soundToggle.isOn;
        soundToggle.onValueChanged.AddListener(isOn =>
        {
            volumeSlider.interactable = isOn;
            SaveSettings();
        });

        volumeSlider.onValueChanged.AddListener(value =>
        {
            logicManager.SetSoundVolume(value);
            SaveSettings();
        });

        cameraRotationToggle.onValueChanged.AddListener(isEnabled =>
        {
            logicManager.ToggleCameraRotation(isEnabled);
            SaveSettings();
        });

        FillResolutionDropdown();
        resolutionDropdown.onValueChanged.AddListener((int index) =>
        {
            ChangeResolution(index);
            SaveSettings();
        });
    }

    private void FillResolutionDropdown()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string resolutionOption = $"{availableResolutions[i].width}x{availableResolutions[i].height}";
            options.Add(resolutionOption);

            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void ChangeResolution(int index)
    {
        if (index >= 0 && index < availableResolutions.Length)
        {
            Resolution selectedResolution = availableResolutions[index];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

            Debug.Log($"Resolution changed to: {selectedResolution.width}x{selectedResolution.height}");
        }
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("SoundEnabled", soundToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("SoundVolume", volumeSlider.value);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("CameraRotationEnabled", cameraRotationToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        bool soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        soundToggle.isOn = soundEnabled;

        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        volumeSlider.value = soundVolume;

        bool cameraRotationEnabled = PlayerPrefs.GetInt("CameraRotationEnabled", 1) == 1;
        cameraRotationToggle.isOn = cameraRotationEnabled;
    }

    public void ShowPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void goBack()
    {
        if (panel != null)
        {
            panel.SetActive(false);
            layoutUI.gameObject.SetActive(true);
            logicManager.isPromotionActive = false; //enable pieces selecting
        }
    }

    public void ShowRestartConfirmation()
    {
        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(true);
        }
    }

    public void ConfirmRestart()
    {
        if (logicManager != null)
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("ChessScene");
            logicManager.Initialize();
        }

        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(false);
        }
    }

    public void CancelRestart()
    {
        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(false);
        }
    }
}