using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInHangar : MonoBehaviour
{
	public Transform toFollow;
	
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(toFollow.position.x, transform.position.y, transform.position.z);
    }
}
