using UnityEngine;
using UnityEngine.UI;

public class MoneyManagement : MonoBehaviour {

    private static int money = 30;
    private static Text moneytext;

    void Start () {
        moneytext = GameObject.Find("Money").GetComponentInChildren<Text>();
        moneytext.text = "Current Money: " + money.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public static void DropMoney(int getMoney)
    {
        Text moneytext = GameObject.Find("Money").GetComponentInChildren<Text>();
        money += getMoney;
        moneytext.text = "Current Money: " + money.ToString();
    }

    public static void BuyItems(int costs)
    {
        money -= costs;
        moneytext.text = "Current Money: " + money.ToString();
    }

    public static int GetMoney()
    {
        return money;
    }
}
