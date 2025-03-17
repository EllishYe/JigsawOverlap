using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

            Rotation = 0;
        }

        private int3x3 Rotate90Clockwise(int3x3 matrix)
        {
            return new int3x3(
                matrix[2][0], matrix[1][0], matrix[0][0], // Row 0 becomes Column 2
                matrix[2][1], matrix[1][1], matrix[0][1], // Row 1 becomes Column 1
                matrix[2][2], matrix[1][2], matrix[0][2]  // Row 2 becomes Column 0
            );
        }

        private int3x3 Rotate180(int3x3 matrix)
        {
            return new int3x3(
                matrix[2][2], matrix[2][1], matrix[2][0], // Row 2 reversed
                matrix[1][2], matrix[1][1], matrix[1][0], // Row 1 reversed
                matrix[0][2], matrix[0][1], matrix[0][0]  // Row 0 reversed
            );
        }

        private int3x3 Rotate270Clockwise(int3x3 matrix)
        {
            return new int3x3(
                matrix[0][2], matrix[1][2], matrix[2][2], // Row 0 becomes Column 0
                matrix[0][1], matrix[1][1], matrix[2][1], // Row 1 becomes Column 1
                matrix[0][0], matrix[1][0], matrix[2][0]  // Row 2 becomes Column 2
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
                newcell.name = "Cell" + i.ToString() + j.ToString();
                CellList.Add(newcell);
            }
        }
    }

    public void UpdateCellColor()
    {
        foreach (GameObject Cell in CellList)
        {
            Vector2 CellPos = Cell.transform.position;
            foreach (Tile tile in TileList)
            {
                int PreviousColor = 2;
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        Vector2 CurrentCellPos = new Vector2(tile.TilePosition.x + i, tile.TilePosition.y - j);
                        if (CurrentCellPos == CellPos)
                        {
                            if (PreviousColor == 2)
                            {
                                PreviousColor = tile.Colors[i][j];
                            }
                            else
                            {
                                //Overlap Caculation here...
                            }

                            Color CellColor = Color.gray;

                            if (PreviousColor == 0)
                            {
                                CellColor = Color.black;
                            }
                            else if (PreviousColor == 1)
                            {
                                CellColor = Color.white;
                            }

                            Cell.transform.GetChild(0).GetComponent<Renderer>().material.color = CellColor;
                        }
                    }
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
                tile.RotateTile();
            }
        }


    }
}
