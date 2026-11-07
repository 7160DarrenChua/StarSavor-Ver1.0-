using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using Color = System.Drawing.Color;

//Namespace
namespace NovaLen
{
    public class ItemDialogueCanvas : MonoBehaviour
    {

        public Animator animator;
        public enum EntryAnimationType { FadeIn, SlideIn };
        public EntryAnimationType entryAnimationStyle;

        public enum ExitAnimationType { FadeOut, SlideOut };
        public ExitAnimationType exitAnimationStyle;

        private string exitAnimString;

        public string itemName;
        public int dialogueArrayIndex = 0;
        [TextArea]
        public string[] dialogueArray;


        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;

        public GameObject profileImages;
        public float typingSpeed;

        public int clickCount = 0;
        public GameObject nextButton;

        // Sound Effects
        public AudioSource source;
        public AudioClip[] clips;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            nameText.text = itemName;
            dialogueText.text = "";
            StartCoroutine(Typing());

            switch (entryAnimationStyle)
            {
                case EntryAnimationType.FadeIn:
                    animator.SetTrigger("Fade In");
                    break;
                case EntryAnimationType.SlideIn:
                    animator.SetTrigger("Slide In");
                    break;
                default:
                    break;
            }

            switch (exitAnimationStyle)
            {
                case ExitAnimationType.FadeOut:
                    exitAnimString = "Fade Out";
                    break;
                case ExitAnimationType.SlideOut:
                    exitAnimString = "Slide Out";
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Plays sound effects at specific point in animation using animation event 
        /// </summary>
        public void PlaySFX()
        {

        }

        /// <summary>
        /// Turns off the dialogue 
        /// </summary>
        public void zeroText()
        {

            dialogueArrayIndex = 0;
            dialogueText.text = "";
            StopAllCoroutines();
            animator.SetTrigger(exitAnimString);
        }

        public void NextLine()
        {
            string nextDialogueLine = dialogueArray[dialogueArrayIndex];
            dialogueText.text = "";

            if (dialogueArrayIndex < dialogueArray.Length - 1)
            {
                dialogueArrayIndex++;
                clickCount++;
                dialogueText.text = "";
                StartCoroutine(Typing());
            }
            else
            {
                zeroText();
            }
        }

        //Keyboard Inputs for the next button
        public void NextLinePressed()
        {
            if (nextButton.activeSelf && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.KeypadEnter)))
            {
                NextLine();
            }
        }


        /// <summary>
        /// Responsible for typing out the dialogue in realtime
        /// </summary>
        private IEnumerator Typing()
        {
            nextButton.SetActive(false);
            int num = 0;

            foreach (char letter in dialogueArray[dialogueArrayIndex].ToCharArray())
            {
                dialogueText.text += letter;
                num++;

                if (dialogueText.text.Length == dialogueArray[dialogueArrayIndex].Length)
                {
                    nextButton.SetActive(true);
                }
                yield return new WaitForSeconds(typingSpeed);


            }
        }

        private void Update()
        {
            NextLinePressed();
        }
    }

}