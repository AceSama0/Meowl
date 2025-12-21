using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsStarter : MonoBehaviour
{
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CreditsActive());
        }
    }

    IEnumerator CreditsActive()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Credits");
    }
}
