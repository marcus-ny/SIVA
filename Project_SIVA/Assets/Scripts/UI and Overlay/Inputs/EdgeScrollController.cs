using UnityEngine;

public class EdgeScrollController : MonoBehaviour
{
    private float edgeSize;
    private float initialX;
    private float initialY;
    private float cameraSpeed;

    // Start is called before the first frame update
    void Start()
    {
        edgeSize = 5f;
        cameraSpeed = 3f;
        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(Screen.width);
        if (Input.mousePosition.x > Screen.width - edgeSize && !(transform.position.x > initialX + 5))
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(moveAmt, 0, 0));
        }

        if (Input.mousePosition.x < edgeSize && !(transform.position.x < initialX - 5))
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(-moveAmt, 0, 0));
        }

        if (Input.mousePosition.y > Screen.height - edgeSize && !(transform.position.y > initialY + 2.5))
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, moveAmt, 0));
        }

        if (Input.mousePosition.y < edgeSize && !(transform.position.y < initialY - 2.5))
        {
            float moveAmt = cameraSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, -moveAmt, 0));
        }
    }
}
