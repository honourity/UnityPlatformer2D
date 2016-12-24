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
                for (int i = previousHP; i < Player.HealthCurrent; i++)
                {
                    //health has increased, so set full hearts
                    hearts[i].sprite = FullHeart;
                }
            }
            else
            {
                for (int i = Player.HealthCurrent - 1; i <= previousHP; i++)
                {
                    //health has dropped so set empty hearts
                    hearts[i].sprite = EmptyHeart;
                }
            }
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