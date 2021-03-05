using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DQU.GUI
{
    /// <summary>
    /// Animates the gradual reveal of the text of a specified <see cref="Text"/> component.
    /// </summary>
    public class RevealText : MonoBehaviour
    {
        [Range(0f, 10f)]
        public float SpeedMultiplier = 1f;

        [SerializeField]
        private Text _targetText;
        public Text TargetText
        {
            get { return _targetText; }
            set
            {
                Stop();
                _targetText = value;
            }
        }

        private StringBuilder _stringBuilder = new StringBuilder();
        private Coroutine _revealRoutine;
        private float _revealSpeed = 60f;   // Characters per second

        private float sentenceDelayMin = 0.3333333f,
                      sentenceDelayMax = 1f,
                      lineBreakDelayMin = 0.75f,
                      lineBreakDelayMax = 2f;


        public void BeginReveal( string text, float speedMultiplier, float delay = 0f )
        {
            _revealRoutine = StartCoroutine( 
                RevealTextRoutine( text, _revealSpeed * speedMultiplier, delay ) );
        }


        public void BeginReveal( float speedMultiplier, float delay = 0f )
        {
            _revealRoutine = StartCoroutine(
                RevealTextRoutine( TargetText.text, _revealSpeed * speedMultiplier, delay ) );
        }


        private IEnumerator RevealTextRoutine( string text, float revealSpeed, float delay )
        {
            _targetText.text = string.Empty;

            if( delay > 0 )
                yield return new WaitForSeconds( delay );

            int charactersRevealed = 0;
            float perCharDelay = 1f / revealSpeed;
            WaitForSeconds perCharYield = new WaitForSeconds( perCharDelay );
            WaitForSeconds sentenceYield = new WaitForSeconds( Mathf.Clamp( 
                perCharDelay * 50f, sentenceDelayMin, sentenceDelayMax ) );
            WaitForSeconds lineBreakYield = new WaitForSeconds( Mathf.Clamp( 
                perCharDelay * 100f, lineBreakDelayMin, lineBreakDelayMax ) );

            while( charactersRevealed < text.Length )
            {
                _stringBuilder.Clear();

                _stringBuilder.Append( text.Substring( 0, charactersRevealed + 1 ) );
                _stringBuilder.Append( "<color=#FFFFFF00>" );
                _stringBuilder.Append( text.Substring( charactersRevealed + 1 ) );
                _stringBuilder.Append( "</color>" );

                _targetText.text = _stringBuilder.ToString();

                if( charactersRevealed + 1 < text.Length )
                {
                    char c = text[charactersRevealed];
                    if( StringHelper.IsEndOfSentence( c, text[charactersRevealed + 1] ) )
                    {
                        yield return sentenceYield;
                    }
                    else if( StringHelper.IsLineBreak( c ) )
                    {
                        yield return lineBreakYield;
                        // Essentially fast-forward to the next actual letter.
                        // We don't want multiple line break delays at once.
                        while( charactersRevealed + 1 < text.Length &&
                            char.IsWhiteSpace( text[charactersRevealed + 1] ) )
                        {
                            ++charactersRevealed;
                        }
                    }
                    else if( !StringHelper.IsSpace( c ) )
                        yield return perCharYield;
                }

                ++charactersRevealed;
            }
        }


        public void Stop()
        {
            if( _revealRoutine != null )
                StopCoroutine( _revealRoutine );
        }

    }
}