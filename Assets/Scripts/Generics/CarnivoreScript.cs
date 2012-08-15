using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	protected new void LookForFood(){
		target = null;
		List<GameObject> possibleTargets = new List<GameObject>();
		
		
			GameObject[] tmp = GameObject.FindGameObjectsWithTag("Critter");
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
