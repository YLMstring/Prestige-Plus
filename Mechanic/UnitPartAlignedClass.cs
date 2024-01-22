using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes;
using static LayoutRedirectElement;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Class.LevelUp;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.PrestigeClass;

namespace PrestigePlus.Mechanic
{
    internal class UnitPartAlignedClass : OldStyleUnitPart, ILevelUpSelectClassHandler
    {
        public void Add(BlueprintCharacterClass clazz)
        {
            if (clazz == null) return;
            if (Owner.Progression.GetClassLevel(clazz) > 1)
            {
                Classes.Add(clazz);
            }
        }

        public void Clear()
        {
            Classes.Clear();
        }

        public BlueprintCharacterClass GetMax(LevelUpState state)
        {
            if (Evangelist != null && state.SelectedClass == BlueprintTool.GetRef<BlueprintCharacterClassReference>(ExaltedEvangelist.ArchetypeGuid).Get())
            {
                return Evangelist;
            }
            if (Classes.Count == 0) return null;
            var highCountItems = Classes
                .GroupBy(item => item)
                .GroupBy(g => g.Count(), g => g.Key)
                .OrderByDescending(counts => counts.Key)
                .First();

            return (BlueprintCharacterClass)highCountItems;
        }

        void ILevelUpSelectClassHandler.HandleSelectClass(UnitDescriptor unit, LevelUpState state)
        {
            Add(state.SelectedClass);
        }

        private readonly List<BlueprintCharacterClass> Classes = new();
        public BlueprintCharacterClass Evangelist;
        public int SkillPointPenalty = 0;
    }
}
