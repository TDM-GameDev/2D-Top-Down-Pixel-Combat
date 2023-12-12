using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField]
    private Material whiteFlashMaterial;

    [SerializeField]
    private float flashDuration = .15f;
    public float FlashDuration => flashDuration;

    private Material defaultMaterial;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = defaultMaterial;
    }
}
