using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakingController : MonoBehaviour
{
    //������ �ִ� ��� ����(�������� �޾ƿ���)
    public int can = 0;             //ĵ
    public int paper = 0;         // ����
    public int glass = 0;       //����
    public int plasticBottle = 0; //��Ʈ
    public int vinyl = 0;       //���
    public int plastic = 0;        // �ö�ƽ
    public int oldCloth = 0;      // �� ��

    public int aluminum = 0; // �˷�̴�
    public int compressedPaper = 0; //��������
    public int moltenGlass = 0; //���� ����
    public int plasticThread = 0;  // ��Ʈ��
    public int moltenPlastic = 0; //���� �ö�ƽ

    //���� ����
    /*���׸���*/
    private int P_PotMaiking = 0;
    private int G_PotMaiking = 0;
    private int C_PotMaiking = 0;
    private int TableMaking = 0;
    private int ChairMaking = 0;
    private int StorageBoxMaking = 0;
    private int MobileMaking = 0;
    private int ClockMaking = 0;
    /*�Ǹſ�*/
    private int KeyringMaking = 0;
    private int TongsMaking = 0;
    private int CupMaking = 0;
    private int BowlMaking = 0;

    // ��ư �Ҵ�
    [Header("���׸������ ��ư")]
    public Button p_potButton;
    public Button g_potButton;
    public Button c_potButton;
    public Button tableButton;
    public Button chairButton;
    public Button storageBoxButton;
    public Button mobileButton;
    public Button clockButton;
    [Header("�Ǹſ���� ��ư")]
    public Button keyringButton;
    public Button tongsButton;
    public Button cupButton;
    public Button bowlButton;

    //text UI
    [Header("�ö�ƽ ȭ��")]
    public TextMeshProUGUI potPt;
    private int PotPt = 1;
    public TextMeshProUGUI p_potMaking;

    [Header("ĵ ȭ��")]
    public TextMeshProUGUI potCan;
    private int PotCan = 1;
    public TextMeshProUGUI c_potMaking;

    [Header("���� ȭ��")]
    public TextMeshProUGUI potGlass;
    private int PopGlass = 1;
    public TextMeshProUGUI g_potMaking;

    [Header("���̺�")]
    public TextMeshProUGUI tableCompressedPaper;
    private int TableCompressedPaper = 4;
    public TextMeshProUGUI tableMaking;

    [Header("����")]
    public TextMeshProUGUI chairCompressedPaper;
    private int ChairCompressedPaper = 3;
    public TextMeshProUGUI chairMaking;

    [Header("������")]
    public TextMeshProUGUI storageBoxCompressedPaper;
    private int StorageBoxCompressedPaper = 2;
    public TextMeshProUGUI storageBoxCan;
    private int StorageBoxCan = 3;
    public TextMeshProUGUI storageBoxMaking;

    [Header("���")]
    public TextMeshProUGUI mobileMoltenGlass;
    private int MobileMoltenGlass = 1;
    public TextMeshProUGUI mobileVinyl;
    private int MobileVinyl = 3;
    public TextMeshProUGUI mobileCan;
    private int MobileCan = 1;
    public TextMeshProUGUI mobilePtThread;
    private int MobilePtThread = 2;
    public TextMeshProUGUI mobileMaking;

    [Header("�ð�")]
    public TextMeshProUGUI clockMoltenGlass;
    private int ClockMoltenGlass = 1;
    public TextMeshProUGUI clockCompressedPaper;
    private int ClockCompressedPaper = 2;
    public TextMeshProUGUI clockCan;
    private int ClockCan = 1;
    public TextMeshProUGUI clockMaking;

    [Header("Ű��")]
    public TextMeshProUGUI keyringMoltenPlastic;
    private int KeyringMoltenPlastic = 1;
    public TextMeshProUGUI keyringMaking;

    [Header("����")]
    public TextMeshProUGUI tongsAluminum;
    private int TongsAluminum = 2;
    public TextMeshProUGUI tongsMaking;

    [Header("��")]
    public TextMeshProUGUI cupCan;
    private int CupCan = 2;
    public TextMeshProUGUI cupMaking;

    [Header("�׸�")]
    public TextMeshProUGUI bowlMoltenGlass;
    private int BowlMoltenGlass = 2;
    public TextMeshProUGUI bowlMaking;


    // Start is called before the first frame update
    void Start()
    {
        //��ư Ŭ�� �̺�Ʈ
        /*���׸���*/
        p_potButton.onClick.AddListener(() => StartP_PotMaking()); //�ö�ƽ ȭ��
        c_potButton.onClick.AddListener(() => StartC_PotMaking()); //ĵ ȭ��
        g_potButton.onClick.AddListener(() => StartG_PotMaking()); //���� ȭ��
        tableButton.onClick.AddListener(() => StartTableMaking()); //���̺�
        chairButton.onClick.AddListener(() => StartChairMaking()); //����
        storageBoxButton.onClick.AddListener(() => StartStorageBoxMaking()); //������
        mobileButton.onClick.AddListener(() => StartMobileMaking()); //���
        clockButton.onClick.AddListener(() => StartClockMaking()); //�ð�

        /*�Ǹſ�*/
        keyringButton.onClick.AddListener(() => StartKeyringMaking()); //Ű��
        tongsButton.onClick.AddListener(() => StartTongsMaking()); //����
        cupButton.onClick.AddListener(() => StartCupMaking()); //��
        bowlButton.onClick.AddListener(() => StartBowlMaking()); //�׸�

        // �ʱ� ��ư ���� üũ
        UpdateButtonStates();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        UpdateButtonStates();
    }

    //UI������Ʈ
    void UpdateUI()
    {
        /* ���׸��� */
        //�ö�ƽ ȭ�� ����
        potPt.text = "��Ʈ\n" + plasticBottle.ToString() + "/1";
        p_potMaking.text = "���� Ƚ��: " + P_PotMaiking.ToString();

        //ĵ ȭ�� ����
        potCan.text = "ĵ\n" + can.ToString() + "/1";
        c_potMaking.text = "���� Ƚ��: " + C_PotMaiking.ToString();

        //���� ȭ�� ����
        potGlass.text = "����\n" + glass.ToString() + "/1";
        g_potMaking.text = "���� Ƚ��: " + G_PotMaiking.ToString();

        //���̺� ����
        tableCompressedPaper.text = "���� ����\n" + compressedPaper.ToString() + "/4";
        tableMaking.text = "���� Ƚ��: " + TableMaking.ToString();

        //���� ����
        chairCompressedPaper.text = "���� ����\n" + compressedPaper + "/3";
        chairMaking.text = "���� Ƚ��: " + ChairMaking.ToString();

        //������ ����
        storageBoxCompressedPaper.text = "���� ����\n" + compressedPaper.ToString() + "/2";
        storageBoxCan.text = "ĵ\n" + can.ToString() + "/3";
        storageBoxMaking.text = "���� Ƚ��: " + StorageBoxMaking.ToString();

        //��� ����
        mobileMoltenGlass.text = "���� ����\n" + moltenGlass.ToString() + "/1";
        mobileVinyl.text = "���\n" + vinyl.ToString() + "/3";
        mobileCan.text = "ĵ\n" + can.ToString() + "/1";
        mobilePtThread.text = "��Ʈ��\n" + plasticThread.ToString() + "/2";
        mobileMaking.text = "���� Ƚ��: " + MobileMaking.ToString();

        //�ð� ����
        clockMoltenGlass.text = "���� ����\n" + moltenGlass.ToString() + "/1";
        clockCompressedPaper.text = "���� ����\n" + compressedPaper.ToString() + "/2";
        clockCan.text = "ĵ\n" + can.ToString() + "/1";
        clockMaking.text = "���� Ƚ��: " + ClockMaking.ToString();

        /* �Ǹſ� */
        //Ű�� ����
        keyringMoltenPlastic.text = "����\n�ö�ƽ\n" + moltenPlastic.ToString() + "/1";
        keyringMaking.text = "���� Ƚ��: " + KeyringMaking.ToString();

        //���� ����
        tongsAluminum.text = "�˷�̴�\n" + aluminum.ToString() + "/2";
        tongsMaking.text = "���� Ƚ��: " + TongsMaking.ToString();

        //�� ����
        cupCan.text = "ĵ\n" + can.ToString() + "/2";
        cupMaking.text = "���� Ƚ��: " + CupMaking.ToString();

        //�׸� ����
        bowlMoltenGlass.text = "���� ����\n" + moltenGlass.ToString() + "/2";
        bowlMaking.text = "���� Ƚ��: " + BowlMaking.ToString();
    }

    //��ư ���� ������Ʈ
    void UpdateButtonStates()
    {
        /* ���׸��� */
        p_potButton.interactable = (plasticBottle >= PotPt); //�ö�ƽ ȭ��
        c_potButton.interactable = (can >= PotCan); //ĵ ȭ��
        g_potButton.interactable = (glass >= PopGlass); //���� ȭ��
        tableButton.interactable = (compressedPaper >= TableCompressedPaper); //���̺�
        chairButton.interactable = (compressedPaper >= ChairCompressedPaper); //����
        storageBoxButton.interactable = (compressedPaper >= StorageBoxCompressedPaper && can >= StorageBoxCan); //������
        mobileButton.interactable = (moltenGlass >= MobileMoltenGlass && vinyl >= MobileVinyl && can >= MobileCan && plasticThread >= MobilePtThread); //���
        clockButton.interactable = (moltenGlass >= ClockMoltenGlass && compressedPaper >= ClockCompressedPaper && can >= ClockCan); //�ð�

        /* �Ǹſ� */
        keyringButton.interactable = (moltenPlastic >= KeyringMoltenPlastic); //Ű��
        tongsButton.interactable = (aluminum >= TongsAluminum); //����
        cupButton.interactable = (can >= CupCan); //��
        bowlButton.interactable = (moltenGlass >= BowlMoltenGlass); //�׸�

    }

    /* �Ǹſ� */
    //�׸�
    void StartBowlMaking()
    {
        if(moltenGlass >= BowlMoltenGlass)
        {
            moltenGlass -= BowlMoltenGlass;
            BowlMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //��
    void StartCupMaking()
    {
        if(can >= CupCan)
        {
            can -= CupCan;
            CupMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //����
    void StartTongsMaking()
    {
        if(aluminum >= TongsAluminum)
        {
            aluminum -= TongsAluminum;
            TongsMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //Ű��
    void StartKeyringMaking()
    {
        if (moltenPlastic >= KeyringMoltenPlastic)
        {
            moltenPlastic -= KeyringMoltenPlastic;
            KeyringMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    /* ���׸��� */
    //�ð�
    void StartClockMaking()
    {
        if(moltenGlass >= ClockMoltenGlass && compressedPaper >= ClockCompressedPaper && can >= ClockCan)
        {
            moltenGlass -= ClockMoltenGlass;
            compressedPaper -= ClockCompressedPaper;
            can -= ClockCan;
            ClockMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

   

    //���
    void StartMobileMaking()
    {
        if(moltenGlass >= MobileMoltenGlass && vinyl >= MobileVinyl && can >= MobileCan && plasticThread >= MobilePtThread)
        {
            moltenGlass -= MobileMoltenGlass;
            vinyl -= MobileVinyl;
            can -= MobileCan;
            plasticThread -= MobilePtThread;
            MobileMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //������
    void StartStorageBoxMaking()
    {
        if(compressedPaper >= StorageBoxCompressedPaper && can >= StorageBoxCan)
        {
            compressedPaper -= StorageBoxCompressedPaper;
            can -= StorageBoxCan;
            StorageBoxMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //����
    void StartChairMaking()
    {
        if(compressedPaper >= ChairCompressedPaper)
        {
            compressedPaper -= ChairCompressedPaper;
            ChairMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //���̺�
    void StartTableMaking()
    {
        if(compressedPaper >= TableCompressedPaper)
        {
            compressedPaper -= TableCompressedPaper;
            TableMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //���� ȭ��
    void StartG_PotMaking()
    {
        if(glass >= PopGlass)
        {
            glass -= PopGlass;
            G_PotMaiking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //ĵ ȭ��
    void StartC_PotMaking()
    {
        if(can >= PotCan)
        {
            can -= PotCan;
            C_PotMaiking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //�ö�Ƽ ȭ�� ����
    void StartP_PotMaking()
    {
        if(plasticBottle >= PotPt)
        {
            plasticBottle -= PotPt;
            P_PotMaiking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    
}
