using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class HealthBarShrink : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private Image barImage;
    [SerializeField] private Image damageBarImage;

    [Header("Options")]
    public bool hideWhenFull = false;
    public float shrinkSpeed = 10f;
    public float dameShrinkTimerMax = 1f;

    private float damageShrinkTimer;
    private float targetFill = 1f;

    private void Awake()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
    }

    private void Start()
    {
        enemyAI.OnHealthChanged += SetHealth;
    }

    private void Update()
    {
        damageShrinkTimer -= Time.deltaTime;

        barImage.fillAmount = targetFill;
        if (damageShrinkTimer <= 0f)
        {
            if(barImage.fillAmount < damageBarImage.fillAmount)
            {
                damageBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
           // damageBarImage.fillAmount = Mathf.MoveTowards(damageBarImage.fillAmount, targetFill, shrinkSpeed * Time.deltaTime);
        }
        //barImage.fillAmount = Mathf.MoveTowards(barImage.fillAmount, targetFill, shrinkSpeed * Time.deltaTime);
    }

    private void SetHealth(int currentHealth, int maxHealth)
    {
        damageShrinkTimer = dameShrinkTimerMax;

        targetFill = (float)currentHealth / maxHealth;

        if(hideWhenFull)
        {
            gameObject.SetActive( targetFill < 1f);
        }
    } 
    
    private void OnBossDeath()
    {
        enemyAI.OnHealthChanged -= SetHealth;
    }
}
