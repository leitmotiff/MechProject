using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FloatToTransform : MonoBehaviour
{
    private bool floatTo = false, lookBack = false;
    private Transform location;
    private Vector3 lastLocation;
    public void StartFloatRoutine(Transform lastLoc, Transform loc){
        if (lastLoc == null)
            lastLocation = new Vector3(transform.position.x, 0, transform.position.z);
        else
            lastLocation = lastLoc.position;
        location = loc;
        StartCoroutine(FloatRoutine());
	}

	private void Update() {
		if(lookBack){
            transform.LookAt(lastLocation);
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 10, Time.deltaTime);
        }
        if(floatTo){
            transform.position = Vector3.Lerp(transform.position, location.position, Time.deltaTime);
        }
    }

	private IEnumerator FloatRoutine(){
        int tfocus = 5, t = 0;
        lookBack = true;
        while(t < tfocus){
            t += 1;
            yield return new WaitForSeconds(1f);
		}
        lookBack = false;
        floatTo = true;
        tfocus = 10;
        while(floatTo && t < tfocus){
            if(Vector3.Distance(transform.position, location.position) < 1){
                floatTo = false;
			}
            yield return new WaitForSeconds(1f);
            t++;
        }
	}
}
