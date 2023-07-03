using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class PopUpManager : MonoBehaviour, IObserver
{
    public GameObject damageTextPrefab;
    public Color redColor, greenColor, blackColor, yellowColor;

    // Start is called before the first frame update
    void Start()
    {
        DamageManager.Instance.AddObserver(this);
        BattleSimulator.Instance.AddObserver(this);
    }

    public void OnNotify(GameEvents gameEvent)
    {
        if (gameEvent == GameEvents.EnemyHealthAltered)
        {
            //Enemy recentTarget = DamageManager.Instance.recentTarget;
            float recentDamage = DamageManager.Instance.recentDamage;
            //PopUpAppear("-" + recentDamage.ToString(), PopUp.PopUpType.Damage, recentTarget);
            //XXPopUpAppear("-" + recentDamage.ToString(), PopUp.PopUpType.Damage);
            PopUpAppear("-" + recentDamage.ToString());
        }
    }

    //private void PopUpAppear(string text, PopUp.PopUpType popUpType, Enemy recentTarget)
    //XXprivate void PopUpAppear(string text, PopUp.PopUpType popUpType)
    private void PopUpAppear(string text)
    {
        Enemy recentTarget = DamageManager.Instance.recentTarget;
        if(text == null) 
        { 
            Debug.Log("string is null"); 
            return; 
        }
        // Instantiate the pop up
        GameObject DamageTextInstance = Instantiate(damageTextPrefab, recentTarget.transform);
        DamageTextInstance.transform.position = recentTarget.transform.position;
        //Debug.Log(recentTarget.transform.position.ToString());

        // This is a convoluted way to categorize pop up
        PopUp newPopUp = new PopUp(text, DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>(), PopUp.PopUpType.Damage);

        newPopUp.text = text;
        newPopUp.textObject.text = text;
        newPopUp.textObject.color = redColor;
        //newPopUp.textObject.color = PopUpColor(popUpType);

        /*
        DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().text = text;
        DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().color = blackColor;
        */
    }

    Color PopUpColor(PopUp.PopUpType popUpType)
    {
        Color color = blackColor;

        switch (popUpType)
        {
            case PopUp.PopUpType.Damage:
                color = redColor;
                break;
            case PopUp.PopUpType.LightDamage:
                color = yellowColor;
                break;
            case PopUp.PopUpType.Healing:
                color = greenColor;
                break;
            case PopUp.PopUpType.RedNotify:
                color = redColor;
                break;
            case PopUp.PopUpType.GreenNotify:
                color = greenColor;
                break;
            case PopUp.PopUpType.SystemError:
                color = redColor;
                break;
        }

        return color;
    }
}
