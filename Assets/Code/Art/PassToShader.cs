using System.Collections.Generic;
using UnityEngine;
using DQU.Events;
using SDD.Events;

namespace DQU.Art
{
    /// <summary>
    /// Facilitates passing of data into shaders.
    /// </summary>
    public class PassToShader : MonoBehaviour
    {
        [SerializeField]
        private bool _useShadowColor;

        // Used to preload this component with targets within the Unity editor.
        [SerializeField]
        private Renderer[] _targets;

        private HashSet<Renderer> _runtimeTargets = new HashSet<Renderer>();

        private static int _shadowColorID = Shader.PropertyToID( "_ColorShadow" );


        private void Start()
        {
            for( int i = 0; i < _targets.Length; ++i )
                _runtimeTargets.Add( _targets[i] );
        }


        private void OnEnable()
        {
            if( _useShadowColor )
                EventManager.Instance.AddListener<ShadowColorChangedEvent>( OnShadowColorChanged );
        }


        private void OnDisable()
        {
            if( _useShadowColor )
                EventManager.Instance.RemoveListener<ShadowColorChangedEvent>( OnShadowColorChanged );
        }


        public bool AddTarget( Renderer target )
        {
            return _runtimeTargets.Add( target );
        }

        public bool RemoveTarget( Renderer target )
        {
            return _runtimeTargets.Remove( target );
        }


        private void OnShadowColorChanged( ShadowColorChangedEvent e )
        {
            for( int i = 0; i < _targets.Length; ++i )
                _targets[i].material.SetColor( _shadowColorID, e.ShadowColor );
        }

    }
}