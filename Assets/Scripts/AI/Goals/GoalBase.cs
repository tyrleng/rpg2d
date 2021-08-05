using UnityEngine;

namespace AI.Goals
{

    public enum GoalStatus
    {
        Inactive,
        Failed,
        Completed
    }
    public abstract class GoalBase : MonoBehaviour
    {
        public GoalStatus GoalStatus;

        public abstract void Process();
    }
}