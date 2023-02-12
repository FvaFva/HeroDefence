using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSettings
{
    public const float Hundred = 100;
    public const float Zero = 0;
    public const float HitingRange = 300;

    public static class Character
    {
        private static WaitForSeconds _optimizationDelay = new WaitForSeconds(0.1f);
        private static List<string> _namesPool = new List<string>();

        public const float RangeDelta = 0.1f;
        public const float SocialDistance = 2f;
        public const float ArmorUnitImpact = 0.97f;

        public const int StaminaPointsToAtack = 1000;        

        static Character()
        {
            FillNamePool();
        }

        public static WaitForSeconds OptimizationDelay()
        {
            return _optimizationDelay;
        }

        public static string GetRandomName()
        {
            return _namesPool[Random.Range(0, _namesPool.Count)];
        }

        private static void FillNamePool()
        {
            _namesPool.Add("Josh");
            _namesPool.Add("Ben");
            _namesPool.Add("Alex");
            _namesPool.Add("Nik");
            _namesPool.Add("Adam");
            _namesPool.Add("Mark");
            _namesPool.Add("Rolan");
            _namesPool.Add("Serg");
            _namesPool.Add("Andy");
            _namesPool.Add("Filipp");
            _namesPool.Add("Boris");
            _namesPool.Add("Oleg");
            _namesPool.Add("Pilat");
            _namesPool.Add("Den");
            _namesPool.Add("Zoza");
            _namesPool.Add("Naham");
            _namesPool.Add("Greek");
            _namesPool.Add("Chak");
            _namesPool.Add("Baron");
            _namesPool.Add("Graf");
            _namesPool.Add("Zak");
            _namesPool.Add("Pupa");
            _namesPool.Add("Lupa");
            _namesPool.Add("Biba");
            _namesPool.Add("Boba");
            _namesPool.Add("Aram");
            _namesPool.Add("Adam");
            _namesPool.Add("Daniel");
        }

        public enum SexType
        {
            Female = 0,
            Male,
            Being
        }
    }    

    public static class UI
    {
        public const float CameraMoveSpeed = 10;
    }

    public static class Animator
    {
        public static class States
        {
            public const string Idle = nameof(Idle); 
            public const string Run = "Run";
        }
    }
}

public enum PercActionType
{
    OnAttack = 0,
    OnDamageDelay,
    OnDefence
}
