using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour
{

    public GameObject plant;
    public GameObject wall;
    public GameObject enemy;
    public GameObject player;
    public AudioClip plantPopUp;
    public AudioClip carnivoreTeleport;

    private GameObject player_instance = null;
    private bool isStarted = false;

    public static float bound = 0;

    float plantTime;
    float enemyTime;
    float plantSpawnTime = 5f;
    float enemySpawnTime = 20f;

    int y_start = 10;
    int line_width = 180;
    int line_height = 25;
    int line_offset = 5;


    float nextEnemyRatio = 0.95f;
    // Use this for initialization
    void Start()
    {
        plantTime = 0f;
        enemyTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted)
        {
            isStarted = true;
            Vector3 position = new Vector3(Random.Range(-bound, bound), 0f, Random.Range(-bound, bound));
            player_instance = (GameObject)Instantiate(player, position, Quaternion.identity);
            myGame.hernivores.Add(player_instance);
        }
        SpawnPlant();
        SpawnEnemy();
    }

    private void SpawnPlant()
    {
        plantTime += Time.deltaTime;

        if (plantTime > plantSpawnTime)
            plantTime = plantSpawnTime;

        if (plantTime == plantSpawnTime && myGame.plants.Count < myGame.maxPlants)
        {
            Vector3 position = new Vector3();
            position.x = Random.Range(-bound, bound);
            position.z = Random.Range(-bound, bound);

            bool canSeed = true;

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

    private void SpawnEnemy()
    {
        enemyTime += Time.deltaTime;
        if (enemyTime > enemySpawnTime)
        {
            Vector3 position = new Vector3();
            position.x = Random.Range(-bound, bound);
            position.y = 1;
            position.z = Random.Range(-bound, bound);

            GameObject go = Instantiate(enemy, position, Quaternion.identity) as GameObject;
            myGame.carnivores.Add(go);
            enemyTime -= enemySpawnTime;
            enemySpawnTime *= nextEnemyRatio;
        }
    }

    void OnMouseDown()
    {
        if (myGame.buildPoints <= 0)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 position = new Vector3();

            position.x = Mathf.Floor(hit.point.x);
            position.y = 0;
            position.z = Mathf.Floor(hit.point.z);

            Instantiate(wall, position, Quaternion.identity);
            myGame.buildPoints--;
        }
    }

    void OnGUI()
    {
        ArrayList tt = new ArrayList();
        string[] text = {       "Score " + myGame.score,
                                "Max lvl " + myGame.playerLvl,
                                "Build points " + myGame.buildPoints,
                                "Herbivores " + myGame.hernivores.Count,
                                "Plants " + myGame.plants.Count + " out of " + myGame.maxPlants,
                                "Next enemy in " + (1 + (int)(enemySpawnTime - enemyTime)),
                                "Next plant in " + (1 + (int)(plantSpawnTime - plantTime)),
                                "Time scale " + Time.timeScale,
                            };
        tt.AddRange(text);
        for (int i = 0; i < tt.Count; i++)
        {
            GUI.Box(new Rect(10, y_start + (line_height + line_offset) * i, line_width, line_height), tt[i].ToString());
        }
    }
}

