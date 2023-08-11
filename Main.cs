using PrestigePlus.Feats;
using PrestigePlus.PrestigeClasses;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using UnityModManagerNet;
using PrestigePlus.Modify;

namespace PrestigePlus
{
  public static class Main
  {
    public static bool Enabled;
    private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");

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

    [HarmonyPatch(typeof(BlueprintsCache))]
    static class BlueprintsCaches_Patch
    {
      private static bool Initialized = false;

      [HarmonyPriority(Priority.First)]
      [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
      static void Init()
      {
        try
        {
          if (Initialized)
          {
            Logger.Info("Already configured blueprints.");
            return;
          }
          Initialized = true;

          Logger.Info("Configuring blueprints.");
                    SpellbookLevelUp.AccursedWitch(); SpellbookLevelUp.Alchemist(); SpellbookLevelUp.AngelfireApostle(); SpellbookLevelUp.Arcanist(); SpellbookLevelUp.ArmagsBlade(); SpellbookLevelUp.Bard(); SpellbookLevelUp.Bloodrager(); SpellbookLevelUp.Cleric(); SpellbookLevelUp.Crossblooded(); SpellbookLevelUp.Crusader(); SpellbookLevelUp.Druid(); SpellbookLevelUp.EldritchFont(); SpellbookLevelUp.EldritchScion(); SpellbookLevelUp.EldritchScoundrel(); SpellbookLevelUp.Empyreal(); SpellbookLevelUp.ExploiterWizard(); SpellbookLevelUp.Feyspeaker(); SpellbookLevelUp.Hunter(); SpellbookLevelUp.Inquisitor(); SpellbookLevelUp.LeyLineGuardianWitch(); SpellbookLevelUp.Magus(); SpellbookLevelUp.MasterSpy(); SpellbookLevelUp.NatureMage(); SpellbookLevelUp.Oracle(); SpellbookLevelUp.Paladin(); SpellbookLevelUp.Ranger(); SpellbookLevelUp.Sage(); SpellbookLevelUp.Shaman(); SpellbookLevelUp.Skald(); SpellbookLevelUp.Sorcerer(); SpellbookLevelUp.SwordSaint(); SpellbookLevelUp.ThassilonianAbjuration(); SpellbookLevelUp.ThassilonianConjuration(); SpellbookLevelUp.ThassilonianEnchantment(); SpellbookLevelUp.ThassilonianEvocation(); SpellbookLevelUp.ThassilonianIllusion(); SpellbookLevelUp.ThassilonianNecromancy(); SpellbookLevelUp.ThassilonianTransmutation(); SpellbookLevelUp.UnletteredArcanist(); SpellbookLevelUp.Warpriest(); SpellbookLevelUp.Witch(); SpellbookLevelUp.Wizard();
                    ShadowDancer.Configure(); ShadowDancer.ExtraShadowJump();DefensiveRoll.Configure(); InheritorCrusader.Configure();Chevalier.Configure();
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
