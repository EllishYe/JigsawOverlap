using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private LayoutGroup parentLayoutGroup; // 父物体的布局组件

    public float gridSize = 50f; // 网格精度，单位像素
    private bool isDragging = false; // 标识当前是否在拖拽中


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        // 获取父物体的布局组件（如果有的话）
        parentLayoutGroup = transform.parent.GetComponent<LayoutGroup>();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        // 标记开始拖拽
        isDragging = true;

        // 如果有布局组件，禁用它们
        if (parentLayoutGroup != null)
        {
            parentLayoutGroup.enabled = false;
        }

        Debug.Log($"[Start Drag] Tile position = {rectTransform.anchoredPosition}");
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        // 将鼠标屏幕坐标转换为 UI 世界坐标
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out pos))
        {
            // 更新拖拽过程中物体的位置
            rectTransform.anchoredPosition = pos;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // 如果有布局组件，禁用它们
        if (parentLayoutGroup != null)
        {
            parentLayoutGroup.enabled = false;
        }
        Debug.Log($"[End Drag] Tile position = {rectTransform.anchoredPosition}");


        // 获取拖动结束时的位置并计算吸附点
        Vector2 snappedPosition = GetSnappedPosition(rectTransform.anchoredPosition);

        // 更新物体位置，使其吸附到网格中心
        rectTransform.anchoredPosition = snappedPosition;

        // 恢复正常透明度
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Debug.Log($"Tile {gameObject.name} dropped.");

        // 释放后，检查 Cell 的合并效果
        Cell[] cells = GetComponentsInChildren<Cell>();
        foreach (var cell in cells)
        {
            Collider2D[] overlaps = Physics2D.OverlapPointAll(cell.transform.position);

            foreach (var overlap in overlaps)
            {
                if (overlap.CompareTag("Cell"))
                {
                    Cell otherCell = overlap.GetComponent<Cell>();
                    if (otherCell != null)
                    {
                        cell.SendMessage("UpdateColor", otherCell, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }



    // Snapping 逻辑：让物体对齐到网格
    private Vector2 GetSnappedPosition(Vector2 originalPosition)
    {
        // 计算物体的位置，四舍五入到最近的网格位置
        float snappedX = Mathf.Round(originalPosition.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(originalPosition.y / gridSize) * gridSize;

        // 返回更新后的吸附位置
        return new Vector2(snappedX, snappedY);
    }

    
    // rotating 逻辑：如果在onDrag的过程中同时检测空格输入，则rotate tile
    private void Update()
    {
        if (isDragging && Input.GetKeyDown(KeyCode.Space))
        {
            // 旋转 Tile 90 度
            RotateTile();
        }
    }

    // 旋转 Tile 90 度
    private void RotateTile()
    {
        rectTransform.Rotate(0, 0, 90); // 顺时针旋转 90 度
    }
}
