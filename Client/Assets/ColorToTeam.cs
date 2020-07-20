using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorToTeam : MonoBehaviour
{

    void Start()
    {
		switch (tag) {
			case ("GreenTeam"):
				this.GetComponent<MeshRenderer>().material = Resources.Load("Materials/gridGreen") as Material;
				break;
			case ("BlueTeam"):
				this.GetComponent<MeshRenderer>().material = Resources.Load("Materials/gridBlue") as Material;
				break;
			case ("RedTeam"):
				this.GetComponent<MeshRenderer>().material = Resources.Load("Materials/gridRed") as Material;
				break;
			case ("OrangeTeam"):
				this.GetComponent<MeshRenderer>().material = Resources.Load("Materials/gridOrange") as Material;
				break;
			default:
				break;
		}
	}
}
