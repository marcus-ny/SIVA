using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointIndicator : MonoBehaviour
{
    public Text actionPointCounter;

    void Update()
    {
        int max = BattleSimulator.Instance.MAX_ACTIONS;
        int curr = BattleSimulator.Instance.actionsPerformed;
        actionPointCounter.text = "Action Points: " + (max-curr) + "/" + max;
    }
}
