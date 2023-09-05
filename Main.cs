using PrestigePlus.Feats;
using PrestigePlus.PrestigeClasses;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using UnityModManagerNet;
using PrestigePlus.Modify;
using ModMenu.Settings;
using PrestigePlus.Menu;
using System.Globalization;
using System.Text;

namespace PrestigePlus
{
  public static class Main
  {
    public static bool Enabled;
    private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static readonly string RootKey = "mod-menu.test-settings";
        private static string GetKey(string partialKey)
        {
            return $"{RootKey}.{partialKey}";
        }

        public static bool Load(UnityModManager.ModEntry modEntry)
    {
      try
      {
        modEntry.OnToggle = OnToggle;
        var harmony = new Harmony(modEntry.Info.Id);
        harmony.PatchAll();
        Logger.Info("Finished patching.");
      }
      catch (Exception e)
      {
        Logger.Error("Failed to patch", e);
      }
      return true;
    }

    public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
    {
      Enabled = value;
      return true;
    }
        private static void onclick()
        {
            var log = new StringBuilder();
            log.AppendLine("Current settings: ");
            ///log.AppendLine($"-Toggle: {CheckToggle()}");
            log.AppendLine($"-Default Slider Float: {ModMenu.ModMenu.GetSettingValue<float>(GetKey("float-default"))}");
            log.AppendLine($"-Slider Float: {ModMenu.ModMenu.GetSettingValue<float>(GetKey("float"))}");
            log.AppendLine($"-Default Slider Int: {ModMenu.ModMenu.GetSettingValue<int>(GetKey("int-default"))}");
            log.AppendLine($"-Slider Int: {ModMenu.ModMenu.GetSettingValue<int>(GetKey("int"))}");
            Logger.Info(log.ToString());
        }

        [HarmonyPatch(typeof(BlueprintsCache))]
    static class BlueprintsCaches_Patch
    {
      private static bool Initialized = false;

      [HarmonyPriority(Priority.First)]
      [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]

      static void Init()
      {
       ModMenu.ModMenu.AddSettings(
        SettingsBuilder.New(RootKey, Helpers.CreateString("title", "Prestige Plus"))
          .AddDefaultButton()
          .AddButton(
            Button.New(
              Helpers.CreateString("button-desc", "Restart the game to apply changes!"), Helpers.CreateString("button-text", "Do Not Turn Any Chosen Features Off"), onclick))
          .AddToggle(
            Toggle.New(GetKey("grave"), defaultValue: true, Helpers.CreateString("toggle-desc1", "Agent of the Grave"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("archer"), defaultValue: true, Helpers.CreateString("toggle-desc6", "Arcane Archer"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("devotee"), defaultValue: true, Helpers.CreateString("toggle-desc7", "Deadeye Devotee (Arcane Archer archetype)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("chevalier"), defaultValue: true, Helpers.CreateString("toggle-desc2", "Chevalier"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("lander"), defaultValue: true, Helpers.CreateString("toggle-desc5", "Hinterlander"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("walker"), defaultValue: true, Helpers.CreateString("toggle-desc10", "Horizon Walker"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("crusader"), defaultValue: true, Helpers.CreateString("toggle-desc3", "Inheritor¡¯s Crusader"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("angel"), defaultValue: true, Helpers.CreateString("toggle-desc9", "Sanguine Angel (Lite)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("scar"), defaultValue: true, Helpers.CreateString("toggle-desc8", "Scar Seeker"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("dancer"), defaultValue: true, Helpers.CreateString("toggle-desc4", "Shadowdancer (Defensive Roll)"))
              .ShowVisualConnection()));

                try
        {
          if (Initialized)
          {
            Logger.Info("Already configured blueprints.");
            return;
          }
          Initialized = true;

          Logger.Info("Configuring blueprints.");
                    SpellbookReplace.Select(); SpellbookLevelUp.Select(); ImbueArrow.Configure(); ShadowDancer.CreateProficiencies();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("grave"))) AgentoftheGrave.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("archer"))) { ArcaneArcher.Configure(); StormArrow.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("archer")) && ModMenu.ModMenu.GetSettingValue<bool>(GetKey("devotee"))) DeadeyeDevotee.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("chevalier"))) Chevalier.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("crusader"))) InheritorCrusader.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("lander"))) Hinterlander.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("walker"))) HorizonWalker.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("angel"))) { SanguineAngel.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("scar"))) { ScarSeeker.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("dancer"))) { ShadowDancer.Configure(); ShadowDancer.ExtraShadowJump(); DefensiveRoll.Configure(); }
                }
        catch (Exception e)
        {
          Logger.Error("Failed to configure blueprints.", e);
        }
      }
    }

    [HarmonyPatch(typeof(StartGameLoader))]
    static class StartGameLoader_Patch
    {
      private static bool Initialized = false;

      [HarmonyPatch(nameof(StartGameLoader.LoadPackTOC)), HarmonyPostfix]
      static void LoadPackTOC()
      {
        try
        {
          if (Initialized)
          {
            Logger.Info("Already configured delayed blueprints.");
            return;
          }
          Initialized = true;

          RootConfigurator.ConfigureDelayedBlueprints();
        }
        catch (Exception e)
        {
          Logger.Error("Failed to configure delayed blueprints.", e);
        }
      }
    }
  }
}
