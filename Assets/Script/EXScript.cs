using UnityEngine;

public class EXScript : MonoBehaviour
{
    public BlockController blockController;

    [SerializeField] private bool _machine;
    [SerializeField] private bool _blastFurnace;
    [SerializeField] private bool _compressor;

    // �ν����Ϳ��� ���� �ٲ� ������ BlockController�� �ݿ�
    private void OnValidate()
    {
        if (blockController != null)
        {
            blockController.machine = _machine;
            blockController.blastFurnace = _blastFurnace;
            blockController.compressor = _compressor;
        }
    }
}
