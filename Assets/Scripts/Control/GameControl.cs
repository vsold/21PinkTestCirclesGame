using UnityEngine;
using System.Collections;

namespace CirclesGame
{
    public class GameControl : MonoBehaviour
    {
        readonly GameModel model = new GameModel();

        private void StartGame()
        {
            model.Score = 0;

        }
    }
}

