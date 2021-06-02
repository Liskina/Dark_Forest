using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public int coinsCount;
    [SerializeField] private Text coinsText;
    public BuffReciever buffReciever;
    private List<Item> items;

    public List<Item> Items
    {
        get { return items; }
    }

    private void Start()
    {
        GameManager.Instance.inventory = this;
        coinsText.text = coinsCount.ToString();
        items = new List<Item>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.coinContainer.ContainsKey(col.gameObject))
        {
            coinsCount++;
            coinsText.text = "" + coinsCount;
            var coin = GameManager.Instance.coinContainer[col.gameObject];
            coin.StartDestroy();
        }

        if (GameManager.Instance.itemsConteiner.ContainsKey(col.gameObject))
        {
            var itemComponent = GameManager.Instance.itemsConteiner[col.gameObject];
            items.Add(itemComponent.Item);
            itemComponent.Destroy(col.gameObject);
            SoundManager.PlaySound("getPotion");
        }
    }
}
