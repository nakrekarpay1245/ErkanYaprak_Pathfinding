using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Pathfinding._Abstracts
{
    /// <summary>
    /// Abstract base class responsible for moving an entity from its current node to a target node using the pathfinding algorithm.
    /// </summary>
    public abstract class AbstractNavigatorBase : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("The speed at which the entity moves along the path.")]
        [SerializeField, Range(1f, 50f)] protected float _moveSpeed = 5f;

        [SerializeField, Tooltip("The current node the pathfinder is on.")]
        protected Node _currentNode;

        protected List<Node> _path = new List<Node>();

        [HideInInspector]
        protected bool _isMoving = false;

        protected virtual void Start()
        {
            // Initialize the current node based on the entity's position in the scene
            _currentNode = NodeGrid.singleton.GetClosestNodeToWorldPosition(transform.position);
        }

        /// <summary>
        /// Abstract method that initiates the movement process towards the target node.
        /// To be implemented in derived classes.
        /// </summary>
        /// <param name="targetNode">The target node to move towards.</param>
        public abstract void NavigateTo(Node targetNode);

        /// <summary>
        /// Abstract enumerator method that follows the calculated path towards the target node step by step.
        /// Must be implemented by derived classes.
        /// </summary>
        /// <param name="path">The list of nodes representing the path.</param>
        protected abstract IEnumerator FollowPath(List<Node> path);

        /// <summary>
        /// Stops the entity's movement, if it is currently navigating.
        /// </summary>
        protected void StopNavigation()
        {
            StopAllCoroutines();
            _isMoving = false;
        }

        /// <summary>
        /// Draws gizmos in the editor to show the entity's current position and the path taken.
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if (_currentNode != null)
            {
                // Draw the current node position in red
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_currentNode.Position, 0.5f);

                // Draw the path with yellow for traversed nodes and green for untraversed nodes
                if (_path != null && _path.Count > 0)
                {
                    for (int i = 0; i < _path.Count; i++)
                    {
                        Node node = _path[i];

                        // Set color based on whether the node has been traversed
                        Gizmos.color = (i < _path.IndexOf(_currentNode)) ? Color.yellow : Color.green;

                        // Draw the node's position
                        Gizmos.DrawSphere(node.Position, 0.25f);

                        // Draw a line to the next node in the path
                        if (i < _path.Count - 1)
                        {
                            Gizmos.DrawLine(node.Position, _path[i + 1].Position);
                        }
                    }
                }
            }
        }
    }
}