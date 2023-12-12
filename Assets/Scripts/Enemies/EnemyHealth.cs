using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KnockBack))]
[RequireComponent(typeof(Flash))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int startingHealth = 3;

    [SerializeField]
    private GameObject deathVfxPrefab;
    private int currentHealth;
    private KnockBack knockBack;
    private Flash flash;

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, currentHealth);
        knockBack.GetKnockedBack(PlayerController.Instance.transform, 15f);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckForDeathRoutine());
    }

    private IEnumerator CheckForDeathRoutine()
    {
        yield return new WaitForSeconds(flash.FlashDuration);
        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
