using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
	private string[] teamColors = {"GreenTeam",
		"BlueTeam", 
		"RedTeam", 
		"OrangeTeam"};
		
	private float radius, range = 5;
	public float timeBetweenShots = 1f;
	public int hitDmg = 20;
	public bool canFight = true;
	public bool isFighting = false;
	
	public List<GameObject> EnemiesInRange = new List<GameObject>();
	public List<GameObject> EnemiesInSight = new List<GameObject>();

	void Start(){
		radius = GetComponent<SphereCollider>().radius;
		range = radius*radius;
		StartCoroutine(CheckRange());
	}

    void Update(){
		
		if(EnemiesInRange.Count > 0 && isFighting == false){
			StartCoroutine(ShootFirstThing());
			isFighting = true;
		}
		if(EnemiesInRange.Count <= 0){
			isFighting = false;
			StopCoroutine(ShootFirstThing());
		}
    }
	
	private IEnumerator CheckRange(){
		while(true){
			yield return new WaitForSeconds(0.2f);
			if(EnemiesInSight.Count > 0){
				foreach(GameObject e in EnemiesInSight){
					if(e != null){
						if(Vector3.Distance(e.transform.position, this.transform.position) < range){
							if(!EnemiesInRange.Contains(e))
								EnemiesInRange.Add(e);
						}
						else
							EnemiesInRange.Remove(e);
					}
					else{
						EnemiesInRange.Remove(e);
					}
				}
			
				EnemiesInSight = EnemiesInSight.Where(x =>
					x != null).ToList();
			}
		}
	}

	private IEnumerator ShootFirstThing(){
		GameObject target;
		while(EnemiesInRange.Count > 0){
			target = EnemiesInRange.ToArray()[0];
			try{
				DebugShot(target);
			}
			catch{}
			yield return new WaitForSeconds(timeBetweenShots);
		}
	}
	
	private void DebugShot(GameObject toBeHit){
		if(toBeHit != null){
			if(toBeHit.GetComponent<COMP_stat>().HP < hitDmg)
				EnemiesInRange.Remove(toBeHit);
			
			toBeHit.GetComponent<COMP_stat>().HP -= hitDmg;
		}
		else{
			EnemiesInRange.Remove(toBeHit);
		}
	}

	public void OnTriggerEnter(Collider collider){
		if(teamColors.Contains(collider.tag) && collider.tag != this.tag){
			if(collider.gameObject.GetComponent<DummyMove>() != null)
				collider.gameObject.GetComponent<DummyMove>().EnemiesInSight.Add(this.gameObject);
			if(collider.gameObject.GetComponent<DefenseTower>() != null)
				collider.gameObject.GetComponent<DefenseTower>().EnemiesInSight.Add(this.gameObject);
		}
	}
	
	public void OnTriggerExit(Collider collider){
		if(teamColors.Contains(collider.tag) && collider.tag != this.tag){
			if(collider.gameObject.GetComponent<DummyMove>() != null)
				collider.gameObject.GetComponent<DummyMove>().EnemiesInSight.Remove(this.gameObject);
			if(collider.gameObject.GetComponent<DefenseTower>() != null)
				collider.gameObject.GetComponent<DefenseTower>().EnemiesInSight.Remove(this.gameObject);
		}
	}

}
