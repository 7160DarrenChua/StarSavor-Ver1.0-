using NovaLen;
using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static NovaLen.DialogueSystem; 
using static System.Net.Mime.MediaTypeNames;
using Color = System.Drawing.Color;

//Namespace
namespace NovaLen
{
    public class DialogueSystem : MonoBehaviour
    {
        [TextArea]
        public string toolInfoA = "This is a canvas specifically meant for displaying dialogue" +
            " between two characters showing a single character talking at a time. Press [Space or Enter Key] to progress through the dialogue.";
        [TextArea]
        public string toolInfoB = "This is a canvas specifically meant for displaying dialogue" +
            " between two characters were both character profile images will be displayed at the " +
            "same time on opposite ends sharing a dialogue box. Press [Space or Enter Key] to progress through the dialogue.";
        [TextArea]
        public string toolInfoC = "This is a canvas specifically meant for displaying dialogue" +
            " between two characters were character profile images will be displayed within the dialogue box," +
            " and showing a single character talking at a time. Press [Space or Enter Key] to progress through the dialogue.";

        public enum DialogueCanvasStyle { A, B, C };
        public DialogueCanvasStyle dialogueStyle;
        public enum EntryAnimationStyle { FadeIn, SlideUp, PopIn };
        public EntryAnimationStyle entryAnimation;
        public enum ExitAnimationStyle { FadeOut, SlideDown, PopOut };
        public ExitAnimationStyle exitAnimation;

        public GameObject StyleGroup;

        public Animator animator;
        private bool isOn = false;

        public TextMeshProUGUI nameText;
        public string name1;
        public GameObject profileImage1, profileImage2;
        public string name2;
        public TextMeshProUGUI[] nameTextArray;
        public GameObject[] profileImageArray;

        public int dialogueArrayIndex = 0;
        public float typingSpeed;
        public TextMeshProUGUI dialogueText;
        [TextArea]
        public string[] dialogueArray;
        public int[] characterSwapPoints;

        private bool typeA = false;
        private bool change = false;
        private bool typeB = false;
        private bool typeC = false;


        public GameObject nextButton;
        public int clickCount = 0;
        // Sound Effects
        public AudioSource source;
        public AudioClip[] clips;

        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.SetInteger("Dialogue Line", clickCount);
            dialogueText.text = "";

            ActivateCanvas();

        }

        #region Animation Events

        /// <summary>
        /// Plays sound effects at specific point in animation using animation event 
        /// </summary>
        public void PlaySFX()
        {

        }

        public void ResetProfileImages()
        {
            change = false;
        }

        #endregion

        /// <summary>
        /// Deactivate canvas at specific point in animation using animation event 
        /// </summary>
        public void ActivateCanvas()
        {
            isOn = true;
            animator.SetTrigger("Entry");
            StyleGroup.SetActive(true);
            clickCount = 0;
            dialogueArrayIndex = 0;
            StartCoroutine(Typing());

            switch (entryAnimation)
            {
                case EntryAnimationStyle.FadeIn:
                    animator.SetTrigger("Fade In");
                    break;
                case EntryAnimationStyle.SlideUp:
                    animator.SetTrigger("Slide Up");
                    break;
                case EntryAnimationStyle.PopIn:
                    animator.SetTrigger("Pop In");
                    break;
                default:
                    animator.SetTrigger("Pop In");
                    break;
            }

            switch (dialogueStyle)
            {
                case DialogueCanvasStyle.A:
                    typeA = true;
                    nameText.text = name1;
                    break;

                case DialogueCanvasStyle.B:
                    typeB = true;
                    nameTextArray[0].text = name1;
                    nameTextArray[1].text = name2;
                    break;

                case DialogueCanvasStyle.C:
                    typeC = true;
                    nameTextArray[0].text = name1;
                    nameTextArray[1].text = name2;
                    break;

                default:
                    typeA = true;
                    break;
            }
        }

