using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechStats : MonoBehaviour
{
    //public float 0HULL, 1ARMOR, 2SENSORS, 3SAVETARGET, 4HP, 5REPAIRCAP, 6EVASION, 
    //              7SPEED, 8EDEF, 9TECHA, 10SP, 11HEATCAP;
    public float[] cStats = { 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0};
    public float[] bStats = { 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0 };
    public List<StoreItem> Items;
    public int coins = 100;
    public float tempHP = 100;
    private float maxX, maxY, maxHP;
    public RectTransform HealthRect;
    public StateManager SM;

	private void Start() {
        maxHP = cStats[4];
        tempHP = maxHP;
        maxX = HealthRect.sizeDelta.x;
        maxY = HealthRect.sizeDelta.y;
    }

	private void Update() {
        HealthRect.sizeDelta = Vector2.Lerp(HealthRect.sizeDelta, new Vector2(maxX * tempHP / maxHP, maxY), Time.deltaTime);
        
        if(tempHP <= 0){
            SM.PlayerKill(this.transform);
        }
    }
    public void ResetBaseStats() {
        
    }

    public void SetCurrentStats(){
        foreach(StoreItem i in Items){
            if (i.id1 >= 0 && i.id1 < 12)
                cStats[i.id1] = bStats[i.id1] + i.val1;
            if (i.id2 >= 0 && i.id2 < 12)
                cStats[i.id2] = bStats[i.id2] + i.val2;
        }
	}
}