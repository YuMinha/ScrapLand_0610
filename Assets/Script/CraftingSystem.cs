using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class CraftMaterial
{
    public string materialName;   // ��� �̸�
    public int currentAmount;     // ���� ������
    public TextMeshProUGUI text;  // ��� UI �ؽ�Ʈ
}

[System.Serializable]
public class CraftableItem
{
    public string itemName;       // ���� ������ �̸�
    [System.Serializable]
    public struct MaterialRequirement
    {
        public CraftMaterial material; // ���
        public int requiredAmount;     // �ʿ䷮
    }
    public List<MaterialRequirement> requiredMaterials; // �ʿ��� ��� ����Ʈ
    public Button craftButton;     // ���� ��ư
}

public class CraftingSystem : MonoBehaviour
{
    // ��� ������ (Inspector���� ����)
    public CraftMaterial[] materials;

    // ���� ������ ������ (Inspector���� ����)
    public CraftableItem[] craftableItems;

    void Start()
    {
        // UI �� ��ư ���� �ʱ�ȭ
        UpdateUI();
        UpdateButtonStates();

        // ��ư �̺�Ʈ ����
        for (int i = 0; i < craftableItems.Length; i++)
        {
            int index = i;
            craftableItems[i].craftButton.onClick.AddListener(() => CraftItem(index));
        }
    }

    // UI ������Ʈ
    void UpdateUI()
    {
        // ��� UI ������Ʈ
        foreach (var material in materials)
        {
            int maxRequired = 0;
            foreach (var item in craftableItems)
            {
                foreach (var req in item.requiredMaterials)
                {
                    if (req.material.materialName == material.materialName)
                    {
                        maxRequired = Mathf.Max(maxRequired, req.requiredAmount);
                    }
                }
            }
            material.text.text = $"{material.materialName} {material.currentAmount}/{maxRequired}";
        }

        // ���� ������ UI ������Ʈ
        for (int i = 0; i < craftableItems.Length; i++)
        {
            foreach (var req in craftableItems[i].requiredMaterials)
            {
                req.material.text.text = $"{req.material.materialName} {req.material.currentAmount}/{req.requiredAmount}";
            }
        }
    }

    // ��ư ���� ������Ʈ
    void UpdateButtonStates()
    {
        for (int i = 0; i < craftableItems.Length; i++)
        {
            craftableItems[i].craftButton.interactable = CanCraftItem(craftableItems[i]);
        }
    }

    // ��� �ε��� ã��
    int GetMaterialIndex(string materialName)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].materialName == materialName) return i;
        }
        return -1;
    }

    // ���� ���� ���� Ȯ��
    bool CanCraftItem(CraftableItem item)
    {
        foreach (var req in item.requiredMaterials)
        {
            if (req.material.currentAmount < req.requiredAmount)
                return false;
        }
        return true;
    }

    // ���� ��ư Ŭ�� �� ȣ��
    public void CraftItem(int index)
    {
        CraftableItem item = craftableItems[index];
        if (CanCraftItem(item))
        {
            // ��� �Ҹ�
            foreach (var req in item.requiredMaterials)
            {
                req.material.currentAmount -= req.requiredAmount;
            }
            Debug.Log($"{item.itemName} ���� �Ϸ�!");
        }

        // UI �� ��ư ���� ������Ʈ
        UpdateUI();
        UpdateButtonStates();
    }

    // �׽�Ʈ��: ��� �߰�
    public void AddMaterial(string materialName, int amount)
    {
        int index = GetMaterialIndex(materialName);
        if (index >= 0)
        {
            materials[index].currentAmount += amount;
        }

        UpdateUI();
        UpdateButtonStates();
    }
}