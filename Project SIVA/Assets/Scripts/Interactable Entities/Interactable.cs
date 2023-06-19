using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable: MonoBehaviour
{
    public OverlayTile activeTile;
    public GameObject prefab;

    public abstract void ReceiveInteraction();

    public void Highlight()
    {
        StartCoroutine(ShowHighlighting());
    }

    IEnumerator ShowHighlighting()
    {
        int i = 250;
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(i, 255, i, 255);
        //yield return null;
        Debug.Log("Coroutine started for highlight");
        while (i > 0)
        {
            Color old = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(250, 0, 0, i);
            if (gameObject.GetComponent<SpriteRenderer>().color != old) Debug.Log("Color not same");
            i--;
            Debug.Log("i : " + i);
            yield return new WaitForSecondsRealtime(0.1f);           
        }
        
            /*
            
            while (i > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(i, i, i, i);
                i--;
                yield return null;
                
            }
            yield return null;*/
        

    }
}
