using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class EnteringCutScene : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] Transform cameraFollow;   
    [SerializeField] Transform cameraTarget;   
    [SerializeField] Transform cameraTarget1;  
    [SerializeField] Transform cameraTarget2;  


    [Header("Settings")]
    [SerializeField] float moveSpeed = 5f; 
    [SerializeField] float waitDuration = 2f;   
    [SerializeField] string doorSoundName = "door"; 

    private CinemachineCamera vCam;
    private Player player;
    private WaitForSeconds wait; 

    void Awake()
    {
        vCam = FindAnyObjectByType<CinemachineCamera>();
        player = FindAnyObjectByType<Player>();


        wait = new WaitForSeconds(waitDuration);
    }

    void Start()
    {

        if (vCam != null && player != null && cameraFollow != null)
        {
            StartCoroutine(StartCutScene());
        }
    }

    IEnumerator StartCutScene()
    {
        player.canMove = false;
        vCam.Follow = cameraFollow;

        cameraFollow.position = player.transform.position;

        yield return wait; 

        yield return StartCoroutine(Move(cameraTarget));
        yield return wait;

        yield return StartCoroutine(Move(cameraTarget1));
        yield return wait;

        yield return StartCoroutine(Move(cameraTarget2));
        yield return wait;

        SoundEffectManager.Play(doorSoundName);

        
        vCam.Follow = player.transform;

        player.canMove = true;
        enabled = false;
    }

    IEnumerator Move(Transform target)
    {
        while (Vector3.Distance(cameraFollow.position, target.position) > 0.05f)
        {    
            cameraFollow.position = Vector3.MoveTowards(
                cameraFollow.position,
                target.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        cameraFollow.position = target.position;
    }
}