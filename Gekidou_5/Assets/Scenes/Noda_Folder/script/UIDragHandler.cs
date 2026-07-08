using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        // 動かしたいパネルのRectTransformを取得
        rectTransform = GetComponent<RectTransform>();

        // 親にあるCanvasコンポーネントを探す
        canvas = GetComponentInParent<Canvas>();
    }

    // クリックされた（ホールド開始）ときに一番手前に表示する処理
    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling();
    }

    // ドラッグ中に呼ばれる処理
    public void OnDrag(PointerEventData eventData)
    {
        // マウスの移動量に合わせてパネルを移動させる
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}