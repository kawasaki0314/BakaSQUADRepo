using UnityEngine;
using UnityEngine.UI; // UIを扱うために必要

public class BlinkCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownImage; // Fill Amountを変更するImage
    [SerializeField] private float cooldownTime = 3.0f; // 再使用までの時間（秒）

    private float currentCooldown = 0.0f;
    private bool isCooldown = false;

    void Update()
    {
        // テスト用：スペースキーでブリンク発動
        if (Input.GetMouseButtonDown(1) && !isCooldown)
        {
            TriggerBlink();
        }

        // クールダウン中の処理
        if (isCooldown)
        {
            // 時間を進行させる
            currentCooldown += Time.deltaTime;

            // Fill Amountを更新 (0.0 から 1.0 に向かって増える)
            cooldownImage.fillAmount = Mathf.Clamp01(currentCooldown / cooldownTime);

            // 満タンになったらタイマー終了
            if (currentCooldown >= cooldownTime)
            {
                isCooldown = false;
                currentCooldown = 0.0f;
            }
        }
    }

    // ブリンクを発動したときの処理
    public void TriggerBlink()
    {
        // 実際のブリンク処理をここに書く
        Debug.Log("ブリンク発動！");

        // クールダウン開始
        isCooldown = true;
        currentCooldown = 0.0f;
        cooldownImage.fillAmount = 0.0f; // ゲージをゼロにする
    }
}