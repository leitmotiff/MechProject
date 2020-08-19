using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move3 : MonoBehaviour
{
	#region Body
	Rigidbody rb;
	Camera playerCam;
	public float speed, jumpForce = 20;
	Vector3 inputVelocity, localInput;
	[HideInInspector]
	public bool canMove = true;
	private bool jb = false;

	private Animator anim;
	#endregion

	#region Walls
	bool isTouchingWall = false, isRidingWall = false;
	private Collision ActiveWall;
	#endregion

	#region Cam 
	private float rotateInputFactor = 100f, rotationSpeed = 100f;
	private float maxRotationSpeed = 60f;
	private float rxSpeed = 0, rySpeed = 0;
	public bool invertYAxis = false;
	private Vector3 camBasePos = new Vector3(0, 2, -10), camLerpPos;
	#endregion

	#region HUD
	public RectTransform JumpRect, SprintRect;
	public float JumpRegen = 1, SprRegen = 1, SprUsage = 0.1f;
	private bool sparking = false;
	private float maxX, maxY, tempJ = 100, tempS = 100;
	#endregion

	#region Misc
	private StateManager SM;
	private StatTracker ST;
	#endregion

	void Start()
	{
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
		ST = GameObject.Find("EventSystem").GetComponent<StatTracker>();

		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();

		JumpRect = GameObject.Find("JumpBar").GetComponent<RectTransform>();
		SprintRect = GameObject.Find("SprintBar").GetComponent<RectTransform>();

		camBasePos = new Vector3(0, 2, -10);
		maxX = JumpRect.sizeDelta.x;
		maxY = JumpRect.sizeDelta.y;

		if (playerCam == null)
		{
			playerCam = Camera.main;
			playerCam.transform.SetParent(this.transform);
			playerCam.GetComponent<FloatToTransform>().FloatToBase();
		}
	}

	// Update will handle (most) player inputs, FixedUpdate will handle the rigidbody physics, as this should be more compatable for each Update type
	private void Update() {
		if (SM.PlayState) {
			//	CAM
			CamMove();

			//	ANIM
			if (Mathf.Abs(localInput.x) > 0.5f || Mathf.Abs(localInput.y) > 0.5f) { anim.SetBool("isMoving", true); }
			else { anim.SetBool("isMoving", false); }

			//	JUMP
			if (Input.GetKeyDown(KeyCode.Space)
				&& tempJ > 80) {
				jb = true;
				StartCoroutine(JustJumped());
			}
		}
		JumpRect.sizeDelta = Vector2.Lerp(JumpRect.sizeDelta, new Vector2(maxX * tempJ / 100, maxY), Time.deltaTime * 50);
	}

	void FixedUpdate()
	{
		if (SM.PlayState) {
			float mH = Input.GetAxis("Horizontal");
			float mV = Input.GetAxis("Vertical");
			if (canMove) {
				localInput = Vector3.ClampMagnitude(transform.TransformDirection(new Vector3(mH, 0, mV)), 1f);
				inputVelocity = Vector3.MoveTowards(inputVelocity, localInput, Time.deltaTime * 5f);
			}
			rb.velocity = new Vector3(inputVelocity.x * speed, rb.velocity.y, inputVelocity.z * speed);

			// JUMP
			if (jb) {
				jb = false;
				rb.velocity += jumpForce * Vector3.up;
			}
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
		if (isRidingWall)
		{
			try
			{
				camTo += new Vector3(ActiveWall.GetContact(0).normal.x, 0, ActiveWall.GetContact(0).normal.z) * 10;
			}
			catch
			{

			}
		}
		playerCam.transform.localPosition = Vector3.Lerp(playerCam.transform.localPosition,
			camTo,
			Time.deltaTime);
	}

	#region Enums
	private IEnumerator JustJumped()
	{
		tempJ = 0;
		while (tempJ < 100)
		{
			yield return new WaitForSeconds(0.02f);
			tempJ += JumpRegen;
		}
	}
	private IEnumerator WallRideSpark()
	{
		GameObject spark = Instantiate(Resources.Load("Effect/Explosion9") as GameObject, transform);
		spark.transform.localPosition += Vector3.down * 2;

		while (tempS > 0 && Input.GetButton("Fire3"))
		{
			yield return new WaitForSeconds(0.1f);
			if (Input.GetButtonUp("Fire3"))
				break;
		}
		isRidingWall = false;
		Destroy(spark);
	}
	#endregion

}
