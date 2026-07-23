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

    [Header("SE Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dropSE; // 文字が着地した時の音

    private RectTransform rectTransform;
    private Vector2 startPosition; // 計算後に保持する開始位置
    private Vector2 goalPosition;  // 計算後に保持する目標位置

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // AudioSourceが未設定の場合、自分自身から自動取得を試みる保険
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Start()
    {
        StartCoroutine(AnimateRoutine());
    }

    IEnumerator AnimateRoutine()
    {
        // === ここで1フレーム待つ（UIの配置を確定させる） ===
        yield return null;

        // === 1フレーム待った後に、座標の計算と初期配置を行う ===
        goalPosition = rectTransform.anchoredPosition;

        float startY = (direction == StartDirection.FromTop) ? startOffset : -startOffset;
        startPosition = new Vector2(goalPosition.x, startY);

        rectTransform.anchoredPosition = startPosition;

        // === ここからアニメーションを開始する ===
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        // 移動にかかる時間（duration）が0以下の場合は、一瞬でゴールに移動させて音を鳴らす
        if (duration <= 0f)
        {
            rectTransform.anchoredPosition = goalPosition;
            OnArrivedAtGoal(); // 【追加】到着処理を呼ぶ
            yield break;
        }

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float t = timeElapsed / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, goalPosition, t);

            yield return null;
        }

        // === 【追加】ループが終わった＝目標地点に到着！確実にゴール座標にし、音を鳴らす ===
        rectTransform.anchoredPosition = goalPosition;
        OnArrivedAtGoal();
    }

    public void ForceToGoalPosition()
    {
        StopAllCoroutines();

        // 移動途中でスキップされた場合でも、まだ目標座標を取得していれば移動を完了させる
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = goalPosition;
        }
    }

    // 文字が目標地点に到着したタイミングで呼ばれる関数
    private void OnArrivedAtGoal()
    {
        if (audioSource != null && dropSE != null)
        {
            audioSource.PlayOneShot(dropSE);
        }
    }
}