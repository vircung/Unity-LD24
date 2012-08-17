using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myGame : MonoBehaviour
{

    public static int score;
    public static int buildPoints;
    protected float time;
    protected float timeToRestart;
    public static bool gamesOn;
    public GameObject player;
    public static List<GameObject> hernivores = new List<GameObject>();


    // Use this for initialization
    void Start()
    {

        Vector3 position = new Vector3(Random.Range(-9f, 9f), 0f, Random.Range(-9f, 9f));
        hernivores.Add((GameObject)Instantiate(player, position, Quaternion.identity));

        score = 0;
        buildPoints = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (hernivores.Count > 0)
            gamesOn = true;
        else
        {
            gamesOn = false;
        }
    }

    void OnGUI()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        int boxWidth = 100;
        int boxHeight = 100;
        int buttonWidth = 80;
        int buttonHeight = 20;

        if (gamesOn)
            GUI.Box(new Rect(10, 10, 100, 50), new GUIContent("Score " + score + "\nBuildpoints " + buildPoints + "\nHerbivores " + hernivores.Count));
        else
        {
            GUI.Box(new Rect(x - boxWidth / 2, y - boxHeight / 2, boxWidth, boxHeight), new GUIContent("GAME OVER! \n" + "Score " + score));
            if (GUI.Button(new Rect(x - buttonWidth / 2, y, buttonWidth, buttonHeight), "Restart"))
            {
                Application.LoadLevel("Level1");
            }
        }
    }
}
