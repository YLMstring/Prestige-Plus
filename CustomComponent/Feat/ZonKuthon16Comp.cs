using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ZonKuthon16Comp : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {

        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            bool unarmed = false;
            if (evt.Weapon?.Blueprint.Category == WeaponCategory.UnarmedStrike)
            {
                unarmed = true;
            }
            else if (Owner.HasFact(Feral) && evt.Weapon?.Blueprint.IsNatural == true)
            {
                unarmed = true;
            }
            if (!unarmed) { return; }
            if (evt.AttackRoll?.IsHit == true) 
            { 

            }
            Buff.Remove();
        }

        //FeralCombatTrainingFeature	edb2546d1215491ca404f8dd1a0c2af3	Kingmaker.Blueprints.Classes.BlueprintFeature
        private static BlueprintFeatureReference Feral = BlueprintTool.GetRef<BlueprintFeatureReference>("edb2546d1215491ca404f8dd1a0c2af3");
    }
}
