using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.Designers;
using Kingmaker.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.Feat;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class NiviGemAttack : UnitFactComponentDelegate<NiviGemAttack.ComponentData>, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>
    { 

		// Token: 0x0600CD47 RID: 52551 RVA: 0x00352FDC File Offset: 0x003511DC
		public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.IsFake)
            {
                return;
            }
            int bonus = Owner.Progression.CharacterLevel / 2;
            if (wager && UnityEngine.Random.Range(0, 2) == 0)
            {
                Data.AttackBonus = -bonus;
                Data.Des = ModifierDescriptor.Penalty;
            }
            else
            {
                Data.AttackBonus = bonus;
                Data.Des = ModifierDescriptor.Luck;
            }
            Data.Target = evt.Target;
        }

        // Token: 0x0600CD48 RID: 52552 RVA: 0x00352FE0 File Offset: 0x003511E0
        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.IsFake || Data.Target == null)
            {
                return;
            }
            if (evt.IsHit && (evt.D20 + evt.AttackBonus >= evt.TargetAC + Data.AttackBonus || evt.D20 == 20))
            {
                GameHelper.ApplyBuff(Owner, ExtraBuff);
            }
            if (wager)
            {
                if (Owner.GetFact(ExtraBuff) is not Buff extra || extra.Rank <= 1)
                {
                    GameHelper.RemoveBuff(Owner, ExtraBuff);
                    GameHelper.RemoveBuff(Owner, BasicBuff);
                }
                else
                {
                    extra.Rank -= 1;
                }
            }
            if (Fact is Buff self)
            {
                self.Remove();
            }
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            if (base.Data.Target != null && base.Data.Target == evt.Target)
            {
                evt.AddModifier(Data.AttackBonus, Fact, Data.Des);
            }
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
            
        }

        public class ComponentData
        {
            public int AttackBonus;
            public UnitReference Target;
            public ModifierDescriptor Des;
        }

        public bool wager = false;

        private static BlueprintBuffReference ExtraBuff = BlueprintTool.GetRef<BlueprintBuffReference>(DeificObedience.Nivi3Buff2Guid);
        private static BlueprintBuffReference BasicBuff = BlueprintTool.GetRef<BlueprintBuffReference>(DeificObedience.Nivi3Buff4Guid);
    }
}
