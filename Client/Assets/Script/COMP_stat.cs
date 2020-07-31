using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMP_stat : MonoBehaviour
{
    public int HP = 1;
	public GameObject deathExplosion;
	
	public void Update(){
		if(HP <= 0)	{
			StartCoroutine("Die");	
		}
	}
	
	private IEnumerator Die(){
		this.GetComponent<DummyMove>().enabled = false;
		GameObject go = Instantiate(deathExplosion, this.transform.position, new Quaternion(0,0,0,0), this.transform);
		go.transform.localScale *= .1f;
		yield return new WaitForSeconds(1f);
		Destroy(go);
		Destroy(this.gameObject);
	}
}
