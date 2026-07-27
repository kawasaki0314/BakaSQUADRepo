using UnityEngine;
using TMPro; // ★ ImageからTextMeshProUGUIに変更するために追加

public class StatusGaugeUI : MonoBehaviour
{
    [Header("レベル表示用テキスト")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI bulletText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI fireRateText;

    // ※自分自身の「gaugeUI」変数は不要なので削除しています

    // 共通のテキスト更新処理
    private void UpdateLevelText(TextMeshProUGUI targetText, int level)
    {
        if (targetText == null) return;

        // レベル0の時は非表示（または "×0" と出したい場合は `$"×{level}"` だけにする）
            targetText.text = $"+{level}";
    }

    public void UpdateAttack(int value)
    {
        UpdateLevelText(attackText, value);
    }

    public void UpdateSpeed(int value) // 引数をintに合わせてもOKです
    {
        UpdateLevelText(speedText, (int)value);
    }

    public void UpdateBullet(int value)
    {
        UpdateLevelText(bulletText, value);
    }

    public void UpdateHP(int value)
    {
        UpdateLevelText(hpText, value);
    }

    public void UpdateFireRate(float value)
    {
        UpdateLevelText(fireRateText, (int)value);
    }
}