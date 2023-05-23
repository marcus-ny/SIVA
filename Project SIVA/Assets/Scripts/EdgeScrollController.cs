using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScrollController : MonoBehaviour
{
    private float edgeSize;
    private float cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        edgeSize = 5f;
        cameraSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(moveAmt, 0, 0));
        }

        if (Input.mousePosition.x < edgeSize)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(-moveAmt, 0, 0));
        }

        if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, moveAmt, 0));
        }

        if (Input.mousePosition.y < edgeSize)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, -moveAmt, 0));
        }
    }
}
