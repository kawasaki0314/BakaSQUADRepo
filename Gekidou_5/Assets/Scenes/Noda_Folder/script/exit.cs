using UnityEngine;

public class exit : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        // Unityエディタ上で実行中の場合、再生モードを停止する
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // ビルドされたゲーム（PC / Androidなど）を終了する
            Application.Quit();
#endif
    }
}