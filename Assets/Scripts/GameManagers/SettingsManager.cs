using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private float volume = 1f;

    public bool vibrations = true;
    public bool music = true;
   
    private void Awake()
    {
        Instance = this;
    }
  
    public void changeMusicStat()
    {
        music = !music;
        
        if (music)
        {
            AudioListener.volume = volume;
            return;
        }
        AudioListener.volume = 0f;
    }

    public void setVolume(Slider slider)
    {
        if (music)
            AudioListener.volume = slider.value;
        volume = slider.value;
    }
    
    public void changeVibrationsStat()
    {
        vibrations = !vibrations;
        GameManager.Instance.vibrationEnabled = vibrations;
    }
   
}
