using UnityEngine;

namespace DQU
{
    /// <summary>
    /// <para>Assists in working with Unity's Layers.</para>
    /// Always use this Helper class to reference specific Layers, rather than by string or number.
    /// </summary>
    public static class LayerHelper
    {
        // Unity default layers
        public static int Default       = LayerMask.NameToLayer( "Default" );
        public static int TransparentFX = LayerMask.NameToLayer( "TransparentFX" );
        public static int IgnoreRaycast = LayerMask.NameToLayer( "Ignore Raycast" );
        public static int Water         = LayerMask.NameToLayer( "Water" );
        public static int UI            = LayerMask.NameToLayer( "UI" );

        // DQU layers
        public static int NoCollision   = LayerMask.NameToLayer( "NoCollision" );


        /// <summary>
        /// Examine all 32 potential Layers and test if they are set to collide with 
        /// the specified Layer. The results are collected into a LayerMask and returned.
        /// </summary>
        public static LayerMask GetCollisionMaskForLayer( int layer )
        {
            LayerMask collisionMask = 0;
            for( int i = 0; i < 32; ++i )
            {
                if( !Physics.GetIgnoreLayerCollision( layer, i ) )
                {
                    collisionMask.AddLayer( i );
                }
            }
            return collisionMask;
        }

    }
}