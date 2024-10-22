using UnityEngine;

namespace _Pathfinding.Pathfinding
{
    /// <summary>
    /// This script is used to test the Navigator class by setting a target destination 
    /// when the player clicks on a point in the game world.
    /// </summary>
    public class NavigationTest : MonoBehaviour
    {
        [Header("Navigator Reference")]
        [Tooltip("The reference to the Navigator component responsible for moving the entity.")]
        [SerializeField] private Navigator _navigator;

        [Header("Layer Mask Settings")]
        [Tooltip("The layer that is considered valid for selecting a destination.")]
        [SerializeField] private LayerMask _groundLayer;

        private Camera _mainCamera;

        private void Awake()
        {
            // Get the main camera reference
            _mainCamera = Camera.main;

            if (_navigator == null)
            {
                Debug.LogError("Navigator is not assigned. Please assign a Navigator component.");
            }
        }

        private void Update()
        {
            // Check for left mouse button click
            if (Input.GetMouseButtonDown(0))
            {
                TrySetDestination();
            }
        }

        /// <summary>
        /// Attempts to set the destination for the Navigator by detecting the point clicked on the ground.
        /// </summary>
        private void TrySetDestination()
        {
            // Raycast from the mouse position into the world
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform raycast to check for ground click
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
            {
                Node destinationNode = NodeGrid.singleton.GetNodeFromWorldPosition(hit.point);

                if (destinationNode != null && destinationNode.IsWalkable)
                {
                    // Set the destination on the Navigator
                    _navigator.NavigateTo(destinationNode);
                }
                else
                {
                    Debug.LogWarning("The selected node is not walkable or is invalid.");
                }
            }
        }
    }
}