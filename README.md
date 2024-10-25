# 🚀 Unity Custom Pathfinding Package

A customizable pathfinding package for Unity, built with C# and designed with an optimized A* algorithm. This package offers nearest-node fallback if a path is unavailable, integrates with Unity’s Gizmos for visual debugging, and leverages the SOLID and OOP principles for high performance and readability.

## 📦 Package Overview

| Feature          | Description                                                                                   |
|------------------|-----------------------------------------------------------------------------------------------|
| **Node**         | Serializable C# class representing a single grid node.                                        |
| **Node Grid**    | Grid structure for pathfinding, supporting customizable walkable layers and Gizmos visualization. |
| **Navigator**    | Navigation class using `NavigateTo` for movement and rotation from the start to target position. |
| **AbstractNavigatorBase** | Base class for all navigators, enabling customization.                           |
| **MonoSingleton**| A singleton pattern implementation from the `ErkanYaprak_UnityHelpers` package.               |
| **Pathfinding**  | Pathfinding calculations via `FindPath(Node startNode, Node targetNode)`, visualizing path and nodes using Gizmos. |
| **NavigationTest** | Test tool for clicking navigation points. (Remove from the package for production.)       |

## 🌌 Package Features

- **Optimized A* Pathfinding** with fallback to the nearest node when paths are blocked
- **Gizmos Visual Debugging** for grid, path, open, and closed nodes
- **Detailed Documentation** using `Header`, `Tooltip`, `Range`, `SerializeField`, and `HideInInspector` attributes for clarity
- **OnDrawGizmos** and **OnDrawGizmosSelected** for flexible debugging
- **Fully Unity and C# Native** without external dependencies

## 🛠️ Setup & Usage

### Clone the Repository

1. Clone the repository:
   ```bash
   git clone https://github.com/nakrekarpay1245/ErkanYaprak_Pathfinding.git
## Add to Your Project
1. Import or drag the package into your Unity project.
2. Attach the **Navigator** component to any object you want to move.
3. Adjust **Node Grid** properties like grid size and walkable layers in the Inspector.

## Testing the Pathfinding
- Use the **NavigationTest** script to click in-game and watch the **Navigator** object find a path.

## Visual Debugging
- Activate **Gizmos** in the Scene View to visualize your grid, open and closed nodes, and paths.

## 📐 Classes Overview
- **Node**: Defines individual grid units with properties like walkability and grid position.
- **Node Grid**: Creates a grid of nodes, visualized with Gizmos. Customizable properties include grid size, walkable layers, and node size.
- **Navigator**: Handles object movement, using `NavigateTo` for pathfinding. Includes rotation towards the target for realism.
- **Pathfinding**: Calculates paths between nodes with `FindPath(Node startNode, Node targetNode)` and falls back to the nearest accessible node if blocked.
- **MonoSingleton**: A singleton helper from the ErkanYaprak_UnityHelpers package.
- **AbstractNavigatorBase**: Base class for different navigator types.

## 🌟 Future Enhancements
- **Path Optimizations**: Adjustments to node connection heuristics.
- **AI Integration**: Designed for flexible integration with custom AI behaviors.
- **Dynamic Tracking Algorithms**: Enable real-time adaptive pathfinding for moving targets.

### Custom Navigator Types:
- **Humanoid**: Movement and rotation combined.
- **UFO**: Movement only, without rotation.
- **Tank**: Continuous forward movement with in-place rotation.
- **Car**: Continuous forward movement, rotating while moving forward.
- **Group**: Group movement with cohesive pathfinding.
- **Robot**: Tile-by-tile movement.

## 📹 YouTube Tutorial
[Watch the Tutorial Here!](<Your_Tutorial_Link>)

## 📬 Contact
Feel free to reach out with questions or feedback!

**Developer**: Erkan Yaprak  
**GitHub**: [nakrekarpay1245](https://github.com/nakrekarpay1245)  
**Website**: [erkanyaprak.w3spaces.com](https://erkanyaprak.w3spaces.com)  
**LinkedIn**: [Erkan Yaprak](https://www.linkedin.com/in/erkan-yaprak)  
**Email**: rknyprk79@gmail.com
