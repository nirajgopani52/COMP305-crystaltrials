using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    [SerializeField] private int maxHealth = 3; // maxhealth in full hearts

    private List<GameObject> hearts;
    private int healthCount = 6;

    // Start is called before the first frame update
    void Start()
    {
        hearts = new List<GameObject>();

        healthCount = maxHealth * 2;

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = new GameObject("heart");
            RectTransform trans = newHeart.AddComponent<RectTransform>();
            trans.transform.SetParent(transform);
            trans.anchoredPosition = new Vector3(i * 32, 0, 0);
            trans.transform.localScale = new Vector3(0.25f, 0.25f, 1);

            Image newHeartImg = newHeart.AddComponent<Image>();
            newHeartImg.sprite = fullHeart;

            hearts.Add(newHeart);
        }
    }

    public void GainHeart()
    {
        maxHealth++;
        healthCount += 2;
        UpdateHealthBar();
    }

    public void Hit(int damage)
    {
        healthCount -= damage;
        UpdateHealthBar();
    }

    public void Heal(int damage)
    {
        healthCount += damage;
        if (healthCount > maxHealth * 2)
        {
            healthCount = maxHealth * 2;
        }
        UpdateHealthBar();
    }

    public void setHealth(int health)
    {
        healthCount = health;
        if (healthCount > maxHealth * 2)
        {
            healthCount = maxHealth * 2;
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        int index = 1;
        foreach (GameObject h in hearts)
        {
            Image img = h.GetComponent<Image>();

            if (index * 2 <= healthCount)
            {
                img.sprite = fullHeart;
            }
            else if (index * 2 - 1 <= healthCount)
            {
                img.sprite = halfHeart;
            }
            else
            {
                img.sprite = emptyHeart;
            }
        }
    }
}
