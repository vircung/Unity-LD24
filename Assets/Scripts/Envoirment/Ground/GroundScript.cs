using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour
{
    public GameObject wall;
    public GameObject player;
    public AudioClip carnivoreTeleport;

    public static float bound = 0;

    private GameObject player_instance = null;
    private bool isStarted = false;



    void Start()
    {
        isStarted = false;
    }

    void Update()
    {
        if (!isStarted)
        {
            isStarted = true;
            Vector3 position = new Vector3(Random.Range(-bound, bound), 0f, Random.Range(-bound, bound));
            player_instance = (GameObject)Instantiate(player, position, Quaternion.identity);
            myGame.hernivores.Add(player_instance);
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

