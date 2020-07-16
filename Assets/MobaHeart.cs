using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobaHeart : MonoBehaviour
{
	public GameObject heart;
	private StateManager SM;
	
    void Start()
    {
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
        StartCoroutine(LoseCondition());
    }

    IEnumerator LoseCondition()
    {
        while(true){
			yield return new WaitForSeconds(2f);
			if(heart == null)
				SM.WinCondition(gameObject.tag);
		}
    }
}
