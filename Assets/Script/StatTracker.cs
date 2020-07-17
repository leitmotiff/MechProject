using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerStats{
	public string id;
	public string team;
	public int dmgDealt, dmgReceived, kills, deaths;
};

public class StatTracker : MonoBehaviour
{
	
	
	public List<PlayerStats> statList = new List<PlayerStats>();
	
	public void NewPlayerJoined(string id, string team){
		PlayerStats newP = new PlayerStats();
		if(statList.Exists(x => x.id == id)){
			int q = 2;
			string newid = id + q.ToString(); 
			while(statList.Exists(x => x.id == id)){
				q++;
			}
			
			
		}
		else
			newP.id = id;
		newP.team = team;
		newP.dmgDealt = 0;
		newP.dmgReceived = 0;
		newP.kills = 0;
		newP.deaths = 0;
		
		statList.Add(newP);
	}
	
	public void PlayerExit(string id){
		PlayerStats toRemove = statList.Find(x => x.id == id);//.ToArray()[0];
		statList.Remove(toRemove);
	}
    
}
