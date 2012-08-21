using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public static float enemySpawnTime = 20f;
    public static float enemyTime;

    float levelBound;
    float nextEnemyRatio = 0.95f;

    GameObject enemy;
    AudioClip enemyPopUp;

    void Start()
    {
        enemy = Resources.Load("Prefabs/Actors/Carnivore") as GameObject;
        enemyPopUp = Resources.Load("Sounds/spawn enemy") as AudioClip;
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
            audio.clip = enemyPopUp;
            audio.Play();
            myGame.carnivores.Add(go);
            enemyTime -= enemySpawnTime;
            enemySpawnTime *= nextEnemyRatio;
        }
    }
}
