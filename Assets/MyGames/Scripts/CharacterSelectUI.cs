using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [Header("Button")]
    public Button btnNinja;
    public Button btnKnight;
    [Header("Highlight")]
    public Image highlightNinja;
    public Image highlightKnight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btnNinja.onClick.AddListener(() => OnCharacterSelected("Ninja"));            //    (x,y) => OnCharacterSelected(x,y)
        btnKnight.onClick.AddListener(() => OnCharacterSelected("Knight"));
        UpdateHightLight(CharacterSelector.Instance.GetSelectedCharacter());
    }

    private void OnCharacterSelected(string name)
    {
        CharacterSelector.Instance.SelectCharacter(name);
        UpdateHightLight(name);
    }

    // Update is called once per frame
    void UpdateHightLight(string name)
    {
        if(highlightNinja!=null)
        {
            highlightNinja.enabled = name == "Ninja";            
        }
        if (highlightKnight != null)
        {
           highlightKnight.enabled = name == "Knight";
        }
    }
    
}
