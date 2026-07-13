using System.Collections;
using UnityEngine;
using TMPro; // TextMeshProを使う場合

public class GameStartDirector : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI goText;

    [Header("Ready Settings")]
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float readyStayDuration = 0.5f;

    [Header("Go Settings")]
    [SerializeField] private float goDisplayDuration = 1.0f;
    [SerializeField] private float shakeMagnitude = 15.0f; // 揺れの強さ

    private Vector3 goTextOriginalPosition;

    void Start()
    {
        // 1. 最初は時間を止める
        Time.timeScale = 0f;

        // Goテキストの初期位置を記憶
        if (goText != null)
        {
            goTextOriginalPosition = goText.transform.localPosition;
        }

        // 演出スタート
        StartCoroutine(StartProductionRoutine());
    }

    private IEnumerator StartProductionRoutine()
    {
        //① Ready? のフェードイン ---
        readyText.gameObject.SetActive(true);
        Color textColor = readyText.color;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            // Time.timeScale = 0 なので、Time.unscaledDeltaTime を使う
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeInDuration);

            textColor.a = alpha;
            readyText.color = textColor;
            yield return null;
        }

        // Ready? 表示のまま少し待つ
        yield return new WaitForSecondsRealtime(readyStayDuration);

        // Ready? を消す
        readyText.gameObject.SetActive(false);


        // ② Go! の全方向シェイク出現
        goText.gameObject.SetActive(true);
        elapsed = 0f;

        while (elapsed < goDisplayDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            // ランダムな全方向に揺らす
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            goText.transform.localPosition = goTextOriginalPosition + new Vector3(offsetX, offsetY, 0);

            yield return null;
        }

        // 揺れを戻してGo!を非表示に
        goText.transform.localPosition = goTextOriginalPosition;
        goText.gameObject.SetActive(false);


        // ③ ゲーム開始
        // タイムスケールを元に戻して、他の処理を動かす
        Time.timeScale = 1f;
        Debug.Log("ゲームスタート！");
    }
}