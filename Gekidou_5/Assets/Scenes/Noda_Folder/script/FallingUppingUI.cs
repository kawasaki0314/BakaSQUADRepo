using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class FallingUppingUI : MonoBehaviour
{
    public enum StartDirection { FromTop, FromBottom }

    [Header("スライドの方向")]
    [SerializeField] private StartDirection direction = StartDirection.FromTop;

    [Header("アニメーション設定")]
    [Tooltip("移動にかかる時間（秒）。0にすると一瞬で着きます。")]
    [SerializeField] private float duration = 0.5f;     // 移動時間（速度の代わり）

    [Tooltip("左から順に動かすための遅延時間（秒）。0.1ずつずらすと綺麗です。")]
    [SerializeField] private float delay = 0f;

    [Tooltip("画面外の初期位置（画面中央からの距離）。")]
    [SerializeField] private float startOffset = 800f;

    private RectTransform rectTransform;
    private Vector2 startPosition; // 計算後に保持する開始位置
    private Vector2 goalPosition;  // 計算後に保持する目標位置

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        StartCoroutine(AnimateRoutine());
    }

    IEnumerator AnimateRoutine()
    {
        // === ここで1フレーム待つ（UIの配置を確定させる） ===
        yield return null;

        // === 【重要】1フレーム待った後に、座標の計算と初期配置を行う ===

        // インスペクターで配置した「現在の正しい位置」をゴール（所定の位置）として記憶
        goalPosition = rectTransform.anchoredPosition;

        // 初期位置（画面外）を計算
        float startY = (direction == StartDirection.FromTop) ? startOffset : -startOffset;
        startPosition = new Vector2(goalPosition.x, startY);

        // まずは、計算した初期位置（画面外）に飛ばす
        rectTransform.anchoredPosition = startPosition;

        // === ここからアニメーションを開始する ===

        // 指定された秒数だけ待つ
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        // 移動にかかる時間（duration）が0以下の場合は、一瞬でゴールに移動させて終わる
        if (duration <= 0f)
        {
            rectTransform.anchoredPosition = goalPosition;
            yield break;
        }

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            // 進捗の割合（0～1）
            float t = timeElapsed / duration;

            // イージング（後半ゆっくり）：数学的な調整
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            // 計算したstartPositionからgoalPositionまで移動
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, goalPosition, t);

            yield return null;
        }
    }
    public void ForceToGoalPosition()
    {
        StopAllCoroutines();
        // Awake等で記憶しておいた元の正しい位置を強制代入
        GetComponent<RectTransform>().anchoredPosition = goalPosition;
    }
}