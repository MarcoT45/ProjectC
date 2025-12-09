using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color tintColor = Color.red;
    [SerializeField] private float tintFadeSpeed = 0.25f;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  
        material = spriteRenderer.material;
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
            material.SetColor("_Color", tempColor);
            yield return null;
        }
    }
}
