using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomComponent.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using System.Drawing;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PrestigePlus.Blueprint.Spell
{
    internal class VexGiant
    {
        private const string VexGiantAbility = "NewSpell.UseVexGiant";
        public static readonly string VexGiantAbilityGuid = "{63A7C188-E1CD-4FF6-9853-FFAD79833341}";

        private const string VexGiantAbility2 = "NewSpell.UseVexGiant2";
        public static readonly string VexGiantAbility2Guid = "{9B8519A4-F13A-4590-949F-EBEF53789C52}";

        private const string VexGiantBuff = "NewSpell.VexGiantBuff";
        private static readonly string VexGiantBuffGuid = "{B592E681-7F13-40C9-AB16-C8BB7F152B5D}";

        private const string VexGiantBuff2 = "NewSpell.VexGiantBuff2";
        private static readonly string VexGiantBuff2Guid = "{C48706D8-C233-44B4-AE12-A5AD05CBCA30}";

        internal const string DisplayName = "NewSpellVexGiant.Name";
        private const string Description = "NewSpellVexGiant.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.MasterHunterAbilityGiants.Reference.Get().Icon;
            var fx = AbilityRefs.InstantEnemy.Reference.Get().GetComponent<AbilitySpawnFx>();

            var enemybuff = BuffConfigurator.New(VexGiantBuff2, VexGiantBuff2Guid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddUniqueBuff()
              .AddComponent<VexGiantComp>()
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(enemybuff, ContextDuration.Variable(ContextValues.Rank()), isFromSpell: true, sameDuration: true)
                .Build();

            var ability2 = AbilityConfigurator.New(VexGiantAbility2, VexGiantAbility2Guid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent(fx)
              .AllowTargeting(false, true, false, false)
              .SetAnimation(CastAnimationStyle.Point)
              .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
              .AddAbilityTargetIsSuitableMountSize()
              .SetRange(AbilityRange.Custom)
              .SetCustomRange(60)
              .SetType(AbilityType.Special)
              .AddAbilityEffectRunAction(action)
              .Configure();

            var selfbuff = BuffConfigurator.New(VexGiantBuff, VexGiantBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .Configure();

            AbilityConfigurator.NewSpell(
                VexGiantAbility, VexGiantAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent(fx)
              .AllowTargeting(false, true, false, false)
              .SetLocalizedDuration(AbilityRefs.Haste.Reference.Get().LocalizedDuration)
              .SetAnimation(CastAnimationStyle.Point)
              .AddAbilityTargetIsSuitableMountSize()
              .SetRange(AbilityRange.Custom)
              .SetCustomRange(60)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend)
              .AddToSpellLists(level: 1, SpellList.Ranger)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(selfbuff, ContextDuration.Variable(ContextValues.Rank()), isFromSpell: true, toCaster: true)
                  .ApplyBuff(enemybuff, ContextDuration.Variable(ContextValues.Rank()), isFromSpell: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }
    }
}