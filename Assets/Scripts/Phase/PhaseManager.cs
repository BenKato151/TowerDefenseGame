using UnityEngine;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour {

    public static int roundnumber = 1;
    public static bool isInFight;
    public static Text consoletext;

    private static GameObject shop;
    private static MapManager mapManager;
    private static Text phaseText;

	// Use this for initialization
	void Start () {

        mapManager = this.gameObject.GetComponent<MapManager>();
        shop = GameObject.Find("ShopMenu");
        phaseText = GameObject.Find("Phase").GetComponentInChildren<Text>();
        consoletext = GameObject.Find("Console").GetComponentInChildren<Text>();
        phaseText.text = "Build-Phase " + roundnumber.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        CheckRound();
	}

    public void StartFightPhase()
    {
        shop.SetActive(false);
        consoletext.gameObject.SetActive(false);
        consoletext.text = "";
        phaseText.text = "Kampf-Phase " + roundnumber.ToString();
        GameManager.SetLifepoints();
        isInFight = true;
        mapManager.StartSpawnEnemies();
    }

    public static void EndFightPhase()
    {
        isInFight = false;
        mapManager.EndSpawnEnemies();
        roundnumber++;
        phaseText.text = "Build-Phase " + roundnumber.ToString();
        shop.SetActive(true);
        consoletext.gameObject.SetActive(true);
    }

    private void CheckRound()
    {
        if (roundnumber > 20)
        {
            consoletext.text = "Demo Finished! Please Press Enter to Exit!";
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameObject.Find("Canvas").GetComponent<PauseMenu>().LoadMenu();
            }
        }

        switch (roundnumber)
        {
            case 5:
                consoletext.text = "Auto-Upgrade not implemented yet";
                break;
            case 10:
                consoletext.text = "Auto-Upgrade not implemented yet";
                break;
            case 15:
                consoletext.text = "Auto-Upgrade not implemented yet";
                break;
            case 20:
                break;
            default:
                break;
        }
    }

}
