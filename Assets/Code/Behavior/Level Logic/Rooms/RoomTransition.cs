using System.Collections;
using UnityEngine;

namespace DQU
{
    /// <summary>
    /// A trigger area that determines when the Player moves from one Room to another.
    /// </summary>
    [RequireComponent( typeof( Collider2D ))]
    public class RoomTransition : MonoBehaviour
    {
        [SerializeField]
        private Room _targetRoom;

        private void Awake()
        {
            if( _targetRoom == null )
                Debug.LogError( "No target room defined!", gameObject );
        }

        private void OnTriggerEnter2D( Collider2D collision )
        {
            if( collision.gameObject.layer == LayerHelper.Player )
                _targetRoom.EnterRoom();
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            if( _targetRoom != null )
                Gizmos.DrawLine(
                    transform.position,
                    _targetRoom.transform.position );

        }

    }
}