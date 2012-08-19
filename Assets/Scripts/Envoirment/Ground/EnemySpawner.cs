using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;
    public static float enemySpawnTime = 20f;
    public static float enemyTime;

    float levelBound;
    float nextEnemyRatio = 0.95f;

    void Start()
    {
        enemy = Resources.Load("Carnivore") as GameObject;
        enemyTime = 0f;
        levelBound = 0f;
    }

    void Update()
    {
        if (levelBound == 0)
        {
            GetLevelBounds();
        }
        else
        {
            SpawnEnemy();
        }
    }

    private void GetLevelBounds()
    {
        levelBound = GroundScript.bound;
    }

    private void SpawnEnemy()
    {
        enemyTime += Time.deltaTime;
        if (enemyTime > enemySpawnTime)
        {
            Vector3 position = new Vector3();
            position.x = Random.Range(-levelBound, levelBound);
            position.y = 1;
            position.z = Random.Range(-levelBound, levelBound);

            GameObject go = Instantiate(enemy, position, Quaternion.identity) as GameObject;
            myGame.carnivores.Add(go);
            enemyTime -= enemySpawnTime;
            enemySpawnTime *= nextEnemyRatio;
        }
    }
}
