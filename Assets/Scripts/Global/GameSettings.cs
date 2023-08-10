using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public const float Hundred = 100;
    public const float Zero = 0;
    public const float HittingRange = 300;
    public const int PlayerBagSize = 100;

    public static bool CheckCorrespondencePositions(Vector3 position1, Vector3 position2, float range = 0)
    {
        float distance = Vector3.Distance(position1, position2);
        return distance - range <= Character.RangeDelta;
    }

    public static class Character
    {
        public const float RangeDelta = 0.1f;
        public const float AngleDelta = 0.97f;
        public const float SocialDistance = 1.2f;
        public const float ArmorUnitImpact = 0.97f;
        public const float MinMoveSpeed = 2;
        public const float MaxMoveSpeed = 15;
        public const float AngleAttack = 0.40f;
        public const float AngularSpeed = 10;
        public const float DistanceDeltaToPoint = 0.4f;
        public const float SecondsDelay = 0.1f;
        public const float FlightDeathHight = 30;
        public const int CountOfCharacterSpells = 2;
        public const int StaminaPointsToAttack = 1000;

        public static readonly WaitForSeconds OptimizationDelay = new WaitForSeconds(SecondsDelay);

        private static List<string> _namesPool = new List<string>();

        static Character()
        {
            FillNamePool();
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
            _namesPool.Add("Rowan");
            _namesPool.Add("Sergey");
            _namesPool.Add("Andy");
            _namesPool.Add("Filip");
            _namesPool.Add("Boris");
            _namesPool.Add("Oleg");
            _namesPool.Add("Den");
            _namesPool.Add("Greek");
            _namesPool.Add("Chuck");
            _namesPool.Add("Baron");
            _namesPool.Add("Graf");
            _namesPool.Add("Zak");
            _namesPool.Add("Pupa");
            _namesPool.Add("Biba");
            _namesPool.Add("Boba");
            _namesPool.Add("Aram");
            _namesPool.Add("Adam");
            _namesPool.Add("Daniel");
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
