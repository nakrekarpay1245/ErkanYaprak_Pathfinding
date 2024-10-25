using System;
using UnityEngine;

namespace _Pathfinding.Grid
{
    /// <summary>
    /// Represents a node in the pathfinding grid. Each node contains information 
    /// about its position, walkability, and costs for the A* algorithm.
    /// </summary>
    [Serializable]
    public class Node
    {
        // Fields
        // Node Properties
        #region Node Properties
        /// <summary>
        /// The X position of the node in the grid
        /// </summary>
        private int _x;

        /// <summary>
        /// The Y position of the node in the grid
        /// </summary>
        private int _y;

        /// <summary>
        /// Indicates whether the node is walkable or blocked
        /// </summary>
        private bool _isWalkable;

        // World Position
        /// <summary>
        /// The world position of the nod
        /// </summary>
        private Vector3 _worldPosition;
        #endregion

        // Properties
        #region Properties
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
        #endregion

        // Constructor
        #region Constructor
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
        }
        #endregion
    }
}