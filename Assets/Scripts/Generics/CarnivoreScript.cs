using UnityEngine;
using System.Collections;

public class CarnivoreScript : LiveScript {
	
	// Use this for initialization
	protected new void Start () {
		base.Start();
		eats = TYPE.MEAT;
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update();
	}
	
	protected new void FixedUpdate(){
		base.FixedUpdate();
	}
}
