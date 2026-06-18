using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using Unity.VisualScripting;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private SceneReference otherScene;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SetScene(otherScene);
        }
    }

    public void SetScene(SceneReference sceneValue)
    {
        if (sceneValue != null)
            SceneManager.LoadScene(sceneValue.BuildIndex);
    }
}
