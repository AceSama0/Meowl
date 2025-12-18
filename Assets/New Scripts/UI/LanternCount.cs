using UnityEngine;
using UnityEngine.UI;

public class LanternCount : MonoBehaviour
{
    Player player;
    Text textComponent;
    Image image;
    [SerializeField] Sprite Image1, Image2;
    void Start()
    {
        image = GetComponentInParent<Image>();
        textComponent = GetComponent<Text>();
        player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (player.lightTime == 0) image.sprite = Image2;
        else image.sprite = Image1;
        if (player != null && textComponent != null) textComponent.text = "= " + player.lightCount.ToString();
    }
}
