using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class TileGenerator : MonoBehaviour
{
    public TMP_InputField sizeInput; // 输入模数（例如3）
    public TMP_InputField ratioInput; // 输入黑白比（例如8:1）
    public Button generateButton; // 生成按钮
    public Transform tileParent; // 用于存放生成的Tile
    public GameObject tilePrefab; // Tile预制体

    private List<GameObject> generatedTiles = new List<GameObject>(); // 存储生成的Tile

    void Start()
    {
        // 生成按钮
        generateButton.onClick.AddListener(GenerateTiles);
        

    }

    void GenerateTiles()
    {
        // 读取输入的模数和黑白比
        int size = int.Parse(sizeInput.text);
        string[] ratio = ratioInput.text.Split(':');
        int blackCount = int.Parse(ratio[0]);
        int whiteCount = int.Parse(ratio[1]);

        if (size * size != blackCount + whiteCount)
        {
            Debug.LogError("黑白比总和必须等于网格大小！");
            return;
        }

        // 存储生成的唯一图案
        HashSet<string> uniquePatterns = new HashSet<string>();

        // 生成新Tile，直到填满或满足唯一性
        int attempts = 0;
        int maxAttempts = 1000;

        while (uniquePatterns.Count < 5 && attempts < maxAttempts) // 生成最多5个Tile
        {
            attempts++;
            int[,] pattern = GeneratePattern(size, blackCount, whiteCount);

            // 将图案转换为字符串用于唯一性判定
            string patternKey = GetPatternKey(pattern);
            bool isUnique = true;

            // 旋转 4 次检查唯一性
            for (int i = 0; i < 4; i++)
            {
                patternKey = RotatePattern(patternKey, size);
                if (uniquePatterns.Contains(patternKey))
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
                uniquePatterns.Add(patternKey);
                CreateTile(size, pattern);
            }
        }
    }

    // 生成黑白图案
    int[,] GeneratePattern(int size, int blackCount, int whiteCount)
    {
        int[,] pattern = new int[size, size];
        List<Vector2Int> positions = new List<Vector2Int>();

        // 将所有可能的位置加入到列表中
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                positions.Add(new Vector2Int(x, y));
            }
        }

        // 随机放置黑色格子
        for (int i = 0; i < blackCount; i++)
        {
            int index = Random.Range(0, positions.Count);
            Vector2Int pos = positions[index];
            pattern[pos.y, pos.x] = 1;
            positions.RemoveAt(index);
        }

        return pattern;
    }

    // 将图案转换为唯一性字符串
    string GetPatternKey(int[,] pattern)
    {
        int size = pattern.GetLength(0);
        string key = "";
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                key += pattern[y, x].ToString();
            }
        }
        return key;
    }

    // 旋转图案（90°）
    string RotatePattern(string patternKey, int size)
    {
        int[,] pattern = new int[size, size];
        int index = 0;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                pattern[y, x] = patternKey[index] - '0';
                index++;
            }
        }

        int[,] rotatedPattern = new int[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                rotatedPattern[x, size - 1 - y] = pattern[y, x];
            }
        }

        return GetPatternKey(rotatedPattern);
    }

    // 生成Tile对象
    public void CreateTile(int size, int[,] pattern)
    {
        Debug.Log("Creating tile...");

        // 确认 tileParent 的大小和缩放
        tileParent.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 600);
        tileParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        tileParent.localScale = Vector3.one;

        // 生成 tile
        GameObject tile = Instantiate(tilePrefab, tileParent);
        tile.transform.localScale = Vector3.one;

        RectTransform tileRect = tile.GetComponent<RectTransform>();
        tileRect.sizeDelta = new Vector2(size * 50, size * 50); // 修改为合适的大小
        tileRect.anchoredPosition = new Vector2(0, 0);

        // 🌟 添加 TileDragHandler 组件（用于拖拽）
        if (tile.GetComponent<TileDragHandler>() == null)
        {
            tile.AddComponent<TileDragHandler>();
        }

        // 设置 GridLayoutGroup 的属性
        GridLayoutGroup grid = tile.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            grid = tile.AddComponent<GridLayoutGroup>();
        }
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = size;
        grid.cellSize = new Vector2(50, 50);
        grid.spacing = new Vector2(0, 0);
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment = TextAnchor.MiddleCenter;

        // 创建 cell
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject cell = new GameObject($"Cell_{x}_{y}", typeof(RectTransform), typeof(Image));
                cell.transform.SetParent(tile.transform);

                RectTransform cellRect = cell.GetComponent<RectTransform>();
                cellRect.sizeDelta = new Vector2(50, 50);
                cellRect.anchorMin = new Vector2(0.5f, 0.5f);
                cellRect.anchorMax = new Vector2(0.5f, 0.5f);
                cellRect.pivot = new Vector2(0.5f, 0.5f);
                cell.transform.localScale = Vector3.one;

                // 设置 tag方便后续cell的overlap检测
                cell.tag = "Cell";

                //  设置Rigidbody2D
                Rigidbody2D rb = cell.AddComponent<Rigidbody2D>();

                // 设置 Rigidbody2D 的属性，确保适合动态物体
                rb.isKinematic = false; // 设置为动态物体
                rb.gravityScale = 0; // 不受重力影响，如果需要受重力，可以设置为其他值

                // 设置颜色
                Image img = cell.GetComponent<Image>();
                Color cellColor = (pattern[y, x] == 1) ? Color.black : Color.white;
                img.color = cellColor;

                Debug.Log($"Cell created at [{x},{y}] with color: {(pattern[y, x] == 1 ? "Black" : "White")}");

                
                
                // 添加 BoxCollider2D 以支持重叠检测
                BoxCollider2D collider = cell.GetComponent<BoxCollider2D>();
                if (collider == null)
                {
                    collider = cell.AddComponent<BoxCollider2D>(); // 添加缺失的组件；初始cell均无collider，则动态添加
                }

                collider.size = new Vector2(50, 50);
                collider.isTrigger = true;

                //  挂载 Cell.cs 脚本
                Cell cellScript = cell.AddComponent<Cell>();
                cellScript.InitializeColor(cellColor);
                
            }
        }

        // 🌟 添加 CanvasGroup（便于控制拖拽时的透明度和射线检测）
        CanvasGroup canvasGroup = tile.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tile.AddComponent<CanvasGroup>();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        // 添加到保存列表
        generatedTiles.Add(tile);

        Debug.Log("Tile generated and added to list.");
    }






}
