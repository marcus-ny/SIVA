using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;

    public float hitpoints;

    private Animator animator;

    public Vector3Int cur, prev;


    private void Start()
    {
        hitpoints = 100;
        animator = GetComponent<Animator>();
    }

    /*
     * For showing red visual when taking damage
     */
    IEnumerator DamageVisual()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1);
        yield return new WaitForSecondsRealtime(0.3f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    public void DisplayDamageVisual()
    {       
        StartCoroutine("DamageVisual");
    }

    
    

}
