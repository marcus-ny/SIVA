using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    /*
     * Function for carrying out actions on receiving a particular gameEvent raised from Publisher
     */
    public void OnNotify(GameEvents gameEvent);
}


/*
 * Stack that stores all the information that comes in (attacks, damage)
 * AddEvent() { Stack.add(event) or Stack.add("Message") ; UpdateStack()}
 * UpdateStack() { for (int i = 0; i < 4; i++) { gameObject.transform.GetChild(3-i).ChangeText("new message")
 */