using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public GameObject ShopPanel, AimPanel;
	private StateManager SM;
	private StatTracker ST;
	private bool shopmenu = false;
	public Texture2D cursorTexture;

	private void Awake() {
		CursorMode cursorMode = CursorMode.Auto;
		Vector2 hotSpot = Vector2.zero;
		//Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
		Cursor.visible = true;
	}

	private void Start() {
		if (AimPanel == null) AimPanel = GameObject.Find("AimPanel");
		if (ShopPanel == null) ShopPanel = GameObject.Find("ShopPanel");
		if (SM == null) SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
		if (ST == null) ST = GameObject.Find("EventSystem").GetComponent<StatTracker>();
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.M)){
			shopmenu = !shopmenu;
			SM.PlayState = !shopmenu;
			SM.PausedState = shopmenu;
			AimPanel.SetActive(!shopmenu);
			ShopPanel.SetActive(shopmenu);
		}
	}
}
