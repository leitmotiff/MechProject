using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobaHeart : MonoBehaviour
{
	private StateManager SM;

	public int HP = 1;
	public GameObject deathExplosion;

	void Start()
    {
		//heart = transform.GetChild(0).gameObject;
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
        StartCoroutine(LoseCondition());
    }

	private IEnumerator Die() {
		int s = 16;
		//Instantiate(deathExplosion, this.transform.position + Vector3.up*s, new Quaternion(0, 0, 0, 0), this.transform);
		//Instantiate(deathExplosion, this.transform.position + Vector3.down*s, new Quaternion(0, 0, 0, 0), this.transform);
		//Instantiate(deathExplosion, this.transform.position + Vector3.left*s, new Quaternion(0, 0, 0, 0), this.transform);
		//Instantiate(deathExplosion, this.transform.position + Vector3.right*s, new Quaternion(0, 0, 0, 0), this.transform);
		//Instantiate(deathExplosion, this.transform.position + Vector3.forward*s, new Quaternion(0, 0, 0, 0), this.transform);
		//Instantiate(deathExplosion, this.transform.position + Vector3.back*s, new Quaternion(0, 0, 0, 0), this.transform);
		var g = Instantiate(deathExplosion, this.transform.position, new Quaternion(0, 0, 0, 0), this.transform);
		g.transform.localScale *= s;
		//go.transform.localScale *= .1f;
		yield return new WaitForSeconds(1f);
		//Destroy(go);
		//Destroy(this.gameObject);
	}

	private IEnumerator LoseCondition()
    {
        while(true){
			yield return new WaitForSeconds(1f);
			if (HP <= 0) {
				SM.WinCondition(gameObject.tag);
				StartCoroutine("Die");
				break;
			}
		}
    }
}
