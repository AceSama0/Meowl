using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QTE : MonoBehaviour
{
    [Header("QTE")]
    [SerializeField] float QTETime;
    private bool isQTEActive = false;
    bool successed = false;
    [SerializeField] GameObject QTEEvent;
    PointerController pC;
    public bool successQTE;
    
    void Update()
    {
        // if(isQTEActive)
        // {
        //     if(pC.success)
        //     {
        //         FinishQTE();
        //     }
        // }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(QTETimer());
        }
    }
    public void QTEStarter()
    {
        isQTEActive = true;
        QTEEvent.SetActive(true);
        pC = FindAnyObjectByType<PointerController>();
    }
    IEnumerator QTETimer()
    {
        QTEStarter();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            successed = true;
        }
        yield return new WaitForSeconds(QTETime);
        if (successed)
        {
           FinishQTE(true);
           successed = false; 
        }
        else
        {
            FinishQTE(false);
        }
    }
    public void FinishQTE(bool succesful)
    {
        QTEEvent.SetActive(false);
        if (succesful)
        {
            successQTE = true;
        }
        else
        {
            SceneManager.LoadScene("KızÖlüm");
            successQTE = false;
        }
    }
}
