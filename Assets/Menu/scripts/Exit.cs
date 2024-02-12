using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Exit : MonoBehaviour
{
    public GameObject q;
    public GameObject w;

    public void exit()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
    public void start()
    {
        q.SetActive(false);
        w.SetActive(true);
        SceneManager.LoadScene("Village");

    }
}
