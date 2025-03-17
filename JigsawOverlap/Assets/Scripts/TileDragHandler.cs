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
    private LayoutGroup parentLayoutGroup; // ������Ĳ������

    public float gridSize = 50f; // ���񾫶ȣ���λ����
    private bool isDragging = false; // ��ʶ��ǰ�Ƿ�����ק��



    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        // ��ȡ������Ĳ������������еĻ���
        parentLayoutGroup = transform.parent.GetComponent<LayoutGroup>();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��ǿ�ʼ��ק
        isDragging = true;

        // ����в����������������
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
        // �������Ļ����ת��Ϊ UI ��������
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out pos))
        {
            // ������ק�����������λ��
            rectTransform.anchoredPosition = pos;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // ����в����������������
        if (parentLayoutGroup != null)
        {
            parentLayoutGroup.enabled = false;
        }
        Debug.Log($"[End Drag] Tile position = {rectTransform.anchoredPosition}");


        // ��ȡ�϶�����ʱ��λ�ò�����������
        Vector2 snappedPosition = GetSnappedPosition(rectTransform.anchoredPosition);

        // ��������λ�ã�ʹ����������������
        rectTransform.anchoredPosition = snappedPosition;

        // �ָ�����͸����
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Debug.Log($"Tile {gameObject.name} dropped.");

        // �ͷź󣬼�� Cell �ĺϲ�Ч��
        Cell[] cells = GetComponentsInChildren<Cell>();

        /*if (cells.Length > 0)
            foreach (Cell cell in cells)
            {
                if (cell != this.gameObject.GetComponent<Cell>())
                {
                    print(this.gameObject);
                    this.gameObject.GetComponent<Cell>().UpdateColor(cell);
                }
            }*/


        foreach (var cell in cells)
        {
            Collider2D[] overlaps = Physics2D.OverlapPointAll(cell.transform.position);

            if (overlaps.Length > 0)
            {
                foreach (var overlap in overlaps)
                {
                    print("overlap    " + overlap);
                    if (overlap.CompareTag("Cell"))
                    {
                        Cell otherCell = overlap.GetComponent<Cell>();
                        print("otherce;;    " + otherCell);

                        otherCell.OverlapCells.Add(otherCell);
                        print(otherCell.OverlapCells.Count);

                        if (otherCell != null)
                        {
                            //cell.UpdateColor(otherCell);
                        }
                    }
                    // foreach (var overlap in overlaps)
                    // {
                    //     if (overlap.CompareTag("Cell"))
                    //     {
                    //         Cell otherCell = overlap.GetComponent<Cell>();
                    //         if (otherCell != null)
                    //         {
                    //             otherCell
                    //             //cell.SendMessage("UpdateColor", otherCell, SendMessageOptions.DontRequireReceiver);
                    //     }
                    //     }
                    // }
                }
            }
        }
    }



    // Snapping �߼�����������뵽����
    private Vector2 GetSnappedPosition(Vector2 originalPosition)
    {
        // ���������λ�ã��������뵽���������λ��
        float snappedX = Mathf.Round(originalPosition.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(originalPosition.y / gridSize) * gridSize;

        // ���ظ��º������λ��
        return new Vector2(snappedX, snappedY);
    }


    // rotating �߼��������onDrag�Ĺ�����ͬʱ���ո����룬��rotate tile
    private void Update()
    {
        if (isDragging && Input.GetKeyDown(KeyCode.Space))
        {
            // ��ת Tile 90 ��
            RotateTile();
        }
    }

    // ��ת Tile 90 ��
    private void RotateTile()
    {
        rectTransform.Rotate(0, 0, 90); // ˳ʱ����ת 90 ��
    }
}
