using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SDD.Events;
using DQU.Events;
using UnityEngine.Localization.Components;

namespace DQU.GUI
{

    /// <summary>
    /// Listens for <see cref="ShowRoomDescriptionsEvent"/>s and 
    /// passes the description string along to a Text GUI element.
    /// </summary>
    public class RoomDescriptionsGUI : MonoBehaviour
    {
        [Header( "Text Components" )]
        [SerializeField] private Text _screenTopText;
        [SerializeField] private Text _screenBottomText;
        [SerializeField] private Text _screenLeftText;
        [SerializeField] private Text _screenRightText;

        /// <summary>
        /// Maintains reference to the current room description string and notifies listeners when it changes.
        /// </summary>
        [SerializeField] private LocalizeStringEvent _localizeComponent;
        [SerializeField] private RevealText _revealTextComponent;

        private Text _currentTargetText;


        private void OnEnable()
        {
            EventManager.Instance.AddListener<ShowRoomDescriptionsEvent>( OnShowDescription );
        }


        private void OnDisable()
        {
            EventManager.Instance.RemoveListener<ShowRoomDescriptionsEvent>( OnShowDescription );
        }


        private void OnShowDescription( ShowRoomDescriptionsEvent e )
        {
            _currentTargetText = ToggleAndRetrieveTextElement( e.TextLayout );

            // Note that we must update the listener before changing the string reference.
            _localizeComponent.OnUpdateString.RemoveAllListeners();
            _localizeComponent.OnUpdateString.AddListener( LocalizationChanged ); //textGUI.text = x );

            // Here we update the string, which should call the UnityAction delegate we just passed in.
            // Why do it this way? If the Player were to change their Language 
            // setting, it should ensure that the Room Description dynamically updates.
            _localizeComponent.StringReference = e.RoomDescription;

            _revealTextComponent.BeginReveal( _currentTargetText, 1.0f, e.Delay );
        }


        private Text ToggleAndRetrieveTextElement( RoomDescriptionLayout layout )
        {
            switch( layout )
            {
                case RoomDescriptionLayout.ScreenTop:
                    _screenTopText.gameObject.SetActive( true );
                    _screenBottomText.gameObject.SetActive( false );
                    _screenLeftText.gameObject.SetActive( false );
                    _screenRightText.gameObject.SetActive( false );
                    return _screenTopText;
                case RoomDescriptionLayout.ScreenBottom:
                    _screenTopText.gameObject.SetActive( false );
                    _screenBottomText.gameObject.SetActive( true );
                    _screenLeftText.gameObject.SetActive( false );
                    _screenRightText.gameObject.SetActive( false );
                    return _screenBottomText;
                case RoomDescriptionLayout.ScreenLeft:
                    _screenTopText.gameObject.SetActive( false );
                    _screenBottomText.gameObject.SetActive( false );
                    _screenLeftText.gameObject.SetActive( true );
                    _screenRightText.gameObject.SetActive( false );
                    return _screenLeftText;
                case RoomDescriptionLayout.ScreenRight:
                    _screenTopText.gameObject.SetActive( false );
                    _screenBottomText.gameObject.SetActive( false );
                    _screenLeftText.gameObject.SetActive( false );
                    _screenRightText.gameObject.SetActive( true );
                    return _screenRightText;
                default:
                    Debug.LogWarning( "Unsupported Room Description Layout enum", this );
                    return _screenBottomText;
            }
        }


        private void LocalizationChanged( string text )
        {
            _revealTextComponent.UpdateText( _currentTargetText, text );
        }


    }
}