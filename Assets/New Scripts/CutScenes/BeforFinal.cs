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
        mapTransition.doorCanOpen = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(CutScene());
    }

    IEnumerator CutScene()
    {
        yield return new WaitForSeconds(2);
        spriteRenderer.enabled = false;
        SoundEffectManager.Play("door");
        yield return new WaitForSeconds(1);
        NextCutScene.SetActive(true);
        Destroy(gameObject);
    }
}
