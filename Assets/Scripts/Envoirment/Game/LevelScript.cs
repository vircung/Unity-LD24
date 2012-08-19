using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour
{

    public static float levelSize = 1f;
    public static float scale;
    public static float pos;

    float levelize_time = 2f;
    float max_level = 10f;
    float amount = .5f;
    bool levelize = true;

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

        wall = GameObject.Find("Ground(Clone)");
        wall.transform.localScale = new Vector3(levelSize, levelSize, levelSize);
        wall.transform.position = new Vector3(0, 0, 0);

        wall = GameObject.Find("North wall(Clone)");
        wall.transform.position = new Vector3(-pos, 0, pos);
        wall.transform.localScale = new Vector3(1, 1, scale);

        wall = GameObject.Find("South wall(Clone)");
        wall.transform.position = new Vector3(-pos, 0, -pos);
        wall.transform.localScale = new Vector3(1, 1, scale);

        wall = GameObject.Find("West wall(Clone)");
        wall.transform.position = new Vector3(-pos, 0, -pos);
        wall.transform.localScale = new Vector3(1, 1, scale);

        wall = GameObject.Find("East wall(Clone)");
        wall.transform.position = new Vector3(pos, 0, -pos);
        wall.transform.localScale = new Vector3(1, 1, scale);

        levelSize = Mathf.Lerp(levelSize, levelSize + amount, 0.01f);
        yield return new WaitForSeconds(levelize_time);
        levelize = true;
    }
}
