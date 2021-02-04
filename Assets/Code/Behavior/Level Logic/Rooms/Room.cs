using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using DQU.Events;

namespace DQU
{
    public class Room : MonoBehaviour
    {
        private CameraTrack _cameraTrack;
        public CameraTrack CameraTrack { get { return _cameraTrack; } }

        /// <summary>The world space bounding box for this Room.</summary>
        public Bounds Bounds { get { return _collider.bounds; } }

        private static RoomEnteredEvent enterEvent = new RoomEnteredEvent();

        private BoxCollider2D _collider;
        private RoomTransition[] _transitions;


        private void Awake()
        {
            // Acquire references
            _collider = GetComponent<BoxCollider2D>();

            // Camera track and whine if it's not found.
            _cameraTrack = GetComponentInChildren<CameraTrack>();
            if( _cameraTrack == null )
                Debug.LogError( "Room is missing a Camera Track.", gameObject );

            _transitions = GetComponentsInChildren<RoomTransition>();

            Activate( false );
        }

        /// <summary>
        /// Toggle all of this room's Transitions.
        /// </summary>
        private void Activate( bool active )
        {
            for( int i = 0; i < _transitions.Length; ++i )
                if( _transitions[i] != null )
                    _transitions[i].gameObject.SetActive( active );

            if( active )
                EventManager.Instance.AddListener<RoomEnteredEvent>( OnRoomEntered );
            else
                EventManager.Instance.RemoveListener<RoomEnteredEvent>( OnRoomEntered );

            // TO DO: Activate all room-specific art.
        }

        private void OnRoomEntered( RoomEnteredEvent e )
        {
            Activate( e.Room == this );
        }

        /// <summary>
        /// Call when the Player enters this Room.
        /// </summary>
        public void EnterRoom()
        {
            // Raise this before Activate(), so that it's not called redundantly.
            EventManager.Instance.Raise( enterEvent.Configure( this ) );

            Activate( true );
        }


        private void OnDrawGizmosSelected()
        {
            if( CameraTrack != null )
            {
                CameraTrack.DrawGizmo();
            }
        }

    }
}