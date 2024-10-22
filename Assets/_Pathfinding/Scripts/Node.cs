using System;
using UnityEngine;

namespace _Pathfinding.Pathfinding
{
    /// <summary>
    /// Represents a node in the pathfinding grid. Each node contains information 
    /// about its position, walkability, and costs for the A* algorithm.
    /// </summary>
    [Serializable]
    public class Node
    {
        // Fields
        [Header("Node Properties")]
        [Tooltip("The X position of the node in the grid.")]
        [SerializeField] private int _x;

        [Tooltip("The Y position of the node in the grid.")]
        [SerializeField] private int _y;

        [Tooltip("Indicates whether the node is walkable or blocked.")]
        [SerializeField] private bool _isWalkable;

        [HideInInspector]
        public Node ParentNode; // The node that led to this node (used for path reconstruction)

        [HideInInspector]
        public float GCost; // The cost from the start node to this node

        [HideInInspector]
        public float HCost; // The heuristic cost estimate from this node to the target

        [Header("World Position")]
        [Tooltip("The world position of the node.")]
        [SerializeField] private Vector3 _worldPosition;

        // Properties
        /// <summary>
        /// Gets the X coordinate of the node.
        /// </summary>
        public int X => _x;

        /// <summary>
        /// Gets the Y coordinate of the node.
        /// </summary>
        public int Y => _y;

        /// <summary>
        /// Gets the world position of the node.
        /// </summary>
        public Vector3 Position => _worldPosition;

        /// <summary>
        /// Indicates whether this node is walkable (accessible).
        /// </summary>
        public bool IsWalkable => _isWalkable;

        // Constructor
        /// <summary>
        /// Initializes a new instance of the Node class with grid coordinates, walkability, and world position.
        /// </summary>
        /// <param name="x">The X coordinate of the node in the grid.</param>
        /// <param name="y">The Y coordinate of the node in the grid.</param>
        /// <param name="isWalkable">Specifies if the node is walkable.</param>
        /// <param name="worldPosition">The node's world position.</param>
        public Node(int x, int y, bool isWalkable, Vector3 worldPosition)
        {
            _x = x;
            _y = y;
            _isWalkable = isWalkable;
            _worldPosition = worldPosition;
            GCost = 0f;
            HCost = 0f;
        }
    }
}