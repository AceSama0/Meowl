using UnityEngine;
using UnityEngine.UI;

public class LanternCount : MonoBehaviour
{
    Player player;
    Text textComponent;
    void Start()
    {
        textComponent = GetComponent<Text>();
        player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (player != null && textComponent != null) textComponent.text = "= " + player.lightCount.ToString();
    }
}
