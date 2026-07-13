using System.Collections;
using UnityEngine;
using TMPro;

public class GameStartDirector : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform readyTextRect;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI goText;

    [Header("Ready Settings (Falling)")]
    [SerializeField] private float dropDuration = 0.6f;     // 落ちてくる時間
    [SerializeField] private float readyStayDuration = 0.5f; // 中央で止まっている時間

    [Header("Go Settings")]
    [SerializeField] private float goDisplayDuration = 1.0f;
    [SerializeField] private float shakeMagnitude = 15.0f;

    private Vector3 goTextOriginalPosition;
    private Vector2 readyTextTargetPosition;

    void Start()
    {
        // 1. 最初は時間を止める
        Time.timeScale = 0f;

        // 初期位置の記憶
        if (goText != null) goTextOriginalPosition = goText.transform.localPosition;
        if (readyTextRect != null) readyTextTargetPosition = readyTextRect.anchoredPosition;

        // 演出スタート
        StartCoroutine(StartProductionRoutine());
    }

    private IEnumerator StartProductionRoutine()
    {
        // --- ① Ready? が上から降ってくる ---
        readyText.gameObject.SetActive(true);
        Color textColor = readyText.color;

        // 【改善】画面全体の高さを取得し、画面のすぐ上を初期位置にする
        float canvasHeight = readyTextRect.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height;
        float startY = readyTextTargetPosition.y + (canvasHeight / 2f) + 100f; // 画面の上外側

        float elapsed = 0f;

        while (elapsed < dropDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / dropDuration);

            // イージング（滑らかな減速）
            float tFalling = 1f - Mathf.Pow(1f - t, 2);

            // 位置の更新
            float currentY = Mathf.Lerp(startY, readyTextTargetPosition.y, tFalling);
            readyTextRect.anchoredPosition = new Vector2(readyTextTargetPosition.x, currentY);

            // 【改善】フェードインの適用とTMPの強制再描画
            textColor.a = t;
            readyText.color = textColor;
            readyText.SetVerticesDirty(); // これがないとTimeScale=0で色が変わらない場合がある

            yield return null;
        }

        // 位置とアルファを完全に固定
        readyTextRect.anchoredPosition = readyTextTargetPosition;
        textColor.a = 1f;
        readyText.color = textColor;
        readyText.SetVerticesDirty();

        // 中央で少し待つ
        yield return new WaitForSecondsRealtime(readyStayDuration);
        readyText.gameObject.SetActive(false);


        // --- ② Go! の全方向シェイク出現 ---
        goText.gameObject.SetActive(true);
        elapsed = 0f;

        while (elapsed < goDisplayDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            goText.transform.localPosition = goTextOriginalPosition + new Vector3(offsetX, offsetY, 0);

            yield return null;
        }

        goText.transform.localPosition = goTextOriginalPosition;
        goText.gameObject.SetActive(false);


        // --- ③ ゲーム開始 ---
        Time.timeScale = 1f;
        Debug.Log("ゲームスタート！");
    }
}