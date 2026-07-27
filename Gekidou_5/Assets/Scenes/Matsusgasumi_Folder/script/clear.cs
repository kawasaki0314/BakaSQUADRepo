using UnityEngine;

using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour

{

    private static BGM instance;

    private AudioSource audioSource;

    [Header("BGM設定")]

    [SerializeField] private AudioClip titleBGM;

    [SerializeField] private AudioClip clearBGM;

    [SerializeField] private AudioClip gameOverBGM;

    void Awake()

    {

        if (instance != null)

        {

            Destroy(gameObject);

            return;

        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)

    {

        // シーン名に応じて再生するBGMを分岐

        if (scene.name == "startscene")

        {

            PlayBGM(titleBGM);

        }

        else if (scene.name == "Clear")

        {

            PlayBGM(clearBGM);

        }

        else if (scene.name == "GameOver")

        {

            PlayBGM(gameOverBGM);

        }

        else if (scene.name == "BattleScene")

        {

            // 戦闘シーンなど、BGMを止めたい場所では停止

            if (audioSource.isPlaying)

            {

                audioSource.Stop();

            }

        }

    }

    /// <summary>

    /// 指定されたBGMを再生する処理

    /// </summary>

    private void PlayBGM(AudioClip clip)

    {

        if (clip == null) return;

        // すでに同じ曲が流れている場合は最初から再生し直さず、そのまま鳴らし続ける

        if (audioSource.isPlaying && audioSource.clip == clip) return;

        audioSource.clip = clip;

        audioSource.Play();

    }

    void OnDestroy()

    {

        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

}
