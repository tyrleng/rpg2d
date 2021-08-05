using AI.Systems;
using CharacterControllers;

namespace AI.Evaluators
{
    public class EvaluatorAttackTarget : EvaluatorBase
    {
        /*
     * Internally will use the targeting system's suggestions.
     */


        public override int GetDesirability(AiController aiController)
        {
            TargetingSystem targetingSystem = aiController.TargetingSystem;
            TargetAndDesirability bestTarget = targetingSystem.BestTarget;
            if (bestTarget != null)
                return 80;
            // find target with the most desirability. Then convert that desirability into a numerical scaled value between 1 and 100.
            // throw new System.NotImplementedException();
            return 0;
        }
    }
}
