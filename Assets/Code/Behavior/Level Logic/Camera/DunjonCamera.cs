using UnityEngine;
using System.Collections;
using SDD.Events;
using DQU.Events;

namespace DQU
{
    /// <summary>
    /// Specialized behavior for the game Camera used while in Dunjons.
    /// </summary>
    public class DunjonCamera : MonoBehaviour
    {
        /// <summary>Reference to the current Player.</summary>
        private Transform _player;

        private Room _currentRoom;

        private Camera _camera;


        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            EventManager.Instance.AddListener<PlayerSpawnedEvent>( OnPlayerSpawn );
            EventManager.Instance.AddListener<RoomEnteredEvent>( OnRoomEntered );
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<PlayerSpawnedEvent>( OnPlayerSpawn );
            EventManager.Instance.RemoveListener<RoomEnteredEvent>( OnRoomEntered );
        }

        private void LateUpdate()
        {
            if( _player != null && _currentRoom != null )
            {
                // Try to follow the player, as allowed by the current Room's Camera Track.
                Vector2 goalPosition = _currentRoom.CameraTrack.GetNearestPosition( _player.transform.position );
                goalPosition = ClampViewToRoomBounds( goalPosition, _currentRoom.Bounds );
#if TEST
                goalPositionDebug = goalPosition;
#endif
                transform.position = new Vector3( goalPosition.x, goalPosition.y, transform.position.z );
            }
        }


        private void OnPlayerSpawn( PlayerSpawnedEvent e )
        {
            this._player = e.Player;
        }

        private void OnRoomEntered( RoomEnteredEvent e )
        {
            _currentRoom = e.Room;
        }


        private void ChangeBackgroundColor( Color color )
        {
            _camera.backgroundColor = color;
        }

        
        /// <summary>
        /// Constrain goalPosition so that the camera's 
        /// view remains wholly within the Room's bounds.
        /// </summary>
        private Vector2 ClampViewToRoomBounds( Vector2 goalPosition, Bounds bounds )
        {
            float halfCameraWidth = _camera.orthographicSize * _camera.aspect;

            Vector2 positionalMin = new Vector2( 
                bounds.min.x + halfCameraWidth,
                bounds.min.y + _camera.orthographicSize );
            Vector2 positionalMax = new Vector2(
                bounds.max.x - halfCameraWidth,
                bounds.max.y - _camera.orthographicSize );

            // If the camera's view exceeds the Room's bounds, just use the bounds' center position.
            if( positionalMin.x > positionalMax.x )
                positionalMin.x = positionalMax.x = bounds.center.x;
            if( positionalMin.y > positionalMax.y )
                positionalMin.y = positionalMax.y = bounds.center.y;

            return new Vector2(
                Mathf.Clamp( goalPosition.x, positionalMin.x, positionalMax.x ),
                Mathf.Clamp( goalPosition.y, positionalMin.y, positionalMax.y ) );
        }

#if TEST
        private Vector2 goalPositionDebug;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GizmosHelper.DrawCrosshairs2D( goalPositionDebug, 1f );
            Gizmos.DrawWireSphere( (Vector3)goalPositionDebug, 0.5f );
        }
#endif

    }
}