using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

using UnityEngine.UIElements;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool CanRun = false;

    public GenerateGrid GridSc;
    public GameObject HoveredTile_HighLight;
    public GameObject SelectedTile_HighLight;

    public bool isTileHovered = false;


    public
    void Start()
    {

    }

    public Vector3 MousePos()
    {
        // Get the mouse position in screen coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert the screen position to world position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Set the z-coordinate to 0 for 2D
        mouseWorldPosition.z = 0;

        return mouseWorldPosition;
    }

    private void Check_MousePos_Hovers_TilePos()
    {

        isTileHovered = false;

        // ����������ڵ� Tile λ�ã�������룩
        Vector3 mouseWorldPos = MousePos();
        Vector2 mouseGridPos = new Vector2(Mathf.FloorToInt(mouseWorldPos.x), -(Mathf.FloorToInt(mouseWorldPos.y) + 1));

        //Debug.Log("Mouse Grid Position: " + mouseGridPos);

        for (int i = 0; i < GridSc.TileList.Count; i++)
        //foreach (var Tile in GridSc.TileList)
        {
            //Debug.Log(i);
            //get tile Pos and Compare to Mouse Pos Range
            Vector2 tilePos = GridSc.TileList[i].TilePosition;
            float tileWidth = 2;
            float tileHeight = 2;
            Vector2 tileBottomRight = new Vector2(tilePos.x + tileWidth, tilePos.y + tileHeight);

            // If overlap
            if (mouseGridPos.x >= tilePos.x && mouseGridPos.x <= tileBottomRight.x &&
            mouseGridPos.y >= tilePos.y && mouseGridPos.y <= tileBottomRight.y)
            {
                isTileHovered = true;
                //Tile_HightLight.pos = Tile Pos
                HoveredTile_HighLight.SetActive(true);
                HoveredTile_HighLight.transform.position = new Vector3(GridSc.TileList[i].TilePosition.x, -GridSc.TileList[i].TilePosition.y, 0); // I have a question here!

                GridSc.CurrentHoverTile_Num = i;
            }
        }

        //If no Overlay --> Tile_HightLight --> Set Active False
        if (isTileHovered == false)
        {
            HoveredTile_HighLight.SetActive(false);
        }

    }





    // Update is called once per frame
    void Update()
    {
        //If mouse hover over any tile - highlight Tile
        if (!CanRun)
        {
            Check_MousePos_Hovers_TilePos();

            //If Mouse Click && HoverOver Tile != null
            //SelectTile = HoverTile
            //SelectHighLight Pos 

            HandleMouseClick();

            if (SelectedTile_HighLight.activeSelf)
            {
                Update_SelectedTilePos();
            }
        }

        //Input p
        //canrun = true

        //Generate Collider


    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // ������
        {
            if (isTileHovered == true) // �������Tile
            {
                SelectedTile_HighLight.SetActive(true);
                SelectedTile_HighLight.transform.position = HoveredTile_HighLight.transform.position;

                GridSc.CurrentSelectTile_Num = GridSc.CurrentHoverTile_Num;
            }
            else // ����հ�������ȡ����ǰѡ�е�Tile
            {
                SelectedTile_HighLight.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) // ����ESC��
        {
            SelectedTile_HighLight.SetActive(false);
        }
    }



    void Update_SelectedTilePos()
    {
        //wasd
        Vector3 newPosition = SelectedTile_HighLight.transform.position;

        if (Input.GetKeyDown(KeyCode.W))
        {
            newPosition.y += 1;
            SelectedTile_HighLight.transform.position = newPosition;
            GridSc.SetSelectTilePos(newPosition);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            newPosition.y -= 1;
            SelectedTile_HighLight.transform.position = newPosition;
            GridSc.SetSelectTilePos(newPosition);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            newPosition.x += 1;
            SelectedTile_HighLight.transform.position = newPosition;
            GridSc.SetSelectTilePos(newPosition);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            newPosition.x -= 1;
            SelectedTile_HighLight.transform.position = newPosition;
            GridSc.SetSelectTilePos(newPosition);
        }

        // ����λ��




    }



}
