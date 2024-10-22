using _Pathfinding._helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace _Pathfinding.Pathfinding
{
    /// <summary>
    /// Provides A* pathfinding functionality for navigating through nodes.
    /// </summary>
    public class Pathfinding : MonoSingleton<Pathfinding>
    {
        [Header("Gizmos Settings")]
        [Tooltip("Radius of the nodes in the Gizmos.")]
        [SerializeField]
        private float _closedNodeGizmosRadius = 0.35f;

        [Tooltip("Radius of the nodes in the Gizmos.")]
        [SerializeField]
        private float _openNodeGizmosRadius = 0.15f;

        [Tooltip("Color of the closed nodes in the Gizmos.")]
        [SerializeField] private Color closedNodeColor = Color.gray;

        [Tooltip("Color of the open nodes in the Gizmos.")]
        [SerializeField] private Color openNodeColor = Color.yellow;

        [HideInInspector] public List<Node> OpenSet = new List<Node>(); // Nodes to be evaluated
        [HideInInspector] public HashSet<Node> ClosedSet = new HashSet<Node>(); // Nodes already evaluated

        /// <summary>
        /// Finds the shortest path between the start and target nodes using the A* algorithm.
        /// </summary>
        /// <param name="startNode">The starting node.</param>
        /// <param name="targetNode">The target node.</param>
        /// <returns>A list of nodes representing the path to the target.</returns>
        public List<Node> FindPath(Node startNode, Node targetNode)
        {
            Dictionary<Node, float> gCost = new Dictionary<Node, float>(); // Cost from start to each node
            Dictionary<Node, float> fCost = new Dictionary<Node, float>(); // Total cost from start to target through each node
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>(); // Track the most efficient previous step
            List<Node> currentPath = new List<Node>();

            OpenSet.Clear();
            ClosedSet.Clear();

            OpenSet.Add(startNode);
            gCost[startNode] = 0;
            fCost[startNode] = GetHeuristic(startNode, targetNode);

            while (OpenSet.Count > 0)
            {
                Node currentNode = GetNodeWithLowestFCost(OpenSet, fCost); // Get the node with the lowest F cost

                if (currentNode == targetNode) // If target is reached, reconstruct the path
                {
                    currentPath = ReconstructPath(cameFrom, currentNode);
                    return currentPath;
                }

                OpenSet.Remove(currentNode);
                ClosedSet.Add(currentNode);

                // Evaluate neighbors
                foreach (Node neighbor in NodeGrid.singleton.GetNeighbors(currentNode))
                {
                    if (ClosedSet.Contains(neighbor) || !neighbor.IsWalkable) // Skip if already evaluated or not walkable
                    {
                        continue;
                    }

                    float tentativeGCost = gCost[currentNode] + GetDistance(currentNode, neighbor);

                    if (!OpenSet.Contains(neighbor)) // If neighbor is not in open set, add it
                    {
                        OpenSet.Add(neighbor);
                    }
                    else if (tentativeGCost >= gCost[neighbor]) // Not a better path
                    {
                        continue;
                    }

                    // This path is the best so far
                    cameFrom[neighbor] = currentNode;
                    gCost[neighbor] = tentativeGCost;
                    fCost[neighbor] = gCost[neighbor] + GetHeuristic(neighbor, targetNode);
                }
            }

            Debug.LogWarning("Path cannot be found, but the closest path is being returned!");
            Node bestNode = GetLowestFCostClosestNode(ClosedSet, fCost, targetNode);
            currentPath = FindPath(startNode, bestNode);
            return currentPath; // Find path to the best node
        }

        /// <summary>
        /// Reconstructs the path from the target node back to the start node.
        /// </summary>
        /// <param name="cameFrom">The dictionary of navigated nodes.</param>
        /// <param name="currentNode">The current node being reconstructed.</param>
        /// <returns>The reconstructed path as a list of nodes.</returns>
        private List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node currentNode)
        {
            List<Node> path = new List<Node>();
            while (cameFrom.ContainsKey(currentNode))
            {
                path.Add(currentNode);
                currentNode = cameFrom[currentNode]; // Move to the previous node
            }
            path.Reverse(); // Reverse the path to start from the beginning
            return path;
        }

        /// <summary>
        /// Gets the node in the open set with the lowest F cost.
        /// </summary>
        /// <param name="openSet">The set of open nodes.</param>
        /// <param name="fCost">The dictionary of F costs.</param>
        /// <returns>The node with the lowest F cost.</returns>
        private Node GetNodeWithLowestFCost(List<Node> openSet, Dictionary<Node, float> fCost)
        {
            Node lowestFCostNode = openSet[0];
            foreach (Node node in openSet)
            {
                if (fCost[node] < fCost[lowestFCostNode])
                {
                    lowestFCostNode = node; // Update if this node has a lower F cost
                }
            }
            return lowestFCostNode;
        }

        /// <summary>
        /// Estimates the heuristic cost from the given node to the target node (using Manhattan distance).
        /// </summary>
        /// <param name="nodeA">The start node.</param>
        /// <param name="nodeB">The target node.</param>
        /// <returns>The heuristic cost.</returns>
        private float GetHeuristic(Node nodeA, Node nodeB)
        {
            return Mathf.Abs(nodeA.X - nodeB.X) + Mathf.Abs(nodeA.Y - nodeB.Y); // Manhattan distance
        }

        /// <summary>
        /// Finds the closest 10 nodes to the target node and returns the one with the lowest F cost.
        /// </summary>
        /// <param name="closedSet">The set of evaluated nodes.</param>
        /// <param name="fCost">The dictionary of F costs.</param>
        /// <param name="targetNode">The target node.</param>
        /// <returns>The node with the lowest F cost among the closest 10 nodes.</returns>
        private Node GetLowestFCostClosestNode(HashSet<Node> closedSet, Dictionary<Node, float> fCost, Node targetNode)
        {
            // Get the closest 10 nodes based on distance to the target node
            var closestNodes = closedSet
                .OrderBy(node => Vector3.Distance(node.Position, targetNode.Position))
                .Take(10) // Take the closest 10 nodes
                .ToList();

            // Return the one with the lowest F cost
            return closestNodes
                .OrderBy(node => fCost[node])
                .FirstOrDefault();
        }

        /// <summary>
        /// Calculates the distance between two adjacent nodes.
        /// </summary>
        /// <param name="nodeA">The first node.</param>
        /// <param name="nodeB">The second node.</param>
        /// <returns>The distance between the two nodes.</returns>
        private float GetDistance(Node nodeA, Node nodeB)
        {
            return Vector3.Distance(nodeA.Position, nodeB.Position); // Euclidean distance
        }

        /// <summary>
        /// Draws Gizmos to visualize nodes in the pathfinding grid.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Draw open nodes
            Gizmos.color = openNodeColor; // Set color for the open nodes
            foreach (Node node in OpenSet) // Assuming NodeGrid has an OpenSet property
            {
                Gizmos.DrawWireSphere(node.Position, _openNodeGizmosRadius); // Draw open node with a smaller sphere
            }

            // Draw closed nodes
            Gizmos.color = closedNodeColor; // Set color for the closed nodes
            foreach (Node node in ClosedSet)
            {
                Gizmos.DrawWireSphere(node.Position, _closedNodeGizmosRadius); // Draw closed node with a smaller sphere
            }
        }
    }
}