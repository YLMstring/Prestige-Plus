using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Armies.TacticalCombat.Components.ChangeLeaderSkillPowerOnAbilityUse;
using static Kingmaker.Blueprints.Area.FactHolder;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Designers;
using BlueprintCore.Blueprints.References;
using PrestigePlus.Blueprint.Archetype;
using Kingmaker.Utility;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.ElementsSystem;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class MagusCloseRange : ContextAction
    {
        public override string GetCaption()
        {
            return "MagusCloseRange";
        }

        public override void RunAction()
        {
            bool isCrit = ContextData<ContextAttackData>.Current?.AttackRoll?.IsCriticalConfirmed ?? false;
            var ability = Context.SourceAbilityContext.Ability?.StickyTouch?.ConvertedFrom;
            if (Context.MaybeCaster == null || ability?.AbilityDeliverProjectile.NeedAttackRoll != true) { return; }
            AbilityExecutionContext abilityContext3 = base.AbilityContext;
            bool isDuplicateSpellApplied = abilityContext3 != null && abilityContext3.IsDuplicateSpellApplied;
            Rulebook.Trigger(new RuleCastSpell(ability, base.Target)
            {
                IsDuplicateSpellApplied = isDuplicateSpellApplied
            });
            GameHelper.ApplyBuff(Context.MaybeCaster, CloseBuff, new Rounds?(1.Rounds()));
            if (isCrit)
            {
                GameHelper.ApplyBuff(Context.MaybeCaster, CloseBuff, new Rounds?(1.Rounds()));
            }
            
        }

        private static readonly BlueprintBuffReference CloseBuff = BlueprintTool.GetRef<BlueprintBuffReference>(SpireDefender.CloseRangeBuff2Guid);
    }
}