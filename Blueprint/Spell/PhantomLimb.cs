using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.ModReferences;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PrestigePlus.Blueprint.Spell
{
    internal class PhantomLimb
    {
        private const string PhantomLimbAbility = "NewSpell.UsePhantomLimb";
        public static readonly string PhantomLimbAbilityGuid = "{2AEE92B9-3EAC-4290-AE38-A7A131ABE7D7}";

        private const string PhantomLimbBuff = "NewSpell.PhantomLimbBuff";
        private static readonly string PhantomLimbBuffGuid = "{394935A6-9DDD-485A-B8E3-41C148806334}";

        internal const string DisplayName = "NewSpellPhantomLimb.Name";
        private const string Description = "NewSpellPhantomLimb.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.ConstrictingCoils.Reference.Get().Icon;

            var buff = BuffConfigurator.New(PhantomLimbBuff, PhantomLimbBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddAdditionalLimb(ItemWeaponRefs.Claw1d4.ToString())
              .AddAdditionalLimb(ItemWeaponRefs.Claw1d4.ToString())
              .Configure();

            AbilityConfigurator.NewSpell(
                PhantomLimbAbility, PhantomLimbAbilityGuid, SpellSchool.Necromancy, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent(AbilityRefs.MageShield.Reference.Get().GetComponent<AbilitySpawnFx>())
              .AllowTargeting(false, false, true, true)
              .SetAnimation(CastAnimationStyle.Touch)
              .SetRange(AbilityRange.Touch)
              .SetType(AbilityType.Spell)
              .SetLocalizedDuration(Duration.TenMinutesPerLevel)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend, Metamagic.Quicken)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), Kingmaker.UnitLogic.Mechanics.DurationRate.TenMinutes), isFromSpell: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }
    }
}
