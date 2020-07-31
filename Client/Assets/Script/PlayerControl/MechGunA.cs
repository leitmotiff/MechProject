using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MechGunA : MonoBehaviour
{
	private string[] teamColors = {"GreenTeam",
		"BlueTeam", 
		"RedTeam", 
		"OrangeTeam"};
	public string PlayerTeam;
	public int GunSelected = 0;
	private float timeD;
	private Camera cam;
	public Transform ArmModel;
	private Transform objectHit;
	private Vector3 aimAngles = Vector3.zero;
	public int hitDmg = 1;
	private float LOFtime = 4, t, ft;
	public float rayFit = -10f;
	private Ray ray;
	
	public GameObject TimerPanel, hackLinkPrefab;
	private TextMeshProUGUI timertxt;
	private StateManager SM;
	private StatTracker ST;

	public GameObject AimPanel, MF_image;

	private bool linkActive = false;
	private GameObject linkObj;
	private RaycastHit hit;

	#region Startup
	private void Awake(){
		
	}
    private void Start(){
		FindMyThings();
	}
	public void FindMyThings(){
		cam = GetComponentInChildren<Camera>();
		TimerPanel = GameObject.Find("HUD_Canvas").transform.GetChild(1).gameObject;
		AimPanel = GameObject.Find("HUD_Canvas").transform.GetChild(0).gameObject;
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
		ST = GameObject.Find("EventSystem").GetComponent<StatTracker>();

		timertxt = TimerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

		PlayerTeam = teamColors[0];
	}
	private void Update(){
		AimArm();
		ArmModel.eulerAngles = aimAngles;

		switch (GunSelected){
			case 1:
				hitDmg = 20;
				Gun_1();
				break;
			
			case 2:
				hitDmg = 3;
				Gun_2();
				break;
			
			default:
				hitDmg = 1;
				Gun_0();
				break;
		}
    }
	#endregion
	
	#region Gun List
	// Tech Attack
	void Gun_2(){
		if (Input.GetButtonDown("Fire1") && SM.PlayState)
			HackLink(true);
		if (Input.GetButtonUp("Fire1") && SM.PlayState) {
			HackLink(false);
		}
	}
	// Charge Shot
	void Gun_1(){
		if(Input.GetButtonDown("Fire1") && SM.PlayState)
			ChargeFireCtrl(true);
		if(Input.GetButtonUp("Fire1") && SM.PlayState){
			ChargeFireCtrl(false);
		}
	}
	// Endless Gat
	void Gun_0(){
		if(Input.GetButtonDown("Fire1") && SM.PlayState)
			GatCtrl(true);
		if(Input.GetButtonUp("Fire1") && SM.PlayState){
			GatCtrl(false);
		}
	}
	#endregion

	#region Gun Control
	private void HackLink(bool go) {
		if (go) {
			StartCoroutine(HackLink());
			//StartCoroutine(CountDownTimer(6f));
		}
		else {
			StopCoroutine(HackLink());
			//StopCoroutine(CountDownTimer(6f));
			//closeTimer();
			linkActive = false;
		}
	}
	private IEnumerator HackLink(){
		
		//Waiit for a few seconds, then spawn blue rope link here
		//Shoot 3 times, then clear
		linkObj = Instantiate(hackLinkPrefab, this.transform.position, new Quaternion(0, 0, 0, 0), this.transform);
		linkObj.SetActive(false);
		StartCoroutine(HackLinkMove());
		yield return new WaitForSeconds(5f);
		//If time has completed and link has not been severed, apply status to enemy

		Destroy(linkObj);
	}
	private IEnumerator HackLinkMove() {
		
		for (int i = 3; i > 0; i--) {
			linkObj.transform.position = transform.position;
			Vector3 startPos = linkObj.transform.position;
			Physics.Raycast(ray, out hit);
			linkObj.SetActive(true);
			float startTime = Time.time;
			while (Time.time < startTime + 1f) {
				linkObj.transform.position = Vector3.Lerp(startPos, hit.point, Time.deltaTime);
				yield return null;
			}
			linkObj.SetActive(false);
		}
		
	}

		private void ChargeFireCtrl(bool go){
		if(go){
			StartCoroutine("ChargeFire");
			StartCoroutine(CountDownTimer(LOFtime));
		}
		else{
			StopCoroutine("ChargeFire");
			StopCoroutine(CountDownTimer(LOFtime));
			closeTimer();
		}
	}
	private IEnumerator ChargeFire(){
		yield return new WaitForSeconds(LOFtime);
		DebugShot();
	}
	
	private void GatCtrl(bool go){
		if(go){
			AimPanel.SetActive(true);
			StartCoroutine("GatGun");
		}
		else{
			AimPanel.SetActive(false);
			StopCoroutine("GatGun");
		}
	}
	private IEnumerator GatGun(){
		DebugAimShot();
		for (int i = 3; i > 0; i--){
			yield return new WaitForSeconds(i*.2f);
			DebugAimShot();
		}
		
		while (true){
			yield return new WaitForSeconds(0.1f);
			DebugAimShot();
		}
	}
	#endregion
	
	#region Debug / Flavor
	private void AimArm(){
		if(Input.GetButton("Fire1") && SM.PlayState){
			RaycastHit hit;
			ray = cam.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(ray, out hit);
			
			//aimAngles = new Vector3(-ray.direction.z, 0, ray.direction.x)*hit.distance 
				//		+ new Vector3(-20, transform.eulerAngles.y,rayFit);
			aimAngles = new Vector3(-90, transform.eulerAngles.y,0);
			ArmModel.localPosition = new Vector3(0.65f,0,1);
		}
		else{
			ArmModel.localPosition = new Vector3(0.65f,-.75f,0);
			aimAngles = transform.eulerAngles;
		}
	}
	private void DebugAimShot(){
		StartCoroutine(MuzzleFlash());
		if (Physics.Raycast(ray, out hit)){
            objectHit = hit.transform;
			Debug.DrawRay(transform.position, ray.direction * hit.distance, Color.blue, 2f);
			
			IEnumerator routine = BulletHitSpark(hit);
			StartCoroutine(routine);
			
			if(teamColors.Contains(hit.collider.tag) && hit.collider.tag != PlayerTeam)
				if(hit.collider.GetComponent<COMP_stat>())
					hit.collider.GetComponent<COMP_stat>().HP -= hitDmg;
		
		}
	}
	private void DebugShot(GameObject toBeHit = null){
		if(toBeHit == null)
			DebugAimShot();
		else{
			toBeHit.GetComponent<COMP_stat>().HP -= hitDmg;
		}
	}
	private IEnumerator BulletHitSpark(RaycastHit hit){
		GameObject bulletHitSpark = Resources.Load("Effect/Explosion9") as GameObject;
		GameObject go = Instantiate(bulletHitSpark, hit.point, new Quaternion(0,0,0,0));
		go.transform.localScale *= 1f;
		yield return new WaitForSeconds(.2f);
		Destroy(go);
	}
	private IEnumerator MuzzleFlash(){
		MF_image.SetActive(true);
		yield return new WaitForSeconds(0.02f);
		MF_image.SetActive(false);
	}
	#endregion
	
	#region HUD stuff
	public IEnumerator CountDownTimer(float countDownFrom){
		t = countDownFrom;
		ft = Time.time + countDownFrom;
		timertxt.text = t.ToString();
		TimerPanel.SetActive(true);
		while(t > 0 || t >= ft){
			yield return new WaitForSeconds(0.02f);
			t = ft - Time.time;
			timertxt.text = t.ToString("0.00");
		}
		t = 0;
		while(true)
		{
			timertxt.text = t.ToString("0.00");
			yield return new WaitForSeconds(0.2f);
			timertxt.text = "";
			yield return new WaitForSeconds(0.2f);
			
		}
		//t = countDownFrom;
		//ft = Time.time + countDownFrom;
		//if(t < 0.5f)
			//closeTimer();
	}
	public void closeTimer(){
		timertxt.text = "";
		TimerPanel.SetActive(false);
	}
	#endregion
}