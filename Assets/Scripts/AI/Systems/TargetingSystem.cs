using System;
using System.Collections;
using System.Collections.Generic;
using CharacterControllers;
using SceneControllers;
using UnityEngine;

namespace AI.Systems
{
    public enum DesirabilityForTarget
    {
        Desirable,
        Neutral,
        Undesirable
    }

/*
 * Data class meant to be used by the TargetingSystem.
 */
    public class TargetAndDesirability 
    {
        public readonly PlayerController Target;
        public DesirabilityForTarget Desirability;
    
        public TargetAndDesirability(PlayerController target, DesirabilityForTarget desirability)
        {
            Target = target;
            Desirability = desirability;
        }
    }

/*
 * Each AI character has 1 targeting system attached to the character.
 * The evaluators will find their appropriate data structure.
 * This class isn't meant to be added through the editor; The AiController should 
 */
    public class TargetingSystem
    {
        public TargetingSystem(AiController aiCharacter)
        {
            _aiCharacter = aiCharacter;
        }
        private readonly AiController _aiCharacter;
    
        public List<TargetAndDesirability> AttackTargetAndDesirabilityList  = new List<TargetAndDesirability>();
        public TargetAndDesirability BestTarget;

        /*
         * Note: This is from perspective of AI. Hence, the player characters are the enemies.
         * Purpose: calculate the desirability of attacking each enemy, then choosing the highest desirability as the target.
         * For each enemy within realistic range, have to obtain the battle outcome prediction. The prediction will be a major part of the desirability.
         * Are you able to immediately kill the enemy? Will you take any damage whilst doing so?
         * How many of your pals around are also capable of attacking the target you're considering? How can you tell them to attack that target you're considering?
         * The range to each potential enemy matters too - can the enemy be reached in 1, 2 or 3 turns?
         * Furthermore, for that 1 enemy, how many of his/her pals are around to potentially gang up on you?
         * Were you already tracking a target? Even if that target disappeared into the fog of war, should you continue tracking the target?
         */
        public void GenerateDesirabilityAndTargets()
        {
            BaseSceneController sceneController = _aiCharacter.SceneController;
            List<PlayerController> playerCharacters = sceneController.GetPlayerCharacters();
            List<PlayerController> closestPlayerCharacters = GetClosestEnemies(playerCharacters);
        
            // calculate the desirability for the closest targets
            foreach (var playerCharacter in closestPlayerCharacters)
            {
                DesirabilityForTarget desirability = DesirabilityForTarget.Undesirable;
                BattleSystem battleSystem = new BattleSystem(_aiCharacter, playerCharacter);
                BattleOutcomePrediction battlePrediction = battleSystem.PredictBattleOutcome();
                
                //todo: generate TargetingSystem's predictions and desirability for targets
                // todo: Have a better solution that this simplistic target desirability
                if (battlePrediction == BattleOutcomePrediction.Unfavourable)
                    desirability = DesirabilityForTarget.Undesirable;
                if (battlePrediction == BattleOutcomePrediction.Mixed)
                    desirability = DesirabilityForTarget.Neutral;
                if (battlePrediction == BattleOutcomePrediction.Favourable)
                    desirability = DesirabilityForTarget.Desirable;
                TargetAndDesirability targetAndDesirability = new TargetAndDesirability(playerCharacter, desirability);
                
                // select the first desirable target 
                // TODO: have a better solution than this
                if (BestTarget == null && desirability == DesirabilityForTarget.Desirable)
                {
                    BestTarget = targetAndDesirability;
                }
                AttackTargetAndDesirabilityList.Add(targetAndDesirability);
            }
        }

        private List<PlayerController> GetClosestEnemies(List<PlayerController> playerCharactersList)
        {
            int considerationSize = 10;
            KeyValuePair<PlayerController, float>[] closestEnemiesList = new KeyValuePair<PlayerController, float>[considerationSize];

            foreach (var playerCharacter in playerCharactersList)
            {
                float playerCharacterDistance = CalcEnemyDistance(playerCharacter);
                KeyValuePair<PlayerController, float> keyValuePair = new KeyValuePair<PlayerController, float>(playerCharacter, playerCharacterDistance);
                // if the biggest element in the array is bigger than the newbie, it will be kicked out.
                if (CompareEnemiesDistance(keyValuePair, closestEnemiesList[considerationSize - 1]) < 0)
                {
                    closestEnemiesList[0] = keyValuePair;
                    // it's not guaranteed that element 0 has the smallest distance, hence need to Sort.
                    Array.Sort(closestEnemiesList, CompareEnemiesDistance);
                }
            }
            // tidying up to return the values in proper form.
            List<PlayerController> closestPlayerCharacters = new List<PlayerController>();
            for (int i = 0; i < considerationSize; i++)
            {
                closestPlayerCharacters.Add(closestEnemiesList[i].Key);
            }
            return closestPlayerCharacters;
        }

        private float CalcEnemyDistance(PlayerController enemy)
        {
            Vector3 enemyPos = enemy.transform.position;
            return Vector3.Distance(_aiCharacter.transform.position, enemyPos);
        }

        private int CompareEnemiesDistance(KeyValuePair<PlayerController, float> first, KeyValuePair<PlayerController, float> second)
        {
            float firstDistance = first.Value;
            float secondDistance = second.Value;
            if (firstDistance > secondDistance)
                return 1;
            if (secondDistance > firstDistance)
                return -1;
            return 0;
        }
    }
}