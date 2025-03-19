
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class BarHunger : MonoBehaviour
{
    private Player player;
    private float Hunger;
    [SerializeField] private Sprite Green;
    [SerializeField] private Sprite Yellow;
    [SerializeField] private Sprite Red;
    private Image image;
        void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        image = gameObject.GetComponent<Image>();
        Debug.Log(Yellow.name);
        Hunger = player.getHunger();
        changeBar();
    }

    void Update()
    {
        if (Hunger != player.getHunger())
        {
            changeBar();
        }
    }
    private void changeBar()
    {
        Hunger = player.getHunger();
        if (Hunger > 70)
        {
            image.overrideSprite = Green;

        }
        else if (Hunger > 30)
        {
            image.overrideSprite = Yellow;
        }
        else
        {
            image.overrideSprite = Red;
        }
        image.fillAmount = Hunger / 100f;
    }
}
