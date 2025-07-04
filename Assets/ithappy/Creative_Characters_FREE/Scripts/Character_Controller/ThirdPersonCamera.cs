using GLTF.Schema;
using UnityEngine;


namespace Controller
{
    public class ThirdPersonCamera : PlayerCamera
    {
        [SerializeField, Range(0f, 2f)]
        private float m_Offset = 1.5f;
        [SerializeField, Range(0f, 360f)]
        private float m_CameraSpeed = 90f;
        [SerializeField, Range(0f, 360f)]
        private float m_RotationSpeed = 100f;
        [SerializeField, Range(0f, 50f)]
        public float m_RaycastDistance = 10f;

        private Vector3 m_LookPoint;
        private Vector3 m_TargetPos;

        //UI관련(키입력하시오 이런거)
        public GameObject machine_ui;
        public GameObject breaker_ui;
        public GameObject blastFurnace_ui;
        public GameObject compressor_ui;
        public GameObject trash_ui;
        public GameObject chest_ui;
        public GameObject making_ui;
        public GameObject sewing_ui;
        public GameObject Maker_ui; 
        public GameObject Sewing_ui;
        public GameObject Shop_ui;

        private ThirdPersonCamera Camera;

        private void Start()
        {
            Camera = GetComponent<ThirdPersonCamera>();
            Maker_ui.SetActive(false);
            Sewing_ui.SetActive(false);
        }

        private void LateUpdate()
        {
            Move(Time.deltaTime);
            CheckAimTarget();
            if(PlayerInvenManager.instance.InvenMode == true)
            {
                Camera.enabled = false;
            }
            else
            {
                Camera.enabled = true;
            }
        }

        public override void SetInput(in Vector2 delta, float scroll)
        {
            base.SetInput(delta, scroll);

            var dir = new Vector3(0, 0, -m_Distance);
            var rot = Quaternion.Euler(m_Angles.x, m_Angles.y, 0f);

            var playerPos = (m_Player == null) ? Vector3.zero : m_Player.position;
            m_LookPoint = playerPos + m_Offset * Vector3.up;
            m_LookPoint += m_Transform.right * 0.3f; // 또는 m_Player.right * 0.3f
            m_TargetPos = m_LookPoint + rot * dir;
        }

        private void Move(float deltaTime)
        {
            camera();
            target();

            void camera()
            {
                var direction = m_TargetPos - m_Transform.position;
                var delta = m_CameraSpeed * deltaTime;

                if(delta * delta > direction.sqrMagnitude)
                {
                    m_Transform.position = m_TargetPos;
                }
                else
                {
                    m_Transform.position += delta * direction.normalized;
                }

                m_Transform.LookAt(m_LookPoint);
            }

            void target()
            {
                if(m_Target == null)
                {
                    return;
                }

                m_Target.position = m_Transform.position + m_Transform.forward * TargetDistance;
            }
        }

        private void CheckAimTarget()
        {
            Ray ray = new Ray(m_Transform.position, m_Transform.forward);
            RaycastHit hit;

            // 디버깅을 위해 레이 시각화
            Debug.DrawRay(m_Transform.position, m_Transform.forward * m_RaycastDistance, Color.red);

            // ItemBox 레이어 제외한 마스크 설정
            int layerMask = ~(1 << LayerMask.NameToLayer("ItemBox"));

            if (Physics.Raycast(ray, out hit, m_RaycastDistance, layerMask))
            {
                // 충돌한 오브젝트의 이름과 태그를 출력
                Debug.Log($"조준된 오브젝트: {hit.collider.gameObject.name}, 태그: {hit.collider.tag}");

                //방적기(machine) UI 
                if (hit.collider.tag == "machine")
                {
                    machine_ui.SetActive(true);
                }
                else if (hit.collider.tag == "breaker")
                {
                    breaker_ui.SetActive(true);
                }
                else if (hit.collider.tag == "BlastFurnace")
                {
                    blastFurnace_ui.SetActive(true);
                }
                else if (hit.collider.tag == "compressor")
                {
                    compressor_ui.SetActive(true);
                }
                else if (hit.collider.tag == "Trash" || hit.collider.tag == "pt" || hit.collider.tag == "glass" || hit.collider.tag == "plastic" || hit.collider.tag == "can" || hit.collider.tag == "paper")
                {
                    trash_ui.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        string itemName = hit.collider.gameObject.name;
                        Destroy(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.tag == "OldChest")
                {
                    chest_ui.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Animator animator = hit.collider.gameObject.GetComponent<Animator>();
                        animator.SetTrigger("OpenChest");
                        string itemName = hit.collider.gameObject.name;
                        Invoke("ReSetChest", 3f);
                        animator.SetTrigger("CloseChest");

                        //hr
                        StorageManager.instance.OpenStorage(); //StorageManager의 OpenStorage 메소드 호출
                        chest_ui.SetActive(false);
                        Camera.enabled = false;
                    }
                }
                else if (hit.collider.tag == "Maker")
                {
                    making_ui.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Maker_ui.SetActive(true);
                        Camera.enabled = false;
                    }
                }
                else if (hit.collider.tag == "sewing")
                {
                    sewing_ui.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Sewing_ui.SetActive(true);
                        Camera.enabled = false;
                    }
                }
                else if (hit.collider.tag == "shop")
                {
                    Shop_ui.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.Q))
                    { //판매 상점
                        ShopManager.Instance.ShopModeOn();
                        Camera.enabled = false;
                        Shop_ui.SetActive(false);
                    }
                    if (Input.GetKeyDown(KeyCode.E)) //구매 상점
                    {
                        ShopManager.Instance.BuyShopModeOn();
                        Camera.enabled = false;
                        Shop_ui.SetActive(false);
                    }
                }
                else
                {
                    machine_ui.SetActive(false);
                    breaker_ui.SetActive(false);
                    blastFurnace_ui.SetActive(false);
                    compressor_ui.SetActive(false);
                    trash_ui.SetActive(false);
                    chest_ui.SetActive(false);
                    making_ui.SetActive(false);
                    sewing_ui.SetActive(false);
                    Shop_ui.SetActive(false);
                }
            }
            else
            {
                Debug.Log("아무 오브젝트도 감지되지 않음");
                machine_ui.SetActive(false); //방적기(machine) UI 
                breaker_ui.SetActive(false);
                blastFurnace_ui.SetActive(false);
                compressor_ui.SetActive(false);
                trash_ui.SetActive(false);
                chest_ui.SetActive(false);
                making_ui.SetActive(false);
                sewing_ui.SetActive(false);
                Shop_ui.SetActive(false);
            }
        }
        
        void ReSetChest()
        {
            Debug.Log("3초 기다림");
        }
    }
       
}
