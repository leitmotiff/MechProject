﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move2 : MonoBehaviour
{
	#region Old Var
	public bool isPlayer = false, canMove = true, canWallRide = false, canJump = true, canFlight = false,
		isTouchingWall = false, isRidingWall = false;
	public bool invertYAxis = false, isGrounded = false;
	public float InputFactor = 1000f;
    private Vector3 inputVelocity;
	private Vector3 worldVelocity;
    public float walkSpeed = 2f, sprintSpeed = 5f, airSpeed = 2f, jumpForce = 50f;
	
    private float rotateInputFactor = 100f, rotationSpeed = 100f;
	private float maxRotationSpeed = 40f;
	public float mSpeed = 0;
	private float rxSpeed = 0, rySpeed = 0;
	private bool checkYStatus = true;
	
	private Rigidbody rb;
	private Transform targetCam;
	private Camera playerCam;
	private Animator anim;
	private StateManager SM;
	#endregion
	
	public float JumpRegen = 1, SprRegen = 1, SprUsage = 0.1f;
	private bool sparking = false;
	private float maxX, maxY, tempJ = 100, tempS = 100;
	public RectTransform JumpRect, SprintRect;
	private Ray groundRay;
	private Collision ActiveWall;
	private ManualPhysics MP;

	private Ray WallRay;
	private Vector3 camBasePos, camLerpPos;

	void Awake(){
		groundRay = new Ray(transform.position, Vector3.down);
		MP = GetComponent<ManualPhysics>();
		rb = GetComponent<Rigidbody>();
		playerCam = GetComponentInChildren<Camera>();
		anim = GetComponent<Animator>();
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
	}
    void Start() {
		//StartCoroutine(UpdateYStatus());
		camBasePos = new Vector3( 0, 2, -10);
		maxX = JumpRect.sizeDelta.x;
		maxY = JumpRect.sizeDelta.y;

		StartCoroutine(CatchWhenFall());
	}
    void Update() 
	{
		if (isPlayer && SM.PlayState) 
		{
			PhysicsMove();
			CamMove();
		}
	}
    private void PhysicsMove() {
		//	Shift
		mSpeed = isGrounded&&(tempS > 0) ? (Input.GetButton("Fire3") ? sprintSpeed : walkSpeed) : walkSpeed;
		MP.mSpeed = mSpeed;

		if (Input.GetButton("Fire3")) {
			if(tempS > 0)
				tempS -= SprUsage;
		}
		else {
			if (tempS < 99f)
				tempS += SprRegen;
		}
		SprintRect.sizeDelta = Vector2.Lerp(SprintRect.sizeDelta, new Vector2(maxX * tempS / 100, maxY), Time.deltaTime * 10);

		//	WASD
		Vector3 localInput = new Vector3();
		if(canMove){ 
			localInput = Vector3.ClampMagnitude(transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))), 1f);
			inputVelocity = Vector3.MoveTowards(inputVelocity, localInput, Time.deltaTime * 5f);
		}
		bool[] onkey = { Input.GetKey(KeyCode.W) , Input.GetKey(KeyCode.S) , Input.GetKey(KeyCode.A) , Input.GetKey(KeyCode.D) };
		if (onkey.ToList<bool>().Contains(true)) {
			MP.ApplyForce(mSpeed * InputFactor, inputVelocity);
		}
		
		//	animation
		if(Mathf.Abs(localInput.x) > 0.5f || Mathf.Abs(localInput.z) > 0.5f) {anim.SetBool("isMoving", true);}
		else {anim.SetBool("isMoving", false);}
		
		//	JUMP
		if (Input.GetKeyDown(KeyCode.Space)
			&& canJump)
		{
			canJump = false;
			StartCoroutine(JustJumped());
			MP.ApplyForce(jumpForce * InputFactor * 10, Vector3.up);
		}
		JumpRect.sizeDelta = Vector2.Lerp(JumpRect.sizeDelta, new Vector2(maxX * tempJ / 100, maxY), Time.deltaTime*50);

		//	WallRide
		if (canWallRide && Input.GetButton("Fire3")
			&& !isGrounded
			&& tempS > 0
			&& isTouchingWall)
		{
			isRidingWall = true;
			MP.conH = true;
			MP.freezeV = true;
			if (!sparking) {
				StartCoroutine(WallRideSpark());
				sparking = true;
			}
		}
		else{
			MP.freezeV = false;
			sparking = false;
			camLerpPos = camBasePos;
		}

		//	Flight
		if (canFlight && Input.GetKeyDown(KeyCode.Space)
			&& !isGrounded
			&& !isTouchingWall) 
		{
			
		}
	}
	private void CamMove()
	{
		//	Rotation
		float rx = Input.mousePosition.x, ry = Input.mousePosition.y;
		float rxnorm = 0f, rynorm = 0f;
		float xbuff = 0.7f, ybuff = 0.7f;
		if (rx > Screen.width * xbuff)
			rxnorm = rx / Screen.width;
		if (rx < Screen.width * (1 - xbuff))
			rxnorm = (Screen.width * (1 - xbuff) - rx) / (Screen.width * xbuff) * -1;

		if (ry > Screen.height * ybuff)
			rynorm = ry / Screen.height;
		if (ry < Screen.height * (1 - ybuff))
			rynorm = (Screen.height * (1 - ybuff) - ry) / (Screen.height * ybuff) * -1;
		if (!invertYAxis)
			rynorm *= -1;

		rxSpeed = Mathf.MoveTowards(rxSpeed, rxnorm * rotationSpeed, rotateInputFactor * Time.deltaTime);
		rySpeed = Mathf.MoveTowards(rySpeed, rynorm * rotationSpeed, rotateInputFactor * Time.deltaTime);
		if (rxSpeed > maxRotationSpeed)
			rxSpeed = maxRotationSpeed;
		if (rySpeed > maxRotationSpeed)
			rySpeed = maxRotationSpeed;
		transform.Rotate(0f, rxSpeed * Time.deltaTime, 0f);

		playerCam.transform.Rotate(rySpeed * Time.deltaTime, 0f, 0f);

		if (playerCam.transform.eulerAngles.x > 20f && playerCam.transform.eulerAngles.x < 30f)
			playerCam.transform.eulerAngles = new Vector3(19.8f, playerCam.transform.eulerAngles.y, 0);
		if (playerCam.transform.eulerAngles.x < 340f && playerCam.transform.eulerAngles.x > 30f)
			playerCam.transform.eulerAngles = new Vector3(340.2f, playerCam.transform.eulerAngles.y, 0);

		Vector3 camTo = camBasePos;
		if (isRidingWall) {
			try {
				camTo += new Vector3(ActiveWall.GetContact(0).normal.x, 0, ActiveWall.GetContact(0).normal.z) * 10;
			}
			catch{

			}
		}
		playerCam.transform.localPosition = Vector3.Lerp(playerCam.transform.localPosition, 
			camTo,
			Time.deltaTime);
	}

	private IEnumerator JustJumped() {
		tempJ = 0;
		while (tempJ < 100) {
			yield return new WaitForSeconds(0.02f);
			tempJ += JumpRegen;
		}
		canJump = true;
	}
	private IEnumerator WallRideSpark()
	{
		GameObject spark = Instantiate(Resources.Load("Effect/Explosion9") as GameObject, transform);
		spark.transform.localPosition += Vector3.down * 2;

		while(tempS > 0 && Input.GetButton("Fire3"))
		{
			yield return new WaitForSeconds(0.1f);
		}
		isRidingWall = false;
		Destroy(spark);
	}
	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal*10, Color.white, 2f);
		}

		if (collision.gameObject.name.Contains("Wall")) {
			isTouchingWall = true;
			ActiveWall = collision;
		}
		if (collision.gameObject.name.Contains("Floor")){
			isGrounded = true;
			MP.conH = false;
		}
	}
	void OnCollisionExit(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}

		if (collision.gameObject.name.Contains("Wall")) {
			isTouchingWall = false;
		}
		if (collision.gameObject.name.Contains("Floor")) {
			isGrounded = false;
			MP.conH = true;
		}
	}

	private IEnumerator CatchWhenFall()
	{
		int posFixCount = 0;
		for (; ; ) {
			yield return new WaitForSeconds(1f);
			if(posFixCount > 3){
				//Force player respawn

				posFixCount = 0;
			}
			if(transform.position.y < 3){
				transform.position = new Vector3(transform.position.x, 4, transform.position.z);
				posFixCount++;
			}
		}
	}
}