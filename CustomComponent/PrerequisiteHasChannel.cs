using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;

namespace PrestigePlus.CustomComponent
{
    internal class PrerequisiteHasChannel : Prerequisite
    {
        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
        {
            try
            {
                foreach (var ability in unit.Abilities.RawFacts)
                {
                    if (ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelNegativeHarm)) return true;
                    if (ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelNegativeHeal)) return true;
                    if (ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelPositiveHarm)) return true;
                    if (ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelPositiveHeal)) return true;
                }
                return false;
            }
            catch (Exception ex) { Main.Logger.Error("Failed to CheckPrerequisiteHasChannel.", ex); return false; }
        }

        // Token: 0x0600B3FA RID: 46074 RVA: 0x002EFFB4 File Offset: 0x002EE1B4
        public override string GetUITextInternal(UnitDescriptor unit)
        {
            return "Channel energy class feature";
        }
    }
}