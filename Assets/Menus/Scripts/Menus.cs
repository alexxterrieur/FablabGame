using UnityEngine;

public class Menus : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadGivenScene();
        }
    }

    private void LoadGivenScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
