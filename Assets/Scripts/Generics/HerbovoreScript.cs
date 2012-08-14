using UnityEngine;
using System.Collections;

public class HerbovoreScript : LiveScript {
	
	
	protected float energyRefill = 0.01f;
	protected float attackDist;
	// Use this for initialization
	protected new void Start (){
		base.Start();
		eats = TYPE.HERB;
		attackDist = safeDistance + 1;
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update();
	}
	
	protected new void FixedUpdate(){
		if(energy > 50f && enemyDist <+ attackDist && enemy != null){
			Destroy(enemy);
			energy -= 50;
		}
		base.FixedUpdate();
		energy += energyRefill;
	}
}
