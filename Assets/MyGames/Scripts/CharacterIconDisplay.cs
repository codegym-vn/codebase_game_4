using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIconDisplay : MonoBehaviour
{
    [Header("Character Icon")]
    public Image iconImage;
    [Header("Character Icon Sprite")]
    public Sprite ninjaIcon;
    public Sprite knightIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowIcon();
    }

    private void ShowIcon()
    {
       string selectedCharacter = CharacterSelector.Instance.GetSelectedCharacter();
        if (iconImage != null)
        {
            if (selectedCharacter == "Ninja")
            {
                iconImage.sprite = ninjaIcon;
            }
            else if (selectedCharacter == "Knight")
            {
                iconImage.sprite = knightIcon;
            }
        }
    }

    
}
