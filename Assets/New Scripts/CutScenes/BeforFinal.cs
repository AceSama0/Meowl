using System.Collections;
using UnityEngine;

public class BeforFinal : MonoBehaviour
{
    Player player;
    SpriteRenderer spriteRenderer;    
    [SerializeField] MapTransition mapTransition;
    [SerializeField] GameObject NextCutScene;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindAnyObjectByType<Player>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(CutScene()); 
    }
    
    IEnumerator CutScene()
    {
        player.canMove = false;
        yield return new WaitForSeconds(2);
        spriteRenderer.enabled = false;
        SoundEffectManager.Play("door");
        yield return new WaitForSeconds(1);
        player.canMove = true;
        mapTransition.doorCanOpen = true;
        NextCutScene.SetActive(true);
        Destroy(gameObject);
    }
}
