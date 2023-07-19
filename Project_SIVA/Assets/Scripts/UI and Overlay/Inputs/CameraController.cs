using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float cameraSpeed;
    private float initialX;
    private float initialY;

    // Start is called before the first frame update
    void Start()
    {
        cameraSpeed = 5f;
        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A) && transform.position.x > initialX - 5)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(-moveAmt, 0, 0));
        }

        if (Input.GetKey(KeyCode.D) && transform.position.x < initialX + 5)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(moveAmt, 0, 0));
        }

        if (Input.GetKey(KeyCode.W) && transform.position.y < initialY + 2.5)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, moveAmt, 0));
        }

        if (Input.GetKey(KeyCode.S) && transform.position.y > initialY - 2.5)
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, -moveAmt, 0));
        }

        //transform.Translate(new Vector3(xAxisValue, yAxisValue, 0));
    }
}
