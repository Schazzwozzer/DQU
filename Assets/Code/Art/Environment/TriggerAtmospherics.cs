using System.Collections;
using UnityEngine;
using SDD.Events;
using DQU.Events;

namespace DQU.Art
{
    public class TriggerAtmospherics : MonoBehaviour
    {

        [SerializeField]
        private bool _changeShadowColor;

        [SerializeField]
        private Color _shadowColor;


        public void Activate()
        {
            if( _changeShadowColor )
            {
                EventManager.Instance.Raise( new ShadowColorChangedEvent( _shadowColor ) );
            }
        }
        


    }
}