using UnityEngine;

public class LevelUnlock : MonoBehaviour
{
    public int currentLevel = 1;

    public void CompleteLevel()
    {
        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if(currentLevel>=unlocked)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }
}
