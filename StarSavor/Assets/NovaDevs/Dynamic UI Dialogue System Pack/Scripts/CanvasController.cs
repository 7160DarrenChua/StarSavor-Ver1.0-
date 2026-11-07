using UnityEngine;

//Namespace
namespace NovaLen
{
    public class CanvasController : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [TextArea]
        public string toolInfo = "Drag the active canvas in the scene you wish to activate/test. " +
            "Press the [E Key] to activate dialogue within the scene.";
        [SerializeField]
        NovaLen.DialogueSystem dialogueSystem;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Activate Dialogue UI
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueSystem.ActivateCanvas();
            }
        }
    }
}
