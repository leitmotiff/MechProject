using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatRotate : MonoBehaviour
{
    public float RotationSpeed = 200f;
	private void Start() {
	}
	void Update()
    {
        transform.Rotate(new Vector3(0,RotationSpeed * Time.deltaTime,0));
    }
}
