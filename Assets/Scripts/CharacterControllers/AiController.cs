using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using AI.Systems;
using SceneControllers;
using UnityEngine;

namespace CharacterControllers
{
    public class AiController : CharacterBaseController
    {
        public BaseSceneController SceneController { get; set; }
        public TargetingSystem TargetingSystem;
        private GoalPlanner _goalPlanner;
        public bool GoalHasBeenChosen;
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    
        // Start is called before the first frame update
        void Start()
        {
            SceneController.EndTurnEvent += EndTurnEventActions;
            //Every AI character should have its own targeting system.
            TargetingSystem = new TargetingSystem(this);
            // Every AI character should have its own goal planner, the brains class.
            _goalPlanner = gameObject.AddComponent<GoalPlanner>();
        }
        
        private void EndTurnEventActions()
        {
            Debug.Log("I am " + name + "enacting my end turn actions");
            TargetingSystem.GenerateDesirabilityAndTargets();
            // choose the goal for the AI to enact.
            GoalHasBeenChosen = _goalPlanner.SetGoal();
            _goalPlanner.EnactGoalForTurn();
        }
    }
}

