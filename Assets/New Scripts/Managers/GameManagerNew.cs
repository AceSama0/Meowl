using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UIElements;
public class GameManagerNew : MonoBehaviour
{
    public static GameManagerNew Instance { get; private set;}
    Player player;
    private Vector3 playerPosition;
    public bool sceneChangedVerrified;
    public bool isTouchingDoor = false;
    [SerializeField] GameObject kitchenVision;
    [SerializeField] GameObject corridorVision;
    [SerializeField] Camera camera1;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        player = FindAnyObjectByType<Player>();
        
    }

    void Start()
    {
        playerPosition = player.transform.position;
        Console.WriteLine(playerPosition + "Bulundu");
    }
    
    void Update()
    {
        // if (isTouchingDoor)
        // {
        //     if (player.transform.position.x < 3)
        //     {
        //         kitchenVision.SetActive(true);
        //         corridorVision.SetActive(false);
        //         player.transform.position = new Vector3(4.88f, -2.18f, 0);
        //         camera1.transform.position = new Vector3(10.34f, -2.18f, 0);
        //         camera1.orthographicSize = 3.69f; 
        //     }
        //     else
        //     {
        //         kitchenVision.SetActive(false);
        //         corridorVision.SetActive(true);
        //         player.transform.position = new Vector3(2f, -2.18f, 0);
        //         camera1.transform.position = new Vector3(2f, -2.18f, 0);
        //         camera1.orthographicSize = 5.69f; 
        //     }
        // } 
        // isTouchingDoor = false;
    }
}
