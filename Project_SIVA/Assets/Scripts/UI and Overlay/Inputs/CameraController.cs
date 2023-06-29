using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float cameraSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cameraSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        float xAxisValue = 0;
        float yAxisValue = 0;

        if (Input.GetKey(KeyCode.A) && transform.position.x > -5f)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(-moveAmt, 0, 0));
        }

        if (Input.GetKey(KeyCode.D) && transform.position.x < 5f)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(moveAmt, 0, 0));
        }

        if (Input.GetKey(KeyCode.W) && transform.position.y < 2.5f)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, moveAmt, 0));
        }

        if (Input.GetKey(KeyCode.S) && transform.position.y > -2.5f)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, -moveAmt, 0));
        }

        //transform.Translate(new Vector3(xAxisValue, yAxisValue, 0));
    }
}
