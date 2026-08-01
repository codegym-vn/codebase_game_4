using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RenameButton : MonoBehaviour
{
    private Button[] buttonList;
    private void Start()
    {
        buttonList = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].name = "ButtonLevel" + (i + 1);
            buttonList[i].GetComponentInChildren<TextMeshProUGUI>().text = "Level"+(i+1);
            buttonList[i].GetComponent<LevelButton>().levelIndex = (i + 1);
        }
    }
}
