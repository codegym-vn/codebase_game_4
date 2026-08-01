using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;
    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("OnLevelClick", 0.5f);
    }
    public void OnLevelButtonClicked()
    {
        if (button.interactable)
        {
        SceneManager.LoadScene("Level" + levelIndex);
        }
    }
    void OnLevelClick()
    {
        button = GetComponent<Button>();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (levelIndex > unlockedLevel)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
