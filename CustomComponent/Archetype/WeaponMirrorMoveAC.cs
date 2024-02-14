using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class WeaponMirrorMoveAC : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, ITargetRulebookSubscriber
    {
        // Token: 0x0600E370 RID: 58224 RVA: 0x003A9B80 File Offset: 0x003A7D80
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (Owner.State.IsHelpless || Owner.State.HasCondition(UnitCondition.Prone)) { return; }
            var weaponTraining = base.Owner.Get<UnitPartWeaponTraining>();
            var weapon = base.Owner.Body.PrimaryHand.Weapon;
            var trainingBonus = weaponTraining?.GetWeaponRank(weapon) ?? 0;
            if (trainingBonus > 0 && evt.Weapon.Blueprint.Category == weapon.Blueprint.Category)
            {
                evt.Target.Stats.AC.AddModifierUnique(trainingBonus, base.Runtime, ModifierDescriptor.Insight);
            }
        }

        // Token: 0x0600E371 RID: 58225 RVA: 0x003A9BD8 File Offset: 0x003A7DD8
        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            evt.Target.Stats.AC.RemoveModifiersFrom(base.Runtime);
        }
    }
}
