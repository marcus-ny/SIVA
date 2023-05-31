using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
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
        if (GetComponentInParent<Soldier>() != null)
        {
            Debug.Log("Bar size: " + barSize.x);
            barSize.x = GetComponentInParent<Soldier>().hitpoints / 100;
            transform.localScale = barSize;
        }
    }
}
