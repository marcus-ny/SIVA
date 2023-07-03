using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp
{
    public string text;
    public TextMeshPro textObject;
    public PopUpType popUpType;

    public enum PopUpType
    {
        Damage,
        LightDamage,
        Healing,
        RedNotify,
        GreenNotify,
        SystemError
    }

    public PopUp(string text, TextMeshPro textObject, PopUpType popUpType)
    {
        this.text = text;
        this.textObject = textObject;
        this.popUpType = popUpType;
    }
}
