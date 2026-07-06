using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("UIオブジェクトの設定")]
    [SerializeField] private List<FallingUppingUI> textAnimators;
    [SerializeField] private SceneFlashEffect flashEffect;

    [Header("出現させるボタンの設定")]
    [SerializeField] private List<GameObject> buttonsToActivate;

    [Header("タイミング設定")]
    [Tooltip("最後の文字が到着してから、光るまでの時間（秒）")]
    [SerializeField] private float timeAfterLastChar = 0.1f;

    private Coroutine sequenceCoroutine;
    private bool isSequenceFinished = false; // 演出が終わったかどうかのフラグ

    void Start()
    {
        // ゲーム開始時はボタンを非表示にする
        SetButtonsActive(false);

        // コルーチンを変数に保存して開始する（後で強制停止できるようにするため）
        sequenceCoroutine = StartCoroutine(SequenceRoutine());
    }

    void Update()
    {
        // 演出がまだ終わっていない状態で、画面がクリック（タップ）されたらスキップ
        if (!isSequenceFinished && Input.GetMouseButtonDown(0))
        {
            SkipAnimation();
        }
    }

    IEnumerator SequenceRoutine()
    {
        // 1. 文字が揃うのを待つ（約1秒）
        yield return new WaitForSeconds(1.0f);

        // 2. 少し余韻を持たせる
        if (timeAfterLastChar > 0f)
        {
            yield return new WaitForSeconds(timeAfterLastChar);
        }

        // 演出終了処理へ
        FinishSequence();
    }

    /// <summary>
    /// 演出を通常通り、またはスキップして完了させる共通処理
    /// </summary>
    private void FinishSequence()
    {
        if (isSequenceFinished) return;
        isSequenceFinished = true;

        // 3. ボタンを出現させる
        SetButtonsActive(true);

        // 4. 光の演出（パッと光ってフェードアウト）を再生
        if (flashEffect != null)
        {
            flashEffect.PlayFlash();
        }
    }

    /// <summary>
    /// クリックされたときに呼び出されるスキップ処理
    /// </summary>
    private void SkipAnimation()
    {
        if (sequenceCoroutine != null) StopCoroutine(sequenceCoroutine);

        // 文字側に追加した関数を呼び出して一瞬で整列させる
        foreach (var animator in textAnimators)
        {
            if (animator != null)
            {
                animator.ForceToGoalPosition();
            }
        }

        FinishSequence();
    }

    /// <summary>
    /// リストに登録されたボタンの表示・非表示を一括で切り替える関数
    /// </summary>
    private void SetButtonsActive(bool isActive)
    {
        if (buttonsToActivate == null) return;

        foreach (GameObject btn in buttonsToActivate)
        {
            if (btn != null)
            {
                btn.SetActive(isActive);
            }
        }
    }

}