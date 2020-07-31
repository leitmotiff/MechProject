using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathClass : MonoBehaviour {
    public bool Add, RemoveLast, Clear;
    public List<Vector3> nodePositions;

    void Update(){
        if(Add){
            nodePositions.Add(transform.position);
            Add = false;
		}
        if (RemoveLast) {
            try {
                nodePositions.RemoveRange(nodePositions.Count - 1, 1);
            }
            catch { }
            RemoveLast = false;
        }
        if (Clear) {
            nodePositions.Clear();
            Clear = false;
        }
    }
}
