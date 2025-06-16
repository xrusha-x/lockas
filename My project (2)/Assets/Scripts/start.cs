using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    
    public void NextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
