using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingLantern : MonoBehaviour
{
    Player player;
    PlayerInteract playerInteract;
    bool isPlayerNear;
    bool isRaiseing;
    Animator animator;
    

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        playerInteract = player.GetComponent<PlayerInteract>();
        animator = player.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("tetiklendi");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerNear = false;
    }

    void Update()
    {
        if (isPlayerNear && playerInteract.interacting && !isRaiseing)
        {
            StartCoroutine(RaiseLantern());
        }
    }

    IEnumerator RaiseLantern()
    {
        Debug.Log("Animasyon devreye girdi");
        isRaiseing = true;
        player.canMove = false;
        animator.SetBool("RaiseLanternLeft", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("RaiseLanternLeft", false);
        isRaiseing = false;
        player.canMove = true;
        player.lightTime = 40f;
        enabled = false;
    }
}
