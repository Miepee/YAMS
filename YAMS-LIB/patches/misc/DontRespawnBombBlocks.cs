using UndertaleModLib;
using UndertaleModLib.Decompiler;
using UndertaleModLib.Models;

namespace YAMS_LIB.patches.misc;

public class DontRespawnBombBlocks
{
    public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, SeedObject seedObject)
    {
        UndertaleCode? characterVarsCode = gmData.Code.ByName("gml_Script_load_character_vars");

        // Stop Bomb blocks from respawning
        characterVarsCode.AppendGMLInCode($"global.respawnBombBlocks = {(seedObject.Patches.RespawnBombBlocks ? "1" : "0")}");

        // The position here is for a puzzle in a2, that when not respawned makes it a tad hard.
        gmData.Code.ByName("gml_Object_oBlockBomb_Other_10").PrependGMLInCode("if (!global.respawnBombBlocks && !(room == rm_a2a06 && x == 624 && y == 128)) regentime = -1");

        // The bomb block puzzle in te room before varia don't need to have their special handling from am2random
        var rm_a2a06 = gmData.Rooms.ByName("rm_a2a06");
        rm_a2a06.GameObjects.First(go => go.X == 608 && go.Y == 112 && go.ObjectDefinition.Name.Content == "oBlockBomb").CreationCode.ReplaceGMLInCode(
            "if (oControl.mod_randomgamebool == 1 && global.hasBombs == 0 && !global.hasJumpball && global.hasGravity == 0)",
            "if (false)");
        rm_a2a06.GameObjects.First(go => go.X == 624 && go.Y == 48 && go.ObjectDefinition.Name.Content == "oBlockBomb").CreationCode.ReplaceGMLInCode(
            "if (oControl.mod_randomgamebool == 1 && global.hasBombs == 0 && global.hasGravity == 0)",
            "if (false)");
    }
}
