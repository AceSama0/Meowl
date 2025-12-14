using System.Collections;
using UnityEngine;

public class DeadAnimation : MonoBehaviour
{
    [SerializeField] GameObject[] Animation;
    [SerializeField] GameObject Pause;
    void Awake()
    {
        
    }
    void Start()
    {
        StartCoroutine(RandomDeadAnimation());
    }

    
    void Update()
    {
        
    }
    IEnumerator RandomDeadAnimation()
    {
        Animation[UnityEngine.Random.Range(0, Animation.Length)].SetActive(true);
        SoundEffectManager.Play("succesed");
        yield return new WaitForSeconds(3);
        Pause.SetActive(true);
    }
}
