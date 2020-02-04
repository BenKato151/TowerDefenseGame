using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuyItems : MonoBehaviour
{
    public int cost;
    public static bool isBuying;
    public bool whichItem;

    private GameObject prefab;
    private GameObject parent;
    private string itemName;
    private Text itemNameUI;

    #region Start
    private void Start()
    {
        itemName = this.gameObject.name;
        foreach (Text item in this.gameObject.GetComponentsInChildren<Text>())
        {
            if (item.gameObject.name.Contains("ItemName"))
            {
                item.text = itemName;
            }

            if (item.gameObject.name.Contains("Attack"))
            {
                // TODO: Get Attack Attributes from Item
                
                item.text = "Attack: " + 10;
            }

            if (item.gameObject.name.Contains("Costs"))
            {
                item.text = "Costs: " + cost.ToString();
            }

            if (item.gameObject.name.Contains("Reach"))
            {
                // TODO: Get Reach Attributes from Item
                // Because I dont want to formate different UIs and 
                // the Vector3 properties of the reach is always the same, I will display only the X Value here

                item.text = "Reach: " + 5;
            }

            if (item.gameObject.name.Contains("Lifepoints"))
            {
                // TODO: Get Lifepoints Attributes from Item
                item.text = "Lifepoints: " + 10;
            }
        }
    }
    #endregion

    #region Create Item In World (SetItem())
    public void SetItem(Field field)
    {
        try
        {
            if (isBuying && MoneyManagement.GetMoney() >= 0 && MoneyManagement.GetMoney() >= cost)
            {
                if (!field.isOccupied)
                {
                    Vector3 itempos = field.gameObject.transform.position;
                    if (prefab.name.Contains("Tower"))
                    {
                        if (Tower.allowedFieldID == field.fieldid)
                        {
                            GameObject newItem = Instantiate(prefab, new Vector3(itempos.x, itempos.y + 5f, itempos.z), new Quaternion(), parent.transform);
                            newItem.AddComponent<Tower>();
                            newItem.GetComponent<Tower>().posInGrid = field.position;
                            MoneyManagement.BuyItems(cost);
                            PhaseManager.consoletext.text = itemName + " gekauft für " + cost.ToString() + " Gold.";
                            field.isOccupied = true;
                        }
                        else
                        {
                            PhaseManager.consoletext.text = "Not a valid field!";
                        }
                    }
                    if (prefab.name.Contains("Hero"))
                    {
                        if (Hero.allowedFieldIDs.Contains(field.fieldid))
                        {
                            GameObject newItem = Instantiate(prefab, new Vector3(itempos.x, itempos.y + 5f, itempos.z), new Quaternion(), parent.transform);
                            newItem.AddComponent<Hero>();
                            newItem.GetComponent<Hero>().posInGrid = field.position;  
                            MoneyManagement.BuyItems(cost);
                            PhaseManager.consoletext.text = itemName + " gekauft für " + cost.ToString() + " Gold.";
                            field.isOccupied = true;
                        }
                        else
                        {
                            PhaseManager.consoletext.text = "Not a valid field!";
                        }
                    }
                }
                else
                {
                    PhaseManager.consoletext.text = "Field is occupied!";
                }
            }
            else
            {
                PhaseManager.consoletext.text = "Not enough Money!";
            }
        }
        catch (Exception)
        {
            PhaseManager.consoletext.text = "Not implemented Item";
        }
    }
    #endregion

    public void BuyItem()
    {
        try
        {
            if (itemName.Contains("Tower"))
            {
                prefab = Resources.Load<GameObject>("Prefabs/Tower/" + itemName);
                parent = GameObject.Find("Towers");
                isBuying = true;
                whichItem = true;
            }

            if (itemName.Contains("Hero"))
            {
                prefab = Resources.Load<GameObject>("Prefabs/Heroes/" + itemName);
                parent = GameObject.Find("Heroes");
                isBuying = true;
                whichItem = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error:\n" + e);
        }
    }
}
