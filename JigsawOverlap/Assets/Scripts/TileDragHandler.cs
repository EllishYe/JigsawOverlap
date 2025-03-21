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
    private LayoutGroup parentLayoutGroup; // 

    public float gridSize = 50f; // 
    private bool isDragging = false; // 



    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        parentLayoutGroup = transform.parent.GetComponent<LayoutGroup>();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {

        isDragging = true;


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
        // 
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out pos))
        {
            // 
            rectTransform.anchoredPosition = pos;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // 
        if (parentLayoutGroup != null)
        {
            parentLayoutGroup.enabled = false;
        }
        Debug.Log($"[End Drag] Tile position = {rectTransform.anchoredPosition}");


        // 
        Vector2 snappedPosition = GetSnappedPosition(rectTransform.anchoredPosition);

        // 
        rectTransform.anchoredPosition = snappedPosition;

        // 
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Debug.Log($"Tile {gameObject.name} dropped.");

        // 
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

            //if (overlaps.Length > 0)
            //{
            //    foreach (var overlap in overlaps)
            //    {
            //        print("overlap    " + overlap);
            //        if (overlap.CompareTag("Cell"))
            //        {
            //            Cell otherCell = overlap.GetComponent<Cell>();
            //            print("otherce;;    " + otherCell);

            //            otherCell.OverlapCells.Add(otherCell);
            //            print(otherCell.OverlapCells.Count);

            //            if (otherCell != null)
            //            {
            //                //cell.UpdateColor(otherCell);
            //            }
            //        }
            //        // foreach (var overlap in overlaps)
            //        // {
            //        //     if (overlap.CompareTag("Cell"))
            //        //     {
            //        //         Cell otherCell = overlap.GetComponent<Cell>();
            //        //         if (otherCell != null)
            //        //         {
            //        //             otherCell
            //        //             //cell.SendMessage("UpdateColor", otherCell, SendMessageOptions.DontRequireReceiver);
            //        //     }
            //        //     }
            //        // }
            //    }
            //}
        }
    }



    // Snapping to Grids
    private Vector2 GetSnappedPosition(Vector2 originalPosition)
    {
        
        float snappedX = Mathf.Round(originalPosition.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(originalPosition.y / gridSize) * gridSize;

        return new Vector2(snappedX, snappedY);
    }


    // Rotate Tiles
    private void Update()
    {
        if (isDragging && Input.GetKeyDown(KeyCode.Space))
        {
            // Only when dragging and hitting blankspace Button
            RotateTile();
        }
    }

    // Rotate tile 90 degrees
    private void RotateTile()
    {
        rectTransform.Rotate(0, 0, 90); // ˳ʱ����ת 90 ��
    }
}
