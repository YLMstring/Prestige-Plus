using PrestigePlus.Feats;
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
using Kingmaker.Controllers.Units;
using Kingmaker.GameModes;
using PrestigePlus.Maneuvers;
using PrestigePlus.BasePrestigeEnhance;
using PrestigePlus.Patch;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Blueprint.RogueTalent;
using PrestigePlus.Blueprint.CombatStyle;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Blueprint.MythicFeat;
using PrestigePlus.Blueprint.ManeuverFeat;
using PrestigePlus.Blueprint;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.Blueprint.SpecificManeuver;
using PrestigePlus.Blueprint.Spell;

namespace PrestigePlus
{
    public static class Main
  {
    public static bool Enabled;
    private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static readonly string RootKey = "mod-menu.test-settings";
        public static string GetKey(string partialKey)
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
      //[HarmonyBefore(new string[] { "TabletopTweaks-Base" })]
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
            Toggle.New(GetKey("thc"), defaultValue: false, Helpers.CreateString("toggle-desc34", "Estimated THC (not precise)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("autoheal"), defaultValue: true, Helpers.CreateString("toggle-desc35", "Auto Heal"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("weakencrit"), defaultValue: false, Helpers.CreateString("toggle-desc37", "Weak Crits against Enemies"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("maneuver"), defaultValue: true, Helpers.CreateString("toggle-desc20", "Combat Maneuver+"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("bullrush"), defaultValue: true, Helpers.CreateString("toggle-desc43", "Bull Rush Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("dirty"), defaultValue: true, Helpers.CreateString("toggle-desc52", "Dirty Trick Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("disarm"), defaultValue: true, Helpers.CreateString("toggle-desc42", "Disarm Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("grapple"), defaultValue: true, Helpers.CreateString("toggle-desc12", "Grapple Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("sunder"), defaultValue: true, Helpers.CreateString("toggle-desc44", "Sunder Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("trip"), defaultValue: true, Helpers.CreateString("toggle-desc45", "Trip Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("grave"), defaultValue: true, Helpers.CreateString("toggle-desc1", "Agent of the Grave"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("dawnflower"), defaultValue: true, Helpers.CreateString("toggle-desc27", "Anchorite of Dawn"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("archer"), defaultValue: true, Helpers.CreateString("toggle-desc6", "Arcane Archer"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("devotee"), defaultValue: true, Helpers.CreateString("toggle-desc7", "Deadeye Devotee (Arcane Archer archetype)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("asavir"), defaultValue: true, Helpers.CreateString("toggle-desc19", "Asavir"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("chevalier"), defaultValue: true, Helpers.CreateString("toggle-desc2", "Chevalier"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("templar"), defaultValue: true, Helpers.CreateString("toggle-desc56", "Crimson Templar"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("bitch"), defaultValue: true, Helpers.CreateString("toggle-desc82", "Enchanting Courtesan"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("kineticknight"), defaultValue: true, Helpers.CreateString("toggle-desc31", "Esoteric Knight"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("furious"), defaultValue: true, Helpers.CreateString("toggle-desc36", "Furious Guardian"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("goldenl"), defaultValue: true, Helpers.CreateString("toggle-desc88", "Golden Legionnaire"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("halfling"), defaultValue: true, Helpers.CreateString("toggle-desc21", "Halfling Opportunist"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("justshowup"), defaultValue: true, Helpers.CreateString("toggle-desc87", "Heritor Knight"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("lander"), defaultValue: true, Helpers.CreateString("toggle-desc5", "Hinterlander"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("walker"), defaultValue: true, Helpers.CreateString("toggle-desc10", "Horizon Walker"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("crusader"), defaultValue: true, Helpers.CreateString("toggle-desc3", "Inheritor's Crusader"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("lionblade"), defaultValue: true, Helpers.CreateString("toggle-desc81", "Lion Blade"))
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
            Toggle.New(GetKey("dancer"), defaultValue: true, Helpers.CreateString("toggle-desc4", "Shadowdancer"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("drinker"), defaultValue: true, Helpers.CreateString("toggle-desc74", "Souldrinker"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("umbral"), defaultValue: true, Helpers.CreateString("toggle-desc17", "Umbral Agent"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("construct"), defaultValue: true, Helpers.CreateString("toggle-desc49", "Construct Rider"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("internal"), defaultValue: true, Helpers.CreateString("toggle-desc61", "Internal Alchemist"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("scientist"), defaultValue: true, Helpers.CreateString("toggle-desc50", "Mad Scientist"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("skymagic"), defaultValue: true, Helpers.CreateString("toggle-desc65", "Aeromancer"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("drunk"), defaultValue: true, Helpers.CreateString("toggle-desc76", "Drunken Brute"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("geisha"), defaultValue: true, Helpers.CreateString("toggle-desc90", "Geisha"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("juggle"), defaultValue: true, Helpers.CreateString("toggle-desc64", "Juggler"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("soundstriker"), defaultValue: true, Helpers.CreateString("toggle-desc91", "Sound Striker"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("conduit"), defaultValue: true, Helpers.CreateString("toggle-desc63", "Blood Conduit"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("constable"), defaultValue: true, Helpers.CreateString("toggle-desc70", "Constable"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("emissary"), defaultValue: true, Helpers.CreateString("toggle-desc71", "Emissary"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("menhir"), defaultValue: true, Helpers.CreateString("toggle-desc72", "Menhir Savant"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("toxic"), defaultValue: true, Helpers.CreateString("toggle-desc78", "Toxicologist"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("fighterdd"), defaultValue: true, Helpers.CreateString("toggle-desc84", "Dawnflower Dervish"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("fightereg"), defaultValue: true, Helpers.CreateString("toggle-desc23", "Eldritch Guardian"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("lorew"), defaultValue: true, Helpers.CreateString("toggle-desc24", "Lore Warden"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("warlord"), defaultValue: true, Helpers.CreateString("toggle-desc25", "Warlord"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("fighteraa"), defaultValue: true, Helpers.CreateString("toggle-desc85", "Weapon Master"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("seeker"), defaultValue: true, Helpers.CreateString("toggle-desc58", "Truth Seeker"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("spire"), defaultValue: true, Helpers.CreateString("toggle-desc33", "Spire Defender"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("mm"), defaultValue: true, Helpers.CreateString("toggle-desc73", "Maneuver Master"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("mms"), defaultValue: true, Helpers.CreateString("toggle-desc83", "Master of Many Styles"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("wadept"), defaultValue: true, Helpers.CreateString("toggle-desc77", "Weapon Adept"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("shining"), defaultValue: true, Helpers.CreateString("toggle-desc62", "Shining Knight"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("marksman"), defaultValue: true, Helpers.CreateString("toggle-desc47", "Divine Marksman"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("infil"), defaultValue: true, Helpers.CreateString("toggle-desc57", "Infiltrator"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("witchguard"), defaultValue: true, Helpers.CreateString("toggle-desc48", "Witchguard"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("kidnapper"), defaultValue: true, Helpers.CreateString("toggle-desc67", "Kidnapper"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("ankou"), defaultValue: true, Helpers.CreateString("toggle-desc92", "Ankou's Shadow"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("fated"), defaultValue: true, Helpers.CreateString("toggle-desc79", "Fated Champion"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("divinechamp"), defaultValue: true, Helpers.CreateString("toggle-desc66", "Divine Champion"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("cardplayer"), defaultValue: true, Helpers.CreateString("toggle-desc75", "Cartomancer"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("whitehair"), defaultValue: true, Helpers.CreateString("toggle-desc32", "White-Haired Witch"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("ragonury"), defaultValue: true, Helpers.CreateString("toggle-desc26", "Dragon Fury (Path of War)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("seraph"), defaultValue: true, Helpers.CreateString("toggle-desc11", "Black Seraph Style (Path of War)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("cloak"), defaultValue: true, Helpers.CreateString("toggle-desc53", "Cloak and Dagger Style"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("stag"), defaultValue: true, Helpers.CreateString("toggle-desc14", "Charging Stag Style"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("jabbing"), defaultValue: true, Helpers.CreateString("toggle-desc40", "Jabbing Style"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("grabbing"), defaultValue: true, Helpers.CreateString("toggle-desc13", "Grabbing Style"))
              .ShowVisualConnection())
           .AddToggle(
            Toggle.New(GetKey("kraken"), defaultValue: true, Helpers.CreateString("toggle-desc16", "Kraken Style"))
              .ShowVisualConnection())
           .AddToggle(
            Toggle.New(GetKey("panther"), defaultValue: true, Helpers.CreateString("toggle-desc41", "Panther Style"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("turtle"), defaultValue: true, Helpers.CreateString("toggle-desc15", "Snapping Turtle Style"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("enhance"), defaultValue: true, Helpers.CreateString("toggle-desc22", "Mythic Prestige Class Features"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("rogue"), defaultValue: true, Helpers.CreateString("toggle-desc30", "Rogue Talents"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("atheism"), defaultValue: true, Helpers.CreateString("toggle-desc29", "Atheism Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("bodyguard"), defaultValue: true, Helpers.CreateString("toggle-desc28", "Bodyguard Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("obedience"), defaultValue: true, Helpers.CreateString("toggle-desc55", "Deific Obedience"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("guided"), defaultValue: true, Helpers.CreateString("toggle-desc60", "Guided Hand"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("sentinel"), defaultValue: true, Helpers.CreateString("toggle-desc59", "Sentinel"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("exalted"), defaultValue: true, Helpers.CreateString("toggle-desc69", "Exalted Evangelist"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("mythicheritage"), defaultValue: true, Helpers.CreateString("toggle-desc86", "Racial Heritage (Mythic)"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("magehand"), defaultValue: true, Helpers.CreateString("toggle-desc39", "Magic Trick Feats"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("spell"), defaultValue: true, Helpers.CreateString("toggle-desc54", "New Spells"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("teleport"), defaultValue: true, Helpers.CreateString("toggle-desc80", "Better Teleport Stuff"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("nerfsop"), defaultValue: false, Helpers.CreateString("toggle-desc38", "(Nerf) Seize the Opportunity as a Mythic Feat"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("cmdminus"), defaultValue: true, Helpers.CreateString("toggle-desc51", "Fix Difficulty CMD Tripple-Dipped"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("ecpatch"), defaultValue: true, Helpers.CreateString("toggle-desc89", "Ensure Compability with Expanded Content"))
              .ShowVisualConnection())
          .AddToggle(
            Toggle.New(GetKey("duplicate"), defaultValue: false, Helpers.CreateString("toggle-desc46", "Remove Duplicate Feats from Other MODs"))
              .ShowVisualConnection()));

                try
        {
          if (Initialized)
          {
            Logger.Info("Already configured blueprints.");
            return;
          }
          Initialized = true;

                    Logger.Info("Configuring blueprints!"); 
                    SpellbookReplace.Select(); SpellbookLevelUp.Select(); ImbueArrow.Configure(); ShadowDancer.CreateProficiencies(); ShadowDancer.ExtraShadowJump(); ShadowDancer.ExtraFeat(); SeizetheOpportunity.Configure(); AerialAssault.ConfigureAbility(); FakeAlignedClass.Configure(); BodyGuard.StyleMasterConfigure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("grapple"))) { ImprovedGrapple.StyleConfigure(); ImprovedGrappleMythic.Configure(); GreaterGrapple.Configure(); ManeuverExpert.Configure(); RapidGrappler.Configure(); UnfairGrip.Configure(); PinningKnockout.Configure(); PinningRend.Configure(); SavageSlam.Configure(); UncannyGrapple.Configure(); HamatulaStrike.Configure(); DramaticSlam.Configure(); KnotExpert.Configure(); AerialAssault.Configure(); MeatShield.Configure(); ThroatSlicer.Configure(); CrabKing.Configure(); CrabKing.Configure2(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("grave"))) AgentoftheGrave.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("archer"))) { ArcaneArcher.Configure(); StormArrow.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("archer")) && ModMenu.ModMenu.GetSettingValue<bool>(GetKey("devotee"))) DeadeyeDevotee.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("chevalier"))) Chevalier.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("crusader"))) InheritorCrusader.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("lander"))) Hinterlander.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("walker"))) HorizonWalker.Configure();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("angel"))) { SanguineAngel.Configure(); BullRushFeats.DragConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("scar"))) { ScarSeeker.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("dancer"))) { ShadowDancer.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("seraph"))) { BlackSeraphStyle.StyleConfigure(); BlackSeraphStyle.MalevolenceConfigure(); BlackSeraphStyle.AnnihilationConfigure(); RippleWater.Configure(); SeizetheOpportunity.FeatConfigure(); BodyGuard.GreaterNameConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("grabbing"))) { GrabbingStyle.StyleConfigure(); GrabbingStyle.DragConfigure(); GrabbingStyle.MasterConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("stag"))) { StagStyle.StyleConfigure(); StagStyle.HornsConfigure(); StagStyle.SubmissionConfigure(); PlainsDruid.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("turtle"))) { SnappingTurtleStyle.StyleConfigure(); SnappingTurtleStyle.ClutchConfigure(); SnappingTurtleStyle.ShellConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("kraken"))) { KrakenStyle.StyleConfigure(); KrakenStyle.WrackConfigure(); KrakenStyle.WrathConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("umbral"))) { UmbralAgent.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("biggg"))) { MammothRider.Configure(); GiganticAssault.Configure(); FungalPilgrim.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("asavir")) || ModMenu.ModMenu.GetSettingValue<bool>(GetKey("ecpatch"))) { Asavir.Configure(); BondGenie.Configure(); TrampleMythic.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("maneuver"))) { ReplaceAttack.BullRushQuickConfigure(); ReplaceAttack.BullRushAngryConfigure(); ReplaceAttack.BullRushConfigure(); ReplaceAttack.DisarmConfigure(); ReplaceAttack.SunderConfigure(); ReplaceAttack.TripConfigure(); ReplaceAttack.DirtyBlindQuickConfigure(); ReplaceAttack.DirtyEntangleQuickConfigure(); ReplaceAttack.DirtySickenQuickConfigure();
                        StrengthSurge.StrengthSurgeFeat(); KiThrow.DragConfigure(); ImprovedKiThrow.DragConfigure(); EnhancedThrow.DragConfigure(); BindingThrow.DragConfigure(); 
                        RhinoCharge.Configure();  SunderStorm.CreateCombatTrickery(); DirtyFighting.DirtyFightingConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("halfling")) || ModMenu.ModMenu.GetSettingValue<bool>(GetKey("ecpatch"))) { HalflingOpportunist.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("enhance"))) { SwiftDeath.Configure(); SwiftDeath.Configure2(); UnbreakableDefance.Configure(); MythicWings.Configure(); MythicWings.Configure2(); MythicRiposte.Configure(); MetaphysicalSneak.Configure(); LesserSpellSynthesis.Configure(); SurprisingStrategy.Configure(); MythicRiposte.Configure2(); SwordLord.CreateQuickDraw(); SwordLord.CreateCatechesis(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("fightereg"))) { EldritchGuardian.Configure(); EldritchAssault.EldritchAssaultFeatFeat(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("lorew"))) { LoreWarden.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("warlord"))) { Warlord.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("ragonury"))) { DragonFury.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("dawnflower"))) { AnchoriteofDawn.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("bodyguard"))) { BodyGuard.Configure(); BodyGuard.Configure2(); BodyGuard.NinjaTrickConfigure(); HighGuardian.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("atheism"))) { DivineDefiance.StyleConfigure(); DivineDefiance.IconoclastConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("rogue"))) { QuickShot.Configure(); CloyingShades.CloyingShadesFeat(); DefensiveRoll.Configure(); SneakyManeuver.SneakyManeuverFeat(); SurpriseManeuver.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("kineticknight"))) { EsotericKnight.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("whitehair"))) { WhiteHairedWitch.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("spire"))) { SpireDefender.Configure(); SpireDefender.CreateReachSpellstrike(); SpireDefender.CreateCloseRange(); SpireDefender.CreateDisruptive(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("autoheal"))) { AutoHeal.StyleConfigure(); } else { AutoHeal.DisableConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("furious"))) { FuriousGuardian.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("magehand"))) { MageHandTrick.ConfigureMageHandMain(); MageHandTrick.ConfigureShieldMain(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("jabbing"))) { JabbingStyle.StyleConfigure(); JabbingStyle.DancerConfigure(); JabbingStyle.MasterConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("panther"))) { PantherStyle.StyleConfigure(); PantherStyle.ClawConfigure(); PantherStyle.ParryConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("disarm"))) { RangedDisarm.AceDisarmFeature(); RangedDisarm.ArmBindFeature(); RangedDisarm.BreakGuardFeature(); RangedDisarm.CreateMythicDisarm(); RangedDisarm.CreateRangeDisarm(); RangedDisarm.FollowUpStrikeFeature(); RangedDisarm.StrikeSeizeFeature(); RangedDisarm.SweepingDisarmFeature(); RangedDisarm.WristGrabFeature(); RangedDisarm.HoldtheBladeFeature(); ShieldSnag.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("bullrush"))) { BullRushFeats.Configure(); BullRushFeats.Configure2(); BullRushFeats.StyleConfigure(); BullRushFeats.FollowConfigure(); BullRushFeats.AngelConfigure(); BullRushFeats.MonsterConfigure(); StrengthSurge.RagingThrowFeat(); ShieldSlam.Configure(); SunderStorm.CreateDriveBack(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("sunder"))) { SmashingStyle.StyleConfigure(); SmashingStyle.CounterConfigure(); SmashingStyle.MasterConfigure(); CrushArmor.Configure(); SunderStorm.CreateSunderStorm(); GreaterSunderTabletop.CreateGreaterSunderTabletop(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("trip"))) { DownLikeDominoes.DragConfigure(); SunderStorm.CreateCleavingSweep(); RangedTrip.CreateTripRanged(); RangedTrip.AceTripFeature(); RangedTrip.CreateVindictiveFall(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("marksman"))) { DivineMarksman.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("witchguard"))) { Witchguard.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("construct"))) { ConstructRider.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("scientist"))) { MadScientist.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("cmdminus"))) { CMDPatch.Patch(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("dirty"))) { DirtyFeats.Configure(); DirtyFeats.StyleConfigure(); DirtyFeats.LadderConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("cloak"))) { CloakDaggerStyle.StyleConfigure(); CloakDaggerStyle.SubterfugeConfigure(); CloakDaggerStyle.TacticsConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("spell"))) { LitanyRighteousness.Configure(); ShieldOther.Configure(); InheritorSmite.Configure(); RovagugFury.Configure(); HydraulicTorrent.Configure(); ThunderstompGreater.Configure(); Thunderstomp.Configure(); TelekineticManeuver.Configure(); Linebreaker.Configure(); BladeLash.Configure(); PilferingHand.Configure(); ResinousSkin.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("obedience"))) { DeificObedience.DeificObedienceConfigure(); DiverseObedience.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("templar"))) { CrimsonTemplar.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("seeker"))) { TruthSeeker.Configure(); TruthSooker.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("infil"))) { Infiltrator.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("sentinel"))) { Sentinel.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("guided"))) { GuidedHand.GuidedHandConfigure(); GuidedHand.GuidedHandMythicFeat(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("internal"))) { InternalAlchemist.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("shining"))) { ShiningKnight.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("conduit"))) { BloodConduit.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("kidnapper"))) { Kidnapper.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("juggle"))) { Juggler.Configure(); Juggler.CreateDeflectArrowsMythic(); BodyGuard.KatateDoriFeat(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("divinechamp"))) { DivineChampion.Configure(); DivineParagon.Configure(); Inquisition.SpellkillerFeat(); Inquisition.ReformationFeat(); Inquisition.TacticsFeat(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("skymagic"))) { Aeromancer.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("exalted"))) { ExaltedEvangelist.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("constable"))) { Constable.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("emissary"))) { Emissary.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("menhir"))) { MenhirSavant.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("mm"))) { ManeuverMaster.Configure(); ManeuverMaster.FreedomFeat(); ManeuverMaster.OneTouchFeat(); ManeuverMaster.FreedomFeat2(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("drinker"))) { Souldrinker.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("cardplayer"))) { Cartomancer.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("drunk"))) { DrunkenBrute.Configure(); DrunkenBrute.AtavismFeat(); DrunkenBrute.GreaterAtavismFeat(); DrunkenBrute.CreateGlare(); DrunkenBrute.RoaringDrunkFeat(); DrunkenBrute.LiquidCourageFeat(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("wadept"))) { WeaponAdept.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("toxic"))) { Toxicologist.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("fated"))) { FatedChampion.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("lionblade"))) { LionBlade.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("bitch"))) { EnchantingCourtesan.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("mms"))) { ManyMonk.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("fighterdd"))) { DawnflowerDervish.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("fighteraa"))) { WeaponMaster.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("justshowup"))) { HeritorKnight.Configure(); StrikeTrue.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("mythicheritage"))) { RacialHeritage.RacialHeritageConfigure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("goldenl")) || ModMenu.ModMenu.GetSettingValue<bool>(GetKey("ecpatch"))) { GoldenLegionnaire.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("geisha"))) { Geisha.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("soundstriker"))) { SoundStriker.Configure(); }
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("ankou"))) { }
                    HolyVindicator.DivineWrathFeat(); HolyVindicator.DivineJudgmentFeat(); HolyVindicator.DivineRetributionFeat(); GiganticAssault.Configure2();
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
            Initialized = true; PatchDomain.Patch(); PatchArmorTraining.Patch(); PatchSkaldSharing.Patch(); ImprovedWalk.Patch(); PatchRagingSong.Patch();
            GraveSpellList.CreateSecretDeath(); GraveSpellList.CreateMiracleList(); GraveSpellList.CreateMiracleList3();
            RootConfigurator.ConfigureDelayedBlueprints();
        }
        catch (Exception e)
        {
          Logger.Error("Failed to configure delayed blueprints.", e);
        }
      }
    }

        [HarmonyPatch(typeof(StartGameLoader))]
        static class StartGameLoader_Patch2
        {
            private static bool Initialized = false;

            [HarmonyAfter(new string[] { "HolyVindicator" })]
            [HarmonyPatch(nameof(StartGameLoader.LoadAllJson)), HarmonyPostfix]
            static void LoadAllJson()
            {
                try
                {
                    if (Initialized)
                    {
                        Logger.Info("Already configured Holy Vindicator.");
                        return;
                    }
                    Initialized = true; PatchHolyVindicator.Patch(); PrerequisitePatch.Patch(); StyleMasterPatch.Patch();
                    if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("duplicate"))) { RemoveFeats.Patch(); }
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to configure Holy Vindicator.", e);
                }
            }
        }
    }
}
