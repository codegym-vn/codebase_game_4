using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Slider sliderMusic;
    public Slider slidersfx;
    private void Start()
    {
        sliderMusic.value = DataManager.DataMusic;
        slidersfx.value = DataManager.DataSfx;
    }
    // Update is called once per frame
    public void UpdateMusic(float volume)
    {
        AudioManager.Instance.OnVolumeBackGround(volume);
        DataManager.DataMusic = volume;
    }
    public void UpdateSfx(float volume)
    {
        AudioManager.Instance.OnVolumeSfx(volume);
        DataManager.DataSfx = volume;
    }
}
