using _Pathfinding._helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Pathfinding.Pathfinding
{
    /// <summary>
    /// Provides A* pathfinding functionality for navigating through nodes.
    /// </summary>
    public class Pathfinding : MonoSingleton<Pathfinding>
    {
        /// <summary>
        /// Finds the shortest path between the start and target nodes using the A* algorithm.
        /// </summary>
        /// <param name="startNode">The starting node.</param>
        /// <param name="targetNode">The target node.</param>
        /// <returns>A list of nodes representing the path to the target.</returns>
        public List<Node> FindPath(Node startNode, Node targetNode)
        {
            // The set of nodes to be evaluated
            List<Node> openSet = new List<Node>();
            // The set of nodes already evaluated
            HashSet<Node> closedSet = new HashSet<Node>();

            // Dictionary to store the cost from start to each node
            Dictionary<Node, float> gCost = new Dictionary<Node, float>();
            // Dictionary to store the cost from start to the target through each node
            Dictionary<Node, float> fCost = new Dictionary<Node, float>();
            // Dictionary to track the most efficient previous step
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

            openSet.Add(startNode);
            gCost[startNode] = 0;
            fCost[startNode] = GetHeuristic(startNode, targetNode);

            while (openSet.Count > 0)
            {
                // Get the node in the open set with the lowest F cost
                Node currentNode = GetNodeWithLowestFCost(openSet, fCost);

                // If we reached the target, reconstruct and return the path
                if (currentNode == targetNode)
                {
                    return ReconstructPath(cameFrom, currentNode);
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // Evaluate neighbors
                foreach (Node neighbor in NodeGrid.singleton.GetNeighbors(currentNode))
                {
                    if (closedSet.Contains(neighbor) || !neighbor.IsWalkable)
                    {
                        continue; // Ignore the neighbor which is already evaluated or not walkable
                    }

                    float tentativeGCost = gCost[currentNode] + GetDistance(currentNode, neighbor);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentativeGCost >= gCost[neighbor])
                    {
                        continue; // This is not a better path
                    }

                    // This path is the best so far
                    cameFrom[neighbor] = currentNode;
                    gCost[neighbor] = tentativeGCost;
                    fCost[neighbor] = gCost[neighbor] + GetHeuristic(neighbor, targetNode);
                }
            }

            Debug.LogWarning("Path can not find but this is the closest path!");
            Node bestNode = GetLowestFCostClosestNode(closedSet, fCost, targetNode);
            return FindPath(startNode, bestNode);
            //// Return an empty path if no path is found
            //return new List<Node>();
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
                currentNode = cameFrom[currentNode];
            }
            path.Reverse(); // Optional: reverse the path to start from the start node
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
                    lowestFCostNode = node;
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
            return Mathf.Abs(nodeA.X - nodeB.X) + Mathf.Abs(nodeA.Y - nodeB.Y);
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
            return Vector3.Distance(nodeA.Position, nodeB.Position);
        }
    }
}