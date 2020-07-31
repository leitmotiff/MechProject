using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
	public bool MinionOnly = false;
    public int intensity;

	private void OnTriggerEnter(Collider collision) {
		if (MinionOnly){
			if (collision.gameObject.name.Contains("Minion")) {
				collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
			}
		}
		else {
			if (collision.gameObject.GetComponent<ManualPhysics>()) {
				collision.gameObject.GetComponent<ManualPhysics>().ApplyForce(intensity * 100, Vector3.up);
			}
			else {
				collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
			}
		}
	}
}
