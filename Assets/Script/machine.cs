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


        //����
        [Header("items")]
        public GameObject pt_thread; //��Ʈ��

        public GameObject breakGlass; //�� ����
        public GameObject breakPlastic; //�� �ö�ƽ
        public GameObject breakCan; //�� ĵ

        //������ UI
        [Header("machine")]
        public GameObject loading_ui;//���ۺ��� �ε� UI
        public GameObject finish_ui; //��� �ϼ� UI
        public TextMeshProUGUI countText; //��Ʈ ���� Ȯ�� UI

        //�м�� UI
        [Header("breaker")]
        public GameObject B_loading_ui;//���ۺ��� �ε� UI
        public GameObject B_finish_ui; //��� �ϼ� UI
        public TextMeshProUGUI breaker_countText; //�м�� �ܷ� UI

        //������ ��� ����
        private int pt_deleteCount = 0; //��Ʈ ���� Ȯ��
        private bool ptthread = false; //��Ʈ�� �ϼ� ����

        //�м�� ��� ����
        private int glass_deleteCount = 0; //���� ���� Ȯ��
        private int plastic_deleteCount = 0;//�ö�ƽ ���� Ȯ��
        private int can_deleteCount = 0; //ĵ ���� Ȯ��
        private bool glass_break = false; //�� ���� �ϼ� ����
        private bool plastic_break = false; //�� �ö�ƽ �ϼ� ����
        private bool can_break = false; //�� ĵ �ϼ� ����

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

            UpdateText();
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
                Transform child = null;

                // �ڽ��� ���� ���� GetChild(0) ���
                if (handPos != null && handPos.transform.childCount > 0)
                {
                    child = handPos.transform.GetChild(0);
                    heldTag = child.tag;
                }

                // Ŭ���� ������Ʈ�� machine �±��� ���� ����
                if (hit.collider.CompareTag("machine"))
                {
                    //���� ������� ������, ptthread(��Ʈ��)�� ��������� ���� �����϶�
                    if (handPos != null && handPos.transform.childCount > 0 && ptthread == false)
                    {
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
                            ptthread = false; //�̿ϼ����� ����
                            GameObject newObj = Instantiate(pt_thread, handPos.transform); //�տ� ������ ����
                        }
                    }
                }
                //Ŭ���� ������Ʈ�� �±װ� breaker�� ��
                else if (hit.collider.CompareTag("breaker"))
                {
                    //��� ��ᵵ �ϼ����� �ʾҰ� �տ� ������Ʈ�� ���� ��
                    if (handPos != null && handPos.transform.childCount > 0 && glass_break == false && plastic_break == false && can_break == false)
                    {
                        if (heldTag == "glass") //������ ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����

                            glass_deleteCount++; //���� +1
                            UpdateText(); //UI �ݿ�

                            if (glass_deleteCount == 1) //���� ����
                            {
                                item_name = "breakGlass"; //���� ������: �� ����
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                        else if (heldTag == "plastic") //�ö�ƽ�� ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����

                            plastic_deleteCount++; //�ö�ƽ +1
                            UpdateText(); //UI �ݿ�

                            if(plastic_deleteCount == 1) //����
                            {
                                item_name = "breakPlastic"; //���� ������: �� �ö�ƽ
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                        else if (heldTag == "can") //ĵ�� ��� ���� ��
                        {
                            Destroy(child.gameObject); //�տ� �ִ� ������Ʈ ����

                            can_deleteCount++; //ĵ +1
                            UpdateText(); //UI �ݿ�

                            if(can_deleteCount == 1) //����
                            {
                                item_name = "breakCan"; //���� ������: �� ĵ
                                StartCoroutine(DelayTime(item_name)); //�ڷ�ƾ ȣ��
                            }
                        }
                        else
                        {
                            Debug.Log("�ƹ��ϵ� ������.");
                        }
                    }
                    else
                    {
                        if (glass_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            glass_break = false;
                            GameObject newObj = Instantiate(breakGlass, handPos.transform); //�տ� ������ ����
                        }
                        else if (plastic_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            plastic_break = false;
                            GameObject newObj = Instantiate(breakPlastic, handPos.transform); //�տ� ������ ����
                        }
                        else if (can_break == true && handPos != null && handPos.transform.childCount == 0)
                        {
                            B_finish_ui.SetActive(false); //�ϼ�UI ��Ȱ��ȭ
                            can_break = false;
                            GameObject newObj = Instantiate(breakCan, handPos.transform); //�տ� ������ ����
                        }
                    }

                }
                Debug.Log($"Ŭ���� ������Ʈ: {hit.collider.gameObject.name}, �±�: {hit.collider.tag} [�տ� �ִ� ������Ʈ �±�: {heldTag}]");

            }
        }

        private void UpdateText()
        {
            //������ ���� UI
            countText.text = $"pt: {pt_deleteCount}/1"; //��Ʈ ����

            //�м�� ���� UI
            breaker_countText.text = $" glass: {glass_deleteCount}/1 \n plastic: {plastic_deleteCount}/1 \n can: {can_deleteCount}/1";
        }

        IEnumerator DelayTime(string item_name) //��� ���� �ð�
        {
            if(item_name == "pt_thread")
            {
                loading_ui.SetActive(true); //������ �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); // 2�� ���� ���
                loading_ui.SetActive(false);//�ε� ui ��Ȱ��ȭ

                pt_deleteCount = 0; //0���� ��������
                UpdateText(); //UI ����

                ptthread = true; //������ �غ� �Ϸ�
                finish_ui.SetActive(true); //�ϼ� uiȰ��ȭ
                Debug.Log($"ptthread: {ptthread}");
            }
            else if(item_name == "breakGlass")
            {
                B_loading_ui.SetActive(true);//�м�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                B_loading_ui.SetActive(false); //�м�� �ε� ui ��Ȱ��ȭ

                glass_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                glass_break = true; //�� ���� �غ� �Ϸ�
                B_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }
            else if (item_name == "breakPlastic")
            {
                B_loading_ui.SetActive(true);//�м�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                B_loading_ui.SetActive(false); //�м�� �ε� ui ��Ȱ��ȭ

                plastic_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                plastic_break = true; //�� �ö�ƽ �غ� �Ϸ�
                B_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }
            else if (item_name == "breakCan")
            {
                B_loading_ui.SetActive(true);//�м�� �ε� uiȰ��ȭ
                yield return new WaitForSeconds(2.0f); //2�� ���
                B_loading_ui.SetActive(false); //�м�� �ε� ui ��Ȱ��ȭ

                can_deleteCount = 0; //0���� �ٽ� �ʱ�ȭ
                UpdateText(); //ui ������Ʈ

                can_break = true; //�� ĵ �غ� �Ϸ�
                B_finish_ui.SetActive(true); //�ϼ� ui Ȱ��ȭ
            }

        }

    }
}
