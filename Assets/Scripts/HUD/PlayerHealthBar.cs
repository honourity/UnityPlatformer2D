using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Player Player;
    public Transform HeartPrefab;

    public Sprite EmptyHeart;
    public Sprite FullHeart;

    private Image[] hearts;
    private int previousHP = 0;

    void Start()
    {
        hearts = new Image[Player.HealthMax];
        CreateEmptyHearts();
    }

    void Update()
    {
        if (Player.HealthCurrent != previousHP)
        {
            if (Player.HealthCurrent > previousHP)
            {
                int tempPreviousHealth = previousHP;
                if (tempPreviousHealth < 0) tempPreviousHealth = 0;

                for (int i = tempPreviousHealth; i < Player.HealthCurrent; i++)
                {
                    //health has increased, so set full hearts
                    if (i < hearts.Length) hearts[i].sprite = FullHeart;
                }
            }
            else
            {
                int healthCurrent = Player.HealthCurrent;
                if (healthCurrent < 0) healthCurrent = 0;

                for (int i = healthCurrent; i < previousHP; i++)
                {
                    //health has dropped so set empty hearts.
                    if (i < hearts.Length) hearts[i].sprite = EmptyHeart;
                }
            }

            previousHP = Player.HealthCurrent;
        }
    }

    private void CreateEmptyHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            var heartTransform = (Transform)Instantiate(HeartPrefab, new Vector2((i * (EmptyHeart.rect.size.x + 2))+5,  transform.position.y), transform.rotation, transform);
            hearts[i] = heartTransform.GetComponent<Image>();
            hearts[i].sprite = EmptyHeart;
        }
    }
}