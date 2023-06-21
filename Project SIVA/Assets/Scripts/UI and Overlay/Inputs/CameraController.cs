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
        float xAxisValue = Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime;
        float yAxisValue = Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;

        transform.Translate(new Vector3(xAxisValue, yAxisValue, 0));
    }
}
