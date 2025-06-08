using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

public class InventorySelectionManager : MonoBehaviour
{
    public static InventorySelectionManager Instance;
    public static GameObject SelectedSlot { get; private set; }
    public static InventoryUIManager SelectedInventoryUI { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SetSelection(GameObject slot, InventoryUIManager inventoryUIManager)
    {
        SelectedSlot = slot;
        SelectedInventoryUI = inventoryUIManager;

        InventoryItem item = slot.GetComponent<Slot>().GetItem();
        if (item != null)
        {
            int price = item.GetItemPrice(); // InventoryItem���� ������ ����
            ShopManager.Instance.ShopText.text = $"{price}���� �Դϴ�. \n�Ǹ��Ͻðڽ��ϱ�?";
        }
        else
        {
            ShopManager.Instance.ShopText.text = $"�������� �����ϴ�.";
        }
    }

    public static void ClearSelection()
    {
        SelectedSlot = null;
        SelectedInventoryUI = null;
    }


    //��������Ʈ ����

    // ������ Ÿ�� -> �Ǹ� Ƚ��
    private Dictionary<string, int> sellCounts = new Dictionary<string, int>();

    public int GetSellCount(string itemType)
    {
        if (sellCounts.TryGetValue(itemType, out int count))
            return count;
        return 0;
    }

    public void IncrementSellCount(string itemType)
    {
        if (sellCounts.ContainsKey(itemType))
            sellCounts[itemType]++;
        else
            sellCounts[itemType] = 1;
    }
}
