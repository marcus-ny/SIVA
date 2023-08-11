using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float cameraSpeed;
    private float initialX;
    private float initialY;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().orthographicSize = 2.5f;
        cameraSpeed = 5f;
        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (BattleSimulator.Instance.State == BattleState.TRANSITION)
        {
            if (BattleSimulator.Instance.GetCurrentEnemy() != null)
            {
                transform.position = new(BattleSimulator.Instance.GetCurrentEnemy().transform.position.x, BattleSimulator.Instance.GetCurrentEnemy().transform.position.y, transform.position.z);
            }
        }
        else
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
        }

    }
}
