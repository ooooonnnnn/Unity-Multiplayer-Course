namespace Enums
{
    [System.Flags]
    public enum GameModes
    {
        Any = 0xFFFF,
        Fun = 0b1,
        Boring = 0b10
    }
}