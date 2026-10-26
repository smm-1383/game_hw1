using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] groundObstacles, airObstacles, coins;
    [SerializeField] float spawnX = 12f, groundY;
    [SerializeField] float[] airHeights, coinHeights;
    [SerializeField] float minGap = 1f, maxGap = 2.2f;
    [SerializeField, Range(0f, 1f)] float airChance = 0.3f, coinChance = 0.4f;

    float timer = 1f;

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm.IsOver) return;
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        if (airObstacles.Length > 0 && airHeights.Length >= airObstacles.Length && Random.value < airChance)
        {
            int i = Random.Range(0, airObstacles.Length);
            Instantiate(airObstacles[i], new Vector3(spawnX, airHeights[i], 0f), Quaternion.identity);
        }
        else
            Spawn(groundObstacles, spawnX, groundY);

        if (coins.Length > 0 && coinHeights.Length > 0 && Random.value < coinChance)
            Spawn(coins, spawnX + 3f, coinHeights[Random.Range(0, coinHeights.Length)]);

        timer = Random.Range(minGap, maxGap) * (6f / Mathf.Max(6f, gm.Speed));
    }

    void Spawn(GameObject[] pool, float x, float y)
    {
        if (pool.Length == 0) return;
        Instantiate(pool[Random.Range(0, pool.Length)], new Vector3(x, y, 0f), Quaternion.identity);
    }
}
