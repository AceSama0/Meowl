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
    [SerializeField] GameObject MouseIcon;

    void Update()
    {
        if(isQTEActive)
        {
            if(pC.success)
            {
                FinishQTE(true);
            }
            else
            {
                FinishQTE(false);
            }
        }
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
        MouseIcon.SetActive(true);
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
        MouseIcon.SetActive(false);
    }
    public void FinishQTE(bool succesful)
    {
        QTEEvent.SetActive(false);
        if (succesful)
        {
            successQTE = true;
            Debug.Log("QTE başarılı");
        }
        else
        {
            SceneManager.LoadScene("KızÖlüm");
            successQTE = false;
            Debug.Log("QTE başarısız");
        }
    }
}
