using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Blueprint.Archetype;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class SavageChargeComp : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ITargetRulebookSubscriber
    {
        // Token: 0x0600E94C RID: 59724 RVA: 0x003BCC54 File Offset: 0x003BAE54
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (evt.Weapon == null || !evt.Weapon.Blueprint.FighterGroup.Contains(this.WeaponGroup))
            {
                return;
            }
            int num = Owner.Progression.GetClassLevel(CharacterClassRefs.FighterClass.Reference) / 2 - 2;
            evt.AddModifier(num, base.Fact);
        }

        // Token: 0x0600E94D RID: 59725 RVA: 0x003BCCCF File Offset: 0x003BAECF
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            int dividedby = Owner.HasFact(Def1) ? 4 : 2;
            int num = -Owner.Progression.GetClassLevel(CharacterClassRefs.FighterClass.Reference) / dividedby + 2;
            evt.AddModifier(num, Fact, ModifierDescriptor.Penalty);
        }
        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        // Token: 0x04009984 RID: 39300
        public WeaponFighterGroup WeaponGroup = WeaponFighterGroup.Natural;

        private static BlueprintFeatureReference Def1 = BlueprintTool.GetRef<BlueprintFeatureReference>(SavageWarrior.GreaterChargeGuid);

    }
}

