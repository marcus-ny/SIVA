using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class APUpdater : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionPointCounter;

    // Update is called once per frame
    void Update()
    {
        int max = BattleSimulator.Instance.MAX_ACTIONS;
        int curr = BattleSimulator.Instance.actionsPerformed;
        actionPointCounter.text = (max-curr) + "/" + max;
    }
}