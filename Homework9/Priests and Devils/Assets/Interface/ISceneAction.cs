using UnityEngine;

namespace Interface
{
    public interface ISceneAction
    {
        void BoatGo();
        void CharacterMove(GameObject character);
        void StartGame();
        void Help();
    }
}