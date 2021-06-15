using UnityEngine;
using System.Linq;

namespace DQU
{
    /// <summary>
    /// Constrains the camera position according to a 2D 
    /// shape (as defined by Collider2Ds) or a single point.
    /// </summary>
    public class CameraTrack : MonoBehaviour
    {
        [SerializeField] private Collider2D[] _colliders;

        private void Awake()
        {
            // Camera Track only uses colliders as 2D shapes, for geometry 
            // calculations. Make sure they're not used for actual collision.
            gameObject.layer = LayerHelper.NoCollision;

            // Trim any empty elements from the Colliders array.
            _colliders = _colliders.Where( x => x != null ).ToArray();
        }

        public Vector2 GetNearestPosition( Vector2 position )
        {
            if( _colliders.Length == 0 )
                return transform.position;
            else
                return GetNearestPositionOnCollider( position );
        }


        private Vector2 GetNearestPositionOnCollider( Vector2 position )
        {
            if( _colliders.Length == 1 )
                return _colliders[0].ClosestPoint( position );

            // There are multiple colliders. Calculate nearest 
            // position for each, and return the nearest position of all.

            // These variables will track the nearest thus far.
            Vector2 nearestPoint = new Vector2( float.MaxValue, float.MaxValue );
            float nearestDistanceSqr = float.MaxValue;

            for( int i = 0; i < _colliders.Length; ++i )
                if( _colliders[i] != null )
                {
                    Vector2 nearestPos = _colliders[i].ClosestPoint( position );

                    // Early exit if the nearest position could not be any nearer.
                    if( nearestPos == position )
                        return nearestPos;

                    // Square magnitude is cheaper to compute than full magnitude/distance...
                    float distance = Vector2.SqrMagnitude( position - nearestPos );
                    if( distance < nearestDistanceSqr )
                    {
                        nearestPoint = nearestPos;
                        nearestDistanceSqr = distance;
                    }
                }
            if( nearestDistanceSqr == float.MaxValue )
                Debug.LogError( "There are multiple colliders, but no nearest position could be derived.", this );

            return nearestPoint;

        }

        private void OnDrawGizmosSelected()
        {
            DrawGizmo();
        }

        /// <summary>
        /// If this Camera Track is a single position (rather than a shape), draw a reticle gizmo at that position.
        /// </summary>
        public void DrawGizmo()
        {
            if( _colliders == null || _colliders.Length == 0 )
            {
                Gizmos.color = GizmosHelper.ColliderColor;
                GizmosHelper.DrawCrosshairs2D( transform.position, 1f );
                GizmosHelper.DrawShapeDiamond2D( transform.position, 0.5f );
            }
        }



    }
}
