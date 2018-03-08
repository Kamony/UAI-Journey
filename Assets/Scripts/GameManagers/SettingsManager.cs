using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private float volume = 1f;

    [SerializeField] private bool vibrations = true;
    [SerializeField] private bool music = true;
   
    private void Awake()
    {
        Instance = this;
        PlayerPrefs.SetInt("vibrations",1);   
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
        if (vibrations)
            PlayerPrefs.SetInt("vibrations",1);    
        else
            PlayerPrefs.SetInt("vibrations",0);
        
    }
   
}
