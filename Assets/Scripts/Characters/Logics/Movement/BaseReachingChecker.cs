using UnityEngine.AI;
using UnityEngine;

namespace MovementSystem
{
    public abstract class BaseReachingChecker
    {
        protected float _distance;
        protected NavMeshAgent _moveAgent;
        protected Vector3 _currentTargetPoint;
        protected IFightable _target;

        public virtual bool CheckPathEnd(Vector3 currentPosition)
        {
            if (CheckDistance(currentPosition))
            {
                return false;
            }
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
    }

}
