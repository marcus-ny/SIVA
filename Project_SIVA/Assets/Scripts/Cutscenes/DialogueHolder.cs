using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour, IObserver
    {
        [SerializeField] TurnsManager turnsManager;
        [SerializeField] GameEvents interestedGameEvent;

        
        private void Awake()
        {
            Deactivate();
            
            //StartCoroutine(dialogueSequence());
            
            //StartCoroutine(cleanDialogues());
            
            //turnsManager.AddObserver(this);
            
        }

        private void Start()
        {
            BattleSimulator.Instance.AddObserver(this);
        }

        
        private IEnumerator dialogueSequence()
        {
            
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<DialogueLine>().playDialogue();
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
            
            turnsManager.EndCutscene();
        }

        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public void OnNotify(GameEvents gameEvent)
        {
            
            if (gameEvent == interestedGameEvent)
            {
                Debug.Log("Event used: " + gameEvent);
                turnsManager.StartCutscene();
                
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    transform.parent.GetChild(i).gameObject.SetActive(false);
                }

                gameObject.SetActive(true);
                StartCoroutine(dialogueSequence());
            }
        }
    }
}

