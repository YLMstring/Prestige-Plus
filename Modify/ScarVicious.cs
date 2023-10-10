using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class ScarVicious : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (!checkBuff || Owner.HasFact(BlueprintTool.GetRef<BlueprintBuffReference>(Buff)))
            {
                DamageDescription item = new DamageDescription
                {
                    TypeDescription = new DamageTypeDescription
                    {
                        Type = DamageType.Energy,
                        Energy = Kingmaker.Enums.Damage.DamageEnergyType.Magic
                    },
                    Dice = new DiceFormula(num, type),
                    SourceFact = base.Fact
                };
                evt.DamageDescription.Add(item);
            }
        }

        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {

        }
        //private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>("{DAE72E0A-9F44-4CD3-A26E-3D48DFEB23A1}");
        public string Buff = "{DAE72E0A-9F44-4CD3-A26E-3D48DFEB23A1}";
        public bool checkBuff = true;
        public int num = 2;
        public DiceType type = DiceType.D6;
    }
}
