using System.Collections;
using System.Collections.Generic;
using CharacterControllers;
using UnityEngine;

namespace SceneControllers
{
    public class SampleSceneController : BaseSceneController
    {
        public void Start()
        {
            foreach (AiController aiCharacter in aiCharacters)
            {
                aiCharacter.SceneController = this;
            }
        }
        
        // Subscribes to the end turn button event.
        // In turn, will raise the EndTurn event, allowing AiControllers to react and plan. 
        public override void EndTurnListener()
        {
            TurnCurrent += 1;
            Debug.Log("Turn now is " + TurnCurrent);
            TriggerSceneResponses();
            EndTurn();
        }
        protected override void TriggerSceneResponses()
        {
            switch (TurnCurrent)
            {
                case 3:
                    aiCharacters[0].gameObject.SetActive(true);
                    break;
                case 5:
                    aiCharacters[1].gameObject.SetActive(true);
                    break;
            }
        }
    }
}