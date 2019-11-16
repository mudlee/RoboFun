using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    public void OnRestart() {
        SceneManager.LoadScene("Level1");
    }
}
