using System.Collections;
using UnityEngine;

public class ShowNumbers : MonoBehaviour
{
    [SerializeField] float delay = 3;
    [SerializeField] GameObject[] numbers;
    void Start()
    {
        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < numbers.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            numbers[i].SetActive(true);
        }
            
    }
}
