using UnityEngine;
using UnityEngine.InputSystem;

namespace DQU
{
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField]
        private PlayerLocomotion locomotionComponent;

        private void OnInteract()
        {
            Debug.Log( "Interact!" );
        }

        // Input is received normalized.
        private void OnMove( InputValue input )
        {
            locomotionComponent.CurrentMovement = input.Get<Vector2>();
        }

    }
}