using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    private Player player;
    private Image bar;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        bar = gameObject.GetComponent<Image>();
    }

        void Update()
        {
        bar.fillAmount = player.getStress();
        }
    }

