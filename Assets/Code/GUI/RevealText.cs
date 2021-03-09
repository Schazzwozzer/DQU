using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Archon.SwissArmyLib.Coroutines;

namespace DQU.GUI
{
    /// <summary>
    /// Animates the gradual reveal of the text of a specified <see cref="Text"/> component.
    /// </summary>
    public class RevealText : MonoBehaviour
    {
        [Range(0f, 10f)]
        public float SpeedMultiplier = 1f;

        private StringBuilder _stringBuilder = new StringBuilder();
        private int _revealRoutineID;
        private float _revealSpeed = 60f;   // Characters per second

        private float sentenceDelayMin = 0.3333333f,
                      sentenceDelayMax = 1f,
                      lineBreakDelayMin = 0.75f,
                      lineBreakDelayMax = 2f;

        // These are class-scope so that they can be referenced and changed from outside of the coroutine.
        private string _currentString;
        private int _currentCharactersRevealed;


        private void OnDisable()
        {
            _currentString = default;
            _currentCharactersRevealed = 0;
        }


        public void BeginReveal( Text targetGUI, float speedMultiplier, float delay = 0f )
        {
            StartNewCoroutine( targetGUI, speedMultiplier, delay );
        }


        private void StartNewCoroutine( Text targetGUI, float speedMultiplier, float delay = 0f )
        {
            BetterCoroutines.Stop( _revealRoutineID );

            _revealRoutineID = BetterCoroutines.Start( RevealTextRoutine(
                targetGUI, _revealSpeed * speedMultiplier, delay ) );
        }


        private IEnumerator RevealTextRoutine( Text targetGUI, float revealSpeed, float delay )
        {
            _currentCharactersRevealed = 0;
            targetGUI.text = string.Empty;

            yield return BetterCoroutines.WaitForOneFrame;

            if( delay > 0 )
                yield return BetterCoroutines.WaitForSeconds( delay );

            float perCharDelay = 1f / revealSpeed;
            float sentenceDelay = Mathf.Clamp( perCharDelay * 50f, sentenceDelayMin, sentenceDelayMax );
            float lineBreakDelay = Mathf.Clamp( perCharDelay * 100f, lineBreakDelayMin, lineBreakDelayMax );

            while( _currentCharactersRevealed < _currentString.Length )
            {
                UpdateTextGUI( targetGUI );

                if( _currentCharactersRevealed + 1 < _currentString.Length )
                {
                    char c = _currentString[_currentCharactersRevealed];
                    if( StringHelper.IsEndOfSentence( c, _currentString[_currentCharactersRevealed + 1] ) )
                    {
                        yield return BetterCoroutines.WaitForSeconds( sentenceDelay );
                    }
                    else if( StringHelper.IsLineBreak( c ) )
                    {
                        yield return BetterCoroutines.WaitForSeconds( lineBreakDelay );
                        // Essentially fast-forward to the next actual letter.
                        // We don't want multiple line break delays at once.
                        while( _currentCharactersRevealed + 1 < _currentString.Length &&
                            char.IsWhiteSpace( _currentString[_currentCharactersRevealed + 1] ) )
                        {
                            ++_currentCharactersRevealed;
                        }
                    }
                    else if( !StringHelper.IsSpace( c ) )
                        yield return BetterCoroutines.WaitForSeconds( perCharDelay );// perCharYield;
                }

                ++_currentCharactersRevealed;
            }

            targetGUI.text = _currentString;
        }


        public void Stop()
        {
            BetterCoroutines.Stop( _revealRoutineID );
        }


        /// <summary>
        /// Change the text that is being revealed.
        /// </summary>
        public void UpdateText( Text targetGUI, string text )
        {
            _currentString = text;
            _currentCharactersRevealed = Math.Min( 0, Math.Min( _currentCharactersRevealed, _currentString.Length - 1 ) );

            if( BetterCoroutines.IsRunning( _revealRoutineID ) )
                UpdateTextGUI( targetGUI );
            else
                targetGUI.text = _currentString;
        }


        /// <summary>
        /// Set the <see cref="Text"/> component's display string,
        /// depending on the current progress of the Text Reveal routine.
        /// </summary>
        private void UpdateTextGUI( Text targetGUI )
        {
            _stringBuilder.Clear();

            _stringBuilder.Append( _currentString.Substring( 0, _currentCharactersRevealed + 1 ) );
            _stringBuilder.Append( "<color=#FFFFFF00>" );
            _stringBuilder.Append( _currentString.Substring( _currentCharactersRevealed + 1 ) );
            _stringBuilder.Append( "</color>" );

            targetGUI.text = _stringBuilder.ToString();
        }

    }
}