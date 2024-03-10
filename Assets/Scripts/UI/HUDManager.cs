using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private AnimGameOverScreen _gameOverScreen;

    private void Awake()
    {
        _gameOverScreen.Init();
    }
}
