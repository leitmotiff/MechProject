using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarPilot : MonoBehaviour
{
    #region
	//public Transform frontFacing;
	//[HideInInspector]
	public Vector3 lookAngle, pos, movement;
	
	private Quaternion angleTo;
	
	public Transform tr, model;
	
	private Rigidbody rb;
	
	[HideInInspector]
	public bool isPaused = false, canMove = true;
	private float newy;//hLock = 0f, vLock = 0f,
	public int speed = 12;
		
	//private GameObject model, EventSystem;
	//public GameObject NearestVillager, BackPackUI;
	//private VillagerScript VS;
	private IEnumerator coroutine;
	
	//private Animator m_Animator, model_anim;
	
	#endregion
	
    void Start() {
		GetAllComponents();
		
	}
	
	public void GetAllComponents(){
		tr = transform;
        pos = tr.position;
		
		//rb = GetComponent<Rigidbody2D>();
		//AC = GetComponent<AudioController>();
		//myCOL = GetComponent<CircleCollider2D>();
		//model = GameObject.Find("1_Model");
		//model_anim = model.GetComponent<Animator>();
		
		//HUD = GameObject.Find("HUD");
		//PAUSE_menu = GameObject.Find("PAUSE_menu");
		//EventSystem = GameObject.Find("EventSystem");
		//aimRing = GameObject.Find("AimRing").transform;
		//UIDS = this.transform.GetChild(0).GetChild(0).GetComponent<UI_DisplayStats>();
		
		//centerScreen = new Vector2(Screen.width/2, Screen.height/2);
    }

    void Update() {
		//if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))//start button
			//PauseMenuM();
		
		if(!isPaused){
			Controls1();
			//PadControls1();
		}
		
		//model_anim.SetBool("isMove", isMoving);
		ProcessInputs();
        Move();

    }
	
	#region Player Trigger
	void OnTriggerEnter(Collider col){
		
	}
	
	void OnTriggerExit(Collider col){
		
	}
	#endregion
	
	#region Control Schemes
	void Controls1(){
		
		if(Input.GetKeyDown(KeyCode.T)){
			//RaycastHit hit;
			
			/*if(Physics.Raycast(transform.position - Vector3.up, lookAngle, out hit)){
				//Debug.Log(hit.collider.gameObject.name);
				//Talks to Villager
				if(hit.collider.gameObject.tag == "Villager"){
					VS = hit.collider.gameObject.GetComponent<VillagerScript>();
					VS.focus = this.transform.GetChild(0).transform;
					canMove = VS.TalkFunc();
				}
				
				//Picks up item
				if(hit.collider.gameObject.tag == "PickUpItem"){
					StartCoroutine(MomentaryPause());
					AC.PlaySFX("pop1");
					hit.collider.gameObject.transform.position = transform.position +
						new Vector3(0f,1f,0f);
					hit.collider.gameObject.transform.SetParent(transform);
					hit.collider.gameObject.GetComponent<ItemPickup>().enabled = false;
					StartCoroutine(WaitNDestroy(0.8f, hit.collider.gameObject));
				}
				
				//Start Transition to new scene
				if(hit.collider.gameObject.tag == "Door"){
					Debug.Log("Door Transition!");
					AC.PlaySFX("door1");
					canMove = false;
					DoorToRoom d = hit.collider.gameObject.GetComponent<DoorToRoom>();
					
					d.Transport(transform);
					
				}
			}*/
		}
		
		#region Debug Controls
		
		#endregion
	}
	void PadControls1(){
		//lookAngle = new Vector3(Input.GetAxis("RightStickX"), -1 * Input.GetAxis("RightStickY"), 0.0f);
	}
	#endregion

	#region Move Functions
	
	private void ProcessInputs(){
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		
		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
			y = 0;
		if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
			x = 0;
		
		movement = new Vector3(x, 0f, y);
		//lookAngle = frontFacing.position;

		if(canMove)
			RotateModel(x, y);
		
		movement.Normalize();
		
		/*if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
			isMoving = true;
		else
			isMoving = false;*/
	}
	
    private void Move(){
		if(canMove)
			tr.Translate(movement*speed*Time.deltaTime);
    }
	
	private void RotateModel(float x, float y){
		if(x != 0 || y != 0)
			newy = Mathf.Atan2(-y, x) * Mathf.Rad2Deg;
		
		//model.rotation = Quaternion.Euler(0f, newy, 0f);
	}
	public float RSAngle(float x, float y){
		y *= -1;
		if(x > 0){
			if(y > 0){
				return Mathf.Atan2(y, x) * Mathf.Rad2Deg ;
			}
			if(y < 0){
				return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
			}
		}
		if(x < 0){
			if(y > 0f){
				return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
			}
			if(y < 0){
				return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
			}
		}
		return 0;
	}
	public float RSAngle(Vector3 A){
		if(A.x > 0){
			if(A.y > 0){
				return Mathf.Atan2(A.y, A.x) * Mathf.Rad2Deg - 90;
			}
			if(A.y < 0){
				return Mathf.Atan2(A.y, A.x) * Mathf.Rad2Deg - 90;
			}
		}
		if(A.x < 0){
			if(A.y > 0f){
				return Mathf.Atan2(A.y, A.x) * Mathf.Rad2Deg - 90;
			}
			if(A.y < 0){
				return Mathf.Atan2(A.y, A.x) * Mathf.Rad2Deg - 90;
			}
		}
		return 0;
	}

	#endregion
	
	private IEnumerator WaitNDestroy(float time, GameObject go){
		yield return new WaitForSeconds(time);
		Destroy(go);
	}
	
	private IEnumerator MomentaryPause(){
		canMove = false;
		yield return new WaitForSeconds(0.5f);
		canMove = true;
	}
	/*#region Old/Test Functions
    public int DieCalc(string Strin){
        int Val = 0, i = 0;
        int[] a = new int[10];
        System.Random rand = new System.Random();

        while (i < Strin.Length)
        {
            if (Strin[i] == 'd')
            {
                for (int x = 1; x <= (Strin[i - 1] - '0'); x++)
                    Val += rand.Next(1, (Strin[i + 1] - '0') + 1);
            }

            if (Strin[i] == '+' && (Strin[i + 2] == '+' || Strin[i + 2] == '\0'))
            {
                Val += (Strin[i + 1] - '0');
            }

            i++;
        }

        return Val;
    }

	private void TestGamePadInputs(){
		if(Input.GetKeyDown(KeyCode.JoystickButton0))
			Debug.Log(0); // A
		if(Input.GetKeyDown(KeyCode.JoystickButton1))
			Debug.Log(1); // B
		if(Input.GetKeyDown(KeyCode.JoystickButton2))
			Debug.Log(2); // X
		if(Input.GetKeyDown(KeyCode.JoystickButton3))
			Debug.Log(3); // Y
		if(Input.GetKeyDown(KeyCode.JoystickButton4))
			Debug.Log(4); // L shoulder
		if(Input.GetKeyDown(KeyCode.JoystickButton5))
			Debug.Log(5); // R shoulder
		if(Input.GetKeyDown(KeyCode.JoystickButton6))
			Debug.Log(6); // Select
		if(Input.GetKeyDown(KeyCode.JoystickButton7))
			Debug.Log(7); // Start
		if(Input.GetKeyDown(KeyCode.JoystickButton8))
			Debug.Log(8); // L stick click
		if(Input.GetKeyDown(KeyCode.JoystickButton9))
			Debug.Log(9); // R stick click
		if(Input.GetKeyDown(KeyCode.JoystickButton10))
			Debug.Log(10); // 
		if(Input.GetKeyDown(KeyCode.JoystickButton11))
			Debug.Log(11); // 
		if(Input.GetKeyDown(KeyCode.JoystickButton12))
			Debug.Log(12); // 
		if(Input.GetKeyDown(KeyCode.JoystickButton13))
			Debug.Log(13); // 
		if(Input.GetKeyDown(KeyCode.JoystickButton14))
			Debug.Log(14); // 
		if(Input.GetKeyDown(KeyCode.JoystickButton15))
			Debug.Log(15); // 
		
		if(Input.GetAxis("Right Trigger") > 0.2f)
			Debug.Log(Input.GetAxis("Right Trigger"));
		if(Input.GetAxis("Left Trigger") > 0.2f)
			Debug.Log(Input.GetAxis("Left Trigger"));
	}
	#endregion
	*/
}
