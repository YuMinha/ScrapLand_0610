using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SewingMachineController : MonoBehaviour
{
    // ��� ������ ������ public ����
    public int plasticThread = 0;  // ��Ʈ��
    public int paper = 0;         // ����
    public int plastic = 0;        // ���� �ö�ƽ
    public int oldCloth = 0;      // �� ��

    // ���� ���¸� ������ ����
    private int CapMaking = 0;
    private int GroveMaking = 0;
    private int TopMaking = 0;
    private int BottomMaking = 0;
    private int ShoesMaking = 0;

    // UI ��ư �� �ؽ�Ʈ (Inspector���� �Ҵ�)
    public Button capButton;       // ���� ���� ��ư
    public Button gloveButton;     // �尩 ���� ��ư
    public Button topButton;       // ���� ���� ��ư
    public Button bottomButton;    // ���� ���� ��ư
    public Button shoesButton;     // �Ź� ���� ��ư

    //text UI
    [Header("����")]
    public TextMeshProUGUI capPt;
    public TextMeshProUGUI capPaper;
    public TextMeshProUGUI capMaking;
    [Header("�尩")]
    public TextMeshProUGUI glovePt;
    public TextMeshProUGUI gloveOldCloth;
    public TextMeshProUGUI gloveMaking;
    [Header("����")]
    public TextMeshProUGUI topPt;
    public TextMeshProUGUI topOldCloth;
    public TextMeshProUGUI topMaking;
    [Header("����")]
    public TextMeshProUGUI bottomPt;
    public TextMeshProUGUI bottomOldCloth;
    public TextMeshProUGUI bottomMaking;
    [Header("�Ź�")]
    public TextMeshProUGUI shoesOldCloth;
    public TextMeshProUGUI shoesPlastic;
    public TextMeshProUGUI shoesMaking;

    // ���ۿ� �ʿ��� �ּ� ��� ����
    //����
    private int CapPt = 5;   // ���� ���ۿ� �ʿ��� �ּ� �ö�ƽ
    private int CapPaper = 1;     // ���� ���ۿ� �ʿ��� �ּ� ����
    //�尩
    private int GrovePt = 1;    // ���� ���ۿ� �ʿ��� �ּ� ��Ʈ��
    private int GloveOldCloth = 1;     // �尩 ���ۿ� �ʿ��� �ּ� �� ��
    //����
    private int TopPt = 1;
    private int TopOldCloth = 3;
    //����
    private int BottomPt = 1;
    private int BottomOldCloth = 3;
    //�Ź�
    private int ShoesPlastic = 2;
    private int ShoesOldCloth = 3;

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        capButton.onClick.AddListener(() => StartCapMaking());
        gloveButton.onClick.AddListener(() => StartGloveMaking());
        topButton.onClick.AddListener(() => StartTopMaking());
        topButton.onClick.AddListener(() => StartBottomMaking());
        shoesButton.onClick.AddListener(() => StartShoesMaking());


        // �ʱ� ��ư ���� üũ
        UpdateButtonStates();
    }

    void Update()
    {
        UpdateUI();
        UpdateButtonStates();
    }

    // UI ������Ʈ �޼���
    void UpdateUI()
    {
        // ���� ���� ���� UI ������Ʈ
        capPt.text = "��Ʈ��\n" + plasticThread.ToString() + "/5";         
        capPaper.text = "����\n" + paper.ToString() + "/1";               
        capMaking.text = "���� Ƚ��: " + CapMaking.ToString();

        // �尩 ���� ���� UI ������Ʈ
        glovePt.text = "��Ʈ��\n" + plasticThread.ToString() + "/1";   
        gloveOldCloth.text = "�� ��\n" + oldCloth.ToString() + "/1";     
        gloveMaking.text = "���� Ƚ��: " + GroveMaking.ToString();

        // ���� ���� ���� UI ������Ʈ
        topPt.text = "��Ʈ��\n" + plasticThread.ToString() + "/1";      
        topOldCloth.text = "�� ��\n" + oldCloth.ToString() + "/3";    
        topMaking.text = "���� Ƚ��: " + TopMaking.ToString();

        // ���� ���� ���� UI ������Ʈ
        bottomPt.text = "��Ʈ��\n" + plasticThread.ToString() + "/1";      
        bottomOldCloth.text = "�� ��\n" + oldCloth.ToString() + "/3";     
        bottomMaking.text = "���� Ƚ��: " + BottomMaking.ToString();

        // �Ź� ���� ���� UI ������Ʈ
        shoesPlastic.text = "����\n�ö�ƽ\n" + plastic.ToString() + "/2";
        shoesOldCloth.text = "�� ��\n" + oldCloth.ToString() + "/3";
        shoesMaking.text = "���� Ƚ��: " + ShoesMaking.ToString();
    }

    // ��ư ���� ������Ʈ �޼���
    void UpdateButtonStates()
    {
        // ���� ���� ��ư Ȱ��ȭ
        capButton.interactable = (plasticThread >= CapPt && paper >= CapPaper);

        // �尩 ���� ��ư Ȱ��ȭ
        gloveButton.interactable = (plasticThread >= GrovePt && oldCloth >= GloveOldCloth);

        //���� ���� ��ư Ȱ��ȭ
        topButton.interactable = (plasticThread >= TopPt && oldCloth >= TopOldCloth);

        //���� ���� ��ư Ȱ��ȭ
        bottomButton.interactable = (plasticThread >= BottomPt && oldCloth >= BottomOldCloth);

        //�Ź� ���� ��ư Ȱ��ȭ
        shoesButton.interactable = (plastic >= ShoesPlastic && oldCloth >= ShoesOldCloth);
    }

    // ���� ���� ����
    void StartCapMaking()
    {
        if (plasticThread >= CapPt && paper >= CapPaper)
        {
            plasticThread -= CapPt;
            paper -= CapPaper;
            CapMaking++;
            Debug.Log("���� ���� ����! CapMaking: " + CapMaking);
            UpdateUI();
        }
        UpdateButtonStates();
    }

    // �尩 ���� ����
    void StartGloveMaking()
    {
        if (plasticThread >= GrovePt && oldCloth >= GloveOldCloth)
        {
            plasticThread -= GrovePt;
            oldCloth -= GloveOldCloth;
            GroveMaking++;
            Debug.Log("�尩 ���� ����!");
            UpdateUI();
        }
        UpdateButtonStates();
    }

    // ���� ���� ����
    void StartTopMaking()
    {
        if (plasticThread >= TopPt && oldCloth >= TopOldCloth)
        {
            plasticThread -= TopPt;
            oldCloth -= TopOldCloth;
            TopMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    // ���� ���� ����
    void StartBottomMaking()
    {
        if (plasticThread >= BottomPt && oldCloth >= BottomOldCloth)
        {
            plasticThread -= BottomPt;
            oldCloth -= BottomOldCloth;
            BottomMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    //�Ź� ���� ����
    void StartShoesMaking()
    {
        if(plastic >= ShoesPlastic && oldCloth >= ShoesOldCloth)
        {
            plastic -= ShoesPlastic;
            oldCloth -= ShoesOldCloth;
            ShoesMaking++;
            UpdateUI();
        }
        UpdateButtonStates();
    }

    // ��� �߰� �޼��� (���÷� ȣ�� ����)
    public void AddMaterial(string material, int amount)
    {
        switch (material)
        {
            case "plasticThread":
                plasticThread += amount;
                break;
            case "paper":
                paper += amount;
                break;
            case "plastic":
                plastic += amount;
                break;
            case "oldCloth":
                oldCloth += amount;
                break;
        }
        UpdateButtonStates();
    }
}