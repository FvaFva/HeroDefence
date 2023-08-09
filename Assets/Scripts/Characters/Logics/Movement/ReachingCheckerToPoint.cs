using UnityEngine;
using UnityEngine.AI;

namespace MovementSystem
{
    public class ReachingCheckerToPoint : BaseReachingChecker
    {
        private float _height;

        public ReachingCheckerToPoint(NavMeshAgent moveAgent, float height)
            : base(0, moveAgent)
        {
            _height = height;
        }

        public override bool CheckPathEnd(Vector3 currentPosition)
        {
            return GameSettings.CheckCorrespondencePositions(currentPosition, CurrentTargetPoint) == false;
        }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            Vector3 targetPoint = target.CurrentPosition();
            targetPoint.y += _height;
            targetPoint = Vector3.MoveTowards(targetPoint, currentPosition, GameSettings.Character.DistanceDeltaToPoint);
            SetNewTargetPointToAgent(currentPosition, targetPoint);
        }
    }
}
