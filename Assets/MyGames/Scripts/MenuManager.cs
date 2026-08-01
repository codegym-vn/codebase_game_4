using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public void OnButtonClick(string levelName)
    {
        AudioManager.Instance.OnClickMusic();
        SceneManager.LoadScene(levelName);
    }
}
