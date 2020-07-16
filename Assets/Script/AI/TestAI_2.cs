using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI_2 : MonoBehaviour
{
	public bool testTrig = false;
	public List<GameObject> OBJ_Zone_List;
	private GameObject currentActiveOBJ;
	private Vector3 cNode;
	private int cNodeIndex = 0;
	
	//public GameObject drop_node_prefab;
	public int nodeListSize = 5;
	private Vector3[] nodeList;
	
	public float walkSpeed = 10;
	private float maxX, maxZ;
	
	public bool ActiveMoving = false, MoveToObjective = false;
	private bool insideObj = false;
	private System.Random rand = new System.Random();
	private RaycastHit hit;
	private Ray ray;
    
	void Awake(){
		nodeList = new Vector3[nodeListSize];
	}
	
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
		int badNodeIndx = 0;
		maxX = mainObj.position.x - transform.position.x;
		maxZ = mainObj.position.z - transform.position.z;
		
		for(int i = badNodeIndx; i < nodeListSize; i++){
			if(nodeList[i] == null)
				nodeList[i] = new Vector3();
			nodeList[i] = transform.position + new Vector3(maxX*(i+1)/5, 0, maxZ*(i+1)/5);
		}
		badNodeIndx = ConfirmClearPath();
		if(badNodeIndx < 0)
			CLEAR_PATH = true;
		
		while(!CLEAR_PATH){
			int q = 0, p = -1, attempt = -1;
			Vector3 direction = nodeList[badNodeIndx+1] - nodeList[badNodeIndx];
			Vector3 dNorm = Vector3.Cross(direction, new Vector3(0,1,0));
			Debug.DrawRay(nodeList[badNodeIndx], dNorm, Color.green, 1f);
			Vector3 newNode;
			do{
				attempt++;
				if(attempt%4 == 0)
					q++;
				if(attempt%4 == 1 || attempt%4 == 2)
					newNode = direction+(dNorm*q*p);
				else{
					newNode = (dNorm*q*p);
					p*=-1;
				}
				ray = new Ray(nodeList[badNodeIndx], newNode);
				Debug.DrawRay(nodeList[badNodeIndx], newNode, Color.red, 12f);
				Physics.Raycast(ray, out hit);
			}
			while(hit.collider != null && q < 5 && !hit.collider.gameObject.name.Contains("Floor"));
			nodeList[badNodeIndx+1] = nodeList[badNodeIndx] + newNode;
			
			badNodeIndx = ConfirmClearPath();
			if(badNodeIndx < 0)
				CLEAR_PATH = true;
		}
	}
	
	private int ConfirmClearPath(){		
		for(int i = 1; i < nodeListSize; i++){
			Vector3 direction = nodeList[i] - nodeList[i-1];
			ray = new Ray(nodeList[i-1], direction);
			Debug.DrawRay(nodeList[i-1], direction/10, Color.blue, 12f);
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider != null && !hit.collider.gameObject.name.Contains("Floor")){
					//Debug.Log("INVALID PATH: " + hit.collider.gameObject.name);
					return i-1;
				}
			}
		}
		return -1;
	}

	private void FollowNodes(){
		try{
			cNode = nodeList[cNodeIndex];
		}
		catch{
			PlaceNavDrops();
			cNodeIndex = 0;
			cNode = nodeList[0];
		}
		Vector3 directionOfTravel = cNode - transform.position;
        directionOfTravel.Normalize();
		
		transform.Translate(
			(directionOfTravel.x * walkSpeed * Time.deltaTime),
			(0f),
			(directionOfTravel.z * walkSpeed * Time.deltaTime),
			Space.World);
		
		if(Mathf.Abs(transform.position.x - cNode.x) < 1f && 
			Mathf.Abs(transform.position.z - cNode.z) < 1f)
		{			
			cNodeIndex++;
		}
		
		if(Mathf.Abs(transform.position.x - currentActiveOBJ.transform.position.x) < currentActiveOBJ.transform.localScale.x/3 && 
			Mathf.Abs(transform.position.z - currentActiveOBJ.transform.position.z) < currentActiveOBJ.transform.localScale.z/3)			
			insideObj = true;
		
		if(insideObj)
			MoveToObjective = false;
		
		if(cNodeIndex >= nodeListSize){
			PlaceNavDrops();
			cNodeIndex = 0;
		}
	}

}