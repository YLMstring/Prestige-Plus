using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.Modify
{
    internal class RiderCombinedMight : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.Weapon == null)
            {
                return;
            }
            if (!evt.Weapon.Blueprint.IsMelee)
            {
                return;
            }
            if (Owner.HasFact(Cooldown) || !Owner.HasFact(BlueprintRoot.Instance.SystemMechanics.ChargeBuff))
            {
                return;
            }
            UnitEntityData mount = Owner.GetSaddledUnit();
            if (mount == null) return;
            if (mount.Descriptor.State.IsHelpless) return;
            int bonus = mount.Stats.Strength.Bonus / 2;
            bonus = Math.Max(bonus, 0);
            evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(bonus, base.Runtime, ModifierDescriptor.UntypedStackable));
            GameHelper.ApplyBuff(Owner, Cooldown, new Rounds?(1.Rounds()));
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {

        }
        private static BlueprintBuffReference Cooldown = BlueprintTool.GetRef<BlueprintBuffReference>("{49F7F0A5-B8E5-4C21-AFB7-E2E509D6C34E}");
    }
}
