using System.Collections;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool completed
        { get; protected set; }

        protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, Color textColor, float delay, AudioClip sound)
        {
            if (sound == null)
            {
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
            completed = true;
        }
    }
}