        /// <summary>
        /// Turns off the dialogue 
        /// </summary>
        public void zeroText()
        {
            clickCount = 0;
            dialogueArrayIndex = 0;
            dialogueText.text = "";

            animator.SetTrigger("Exit");
            switch (exitAnimation)
            {
                case ExitAnimationStyle.FadeOut:
                    animator.SetTrigger("Fade Out");
                    break;
                case ExitAnimationStyle.SlideDown:
                    animator.SetTrigger("Slide Down");
                    break;
                case ExitAnimationStyle.PopOut:
                    animator.SetTrigger("Pop Out");
                    break;
                default:
                    animator.SetTrigger("Pop Out");
                    break;
            }
            StopAllCoroutines();
        }

        public void NextLine()
        {
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

            //Style type A
            if (typeA == true || typeC == true)
            {
                //When the player clicks the next button this loop is repsonsible for changing
                for (int i = 0; i != characterSwapPoints.Length; i++)
                {
                    if (dialogueArrayIndex == characterSwapPoints[i])
                    {
                        change = !change;
                    }

                    /*Change to person number two (Raspberry) in the scene, else
                    change to person number one (Mango) in the scene*/

                    if (change == true)
                    {
                        if (typeA)
                        {
                            profileImage1.SetActive(false);
                            profileImage2.SetActive(true);
                            nameText.text = name2;
                        }

                        if (typeC)
                        {
                            profileImageArray[0].SetActive(false);
                            profileImageArray[1].SetActive(true);
                            nameTextArray[0].text = name1;
                            nameTextArray[1].text = name2;
                        }
                    }
                    else
                    {
                        if (typeA)
                        {
                            profileImage1.SetActive(true);
                            profileImage2.SetActive(false);
                            nameText.text = name1;
                        }

                        if (typeC)
                        {
                            profileImageArray[0].SetActive(true);
                            profileImageArray[1].SetActive(false);
                            nameTextArray[0].text = name1;
                            nameTextArray[1].text = name2;
                        }

                    }

                }
            }

            //Style type B
            if (typeB)
            {
                nameTextArray[0].text = name1;
                nameTextArray[1].text = name2;
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

            if (isOn)
            {
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
        }

        private void Update()
        {
            animator.SetInteger("Dialogue Line", clickCount);
            NextLinePressed();
        }
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(DialogueSystem))]
    public class DialogueSystemEditor : Editor
    {
        #region SerializedProperties

        SerializedProperty dialogueStyle;
        SerializedProperty entryAnimation;
        SerializedProperty exitAnimation;

        SerializedProperty animator;
        SerializedProperty StyleGroup;


        SerializedProperty name1;
        SerializedProperty name2;
        SerializedProperty nameText;
        SerializedProperty nameTextArray;

        SerializedProperty profileImage1;
        SerializedProperty profileImage2;
        SerializedProperty profileImageArray;

        SerializedProperty dialogueArray;
        SerializedProperty dialogueText;
        SerializedProperty characterSwapPoints;

        SerializedProperty typingSpeed;
        SerializedProperty change;

        SerializedProperty nextButton;
        SerializedProperty clickCount;

        SerializedProperty source;
        SerializedProperty clips;

        bool characterNameGroup = false;

        SerializedProperty toolInfoA;
        SerializedProperty toolInfoB;
        SerializedProperty toolInfoC;
        #endregion

        private void OnEnable()
        {
            toolInfoA = serializedObject.FindProperty("toolInfoA");
            toolInfoB = serializedObject.FindProperty("toolInfoB");
            toolInfoC = serializedObject.FindProperty("toolInfoC");

            dialogueStyle = serializedObject.FindProperty("dialogueStyle");
            entryAnimation = serializedObject.FindProperty("entryAnimation");
            exitAnimation = serializedObject.FindProperty("exitAnimation");

            animator = serializedObject.FindProperty("animator");
            StyleGroup = serializedObject.FindProperty("StyleGroup");

            profileImage1 = serializedObject.FindProperty("profileImage1");
            profileImage2 = serializedObject.FindProperty("profileImage2");
            profileImageArray = serializedObject.FindProperty("profileImageArray");

            name1 = serializedObject.FindProperty("name1");
            name2 = serializedObject.FindProperty("name2");
            nameText = serializedObject.FindProperty("nameText");
            nameTextArray = serializedObject.FindProperty("nameTextArray");

            typingSpeed = serializedObject.FindProperty("typingSpeed");
            dialogueText = serializedObject.FindProperty("dialogueText");
            dialogueArray = serializedObject.FindProperty("dialogueArray");
            characterSwapPoints = serializedObject.FindProperty("characterSwapPoints");

            nextButton = serializedObject.FindProperty("nextButton");
            clickCount = serializedObject.FindProperty("clickCount");

            change = serializedObject.FindProperty("change");

            source = serializedObject.FindProperty("source");
            clips = serializedObject.FindProperty("clips");

        }

