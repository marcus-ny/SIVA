using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActionBoxManager : MonoBehaviour
{
    [SerializeField] GameObject actionBox;
    [SerializeField] GameObject attackBox;
    [SerializeField] Button attackButton;
    [SerializeField] Button returnButton;
    public float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        attackBox.SetActive(false);
        attackButton.onClick.AddListener(AttackPhase);
        returnButton.onClick.AddListener(ActionPhase);
    }

    private void ActionPhase()
    {
        StartCoroutine(ActionPhaseAnimation());
    }

    private void AttackPhase()
    {
        StartCoroutine(AttackPhaseAnimation());
    }

    IEnumerator AttackPhaseAnimation()
    {
        attackBox.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        actionBox.SetActive(false);
    }

    IEnumerator ActionPhaseAnimation()
    {
        actionBox.SetActive(true);
        yield return new WaitForSeconds(transitionTime);
        attackBox.SetActive(false);
    }
}
