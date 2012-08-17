using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour
{

    public GameObject plant;
    public GameObject wall;
    public GameObject enemy;
    public AudioClip plantPopUp;
    public AudioClip carnivoreTeleport;


    float plantTime;
    float enemyTime;
    float plantSpawnTime = 5;
    float enemySpawnTime = 20;
    // Use this for initialization
    void Start()
    {
        plantTime = 0f;
        enemyTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        plantTime += Time.deltaTime;
        enemyTime += Time.deltaTime;

        if (plantTime > plantSpawnTime)
        {
            Vector3 position = new Vector3();
            position.x = Random.Range(-9, 9);
            position.z = Random.Range(-9, 9);

            bool canSeed = true;

            Collider[] objInrange = Physics.OverlapSphere(position, 1);
            if (objInrange != null)
            {
                foreach (Collider col in objInrange)
                {
                    GameObject obj = col.gameObject;
                    if (obj.tag == "Plant")
                    {
                        canSeed = false;
                        break;
                    }
                }
            }
            if (canSeed)
            {
                Instantiate(plant, position, new Quaternion(1, Random.Range(5f, 360f), 1, 1));
                plantTime -= plantSpawnTime;
            }
        } if (enemyTime > enemySpawnTime)
        {
            Vector3 position = new Vector3();
            position.x = Random.Range(-9, 9);
            position.z = Random.Range(-9, 9);

            Instantiate(enemy, position, Quaternion.identity);
            enemyTime -= enemySpawnTime;
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
}
