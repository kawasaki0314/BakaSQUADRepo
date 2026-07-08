using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMP_GlitchText_Similar : MonoBehaviour
{
    private TextMeshProUGUI tmpText;
    private string originalText;
    private bool isGlitching = false;

    [Header("ノイズ設定")]
    [SerializeField] private float glitchDuration = 0.4f; // ノイズが走る長さ
    [SerializeField] private float glitchInterval = 2.5f;   // ノイズが発生する間隔
    [SerializeField, Range(0f, 1f)] private float noiseChance = 0.4f; // 各文字が化ける確率

    // 文字ごとの「似た文字・化け先」のリストを定義
    private Dictionary<char, string> similarCharMap = new Dictionary<char, string>()
    {
        { 'Y', "yv¥Ý" },
        { 'O', "o00Q" },
        { 'U', "uvµ" },
        { 'D', "d0bđ" },
        { 'I', "i1|l!" },
        { 'E', "e3£ê" },
        { '.', "." }
    };

    void Start()
    {
        Time.timeScale = 1f;

        tmpText = GetComponent<TextMeshProUGUI>();
        originalText = tmpText.text;

        StartCoroutine(GlitchLoop());
    }

    public void SetText(string newText)
    {
        originalText = newText;
        if (!isGlitching) tmpText.text = newText;
    }

    private IEnumerator GlitchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(glitchInterval);
            yield return StartCoroutine(DoGlitch());
        }
    }

    private IEnumerator DoGlitch()
    {
        isGlitching = true;
        float elapsedTime = 0f;

        while (elapsedTime < glitchDuration)
        {
            char[] scrambled = originalText.ToCharArray();

            for (int i = 0; i < scrambled.Length; i++)
            {
                char originalChar = scrambled[i];

                // 大文字小文字を区別せず判定するために、判定用に大文字化
                char upperChar = char.ToUpper(originalChar);

                // 確率に当選し、かつ変換辞書に登録されている文字の場合
                if (Random.value < noiseChance && similarCharMap.ContainsKey(upperChar))
                {
                    string candidates = similarCharMap[upperChar];
                    // 候補の中からランダムに1文字選んで置き換える
                    scrambled[i] = candidates[Random.Range(0, candidates.Length)];
                }
            }

            tmpText.text = new string(scrambled);
            yield return null; 
            elapsedTime += Time.deltaTime;
        }

        // 元に戻す
        tmpText.text = originalText;
        isGlitching = false;
    }
}