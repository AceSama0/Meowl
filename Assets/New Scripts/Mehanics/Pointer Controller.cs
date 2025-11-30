using System;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] Transform pointA, pointB;
    [SerializeField] RectTransform safeZone;
    [SerializeField] float movementSpeed = 100f;

    private float direction = 1;
    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private float duration = 10f;
    public bool success = false;
    int currentSuccessCount;
    public int successCount;
    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        pointerTransform.position = Vector2.MoveTowards(pointerTransform.position, targetPosition, movementSpeed * Time.deltaTime);


        if (Vector2.Distance(pointerTransform.position, pointA.position) < 0.1f)
        {
            targetPosition = pointB.position;
            direction *= -1;
        }
        else if (Vector2.Distance(pointerTransform.position, pointB.position) < 0.1f)
        {
            targetPosition = pointA.position;
            direction *= -1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && successCount != currentSuccessCount)
        {
            if (CheckSuccess() == true)
            {
                currentSuccessCount++;
            }
            else
            {
                success = false;
            }
        }
        else if (currentSuccessCount == successCount)
        {
            canvas.SetActive(false);
            success = true;
        }
    }

    private bool CheckSuccess()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
