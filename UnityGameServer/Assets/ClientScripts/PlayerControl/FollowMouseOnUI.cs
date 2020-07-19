using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseOnUI : MonoBehaviour
{
	void Awake(){
		Cursor.visible = false; 
	}
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
