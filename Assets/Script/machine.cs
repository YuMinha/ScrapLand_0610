using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEditor;


namespace Controller
{
    public class Machine : MonoBehaviour
    {
        private ThirdPersonCamera cameraScript;

        //�� ��ġ
        public GameObject handPos; 
        //�� �� �־����� Ȯ��
        public TextMeshProUGUI countText; //��Ʈ

        //����
        public GameObject pt_thread; //��Ʈ��

        //UI
        public GameObject loading_ui;//���ۺ��� �ε� UI
        public GameObject finish_ui; //��� �ϼ� UI

        private int pt_deleteCount = 0;
        private bool ptthread = false;

        private void Start()
        {
            cameraScript = GetComponent<ThirdPersonCamera>();

            if (cameraScript == null)
            {
                Debug.LogError("ThirdPersonCamera ��ũ��Ʈ�� ã�� �� �����ϴ�!");
            }

            if (handPos == null)
            {
                Debug.LogWarning("handPos�� �������� �ʾҽ��ϴ�.");
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // ��Ŭ�� ����
            {
                TryDeleteHeldObject();
            }
        }

        private void TryDeleteHeldObject()
        {
            string item_name = " ";
            if (cameraScript == null)
                return;

            Ray ray = new Ray(cameraScript.transform.position, cameraScript.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, cameraScript.m_RaycastDistance))
            {
                string heldTag = "����";

                // Ŭ���� ������Ʈ�� machine �±��� ���� ����
                if (hit.collider.CompareTag("machine"))
                {
                    //���� ������� ������, ptthread(��Ʈ��)�� ��������� ���� �����϶�
                    if (handPos != null && handPos.transform.childCount > 0 && ptthread == false)
                    {
                        Transform child = handPos.transform.GetChild(0);
                        heldTag = child.tag;

                        if (heldTag == "pt") //�տ� pt�� ��� ���� ��
                        {
                            Destroy(child.gameObject); //������Ʈ ����
                            
                            pt_deleteCount++; //�� �� �־����� ī��Ʈ
                            UpdateText(); //UI ����

                            if (pt_deleteCount == 1) //��Ʈ�� ����� ��Ʈ���� �� ���� ��
                            {
                                item_name = "pt_thread"; //���� ������: ��Ʈ��
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }

                            Debug.Log($"Ŭ���� ������Ʈ: {hit.collider.gameObject.name}, �±�: {hit.collider.tag} [�տ� �ִ� ������Ʈ �±�: {heldTag}]");
                        }
                    }
                    else
                    {
                        Debug.Log($"Ŭ���� ������Ʈ: {hit.collider.gameObject.name}, �±�: {hit.collider.tag} [�տ� �ִ� ������Ʈ ����]");
                        if(ptthread == true && handPos != null && handPos.transform.childCount == 0) //���� ���������
                        {
                            finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            GameObject newObj = Instantiate(pt_thread, handPos.transform); //�տ� ������ ����
                        }
                    }
                }
                Debug.Log($"Ŭ���� ������Ʈ: {hit.collider.gameObject.name}, �±�: {hit.collider.tag} [�տ� �ִ� ������Ʈ �±�: {heldTag}]");

            }
        }

        private void UpdateText()
        {
            if (countText != null)
            {
                countText.text = $"{pt_deleteCount}/1";
            }
        }

        IEnumerator DelayTime(string iteam_name) //��� ���� �ð�
        {
            loading_ui.SetActive(true); //�ε� uiȰ��ȭ
            yield return new WaitForSeconds(2.0f); // 2�� ���� ���
            Debug.Log("2�� �� ȣ���");
            loading_ui.SetActive(false);//�ε� ui ��Ȱ��ȭ

            if(iteam_name == "pt_thread") //���� ��Ʈ�� �ϼ��̶��
            {
                pt_deleteCount = 0; //0���� ��������
                UpdateText(); //UI ����

                ptthread = true; //������ �غ� �Ϸ�
                finish_ui.SetActive(true); //�ϼ� uiȰ��ȭ
                Debug.Log($"ptthread: {ptthread}");
                
            }
        }

    }
}
