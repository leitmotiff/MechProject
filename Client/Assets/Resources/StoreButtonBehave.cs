using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class StoreButtonBehave : MonoBehaviour
{
	public StoreItemLibrary SIL = new StoreItemLibrary();
	
	public void TaskOnClick() {
		SIL.buttonName = GetComponentInChildren<TextMeshProUGUI>().text;
		SIL.holdItem = SIL.GetItem(SIL.buttonName);
		Debug.Log(SIL.buttonName + " button.");
    }

	public void BUYbutton(){
		int price = -1;
		if (SIL.buttonName != null)
			price = SIL.GetItem(SIL.buttonName).cost;

		int coins = SIL.MS.coins;

		if(price < -1){
			Debug.Log("fuck");
		}
		else if(coins < price) {
			Debug.Log("Not enough cash - cannot buy");
		}
		else if(coins >= price) {
			try {
				SIL.MS.coins -= price;
				SIL.setCC();
				SIL.MS.Items.Add(SIL.holdItem);
			}
			catch(Exception _ex){
				Debug.Log($"Didn't work - Refunding - {_ex}");
				SIL.MS.coins += price;
				SIL.setCC();
			}
		}
		else Debug.LogError("Shop Mechanic Malfunction");
	}
}
