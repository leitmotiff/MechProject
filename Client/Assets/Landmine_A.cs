using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine_A : MonoBehaviour
{
	public int dmg;
	private bool tripped = false; 
	private void OnTriggerEnter(Collider col) {
		if (!tripped && col.gameObject.tag.Contains("Team") && col.gameObject.tag != this.gameObject.tag) {
			tripped = true;
			Debug.Log("Killer Queen! Bites Za : " + col.gameObject.name);
			GetComponent<Rigidbody>().AddExplosionForce(100000, transform.position, 5, 0, ForceMode.Acceleration);
			StartCoroutine(Explode());
			if(col.GetComponent<COMP_stat>()){
				col.GetComponent<COMP_stat>().HP -= dmg;
			}
			if (col.GetComponent<MechStats>()) {
				col.GetComponent<MechStats>().tempHP -= dmg;
			}
		}
	}

	private IEnumerator Explode(){
		var exp = Resources.Load("Effect/Explosion9") as GameObject;
		var e = Instantiate(exp, this.transform.position, new Quaternion(0,0,0,0), this.transform);
		yield return new WaitForSeconds(0.5f);
		Destroy(e);

		Destroy(this.gameObject);

	}
}
