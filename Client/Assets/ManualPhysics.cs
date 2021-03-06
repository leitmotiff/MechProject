﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ManualPhysics : MonoBehaviour
{
    // Keep Rigidbody for collisions but turn off mass/gravity to be simulated here
    private Rigidbody rb;
    private Transform tr;
    [Range(1, 50)]
    public float UpdateSpeed = 1;

    public float gravityF = -9.8f, gravMod = 1f;

    public float mass, mSpeed;
    public Vector3 baseAcceleration, gravity, acceleration, velocity, deltaD;

    // Check Height
    private Ray ray;
    private RaycastHit hit;

    [HideInInspector]
    public bool conA = false, conV = false, conH = false, freezeV = false;

    void Awake(){
        tr = transform;
        rb = GetComponent<Rigidbody>();
	}
    void Start()
    {
        ray = new Ray(transform.position, Vector3.down);
        baseAcceleration = Vector3.zero;
        gravity = new Vector3(0, gravMod * gravityF, 0);
        acceleration = Vector3.zero;
    }
    void FixedUpdate()
    {
        ray = new Ray(transform.position, Vector3.down * 100);
        Physics.Raycast(ray, out hit);
        //Debug.DrawRay(transform.position, Vector3.down*100, Color.green, 2f);
        //Debug.Log(hit.distance);

        if (hit.distance >= 4)
            baseAcceleration = gravity;
        else
            baseAcceleration = Vector3.zero;

        Simulate(baseAcceleration + acceleration);

        // Eventually apply floor friction, for now is a constant
        Vector3 friction;
        float fx = -0.4f * acceleration.x,
            fy = -0.4f * acceleration.y,
            fz = -0.4f * acceleration.z;
        if (conH)
            fx = fz = 0;
        if (conV)
            fy = 0;
        friction = new Vector3(fx, fy, fz) + (velocity.y < 5 ? Vector3.down*5 : Vector3.zero);

        acceleration += friction;

    }

    // Translate based on current acceleration
    private void Simulate(Vector3 a)
    {
        float x = Mathf.Clamp(a.x, -mSpeed, mSpeed);
        float y = Mathf.Clamp(a.y, -mSpeed, mSpeed);
        float z = Mathf.Clamp(a.z, -mSpeed, mSpeed);
        a = new Vector3(x, y, z);

        velocity = a * .2f;
        deltaD = velocity * .2f;

        if (freezeV)
            deltaD.y = 0;

        //tr.position = Vector3.MoveTowards(tr.position, tr.position + deltaD, 1f);
        tr.position = Vector3.Slerp(tr.position, tr.position+deltaD, Time.deltaTime* UpdateSpeed);
    }

    public void ApplyForce(float N, Vector3 direction){
        acceleration += (N / mass) * direction;
	}
}
