using BepInEx;
using BepInEx.Configuration;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("blitzo.baldiplus.testfont", "Test Font", "1.0.0")]
public class TestFontPlugin : BaseUnityPlugin
{
    private static TMP_FontAsset? _customTMPFont;
    private static Font? _customFont;
    private ConfigEntry<bool> _enableFontOverride;

    private void Awake()
    {
        _enableFontOverride = Config.Bind("General", "EnableFontOverride", true, "Turn on replace font LiberationSans SDF.");

        if (_enableFontOverride.Value)
        {
            LoadFont();
            StartCoroutine(ApplyFontPeriodically());
        }
        else
        { Logger.LogInfo("Font replace is off.");
        }
           
    }

    private void LoadFont()
    {
        Font arial = Resources.GetBuiltinResource<Font>("Arial.ttf");
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
        }

        var libFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
            .FirstOrDefault(f => f.name.Contains("LiberationSans SDF"));
        if (libFont != null)
        {
            _customTMPFont = libFont;
            return;
        }

    }

    private IEnumerator ApplyFontPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (_enableFontOverride.Value)
                ApplyFontToAll();
            else
                yield break; 
        }
    }

    private void ApplyFontToAll()
    {
        if (_customTMPFont == null && _customFont == null) return;

        var tmpUI = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        foreach (var t in tmpUI)
            if (_customTMPFont != null && t.font != _customTMPFont)
                t.font = _customTMPFont;

        var tmp3D = Resources.FindObjectsOfTypeAll<TextMeshPro>();
        foreach (var t in tmp3D)
            if (_customTMPFont != null && t.font != _customTMPFont)
                t.font = _customTMPFont;

        if (_customFont != null)
        {
            var uiTexts = Resources.FindObjectsOfTypeAll<Text>();
            foreach (var t in uiTexts)
                if (t.font != _customFont)
                    t.font = _customFont;
        }
    }
}