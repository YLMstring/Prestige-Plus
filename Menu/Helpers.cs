using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PrestigePlus.Menu
{
    /// <summary>
    /// Generic utils for simple operations.
    /// </summary>
    internal static class Helpers
    {
        private static readonly List<LocalString> Strings = new();

        public static LocalizedString CreateString(string key, string enGB, string ruRU = "")
        {
            var localString = new LocalString(key, enGB, ruRU);
            Strings.Add(localString);
            localString.Register();
            return localString.LocalizedString;
        }

        private class LocalString
        {
            public readonly LocalizedString LocalizedString;
            private readonly string enGB;
            private readonly string ruRU;

            public LocalString(string key, string enGB, string ruRU)
            {
                LocalizedString = new LocalizedString() { m_Key = key };
                this.enGB = enGB;
                this.ruRU = ruRU;
            }

            public void Register()
            {
                var localized = enGB;
                switch (LocalizationManager.CurrentPack.Locale)
                {
                    case Locale.ruRU:
                        if (!string.IsNullOrEmpty(ruRU))
                            localized = ruRU;
                        break;
                }
                LocalizationManager.CurrentPack.PutString(LocalizedString.m_Key, localized);
            }
        }

        [HarmonyPatch(typeof(LocalizationManager))]
        static class LocalizationManager_Patch
        {
            [HarmonyPatch(nameof(LocalizationManager.OnLocaleChanged)), HarmonyPostfix]
            static void Postfix()
            {
                try
                {
                    Strings.ForEach(str => str.Register());
                }
                catch (Exception e)
                {
                    LogWrapper.Get("PrestigePlus").Error("Failed to handle locale change.", e);
                }
            }
        }
    }
}
