using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;

namespace PrestigePlus.Patch
{
    internal class PatchHolyVindicator
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void Patch()
        {
            string holyvindicator = "7162d325539a4c66a163fe49c42f279a";
            AddFeatureToPro(SpellbookReplace.spellupgradeGuid, holyvindicator, 1);
            RemoveFeatureFromPro("7d94808783e742e8bb8dc4d82e30ca0e", holyvindicator, 2);
            AddFeatureToPro(HolyVindicator.DivineWrathGuid, holyvindicator, 4);
            AddFeatureToPro(HolyVindicator.DivineJudgmentGuid, holyvindicator, 7);
            AddFeatureToPro(HolyVindicator.DivineRetributionGuid, holyvindicator, 10);
            string swiftaid = "8590fb52-921c-4365-832c-ca7635fd5a70";
            AddFeatureToPro(swiftaid, GoldenLegionnaire.ClassProgressGuid, 8);
            //"BullseyeShotFeature": "31fbe31a-7727-46d6-b10a-86602cb1b941",
            //"PinpointTargetingFeature": "583da9f2-3981-471a-bd45-6ccc2be0b3a6",
            AddFeatureToArc("31fbe31a-7727-46d6-b10a-86602cb1b941", DivineMarksman.ArchetypeGuid, 1);
            AddFeatureToArc("583da9f2-3981-471a-bd45-6ccc2be0b3a6", DivineMarksman.ArchetypeGuid, 11);
            //"TrickRiding": "34d1bd97-971d-44d6-be78-e91e79fdedbd",
            //"MountedSkirmisher": "d3886adc-c849-4733-a1cb-a9b787b49576",
            AddFeatureToArc("34d1bd97-971d-44d6-be78-e91e79fdedbd", Emissary.ArchetypeGuid, 9);
            AddFeatureToArc("d3886adc-c849-4733-a1cb-a9b787b49576", Emissary.ArchetypeGuid, 14);

            var comp = Raz.Get()?.GetComponent<SpellTurning>();
            if (comp != null)
            {
                comp.m_SpellResistanceOnly = true;
            }
            var comp2 = BuffRefs.PortalStoneSpiderRingEffectBuff.Reference.Get()?.GetComponent<AddInitiatorAttackWithWeaponTrigger>();
            if (comp2 != null)
            {
                comp2.CriticalHit = true;
            }
        }

        private static BlueprintFeatureReference Raz = BlueprintTool.GetRef<BlueprintFeatureReference>("13d5818737694021b001641437a4ba29");
        private static void AddFeatureToPro(string featguid, string proguid, int level)
        {
            var progress = BlueprintTool.GetRef<BlueprintProgressionReference>(proguid)?.Get();
            if (progress == null) { Logger.Info("not found pro " + proguid); return; }
            var feat = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(featguid);
            if (feat?.Get() == null) { Logger.Info("not found feat " + featguid); return; }
            progress.LevelEntries
                        .Where(entry => entry.Level == level)
                        .ForEach(entry =>
                        {
                            entry.m_Features.Add(feat);
                        });
        }

        private static void RemoveFeatureFromPro(string featguid, string proguid, int level)
        {
            var progress = BlueprintTool.GetRef<BlueprintProgressionReference>(proguid)?.Get();
            if (progress == null) { Logger.Info("not found pro " + proguid); return; }
            var feat = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(featguid);
            if (feat?.Get() == null) { Logger.Info("not found feat " + featguid); return; }
            progress.LevelEntries
                        .Where(entry => entry.Level == level)
                        .ForEach(entry =>
                        {
                            entry.m_Features.Remove(feat);
                        });
        }

        private static void AddFeatureToArc(string featguid, string proguid, int level)
        {
            var progress = BlueprintTool.GetRef<BlueprintArchetypeReference>(proguid)?.Get();
            if (progress == null) { Logger.Info("not found pro " + proguid); return; }
            var feat = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(featguid);
            if (feat?.Get() == null) { Logger.Info("not found feat " + featguid); return; }
            progress.AddFeatures
                        .Where(entry => entry.Level == level)
                        .ForEach(entry =>
                        {
                            entry.m_Features.Add(feat);
                        });
        }
    }
}
