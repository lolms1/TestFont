using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Rewired.UI.ControlMapper;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("blitzo.baldiplus.testfont", "Test Font", "1.0.0")]
public class TestFontPlugin : BaseUnityPlugin
{
    public static TMP_FontAsset? _customTMPFont;
    // private static Font? _customFont;
    private ConfigEntry<bool> _enableFontOverride;

    private void Awake()
    {
        Harmony harmony = new Harmony("blitzo.baldiplus.testfont");

        _enableFontOverride = Config.Bind("General", "EnableFontOverride", true, "Turn on replace font LiberationSans SDF.");

        if (_enableFontOverride.Value)
        {
            LoadFont();
            harmony.PatchAll();
        }
        else
        { Logger.LogInfo("Font replace is off.");
        }
           
    }

    private void LoadFont()
    {
        /* Font arial = Resources.GetBuiltinResource<Font>("Arial.ttf");
        if (arial != null)
        {
            _customFont = arial;
            _customTMPFont = TMP_FontAsset.CreateFontAsset(arial);
            if (_customTMPFont != null)
                return;
        }

        string[] systemFonts = { "Segoe UI", "Times New Roman", "Arial" };
        foreach (string name in systemFonts)
        {
            try
            {
                Font sysFont = Font.CreateDynamicFontFromOSFont(name, 16);
                if (sysFont != null)
                {
                    _customFont = sysFont;
                    _customTMPFont = TMP_FontAsset.CreateFontAsset(sysFont);
                    if (_customTMPFont != null)
                        return;
                }
            }
            catch { }
        } */ // These fonts are not in the game

        var libFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
            .FirstOrDefault(f => f.name.Contains("LiberationSans SDF"));
        if (libFont != null)
        {
            _customTMPFont = libFont;
            return;
        }
    }
}