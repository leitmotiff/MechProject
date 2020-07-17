using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSheet : MonoBehaviour
{
	//	RP
	public string Name = "", CallSign = "";
    public int LL = 0;
	List<string> licenses = new List<string>() {"GMS"};
	//List<string> talents = new List<string>()
	int Background;
	
	//	Combat
	int Grit, HP, Evasion, EDefense, Speed;
	float size;
	
	//	Gear
	string weapon, armor, gear;
	
}
