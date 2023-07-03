using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; protected set; }

        protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, Color textColor, float delay, AudioClip sound)
        {
            if (sound == null)
            {
                Debug.Log("sound is null");
                yield break;
            }
            textHolder.color = textColor;

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                DialogueAudioManager.Instance.PlaySound(sound);
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
        }
    }
}


