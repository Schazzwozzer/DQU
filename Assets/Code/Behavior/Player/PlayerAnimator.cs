using System.Collections;
using UnityEngine;

namespace DQU
{
    /// <summary>
    /// Acts as a go-between for an Actor's behavior code and the Unity 
    /// <see cref="Animator"/> component, to help separate game logic from art logic.
    /// </summary>
    [AddComponentMenu( "Scripts/Components/Animators/Player Animator" )]
    public class PlayerAnimator : MonoBehaviour
    {
        // There's potential for a lot of spaghetti code in a class like this.
        // For now, I'm going to just have this class poll other components for state,
        // though maybe I'll eventually want to use events or something, if it gets too messy.

        [SerializeField]
        private Animator _animatorComponent;

        [SerializeField]
        private PlayerLocomotion _locomotionComponent;

        [SerializeField]
        private Transform _playerFacingTransform;

        // Animation IDs
        /// <summary>The hash ID for the "movementSpeed" parameter.</summary>
        int movementSpeedHash;

        private void Awake()
        {
            if( _animatorComponent == null )
                _animatorComponent = GetComponent<Animator>();
        }


        private void OnEnable()
        {
            CacheParameterHashes();
        }


        private void LateUpdate()
        {
            float speed = _locomotionComponent.PreviousTranslation.magnitude * (1 / Time.fixedDeltaTime);
            _animatorComponent.SetFloat( movementSpeedHash, speed );

            UpdateModelFacing();
        }


        /// <summary>
        /// 
        /// </summary>
        private void UpdateModelFacing()
        {
            Vector2 direction = _locomotionComponent.CurrentMovement.normalized;
            if( direction != Vector2.zero )
            {
                float rotationInDegrees = Mathf.Atan2( direction.x, direction.y ) * Mathf.Rad2Deg;
                rotationInDegrees = MathHelper.CycleThroughRange( rotationInDegrees - 0f, 0, 360f );
                float eulerX = _playerFacingTransform.eulerAngles.x;
                _playerFacingTransform.localRotation = Quaternion.Euler( 0f, rotationInDegrees, 0f );
            }
        }


        private void CacheParameterHashes()
        {
            movementSpeedHash = Animator.StringToHash( "Movement Speed" );
        }

    }
}