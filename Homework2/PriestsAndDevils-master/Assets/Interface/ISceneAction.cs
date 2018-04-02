using UnityEngine;

namespace Interface
{
    public interface IUserAction
    {
        void BoatGo();
        void CharacterMove(GameObject character);
        void StartGame();
    }
}