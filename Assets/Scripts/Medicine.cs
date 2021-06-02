using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public int bonusHealth;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Enemy"))
        {
            Health health = col.gameObject.GetComponent<Health>();
            health.SetHealth(bonusHealth);
            Destroy(gameObject);
        }
    }

    public void StDestroy()
    {
        animator.SetTrigger("StDestroy");
    }

    public void EnDestroy()
    {
        Destroy(gameObject);
    }

}
