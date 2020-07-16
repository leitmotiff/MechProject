using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DummyMove : MonoBehaviour
{
	private string[] teamColors = {"GreenTeam",
		"BlueTeam", 
		"RedTeam", 
		"OrangeTeam"};
	[Range(0,3)]
	public int movementType;
	public int speed = 12, hitDmg = 1;
	private float radius, range = 5;
	public GameObject PathGO;
	public Transform[] path;
	private Transform cNode, blank;
	public bool canMove = true, canFight = true;
	public bool isMoving = false, isFighting = false;
	private int q = 1;
	
	public List<GameObject> EnemiesInRange = new List<GameObject>();
	public List<GameObject> EnemiesInSight = new List<GameObject>();
	
    void Start(){
		radius = GetComponent<SphereCollider>().radius;
		range = radius*radius;
		StartCoroutine(CheckRange());
		if(PathGO != null){
			path = PathGO.GetComponentsInChildren<Transform>();
			cNode = path[q];
		}
    }

    void Update(){
		
		if(EnemiesInRange.Count > 0 && isFighting == false){
			canMove = false;
			StartCoroutine(ShootFirstThing());
			isFighting = true;
		}
		if(EnemiesInRange.Count <= 0){
			canMove = true;
			isFighting = false;
			StopCoroutine(ShootFirstThing());
		}
		
		if(canMove){
			switch(movementType){
				case 3:
					if(path.Length > 0) FollowPathNodes();
					break;
				
				default:
					break;
			}
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
					else
						EnemiesInRange.Remove(e);
				}
				
				EnemiesInSight = EnemiesInSight.Where(x =>
					x != null).ToList();
			}
		}
	}

	private void FollowPathNodes(){
		Vector3 directionOfTravel = cNode.position - this.transform.position;
        directionOfTravel.Normalize();
		if(canMove){
			this.transform.Translate(
				(directionOfTravel.x * speed * Time.deltaTime),
				(0f),
				(directionOfTravel.z * speed * Time.deltaTime),
				Space.World);
		}

		if(Mathf.Abs(transform.position.x - cNode.position.x) < 1f && 
			Mathf.Abs(transform.position.z - cNode.position.z) < 1f)
		{
			q++;
			if(q < path.Length)
				cNode = path[q];
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
			yield return new WaitForSeconds(1f);
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
