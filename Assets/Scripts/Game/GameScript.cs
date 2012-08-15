using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {
	
	public static int score;
	public static int buildPoints;
	
	protected float time;
	protected float timeToRestart;
	public static bool gamesOn;
	
	public GameObject player;
	public static List<GameObject> hernivores = new List<GameObject>();
	

	// Use this for initialization
	void Start () {
		
		Vector3 position = new Vector3(Random.Range(-9f, 9f), 0f, Random.Range(-9f, 9f));
		hernivores.Add((GameObject)Instantiate(player, position, Quaternion.identity));
		
		score = 0;
		buildPoints = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if(hernivores.Count > 0)
			gamesOn = true;
		else {
			gamesOn = false;
		}
	}
	
	
	void OnGUI () {
		
		
		if(gamesOn)
			GUI.Box (new Rect (10,10,100,50), new GUIContent("Score " + score + "\nBuildpoints " + buildPoints));
		else {
			GUI.Box (new Rect (200,150,100,100), new GUIContent("GAME OVER! \n" + "Score " + score));
			if(GUI.Button(new Rect(210, 200, 80, 20), "Restart")){
				Application.LoadLevel(0);
			};
		}
	}
}
