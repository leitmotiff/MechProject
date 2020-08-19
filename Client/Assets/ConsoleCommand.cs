using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro;

public class ConsoleCommand : MonoBehaviour
{
    private StateManager SM;
	public TMP_InputField consoleInput;
	public Transform ContentObj;
	public TextMeshProUGUI TextIn;
	private bool consoleOn = false;

	private void Awake() {
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
	}

	void Update()
    {
        if(Input.GetKeyDown("~") || Input.GetKeyDown("`")){
			consoleOn = !consoleOn;
			SM.PlayState = !SM.PlayState;
			SM.PausedState = !SM.PausedState;

			//transform.GetChild(0).gameObject.SetActive(SM.PausedState);
		}

		if(consoleOn && Input.GetKeyDown(KeyCode.Return)) {
			var newLog = Instantiate(new TextMeshProUGUI(), ContentObj.transform.position, new Quaternion(), ContentObj);
			newLog.text = TextIn.text;
			Debug.Log(TextIn.text + " : " + newLog.text);
			TextIn.text = "";
		}
    }
}
