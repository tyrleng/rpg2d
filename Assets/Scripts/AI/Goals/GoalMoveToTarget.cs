using System;
using System.Collections.Generic;
using AI.Systems;
using CharacterControllers;
using UnityEngine;

namespace AI.Goals
{
    public class GoalMoveToTarget : GoalBase
    {
        public override void Process()
        {
            //TODO: shorten this code length to obtain the target.
            AiController aiController = GetComponent<AiController>();
            TargetingSystem targetingSystem = aiController.TargetingSystem;
            TargetAndDesirability targetAndDesirability = targetingSystem.BestTarget;
            CharacterBaseController target = targetAndDesirability.Target;
            
            /*
             * Purpose: Find the best path to the target.
             * First find the actual optimal path. Use A Star.
             * Then, use a coroutine to continuously click the square along the optimal path within the flood fill area
             */

            /*
             * Calculate the G, H and F cost.
             * H Cost is calculated by euclidean distance.
             * Should take terrain penalties into account.
             */
            
            /*
             * Supporting Infrastructure:
             * Needs access to location of target.
             * Needs access to current location.
             */
            
            /*
             * On Each Run,
             * Must explore 1 neighbour. Choose the neighbour with the lowest f_cost. But if the neighbour is not traversable or neighbour is in explored queue, skip the neighbour.
             * If the explored neighbour is the destination, end the algorithm.
             * Add the unexplored squares, calculating their f_cost along the way.
             * Also, if the cost to reach neighbour squares can be lesser, update the f-Cost of the neighbour squares.
             * Set the parent of neighbour to the current node.
             * Maintain two queues: 1 for explored nodes, 1 for unexplored nodes.
             */
            
            Vector3Int destinationLocation = Vector3Int.FloorToInt(target.transform.position);
            Vector3Int currentLocation = Vector3Int.FloorToInt(transform.position);

            Node destinationNode = new Node(TilemapController.Tiles[destinationLocation]);
            Node currentNode = new Node(TilemapController.Tiles[currentLocation]);

            List<Node> exploredTilesList = new List<Node>();
            List<Node> toBeExploredTilesList = new List<Node>();

            bool pathFound = false;
            while (pathFound == false)
            {
                findAndCalcAdjTiles(currentNode, toBeExploredTilesList);
                //sort the toBeExploredTilesList 
            }
            throw new NotImplementedException();
        }

        private void findAndCalcAdjTiles(Node currentNode, List<Node> toBeExploredTilesList)
        {
            Vector3Int currentLocation = currentNode.tile.WorldLocation;
            Node downNode = new Node(TilemapController.Tiles[currentLocation + Vector3Int.down]);
            Node upNode = new Node(TilemapController.Tiles[currentLocation + Vector3Int.up]); 
            Node leftNode = new Node(TilemapController.Tiles[currentLocation + Vector3Int.left]);
            Node rightNode = new Node(TilemapController.Tiles[currentLocation + Vector3Int.right]);

            downNode.g_cost = currentNode.g_cost + downNode.tile.Penalty;
            downNode.h_cost = currentNode.h_cost + downNode.tile.Penalty;
            
            toBeExploredTilesList.Add(downNode);
            toBeExploredTilesList.Add(upNode);
            toBeExploredTilesList.Add(leftNode);
            toBeExploredTilesList.Add(rightNode);
        }

        private float EuclideanDistance(Node node, Node destination)
        {
            throw new NotImplementedException();
        }
    }
    
    class Node
    {
        public Node(WorldTile tile)
        {
            this.tile = tile;
        }
            
        public WorldTile tile; 
        public float g_cost; 
        public float h_cost;

        public float f_cost()
        {
            return g_cost + h_cost;
        }
    }
}