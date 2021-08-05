using CharacterControllers;

namespace AI.Evaluators
{
    
    /* To pull out the individual feature desirability ratings and combine them into a goal desirability rating .
     * Meant to be subclassed for each goal that exists.
     */
    public abstract class EvaluatorBase 
    {
        // normalised score from 1 to 100.
        public abstract int GetDesirability(AiController aiController);
    }
}