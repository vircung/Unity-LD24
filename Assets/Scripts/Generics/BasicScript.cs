using UnityEngine;
using System.Collections;

public class BasicScript : MonoBehaviour {
		
	public enum TYPE {
		UNKNOWN = 0,
		HERB = 1,
		MEAT = HERB << 1,
		CRITTER = MEAT << 1,
	};
	
	protected TYPE type;
	public float hp;
	
	// Use this for initialization
	protected void Start () {
		
	}
	
	// Update is called once per frame
	protected void Update () {
	
	}
	
	protected void FixedUpdate(){
	}
	
	public float GetHurt(float dmg){
		float damageTaken = dmg;
		hp -= dmg;
		if(hp <= 0){
			damageTaken -=hp;
			GameScript.hernivores.Remove(gameObject);
			Destroy(gameObject);
		}
		return damageTaken;
	}
	
}
