using System.Collections;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [Header("�޺� ����")]
    public Light sunLight;
    public Gradient lightColorOverTime;
    public AnimationCurve intensityCurve;

    [Header("�ϴ� ����")]
    public Material skyboxMaterial;
    public Gradient skyColorOverTime;

    private float totalDayDuration = 10f;//15qns=900 �ϴ� 10���ص�
    private float currentTime = 0f;
    private bool dayEnded = false;

    void Start()
    {
        if (sunLight == null)
        {
            Debug.LogError("Sun Light�� �������� �ʾҽ��ϴ�!");
        }
        if (skyboxMaterial == null)
        {
            Debug.LogError("Skybox Material�� �������� �ʾҽ��ϴ�!");
        }
    }

    void Update()
    {
        if (dayEnded) return;

        currentTime += Time.deltaTime;

        float normalizedTime = currentTime / totalDayDuration;
        UpdateSun(normalizedTime);

        if (currentTime >= totalDayDuration)
        {
            EndDay();
        }
    }

    void UpdateSun(float t)
    {
        sunLight.color = lightColorOverTime.Evaluate(t);
        sunLight.intensity = intensityCurve.Evaluate(t);

        float sunAngle = Mathf.Lerp(-5f, 175f, t); // �� �߰� ���� ȸ��
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0, 0);

        // �ϴ� �� ����
        if (skyboxMaterial != null)
        {
            Color skyTint = skyColorOverTime.Evaluate(t);
            skyboxMaterial.SetColor("_SkyTint", skyTint);

            RenderSettings.skybox = skyboxMaterial;
        }
    }

    void EndDay()
    {
        dayEnded = true;
        Debug.Log("�Ϸ� ��!");

        DaySummery.instance.EndOneDay();
    }
}
