using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils
{
    public static float ExpDecayT(float t)
    {
        return 1f - Mathf.Exp(-t * Time.deltaTime);
    }

    public static void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
