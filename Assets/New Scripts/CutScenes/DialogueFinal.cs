using System.Collections;
using UnityEngine;

public class DialogueFinal : MonoBehaviour
{
    [SerializeField] GameObject Dialogue;
    [SerializeField] float time;
    
    
    void Start()
    {
        StartCoroutine(FinalRoto());
    }

    

    IEnumerator FinalRoto()
    {
        yield return new WaitForSeconds(time);
        Dialogue.SetActive(true);
    }
}
