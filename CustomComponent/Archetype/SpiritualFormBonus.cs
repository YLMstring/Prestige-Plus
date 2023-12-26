using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Designers.Mechanics.Facts.Restrictions;
using Kingmaker.EntitySystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class SpiritualFormBonus : UnitFactComponentDelegate
    {
        public override void OnTurnOn()
        {
            ModifiableValue stat = base.Owner.Stats.Strength;
            if (Owner.Stats.Dexterity > stat)
            {
                stat = base.Owner.Stats.Dexterity;
            }
            if (Owner.Stats.Constitution > stat)
            {
                stat = base.Owner.Stats.Constitution;
            }
            if (Owner.Stats.Intelligence > stat)
            {
                stat = base.Owner.Stats.Intelligence;
            }
            if (Owner.Stats.Wisdom > stat)
            {
                stat = base.Owner.Stats.Wisdom;
            }
            if (Owner.Stats.Charisma > stat)
            {
                stat = base.Owner.Stats.Charisma;
            }
            stat.AddModifier(4, base.Runtime);
        }

        // Token: 0x0600C0FC RID: 49404 RVA: 0x003267DA File Offset: 0x003249DA
        public override void OnTurnOff()
        {
            Owner.Stats.Strength.RemoveModifiersFrom(base.Runtime);
            Owner.Stats.Dexterity.RemoveModifiersFrom(base.Runtime);
            Owner.Stats.Constitution.RemoveModifiersFrom(base.Runtime);
            Owner.Stats.Intelligence.RemoveModifiersFrom(base.Runtime);
            Owner.Stats.Wisdom.RemoveModifiersFrom(base.Runtime);
            Owner.Stats.Charisma.RemoveModifiersFrom(base.Runtime);
        }
    }
}
