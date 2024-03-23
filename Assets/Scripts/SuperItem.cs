using UnityEngine;

public class SuperItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerHealth player)) return;

        foreach (EnemySpawner spawner in FindObjectsOfType<EnemySpawner>())
        {
            spawner.enabled = false;
        }

        foreach (EnemyHealth enemy in FindObjectsOfType<EnemyHealth>())
        {
            enemy.TakeDamage(enemy.Health);
        }

        player.ShowVictoryScreen();
        Destroy(gameObject);
    }
}
