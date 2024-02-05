using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.CustomComponent.BasePrestigeEnhance
{
    internal class PrerequisiteSpellLevel : Prerequisite
    {
        // Token: 0x0600B636 RID: 46646 RVA: 0x002FAAEC File Offset: 0x002F8CEC
        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
        {
            int? casterTypeSpellLevel = this.GetSpellLevel(unit);
            int requiredSpellLevel = this.RequiredSpellLevel;
            return casterTypeSpellLevel.GetValueOrDefault() >= requiredSpellLevel & casterTypeSpellLevel != null;
        }

        // Token: 0x0600B637 RID: 46647 RVA: 0x002FAB20 File Offset: 0x002F8D20
        public override string GetUITextInternal(UnitDescriptor unit)
        {
            return "Can cast " + RequiredSpellLevel.ToString() + "-level spells";
        }

        // Token: 0x0600B638 RID: 46648 RVA: 0x002FAC20 File Offset: 0x002F8E20
        private int? GetSpellLevel(UnitDescriptor unit)
        {
            int num = 0;
            bool flag = false;
            foreach (ClassData classData in unit.Progression.Classes)
            {
                BlueprintSpellbook spellbook = classData.Spellbook;
                if (spellbook != null)
                {
                    flag = true;
                    num = Mathf.Max(num, unit.DemandSpellbook(classData.CharacterClass).MaxSpellLevel);
                }
            }
            if (flag)
            {
                return new int?(num);
            }
            return null;
        }

        public int RequiredSpellLevel = 1;
    }
}
