using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Pathfinding._Abstracts;
using _Pathfinding.Grid;
using _Pathfinding.PathfindingSystem;

namespace _Pathfinding.Navigators
{
    /// <summary>
    /// Handles the movement and navigation logic specific to a UFO entity.
    /// Inherits from AbstractNavigatorBase and implements custom UFO movement logic.
    /// Accelerates at the start, maintains a constant speed, and decelerates at the final node,
    /// while smoothly rotating towards each node on the Y-axis.
    /// </summary>
    public class Navigator : AbstractNavigatorBase
    {
        [Header("Movement Settings")]
        [Tooltip("Acceleration applied when the UFO starts moving.")]
        [SerializeField, Range(0.1f, 10f)] private float _moveAcceleration = 2f;

        [Tooltip("Deceleration applied when the UFO is stopping.")]
        [SerializeField, Range(0.1f, 10f)] private float _moveDeceleration = 2f;

        [Tooltip("The threshold distance to the final target node at which the UFO will start slowing down.")]
        [SerializeField, Range(0.001f, 1f)] private float _distanceThreshold = 1f;

        [Tooltip("The distance at which the UFO considers it has reached the final target.")]
        [SerializeField, Range(0.001f, 1f)] private float _stoppingDistance = 0.5f;

        [Header("Rotation Settings")]
        [Tooltip("Speed at which the UFO rotates towards each node.")]
        [SerializeField, Range(0.1f, 25f)] private float _rotationSpeed = 2f;

        private float _currentSpeed = 0f; // Track the current movement speed
        private bool _isFinalNode = false; // Track if we are on the final node

        protected override void Start()
        {
            base.Start();
            _currentSpeed = 0f; // Initialize current speed to 0
        }

        /// <summary>
        /// Starts navigating the UFO towards the target node.
        /// This method calculates the path using the pathfinding system.
        /// </summary>
        /// <param name="targetNode">The target node to navigate towards.</param>
        public override void NavigateTo(Node targetNode)
        {
            if (targetNode == null)
            {
                Debug.LogError("Target node is null! Cannot navigate.");
                return;
            }

            // Use base class logic to calculate the path
            _path = Pathfinding.singleton.FindPath(_currentNode, targetNode);

            if (_path.Count > 0)
            {
                StopNavigation(); // Stop any ongoing movement
                StartCoroutine(FollowPath(_path)); // Follow the new path
            }
        }

        /// <summary>
        /// Follows the calculated path to the target node, applying acceleration at the start,
        /// maintaining a constant speed, and decelerating only when approaching the final node.
        /// Smoothly rotates towards each node on the Y-axis as it navigates.
        /// </summary>
        /// <param name="path">The list of nodes representing the path.</param>
        protected override IEnumerator FollowPath(List<Node> path)
        {
            _isMoving = true;
            _isFinalNode = false;
            _currentSpeed = 0f; // Reset speed at the beginning of movement

            for (int i = 0; i < path.Count; i++)
            {
                Node node = path[i];

                if (i == path.Count - 1)
                {
                    _isFinalNode = true; // Mark this as the final node
                }

                while (Vector3.Distance(transform.position, node.Position) > _stoppingDistance)
                {
                    // Calculate distance to the final node (if it's the last one)
                    float distanceToNode = Vector3.Distance(transform.position, node.Position);

                    if (!_isFinalNode)
                    {
                        // Accelerate at the start of the movement
                        _currentSpeed = Mathf.Min(_moveSpeed, _currentSpeed + _moveAcceleration * Time.deltaTime);
                    }
                    else if (distanceToNode < _distanceThreshold)
                    {
                        // Decelerate as we approach the final node
                        _currentSpeed = Mathf.Max(0f, _currentSpeed - _moveDeceleration * Time.deltaTime);
                    }

                    // Move towards the target node
                    transform.position = Vector3.MoveTowards(transform.position, node.Position, _currentSpeed * Time.deltaTime);

                    // Smoothly rotate towards the node on the Y-axis
                    RotateTowardsNode(node.Position);

                    yield return null; // Wait for the next frame
                }

                // Update the current node after reaching it
                _currentNode = node;
            }

            _isMoving = false;
        }

        /// <summary>
        /// Smoothly rotates the UFO towards a specified target position on the Y-axis.
        /// </summary>
        /// <param name="targetPosition">The position to rotate towards.</param>
        private void RotateTowardsNode(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Draws gizmos in the editor to visualize the UFO's current position and the path.
        /// </summary>
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos(); // Call base method to draw the current node and path

            // Optionally, draw additional gizmos specific to the UFO's movement
            if (_currentNode != null)
            {
                // Draw a wire sphere around the UFO to show its stopping distance
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, _stoppingDistance);
            }
        }
    }
}