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
    internal class SpiritualFormBonus : UnitFactComponentDelegate<SpiritualFormBonus.ComponentData>
    {
        public override void OnTurnOn()
        {
            ModifiableValue stat;
            stat = base.Owner.Stats.GetStat(Data.Stat);
            if (Owner.Stats.Dexterity.ModifiedValue > stat.ModifiedValue)
            {
                Data.Stat = StatType.Dexterity;
            }
            stat = base.Owner.Stats.GetStat(Data.Stat);
            if (Owner.Stats.Constitution.ModifiedValue > stat.ModifiedValue)
            {
                Data.Stat = StatType.Constitution;
            }
            stat = base.Owner.Stats.GetStat(Data.Stat);
            if (Owner.Stats.Intelligence.ModifiedValue > stat.ModifiedValue)
            {
                Data.Stat = StatType.Intelligence;
            }
            stat = base.Owner.Stats.GetStat(Data.Stat);
            if (Owner.Stats.Wisdom.ModifiedValue > stat.ModifiedValue)
            {
                Data.Stat = StatType.Wisdom;
            }
            stat = base.Owner.Stats.GetStat(Data.Stat);
            if (Owner.Stats.Charisma.ModifiedValue > stat.ModifiedValue)
            {
                Data.Stat = StatType.Charisma;
            }
            stat.AddModifier(4, base.Runtime);
        }

        // Token: 0x0600C0FC RID: 49404 RVA: 0x003267DA File Offset: 0x003249DA
        public override void OnTurnOff()
        {
            ModifiableValue stat = base.Owner.Stats.GetStat(Data.Stat);
            if (stat == null)
            {
                return;
            }
            stat.RemoveModifiersFrom(base.Runtime);
        }
        public class ComponentData
        {
            public StatType Stat = StatType.Strength;
        }
    }
}
