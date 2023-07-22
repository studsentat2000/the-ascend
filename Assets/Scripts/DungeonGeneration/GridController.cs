using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [System.Serializable]
    public struct Grid {
        public int cols, rows;
        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;

    public GameObject gridTile;

    public List<Vector2> availablePoints = new List<Vector2>();

    private void Awake()
    {
        room = GetComponentInParent<Room>();

        grid.cols = Mathf.RoundToInt(room.Width) - 1;
        grid.rows = Mathf.RoundToInt(room.Height) - 3;

        GenerateGrid();
    }

    public void GenerateGrid() {
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;

        for (int y = 0; y < grid.rows; y++) {

            for (int x = 0; x < grid.cols; x++) {
                if (x != 0 && x != grid.cols-1 && x != grid.cols-2 && y != 0 && y != grid.rows-1) {
                    GameObject go = Instantiate(gridTile, transform);

                    go.GetComponent<Transform>().position = new Vector2(x-(grid.cols - grid.horizontalOffset), y - (grid.rows - grid. verticalOffset));

                    go.name = "X: " + x + ", Y: " + y;
                

                    if (x > 4 && y < grid.rows-4 && y > 3 && x < grid.cols - 6)
                        availablePoints.Add(go.transform.position);
                }
            }
        }

        GetComponentInParent<EnemySpawner>().InitialiseObjectSpawning();
    }
}
