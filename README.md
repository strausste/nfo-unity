# Network Flows Optimization
**Pathfinding in Unity graphics engine and a case study with Age of Empires II (Unity project)**

🕹️ Unity Editor's version: 2021.3.26f1

### Before launching the game

Ensure the GridManager game object is active (default is active); \
Ensure the DijkstraManager game object is not set to active (default is not active); \
Ensure the AStarManager game object is not set to active (default is not active); \

## Dijkstra's algorithm

Select the DijkstraManager game object and set costs from the Inspector tab: 
 > Orthogonal Cost \
 > Diagonal Cost 

Play the game and, in the Editor, manually set the DijkstraManager game object to active

## A* algorithm

Select the AStarManager game object and choose the heurist function from the Inspector tab:

 > Euclidean Distance \
 > Manhattan Distance \
 > Chebyshev Distance \
 > Octile Diagonal Distance

Set costs from the Inspector tab: 
 > Orthogonal Cost \
 > Diagonal Cost 

Play the game and, in the Editor, manually set the DijkstraManager game object to active