        public override void OnInspectorGUI()
        {
            DialogueSystem dialogueSystem = (DialogueSystem)target;

            serializedObject.Update();

            if (dialogueSystem.dialogueStyle == DialogueSystem.DialogueCanvasStyle.A) EditorGUILayout.PropertyField(toolInfoA);
            if (dialogueSystem.dialogueStyle == DialogueSystem.DialogueCanvasStyle.B) EditorGUILayout.PropertyField(toolInfoB);
            if (dialogueSystem.dialogueStyle == DialogueSystem.DialogueCanvasStyle.C) EditorGUILayout.PropertyField(toolInfoC);

            EditorGUILayout.LabelField("- Animation and Style Settings -", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(dialogueStyle);
            EditorGUILayout.PropertyField(entryAnimation);
            EditorGUILayout.PropertyField(exitAnimation);

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("- Dialogue Settings -", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            switch (dialogueSystem.dialogueStyle)
            {
                case DialogueSystem.DialogueCanvasStyle.A:
                    EditorGUILayout.PropertyField(StyleGroup);
                    characterNameGroup = EditorGUILayout.BeginFoldoutHeaderGroup(characterNameGroup, "Character Names Settings");
                    if (characterNameGroup)
                    {
                        EditorGUILayout.PropertyField(nameText);
                        EditorGUILayout.PropertyField(profileImage1);
                        EditorGUILayout.PropertyField(name1);
                        EditorGUILayout.PropertyField(profileImage2);
                        EditorGUILayout.PropertyField(name2);
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();

                    EditorGUILayout.Space(10);

                    EditorGUILayout.PropertyField(typingSpeed);
                    EditorGUILayout.PropertyField(dialogueText);
                    EditorGUILayout.PropertyField(dialogueArray);
                    EditorGUILayout.PropertyField(characterSwapPoints);

                    EditorGUILayout.PropertyField(nextButton);
                    EditorGUILayout.PropertyField(clickCount);
                    //EditorGUILayout.PropertyField(change);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("- Sound Settings -", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(source);
                    EditorGUILayout.PropertyField(clips);
                    break;

                case DialogueSystem.DialogueCanvasStyle.B:
                    EditorGUILayout.PropertyField(StyleGroup);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("Character Names Settings", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(name1);
                    EditorGUILayout.PropertyField(name2);
                    EditorGUILayout.PropertyField(nameTextArray);
                    EditorGUILayout.PropertyField(profileImageArray);

                    EditorGUILayout.Space(10);

                    EditorGUILayout.PropertyField(typingSpeed);
                    EditorGUILayout.PropertyField(dialogueText);
                    EditorGUILayout.PropertyField(dialogueArray);

                    EditorGUILayout.PropertyField(nextButton);
                    EditorGUILayout.PropertyField(clickCount);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("- Sound Settings -", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(source);
                    EditorGUILayout.PropertyField(clips);
                    break;


                case DialogueSystem.DialogueCanvasStyle.C:
                    EditorGUILayout.PropertyField(StyleGroup);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("Character Names Settings", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(name1);
                    EditorGUILayout.PropertyField(name2);
                    EditorGUILayout.PropertyField(nameTextArray);
                    EditorGUILayout.PropertyField(profileImageArray);

                    EditorGUILayout.Space(10);

                    EditorGUILayout.PropertyField(typingSpeed);
                    EditorGUILayout.PropertyField(dialogueText);
                    EditorGUILayout.PropertyField(dialogueArray);
                    EditorGUILayout.PropertyField(characterSwapPoints);

                    EditorGUILayout.PropertyField(nextButton);
                    EditorGUILayout.PropertyField(clickCount);
                    //EditorGUILayout.PropertyField(change);

                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("- Sound Settings -", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5);
                    EditorGUILayout.PropertyField(source);
                    EditorGUILayout.PropertyField(clips);
                    break;
            }


            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}