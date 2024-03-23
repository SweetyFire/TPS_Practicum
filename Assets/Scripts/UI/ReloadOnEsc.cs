using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadOnEsc : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
