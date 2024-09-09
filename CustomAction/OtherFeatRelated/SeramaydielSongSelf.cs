using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using Kingmaker.Designers;
using Kingmaker.RuleSystem.Rules;
using static Kingmaker.UI.Context.MenuItem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class SeramaydielSongSelf : ContextAction
    {
        public BlueprintActivatableAbility Ability;
        public BlueprintBuff buff;
        public override string GetCaption()
        {
            return string.Format("Turn on activatable ability {0} for unit {1}", this.Ability);
        }

        // Token: 0x0600F5B7 RID: 62903 RVA: 0x003E5320 File Offset: 0x003E3520
        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            var result = GameHelper.TriggerSkillCheck(new RuleSkillCheck(caster, Kingmaker.EntitySystem.Stats.StatType.SkillUseMagicDevice, 10)
            {
                IgnoreDifficultyBonusToDC = true,
            }, Context, true).RollResult;
            if (caster.GetFact(this.Ability) is ActivatableAbility aab) 
            { 
                aab.IsOn = true;
            }
            var songdc = caster.GetFact(buff) as Buff;
            songdc?.SetRank(result);
        }
    }
}