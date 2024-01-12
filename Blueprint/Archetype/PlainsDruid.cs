using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Feats;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class PlainsDruid
    {
        private const string ArchetypeName = "PlainsDruid";
        private static readonly string ArchetypeGuid = "{7A313766-8ABD-4EAE-9AB4-9B5EAABD137E}";
        internal const string ArchetypeDisplayName = "PlainsDruid.Name";
        private const string ArchetypeDescription = "PlainsDruid.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.DruidClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(2, FeatureRefs.DruidWoodlandStride.ToString())
            .SetRemoveFeaturesEntry(4, FeatureRefs.ResistNaturesLure.ToString(), FeatureRefs.WildShapeIWolfFeature.ToString())
            .SetRemoveFeaturesEntry(6, FeatureRefs.WildShapeIILeopardFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalSmallFeature.ToString())
            .SetRemoveFeaturesEntry(8, FeatureRefs.WildShapeIVBearFeature.ToString(), FeatureRefs.WildShapeElementalFeatureAddMedium.ToString())
            .SetRemoveFeaturesEntry(9, FeatureRefs.VenomImmunity.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.WildShapeIIISmilodonFeature.ToString(), FeatureRefs.WildShapeElementalFeatureAddLarge.ToString(), FeatureRefs.WildShapeIVShamblingMoundFeature.ToString())
            .SetRemoveFeaturesEntry(12, FeatureRefs.WildShapeElementaHugeFeature.ToString())
            .AddToAddFeatures(2, FeatureRefs.FavoriteTerrainPlains.ToString())
            .AddToAddFeatures(3, FeatureRefs.FastMovement.ToString())
            .AddToAddFeatures(9, CannyChargerFeat())
            .AddToAddFeatures(13, FeatureRefs.Evasion.ToString())
            .AddToAddFeatures(6, FeatureRefs.WildShapeIWolfFeature.ToString())
            .AddToAddFeatures(8, FeatureRefs.WildShapeIILeopardFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalSmallFeature.ToString())
            .AddToAddFeatures(10, FeatureRefs.WildShapeIVBearFeature.ToString(), FeatureRefs.WildShapeElementalFeatureAddMedium.ToString())
            .AddToAddFeatures(12, FeatureRefs.WildShapeIIISmilodonFeature.ToString(), FeatureRefs.WildShapeElementalFeatureAddLarge.ToString(), FeatureRefs.WildShapeIVShamblingMoundFeature.ToString())
            .AddToAddFeatures(14, FeatureRefs.WildShapeElementaHugeFeature.ToString())
              .Configure();

        }

        private const string CannyCharger = "PlainsDruid.CannyCharger";
        public static readonly string CannyChargerGuid = "{F553A4FD-51AC-428B-86FF-7A8CB0A034BE}";

        internal const string CannyChargerDisplayName = "PlainsDruidCannyCharger.Name";
        private const string CannyChargerDescription = "PlainsDruidCannyCharger.Description";
        public static BlueprintFeature CannyChargerFeat()
        {
            var icon = FeatureRefs.VulpinePounce.Reference.Get().Icon;

            return FeatureConfigurator.New(CannyCharger, CannyChargerGuid)
              .SetDisplayName(CannyChargerDisplayName)
              .SetDescription(CannyChargerDescription)
              .SetIcon(icon)
              .AddACBonusAgainstBuffOwner(bonus: 4, checkedBuff: BuffRefs.ChargeBuff.ToString(), descriptor: Kingmaker.Enums.ModifierDescriptor.Dodge)
              .AddAuraFeatureComponent(StagStyle.StylebuffGuid)
              .AddFacts(new() { AerialAssault.ReleaseAbilityGuid })
              .Configure();
        }
    }
}