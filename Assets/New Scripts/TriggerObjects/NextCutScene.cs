using System.Collections;
using UnityEngine;

public class NextCutScene : MonoBehaviour
{
    [SerializeField] GameObject Next;
    [SerializeField] float time;
    void Start()
    {
        StartCoroutine(WaitSome());
    }

    IEnumerator WaitSome()
    {
        yield return new WaitForSeconds(time);
        Next.SetActive(true);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
