using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance;
    public Image currentHealthBar;
    public float currentHP = 20;
    public float maxHP = 20;

   

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthBar(20);
    }

    public void UpdateHealthBar(float nHP)
    {
        currentHP = nHP;
        float ratio = currentHP / maxHP;
        currentHealthBar.rectTransform.localPosition = new Vector3(currentHealthBar.rectTransform.rect.width * ratio - currentHealthBar.rectTransform.rect.width, 0, 0);
    }

    private void Awake()
    {
        Instance = this;
    }
}
