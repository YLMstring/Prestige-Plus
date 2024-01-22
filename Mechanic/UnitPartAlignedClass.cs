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
using Kingmaker.UnitLogic.Parts;

namespace PrestigePlus.Mechanic
{
    internal class UnitPartAlignedClass : OldStyleUnitPart
    {
        public void Init(int num)
        {
            if (isInited != true)
            {
                SkillPointPenalty = num;
                isInited = true;
            }
        }

        public BlueprintCharacterClass Evangelist;
        public int SkillPointPenalty;
        public bool isInited;
    }
}
