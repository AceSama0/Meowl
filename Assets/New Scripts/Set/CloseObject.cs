using UnityEngine;

public class CloseObject : MonoBehaviour
{
    [SerializeField] GameObject brokenMirror;
    void Start()
    {
        Destroy(brokenMirror);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
