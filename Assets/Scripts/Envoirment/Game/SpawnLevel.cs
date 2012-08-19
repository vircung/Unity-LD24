using UnityEngine;
using System.Collections;

public class SpawnLevel : MonoBehaviour
{
    GameObject northWall;
    GameObject southWall;
    GameObject eastWall;
    GameObject westWall;

    GameObject ground;
    
    GameObject firstPlayer;

    void Awake()
    {
        northWall = Resources.Load("Prefabs/Envoirment/North wall") as GameObject;
        southWall = Resources.Load("Prefabs/Envoirment/South wall") as GameObject;
        eastWall = Resources.Load("Prefabs/Envoirment/East wall") as GameObject;
        westWall = Resources.Load("Prefabs/Envoirment/West wall") as GameObject;

        ground = Resources.Load("Prefabs/Envoirment/Ground") as GameObject;
        
        firstPlayer = Resources.Load("Prefabs/Actors/Herbivore") as GameObject;
    }

    void Start()
    {
        GameObject go;

        go = Instantiate(northWall, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.Rotate(new Vector3(0, 90, 0), Space.World);
        go = Instantiate(southWall, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.Rotate(new Vector3(0, 90, 0), Space.World);
        Instantiate(eastWall, Vector3.zero, Quaternion.identity);
        Instantiate(westWall, Vector3.zero, Quaternion.identity);

        Instantiate(ground, Vector3.zero, Quaternion.identity);

        go = Instantiate(firstPlayer, Vector3.up, Quaternion.identity) as GameObject;
        myGame.herbovores.Add(go);
    }
}