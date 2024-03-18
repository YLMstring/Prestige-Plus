using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent
{
    internal class AttackTypeChangePP : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600E496 RID: 58518 RVA: 0x003AD410 File Offset: 0x003AB610
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            var group = evt.Weapon?.Blueprint.FighterGroup;
            if (group != null && evt.Weapon.Blueprint.FighterGroup.HasFlag(Group) == true)
            {
                evt.AttackType = this.NewType;
            }
        }

        // Token: 0x0600E497 RID: 58519 RVA: 0x003AD45B File Offset: 0x003AB65B
        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }

        // Token: 0x040096AA RID: 38570
        public WeaponFighterGroup Group;

        // Token: 0x040096AD RID: 38573
        public AttackType NewType;
    }
}
