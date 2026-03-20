using UnityEngine;

public class RestartScene : MonoBehaviour
{
    public RestartScene Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void RestartCurrentScene()
    {
        // Get the current active scene
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }
}
