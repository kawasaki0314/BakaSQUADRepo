using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StatusGaugeUI : MonoBehaviour
{
    [SerializeField] private Image attackGauge;
    [SerializeField] private Image speedGauge;
    [SerializeField] private Image bulletGauge;
    [SerializeField] private Image hpGauge;
    [SerializeField] private Image fireRateGauge;

    [SerializeField] private StatusGaugeUI gaugeUI;


    public void UpdateAttack(int value)
    {
        attackGauge.fillAmount = value/10f;
    }
    public void UpdateSpeed(float value)
    {
        speedGauge.fillAmount = value / 10f;
    }
    public void UpdateBullet(int value)
    {
        bulletGauge.fillAmount = value / 10f;
    }
    public void UpdateHP(int value)
    {
        hpGauge.fillAmount = value / 10f;
    }
    public void UpdateFireRate(float value)
    {
        fireRateGauge.fillAmount = value / 10f;
    }
}
