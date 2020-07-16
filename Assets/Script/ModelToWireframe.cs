using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelToWireframe : MonoBehaviour
{
	private List<Transform> ShapeL; 
	private List<LineRenderer> lineL;
	public Material lineMat, fillMat;
	
    void Start()
    {
		ShapeL = GetAllShapes();
		ShapeWire();
    }
	
	void Update(){
		
	}
	
	void ShapeWire(){	
		foreach(Transform tr in ShapeL){
			LineRenderer LR = tr.gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
			LR.useWorldSpace = false;
			LR.material = lineMat;
			//LR.startColor = Color.green;
			//LR.endColor = Color.green;
			LR.receiveShadows = false;
			LR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			
			LR.widthMultiplier = 0.1f;
			
			Vector3[] positions = GetShapeV();
			LR.positionCount = positions.Length;
			LR.SetPositions(positions);
			
			if(fillMat != null)
				tr.GetComponent<MeshRenderer>().material = fillMat;
			else
				tr.GetComponent<MeshRenderer>().enabled = false;
		}
	}
	
	private Vector3[] GetShapeV(){
		Vector3[] positions = new Vector3[17];
		Vector3 orig = transform.position;
		float x = transform.localScale.x/2, y = transform.localScale.y/2, z = transform.localScale.z/2;
		
		positions[0] = new Vector3(x, y, z);
		
		positions[1] = new Vector3(-x, y, z);
		positions[2] = new Vector3(-x, -y, z);
		positions[3] = new Vector3(-x, y, z);
		
		positions[4] = new Vector3(-x, y, -z);
		positions[5] = new Vector3(-x, -y, -z);
		positions[6] = new Vector3(-x, y, -z);
		
		positions[7] = new Vector3(x, y, -z);
		positions[8] = new Vector3(x, -y, -z);
		positions[9] = new Vector3(x, y, -z);
		
		positions[10] = new Vector3(x, y, z);
		
		positions[11] = new Vector3(x, -y, z);
		positions[12] = new Vector3(-x, -y, z);
		positions[13] = new Vector3(-x, -y, -z);
		positions[14] = new Vector3(x, -y, -z);
		positions[15] = new Vector3(x, -y, z);
		
		positions[16] = new Vector3(x, y, z);
		
		return positions;
	}

	private List<Transform> GetAllShapes(){
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		List<Transform> childObjects = new List<Transform>();
		foreach (Transform child in allChildren)
		{ 
			if(child.GetComponent<MeshRenderer>() != null)
				childObjects.Add(child.transform);
		}
		return childObjects;
	}

}
