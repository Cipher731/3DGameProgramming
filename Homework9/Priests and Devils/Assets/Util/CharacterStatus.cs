namespace Util
{
    public enum CharacterType
    {
        Priest,
        Devil
    }

    public class CharacterStatus
    {
        public readonly int BankSlot;
        public bool OnBoard;
        public readonly CharacterType Type;

        public CharacterStatus(CharacterType type, int bankSlot)
        {
            Type = type;
            BankSlot = bankSlot;
        }
    }
}