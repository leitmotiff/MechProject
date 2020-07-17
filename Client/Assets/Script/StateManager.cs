using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public bool PausedState = false, PlayState = false, MenuState = false;
	public GameObject winScreen, loseScreen;
	public List<GameObject> PlayerList;

	public void WinCondition(string LoseTeam){
		Debug.Log("GameOver!");
		//PausedState = true;
		//PlayState = false;
		foreach(GameObject P in PlayerList){
			if(P.tag == LoseTeam)
				Instantiate(loseScreen, P.transform.GetChild(0).transform.forward, P.transform.localRotation, P.transform.GetChild(0).transform);
			else
				Instantiate(winScreen, P.transform.GetChild(0).transform.forward, P.transform.localRotation, P.transform.GetChild(0).transform);
		}
	}
}
