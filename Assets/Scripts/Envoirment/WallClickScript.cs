using UnityEngine;
using System.Collections;

public class WallClickScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown(){
		GameScript.buildPoints++;
		//GameScript.hernivores.Clear();
		
	Destroy(transform.parent.gameObject);
	}
}
