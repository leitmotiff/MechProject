using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI_1 : MonoBehaviour
{
	public bool testTrig = false;
	public List<GameObject> OBJ_Zone_List;
	private GameObject currentActiveOBJ, cNode;
	
	//replace GameObjectt with in-script Transform once done with debug
	public GameObject drop_node_prefab;
	private GameObject[] nodeList = new GameObject[5];
	
	public float walkSpeed = 5;
	private float maxX, maxZ;
	
	public bool ActiveMoving = false, MoveToObjective = false;
	
	private System.Random rand = new System.Random();
    
	void Start()
    {
        currentActiveOBJ = OBJ_Zone_List.ToArray()[rand.Next(OBJ_Zone_List.Count)];
		PlaceNavDrops();
    }

    void Update()
    {
        if(testTrig){
			PlaceNavDrops();
			testTrig = false;
		}
		
		if(MoveToObjective)
			FollowNodes();
    }
	
	private void PlaceNavDrops(){
		Transform mainObj = currentActiveOBJ.transform;
		bool CLEAR_PATH = false;
		maxX = mainObj.position.x - transform.position.x;
		maxZ = mainObj.position.z - transform.position.z;
		
		for(int i = 0; i < 5; i++){
			if(nodeList[i] != null)
				Destroy(nodeList[i]);
			nodeList[i] = Instantiate(drop_node_prefab, 
			transform.position + new Vector3(maxX*(i+1)/5, 0, maxZ*(i+1)/5), new Quaternion(0,0,0,0));
		}
		
		//Draw Ray to next node
		
		//if raycast hits anything but next node, recast between those nodes
		// +5, -5, +10, -10 until clear path
		// Set new nodes and test path again  until all true, 
		//then CLEAR_PATH = true; follow path unless interruption, in which case re-trace
		CLEAR_PATH = ConfirmClearPath();
	}
	
	private bool ConfirmClearPath(){
		RaycastHit hit;
		Ray ray;
		
		for(int i = 1; i < 5; i++){
			Vector3 direction = nodeList[i].transform.position - nodeList[i-1].transform.position;
			ray = new Ray(nodeList[i-1].transform.position, direction);
			Debug.DrawRay(nodeList[i-1].transform.position, direction, Color.blue, 12f);
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider != null && !hit.collider.gameObject.name.Contains("Floor")){
					Debug.Log("INVALID PATH: " + hit.collider.gameObject.name);
					return false;
				}
			}
		}
		
		return true;
	}

	private void FollowNodes(){
		cNode = nodeList[0];
		Vector3 directionOfTravel = cNode.transform.position - this.transform.position;
        directionOfTravel.Normalize();
		
		transform.Translate(
			(directionOfTravel.x * walkSpeed * Time.deltaTime),
			(0f),
			(directionOfTravel.z * walkSpeed * Time.deltaTime),
			Space.World);
		
		if(Mathf.Abs(transform.position.x - cNode.transform.position.x) < 1f && 
			Mathf.Abs(transform.position.z - cNode.transform.position.z) < 1f)
		{			
			PlaceNavDrops();
		}
		
		if(Mathf.Abs(transform.position.x - currentActiveOBJ.transform.position.x) < currentActiveOBJ.transform.localScale.x/3 && 
			Mathf.Abs(transform.position.z - currentActiveOBJ.transform.position.z) < currentActiveOBJ.transform.localScale.z/3)
		{			
			MoveToObjective = false;
		}
	}

}