using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class GuidedHandWis : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600C2EE RID: 49902 RVA: 0x0032DE98 File Offset: 0x0032C098
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (cat != null && cat == evt.Weapon.Blueprint.Category)
            {
                evt.AttackBonusStat = StatType.Wisdom;
            }
        }

        // Token: 0x0600C2EF RID: 49903 RVA: 0x0032DF81 File Offset: 0x0032C181
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        public override void OnActivate()
        {
            cat = PrerequisiteDivineWeapon.GetFavoredWeapon(Owner);
        }

        public override void OnDeactivate()
        {
            cat = null;
        }

        private WeaponCategory? cat = null;
    }
}

