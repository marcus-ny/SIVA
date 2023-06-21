using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogManager : MonoBehaviour, IObserver
{
    public int maxMessages = 25;
    public GameObject chatPanel, textObject;
    public Color playerTurnMessage, enemyTurnMessage, neutralMessage;

    [SerializeField] 
    List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        SendMessageToLog("<<Battle Start!>>", Message.MessageType.SystemNotify);
        SendMessageToLog("<<Player's Turn>>", Message.MessageType.SystemNotify);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SendMessageToLog("[Testing] Space key pressed", Message.MessageType.PlayerTurn);
        }
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
        Color color = neutralMessage;

        switch (messageType)
        {
            case Message.MessageType.PlayerTurn:
                color = playerTurnMessage;
                break;
            case Message.MessageType.EnemyTurn:
                color = enemyTurnMessage;
                break;
            case Message.MessageType.SystemNotify:
                color = neutralMessage;
                break;
        }
        
        return color;
    }

    public void OnNotify(GameEvents gameEvent)
    {
        switch (gameEvent)
        {
            // Check for turns
            case gameEvent == GameEvents.PlayerTurn:
                SendMessageToLog("<<Player's Turn>>", Message.MessageType.SystemNotify);
                break;
            case gameEvent == GameEvents.EnemyTurn:
                SendMessageToLog("<<Enemy's Turn>>", Message.MessageType.SystemNotify);
                break;
            // Check for damage done to player during enemy turn
            case gameEvent == GameEvents.PlayerHealthAltered:
                private recentDamage = DamageManager.Instance.recentDamage; 
                SendMessageToLog("[Donovan] received " + recentDamage + " damage!", Message.MessageType.EnemyTurn);
                break;
        }
}
