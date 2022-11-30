using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public static BossHealthBar Instance;
    public Image bCurrentHealthBar;
    public float bCurrentHP = 20;
    public float bMaxHP = 20;



    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthBar(bMaxHP);
    }

    public void UpdateHealthBar(float nHP)
    {
        bCurrentHP = nHP;
        float ratio = bCurrentHP / bMaxHP;
        bCurrentHealthBar.rectTransform.localPosition = new Vector3(bCurrentHealthBar.rectTransform.rect.width * ratio - bCurrentHealthBar.rectTransform.rect.width, 0, 0);
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SetMaxHealth(float max)
    {
        bMaxHP += (int)(bMaxHP * max / 100);
        UpdateHealthBar(bMaxHP);
    }
}
