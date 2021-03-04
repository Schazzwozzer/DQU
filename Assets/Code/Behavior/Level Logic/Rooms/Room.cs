using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using DQU.Events;

namespace DQU
{
    public class Room : MonoBehaviour
    {
        // Used to look up the room's text description.
        [SerializeField, HideInInspector, Range(0,100)]
        private int _roomNumber;
        public int RoomNumber { get { return _roomNumber; } }

        [SerializeField, HideInInspector] private bool _clampToBoundsX = true;
        [SerializeField, HideInInspector] private bool _clampToBoundsY = true;

        [SerializeField]
        private RoomDescriptionLayout _descriptionTextLayout;

        private CameraTrack _cameraTrack;
        public CameraTrack CameraTrack { get { return _cameraTrack; } }

        /// <summary>The world space bounding box for this Room.</summary>
        public Bounds Bounds { get { return _collider.bounds; } }

        private static RoomEnteredEvent enterEvent = new RoomEnteredEvent();

        private BoxCollider2D _collider;
        private RoomTransition[] _transitions;
        [SerializeField]
        private Renderer[] _roomExclusiveArt;

        [SerializeField]
        public UnityEngine.Events.UnityEvent OnRoomEnter;


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

            for( int i = 0; i < _roomExclusiveArt.Length; ++i )
                if( _roomExclusiveArt[i] != null )
                    _roomExclusiveArt[i].gameObject.SetActive( active );

            OnRoomEnter.Invoke();
        }


        /// <summary>
        /// Constrain the camera's position so that the camera's
        /// view does not extend outside of the Room's bounds.
        /// </summary>
        public Vector2 ClampViewToRoomBounds( Vector2 goalPosition, Camera camera )
        {
            float halfCameraWidth = camera.orthographicSize * camera.aspect;

            float xPos, yPos;
            Vector2 positionalMin = new Vector2(
                _collider.bounds.min.x + halfCameraWidth,
                _collider.bounds.min.y + camera.orthographicSize );
            Vector2 positionalMax = new Vector2(
                _collider.bounds.max.x - halfCameraWidth,
                _collider.bounds.max.y - camera.orthographicSize );

            if( _clampToBoundsX )
            {
                // If the camera's view exceeds the Room's bounds, just use the bounds' center position.
                if( positionalMin.x > positionalMax.x )
                    positionalMin.x = positionalMax.x = _collider.bounds.center.x;
                xPos = Mathf.Clamp( goalPosition.x, positionalMin.x, positionalMax.x );
            }
            else
                xPos = goalPosition.x;

            if( _clampToBoundsY )
            {
                // As above, check if camera view exceeds Room bounds.
                if( positionalMin.y > positionalMax.y )
                    positionalMin.y = positionalMax.y = _collider.bounds.center.y;
                yPos = Mathf.Clamp( goalPosition.y, positionalMin.y, positionalMax.y );
            }
            else
                yPos = goalPosition.y;

            return new Vector2( xPos, yPos );
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