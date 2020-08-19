using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotGen : MonoBehaviour
{
	#region Pilot
	string name = "", callSign = "";
	
	/* Backgrounds
		1 Celebrity
		2 Colonist
		3 Criminal
		4 Far-FieldTeam
		5 Hacker
		6 Mechanic
		7 Medic
		8 Mercenary
		9 NHPSpecialist
		10 Noble
		11 Outlaw
		12 PenalColonist
		13 Priest
		14 Scientist
		15 Soldier
		16 Spaceborn
		17 SpecOps
		18 SuperSoldier
		19 StarshipPilot
		20 Worker
	*/
	int bg;
	/* Triggers
		Threaten
		Patch
		GetSomewhereFast
		Spot
		LeadInspire
		InventCreate
		StayCool
		ApplyFistToFace
		Assault
		ReadSituation
		WordOnStreet
		ActUnseen
		Survive
		HackFix
		ShowOff
		BlowUp
		TakeControl
		PullRank
		Charm
		TakeSomeoneOut
		Investigate
		Threaten
	*/
	List<string> trig = new List<string>();
	/* Talents
		CrackShot
		Infiltrator
		Leader
		
	*/
	//List<string> talents = new List<string>();
	int parmor;
	float psize = 0.5f;
	
	#endregion
	
	#region Mech
	string frame = "GMS", mechName = "";
	// heavy mount, main mount, aux mount
	float msize = 1f;
	int Armor, Grit, HP, Evasion, EDefense, Speed,
	RepairCap, HeatCap, Sensors, TechAttack;
	
	
	#endregion
	
    public void NewPilotGen(){
		// Background
		
		// 4 triggers at +2
		HP = 6;
		Evasion = 10;
		EDefense = 10;
		Speed = 4;
		
		// 1 armor, 2 weapons, 3 gear
		
	}	
	
	public void NewMechGen(){
		
	}
}
