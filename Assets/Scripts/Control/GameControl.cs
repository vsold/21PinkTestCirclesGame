using UnityEngine;
using System.Collections;

namespace CirclesGame
{   
    [RequireComponent(typeof (CirclesSpawnerComponent))]
    public class GameControl : MonoBehaviour
    {
        [SerializeField]
        private CirclesSpawnerComponent circlesSpawner;
        private GameModel model;

        private void Start()
        {
            model = new GameModel();
            circlesSpawner.Model = model;
        }

        private void StartGame()
        {

        }
    }
}

