using UnityEngine;

public class BlockController : MonoBehaviour
{
    private bool blast = false;
    private bool machine = true; // �ʱⰪ true�� ����
    private bool compressor = false;

    private GameObject[] makingBObjects;
    private GameObject[] makingMObjects;
    private GameObject[] makingCObjects;

    void Start()
    {
        // �±� ������Ʈ�� ĳ��
        makingBObjects = GameObject.FindGameObjectsWithTag("making_B");
        makingMObjects = GameObject.FindGameObjectsWithTag("making_M");
        makingCObjects = GameObject.FindGameObjectsWithTag("making_C");

        // �ʱ� ���� üũ
        CheckBlockStates();
    }

    void Update()
    {
        // ������ ����� ��쿡�� ���� üũ (����� ������ �� ������ üũ)
        CheckBlockStates();
    }

    private void CheckBlockStates()
    {
        // blast�� true�� �� making_B �±� ������Ʈ�� Block ��Ȱ��ȭ
        if (blast)
        {
            foreach (GameObject obj in makingBObjects)
            {
                GameObject block = FindChildBlock(obj.transform);
                if (block != null && block.activeSelf) // �̹� ��Ȱ��ȭ���� ���� ��츸 ó��
                {
                    block.SetActive(false);
                    Debug.Log($"making_B Block deactivated in {obj.name}");
                }
            }
        }

        // machine�� true�� �� making_M �±� ������Ʈ�� Block ��Ȱ��ȭ
        if (machine)
        {
            foreach (GameObject obj in makingMObjects)
            {
                GameObject block = FindChildBlock(obj.transform);
                if (block != null && block.activeSelf)
                {
                    block.SetActive(false);
                    Debug.Log($"making_M Block deactivated in {obj.name}");
                }
            }
        }

        // compressor�� true�� �� making_C �±� ������Ʈ�� Block ��Ȱ��ȭ
        if (compressor)
        {
            foreach (GameObject obj in makingCObjects)
            {
                GameObject block = FindChildBlock(obj.transform);
                if (block != null && block.activeSelf)
                {
                    block.SetActive(false);
                    Debug.Log($"making_C Block deactivated in {obj.name}");
                }
            }
        }
    }

    // �ڽ� ������Ʈ���� Block�̶�� �̸��� ������Ʈ�� ã�� �޼���
    private GameObject FindChildBlock(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name == "Block")
            {
                return child.gameObject;
            }
            // ��������� �ڽ��� �ڽı��� �˻�
            GameObject block = FindChildBlock(child);
            if (block != null)
            {
                return block;
            }
        }
        return null;
    }

    // �ܺο��� ���� ������ �޼��� (�ʿ� �� ���)
    public void SetBlast(bool value) { if (blast != value) { blast = value; CheckBlockStates(); } }
    public void SetMachine(bool value) { if (machine != value) { machine = value; CheckBlockStates(); } }
    public void SetCompressor(bool value) { if (compressor != value) { compressor = value; CheckBlockStates(); } }
}