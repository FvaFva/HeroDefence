using UnityEngine.AI;
using UnityEngine;

namespace MovementSystem
{
    public class ReachingCheckerToPoint : BaseReachingChecker
    {
        float _height;

        public ReachingCheckerToPoint(NavMeshAgent moveAgent, float height)
        {
            _moveAgent = moveAgent;
            _height = height;
        }

        public override bool CheckPathEnd(Vector3 currentPosition)
        {
            return GameSettings.CheckCorrespondencePositions(currentPosition, _currentTargetPoint) == false;
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
