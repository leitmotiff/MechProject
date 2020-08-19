using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_node : MonoBehaviour
{
	public List<Transform> NeighborNodes = new List<Transform>();

	public Transform GetRandomNeighbor(){
		return NeighborNodes[Random.Range(0,NeighborNodes.Count)];
	}
}
