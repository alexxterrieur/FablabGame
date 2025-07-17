using UnityEditor.SearchService;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
