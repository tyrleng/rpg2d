using System;
using System.Collections.Generic;
using CharacterControllers;
using UnityEngine;

namespace AI.Systems
{
    public enum BattleOutcomePrediction
    {
        Favourable,
        Mixed,
        Unfavourable
    }

    /*
     * The SceneController should be responsible for generating BattleSystem instances.
     */
    public class BattleSystem
    {

        private CharacterBaseController _aggressor;
        private CharacterBaseController _receiver;
    
        public BattleSystem(CharacterBaseController aggressor, CharacterBaseController receiver)
        {
            _aggressor = aggressor;
            _receiver = receiver;
        }
    
        public enum CharacterType
        {
            Aggressor,
            Receiver
        }
        //TODO: Re-Implement this!!!
        public BattleOutcomePrediction PredictBattleOutcome()
        {
            return BattleOutcomePrediction.Favourable;
        }
    
        //----------------------------------------------------------------------------------
        /*
     * Calculations
     */
    
        public int ChanceHit()
        {
            return _aggressor.Accuracy - _receiver.Evasiveness;
        }

        /*
    * Scaled from 0 - 100, each representing percentage points.
    */
        public int ChanceCriticalHit()
        {
            throw new NotImplementedException();
        }

        public int RemainingHealth(CharacterType characterType)
        {
            if (characterType == CharacterType.Aggressor)
                return _aggressor.Health - DamageTaken(CharacterType.Aggressor);
            return _receiver.Health - DamageTaken(CharacterType.Receiver);
        }

        public int DamageTaken(CharacterType characterType)
        {
            if (characterType == CharacterType.Aggressor)
                return _receiver.Attack - _aggressor.Defense;
            return _aggressor.Attack - _receiver.Defense;
        }

        // ---------------------------------------------------------------------------------
        /*
         * Exposing the individual stats of either the aggressor or defender.
         */

        /*
         * Scaled from 0 - 100, each representing percentage points.
         */
        public int AttackStat(CharacterType characterType)
        {
            throw new NotImplementedException();
        }

        public int DefenseStat(CharacterType characterType)
        {
            throw new NotImplementedException();
        }

        public int AccuracyStat(CharacterType characterType)
        {
            throw new NotImplementedException();
        }

        public int EvasiveStat(CharacterType characterType)
        {
            throw new NotImplementedException();
        }
    }
}