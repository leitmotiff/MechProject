using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechStats : MonoBehaviour
{
    //public float 0HULL, 1ARMOR, 2SENSORS, 3SAVETARGET, 4HP, 5REPAIRCAP, 6EVASION, 
    //              7SPEED, 8EDEF, 9TECHA, 10SP, 11HEATCAP;
    public float[] Stats = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public float[] bStats = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public List<StoreItem> Items;
    public int coins;

    public void SetBaseStats() {
        
    }

    public void SetStats(){
        foreach(StoreItem i in Items){
            if (i.id1 >= 0 && i.id1 < 12)
                Stats[i.id1] = bStats[i.id1] + i.val1;
            if (i.id2 >= 0 && i.id2 < 12)
                Stats[i.id2] = bStats[i.id2] + i.val2;
        }
	}
}