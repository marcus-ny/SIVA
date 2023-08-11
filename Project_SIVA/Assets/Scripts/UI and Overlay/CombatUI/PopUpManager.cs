//using UnityEngine.UI;
using TMPro;
using UnityEngine;

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
        if ((gameEvent == GameEvents.EnemyHealthAltered))
        {

            float recentDamage = DamageManager.Instance.recentDamage;

            PopUpAppear("-" + recentDamage.ToString(), redColor);
        }
        if (gameEvent == GameEvents.EnemyHealed)
        {
            float recentDamage = DamageManager.Instance.recentDamage;
            PopUpAppear("+" + recentDamage.ToString(), greenColor);
        }
    }


    private void PopUpAppear(string text, Color tempColor)
    {
        Enemy recentTarget = DamageManager.Instance.recentTarget;
        if (text == null)
        {
            Debug.Log("string is null");
            return;
        }

        GameObject DamageTextInstance = Instantiate(damageTextPrefab, recentTarget.transform);
        DamageTextInstance.transform.position = recentTarget.transform.position;



        PopUp newPopUp = new PopUp(text, DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>(), PopUp.PopUpType.Damage);

        newPopUp.text = text;
        newPopUp.textObject.text = text;
        newPopUp.textObject.color = tempColor;
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
