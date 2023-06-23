using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogManager : MonoBehaviour, IObserver
{
    public int maxMessages = 25;
    public GameObject chatPanel, textObject;
    public Color redColor, greenColor, blackColor, yellowColor;

    [SerializeField] 
    List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        DamageManager.Instance.AddObserver(this);
        BattleSimulator.Instance.AddObserver(this);
        PlayerController.Instance.AddObserver(this);
        SendMessageToLog("<<Battle Start!>>", Message.MessageType.RedNotify);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SendMessageToLog("[Testing] Space key pressed", Message.MessageType.GreenNotify);
        }
        */
    }

    public void SendMessageToLog(string text, Message.MessageType messageType)
    {
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = blackColor;

        switch (messageType)
        {
            case Message.MessageType.Damage:
                color = blackColor;
                break;
            case Message.MessageType.LightDamage:
                color = yellowColor;
                break;
            case Message.MessageType.Healing:
                color = blackColor;
                break;
            case Message.MessageType.GreenNotify:
                color = greenColor;
                break;
            case Message.MessageType.RedNotify:
                color = redColor;
                break;
            case Message.MessageType.SystemError:
                color = redColor;
                break;
        }
        
        return color;
    }

    public void OnNotify(GameEvents gameEvent)
    {
        // Turn related
        if (gameEvent == GameEvents.PlayerTurn)
        {
            SendMessageToLog("<<Player's Turn>>", Message.MessageType.GreenNotify);
        }
        if (gameEvent == GameEvents.EnemyTurn)
        {
            SendMessageToLog("<<Enemy's Turn>>", Message.MessageType.RedNotify);
        }

        // System Related
        if (gameEvent == GameEvents.AOEunsuccessful)
        {
            SendMessageToLog("--AOE Unsuccessful--", Message.MessageType.SystemError);
        }
        if (gameEvent == GameEvents.InteractableOFR)
        {
            SendMessageToLog("--Interactable Out of Range--", Message.MessageType.SystemError);
        }
        if (gameEvent == GameEvents.NoInteractable)
        {
            SendMessageToLog("--No Intractables on Clicked Tile--", Message.MessageType.SystemError);
        }
        if(gameEvent == GameEvents.TargetOFR)
        {
            SendMessageToLog("--Target Out of Range--", Message.MessageType.SystemError);
        }
        if (gameEvent == GameEvents.NoTarget)
        {
            SendMessageToLog("--No Target on Clicked Tile--", Message.MessageType.SystemError);
        }

        // Damage related
        if (gameEvent == GameEvents.PlayerHealthAltered)
        {
            float recentDamage = DamageManager.Instance.recentDamage;
            Enemy currentEnemy = BattleSimulator.Instance.GetCurrentEnemy();

            switch (currentEnemy.ToString())
            {
                case "Soldier":
                    SendMessageToLog("[Donovan] is assaulted by enemy [Sodier]; -" + recentDamage + "HP", Message.MessageType.Damage);
                    break;
                case "Generator":
                    SendMessageToLog("[Donovan] is zapped by the [Generator]; -" + recentDamage + "HP", Message.MessageType.Damage);
                    break;
            }
                
        }
        if (gameEvent == GameEvents.EnemyHealthAltered)
        {
            Enemy recentTarget = DamageManager.Instance.recentTarget;
            float recentDamage = DamageManager.Instance.recentDamage;
            // TODO: Change log depending on vampire attack
            //SendMessageToLog("[" + recentTarget.ToString() + "] is spin attacked by [Donovan]; -" + recentDamage + "HP", Message.MessageType.Damage);
            SendMessageToLog("[" + recentTarget.ToString() + "] is attacked by [Donovan]; -" + recentDamage + "HP", Message.MessageType.Damage);
        }
        if (gameEvent == GameEvents.EnemyFriendlyFire)
        {
            Enemy recentTarget = DamageManager.Instance.recentTarget;
            float recentDamage = DamageManager.Instance.recentDamage;
            Enemy currentEnemy = BattleSimulator.Instance.GetCurrentEnemy();
            SendMessageToLog("[" + recentTarget.ToString() + "] is friendly fired by [" + currentEnemy.ToString() + "]; -" + recentDamage + "HP", Message.MessageType.Damage);
        }
        if (gameEvent == GameEvents.EnemyHealed)
        {
            Enemy recentTarget = DamageManager.Instance.recentTarget;
            float recentDamage = DamageManager.Instance.recentDamage;
            Enemy currentEnemy = BattleSimulator.Instance.GetCurrentEnemy();
            SendMessageToLog("[" + recentTarget.ToString() + "] is healed by [" + currentEnemy.ToString() + "]; +" + recentDamage + "HP", Message.MessageType.Healing);
        }
        if (gameEvent == GameEvents.LightDamage)
        {
            float lightDamage = DamageManager.Instance.lightDamage;
            SendMessageToLog("[Donovan] screams in the light; -" + lightDamage + "HP", Message.MessageType.LightDamage);
        }
    }
}
