using UnityEngine;
using System.Collections.Generic;

public class PlacementManager : MonoBehaviour
{
    public Transform playerHand; // �÷��̾� ��
    public LayerMask floorLayer, wallLayer, ceilingLayer;
    public float placeDistance = 0.5f;

    private GameObject heldItem;
    private GameObject previewItem;  // �̸����� ������
    private bool isPreviewActive = false;  // �̸����� ������ Ȱ��ȭ ����
    private Dictionary<string, int> itemScores = new Dictionary<string, int>()
    {
        { "Bench", 45 },
        { "Can Pot", 28 },
        { "Clock", 50 },
        { "Glass Pot", 42 },
        { "Mobile", 48 },
        { "Old Chest", 43 },
        { "Plastic Pot", 28 },
        { "Table", 52 }
    };

    void Update()
    {
        if (heldItem == null) return;

        HandleRotation();

        // �̸����� �������� Ȱ��ȭ�Ǿ� ������ ��ġ ������Ʈ
        if (isPreviewActive)
        {
            UpdatePreviewItem();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            TryPlaceItem();
        }
    }

    public void SetHeldItem(GameObject item)
    {
        heldItem = item;

        // �̸����� �������� �����ϰ� Ȱ��ȭ
        if (previewItem == null)
        {
            previewItem = Instantiate(heldItem, Vector3.zero, Quaternion.identity);
            previewItem.SetActive(true);  // �̸����� ������ Ȱ��ȭ
            isPreviewActive = true;  // �̸����� Ȱ��ȭ
            SetPreviewItemTransparency(0.3f);  // �̸����� �������� ���� ����
        }
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            heldItem.transform.Rotate(Vector3.up, -90f);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            heldItem.transform.Rotate(Vector3.up, 90f);
    }

    void UpdatePreviewItem()
    {
        if (previewItem == null) return;

        PlaceableItem item = heldItem.GetComponent<PlaceableItem>();
        if (item == null) return;

        Vector3 placePos;
        Quaternion placeRot = heldItem.transform.rotation;
        RaycastHit hit;

        previewItem.transform.rotation = placeRot;  // �̸����� �����ۿ� ��� ȸ�� ����

        // ��ġ�� ��ġ�� �����ϴ� ���� (�̸����� ������ ��ġ ����)
        switch (item.placeType)
        {
            case PlaceType.Floor:
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, floorLayer))
                {
                    if (item.name == "Old Chest")
                    {
                        placePos = hit.point + new Vector3(0, 0.5f, 0); // �ٴڿ��� ���� �� ���� ��ġ
                    }

                    else
                    {
                        placePos = hit.point + new Vector3(0, 0, 0); // �ٴڿ��� ���� �� ���� ��ġ
                    }
                    // �÷��̾�� �ʹ� ������� üũ�Ͽ� �ּ� �Ÿ� ����
                    if (Vector3.Distance(placePos, playerHand.position) < 0.5f)
                    {
                        placePos = playerHand.position + playerHand.forward * 0.5f; // �ּ� �Ÿ� 0.5�� ����
                    }
                }
                else
                {
                    return;
                }
                break;


