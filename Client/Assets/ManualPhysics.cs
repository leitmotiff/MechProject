using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ManualPhysics : MonoBehaviour
{
    // Keep Rigidbody for collisions but turn off mass/gravity to be simulated here
    private Rigidbody rb;
    private Transform tr;
    public float gravityF = -9.8f, gravMod = 1f;

    public float mass, mSpeed;
    public Vector3 baseAcceleration, acceleration, velocity, deltaD;
    private Vector3 refP1, refP2;

    [HideInInspector]
    public bool conA = false, conV = false, conH = false, freezeV = false;

    void Awake(){
        tr = transform;
        rb = GetComponent<Rigidbody>();
	}

    void Start()
    {
        InitializeReferences();

        baseAcceleration = new Vector3(0, gravMod * gravityF, 0);
        acceleration = Vector3.zero;
    }
    void FixedUpdate()
    {
        refP1 = transform.position;

        //rb.AddForce(new Vector3(0, gravMod*gravityF,0), ForceMode.Acceleration);
        //acceleration = new Vector3(Mathf.Clamp(acceleration.x, -mSpeed, mSpeed),acceleration.y, Mathf.Clamp(acceleration.z, -mSpeed, mSpeed));
        Simulate(baseAcceleration + acceleration);

        // Eventually apply floor friction, for now is a constant
        Vector3 friction;
        float fx = -0.4f * acceleration.x, 
            fy = -0.1f * acceleration.y, 
            fz = -0.4f * acceleration.z;
        if (conH)
            fx = fz = 0;
        if (conV)
            fy = 0;
        
        friction = new Vector3(fx, fy, fz) + (velocity.y < 0 ? Vector3.down : Vector3.zero);
        
        acceleration += friction;

        refP2 = transform.position;
    }

    // Translate based on current acceleration
    private void Simulate(Vector3 a)
    {
        float x = Mathf.Clamp(a.x, -mSpeed, mSpeed);
        float y = a.y;
        float z = Mathf.Clamp(a.z, -mSpeed, mSpeed);
        a = new Vector3(x, y, z);

        velocity = a * .2f;
        deltaD = velocity * .2f;

        if (freezeV)
            deltaD.y = 0;

        tr.position = Vector3.Lerp(tr.position, tr.position+deltaD, Time.deltaTime*25);
	}

    public void ApplyForce(float N, Vector3 direction){
        acceleration += (N / mass) * direction;
	}
    public void ApplyConstantForce(float N, Vector3 direction) {
        conA = true;


    }

    private void InitializeReferences()
    {
        refP1 = transform.position;
        refP2 = transform.position;
    }
}
