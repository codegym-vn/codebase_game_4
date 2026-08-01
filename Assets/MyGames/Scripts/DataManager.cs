using UnityEngine;

public class DataManager : MonoBehaviour
{
    const int defaultValue = 0;
    const float defaultValueAudio = 1;

    public static int DataCoin
    {
        get { return PlayerPrefs.GetInt(DataKey.KeyCoinId, defaultValue); }
        set { PlayerPrefs.SetInt(DataKey.KeyCoinId, value); }
    }
    public static float DataMusic
    {
        get { return PlayerPrefs.GetFloat(DataKey.KeyMusicId, defaultValueAudio); }
        set { PlayerPrefs.SetFloat(DataKey.KeyMusicId, value); }
    }
    public static float DataSfx
    {
        get { return PlayerPrefs.GetFloat(DataKey.KeySfxId, defaultValueAudio); }
        set { PlayerPrefs.SetFloat(DataKey.KeySfxId, value); }
    }
}
