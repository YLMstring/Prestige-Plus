using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Utils;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class WeaponAdept
    {
        private const string ArchetypeName = "WeaponAdept";
        private static readonly string ArchetypeGuid = "{67216A5C-4153-44FB-9547-4DE79C57D0C4}";
        internal const string ArchetypeDisplayName = "WeaponAdept.Name";
        private const string ArchetypeDescription = "WeaponAdept.Description";

        //"PerfectStrikeZenArcherUpgrade": "4ac24d1f-7398-488c-9482-493db1c0863a",
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.MonkClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            //.SetRemoveFeaturesEntry(4, FeatureRefs.StunningFistFatigueFeature.ToString())
            //.SetRemoveFeaturesEntry(8, FeatureRefs.StunningFistSickenedFeature.ToString())
            .SetRemoveFeaturesEntry(1, FeatureRefs.StunningFist.ToString())
            .SetRemoveFeaturesEntry(2, FeatureRefs.Evasion.ToString())
            .SetRemoveFeaturesEntry(5, FeatureSelectionRefs.MonkStyleStrike.ToString())
            .SetRemoveFeaturesEntry(9, FeatureRefs.ImprovedEvasion.ToString(), FeatureSelectionRefs.MonkStyleStrike.ToString())
            .SetRemoveFeaturesEntry(13, FeatureSelectionRefs.MonkStyleStrike.ToString())
            .SetRemoveFeaturesEntry(17, FeatureSelectionRefs.MonkStyleStrike.ToString())
            .SetRemoveFeaturesEntry(20, FeatureRefs.KiPerfectSelfFeature.ToString())
            .AddToAddFeatures(1, FeatureRefs.PerfectStrikeFeature.ToString())
            .AddToAddFeatures(2, CreateFocus())
            .AddToAddFeatures(6, CreateFocus2())
            .AddToAddFeatures(9, FeatureRefs.Evasion.ToString())
            .AddToAddFeatures(10, "4ac24d1f-7398-488c-9482-493db1c0863a")
            .AddToAddFeatures(17, UncannyInitiativeFeat())
            .AddToAddFeatures(20, PurePowerFeat())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.MonkProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.PerfectStrikeFeature.ToString(), FocusGuid, Focus2Guid, "4ac24d1f-7398-488c-9482-493db1c0863a", UncannyInitiativeGuid, PurePowerGuid })
                .Configure();
        }

        private const string Focus = "WeaponAdept.Focus";
        private static readonly string FocusGuid = "{B200D27E-96C7-4A17-87C5-2D2AFB26BF9F}";

        internal const string FocusDisplayName = "WeaponAdeptFocus.Name";
        private const string FocusDescription = "WeaponAdeptFocus.Description";

        private static BlueprintFeature CreateFocus()
        {
            var icon = ParametrizedFeatureRefs.WeaponFocus.Reference.Get().Icon;

            var focus = ParametrizedFeatureRefs.WeaponFocus.Reference.Get().ToReference<BlueprintParametrizedFeatureReference>();
            var kama = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus,
                ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Kama
            };
            var nunchaku = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus,
                ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Nunchaku
            };
            var sai = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus,
                ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Sai
            };
            var siangham = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus,
                ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Siangham
            };

            return FeatureConfigurator.New(Focus, FocusGuid)
              .SetDisplayName(FocusDisplayName)
              .SetDescription(FocusDescription)
              .SetIcon(icon)
              .AddParametrizedFeatures(new AddParametrizedFeatures.FeatureData[] { kama, nunchaku, sai, siangham })
              .Configure();
        }

        private const string Focus2 = "WeaponAdept.Focus2";
        private static readonly string Focus2Guid = "{197B475E-C182-4675-98CE-2592F7E42F63}";

        internal const string Focus2DisplayName = "WeaponAdeptFocus2.Name";
        private const string Focus2Description = "WeaponAdeptFocus2.Description";

        private static BlueprintFeature CreateFocus2()
        {
            var icon = ParametrizedFeatureRefs.WeaponSpecialization.Reference.Get().Icon;

            var focus2 = ParametrizedFeatureRefs.WeaponSpecialization.Reference.Get().ToReference<BlueprintParametrizedFeatureReference>();
            var kama = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus2,
                ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Kama
            };
            var nunchaku = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus2,
                ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Nunchaku
            };
            var sai = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus2,
                ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Sai
            };
            var siangham = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus2,
                ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Siangham
            };

            return FeatureConfigurator.New(Focus2, Focus2Guid)
              .SetDisplayName(Focus2DisplayName)
              .SetDescription(Focus2Description)
              .SetIcon(icon)
              .AddParametrizedFeatures(new AddParametrizedFeatures.FeatureData[] { kama, nunchaku, sai, siangham })
              .Configure();
        }

        private const string UncannyInitiative = "WeaponAdept.UncannyInitiative";
        public static readonly string UncannyInitiativeGuid = "{E660A1FB-9385-4277-83A7-1586D0C8E26B}";

        internal const string UncannyInitiativeDisplayName = "WeaponAdeptUncannyInitiative.Name";
        private const string UncannyInitiativeDescription = "WeaponAdeptUncannyInitiative.Description";

        public static BlueprintFeature UncannyInitiativeFeat()
        {
            var icon = AbilityRefs.WalkThroughSpace.Reference.Get().Icon;

            return FeatureConfigurator.New(UncannyInitiative, UncannyInitiativeGuid)
              .SetDisplayName(UncannyInitiativeDisplayName)
              .SetDescription(UncannyInitiativeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddModifyD20(replace: true, rollResult: 20, rule: Kingmaker.Designers.Mechanics.Facts.RuleType.Initiative)
              .Configure();
        }

        private const string PurePower = "WeaponAdept.PurePower";
        public static readonly string PurePowerGuid = "{B1C053F5-1AAA-476F-918A-F86F9C51A573}";

        internal const string PurePowerDisplayName = "WeaponAdeptPurePower.Name";
        private const string PurePowerDescription = "WeaponAdeptPurePower.Description";

        public static BlueprintFeature PurePowerFeat()
        {
            var icon = AbilityRefs.RestorationGreater.Reference.Get().Icon;

            return FeatureConfigurator.New(PurePower, PurePowerGuid)
              .SetDisplayName(PurePowerDisplayName)
              .SetDescription(PurePowerDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddStatBonus(stat: StatType.Strength, value: 3)
              .AddStatBonus(stat: StatType.Dexterity, value: 3)
              .AddStatBonus(stat: StatType.Constitution, value: 3)
              .AddStatBonus(stat: StatType.Intelligence, value: 3)
              .AddStatBonus(stat: StatType.Wisdom, value: 3)
              .AddStatBonus(stat: StatType.Charisma, value: 3)
              .Configure();
        }
    }
}
