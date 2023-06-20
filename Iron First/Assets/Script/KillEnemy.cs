using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    public void SpawnNextEnemy()
    {
        WorldManager.Instance.SpawnRandomEnemy();
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
