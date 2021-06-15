using UnityEngine;
using System.Collections;

namespace DQU
{
    /// <summary>
    /// Responsible for moving the Player. Also provides some stats about, 
    /// for instance, how much the Player moved during the last update.
    /// </summary>
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

        private Vector2 _prevPosition;
        private Vector2 _prevTranslation;
        public Vector2 PreviousTranslation { get { return _prevTranslation; } }


        private void FixedUpdate()
        {
            _prevTranslation = _rigidbody.position - _prevPosition;
            _prevPosition = _rigidbody.position;

            _rigidbody.AddForce(
                CurrentMovement * _movementSpeed, 
                ForceMode2D.Force );
        }

    }
}