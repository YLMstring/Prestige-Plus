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
using JetBrains.Annotations;
using Kingmaker.UnitLogic.Abilities;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.Spell;
using BlueprintCore.Blueprints.References;

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
                    var book = unit.DemandSpellbook(classData.CharacterClass);
                    num = Mathf.Max(num, book.MaxSpellLevel);
                    if (unit.HasFact(Sunfeat))
                    {
                        if (book.IsKnown(AbilityRefs.FlareBurst.Reference) || book.m_KnownSpells.Any((List<AbilityData> l) => l.Any((AbilityData s) => s.Blueprint == AbilityRefs.FlareBurst.Reference.Get())))
                        {
                            num = Math.Max(num, 2);
                        }
                        if (book.IsKnown(BurstRadiance1) || book.m_KnownSpells.Any((List<AbilityData> l) => l.Any((AbilityData s) => s.Blueprint == BurstRadiance1.Get())))
                        {
                            num = Math.Max(num, 3);
                        }
                    }
                }
            }
            if (flag)
            {
                return new int?(num);
            }
            return null;
        }

        public int RequiredSpellLevel = 1;
        private static readonly BlueprintFeatureReference Sunfeat = BlueprintTool.GetRef<BlueprintFeatureReference>(LikeTheSun.FeatGuid);
        private static readonly BlueprintAbilityReference BurstRadiance1 = BlueprintTool.GetRef<BlueprintAbilityReference>(BurstRadiance.BurstRadianceAbilityGuid);
    }
}
