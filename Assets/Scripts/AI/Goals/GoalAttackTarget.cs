using AI.Systems;
using CharacterControllers;
using UnityEngine;

namespace AI.Goals
{
    public class GoalAttackTarget : GoalBase
    {
        public override void Process()
        {
            // obtain the best target from the targeting system.
            // add SubGoal: move towards the player character that you want to attack.
            // then actually attack.
            // call the animator on the AiController
            Debug.Log(gameObject.name + "From GoalAttackTarget!");
            AiController aiCharacter = GetComponent<AiController>();
            TargetingSystem targetingSystem = aiCharacter.TargetingSystem;
            TargetAndDesirability bestTarget = targetingSystem.BestTarget;
        }
    }
}
