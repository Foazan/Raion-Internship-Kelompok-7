using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class BarStress : MonoBehaviour
{
    private Player player;
    private float Stress;
    [SerializeField] private Sprite Green;
    [SerializeField] private Sprite Yellow;
    [SerializeField] private Sprite Red;
    private Image image;
        void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        image = gameObject.GetComponent<Image>();
        Debug.Log(Yellow.name);
        Stress = player.getStress();
    }

    void Update()
    {
        if (Stress != player.getStress())
        {
            Stress = player.getStress();
            if(Stress > 70)
            {
                image.overrideSprite = Red;

            }
            else if (Stress > 30)
            {
                image.overrideSprite = Yellow;
            }
            else
            {
                image.overrideSprite = Green;
            }
            image.fillAmount = Stress / 100f;
        }
    }
}
