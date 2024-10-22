using System.Collections.Generic;
using UnityEngine;
using _Pathfinding._helpers;

namespace _Pathfinding.Pathfinding
{
    /// <summary>
    /// Manages the creation and configuration of a 2D grid of nodes for pathfinding.
    /// Nodes are marked as walkable or unwalkable based on physical collisions with the environment.
    /// </summary>
    public class NodeGrid : MonoSingleton<NodeGrid>
    {
        [Header("Grid Settings")]
        [Tooltip("Width of the grid.")]
        [SerializeField] private int _gridWidth;

        [Tooltip("Height of the grid.")]
        [SerializeField] private int _gridHeight;

        [Tooltip("Size of each node in the world.")]
        [SerializeField] private float _nodeSize;

        [Tooltip("Layer mask used to determine unwalkable areas.")]
        [SerializeField] private LayerMask _unwalkableLayer;

        [Header("Grid Debugging")]
        [Tooltip("Should draw gizmos to visualize the grid?")]
        [SerializeField] private bool _drawGizmos = false;

        // The 2D array of nodes
        private Node[,] _nodeGrid;

        /// <summary>
        /// Initializes the grid by creating nodes and marking them as walkable or unwalkable based on collisions.
        /// </summary>
        private void Awake()
        {
            CreateGrid();
        }

        /// <summary>
        /// Creates the grid of nodes based on the specified width, height, and node size.
        /// Checks for collisions with objects on the specified layer and marks nodes accordingly.
        /// </summary>
        private void CreateGrid()
        {
            _nodeGrid = new Node[_gridWidth, _gridHeight];
            Vector3 bottomLeft = transform.position - Vector3.right * _gridWidth / 2 - Vector3.forward * _gridHeight / 2;

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    // Calculate the world position for the current node
                    Vector3 worldPosition = bottomLeft + Vector3.right * (x * _nodeSize) + Vector3.forward * (y * _nodeSize);
                    // Check for collisions with the unwalkable layer
                    bool isWalkable = !Physics.CheckSphere(worldPosition, _nodeSize / 2, _unwalkableLayer);

                    // Create and store the node
                    _nodeGrid[x, y] = new Node(x, y, isWalkable, worldPosition);
                }
            }
        }

        /// <summary>
        /// Returns the node at the specified world position by converting the position to grid coordinates.
        /// </summary>
        /// <param name="worldPosition">The world position to convert.</param>
        /// <returns>The corresponding node in the grid.</returns>
        public Node GetNodeFromWorldPosition(Vector3 worldPosition)
        {
            float percentX = Mathf.Clamp01((worldPosition.x + _gridWidth / 2) / _gridWidth);
            float percentY = Mathf.Clamp01((worldPosition.z + _gridHeight / 2) / _gridHeight);

            int x = Mathf.RoundToInt((_gridWidth - 1) * percentX);
            int y = Mathf.RoundToInt((_gridHeight - 1) * percentY);

            return _nodeGrid[x, y];
        }

        /// <summary>
        /// Returns the list of neighbors for the given node, considering 8 directions (up, down, left, right, and diagonals).
        /// </summary>
        /// <param name="node">The node for which neighbors are to be found.</param>
        /// <returns>A list of neighboring nodes.</returns>
        public List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            // Loop through all adjacent positions (up, down, left, right, and diagonals)
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Skip the node itself
                    if (x == 0 && y == 0) continue;

                    int checkX = node.X + x;
                    int checkY = node.Y + y;

                    // Check if the neighboring position is within bounds
                    if (checkX >= 0 && checkX < _gridWidth && checkY >= 0 && checkY < _gridHeight)
                    {
                        neighbors.Add(_nodeGrid[checkX, checkY]);
                    }
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Draws gizmos to visualize the grid in the scene view.
        /// Useful for debugging the node layout and walkability.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (_nodeGrid != null && _drawGizmos)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    for (int y = 0; y < _gridHeight; y++)
                    {
                        Node node = _nodeGrid[x, y];
                        Gizmos.color = node.IsWalkable ? Color.white : Color.red;
                        Gizmos.DrawWireCube(node.Position, new Vector3((_nodeSize - 0.1f), 0.1f, (_nodeSize - 0.1f)));
                    }
                }
            }
        }
    }
}