using UnityEngine;
using System.Collections;

namespace DQU
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField]
        private float _movementSpeed = 10f;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        private Vector2 _currentMovement;
        public Vector2 CurrentMovement
        {
            get { return _currentMovement; }
            set { _currentMovement = value; }
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(
                CurrentMovement * _movementSpeed, 
                ForceMode2D.Force );
        }

    }
}