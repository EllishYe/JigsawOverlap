using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cell : MonoBehaviour
{
    private Image cellImage;
    private Color originalColor;
    private Color currentColor;
    private int overlapCount = 0;

    private bool isOverlapping = false;  // 用于标记是否正在交叠
    private float overlapDistance = 0.5f; // 设置交叠检测的距离容忍值

    private void Awake()
    {
        cellImage = GetComponent<Image>();
    }

    // 初始化 Cell 的颜色
    public void InitializeColor(Color color)
    {
        // 如果 originalColor 还未设置，则设置为初始颜色
        if (originalColor == default(Color))
        {
            originalColor = color;
        }

        // 将 currentColor 初始化为 originalColor
        currentColor = originalColor;
        cellImage.color = currentColor;
    }

    // 当有其他 Cell 进入时触发
    private void OnTriggerEnter2D(Collider2D other)
    {

        
        if (other.CompareTag("Cell"))
        {
            overlapCount++;
            UpdateColor(other.GetComponent<Cell>());

            // 交叠发生时的调试日志
            Debug.Log($"Cell {name} entered overlap with {other.name}. Overlap count: {overlapCount}");
        }
    }

    // 当其他 Cell 离开时触发
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            overlapCount--;
            if (overlapCount <= 0)
            {
                RestoreColor(); // 退出交叠，恢复原色
            }
            // 交叠解除时的调试日志
            Debug.Log($"Cell {name} exited overlap with {other.name}. Overlap count: {overlapCount}");
        }
    }




    // 更新颜色（交叠规则）
    private void UpdateColor(Cell otherCell)
    {
        if (otherCell == null) return;

        // 只有在交叠的情况下才进行颜色合并
        if (originalColor != currentColor)
        {
            // 根据新的合并规则进行颜色更新
            if (originalColor == Color.black && otherCell.originalColor == Color.black)
            {
                currentColor = Color.white; // 黑 + 黑 = 白
            }
            else if (originalColor == Color.white && otherCell.originalColor == Color.white)
            {
                currentColor = Color.white; // 白 + 白 = 白
            }
            else
            {
                currentColor = Color.black; // 白 + 黑 = 黑
            }

            // 应用新的颜色
            cellImage.color = currentColor;

            Debug.Log($"Cell {name} color updated to: {currentColor}");
        }
        else
        {
            // 如果没有交叠，保持原始颜色
            currentColor = originalColor;
            cellImage.color = currentColor;
            Debug.Log($"Cell {name} remains at original color: {originalColor}");
        }
    }


    // 恢复初始颜色
    private void RestoreColor()
    {
        currentColor = originalColor;
        cellImage.color = originalColor;
        Debug.Log($"Cell {name} restored to original color: {originalColor}");
    }
}
