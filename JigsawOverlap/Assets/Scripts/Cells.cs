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

    private bool isOverlapping = false;  // ���ڱ���Ƿ����ڽ���
    private float overlapDistance = 0.5f; // ���ý������ľ�������ֵ
    public List<Cell> OverlapCells;

    private void Awake()
    {
        cellImage = GetComponent<Image>();
    }

    // ��ʼ�� Cell ����ɫ
    public void InitializeColor(Color color)
    {
        // ��� originalColor ��δ���ã�������Ϊ��ʼ��ɫ
        if (originalColor == default(Color))
        {
            originalColor = color;
        }

        // �� currentColor ��ʼ��Ϊ originalColor
        currentColor = originalColor;
        cellImage.color = currentColor;
    }

    // �������� Cell ����ʱ����
    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("Cell"))
        {
            overlapCount++;
            //UpdateColor(other.GetComponent<Cell>());

            // ��������ʱ�ĵ�����־
            //Debug.Log($"Cell {name} entered overlap with {other.name}. Overlap count: {overlapCount}");
        }
    }

    // ������ Cell �뿪ʱ����
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            overlapCount--;
            if (overlapCount <= 0)
            {
                RestoreColor(); // �˳��������ָ�ԭɫ
            }
            // �������ʱ�ĵ�����־
            //Debug.Log($"Cell {name} exited overlap with {other.name}. Overlap count: {overlapCount}");
        }
    }




    // ������ɫ����������
    public void UpdateColor(Cell otherCell)
    {
        if (otherCell == null) return;

        // ֻ���ڽ���������²Ž�����ɫ�ϲ�
        // if (originalColor != currentColor)
        // {
        // �����µĺϲ����������ɫ����
        if (originalColor == Color.black && otherCell.originalColor == Color.black)
        {
            currentColor = Color.white; // �� + �� = ��
        }
        else if (originalColor == Color.white && otherCell.originalColor == Color.white)
        {
            currentColor = Color.white; // �� + �� = ��
        }
        else
        {
            currentColor = Color.black; // �� + �� = ��
        }

        // Ӧ���µ���ɫ
        cellImage.color = currentColor;

        Debug.Log($"Cell {name} color updated to: {currentColor}");
        // }
        // else
        // {
        // ���û�н���������ԭʼ��ɫ
        // currentColor = originalColor;
        // cellImage.color = currentColor;
        // Debug.Log($"Cell {name} remains at original color: {originalColor}");
        // }
    }


    // �ָ���ʼ��ɫ
    private void RestoreColor()
    {
        currentColor = originalColor;
        cellImage.color = originalColor;
        Debug.Log($"Cell {name} restored to original color: {originalColor}");
    }
}
