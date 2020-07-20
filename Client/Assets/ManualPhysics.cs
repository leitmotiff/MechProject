using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualPhysics : MonoBehaviour
{
    private Rigidbody rb;
    public float ObjectMass = 1f, gravityF = 9.8f, gravMod = 1f;

    public float mass, acceleration, velocity;
    private Vector3 refP1, refP2;

    void Awake(){
        rb = GetComponent<Rigidbody>();
	}

    void Start()
    {
        InitializeReferences();
    }
    void FixedUpdate()
    {
        refP1 = transform.position;
        //rb.AddForce(new Vector3(0, gravMod*-1*gravityF,0), ForceMode.Acceleration);


        refP2 = transform.position;
    }

    public void ApplyForce(float N){

	}

    private void InitializeReferences()
    {
        refP1 = transform.position;
        refP2 = transform.position;
    }
}
