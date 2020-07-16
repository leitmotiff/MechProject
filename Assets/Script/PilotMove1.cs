using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotMove1 : MonoBehaviour
{
    public bool isPlayer = false;
    public float moveInputFactor = 5f;
    public Vector3 inputVelocity;
    public Vector3 worldVelocity;
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotateInputFactor = 20f;
    public float rotationSpeed = 100f;
    public float averageRotationRadius = 10f;
    private float mSpeed = 0;
    private float rSpeed = 0;
	
	public Animator anim;

	private StateManager SM;
	
	void Awake(){
		anim = GetComponent<Animator>();
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
	}

    void Start() {
    }

    void Update() {		
        if (isPlayer && SM.PlayState) Move();
    }

    private void Move() {
        mSpeed = (Input.GetButton("Fire3") ? sprintSpeed : walkSpeed);
        Vector3 localInput = Vector3.ClampMagnitude(transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))), 1f);
        if(Mathf.Abs(localInput.x) > 0.5f || Mathf.Abs(localInput.z) > 0.5f) {anim.SetBool("isMoving", true);}
		else {anim.SetBool("isMoving", false);}
		inputVelocity = Vector3.MoveTowards(inputVelocity, localInput, Time.deltaTime * moveInputFactor);
        worldVelocity = inputVelocity * mSpeed;
        rSpeed = Mathf.MoveTowards(rSpeed, Input.GetAxis("Turn") * rotationSpeed, rotateInputFactor * Time.deltaTime);
        transform.Rotate(0f, rSpeed * Time.deltaTime, 0f);

        transform.position += (worldVelocity * Time.deltaTime);
    }
}
