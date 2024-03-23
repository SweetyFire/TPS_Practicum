using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private AnimGameOverScreen _gameOverScreen;
    [SerializeField] private AnimGameOverScreen _victoryScreen;

    private void Awake()
    {
        _gameOverScreen.Init();
        _victoryScreen.Init();
    }
}
