using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound)
    {
        if (source != null && sound != null)
        {            
            source.PlayOneShot(sound);
        }
    }
}
