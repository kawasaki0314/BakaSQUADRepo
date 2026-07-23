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
    private bool canSkip = false;

    void Start()
    {
        Time.timeScale = 1f;
        SetButtonsActive(false);
        sequenceCoroutine = StartCoroutine(SequenceRoutine());
    }

    void Update()
    {
        if (canSkip && !isSequenceFinished && Input.GetMouseButtonDown(0))
        {
            SkipAnimation();
        }
    }

    IEnumerator SequenceRoutine()
    {
        yield return null;

        canSkip = true;

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

        // フラッシュ演出を再生（内部でフラッシュSEも鳴ります）
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