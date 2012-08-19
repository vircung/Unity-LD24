using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour
{
    public static float bound = 0;

    GameObject wall = null;
    GameObject player = null;

    GameObject player_instance = null;
    bool isStarted = false;

    void Start()
    {
        isStarted = false;
        player = Resources.Load("Prefabs/Actors/Herbivore") as GameObject;
        wall = Resources.Load("Prefabs/Envoirment/Wall") as GameObject;
    }

    void Update()
    {
        if (!isStarted && player != null)
        {
            isStarted = true;
            Vector3 position = new Vector3(Random.Range(-bound, bound), 0f, Random.Range(-bound, bound));
            player_instance = (GameObject)Instantiate(player, position, Quaternion.identity);
            myGame.herbovores.Add(player_instance);
        }
    }

    void OnMouseDown()
    {
        if (myGame.buildPoints <= 0)
            return;
        Debug.Log(Camera.main);
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

