using System.Collections;
using UnityEngine;

namespace DQU
{
    public class PlayerLight : MonoBehaviour
    {
        private Transform _player;


        public void Configure( Transform player )
        {
            _player = player;
        }

        private void LateUpdate()
        {
            if( _player != null )
                transform.position = _player.position;
        }

    }
}