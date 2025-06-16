using UnityEngine;

public class Quit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Quit_quit()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
