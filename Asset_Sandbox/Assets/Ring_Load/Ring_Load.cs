using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring_Load : MonoBehaviour
{
    public float timeP1, timeP2;
    public TrailRenderer trailA;

    public bool run = false;
    public float speed, radius;
    
    void Start()
    {
        StartCoroutine(SimpleCircle(trailA, radius, 0.2f));
    }

    private IEnumerator SimpleCircle(TrailRenderer trail, float radius, float thickness){
        trail.emitting = false;
        trail.transform.localPosition = Vector3.up*radius;
        trail.startWidth = trail.endWidth = thickness;

        trail.emitting = true;
        while(run){
            trail.transform.RotateAround(Vector3.zero, Vector3.forward, speed);

            yield return new WaitForEndOfFrame();
		}
	}
}
