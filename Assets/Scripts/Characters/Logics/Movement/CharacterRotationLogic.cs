using UnityEngine;

namespace MovementSystem
{
    public class CharacterRotationLogic
    {
        private float _rotationSpeed;
        private Transform _body;
        private float _height;

        private Target _target;
        private Quaternion _targetRotation;
        private Vector3 _currentPosition;

        public CharacterRotationLogic(Transform body, float rotationSpeed, float height)
        {
            _body = body;
            _rotationSpeed = rotationSpeed;
            _height = height;
        }

        public void RotateToTarget()
        {
            _currentPosition = _body.position;
            _targetRotation = Quaternion.LookRotation(_target.CurrentPosition() - _currentPosition);
            _body.rotation = Quaternion.Slerp(_body.rotation, _targetRotation, _rotationSpeed * GameSettings.Character.SecondsDelay);
        }

        public void SetTarget(Target target)
        {
            if (target.IsFightable == false)
            {
                Vector3 currentPosition = target.CurrentPosition();
                currentPosition.y += _height;
                _target = new Target(currentPosition);
            }    
            else
            {
                _target = target;
            }
        }

        public bool CheckRotateToTarget(float checkAngle)
        {
            Vector3 toTargetNormalize = _target.CurrentPosition() - _currentPosition;
            toTargetNormalize.Normalize();
            bool isInAngle = Vector3.Dot(_body.forward, toTargetNormalize) > Mathf.Clamp(checkAngle, -1.0f, 1.0f);
            return isInAngle;
        }
    }
}