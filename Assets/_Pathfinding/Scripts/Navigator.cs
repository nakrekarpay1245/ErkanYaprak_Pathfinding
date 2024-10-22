using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Pathfinding.Pathfinding
{
    /// <summary>
    /// Responsible for moving an entity from its current node to a target node using the pathfinding algorithm.
    /// </summary>
    public class Navigator : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("The speed at which the entity moves along the path.")]
        [SerializeField] private float _moveSpeed = 5f;

        [SerializeField]
        private Node _currentNode;

        private List<Node> _path = new List<Node>();
        private bool _isMoving = false;

        private void Start()
        {
            _currentNode = NodeGrid.singleton.GetClosestNodeToWorldPosition(transform.position);
        }

        /// <summary>
        /// Initiates the movement process towards the target node.
        /// </summary>
        /// <param name="targetNode">The target node to move towards.</param>
        public void NavigateTo(Node targetNode)
        {
            if (Pathfinding.singleton == null)
            {
                Debug.LogError("Pathfinding component is missing!");
                return;
            }

            // Find the path to the target node
            _path = Pathfinding.singleton.FindPath(_currentNode, targetNode);

            if (_path.Count > 0)
            {
                StopNavigation();
                StartCoroutine(FollowPath(_path));
            }
        }

        /// <summary>
        /// Follows the calculated path towards the target node step by step.
        /// Moves smoothly using Unity's built-in Lerp functionality.
        /// </summary>
        /// <param name="path">The list of nodes representing the path.</param>
        private IEnumerator FollowPath(List<Node> path)
        {
            _isMoving = true;

            foreach (Node node in path)
            {
                while (Vector3.Distance(transform.position, node.Position) > 0.1f)
                {
                    // Smoothly move towards the next node using Lerp
                    transform.position = Vector3.MoveTowards(transform.position, node.Position, _moveSpeed * Time.deltaTime);
                    yield return null; // Wait for the next frame
                }

                // Update the current node after moving
                _currentNode = node;
            }

            _isMoving = false;
        }

        /// <summary>
        /// Stops the entity's movement, if it is currently navigating.
        /// </summary>
        public void StopNavigation()
        {
            StopAllCoroutines();
            _isMoving = false;
        }

        /// <summary>
        /// Draws gizmos in the editor to show the entity's current position and the path taken.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (_currentNode != null)
            {
                // Draw the current node position in yellow
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_currentNode.Position, 0.5f);

                // Draw the path with red for traversed nodes and green for untraversed nodes
                if (_path != null && _path.Count > 0)
                {
                    for (int i = 0; i < _path.Count; i++)
                    {
                        Node node = _path[i];

                        // Set color based on whether the node has been traversed
                        if (i < _path.IndexOf(_currentNode)) // If the node is before the current node
                        {
                            Gizmos.color = Color.yellow; // Traversed nodes
                        }
                        else
                        {
                            Gizmos.color = Color.green; // Untraversed nodes
                        }

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