using UnityEngine;

public class CorpseSound : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        SoundEffectManager.Play("Corpse");
    }
}
