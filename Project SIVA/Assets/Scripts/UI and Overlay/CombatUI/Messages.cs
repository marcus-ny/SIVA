using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
    public MessageType messageType;

    public enum MessageType
    {
        PlayerTurn,
        EnemyTurn,
        SystemNotify
    }
}
