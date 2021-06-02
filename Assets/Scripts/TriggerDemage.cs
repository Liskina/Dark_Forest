﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDemage : MonoBehaviour
{
    [SerializeField] private int demage;
    public int Demage
    {
        get { return demage; }
        set { demage = value; }
    }

    [SerializeField] private bool isDestroyingAfterCollision;
    private IObjectDestroyer destroyer;

    private GameObject parent;
    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public void Init(IObjectDestroyer destroyer)
    {
        this.destroyer = destroyer;

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == parent)
            return;

        

        if(GameManager.Instance.healthContainer.ContainsKey(col.gameObject))
        {
            var health = GameManager.Instance.healthContainer[col.gameObject];
                health.TakeHit(demage, gameObject);
        }

        if (isDestroyingAfterCollision)
        {
            if (destroyer == null)
                Destroy(gameObject);
            else destroyer.Destroy(gameObject);
        }
        
    }

}

public interface IObjectDestroyer
{
    void Destroy(GameObject gameObject);
}
