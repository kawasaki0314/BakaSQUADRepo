
using System.Collections;
using UnityEngine;
using TMPro;

public class CharColorChanger : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent; // TextMeshProコンポーネント
    [SerializeField] private Color highlightColor = Color.yellow; // 変化後の色（例: 黄色）
    [SerializeField] private Color normalColor = Color.white;    // 元の色（例: 白）
    [SerializeField] private float durationPerChar = 0.3f;       // 1文字あたりの発光時間

    private void Start()
    {
        Time.timeScale = 1.0f;

        if (textComponent == null)
        {
            textComponent = GetComponent<TMP_Text>();
        }

        // 初期カラーを設定して、ループ演出を開始
        SetAllCharactersColor(normalColor);
        StartCoroutine(AnimateCharacterColor());
    }

    private IEnumerator AnimateCharacterColor()
    {
        // 無限ループで「CLEAR!」の文字を巡回させます
        while (true)
        {
            int characterCount = textComponent.textInfo.characterCount;

            for (int i = 0; i < characterCount; i++)
            {
                // 空白文字などはスキップ
                if (!textComponent.textInfo.characterInfo[i].isVisible)
                {
                    continue;
                }

                // 現在の文字（i番目）の色をハイライト色に変える
                SetSingleCharacterColor(i, highlightColor);

                // 指定した時間だけ待つ
                yield return new WaitForSeconds(durationPerChar);

                // 次の文字に行く前に、今の文字を元の色に戻す
                SetSingleCharacterColor(i, normalColor);
            }

            // 全ての文字が一周した後に少し待機を入れる場合（不要なら消してOK）
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// 特定のインデックスの文字だけ色を変更する
    private void SetSingleCharacterColor(int charIndex, Color color)
    {
        TMP_TextInfo textInfo = textComponent.textInfo;

        // 対象文字のメッシュ情報を取得
        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;
        Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

        // 1つの文字は4つの頂点（四角形）で構成されているため、4頂点すべての色を書き換える
        for (int j = 0; j < 4; j++)
        {
            vertexColors[vertexIndex + j] = color;
        }

        // メッシュの頂点カラーデータを更新して画面に反映
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    /// 全ての文字の色を一括で設定するメソッド
    private void SetAllCharactersColor(Color color)
    {
        // TextMeshProの初期化を強制的に走らせてTextInfoを確定
        textComponent.ForceMeshUpdate();

        for (int i = 0; i < textComponent.textInfo.characterCount; i++)
        {
            if (textComponent.textInfo.characterInfo[i].isVisible)
            {
                SetSingleCharacterColor(i, color);
            }
        }
    }
}