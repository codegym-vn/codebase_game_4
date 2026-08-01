using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.CompareTag("Player"))
            {
                FindAnyObjectByType<LevelUnlock>().CompleteLevel();
                SceneManager.LoadScene("SelectLevel");
            }
        }
    }
}
