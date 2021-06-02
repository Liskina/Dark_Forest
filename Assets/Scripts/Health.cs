using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private string TheBadEnd;


    public Action<int, GameObject> OnTakeHit;
    public int CurrentHealth
    {
        get { return health; }
    }

    private void Start()
    {
        GameManager.Instance.healthContainer.Add(gameObject, this);
    }

    public void TakeHit(int demage, GameObject attacker)
    {
        health -= demage;

        if (OnTakeHit != null)
            OnTakeHit(demage, attacker);
       

        if (health <= 0)
        SceneManager.LoadScene(TheBadEnd);
    }

    public void SetHealth(int bonusHealth)
    {
        health += bonusHealth;
        SoundManager.PlaySound("yes");
    }
}
