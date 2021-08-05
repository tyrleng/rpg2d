using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControllers
{
    public class CharacterBaseController : MonoBehaviour
    {
        public int Health
        {
            get => _health;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be between 0 and 100.");
                _health = value;
            }
        }
        private int _health;
    
        public int Attack
        {
            get => _attack;
            set
            { 
                if (value < 0 || value > 50)
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be between 0 and 50");
                _attack = value;
            }
        }
        private int _attack;
    
        public int Defense
        {
            get => _defense;
            set
            {
                if (value < 0 || value > 40)
                    throw new ArgumentOutOfRangeException($"{nameof(value)} must be between 5 and 40");
                _defense = value;
            }
        }
        private int _defense;
        
        public int Accuracy
        {
            get => _accuracy;
            set => _accuracy = value;
        }
        private int _accuracy;
    
        public int Evasiveness
        {
            get => _evasiveness;
            set => _evasiveness = value;
        }
        private int _evasiveness;
    
        public int Speed
        {
            get => _speed;
            set => _speed = value;
        }
        private int _speed;
    
        public int Morale
        {
            get => _morale;
            set => _morale = value;
        }
        private int _morale;
    }
}

