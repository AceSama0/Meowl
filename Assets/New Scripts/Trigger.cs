using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] string tagFilter;
    [SerializeField] UnityEvent onTriggerEnter2D;
    [SerializeField] UnityEvent onTriggerExit2D;
    [SerializeField] bool destroyObjectBool;


    void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter2D.Invoke();

        if (!String.IsNullOrEmpty(tagFilter) && !collision.gameObject.CompareTag(tagFilter)) return;

        if (destroyObjectBool)
        {
            Destroy(gameObject);
        }        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!String.IsNullOrEmpty(tagFilter) && !collision.gameObject.CompareTag(tagFilter)) return;

        onTriggerExit2D.Invoke();

        if (destroyObjectBool)
        {
            Destroy(gameObject);
        }
        this.gameObject.SetActive(false);
    }
}
