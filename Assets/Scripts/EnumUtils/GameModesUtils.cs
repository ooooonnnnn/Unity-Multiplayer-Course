using System.Diagnostics;
using Enums;

namespace EnumUtils
{
    public static class GameModesUtils
    {
        public static string GetDisplayName(this GameModes mode) => 
            mode switch
            {
                GameModes.Any => "All Game Modes",
                GameModes.Fun => "Fun Mode",
                GameModes.Boring => "Boring Mode"
            };
    }
}