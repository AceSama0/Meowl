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
    IEnumerator RandomDeadAnimation()
    {
        Animation[UnityEngine.Random.Range(0, Animation.Length)].SetActive(true);
        SoundEffectManager.Play("succesed");
        yield return new WaitForSeconds(1);
        Pause.SetActive(true);
    }
}
