using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public static float actHP;
    public static bool isDead;

    private static readonly float fullHP = 100;
    private static UnityEngine.UI.Text lpText;
    // Use this for initialization
    void Start () {
        actHP = fullHP;
        lpText = GameObject.Find("CurrentLife").GetComponentInChildren<UnityEngine.UI.Text>();
        lpText.text = "Current LP: " + actHP.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        CheckLife();
	}

    public static void GetDamage(float damage)
    {
        if (isDead == false)
        {
            actHP -= damage;
        }
    }

    public static void SetLifepoints()
    {
        actHP = fullHP;
        isDead = false;
        lpText.text = "Current LP: " + actHP.ToString();
    }

    private void CheckLife()
    {
        lpText.text = "Current LP: " + actHP.ToString();
        if (actHP <= 0 && isDead == false)
        {
            actHP = 0;
            isDead = true;
            PhaseManager.EndFightPhase();
        }
    }
}
