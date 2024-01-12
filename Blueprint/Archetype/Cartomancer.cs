using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using static TabletopTweaks.Core.NewUnitParts.CustomStatTypes;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Cartomancer
    {
        private const string ArchetypeName = "Cartomancer";
        private static readonly string ArchetypeGuid = "{126D9D91-2514-4D09-BFBA-1D9C918957F8}";
        internal const string ArchetypeDisplayName = "Cartomancer.Name";
        private const string ArchetypeDescription = "Cartomancer.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.WitchClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.WitchFamiliarSelection.ToString())
            .SetRemoveFeaturesEntry(2, FeatureSelectionRefs.WitchHexSelection.ToString())
            .AddToAddFeatures(2, FeatureRefs.ArcaneStrikeFeature.ToString())
            .AddToAddFeatures(3, CreateDeadlyDealer())
              .Configure();

        }

        private const string DeadlyDealer = "Cartomancer.DeadlyDealer";
        public static readonly string DeadlyDealerGuid = "{FB043A7C-269E-4E0D-A64A-F09F0A3438CA}";

        private const string DeadlyDealerBuff = "Cartomancer.DeadlyDealerBuff";
        public static readonly string DeadlyDealerBuffGuid = "{B6588D8E-28C6-4AC9-A146-EE86BA5909D9}";

        internal const string DeadlyDealerDisplayName = "CartomancerDeadlyDealer.Name";
        private const string DeadlyDealerDescription = "CartomancerDeadlyDealer.Description";

        public static BlueprintFeature CreateDeadlyDealer()
        {
            var icon = AbilityRefs.PoisonedDart.Reference.Get().Icon;

            var buff = BuffConfigurator.New(DeadlyDealerBuff, DeadlyDealerBuffGuid)
              .SetDisplayName(DeadlyDealerDisplayName)
              .SetDescription(DeadlyDealerDescription)
              .SetIcon(icon)
              .AddComponent<AddStatBonus>(c =>
              {
                  c.Stat = CustomStatType.MeleeTouchReach.Stat();
                  c.Value = 15;
                  c.Descriptor = ModifierDescriptor.Feat;
              })
              .AddAttackTypeChange(false, false, AttackType.RangedTouch, AttackType.Touch)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            return FeatureConfigurator.New(DeadlyDealer, DeadlyDealerGuid)
              .SetDisplayName(DeadlyDealerDisplayName)
              .SetDescription(DeadlyDealerDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.ArcaneStrikeBuff.ToString(), extraEffectBuff: buff)
              .Configure();
        }
    }
}
