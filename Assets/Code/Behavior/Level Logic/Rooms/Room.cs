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


        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();

            // Acquire the Room's camera track and whine if it's not found.
            _cameraTrack = GetComponentInChildren<CameraTrack>();
            if( _cameraTrack == null )
                Debug.LogError( string.Format( 
                    "Room \"{0}\" requires a Camera Track in its children, but does not have one.", 
                    gameObject.name ) ); 
        }


        private void OnTriggerEnter2D( Collider2D collision )
        {
            EnterRoom();
        }


        private void EnterRoom()
        {
            EventManager.Instance.Raise( enterEvent.Configure( this ) );

            // Activate all required room art.
            
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