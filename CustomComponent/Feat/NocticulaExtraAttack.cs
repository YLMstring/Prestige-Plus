using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class NocticulaExtraAttack : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600EA11 RID: 59921 RVA: 0x003BEF78 File Offset: 0x003BD178
        public void OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
        {
            if (base.Owner.Body.PrimaryHand.MaybeWeapon?.Blueprint.Category == cat)
            {
                evt.AddExtraAttacks(1, false, false, base.Owner.Body.PrimaryHand.Weapon);
            }
            if (base.Owner.Body.SecondaryHand.MaybeWeapon?.Blueprint.Category == cat)
            {
                evt.AddExtraAttacks(1, false, false, base.Owner.Body.SecondaryHand.Weapon);
            }
        }

        // Token: 0x0600EA12 RID: 59922 RVA: 0x003BEFDC File Offset: 0x003BD1DC
        public void OnEventDidTrigger(RuleCalculateAttacksCount evt)
        {
        }

        // Token: 0x04009A20 RID: 39456
        public WeaponCategory cat = WeaponCategory.Dagger;
    }
}