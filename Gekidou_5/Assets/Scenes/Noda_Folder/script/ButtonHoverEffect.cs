using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // シーン遷移に必要
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private float normalSize = 30f;
    [SerializeField] private float hoverSize = 38f;
    [SerializeField] private float changeSpeed = 120f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSE; // ホバー時の音（不要なら空でOK）
    [SerializeField] private AudioClip clickSE; // クリック時の音

    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName = ""; // 遷移先のシーン名（空なら遷移しない）
    [SerializeField] private float delayBeforeSceneChange = 0.2f; // クリック音を鳴らしてから遷移するまでの待ち時間（秒）

    private float targetSize;
    private bool isClicked = false; // 二重クリック防止フラグ

    void Start()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        // AudioSourceが未設定なら自動取得を試みる
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        targetSize = normalSize;
        if (buttonText != null)
        {
            buttonText.fontSize = normalSize;
        }
    }

    void Update()
    {
        if (buttonText == null) return;

        if (!Mathf.Approximately(buttonText.fontSize, targetSize))
        {
            buttonText.fontSize = Mathf.MoveTowards(
                buttonText.fontSize,
                targetSize,
                changeSpeed * Time.unscaledDeltaTime
            );
        }
    }

    // カーソルが乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isClicked) return; // クリック後はホバー反応させない

        targetSize = hoverSize;

        // ホバーSEの設定があれば再生
        if (audioSource != null && hoverSE != null)
        {
            audioSource.PlayOneShot(hoverSE);
        }
    }

    // カーソルが離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isClicked) return;

        targetSize = normalSize;
    }

    // ボタンがクリックされたとき（IPointerClickHandlerにより自動で呼ばれます）
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClicked) return; // 連打防止
        isClicked = true;

        StartCoroutine(PlaySEAndChangeScene());
    }

    private IEnumerator PlaySEAndChangeScene()
    {
        // 1. クリックSEの再生
        if (audioSource != null && clickSE != null)
        {
            audioSource.PlayOneShot(clickSE);
        }

        // 2. 指定した時間だけ待つ（SEの余韻を残すため）
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // 3. シーン名が指定されていれば遷移する
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}