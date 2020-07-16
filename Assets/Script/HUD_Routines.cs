using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD_Routines : MonoBehaviour
{
	public bool testTrig = false;
	public GameObject TimerPanel;
	private TextMeshProUGUI timertxt;
	private float t, ft;
	
	void Start(){
		timertxt = TimerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		timertxt.text = "";		
	}
	void Update(){
		if(testTrig){
			StartCoroutine(CountDownTimer(4));
			testTrig = false;
		}
	}
	
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
		for(int i = 5; i > 0; i--)
		{
			yield return new WaitForSeconds(0.2f);
			timertxt.text = "";
			yield return new WaitForSeconds(0.2f);
			timertxt.text = t.ToString("0.00");
		}
		t = countDownFrom;
		ft = Time.time + countDownFrom;
		closeTimer();
	}
	public void closeTimer(){
		timertxt.text = "";
		TimerPanel.SetActive(false);
	}
}
