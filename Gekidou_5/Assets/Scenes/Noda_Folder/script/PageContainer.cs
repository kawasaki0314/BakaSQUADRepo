using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    private RectTransform pageContainer;
    private float pageWidth = 1920f;
    private float slideSpeed = 10f;
    private Vector3 targetPosition;
    private bool isSliding = false;

    void Start()
    {
        // 1. 時間の流れをリセット
        Time.timeScale = 1f;

        // 2. 今のシーンの PageContainer を取得
        GameObject containerObj = GameObject.Find("PageContainer");
        if (containerObj != null)
        {
            pageContainer = containerObj.GetComponent<RectTransform>();
            pageContainer.anchoredPosition = Vector3.zero;
        }
        targetPosition = Vector3.zero;

        // 3. 今のシーンの「次へボタン」を名前で探して、処理をその場で合体させる
        GameObject nextBtnObj = GameObject.Find("NextButton"); // ※ボタンのオブジェクト名に合わせてください
        if (nextBtnObj != null)
        {
            Button btn = nextBtnObj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners(); // 一旦綺麗にする
            btn.onClick.AddListener(GoToPage2);
        }

        // 4. 今のシーンの「戻るボタン」を名前で探して、処理をその場で合体させる
        GameObject backBtnObj = GameObject.Find("BackButton"); // ※ボタンのオブジェクト名に合わせてください
        if (backBtnObj != null)
        {
            Button btn = backBtnObj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners(); // 一旦綺麗にする
            btn.onClick.AddListener(GoToPage1);
        }
    }

    void Update()
    {
        if (isSliding && pageContainer != null)
        {
            pageContainer.anchoredPosition = Vector3.Lerp(
                pageContainer.anchoredPosition,
                targetPosition,
                Time.deltaTime * slideSpeed
            );

            if (Vector3.Distance(pageContainer.anchoredPosition, targetPosition) < 0.1f)
            {
                pageContainer.anchoredPosition = targetPosition;
                isSliding = false;
            }
        }
    }

    public void GoToPage2()
    {
        //Debug.Log($"[PageManager] GoToPage2が実行されました。ターゲット座標: {new Vector3(-pageWidth, 0, 0)}");
        targetPosition = new Vector3(-pageWidth, 0, 0);
        isSliding = true;
    }

    public void GoToPage1()
    {
        //Debug.Log("[PageManager] GoToPage1が実行されました。");
        targetPosition = Vector3.zero;
        isSliding = true;
    }
}