using System.Collections;
using UnityEngine;

namespace DQU.Art
{
    /// <summary>
    /// Snaps the Transform's position to the nearest pixel, on the X and Y axis.
    /// Used on non-sprite art to restrict sub-pixel filtering.
    /// </summary>
    public class SnapToPixel : MonoBehaviour
    {
        Vector3 _originalOffset;

        private void Start()
        {
            _originalOffset = transform.localPosition;
        }

        private void LateUpdate()
        {
            transform.localPosition = _originalOffset;
            transform.position = new Vector3(
                MathHelper.RoundToNearest( transform.position.x, Constants.PixelSize ) + Constants.HalfPixelSize,
                MathHelper.RoundToNearest( transform.position.y, Constants.PixelSize ) + Constants.HalfPixelSize, 
                transform.position.z );
        }

    }
}