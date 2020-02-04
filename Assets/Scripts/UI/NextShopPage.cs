using UnityEngine;

public class NextShopPage : MonoBehaviour {

    [SerializeField]
    private GameObject newshop;

    [SerializeField]
    private GameObject oldshop;
	
    public void EnableNextPage()
    {
        PhaseManager.consoletext.text = "";
        newshop.SetActive(true);
        oldshop.SetActive(false);
    }
}
