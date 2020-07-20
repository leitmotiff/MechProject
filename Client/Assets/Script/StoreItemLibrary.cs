using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.VersionControl;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Monetization;
using System.Linq.Expressions;

public struct StoreItem {
    public string name, flavor;
    public int id1, id2;
    public float val1, val2;
    public int cost;
};

public class StoreItemLibrary : MonoBehaviour {
    public GameObject ShopPanel, ScrollViewContent, itemButtonPrefab,
                flavorPanel, statsPanel;
    public TextMeshProUGUI cc;
    public MechStats MS;
    public string storefrontTextFile = "Assets/Resources/StoreItemLibrary.txt";
    public List<StoreItem> StoreFront = new List<StoreItem>();
    char[] delimeterChars = { ',', '}', '{', '"' };
    public string[] statStr = {"HULL", "ARMOR", "SENSORS", "SAVETARGET", "HP", "REPAIRCAP",
                            "EVASION", "SPEED", "EDEF", "TECHA", "SP", "HEATCAP"};
    public string buttonName = null;
    public StoreItem holdItem;

    private void Awake() {
        LoadItemsFromTextFile();
        MS = GameObject.Find("LocalPlayerMech").GetComponent<MechStats>();
        if (ShopPanel == null) ShopPanel = this.transform.GetChild(2).gameObject;
        if (ScrollViewContent == null) ScrollViewContent = ShopPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        if (itemButtonPrefab == null) itemButtonPrefab = ScrollViewContent.transform.GetChild(0).GetChild(0).gameObject;

        LoadStorefrontUI();
        ShopPanel.SetActive(false);
    }

	private void LoadItemsFromTextFile() {

        List<string> allLines = new List<string>();
        using (var sr = new StreamReader(storefrontTextFile)) {
            string line;
            while ((line = sr.ReadLine()) != null) {
                allLines.Add(line);
            }
        }

        foreach (string line in allLines) {
            StoreItem item = new StoreItem();
            string[] pline = line.Split(delimeterChars, StringSplitOptions.RemoveEmptyEntries);

            /*var delimited = pline[0]
                 + " , " + pline[1]
                 + " , " + pline[2]
                 + " , " + pline[3]
                 + " , " + pline[4]
                 + " , " + pline[5]
                 + " , " + pline[6];
            Debug.Log(delimited);*/

            item.name = pline[0];
            item.flavor = pline[2];
            Int32.TryParse(pline[3], out item.id1);
            Int32.TryParse(pline[4], out item.id2);
            double val2, val1;
            Double.TryParse(pline[5], out val1);
            Double.TryParse(pline[6], out val2);
            item.val1 = (float)val1;
            item.val2 = (float)val2;
            Int32.TryParse(pline[7], out item.cost);

            //Debug.Log($"Loaded: {item.name}");
            StoreFront.Add(item);
        }
    }
    private void LoadStorefrontUI() {
        float x = 20, y = -20;
        int itemNum = 0;
        foreach (var item in StoreFront) {
            x = 40 * (itemNum % 4);
            y = -40 + -40 * (itemNum / 4);

            var newB = Instantiate(itemButtonPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), ScrollViewContent.transform);
            var trans = newB.GetComponent<RectTransform>();
            newB.GetComponent<RectTransform>().localPosition = new Vector3(x + (trans.pivot.x * trans.rect.width), y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);

            newB.GetComponentInChildren<TextMeshProUGUI>().text = item.name;

            itemNum++;
            newB.SetActive(true);
        }

        setCC();
    }

    public StoreItem GetItem(string name) {
        return StoreFront.Find(s => name.Contains(s.name));
    }
    public int GetItemCost(string name) {
        return GetItem(name).cost;
    }
    public void FillShopInfo() {
        holdItem = GetItem(buttonName);
        flavorPanel.GetComponentInChildren<TextMeshProUGUI>().text = holdItem.flavor;
        statsPanel.GetComponentInChildren<TextMeshProUGUI>().text = statStr[holdItem.id1] +
            " :\n" + (holdItem.val1 > 0 ? "+" : "-") + holdItem.val1.ToString() +
            (holdItem.id2 > 0 ? "\n" +
            statStr[holdItem.id2] +
            " :\n" + (holdItem.val1 > 0 ? "+" : "-") + holdItem.val1.ToString()
            : "");
        //Debug.Log(GetItem(name).flavor);
        //Debug.Log(statsPanel.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void setCC() {cc.text = MS.coins.ToString();}
	
}
