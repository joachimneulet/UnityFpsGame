using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    private Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    void Start(){
      resolutions = Screen.resolutions;
      resolutionDropdown.ClearOptions();

      List<string> options = new List<string>();

      int currentResolutionIndex = 0;
      for (int i=0; i < resolutions.Length; i++) {
        string option = resolutions[i].width + " x " + resolutions[i].height;
        options.Add(option);
        if(resolutions[i].width == Screen.currentResolution.width
          && resolutions[i].height == Screen.currentResolution.height){
          currentResolutionIndex = i;
        }
      }

      resolutionDropdown.AddOptions(options);
      resolutionDropdown.value = currentResolutionIndex;
      resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float _volume){
      audioMixer.SetFloat("masterVolume", _volume);
    }

    public void SetQuality(int _quality){
      QualitySettings.SetQualityLevel(_quality);
    }

    public void SetFullscreen (bool _isFullscreen){
      Screen.fullScreen = _isFullscreen;
    }

    public void SetResolution(int _resolutionIndex){
      Resolution res = resolutions[_resolutionIndex];
      Screen.SetResolution(res.width, res.height, Screen.fullScreen);

    }
}
