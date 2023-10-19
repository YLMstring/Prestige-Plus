﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class DownLikeDominoes
    {
        private static readonly string DragName = "DownLikeDominoes";
        public static readonly string DragGuid = "{D94F385F-3490-48E1-94DF-65D4CA181B02}";

        private static readonly string DragDisplayName = "DownLikeDominoes.Name";
        private static readonly string DragDescription = "DownLikeDominoes.Description";

        private const string StyleAbility = "DownLikeDominoes.StyleAbility";
        private static readonly string StyleAbilityGuid = "{C5AF7D16-EDA0-4CCD-81A4-0E1977D4F32E}";

        private const string StyleBuff = "DownLikeDominoes.StyleBuff";
        public static readonly string StyleBuffGuid = "{3B4CE92D-3F13-49CC-8778-03EBD4334403}";

        public static void DragConfigure()
        {
            var icon = FeatureRefs.FurysFall.Reference.Get().Icon;

            var DoThrow = ActionsBuilder.New()
                .Add<ContextActionDomino>()
                .Build();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(DoThrow)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetCanTargetFriends(false)
                .SetCanTargetPoint(false)
                .SetRange(AbilityRange.Touch)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            var buff = BuffConfigurator.New(StyleBuff, StyleBuffGuid)
               .SetDisplayName(DragDisplayName)
               .SetDescription(DragDescription)
               .SetIcon(icon)
               .AddFacts(new() { ability })
               .SetRanks(10)
               .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Rank)
               .Configure();

            var action = ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).Build();

            var action0 = ActionsBuilder.New()
                .Conditional(ConditionsBuilder.New().HasFact(buff).Build(),
                ifFalse: action)
                .Build();

            FeatureConfigurator.New(DragName, DragGuid, FeatureGroup.MythicAbility)
                    .SetDisplayName(DragDisplayName)
                    .SetDescription(DragDescription)
                    .SetIcon(icon)
                    .AddManeuverTrigger(action0, Kingmaker.RuleSystem.Rules.CombatManeuver.Trip, true)
                    .Configure();
        }
    }
}
