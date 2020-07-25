using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public bool PausedState = false, PlayState = false, MenuState = false, PlayerDeathState = false;
	public Transform RespawnLocation;
	public GameObject winScreen, loseScreen;
	public GameObject P;

	private List<Transform> obsToDestroy = new List<Transform>();

	public void WinCondition(string LoseTeam){
		Debug.Log("GameOver!");
		
		// Can be redone by placing over HUD
		if(P.tag == LoseTeam)
			Instantiate(loseScreen, P.transform.GetChild(0).transform.forward, P.transform.localRotation, P.transform.GetChild(0).transform);
		else
			Instantiate(winScreen, P.transform.GetChild(0).transform.forward, P.transform.localRotation, P.transform.GetChild(0).transform);
	}

	public void PlayerKill(Transform playerBod = null){
		if (playerBod == null)
			playerBod = P.transform;

		PlayState = false;
		PlayerDeathState = true;
		Transform playerCam = playerBod.GetComponentInChildren<Camera>().gameObject.transform;
		playerCam.SetParent(null);

		//Seperate all children from playerbod, then destroy player empty object so pieces fall and player input is killed
		Transform[] childObs = playerBod.GetComponentsInChildren<Transform>();
		foreach(Transform cube in childObs){
			if (!cube.gameObject.name.Contains(playerBod.gameObject.name)) {
				cube.SetParent(null);
				cube.GetComponent<BoxCollider>().enabled = true;
				cube.gameObject.AddComponent<Rigidbody>().useGravity = true;
				cube.GetComponent<Rigidbody>().AddExplosionForce(200, playerBod.position+Vector3.forward, 5);
				//cube.GetComponent<Rigidbody>().AddTorque(Vector3.back * 100);
				obsToDestroy.Add(cube);
			}
		}
		Destroy(playerBod.gameObject);

		//Send camera to respawn location
		playerCam.GetComponent<FloatToTransform>().StartFloatRoutine(playerBod, RespawnLocation);

	}
}
