using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private TextMeshProUGUI charaNameHolder;
        private TextMeshProUGUI textHolder;

        [Header("Text Options")]
        [SerializeField] private string character;
        [SerializeField] private string input;
        [SerializeField] private Color textColor;

        [Header("Time Parameter")]
        [SerializeField] private float delay;

        [Header("Sound Options")]
        [SerializeField] private AudioClip sound;

        [Header("Character Image")]
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;

        private IEnumerator lineAppear;


        private void Awake()
        {
            charaNameHolder = transform.Find("ID").gameObject.GetComponent<TextMeshProUGUI>();
            charaNameHolder.text = "";

            textHolder = GetComponent<TextMeshProUGUI>();
            textHolder.text = "";

            charaNameHolder.text = character;

            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
        }

        public void PlayDialogue()
        {
            lineAppear = WriteText(input, textHolder, textColor, delay, sound);
            StartCoroutine(lineAppear);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (textHolder.text != input)
                {
                    StopCoroutine(lineAppear);
                    textHolder.text = input;
                }
                else
                {
                    completed = true;
                }
            }
        }
    }
}

