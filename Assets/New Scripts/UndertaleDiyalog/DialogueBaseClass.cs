using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace DiaglogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; private set; }
        protected IEnumerator WriteText(string input, Text textHolder,float delay, Color textColor, Font textFont, AudioClip sound)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                SoundManager.instance.PlaySound(sound);
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            
            finished = true;
        }
        
    }   
}

