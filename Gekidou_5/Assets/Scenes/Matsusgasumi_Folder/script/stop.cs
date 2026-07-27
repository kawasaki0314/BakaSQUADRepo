using UnityEngine;

public class PlayBGMOnStart : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private float fadeDuration = 1.0f;

    // Start ではなく Awake に変更して、シーン読み込み直後に最優先で実行させる
    void Awake()
    {
        //if (BGM.Instance != null && bgmClip != null)
        {
          //  BGMManager.Instance.PlayBGM(bgmClip, fadeDuration);
        }
    }
}