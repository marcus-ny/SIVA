using UnityEngine;

public class BossHealthBar : MonoBehaviour
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
        if (GetComponentInParent<Enemy>() != null)
        {
            barSize.x = GetComponentInParent<Enemy>().hpRatio * 2;
            transform.localScale = barSize;
        }
    }
}
