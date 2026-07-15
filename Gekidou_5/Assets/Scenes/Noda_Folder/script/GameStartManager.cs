using System.Collections;
using UnityEngine;
using TMPro;

public class GameStartDirector : MonoBehaviour
{
    public static bool IsGameStarted { get; private set; } = true;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI goText;

    [Header("Ready Settings")]
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float readyStayDuration = 0.5f;
    [SerializeField] private float readyStartOffsetVolume = 50.0f;

    [Header("Go Settings")]
    [SerializeField] private float goDisplayDuration = 1.0f;
    [SerializeField] private float shakeMagnitude = 15.0f;

    private Vector3 readyTextOriginalPosition;
    private Vector3 goTextOriginalPosition;

    // 演出にかかる「合計時間」を外から計算できるようにするプロパティ
    public float TotalProductionDuration
    {
        get { return fadeInDuration + readyStayDuration + goDisplayDuration; }
    }

    void Awake()
    {
        IsGameStarted = false;   
    }

    void Start()
    {
        Time.timeScale = 0f;

        if (readyText != null) readyTextOriginalPosition = readyText.transform.localPosition;
        if (goText != null) goTextOriginalPosition = goText.transform.localPosition;

        StartCoroutine(StartProductionRoutine());
    }

    private IEnumerator StartProductionRoutine()
    {
        // --- Ready? の下からフェードイン 演出 ---
        readyText.gameObject.SetActive(true);
        Color textColor = readyText.color;
        float elapsed = 0f;
        Vector3 readyStartPosition = readyTextOriginalPosition - new Vector3(0, readyStartOffsetVolume, 0);

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / fadeInDuration);
            textColor.a = progress;
            readyText.color = textColor;
            readyText.transform.localPosition = Vector3.Lerp(readyStartPosition, readyTextOriginalPosition, progress);
            yield return null;
        }

        readyText.transform.localPosition = readyTextOriginalPosition;
        yield return new WaitForSecondsRealtime(readyStayDuration);
        readyText.gameObject.SetActive(false);

        // --- Go! の全方向シェイク出現 ---
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

        Time.timeScale = 1f;

        IsGameStarted = true;
    }
}