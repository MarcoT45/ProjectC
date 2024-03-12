using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiController : MonoBehaviour
{
    private bool isHurt;
    private SpriteRenderer spriteRenderer;
    private Material material;
    [SerializeField] private float pushBackSpeed;
    [SerializeField] private float tintFadeSpeed;
    [SerializeField] private Color tintColor;

    public MonsterData monsterData;

    void Awake()
    {
        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
        isHurt = false;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 direction = other.transform.position - transform.position;
        if (other.gameObject.CompareTag("Player"))
        {
            isHurt = true;
            Vector3 pushBack = direction.normalized * -1;
            transform.position += pushBack;

            StartCoroutine(HitFlash());
        }
    }


    private IEnumerator HitFlash()
    {
        material.SetColor("_Tint", tintColor);
        Color tempColor;
        tempColor = tintColor;

        float time = 0f;
        while (time < tintFadeSpeed)
        {
            time += Time.deltaTime;
            tempColor.a = Mathf.Lerp(tintColor.a, 0f, (time / tintFadeSpeed));
            material.SetColor("_Tint", tempColor);
            yield return null;
        }
    }

}
