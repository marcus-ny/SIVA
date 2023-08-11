using System.Collections;
using UnityEngine;

public class Firebolt : MonoBehaviour
{
    private readonly int SPEED = 2;
    private void Start()
    {
        transform.position = PlayerController.Instance.character.transform.position;
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        var step = SPEED * Time.deltaTime;

        OverlayTile destination = PlayerController.Instance.mostrecentEnemy.activeTile;
        int z = 1;

        while (Vector2.Distance(transform.position, destination.transform.position) > 0.001)
        {

            transform.position = Vector2.MoveTowards(transform.position, destination.transform.position, step);
            transform.position = new Vector3(transform.position.x, transform.position.y, z++);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
