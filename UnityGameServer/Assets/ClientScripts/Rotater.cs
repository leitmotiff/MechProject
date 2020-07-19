using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotateSpeed = 5f;

    void Update()
    {
        transform.Rotate(new Vector3(0,rotateSpeed,0));
    }
}
