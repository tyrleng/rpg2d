using AI.Evaluators;
using AI.Goals;
using CharacterControllers;
using UnityEngine;


/*
 * GoalPlanner should run each time its the AI's turn.
 */
namespace AI
{
    public class GoalPlanner : MonoBehaviour
    {
        private GoalBase _selectedGoal;

        /*
     * When called, should pick the best goal of all the goals available.
     * Therefore uses the Goal Evaluators.
     */
        public bool SetGoal()
        {
            int maxDesirability = 0;
            EvaluatorAttackTarget evaluatorAttackTarget = new EvaluatorAttackTarget();
            int attackDesirability = evaluatorAttackTarget.GetDesirability(GetComponent<AiController>());
        
            // go through all the desirability choices, then pick the max desirability.
            if (attackDesirability > maxDesirability)
                _selectedGoal = gameObject.AddComponent<GoalAttackTarget>();
            return true;
        }
        /*
     * Activates the goal that was chosen for the AI character that turn.
     * Separated from SetGoal() because the SceneController might wish for the AI characters to act in a specified order.
     */
        public void EnactGoalForTurn()
        {
            _selectedGoal.Process();
        }
    }
}
