using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogManager : MonoBehaviour
{
    public int maxMessages = 25;

    public GameObject chatPanel, textObject;

    public Color playerTurnMessage, enemyTurnMessage;

    [SerializeField] 
    List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //SendMessageToLog("Space key pressed", Message.MessageType.PlayerTurn);
            SendMessageToLog("Space key pressed");
        }
    }

    //public void SendMessageToLog(string text, Message.MessageType messageType)
    public void SendMessageToLog(string text)
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
        //newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = Color.black;

        switch (messageType)
        {
            case Message.MessageType.PlayerTurn:
                color = playerTurnMessage;
                break;
            case Message.MessageType.EnemyTurn:
                color = enemyTurnMessage;
                break;
        }
        
        return color;
    }
}

[System.Serializable] 
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
    public MessageType messageType;

    public enum MessageType
    {
        PlayerTurn,
        EnemyTurn
    }
}
