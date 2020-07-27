using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FloatToTransform : MonoBehaviour
{
    private bool floatTo = false, lookBack = false, flag1 = false;
    private Transform location;
    private Vector3 lastLocation;
    private Vector3 PlayerBasePos = new Vector3(0,4,-10);

    public void StartFloatRoutine(Transform lastLoc, Transform loc){
        if (lastLoc == null)
            lastLocation = new Vector3(transform.position.x, 0, transform.position.z);
        else
            lastLocation = lastLoc.position;
        location = loc;
        StartCoroutine(FloatRoutine1());
	}
    public void FloatToBase(){
        StartCoroutine(FloatToBase1());
	}
	private void Update() {
		if(lookBack){
            transform.LookAt(lastLocation);
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 10, Time.deltaTime);
        }
        if(floatTo){
            transform.position = Vector3.Lerp(transform.position, location.position + Vector3.up * 5, Time.deltaTime);
        }

        if(flag1)
            transform.localPosition = Vector3.Lerp(transform.localPosition, PlayerBasePos, Time.deltaTime);

    }

    //Respawn float
	private IEnumerator FloatRoutine1(){
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
            if(Vector3.Distance(transform.position, location.position + Vector3.up * 5) < 1){
                floatTo = false;
			}
            yield return new WaitForSeconds(1f);
            t++;
        }

        Destroy(this.gameObject);
	}

    //Move to Player position
    private IEnumerator FloatToBase1(){
        flag1 = true;
        int tfocus = 10, t = 0;
        while (floatTo && t < tfocus) {
            if (Vector3.Distance(transform.position, PlayerBasePos) < 0.2f) {
                flag1 = false;
            }
            yield return new WaitForSeconds(1f);
            t++;
        }
    }
}
