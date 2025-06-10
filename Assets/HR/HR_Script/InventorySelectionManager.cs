using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySelectionManager : MonoBehaviour
{
    public static InventorySelectionManager Instance;
    public static GameObject SelectedSlot { get; private set; }
    public static InventoryUIManager SelectedInventoryUI { get; private set; }

    // �տ� ���
    public TextMeshProUGUI DebugText; //����׿� �ؽ�Ʈ
    public GameObject Handpos; //�տ� ��� ��ġ ������Ʈ

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

        Instance.OnSlotClicked();
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

    //�տ� ���
    public void OnSlotClicked()
    {
        if (SelectedSlot != null)
        {
            InventoryItem item = SelectedSlot.GetComponent<Slot>().GetItem();
            Sprite sprite = item.GetItemImage(); // InventoryItem���� ������ �̹���
            if (item != null && !item.GetIsNull())
            {
                DebugText.text = $"���õ� ������: {item.GetItemType()}";

                // 1. �������� ��Ƽ������ �ִٸ� �ٷ� ����
/*                if (item.itemMaterial != null)
                {
                    Handpos.GetComponent<MeshRenderer>().material = item.itemMaterial;
                }*/
                // 2. �������� ��������Ʈ�� �ִٸ�, �ؽ�ó�� ��ȯ�ؼ� ��Ƽ���� ����
                if (sprite != null)
                {
                    Material mat = Handpos.GetComponent<MeshRenderer>().material;
                    mat.mainTexture = sprite.texture;
                    // �ʿ��ϴٸ� mat.color = Color.white; �� �߰�
                }
                // 3. �����ۿ� �̹����� ������ �⺻ ��Ƽ�����
                else
                {
                    // �⺻ ��Ƽ����� �����ϰų�, ��Ȱ��ȭ ��
                }
            }
            else
            {
                DebugText.text = "���õ� ���Կ� �������� �����ϴ�.";
                // �������� ������ Handpos�� ���ų� �⺻ ��Ƽ�����
            }
        }
        else
        {
            Debug.Log("���õ� ������ �����ϴ�.");
        }
    }
}
