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
    public static List<GameObject> carnivores = new List<GameObject>();
    public static int playerLvl;


    protected float slowTime = 0.3f;
    protected float normalTime = 1.0f;
    protected float fastTime = 2.0f;
    protected float superFastTime = 4.0f;

    protected bool ending = false;

    protected int btnSize = 30;
    protected int btnOff = 5;
    public static bool debug = true;
    private float stopingFactor = 0.07f;
    private float minimalScale = 0.06f;


    public static List<GameObject> plants = new List<GameObject>();
    public static float maxPlants;


    // Use this for initialization
    void Start()
    {
        maxPlants = LevelScript.levelSize * 10;

        NormalTime();
        
        playerLvl = 0;
        score = 0;
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

        if (!gamesOn)
        {
            if (Time.timeScale > minimalScale)
            {
                StopTime();
            }

            GUI.Box(new Rect(x - boxWidth / 2, y - boxHeight / 2, boxWidth, boxHeight), new GUIContent("GAME OVER! \n" + "Score " + score));

            if (GUI.Button(new Rect(x - buttonWidth / 2, y, buttonWidth, buttonHeight), "Restart"))
            {
                Application.LoadLevel("Level1");
            } if (GUI.Button(new Rect(x - buttonWidth / 2, y + buttonHeight + 5, buttonWidth, buttonHeight), "Menu"))
            {
                Application.LoadLevel("Title");
            }
        }

        if (debug && !ending)
        {
            if (GUI.Button(new Rect(Screen.width - btnOff - (btnSize + btnOff) * 1, btnOff, btnSize, btnSize), " S"))
            {
                SlowTime();
            }
            if (GUI.Button(new Rect(Screen.width - btnOff - (btnSize + btnOff) * 2, btnOff, btnSize, btnSize), " N"))
            {
                NormalTime();
            }
            if (GUI.Button(new Rect(Screen.width - btnOff - (btnSize + btnOff) * 3, btnOff, btnSize, btnSize), " F"))
            {
                FastTime();
            } if (GUI.Button(new Rect(Screen.width - btnOff - (btnSize + btnOff) * 4, btnOff, btnSize, btnSize), "SF"))
            {
                SuperFastTime();
            }
        }
    }

    private void StopTime()
    {
        ending = true;
        foreach (GameObject go in plants)
        {
            Destroy(go);
        }
        foreach (GameObject go in carnivores)
        {
            Destroy(go);
        }
        ScaleTime(Mathf.Lerp(Time.timeScale, 0.0f, stopingFactor));
    }

    private void SuperFastTime()
    {
        ScaleTime(superFastTime);
    }

    private void SlowTime()
    {
        ScaleTime(slowTime);
    }

    private void NormalTime()
    {
        ScaleTime(normalTime);
    }

    private void FastTime()
    {
        ScaleTime(fastTime);
    }

    private void ScaleTime(float scale)
    {
        Time.timeScale = scale;
    }
}
