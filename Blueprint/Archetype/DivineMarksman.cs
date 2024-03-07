using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class DivineMarksman
    {
        private const string ArchetypeName = "DivineMarksman";
        public static readonly string ArchetypeGuid = "{D7099593-D7FB-4A91-AAAE-C6A9C1F26AD0}";
        internal const string ArchetypeDisplayName = "DivineMarksman.Name";
        private const string ArchetypeDescription = "DivineMarksman.Description";
        public static void Configure()
        {
            //"BullseyeShotFeature": "31fbe31a-7727-46d6-b10a-86602cb1b941",
            //"PinpointTargetingFeature": "583da9f2-3981-471a-bd45-6ccc2be0b3a6",
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.RangerClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(2, FeatureSelectionRefs.RangerStyleSelection2.ToString())
            .AddToRemoveFeatures(6, FeatureSelectionRefs.RangerStyleSelection6.ToString())
            .AddToRemoveFeatures(10, FeatureSelectionRefs.RangerStyleSelection10.ToString())
            .AddToRemoveFeatures(11, FeatureRefs.Quarry.ToString())
            .AddToRemoveFeatures(14, FeatureSelectionRefs.RangerStyleSelection10.ToString())
            .AddToRemoveFeatures(18, FeatureSelectionRefs.RangerStyleSelection10.ToString())
            .AddToRemoveFeatures(19, FeatureRefs.ImprovedQuarry.ToString())
            .SetRemoveSpellbook()
            .AddToAddFeatures(1)
            .AddToAddFeatures(2, FeatureSelectionRefs.RangerStyleArcherySelection2.ToString())
            .AddToAddFeatures(4, CreateViciousAim())
            .AddToAddFeatures(6, FeatureSelectionRefs.RangerStyleArcherySelection6.ToString())
            .AddToAddFeatures(8, CreateIomedaeInfluence())
            .AddToAddFeatures(10, FeatureSelectionRefs.RangerStyleArcherySelection10.ToString())
            .AddToAddFeatures(11)
            .AddToAddFeatures(14, FeatureSelectionRefs.RangerStyleArcherySelection10.ToString())
            .AddToAddFeatures(18, FeatureSelectionRefs.RangerStyleArcherySelection10.ToString())
            .AddToAddFeatures(19, FeatureRefs.Quarry.ToString())
              .Configure(delayed: true);
        }

        private const string ViciousAim = "DivineMarksman.ViciousAim";
        private static readonly string ViciousAimGuid = "{A37E3FB8-91A1-4B5D-9669-06632A823815}";

        internal const string ViciousAimDisplayName = "DivineMarksmanViciousAim.Name";
        private const string ViciousAimDescription = "DivineMarksmanViciousAim.Description";
        private static BlueprintFeature CreateViciousAim()
        {
            var icon = FeatureSelectionRefs.FavoriteEnemySelection.Reference.Get().Icon;

            return FeatureConfigurator.New(ViciousAim, ViciousAimGuid)
              .SetDisplayName(ViciousAimDisplayName)
              .SetDescription(ViciousAimDescription)
              .SetIcon(icon)
              .AddComponent<ViciousAimComp>()
              .Configure();
        }

        private const string IomedaeInfluence = "DivineMarksman.IomedaeInfluence";
        private static readonly string IomedaeInfluenceGuid = "{C14062D8-4EAC-4C61-8A96-8A340748657C}";

        internal const string IomedaeInfluenceDisplayName = "DivineMarksmanIomedaeInfluence.Name";
        private const string IomedaeInfluenceDescription = "DivineMarksmanIomedaeInfluence.Description";

        private static BlueprintFeature CreateIomedaeInfluence()
        {
            var icon = FeatureRefs.DeftStrike.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(IomedaeInfluence, IomedaeInfluenceGuid)
                .CopyFrom(
                FeatureRefs.ZenArcherWayOfTheBowLongbowFocus,
                typeof(AddParametrizedFeatures))
              .SetDisplayName(IomedaeInfluenceDisplayName)
              .SetDescription(IomedaeInfluenceDescription)
              .SetIcon(icon)
              .Configure();

            var cat = feat.GetComponent<AddParametrizedFeatures>()?.m_Features?.First();
            if (cat != null)
            {
                cat.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.Longsword;
            }
            return feat;
        }
    }
}
