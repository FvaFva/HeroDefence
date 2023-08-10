using UnityEngine;
using UnityEngine.AI;

namespace MovementSystem
{
    public abstract class BaseReachingChecker
    {
        private float _distance;
        private NavMeshAgent _moveAgent;
        private IFightable _target;
        private Vector3 _currentTargetPoint;

        public BaseReachingChecker(float distance, NavMeshAgent moveAgent)
        {
            _distance = distance;
            _moveAgent = moveAgent;
        }

        public Vector3 CurrentTargetPoint => _currentTargetPoint;

        protected IFightable Target => _target;

        public virtual bool CheckPathEnd(Vector3 currentPosition)
        {
            if (CheckDistance(currentPosition))
                return false;
            else
                SetNewTargetPointToAgent(currentPosition, _target.CurrentPosition);

            return true;
        }

        public bool CheckDistance(Vector3 currentPosition)
        {
            if (_target == null)
                return true;

            return GameSettings.CheckCorrespondencePositions(currentPosition, _target.CurrentPosition, _distance);
        }

        public abstract void SetTarget(Target target, Vector3 currentPosition);

        public void SetMoveSpeed(float moveSpeed)
        {
            _moveAgent.speed = Mathf.Clamp(moveSpeed, GameSettings.Character.MinMoveSpeed, GameSettings.Character.MaxMoveSpeed);
        }

        protected void SetNewTargetPointToAgent(Vector3 currentPosition, Vector3 targetPosition)
        {
            Vector3 newTarget = Vector3.MoveTowards(targetPosition, currentPosition, _distance);
            _currentTargetPoint = newTarget;
            _moveAgent.SetDestination(newTarget);
        }

        protected void ChangeDistance(float distance)
        {
            _distance = Mathf.Max(0, distance);
        }

        protected void TryGetFightable(Target target)
        {
            target.TryGetFightable(out _target);
        }
    }
}
