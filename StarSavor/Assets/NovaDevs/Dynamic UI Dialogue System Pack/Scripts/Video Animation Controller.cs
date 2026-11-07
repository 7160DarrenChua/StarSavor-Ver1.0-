using UnityEngine;

//Namespace
namespace NovaLen
{

    public class VideoAnimationController : MonoBehaviour
    {
        [TextArea]
        public string toolInfo = "This script is responsible for keeping track of the time left on a video/animation that is being played. When the timer is greater than " +
            "the limit then another function or action can be called. Look in the script to add further code for specific outcome after the time limit is reached.";

        public float timer = 0f;
        public float limit;

        /// <summary>
        /// Activate a timer till the animation ends and the system loads the next level
        /// </summary>
        private void Update()
        {
            timer += Time.deltaTime;

            while (timer < limit)
            {
                timer += Time.deltaTime;
                break;
            }

            if (timer > limit)
            {

                /*Go to next scene*/


            }

        }

    }
}