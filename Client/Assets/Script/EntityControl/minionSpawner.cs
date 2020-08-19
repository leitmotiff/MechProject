using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minionSpawner : MonoBehaviour
{
	public bool canSpawn = true;
	public GameObject MinionPrefab;
	public GameObject PathGO;
	public float tBMinions = 0.5f, tBGroups = 10f;
	private string[] teamColors = {"GreenTeam",
		"BlueTeam", 
		"RedTeam", 
		"OrangeTeam"};
	public string PlayerTeam;
	private Material teamMat;
	
	private Quaternion QuaternionZero = new Quaternion(0,0,0,0);
	
    void Start()
    {
		/*switch(PlayerTeam){
			case "GreenTeam":	
				teamMat = Resources.Load("Materials/gridGreen")as Material;
				break;
			case "RedTeam":	
				teamMat = Resources.Load("Materials/gridRed")as Material;
				break;
			default:
				break;
		}*/
        StartCoroutine(SpawnMinions());
    }

    void Update()
    {
        
    }
	
	private IEnumerator SpawnMinions(){
		GameObject newMin;
		while(canSpawn){
			for(int m = 0; m < 5; m++){
				newMin = Instantiate(MinionPrefab, this.transform.position, QuaternionZero);
				if(PathGO != null)
					newMin.GetComponent<DummyMove>().PathGO = PathGO;
				newMin.GetComponent<DummyMove>().movementType = 3;
				newMin.tag = PlayerTeam;
				//newMin.GetComponent<MeshRenderer>().material = teamMat;
				newMin.SetActive(true);
				yield return new WaitForSeconds(tBMinions);
			}
			yield return new WaitForSeconds(tBGroups);
		}
	}
	
}
