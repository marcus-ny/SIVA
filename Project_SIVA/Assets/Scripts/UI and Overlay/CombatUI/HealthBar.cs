using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Vector3 barSize;
    // Start is called before the first frame update
    void Start()
    {
        barSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = PlayerController.Instance.character.GetComponent<SpriteRenderer>().sortingOrder;
        barSize.x = GetComponentInParent<CharacterInfo>().hitpoints/200;
        transform.localScale = barSize;
    }
}
