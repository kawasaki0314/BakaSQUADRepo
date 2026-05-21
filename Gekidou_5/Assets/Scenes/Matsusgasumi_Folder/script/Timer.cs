using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private float countTime; // インスペクター用なので小文字スタートが一般的です

    private float remainingTime; // 残り時間を保持する変数
    private bool isTimerRunning = true; // タイマーが動いているかどうかのフラグ

    private void Start()
    {
        // 初期化：インスペクターで設定した時間を代入
        remainingTime = countTime;
    }

    private void Update()
    {
        // タイマーが動いていない、または残り時間が0以下の場合は処理を抜ける
        if (!isTimerRunning) return;

        // 毎フレームの経過時間を引いていく
        remainingTime -= Time.deltaTime;

        // 0秒以下になったらタイマーを止めて、数値を0に固定する
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isTimerRunning = false;

            // 必要であれば、ここに「タイムアップ時の処理」を書けます
        }

        // 時間の表示更新
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        uiText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}