using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic;
using Owlcat.QA.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Utils;

namespace PrestigePlus.Modify
{
    [AllowMultipleComponents]
    internal class PrerequisiteSpellBook : Prerequisite
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        // Token: 0x0600B3F9 RID: 46073 RVA: 0x002EFF82 File Offset: 0x002EE182
        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
        {
            try
            {
                if (unit.m_Spellbooks == null) { return false; }
                if (Book == null) { return false; }
                if (BlueprintTool.GetRef<BlueprintSpellbookReference>(Book) == null) { return false; }
                return unit.m_Spellbooks.ContainsKey(BlueprintTool.GetRef<BlueprintSpellbookReference>(Book));
            }
            catch (Exception ex) { Logger.Error("Failed to CheckInternal.", ex); return false; }
            //unit.Progression.Features.HasFact(this.Feature);
        }

        // Token: 0x0600B3FA RID: 46074 RVA: 0x002EFFB4 File Offset: 0x002EE1B4
        public override string GetUITextInternal(UnitDescriptor unit)
        {
            return "check if have spellbook";
        }

        // Token: 0x0400723A RID: 29242
        public string Book;
    }
}
