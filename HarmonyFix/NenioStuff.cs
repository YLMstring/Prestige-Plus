using HarmonyLib;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.Localization.Shared;
using Kingmaker.Localization;
using Kingmaker.TextTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(BlueprintCue), nameof(BlueprintCue.DisplayText), MethodType.Getter)]
    internal class DualLanguageMode
    {
        static void Postfix(ref string __result, ref BlueprintCue __instance)
        {
            try
            {
                if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("enlearn"))) { return; }
                if (LocalizationManager.CurrentLocale == Locale.enGB)
                {
                    return;
                }
                string en = GetEnglish(__instance.Text);
                __result = __result + "\n" + en;
            }
            catch { }
        }

        public static string GetEnglish(LocalizedString local)
        {
            string text = local.LoadString(pack, Locale.enGB);
            if (local.ShouldProcess)
            {
                return TextTemplateEngine.Process(text);
            }
            return text;
        }

        private static LocalizationPack pack = LocalizationManager.LoadPack(Locale.enGB);
    }

    [HarmonyPatch(typeof(BlueprintAnswer), nameof(BlueprintAnswer.DisplayText), MethodType.Getter)]
    internal class DualLanguageMode2
    {
        static void Postfix(ref string __result, ref BlueprintAnswer __instance)
        {
            try
            {
                if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("enlearn"))) { return; }
                if (LocalizationManager.CurrentLocale == Locale.enGB)
                {
                    return;
                }
                string en = DualLanguageMode.GetEnglish(__instance.Text);
                __result = __result + "\n" + en;
            }
            catch { }
        }
    }

    [HarmonyPatch(typeof(LocalizedString), nameof(LocalizedString.LoadString))]
    internal class DualLanguageMode3
    {
        static void Postfix(ref string __result, ref LocalizedString __instance)
        {
            try
            {
                if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("modsuspect"))) { return; }
                if (!__result.Contains(".") && !__result.Contains("。")) { return; }
                string actualKey = __instance.GetActualKey();
                LocalizationPack.StringEntry stringEntry;
                if (!pack1.m_Strings.TryGetValue(actualKey, out stringEntry) || !pack2.m_Strings.TryGetValue(actualKey, out stringEntry))
                {
                    __result += " (mod)";
                }
            }
            catch { }
        }

        private static LocalizationPack pack1 = LocalizationManager.LoadPack(Locale.enGB);
        private static LocalizationPack pack2 = LocalizationManager.LoadPack(Locale.zhCN);
    }
}
