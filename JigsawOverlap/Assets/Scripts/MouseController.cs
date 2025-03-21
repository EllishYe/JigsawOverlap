using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update

    public GenerateGrid GridSc;
    public GameObject Tile_HightLight;

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

    private void CheckMouse_TilePos()
    {
        foreach (var tile in GridSc.TileList)
        {
            //get tile Pos and Compare to Mouse Pos
            //If Overlap
            //Tile_HightLight.pos = Tile Pos
            //If overlay with several tiles - pick the last one
        }

        //If no Overlay --> Tile_HightLight.pos = Out of Screen
        //Save to HoverTile in Grid script
    }

    void UpdateTilePos()
    {
        //wasd
        //GridSc.CurrentSelectTile.TilePosition Update
    }

    // Update is called once per frame
    void Update()
    {
        //If mouse hover over any tile - highlight Tile
        CheckMouse_TilePos();

        //If Mouse Click && HoverOver Tile != null
        //SelectTile = HoverTile
        //SelectHighLight Pos 

        UpdateTilePos();

    }
}
