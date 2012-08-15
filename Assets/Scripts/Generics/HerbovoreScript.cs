using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HerbovoreScript : LiveScript {
	
	public GameObject yougOne;
	
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
			GameScript.score += 200;
		}
		TrySplit();
		base.FixedUpdate();
		energy += energyRefill;
	}
	
	protected void TrySplit(){
		if(energy > 70){
			Vector3 position = transform.position;
			position.y += .5f;
			
			Instantiate(yougOne, position, Quaternion.identity);
			energy -= 60;
			GameScript.score += 50;
		}
	}
	
	protected new void LookForFood(){
		target = null;
		List<GameObject> possibleTargets = new List<GameObject>();

			GameObject[] tmp = GameObject.FindGameObjectsWithTag("Plant");
			foreach(GameObject obj in tmp){
				possibleTargets.Add(obj.transform.parent.gameObject);
			}
		
		
		foreach(GameObject obj in possibleTargets){
			Vector3 tmpDir = obj.collider.bounds.center - collider.bounds.center;
			Ray ray = new Ray(collider.bounds.center, tmpDir);
			
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider == obj.collider){
					Debug.DrawRay(collider.bounds.center, tmpDir);
					if(hit.distance < targetDist || target == null){
						target = obj;
						targetDist = hit.distance;
						targetDir = ray.direction;
					}
				}
			}
		}
	}
	
}
