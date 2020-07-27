using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public bool PausedState = false, PlayState = false, MenuState = false, PlayerDeathState = false;
	public Transform RespawnLocation;
	public GameObject winScreen, loseScreen, spawnScreen;
	public GameObject P;

	private List<Transform> obsToDestroy = new List<Transform>();
	public GameObject LocalPlayerPrefab;
	public Transform playerCam;
	public float respawnTime = 5f;

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
		playerCam = playerBod.GetComponentInChildren<Camera>().gameObject.transform;
		playerCam.SetParent(null);

		//Seperate all children from playerbod, then destroy player empty object so pieces fall and player input is killed
		Transform[] childObs = playerBod.GetComponentsInChildren<Transform>();
		foreach(Transform cube in childObs){
			if (!cube.gameObject.name.Contains(playerBod.gameObject.name)) {
				cube.SetParent(null);
				cube.GetComponent<BoxCollider>().enabled = true;
				cube.gameObject.AddComponent<Rigidbody>().useGravity = true;
				cube.GetComponent<Rigidbody>().AddExplosionForce(200, playerBod.position+Vector3.forward, 5);
				obsToDestroy.Add(cube);
			}
		}

		//Send camera to respawn location
		playerCam.GetComponent<FloatToTransform>().StartFloatRoutine(playerBod, RespawnLocation);
		StartCoroutine(RespawnPlayer(playerBod));
	}

	public IEnumerator RespawnPlayer(Transform oldBod)
	{
		yield return new WaitForSeconds(3f);

		Destroy(oldBod.gameObject);

		StartCoroutine(IESpawnPlayer());

		foreach (Transform cube in obsToDestroy) {
			Destroy(cube.gameObject);
			yield return new WaitForSeconds(0.2f);
		}

		PlayerDeathState = false;
	}

	public void SpawnPlayer(){
		StartCoroutine(IESpawnPlayer());
		spawnScreen.SetActive(false);
	}
	private IEnumerator IESpawnPlayer()
	{
#pragma warning disable
		GameObject newP = Instantiate(LocalPlayerPrefab, RespawnLocation.position, new Quaternion(0, 0, 0, 0), null);
#pragma warning restore
		playerCam.SetParent(newP.transform);
		playerCam.GetComponent<FloatToTransform>().FloatToBase();
		P = newP;
		newP.GetComponent<ManualPhysics>().enabled = false;

		newP.GetComponent<Move2>().enabled = false;
		newP.GetComponent<ModelToWireframe>().ToWireframe();

		Transform[] childObs = newP.GetComponentsInChildren<Transform>();
		foreach (Transform cube in childObs) {
			if(!cube.name.Contains("Camera") && !cube.name.Contains("Player"))
				cube.gameObject.SetActive(false);
		}

		foreach (Transform cube in childObs) {
			yield return new WaitForSeconds(respawnTime / childObs.Length);
			cube.gameObject.SetActive(true);
		}

		newP.GetComponent<MechStats>().FindMyThings();
		newP.GetComponent<MechGunA>().FindMyThings();
		newP.GetComponent<Move2>().FindMyThings();

		newP.GetComponent<ManualPhysics>().enabled = true;
		newP.GetComponent<Move2>().enabled = true;
		PlayState = true;
	}
}
