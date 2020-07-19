using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StoreItem {
    public string name, flavor;
    public int id1, id2;
    public float val1, val2;
};


public class ItemLibrary : MonoBehaviour
{
    public List<StoreItem> StoreFront = new List<StoreItem>();

    public void LoadItemsFromTextFile(){
        StoreItem item = new StoreItem();

        StoreFront.Add(item);
	}
}
