using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class storyBriefing1 : MonoBehaviour
{
	//public GameObject PlotPanel, wiPanel;
	public TextMeshProUGUI tmp, wi;
	public string tSSF = "", newwi = "";
	private IterativeWorldBuilding IWB;
	
	#region String Lists
	string[] identityL =
	{"an infamous private military corporation",
	"glory-seeking warriors",
	"union regulars, career soldiers",
	"union auxiliaries, recruited from a local world",
	"elite agents of a planetary defense force",
	"enforcers of the law",
	"a gang of criminals, thieves, and swindlers",
	"acolytes of an ancient martial order",
	"devotees of a higher power",
	"guardians of an ancient royal lineage",
	"corporate security; asset protection",
	"explorers of the unknown",
	"pirate scum",
	"defenders of the homeland",
	"the vanguard of the rebellion",
	"saviours of the weak and helpless",
	"hugry travelers, in it for the money",
	"inventors, engineers, and test subjects",
	"heirs of a famous legacy",
	"the only ones who can stop what's coming"};
	string[] patronL =
	{"anyone who pays us",
	"our commanding officer",
	"the high priest",
	"a corporate patron",
	"our ancient martial law; our duty",
	"our mentor",
	"our local union administrator and high command",
	"the whisperings of a long-dead monolith",
	"our liege-lord and king",
	"the eldars of our organization"};
	string[] goalL = 
	{"escort a VIP from a compromised location to a safer one",
	"respond to an SOS from an unknown source with a location noted in the message",
	"Retrieve a valued or strategic object, item, or information from a secure, hostile location",
	"Investigate a rumor or tip from a valued informant.",
	"Escort a long-flight weapon or ordnance to its target.",
	"defend an area expecting an attack (e.g., from pirates, hostile alien fauna, etc)",
	"explore a long-abandoned derelict for artifacts.",
	"bring down a piece of massive infrastructure (e.g., a bridge, skyhook, dam, etc)",
	"cause a distraction to provide cover for a covert mission of utmost importance",
	"assassinate a VIP, discreetly, or in broad daylight, to send a message",
	"attack a hostile defensive position to destroy a key objective",
	"board a hostile ship or station and take it over; or, destroy it",
	"be the first on the ground on a world hostile to human life; create a beachhead",
	"smuggle something safely or securely through hostile territory",
	"hunt down a team of notorious, feared, or respected mech pilots",
	"provide cover for an evacuation",
	"rescue and extract someone from a secure or dangerous location, e.g. a prison or warzone",
	"secure a dangerous location",
	"with Union’s backing, liberate a people held hostage by their cruel ruler",
	"make a desperate attempt to stop an incoming missile or attack"};
	
	#endregion
	
	private StateManager SM;
	void Awake(){
		IWB = GetComponent<IterativeWorldBuilding>();
		IWB.Gen();
		
		SM = GameObject.Find("EventSystem").GetComponent<StateManager>();
	}
	
	public void Start(){
		wi.text = GetWorldInfo();
		tmp.text = QuickStartStory();
	}
	
	public void Update(){
		if(Input.GetButton("Fire1"))
		{
			SM.MenuState = false;
			SM.PlayState = true;
			this.gameObject.SetActive(false);
		}
	}
	
	public string QuickStartStory(){
		System.Random rand = new System.Random();
		
		tSSF = "";
		
		tSSF += "You are " + identityL[rand.Next(0,identityL.Length)];
		
		tSSF += ", sent by " + patronL[rand.Next(0,patronL.Length)] + ".\n";
		
		tSSF += "Your mission: " + goalL[rand.Next(0,goalL.Length)] + ".\n";
		
		return tSSF;
	}
	
	public string GetWorldInfo(){
		
		newwi = "";
		
		newwi += "* " + IWB.worldType + "\n";
		newwi += "* " + IWB.naturalFeat + "\n";
		newwi += "* " + IWB.anthroFeat + "\n";
		newwi += "* " + IWB.envType + "\n";
		
		return newwi;
	}
	
	
}