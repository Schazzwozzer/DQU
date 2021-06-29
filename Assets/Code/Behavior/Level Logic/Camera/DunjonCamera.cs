using UnityEngine;
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
                goalPosition = _currentRoom.ClampViewToRoomBounds( goalPosition, _camera );
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