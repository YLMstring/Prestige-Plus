using BlueprintCore.Utils;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Items;
using PrestigePlus.Blueprint.PrestigeClass;

namespace PrestigePlus.Modify
{
    internal class HolyVindicatorSpellDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            //Logger.Info("start");
            var Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(ability);
            if (evt.Spell == Ability.Get())
            {
                //Logger.Info("start2");
                var spellbook = Owner.GetSpellbook(holy);
                if (spellbook == null) { return; }
                //Logger.Info("start3");
                var stat = spellbook.Blueprint.CastingAttribute;
                int dc = Owner.Stats.GetStat<ModifiableValueAttributeStat>(stat).Bonus + level;
                //Logger.Info(dc.ToString());
                var slot = Owner.Body.PrimaryHand;
                ItemEntityWeapon weapon;
                if (slot.MaybeWeapon != null)
                {
                    weapon = slot.MaybeWeapon;
                }
                else
                {
                    weapon = Owner.Body.EmptyHandWeapon;
                }
                var rule = new RuleCalculateWeaponStats(Owner, weapon, null, null);
                Rulebook.Trigger(rule);
                if (rule.CriticalMultiplier >= 3)
                {
                    dc += 2;
                }
                if (rule.CriticalMultiplier >= 4)
                {
                    dc += 2;
                }
                //Logger.Info(dc.ToString());
                //Logger.Info(spellbook.EffectiveCasterLevel.ToString());
                evt.ReplaceDC = dc;
                evt.ReplaceCasterLevel = spellbook.EffectiveCasterLevel;
            }
        }

        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {

        }

        private static BlueprintCharacterClassReference holy = BlueprintTool.GetRef<BlueprintCharacterClassReference>("b5daf66532f5425aa22df5372c57d766");

        public string ability = HolyVindicator.DivineWrathAblity2Guid;
        public int level = 11;
    }
}
