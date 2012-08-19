using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour
{

    public static float levelSize = 1f;
    public static float scale;
    public static float pos;

    private float levelize_time = 2f;
    private float max_level = 10f;
    public static float amount = .5f;
    private bool levelize = true;

    GameObject wall;

    void Start()
    {
        myGame.buildPoints = (int)levelSize;
    }


    void Update()
    {
        if (levelSize < max_level)
            if (levelize)
                StartCoroutine(Levelize());
    }

    IEnumerator Levelize()
    {
        levelize = false;

        pos = 10 * levelSize / 2;
        scale = pos * 2;
        GroundScript.bound = pos - 1;

        wall = GameObject.Find("Ground");
        wall.transform.localScale = new Vector3(levelSize, levelSize, levelSize);
        wall.transform.position = new Vector3(0, 0, 0);

        wall = GameObject.Find("North wall");
        wall.transform.position = new Vector3(-pos, 0, pos);
        wall.transform.localScale = new Vector3(1, 1, scale);

        wall = GameObject.Find("South wall");
        wall.transform.position = new Vector3(-pos, 0, -pos);
        wall.transform.localScale = new Vector3(1, 1, scale);

        wall = GameObject.Find("West wall");
        wall.transform.position = new Vector3(-pos, 0, -pos);
        wall.transform.localScale = new Vector3(1, 1, scale);

        wall = GameObject.Find("East wall");
        wall.transform.position = new Vector3(pos, 0, -pos);
        wall.transform.localScale = new Vector3(1, 1, scale);

        levelSize = Mathf.Lerp(levelSize, levelSize + amount, 0.01f);
        yield return new WaitForSeconds(levelize_time);
        levelize = true;
    }
}
