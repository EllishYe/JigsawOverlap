using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Cell;
    public List<GameObject> CellList;
    public List<Tile> TileList;

    [System.Serializable]
    public class Tile
    {
        public Vector2 TilePosition;
        public int3x3 Colors;
        public int Rotation;

        public Tile(Vector2 tP, int3x3 Cs)
        {
            this.TilePosition = tP;
            this.Colors = Cs;
        }

        public Tile()
        {
            TilePosition = new Vector2(0, 0);
            Colors = new int3x3(0, 0, 0, 0, 0, 0, 0, 0, 0);
            Rotation = 0;
        }

        public void RotateTile()
        {
            // Normalize the rotation to 0, 90, 180, or 270
            int normalizedRotation = ((Rotation % 360) + 360) % 360;

            // Create a new matrix to store the rotated values
            int3x3 rotatedMatrix = Colors;

            if (normalizedRotation == 90)
            {
                rotatedMatrix = Rotate90Clockwise(Colors);
            }
            else if (normalizedRotation == 180)
            {
                rotatedMatrix = Rotate180(Colors);
            }
            else if (normalizedRotation == 270)
            {
                rotatedMatrix = Rotate270Clockwise(Colors);
            }

            // Update the Colors matrix with the rotated version
            Colors = rotatedMatrix;

            // Keep the rotation cumulative
            Rotation = 0; // Reset to 0 after applying the rotation
        }

        private int3x3 Rotate90Clockwise(int3x3 matrix)
        {
            return new int3x3(
                new int3(matrix.c0.z, matrix.c1.z, matrix.c2.z), // Column 0 becomes Row 2
                new int3(matrix.c0.y, matrix.c1.y, matrix.c2.y), // Column 1 becomes Row 1
                new int3(matrix.c0.x, matrix.c1.x, matrix.c2.x)  // Column 2 becomes Row 0
            );
        }

        private int3x3 Rotate180(int3x3 matrix)
        {
            return new int3x3(
                new int3(matrix.c2.z, matrix.c1.z, matrix.c0.z), // Row 2 reversed
                new int3(matrix.c2.y, matrix.c1.y, matrix.c0.y), // Row 1 reversed
                new int3(matrix.c2.x, matrix.c1.x, matrix.c0.x)  // Row 0 reversed
            );
        }

        private int3x3 Rotate270Clockwise(int3x3 matrix)
        {
            return new int3x3(
                new int3(matrix.c2.x, matrix.c1.x, matrix.c0.x), // Column 2 becomes Row 0
                new int3(matrix.c2.y, matrix.c1.y, matrix.c0.y), // Column 1 becomes Row 1
                new int3(matrix.c2.z, matrix.c1.z, matrix.c0.z)  // Column 0 becomes Row 2
            );
        }


    }
    void Start()
    {


    }

    private void GenerateGridNow()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                GameObject newcell = Instantiate(Cell);
                newcell.transform.position = new Vector3(i, -j, 0);
                newcell.name = "Cell " + i.ToString() + "_" + j.ToString();
                newcell.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
                CellList.Add(newcell);
            }
        }
    }

    public void UpdateCellColor()
    {
        foreach (GameObject Cell in CellList)
        {
            Vector2 CellPos = new Vector2(Cell.transform.position.x, Cell.transform.position.y);
            List<int> ColorsOverlayed = new List<int>();

            foreach (Tile tile in TileList)
            {
                //each cell in tile
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Vector2 CurrentCellPos = new Vector2(tile.TilePosition.x + i, -(tile.TilePosition.y + j));
                        //int PreviousColor = 2;

                        if ((CurrentCellPos - CellPos).magnitude <= .1)
                        {
                            ColorsOverlayed.Add(tile.Colors[i][j]);
                        }
                    }
                }
            }

            if (ColorsOverlayed.Count > 0)
            {
                int colorNow = 2;
                for (int i = 0; i <= ColorsOverlayed.Count; i++)
                {
                    if (i == 0)
                    {
                        colorNow = ColorsOverlayed[0];
                    }
                    else
                    {
                        //if(ColorsOverlayed[i]) 跟 colorNow对比，然后替换colorNow，直到得到最终的颜色
                    }
                }

                if (colorNow == 0)
                {
                    Cell.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;

                }
                else if (colorNow == 1)
                {
                    Cell.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;

                }

            }
            else
            {
                Cell.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateGridNow();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateCellColor();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (Tile tile in TileList)
            {
                if (tile.Rotation != 0)
                    tile.RotateTile();
            }
            UpdateCellColor();
        }


    }
}

