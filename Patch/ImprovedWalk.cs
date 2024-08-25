using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.ModReferences;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace PrestigePlus.Patch
{
    internal class ImprovedWalk
    {
        public static void Patch()
        {
            BuffConfigurator.For(BuffRefs.WalkThroughSpaceBuff)
                .AddMechanicsFeature(AddMechanicsFeature.MechanicsFeatureType.GetUpWithoutAttackOfOpportunity)
                .Configure();

            Configure();

            var feat = BlueprintTool.GetRef<BlueprintFeatureReference>("5110905a-4911-4916-a39c-9cda7a67eca0")?.Get();
            if (feat != null && ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("teleport")))
            {
                feat.AddComponent<AddFacts>(c => { c.m_Facts = new BlueprintUnitFactReference[] { BlueprintTool.GetRef<BlueprintUnitFactReference>(FeatGuid) }; });
                feat.AddComponent<PrerequisiteFeature>(c => { c.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ShadowDancer.ShadowJumpGuidEx); c.Group = Prerequisite.GroupType.Any; });
                feat.AddComponent<PrerequisiteFeature>(c => { c.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(HorizonWalker.DominanceAstralGuid); c.Group = Prerequisite.GroupType.Any; });
                feat.AddComponent<PrerequisiteFeature>(c => { c.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(MenhirSavant.WalkLinesGuid); c.Group = Prerequisite.GroupType.Any; });
            }

            try
            {
                string DimensionalBladeSpell = "aaed2bc8-7c24-4737-83f6-df4c520888ee";
                AbilityConfigurator.For(DimensionalBladeSpell)
                    .RemoveComponents(c => c is SpellListComponent)
                    .AddToSpellList(4, ModSpellListRefs.AntipaladinSpelllist.ToString())
                    .AddToSpellLists(level: 4, SpellList.Bloodrager)
                    .AddToSpellLists(level: 6, SpellList.Cleric)
                    .AddToSpellLists(level: 5, SpellList.Inquisitor)
                    .AddToSpellLists(level: 5, SpellList.Magus)
                    .AddToSpellLists(level: 4, SpellList.Paladin)
                    .AddToSpellLists(level: 6, SpellList.Wizard)
                    .AddToSpellLists(level: 6, SpellList.Warpriest)
                    .Configure();
            }
            catch (Exception e) { Main.Logger.Error("Failed to edit DimensionalBladeSpell.", e); }
        }

        private static readonly string FeatName = "FeatWalkThroughSpace";
        public static readonly string FeatGuid = "{72253385-5EDA-4EF0-A572-392FA8C62785}";

        private static readonly string BuffName = "BuffWalkThroughSpace";
        public static readonly string BuffGuid = "{A61EECF3-9FE5-4C07-9331-4857FE0D222D}";

        private static readonly string DisplayName = "FeatWalkThroughSpace.Name";
        private static readonly string Description = "FeatWalkThroughSpace.Description";

        public static BlueprintFeature Configure()
        {
            var icon = AbilityRefs.WalkThroughSpace.Reference.Get().Icon;
            var variants = AbilityRefs.DimensionDoorBase.Reference.Get()?.GetComponent<AbilityVariants>()?.m_Variants;

            var Buff1 = BuffConfigurator.New(BuffName, BuffGuid)
             .SetDisplayName(DisplayName)
             .SetDescription(Description)
             .SetIcon(icon);
             
            foreach (var variant in variants)
            {
                Buff1 = Buff1.AddFacts(new() { variant.ToString() });
            }

             Buff1.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            return FeatureConfigurator.New(FeatName, FeatGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddBuffExtraEffects(BuffRefs.WalkThroughSpaceBuff.ToString(), extraEffectBuff: BuffGuid)
                    .Configure();
        }
    }
}
