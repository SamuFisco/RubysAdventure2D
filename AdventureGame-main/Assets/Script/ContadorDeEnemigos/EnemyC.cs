using UnityEngine;

public class EnemyC : MonoBehaviour
{
    void Start()
    {
        EnemiesContainer.Instance?.AddEnemy();
    }

    public void Die()
    {
        EnemiesContainer.Instance?.RemoveEnemy();
        Destroy(gameObject);
    }
    public void Fix()
    {
        EnemiesContainer.Instance?.RemoveEnemy();
        Destroy(gameObject);
    }
}

