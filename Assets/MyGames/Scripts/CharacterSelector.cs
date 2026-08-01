using UnityEngine;

public class CharacterSelector : Singleton<CharacterSelector>
{
   private const string KEY = "SelectedCharacter";
    public void SelectCharacter(string CharacterName)
    {
        PlayerPrefs.SetString(KEY, CharacterName);
        PlayerPrefs.Save();
    }
    public string GetSelectedCharacter()
    {
        return PlayerPrefs.GetString(KEY, "Ninja");
    }
}
