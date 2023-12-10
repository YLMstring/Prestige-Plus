using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent.Archetype;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class ShiningKnight
    {
        private const string ArchetypeName = "ShiningKnight";
        private static readonly string ArchetypeGuid = "{702A90E7-FCAD-4F2C-AA41-9B4566966DBD}";
        internal const string ArchetypeDisplayName = "ShiningKnight.Name";
        private const string ArchetypeDescription = "ShiningKnight.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.PaladinClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(3, FeatureRefs.DivineHealth.ToString())
            .AddToRemoveFeatures(5, FeatureSelectionRefs.PaladinDivineBondSelection.ToString())
            .AddToRemoveFeatures(11, FeatureRefs.AuraOfJusticeFeature.ToString())
            .AddToAddFeatures(3, CreateSkilledRider())
            .AddToAddFeatures(5, FeatureSelectionRefs.PaladinDivineMountSelection.ToString())
            .AddToAddFeatures(6, KnightChargeFeat())
              .Configure();
        }

        private const string SkilledRider = "ShiningKnight.SkilledRider";
        private static readonly string SkilledRiderGuid = "{9F3CB2BE-6774-44FE-9025-6C3E00DB5F25}";

        private const string SkilledRider2 = "ShiningKnight.SkilledRider2";
        private static readonly string SkilledRider2Guid = "{D63561FF-92EE-46FD-9CEB-EBD5CACA03EB}";

        internal const string SkilledRiderDisplayName = "ShiningKnightSkilledRider.Name";
        private const string SkilledRiderDescription = "ShiningKnightSkilledRider.Description";
        private static BlueprintFeature CreateSkilledRider()
        {
            var icon = FeatureRefs.MountedCombat.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(SkilledRider2, SkilledRider2Guid)
              .SetDisplayName(SkilledRiderDisplayName)
              .SetDescription(SkilledRiderDescription)
              .SetIcon(icon)
              .AddComponent<SkilledRiderSave>()
              .Configure();

            return FeatureConfigurator.New(SkilledRider, SkilledRiderGuid)
              .SetDisplayName(SkilledRiderDisplayName)
              .SetDescription(SkilledRiderDescription)
              .SetIcon(icon)
              .AddFacts(new() { "272aa4cc-a738-4a69-92da-395f4fae3d22" }) ///ttt
              .AddFeatureToPet(feat, PetType.AnimalCompanion)
              .Configure();
        }

        private const string KnightCharge = "ShiningKnight.KnightCharge";
        private static readonly string KnightChargeGuid = "{79076F84-E2FA-4186-AF17-DF749AC5DEA1}";

        private const string KnightCharge2 = "ShiningKnight.KnightCharge2";
        private static readonly string KnightCharge2Guid = "{C72400A1-3A83-4226-8189-48C102AB4FE6}";

        private const string KnightChargeBuff = "ShiningKnight.KnightChargeBuff";
        private static readonly string KnightChargeBuffGuid = "{39771039-4091-4338-B57D-C3AA26110A4A}";

        private const string KnightChargeBuff2 = "ShiningKnight.KnightChargeBuff2";
        private static readonly string KnightChargeBuff2Guid = "{B9206FF3-F498-428B-8383-A13F2056CF9B}";

        internal const string KnightChargeDisplayName = "ShiningKnightKnightCharge.Name";
        private const string KnightChargeDescription = "ShiningKnightKnightCharge.Description";
        public static BlueprintFeature KnightChargeFeat()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            var buff = BuffConfigurator.New(KnightChargeBuff, KnightChargeBuffGuid)
              .SetDisplayName(KnightChargeDisplayName)
              .SetDescription(KnightChargeDescription)
              .SetIcon(icon)
              .AddCondition(UnitCondition.ImmuneToAttackOfOpportunity)
              .AddComponent<ShiningKnightCharge>()
              //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var buff2 = BuffConfigurator.New(KnightChargeBuff2, KnightChargeBuff2Guid)
              .SetDisplayName(KnightChargeDisplayName)
              .SetDescription(KnightChargeDescription)
              .SetIcon(icon)
              .AddCondition(UnitCondition.ImmuneToAttackOfOpportunity)
              //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var feat = FeatureConfigurator.New(KnightCharge2, KnightCharge2Guid)
              .SetDisplayName(KnightChargeDisplayName)
              .SetDescription(KnightChargeDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(BuffRefs.ChargeBuff.ToString(), extraEffectBuff: buff2)
              .Configure();

            return FeatureConfigurator.New(KnightCharge, KnightChargeGuid)
              .SetDisplayName(KnightChargeDisplayName)
              .SetDescription(KnightChargeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat, PetType.AnimalCompanion)
              .AddBuffExtraEffects(BuffRefs.ChargeBuff.ToString(), extraEffectBuff: buff)
              .Configure();
        }
    }
}
