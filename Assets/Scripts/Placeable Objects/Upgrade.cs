using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Simple but ugly Upgrade Manager... Honestly all code I write is rlly ugly, bc I don't have the time to use OOP
public class Upgrade : MonoBehaviour {

    public GameObject upgradeContainer;

    public void SetNewUpgrade()
    {
        PhaseManager.consoletext.text = upgradeContainer.name + " is not implemented yet!"; 
    }
}
