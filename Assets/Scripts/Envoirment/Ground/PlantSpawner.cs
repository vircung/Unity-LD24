using UnityEngine;
using System.Collections;

public class PlantSpawner : MonoBehaviour
{
    public static float plantTime;
    public static float plantSpawnTime = 5f;

    GameObject plant;
    AudioClip plantPopUp;

    float levelBound;

    void Start()
    {
        plant = Resources.Load("Prefabs/Actors/Plant") as GameObject;
        plantPopUp = Resources.Load("Sounds/spawn Plant") as AudioClip;
        plantTime = 0f;
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
            SpawnPlant();
        }
    }

    private void GetLevelBounds()
    {
        levelBound = GroundScript.bound;
    }

    private void SpawnPlant()
    {
        plantTime += Time.deltaTime;

        if (plantTime > plantSpawnTime)
            plantTime = plantSpawnTime;

        if (plantTime == plantSpawnTime && myGame.plants.Count < myGame.maxPlants)
        {
            bool canSeed = true;

            Vector3 position = new Vector3();
            position.x = Random.Range(-levelBound, levelBound);
            position.z = Random.Range(-levelBound, levelBound);

            Collider[] objInrange = Physics.OverlapSphere(position, 1f);

            if (objInrange != null)
            {
                foreach (Collider col in objInrange)
                {
                    Debug.DrawLine(position, col.transform.position, Color.magenta, 5.0f);

                    GameObject obj = col.gameObject;
                    if (obj.transform.FindChild("TagPlant") || obj.transform.Find("TagPlant"))
                    {
                        canSeed = false;
                        break;
                    }
                }
            }

            if (canSeed)
            {
                GameObject newOne = Instantiate(plant, position, new Quaternion(1, Random.Range(5f, 360f), 1, 1)) as GameObject;
                myGame.plants.Add(newOne);
                plantTime -= plantSpawnTime;
            }
        }
    }

}
