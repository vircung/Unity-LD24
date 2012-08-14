using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveScript : BasicScript {
	
	protected float moveForce = 40f;
	protected TYPE eats;
	protected float strenght;
	public float energy;
	
	public GameObject target;
	public float targetDist;
	protected Vector3 targetDir;
	
	public GameObject enemy;
	public float enemyDist;
	protected Vector3 enemyDir;
	protected float safeDistance = 4f;
	protected float runCost = 10f;
	
	public float interactionDist;
	
	// Use this for initialization
	
	protected new void Start () {
		base.Start();
		strenght = Random.Range(2f, 5f);
		energy = Random.Range(10f, 50f);
		hp = Random.Range(8f, 15f);
		interactionDist = Random.Range(1f, 3f);
		enemy = null;
		type = TYPE.CRITTER | TYPE.MEAT;
		InvokeRepeating("LookForFood", 0.3f, 1f);
	}
	
	// Update is called once per frame
	protected new void Update () {
		base.Update();
	}
	
	protected  new void FixedUpdate(){
		base.FixedUpdate();
		LookForFood();
		LookForEnemy();
		
		if(enemyDist < safeDistance && enemy != null){
			if(energy > runCost){
				Run(-enemyDir);
			}
			else{
				Move(-enemyDir);
			}
		}
		else if(targetDist <= interactionDist && target != null){
			if(eats == TYPE.HERB){
				PlantScript plantScript = (PlantScript)target.GetComponent("PlantScript");
				Eat(plantScript.GetHurt(strenght));
			}
			else if (eats == BasicScript.TYPE.MEAT){
				HerbovoreScript plantScript = (HerbovoreScript)target.GetComponent("HerbovoreScript");
				Eat(plantScript.GetHurt(strenght));
			}
			
			
		}
		else{
			Move(targetDir);
		}
	}
	
	
	public void Eat(float amount){
		energy += amount;
	}
	
	public TYPE WhatEats(){
		return eats;
	}
	
	
	public void Move(Vector3 dir){
		if(rigidbody != null){
			rigidbody.AddForce(dir * moveForce * Time.deltaTime);
		}
	}
	
	public void Run(Vector3 dir){
		if(rigidbody != null){
			rigidbody.AddForce(dir * moveForce * 10 * Time.deltaTime);
			energy -= runCost;
		}
	}
	
	public void LookForFood(){
		target = null;
		List<GameObject> possibleTargets = new List<GameObject>();
		if(eats == TYPE.HERB){
			GameObject[] tmp = GameObject.FindGameObjectsWithTag("Plant");
			foreach(GameObject obj in tmp){
				possibleTargets.Add(obj.transform.parent.gameObject);
			}
		}if(eats == TYPE.MEAT){
			GameObject[] tmp = GameObject.FindGameObjectsWithTag("Critter");
			foreach(GameObject obj in tmp){
				possibleTargets.Add(obj.transform.parent.gameObject);
			}
		}
		
		foreach(GameObject obj in possibleTargets){
			Vector3 tmpDir = obj.collider.bounds.center - collider.bounds.center;
			Ray ray = new Ray(collider.bounds.center, tmpDir);
			
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit)){
				Debug.DrawRay(collider.bounds.center, tmpDir);
				if(hit.distance < targetDist || target == null){
					target = obj;
					targetDist = hit.distance;
					targetDir = ray.direction;
				}
			}
		}
	}
	
	public void LookForEnemy(){
		enemy = null;
		List<GameObject> possibleEnemys = new List<GameObject>();
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Critter");
		foreach(GameObject obj in temp){
			GameObject isEnemy = obj.transform.parent.gameObject;
			CarnivoreScript scripts = (CarnivoreScript)isEnemy.GetComponent("CarnivoreScript");
			if(scripts){
				possibleEnemys.Add(isEnemy);
			}
			
		}
		
		foreach(GameObject en in possibleEnemys){
			Ray ray = new Ray(collider.bounds.center, en.collider.bounds.center - collider.bounds.center);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit)){
				if(enemy == null || enemyDist > hit.distance){
					enemy = en;
					enemyDist = hit.distance;
					enemyDir = ray.direction;
				}
			}
		}
	}
	
}
