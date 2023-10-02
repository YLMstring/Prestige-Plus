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
using PrestigePlus.Grapple;
using Kingmaker.Controllers.Units;
using Kingmaker.GameModes;
using PrestigePlus.Maneuvers;
using PrestigePlus.BasePrestigeEnhance;

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
            Toggle.New(GetKey("grapple"), defaultValue: true, Helpers.CreateString("toggle-desc12", "Grapple Mechanic"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("maneuver"), defaultValue: true, Helpers.CreateString("toggle-desc20", "Combat Maneuver+"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("grave"), defaultValue: true, Helpers.CreateString("toggle-desc1", "Agent of the Grave"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("archer"), defaultValue: true, Helpers.CreateString("toggle-desc6", "Arcane Archer"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("asavir"), defaultValue: true, Helpers.CreateString("toggle-desc19", "Asavir"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("devotee"), defaultValue: true, Helpers.CreateString("toggle-desc7", "Deadeye Devotee (Arcane Archer archetype)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("chevalier"), defaultValue: true, Helpers.CreateString("toggle-desc2", "Chevalier"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("halfling"), defaultValue: true, Helpers.CreateString("toggle-desc21", "Halfling Opportunist"))
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
            Toggle.New(GetKey("biggg"), defaultValue: true, Helpers.CreateString("toggle-desc18", "Mammoth Rider"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("angel"), defaultValue: true, Helpers.CreateString("toggle-desc9", "Sanguine Angel (Lite)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("scar"), defaultValue: true, Helpers.CreateString("toggle-desc8", "Scar Seeker"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("dancer"), defaultValue: true, Helpers.CreateString("toggle-desc4", "Shadowdancer (Defensive Roll)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("umbral"), defaultValue: true, Helpers.CreateString("toggle-desc17", "Umbral Agent"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("seraph"), defaultValue: true, Helpers.CreateString("toggle-desc11", "Black Seraph Style (Path of War)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("stag"), defaultValue: true, Helpers.CreateString("toggle-desc14", "Charging Stag Style"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("grabbing"), defaultValue: true, Helpers.CreateString("toggle-desc13", "Grabbing Style"))
              .ShowVisualConnection())
           .AddToggle(
            Toggle.New(GetKey("kraken"), defaultValue: true, Helpers.CreateString("toggle-desc16", "Kraken Style"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("turtle"), defaultValue: true, Helpers.CreateString("toggle-desc15", "Snapping Turtle Style"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("enhance"), defaultValue: true, Helpers.CreateString("toggle-desc22", "Mythic Prestige Class Features"))
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
                    SpellbookReplace.Select(); SpellbookLevelUp.Select(); ImbueArrow.Configure(); ShadowDancer.CreateProficiencies(); ShadowDancer.ExtraShadowJump(); ShadowDancer.ExtraFeat();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("grapple"))) { ImprovedGrapple.StyleConfigure(); ImprovedGrappleMythic.Configure(); GreaterGrapple.Configure(); ManeuverExpert.Configure(); RapidGrappler.Configure(); UnfairGrip.Configure(); PinningKnockout.Configure(); PinningRend.Configure(); SavageSlam.Configure(); UncannyGrapple.Configure(); HamatulaStrike.Configure(); DramaticSlam.Configure(); KnotExpert.Configure(); AerialAssault.Configure(); MeatShield.Configure(); ThroatSlicer.Configure(); CrabKing.Configure(); CrabKing.Configure2(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("grave"))) AgentoftheGrave.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("archer"))) { ArcaneArcher.Configure(); StormArrow.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("archer")) && ModMenu.ModMenu.GetSettingValue<bool>(GetKey("devotee"))) DeadeyeDevotee.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("chevalier"))) Chevalier.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("crusader"))) InheritorCrusader.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("lander"))) Hinterlander.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("walker"))) HorizonWalker.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("angel"))) { SanguineAngel.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("scar"))) { ScarSeeker.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("dancer"))) { ShadowDancer.Configure(); DefensiveRoll.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("seraph"))) { BlackSeraphStyle.StyleConfigure(); BlackSeraphStyle.MalevolenceConfigure(); BlackSeraphStyle.AnnihilationConfigure(); RippleWater.Configure(); SeizetheOpportunity.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("grabbing"))) { GrabbingStyle.StyleConfigure(); GrabbingStyle.DragConfigure(); GrabbingStyle.MasterConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("stag"))) { StagStyle.StyleConfigure(); StagStyle.HornsConfigure(); StagStyle.SubmissionConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("turtle"))) { SnappingTurtleStyle.StyleConfigure(); SnappingTurtleStyle.ClutchConfigure(); SnappingTurtleStyle.ShellConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("kraken"))) { KrakenStyle.StyleConfigure(); KrakenStyle.WrackConfigure(); KrakenStyle.WrathConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("umbral"))) { UmbralAgent.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("biggg"))) { MammothRider.Configure(); GiganticAssault.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("asavir"))) { Asavir.Configure(); BondGenie.Configure(); TrampleMythic.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("maneuver"))) { ReplaceAttack.DisarmConfigure(); ReplaceAttack.SunderConfigure(); ReplaceAttack.TripConfigure(); StrengthSurge.StrengthSurgeFeat(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("halfling"))) { HalflingOpportunist.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("enhance"))) { SwiftDeath.Configure(); SwiftDeath.Configure2(); UnbreakableDefance.Configure(); MythicWings.Configure(); MythicWings.Configure2(); }
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
