using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinding))]
[RequireComponent(typeof(PathRequestManager))]
public class MyGrid : MonoBehaviour
{
    public static bool is2DMode;

    public bool displayGridGizmos;

    [Header("Основные параметры")]
    public bool is2D;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    [Header("Штрафы на передвижение по слоям")]
    public TerrainType[] walkableRegions;

    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    Node[,] grid;

    float nodeDiameter;   
    int gridSizeX, gridSizeY;

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    private void Awake()
    {
        is2DMode = is2D;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value;
            walkableRegionsDictionary.Add( (int) Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 upVector = (is2D) ? Vector3.up : Vector3.forward;
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - upVector * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeX; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + upVector * (y * nodeDiameter + nodeRadius);
                bool walkable = (is2D) ? (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask) == null)
                    : !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementPenalty = 0;
                
                if (walkable)
                {
                    if (is2D)
                    {
                        RaycastHit2D hit = Physics2D.CircleCast(worldPoint, nodeRadius, Vector2.up, 0.1f, walkableMask, 0, 50);
                        if (hit)
                        {
                            walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                        }
                    }
                    else
                    {
                        Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                        if (Physics.Raycast(ray, out RaycastHit hit, 100, walkableMask))
                        {
                            walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                        }
                    }
                }

                //Debug.Log("walkable " + walkable + ", worldPoint " + worldPoint + ", x=" + x + ", y=" + y + ", movementPenalty = " + movementPenalty);

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) { continue; }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;

        float worldPosY = (is2D) ? worldPosition.y : worldPosition.z;
        float percentY = (worldPosY + gridWorldSize.y / 2) / gridWorldSize.y;
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        if (is2D)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        }

        if (grid != null && displayGridGizmos)
        {
            /*for (int x = 0; x < 23; x++)
            {
                string str = "";
                for (int y = 0; y < 23; y++)
                {
                    str = str + grid[x, y].walkable + ", ";
                }

                Debug.Log(str);
            }*/

            foreach (Node n in grid)
            {
                Gizmos.color = n.walkable ? Color.green : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
