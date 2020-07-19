using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move1 : MonoBehaviour
{
    public bool isPlayer = false, canMove = true;
    public float moveInputFactor = 5f;
    public Vector3 inputVelocity;
    public Vector3 worldVelocity;
    public float walkSpeed = 2f, sprintSpeed = 5f, jumpForce = 50f;
	
    private float rotateInputFactor = 100f, rotationSpeed = 100f;
	public float maxRotationSpeed = 40f;
    public float averageRotationRadius = 10f;
    private float mSpeed = 0, rxSpeed = 0, rySpeed = 0;
	public bool invertYAxis = false, isGrounded = false;
	private bool checkYStatus = true;
	
	private Rigidbody rb;
	private Transform targetCam;
	private Camera playerCam;
	
	public Animator anim;

	private StateManager SM;
	
	void Awake(){
		rb = GetComponent<Rigidbody>();
		playerCam = GetComponentInChildren<Camera>();
		anim = GetComponent<Animator>();
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
	}

    void Start() {
		StartCoroutine(UpdateYStatus());
    }

    void Update() {		
        if (isPlayer && canMove && SM.PlayState) Move();
    }

    private void Move() {
		//	WASD
        mSpeed = (Input.GetButton("Fire3") ? sprintSpeed : walkSpeed);
        
		Vector3 localInput = Vector3.ClampMagnitude(transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))), 1f);
		inputVelocity = Vector3.MoveTowards(inputVelocity, localInput, Time.deltaTime * moveInputFactor);
        worldVelocity = inputVelocity * mSpeed;
		transform.position += (worldVelocity * Time.deltaTime);
		//animation
		if(Mathf.Abs(localInput.x) > 0.5f || Mathf.Abs(localInput.z) > 0.5f) {anim.SetBool("isMoving", true);}
		else {anim.SetBool("isMoving", false);}
		
		//	Rotation
        float rx = Input.mousePosition.x, ry = Input.mousePosition.y;
		float rxnorm = 0f, rynorm = 0f;
		float xbuff = 0.7f, ybuff = 0.7f;
		if (rx > Screen.width* xbuff)
			rxnorm = rx/Screen.width;
		if (rx < Screen.width*(1-xbuff))
			rxnorm = (Screen.width* (1 - xbuff) - rx)/(Screen.width*.7f) * -1;
		
		if (ry > Screen.height* ybuff)
			rynorm = ry/Screen.height;
		if (ry < Screen.height* (1 - ybuff))
			rynorm = (Screen.height* (1 - ybuff) - ry)/(Screen.height*ybuff) * -1;
		if(!invertYAxis)
			rynorm*=-1;
		
		rxSpeed = Mathf.MoveTowards(rxSpeed, rxnorm * rotationSpeed, rotateInputFactor * Time.deltaTime);
        rySpeed = Mathf.MoveTowards(rySpeed, rynorm * rotationSpeed, rotateInputFactor * Time.deltaTime);
		if(rxSpeed > maxRotationSpeed)
			rxSpeed = maxRotationSpeed;
		if(rySpeed > maxRotationSpeed)
			rySpeed = maxRotationSpeed;
		transform.Rotate(0f, rxSpeed * Time.deltaTime, 0f);
		
		playerCam.transform.Rotate(rySpeed * Time.deltaTime, 0f, 0f);
		
		if(playerCam.transform.eulerAngles.x > 20f && playerCam.transform.eulerAngles.x < 30f)
			playerCam.transform.eulerAngles = new Vector3(19.8f,playerCam.transform.eulerAngles.y,0);
		if(playerCam.transform.eulerAngles.x < 340f && playerCam.transform.eulerAngles.x > 30f)
			playerCam.transform.eulerAngles = new Vector3(340.2f,playerCam.transform.eulerAngles.y,0);		
		//Vector3 vec = Vector3.zero;
		//playerCam.transform.eulerAngles = Vector3.SmoothDamp(playerCam.transform.eulerAngles, targetAngles, ref vec, 3f);

		//	JUMP
		if (Input.GetKeyDown(KeyCode.Space)
			&& isGrounded
			&& Mathf.Abs(rb.velocity.y) <= 0.5f)
		{
			isGrounded = false;
			rb.AddForce(Vector3.up * jumpForce * 1000);
		}	
    
	}

	private IEnumerator UpdateYStatus(){
		float y1, y2;
		while(checkYStatus){
			y1 = rb.velocity.y;
			yield return new WaitForSeconds(0.1f);
			y2 = rb.velocity.y;
			
			//if sudden change in accceleration, htis ground
			//if(Mathf.Abs(y1 - y2) > 0.5f)
			if(y2 == 0 && y1 < 0)
				isGrounded = true;
			if(y1 == 0 && y2 == 0)
				isGrounded = true;
		}
	}
}