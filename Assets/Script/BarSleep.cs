using UnityEngine;
using UnityEngine.UI;
public class BarSleep : MonoBehaviour
{
    private Player player;
    private float Sleep;
    [SerializeField] private Sprite Green;
    [SerializeField] private Sprite Yellow;
    [SerializeField] private Sprite Red;
    private Image image;
        void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        image = gameObject.GetComponent<Image>();
        Debug.Log(Yellow.name);
        Sleep = player.getSleep();
    }

    void Update()
    {
        if (Sleep != player.getSleep())
        {
            Sleep = player.getSleep();
            if(Sleep > 70)
            {
                image.overrideSprite = Green;

            }
            else if (Sleep > 30)
            {
                image.overrideSprite = Yellow;
            }
            else
            {
                image.overrideSprite = Red;
            }
            image.fillAmount = Sleep / 100f;
        }
    }
}
