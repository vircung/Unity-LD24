using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour
{
    int y_start = 10;
    int line_width = 130;
    int line_height = 25;
    int line_offset = 5;

    void OnGUI()
    {
        ArrayList textToDisplay = new ArrayList();
        string[] normalText = { "Score " + myGame.score,
                                "Max lvl " + myGame.playerLvl,
                                "Build points " + myGame.buildPoints,
                                "Herbivores " + myGame.herbovores.Count,
                                "Plants " + myGame.plants.Count + " out of " + myGame.maxPlants,
                                "Next enemy in " + (1 + (int)(EnemySpawner.enemySpawnTime - EnemySpawner.enemyTime)),
                                "Next plant in " + (1 + (int)(PlantSpawner.plantSpawnTime - PlantSpawner.plantTime)),
                            };

        string[] debugText = {
                                "Time scale " + Time.timeScale,
                            };

        textToDisplay.AddRange(normalText);
        if (myGame.debug)
        {
            textToDisplay.AddRange(debugText);
        }

        for (int i = 0; i < textToDisplay.Count; i++)
        {
            GUI.Box(new Rect(10, y_start + (line_height + line_offset) * i, line_width, line_height), textToDisplay[i].ToString());
        }

    }
}
