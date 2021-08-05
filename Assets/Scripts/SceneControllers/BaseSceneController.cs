using System.Collections.Generic;
using CharacterControllers;
using UnityEngine;

namespace SceneControllers
{
    

    public abstract class BaseSceneController : MonoBehaviour
    {
        public List<PlayerController> PlayerCharacters;
        public List<AiController> aiCharacters;
        protected int TurnCurrent;
        
        // listens for the event fired by End Turn Button
        public abstract void EndTurnListener();
        
        // End Turn Handler used by whatever characters that want to respond to an end turn.
        public delegate void EndTurnHandler();
        // EndTurnEvent is for the AI controllers to subscribe to, for the AI controllers to take action during an end turn.
        // EndTurn() is a wrapper around EndTurnEvent. The inheriting SceneControllers are meant to call this wrapper. 
        public event EndTurnHandler EndTurnEvent;
        protected void EndTurn()
        {
            if (EndTurnEvent != null)
            {
                Debug.Log("End Turn Event called on turn " + TurnCurrent);
                EndTurnEvent();
            }
        }

        protected abstract void TriggerSceneResponses();

        public List<PlayerController> GetPlayerCharacters()
        {
            return PlayerCharacters;
        }

        public List<AiController> GetEnemyCharacters()
        {
            return aiCharacters;
        }
    }
}