using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f; // 動くスピード
    [SerializeField] private float height = 10.0f; // 動く幅（高さ）

    private Vector3 startPosition;
    private RectTransform rectTransform;
    private bool isUI;

    void Start()
    {
        // UI（Canvas内）か、通常のオブジェクトかを判定
        rectTransform = GetComponent<RectTransform>();
        isUI = rectTransform != null;

        if (isUI)
        {
            startPosition = rectTransform.anchoredPosition;
        }
        else
        {
            startPosition = transform.position;
        }
    }

    void Update()
    {
        // Mathf.Sinを使って上下の往復値を計算
        float newY = Mathf.Sin(Time.time * speed) * height;

        if (isUI)
        {
            rectTransform.anchoredPosition = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
        }
        else
        {
            transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
        }
    }
}