using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarNode
{
    public Vector3Int Position { get; set; }
    public AStarNode Parent { get; set; }
    public float G { get; set; } // Cost from start to current node
    public float H { get; set; } // Heuristic cost from current to target
    public float F => G + H;     // Total cost

    public AStarNode(Vector3Int position, AStarNode parent, float g, float h)
    {
        Position = position;
        Parent = parent;
        G = g;
        H = h;
    }
}

public class PathManager : ManagerBase
{
    [SerializeField] private Tilemap checkTilemap;
    [SerializeField] private TileBase emptyTile;

    public Vector3 GetTileWorldPosition(Vector3Int tilePosition)
    {
        return checkTilemap.CellToWorld(tilePosition) + checkTilemap.cellSize * 0.5f;
    }

    public List<Vector3Int> GetRandomPath(Vector3 currentPosition, int searchRadius = 10)
    {
        Vector3Int currentCell = checkTilemap.WorldToCell(currentPosition);
        Vector3Int targetCell = GetRandomWalkableTile(currentCell, searchRadius);

        if (targetCell == Vector3Int.zero) return null;

        return FindPath(currentCell, targetCell);
    }

    private Vector3Int GetRandomWalkableTile(Vector3Int center, int size)
    {
        List<Vector3Int> walkableTiles = new List<Vector3Int>();

        for (int x = center.x - size / 2; x <= center.x + size / 2; x++)
        {
            for (int y = center.y - size / 2; y <= center.y + size / 2; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (IsWalkable(position))
                {
                    walkableTiles.Add(position);
                }
            }
        }

        if (walkableTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, walkableTiles.Count);
            return walkableTiles[randomIndex];
        }

        Debug.LogWarning("No walkable positions found.");
        return Vector3Int.zero;
    }

    #region A* Algorithm
    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
    {
        List<AStarNode> openList = new List<AStarNode>();
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();

        AStarNode startNode = new AStarNode(start, null, 0, GetHeuristic(start, target));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            AStarNode currentNode = GetLowestFScoreNode(openList);

            if (currentNode.Position == target)
            {
                return ConstructPath(currentNode);
            }

            openList.Remove(currentNode);
            closedSet.Add(currentNode.Position);

            foreach (var neighborPos in GetNeighbors(currentNode.Position))
            {
                if (closedSet.Contains(neighborPos) || !IsWalkable(neighborPos))
                    continue;

                float tentativeGScore = currentNode.G + GetDistance(currentNode.Position, neighborPos);

                AStarNode neighborNode = openList.Find(n => n.Position == neighborPos);

                if (neighborNode == null)
                {
                    neighborNode = new AStarNode(neighborPos, currentNode, tentativeGScore, GetHeuristic(neighborPos, target));
                    openList.Add(neighborNode);
                }
                else if (tentativeGScore < neighborNode.G)
                {
                    neighborNode.G = tentativeGScore;
                    neighborNode.Parent = currentNode;
                }
            }
        }

        return null; // Path not found
    }

    private AStarNode GetLowestFScoreNode(List<AStarNode> nodes)
    {
        AStarNode lowest = nodes[0];

        foreach (var node in nodes)
        {
            if (node.F < lowest.F)
            {
                lowest = node;
            }
        }
        return lowest;
    }

    private float GetHeuristic(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private float GetDistance(Vector3Int a, Vector3Int b)
    {
        return Vector3Int.Distance(a, b);
    }

    private IEnumerable<Vector3Int> GetNeighbors(Vector3Int position)
    {
        yield return new Vector3Int(position.x + 1, position.y, position.z);
        yield return new Vector3Int(position.x - 1, position.y, position.z);
        yield return new Vector3Int(position.x, position.y + 1, position.z);
        yield return new Vector3Int(position.x, position.y - 1, position.z);
    }

    private bool IsWalkable(Vector3Int position)
    {
        return checkTilemap.GetTile(position) == emptyTile;
    }

    private List<Vector3Int> ConstructPath(AStarNode node)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        while (node != null)
        {
            path.Add(node.Position);
            node = node.Parent;
        }

        path.Reverse();
        return path;
    }
    #endregion
}
