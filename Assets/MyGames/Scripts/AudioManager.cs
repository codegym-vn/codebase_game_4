using UnityEngine;
[AddComponentMenu("DangSon/AudioManager")]
public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource backgroundAudio;
    public AudioSource sfxPlayerAudio;
    public AudioSource sfxEnemyAudio;
    public AudioSource sfxUIAudio;
    [Header("Audio Clips")]
    public AudioClip backGroundMusic;
    public AudioClip clickmusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayMusic(backGroundMusic);
        OnVolumeSfx(DataManager.DataSfx);
        OnVolumeBackGround(DataManager.DataMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PlayMusic(AudioClip clip)
    {
        backgroundAudio.clip = clip;
        backgroundAudio.loop = true;
        backgroundAudio.Play();
    }
    public void PlaySfxPlayer(AudioClip clip)
    {
        sfxPlayerAudio.PlayOneShot(clip);
    }
    public void playSfxUIMusic(AudioClip clip)
    {
        sfxUIAudio.PlayOneShot(clip);
    }
    public void OnClickMusic()
    {
        playSfxUIMusic(clickmusic);
    }
    public void OnVolumeBackGround(float volume)
    {
        backgroundAudio.volume = volume;
    }
    public void OnVolumeSfx(float volume)
    {
        sfxPlayerAudio.volume = volume;
        sfxEnemyAudio.volume = volume;
        sfxUIAudio.volume = volume;
    }
}
