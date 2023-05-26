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
        barSize.x = GetComponentInParent<EnemyInfo>().hitpoints / 75;
        transform.localScale = barSize;
    }
}
