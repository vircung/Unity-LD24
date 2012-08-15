using UnityEngine;
using System.Collections;

public class PlantScript : BasicScript {
	
	public GameObject seedInstance;
	public int seeds;
	public float getSeedRate;
	protected float growRate = 0.01f;
	protected float seedCost = 10f;
	
	// Use this for initialization
	protected new void Start () {
		base.Start();
		hp = Random.Range(0.1f, 5f);
		seeds = Random.Range(0, 3);
		getSeedRate = Random.Range(0f, 0.0001f);
	}
	
	// Update is called once per frame
	protected new void Update () {
		if(hp > seedCost && seeds > 0)
			Sow();
	}
	
	protected new void FixedUpdate(){
		Grow();	
	}
	
	public void Grow(){
		hp += growRate;
		float guess = Random.Range(0f, 1f);
		if(guess < getSeedRate){
			Debug.Log("Someones got a seed! " + guess);
			seeds ++;
		}
	}
	
	public void Sow(){
		Vector3 position = gameObject.transform.position;
		position.x += Mathf.Floor(Random.Range(-5, 5));
		if(position.x > 9) position.x = 9;
		if(position.x < -9) position.x = -9;
		position.z += Mathf.Floor(Random.Range(-5, 5));
		if(position.z > 9) position.z = 9;
		if(position.z < -9) position.z = -9;
		
		hp -= seedCost;
		seeds --;
		
		Instantiate(seedInstance, position, Quaternion.identity);
	}
	
	public void OnDestroy(){
				GameScript.buildPoints ++;
		//Destroy(gameObject);
	}
}
