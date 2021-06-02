using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singlton
    public static GameManager Instance { get; private set; }
    #endregion

    [SerializeField] private GameObject inventoryPanel;

    public Dictionary<GameObject, Health> healthContainer;
    public Dictionary<GameObject, Coin> coinContainer;
    public Dictionary<GameObject, BuffReciever> buffRecieverContainer;
    public Dictionary<GameObject, ItemComponent> itemsConteiner;
    [HideInInspector] public PlayerInventory inventory;
    public ItemBase itemDataBase;

    private void Awake()
    {
        Instance = this;
        healthContainer = new Dictionary<GameObject, Health>();
        coinContainer = new Dictionary<GameObject, Coin>();
        buffRecieverContainer = new Dictionary<GameObject, BuffReciever>();
        itemsConteiner = new Dictionary<GameObject, ItemComponent>();
    }

    public void OnClickPause()
    {

        if (Time.timeScale > 0)
        {
            inventoryPanel.gameObject.SetActive(true);
            Time.timeScale = 0;
            SoundManager.PlaySound("pause");
        }
            
        else
        {
            inventoryPanel.gameObject.SetActive(false); 
            Time.timeScale = 1;
            
        }
    }

 }
  
