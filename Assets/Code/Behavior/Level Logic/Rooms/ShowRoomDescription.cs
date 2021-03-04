using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using DQU.Events;
using SDD.Events;

namespace DQU
{
    public class ShowRoomDescription : MonoBehaviour
    {
        [SerializeField]
        private LocalizedString _descriptionString;

        [SerializeField]
        private float _delayInSeconds;

        [SerializeField]
        private RoomDescriptionLayout _textLayout;

        public enum ActivationType { None, OnTriggerEnter, OnRoomEnter }
        [SerializeField]
        private ActivationType _activationType;

        private ShowRoomDescriptionsEvent _showDescriptionEvent = new ShowRoomDescriptionsEvent();


        private void OnEnable()
        {
            if( _activationType == ActivationType.OnRoomEnter )
            {
                Room room = GetComponent<Room>();
                if( room != null )
                {
                    room.OnRoomEnter.AddListener( ShowDescription );
                }
                else
                    Debug.LogError( "Show Room Description is set to activate On Room Enter, but can not find a Room component.", this );
            }
        }


        public void ShowDescription()
        {
            EventManager.Instance.Raise( 
                _showDescriptionEvent.Configure( _descriptionString, _delayInSeconds, _textLayout ) );
        }


        private void OnTriggerEnter2D( Collider2D collision )
        {
            if( _activationType == ActivationType.OnTriggerEnter && 
                collision.gameObject.layer.Equals( LayerHelper.Player) )
            {
                ShowDescription();
            }
        }


    }
}