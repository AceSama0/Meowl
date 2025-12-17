using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class EnteringCutScene : MonoBehaviour
{
    [SerializeField] Transform cameraFollow, cameraTarget, cameraTarget1, cameraTarget2, playerTransform;
    Player player;
    [SerializeField] float speed = 0.5f;
    private CinemachineCamera vCam;

    void Awake()
    {
        vCam = FindAnyObjectByType<CinemachineCamera>();
        player = FindAnyObjectByType<Player>();
    }
    void Start()
    {
        StartCoroutine(StartCutScene());
    }

    IEnumerator StartCutScene()
    {
        player.canMove = false;
        player.canRotate = false;
        vCam.Follow = cameraFollow;


        yield return new WaitForSeconds(2);

        yield return StartCoroutine(Move(cameraTarget));

        yield return new WaitForSeconds(2);

        yield return StartCoroutine(Move(cameraTarget1));

        yield return new WaitForSeconds(2);

        yield return StartCoroutine(Move(cameraTarget2));

        yield return new WaitForSeconds(2);

        SoundEffectManager.Play("door");

        vCam.Follow = playerTransform;
        player.canMove = true;
        player.canRotate = true;

        enabled = false;
    }

    IEnumerator Move(Transform target)
    {
        while (Vector2.Distance(cameraFollow.position, target.position) > 0.1f)
        {
            cameraFollow.position = Vector2.MoveTowards(cameraFollow.position, target.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}