            case PlaceType.Wall:
                placePos = Vector3.zero; // placePos�� �⺻������ �ʱ�ȭ

                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, wallLayer))
                {
                    placePos = hit.point;  // ���� ��ġ�� ��ġ

                    // ���� �°� ȸ��, ���� ��ġ������, �÷��̾ �ٶ󺸴� �������� ���� �����ϵ��� ����
                    Vector3 wallNormal = hit.normal;  // ���� ���� ����
                    Vector3 playerDirection = playerHand.forward;  // �÷��̾��� �ü� ����

                    // �÷��̾ �ٶ󺸴� ���� '��'�� ��ġ�ǵ��� ����
                    if (Vector3.Dot(wallNormal, playerDirection) < 0) // ���� ���� ������ �÷��̾� ����� �ݴ��� ��
                    {
                        placeRot = Quaternion.LookRotation(wallNormal); // ���� �������� ȸ�� (���� ����)
                    }
                    else
                    {
                        placeRot = Quaternion.LookRotation(-wallNormal); // ���� �ݴ� �������� ȸ�� (���� ����)
                    }

                    // ���� �ʹ� ��������� �ʵ��� �÷��̾��� ��ġ���� �Ÿ��� ���
                    placePos = hit.point + (wallNormal * 0.07f);  // ���� ���ʿ� ���� ��ġ (0.07f�� ����)

                    // �÷��̾�� �ʹ� ��������� �ʵ��� 0.5f �Ÿ���ŭ �о
                    float distanceToPlayer = Vector3.Distance(placePos, playerHand.position);
                    if (distanceToPlayer < 0.5f)
                    {
                        // �������� �÷��̾�� �ʹ� ��������� �ʵ��� 0.5f �Ÿ���ŭ �о
                        placePos = playerHand.position + playerHand.forward * 0.5f;  // �ּ� �Ÿ� 0.5 ����
                    }

                    // ������ ��ġ�ǵ��� ���� (���� ����� �ʰ�)
                    if (Vector3.Distance(placePos, hit.point) > placeDistance)
                    {
                        placePos = hit.point + (wallNormal * 0.07f);  // ���� ���ʿ� ��Ȯ�� ��ġ
                    }

                    // �̸����� �������� ������ ��ġ�� ȸ������ �̵�
                    if (previewItem != null)
                    {
                        previewItem.SetActive(true);  // �̸����� ������ Ȱ��ȭ
                        previewItem.transform.position = placePos;
                        previewItem.transform.rotation = placeRot;

                        // �̸����� �����۰� �÷��̾� ���� �浹�� ����
                        Collider previewItemCollider = previewItem.GetComponent<Collider>();
                        Collider playerHandCollider = playerHand.GetComponent<Collider>();

                        if (previewItemCollider != null && playerHandCollider != null)
                        {
                            Physics.IgnoreCollision(previewItemCollider, playerHandCollider, true);  // �浹 ����
                        }
                    }
                }
                else
                {
                    // ���� ���̰� ������ ������ �̸����� �������� ��Ȱ��ȭ���� �ʰ�, ��ġ�� ����
                    if (previewItem != null)
                    {
                        previewItem.SetActive(true);  // �̸����� �������� ��� Ȱ��ȭ
                        previewItem.transform.position = playerHand.position;  // ��ġ�� �÷��̾� �� ��ġ�� ����
                        previewItem.transform.rotation = playerHand.rotation;  // ȸ���� ����
                    }
                }
                break;




            case PlaceType.Ceiling:
                if (Physics.Raycast(playerHand.position, playerHand.forward, out hit, placeDistance, ceilingLayer))
                {
                    placePos = hit.point + new Vector3(0, -1.8f, 0);  // õ�忡�� ���� �� ����
                                                                      // �÷��̾�� �ʹ� ������� üũ�Ͽ� �ּ� �Ÿ� ����
                    if (Vector3.Distance(placePos, playerHand.position) < 0.5f)
                    {
                        placePos = playerHand.position + playerHand.forward * 0.5f; // �ּ� �Ÿ� 0.5�� ����
                    }
                }
                else
                    return;
                break;

            default:
                return;
        }

        // �̸����� �������� ������ ��ġ�� ȸ������ �̵�
        previewItem.transform.position = placePos;
        previewItem.transform.rotation = placeRot;

        // �̸����� �������� �ݶ��̴��� ��Ȱ��ȭ�Ͽ� �÷��̾�� �浹���� �ʰ� ��
        Collider previewCollider = previewItem.GetComponent<Collider>();
        if (previewCollider != null)
        {
            previewCollider.enabled = false;  // �ݶ��̴� ��Ȱ��ȭ
        }
    }

    void TryPlaceItem()
    {
        if (previewItem == null) return;

        PlaceableItem item = heldItem.GetComponent<PlaceableItem>();
        if (item == null) return;

        Vector3 placePos = previewItem.transform.position;
        Quaternion placeRot = previewItem.transform.rotation;

        // �̸����� �������� �ݶ��̴��� ��Ȱ��ȭ (��ġ ��)
        Collider previewCollider = previewItem.GetComponent<Collider>();
        if (previewCollider != null)
        {
            previewCollider.enabled = false;  // �ݶ��̴� ��Ȱ��ȭ
        }

        // �������� ��ġ�� ��ġ�� �ν��Ͻ�ȭ
        GameObject placedObject = Instantiate(heldItem, placePos, placeRot);
        placedObject.SetActive(true);  // ��ġ�� ������ Ȱ��ȭ

        // ��ġ �Ŀ� �ݶ��̴��� �ٽ� Ȱ��ȭ (��ġ�� ������)
        Collider placedCollider = placedObject.GetComponent<Collider>();
        if (placedCollider != null)
        {
            placedCollider.enabled = true;  // ��ġ�� �������� �ݶ��̴� Ȱ��ȭ
        }

        // heldItem�� �����ϰ� �տ� �������� ��� ���� �ʰ� ó��
        heldItem = null;

        // �̸����� ������ ����
        Destroy(previewItem);
        previewItem = null;  // �̸����� ������ �ʱ�ȭ
        isPreviewActive = false;  // �̸����� ��Ȱ��ȭ

        // ������ ��ġ ���� ������Ʈ
        UpdatePlacementInfo(item.itemName);
    }



    void UpdatePlacementInfo(string itemName)
    {
        if (itemScores.ContainsKey(itemName))
        {
            int score = itemScores[itemName];
            Debug.Log($"{itemName} ��ġ��. ����: {score}");
        }
        else
        {
            Debug.Log($"{itemName} ��ġ��. ���� ���� ����.");
        }
    }

    // �̸����� �������� ������ �����ϴ� �Լ�
    void SetPreviewItemTransparency(float alpha)
    {
        // previewItem�� Renderer �Ӹ� �ƴ϶� �ڽ� ������Ʈ�� Renderer�� �����ؾ� �ϹǷ� ��� Renderer�� ã�Ƽ� ����
        Renderer[] renderers = previewItem.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                // �������� ��� ��Ƽ������ �����ɴϴ�.
                Material[] materials = renderer.materials;

                foreach (Material material in materials)
                {
                    // ���� ����ϴ� ���̴��� ������ �����ϴ��� Ȯ��
                    if (material.HasProperty("_Color"))
                    {
                        // ������ �Ķ��� ������ ���� (RGB: 0, 0, 1�� �Ķ���)
                        Color color = material.color;
                        color = new Color(0f / 255f, 255f / 255f, 255f / 255f, alpha); // �Ķ��� (R=0, G=0, B=1) + ���� ������ ���� ����
                        material.color = color;

                        // ���İ��� ������ ��, ���̴��� ������ �����ϵ��� ����
                        material.SetFloat("_Mode", 3); // Transparent ���� ����
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.EnableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = 3000;
                    }
                }
            }
        }
    }
}
