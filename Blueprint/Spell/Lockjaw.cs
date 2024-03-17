using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using PrestigePlus.CustomComponent.Feat;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using PrestigePlus.Blueprint.RogueTalent;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Conditions.Builder.ContextEx;
using PrestigePlus.CustomAction.GrappleThrow;

namespace PrestigePlus.Blueprint.Spell
{
    internal class Lockjaw
    {
        private const string LockjawAbility = "NewSpell.UseLockjaw";
        public static readonly string LockjawAbilityGuid = "{0DA596DD-6C35-42BD-8E17-D963DBA9B3FB}";

        private const string LockjawAbility1 = "NewSpell.UseLockjaw1";
        public static readonly string LockjawAbility1Guid = "{2B7B1B3B-79C6-41B0-A27A-877CC6912D76}";

        private const string LockjawAbility2 = "NewSpell.UseLockjaw2";
        public static readonly string LockjawAbility2Guid = "{72296F5B-25B8-49DF-886A-EE5346325323}";

        private const string LockjawBuff1 = "NewSpell.UseLockjawBuff1";
        public static readonly string LockjawBuff1Guid = "{CFA28542-7BE7-4381-9C3B-6D77601AB0AA}";

        private const string LockjawBuff2 = "NewSpell.UseLockjawBuff2";
        public static readonly string LockjawBuff2Guid = "{C77362AC-B230-4D8B-A625-17ED777C4D56}";

        internal const string DisplayName = "NewSpellLockjaw.Name";
        private const string Description = "NewSpellLockjaw.Description";

        internal const string DisplayName1 = "NewSpellLockjaw1.Name";
        private const string Description1 = "NewSpellLockjaw1.Description";

        internal const string DisplayName2 = "NewSpellLockjaw2.Name";
        private const string Description2 = "NewSpellLockjaw2.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.FrightfulShape_.Reference.Get().Icon;
            var icon2 = FeatureRefs.RakingClawsFeature.Reference.Get().Icon;
            var fx = AbilityRefs.AnimalGrowth.Reference.Get().GetComponent<AbilitySpawnFx>();

            var grab = ActionsBuilder.New()
                .Add<PPActionGrapple>(c => { c.isAway = false; c.isjaw = true; })
                .Build();

            var grapple = ActionsBuilder.New()
                .CombatManeuver(onSuccess: grab, type: Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple)
                .Build();

            var buff1 = BuffConfigurator.New(LockjawBuff1, LockjawBuff1Guid)
             .SetDisplayName(DisplayName1)
             .SetDescription(Description1)
             .SetIcon(icon2)
             .AddInitiatorAttackWithWeaponTrigger(grapple, false, onlyHit: true, category: WeaponCategory.Claw, checkWeaponCategory: true)
             .AddFacts(new() { CrabKing.Feat2Guid, PinAbilityGuid1, TieUpAbilityGuid, ReadyAbilityGuid, ReleaseAbilityGuid })
             .Configure();

            var ability1 = AbilityConfigurator.NewSpell(
                LockjawAbility1, LockjawAbility1Guid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName1)
              .SetDescription(Description1)
              .SetIcon(icon2)
              .AllowTargeting(false, false, true, true)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
              .SetRange(AbilityRange.Touch)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Reach, Metamagic.Extend)
              .AddComponent(fx)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff1, ContextDuration.Variable(ContextValues.Rank()))
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();

            var buff2 = BuffConfigurator.New(LockjawBuff2, LockjawBuff2Guid)
             .SetDisplayName(DisplayName2)
             .SetDescription(Description2)
             .SetIcon(icon)
             .AddInitiatorAttackWithWeaponTrigger(grapple, false, onlyHit: true, category: WeaponCategory.Bite, checkWeaponCategory: true)
             .AddFacts(new() { CrabKing.Feat2Guid, PinAbilityGuid1, TieUpAbilityGuid, ReadyAbilityGuid, ReleaseAbilityGuid })
             .Configure();

            var ability2 = AbilityConfigurator.NewSpell(
                LockjawAbility2, LockjawAbility2Guid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName2)
              .SetDescription(Description2)
              .SetIcon(icon)
              .AllowTargeting(false, false, true, true)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
              .SetRange(AbilityRange.Touch)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Reach, Metamagic.Extend)
              .AddComponent(fx)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff2, ContextDuration.Variable(ContextValues.Rank()))
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();

            AbilityConfigurator.NewSpell(
                LockjawAbility, LockjawAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(false, false, false, false)
              .SetRange(AbilityRange.Touch)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Reach, Metamagic.Extend)
              .AddToSpellLists(level: 2, SpellList.Druid)
              .AddToSpellLists(level: 2, SpellList.Ranger)
              .AddAbilityVariants(new() { ability1, ability2 })
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }

        private static readonly string PinAbilityGuid1 = "{531632AA-0E0F-402C-8A07-18E8E0D35C80}";
        private static readonly string TieUpAbilityGuid = "{DB453CF7-8799-4FDD-941B-FA33EFF5771A}";
        private static readonly string ReadyAbilityGuid = "{A5057A11-9D24-46D8-9BE6-F5C7D605EDC5}";
        private static readonly string ReleaseAbilityGuid = "{A75ED2DD-7F0D-4367-9953-4179F3E531D2}";
    }
}
