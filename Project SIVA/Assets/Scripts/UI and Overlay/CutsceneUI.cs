using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class CutsceneUI : MonoBehaviour
{
    [SerializeField] Button skipCutsceneButton;
    
    // Start is called before the first frame update
    void Start()
    {
        skipCutsceneButton.onClick.AddListener(EndCutscene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndCutscene()
    {
        //Finish the cutscene

        //Disable CutsceneUI and enable BattleUI

        //Change BattleSimulator to PlayerTurn
        //BattleSimulator.Instance.StartGame();
    }
}
