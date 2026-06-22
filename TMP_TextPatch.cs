using HarmonyLib;
using TMPro;

[HarmonyPatch(typeof(TMP_Text), "LoadDefaultSettings")]
class AllSubtitlesFontPatch
{
    static void Postfix(TMP_Text __instance)
    {
        __instance.font = TestFontPlugin._customTMPFont;
    }
}