using UnityEngine;
using UnityEngine.AI;

namespace MovementSystem
{
    public class ReachingCheckerToAlly : BaseReachingChecker
    {
        public ReachingCheckerToAlly(NavMeshAgent moveAgent)
            : base(GameSettings.Character.SocialDistance, moveAgent) { }

        public override void SetTarget(Target target, Vector3 currentPosition)
        {
            TryGetFightable(target);
            SetNewTargetPointToAgent(currentPosition, target.CurrentPosition());
        }
    }
}
