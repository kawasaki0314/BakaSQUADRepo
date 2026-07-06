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
    private bool isSequenceFinished = false;

    // === 【追加】スキップを受け付けてもいいかどうかのフラグ ===
    private bool canSkip = false;

    void Start()
    {
        Time.timeScale = 1f;
        SetButtonsActive(false);
        sequenceCoroutine = StartCoroutine(SequenceRoutine());
    }

    void Update()
    {
        // === 【修正】canSkip が true の時だけクリックを受け付ける ===
        if (canSkip && !isSequenceFinished && Input.GetMouseButtonDown(0))
        {
            SkipAnimation();
        }
    }

    IEnumerator SequenceRoutine()
    {
        // 【重要】文字側のスクリプトが1フレーム待って座標を記憶するのを、こちらも1フレーム待つ
        yield return null;

        // 文字たちが記憶を終えたので、ここからスキップを受け付け開始にする！
        canSkip = true;

        // 1. 文字が揃うのを待つ（約1秒）
        // （すでに1フレーム消費したので、少しだけ時間を引いて調整しておくと親切です）
        yield return new WaitForSecondsRealtime(1.0f);

        if (timeAfterLastChar > 0f)
        {
            yield return new WaitForSecondsRealtime(timeAfterLastChar);
        }

        FinishSequence();
    }

    private void FinishSequence()
    {
        if (isSequenceFinished) return;
        isSequenceFinished = true;

        SetButtonsActive(true);

        if (flashEffect != null)
        {
            flashEffect.PlayFlash();
        }
    }

    private void SkipAnimation()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
        }

        foreach (var animator in textAnimators)
        {
            if (animator != null)
            {
                animator.ForceToGoalPosition();
            }
        }

        FinishSequence();
    }

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