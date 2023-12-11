using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes.Prerequisites;
using BlueprintCore.Blueprints.References;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomComponent
{
    [AllowMultipleComponents]
    internal class PrerequisiteDivineWeapon : Prerequisite
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        // Token: 0x0600B3F9 RID: 46073 RVA: 0x002EFF82 File Offset: 0x002EE182
        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
        {
            try
            {
                var cat = GetFavoredWeapon(unit);
                if (cat.Count() == 0)
                {
                    return false;
                }
                foreach (var item in cat)
                {
                    if (UnitHelper.GetFeature(unit, ParametrizedFeatureRefs.WeaponFocus.Reference, item) != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex) { Logger.Error("Failed to CheckInternal.", ex); return false; }
        }

        // Token: 0x0600B3FA RID: 46074 RVA: 0x002EFFB4 File Offset: 0x002EE1B4
        public override string GetUITextInternal(UnitDescriptor unit)
        {
            return "Weapon Focus (deity’s favored weapon)";
        }

        // Token: 0x0400723A RID: 29242
        public static List<WeaponCategory> GetFavoredWeapon(UnitDescriptor unit)
        {
            List<WeaponCategory> cat = new() { };
            var list = FeatureRefs.WarpriestDeitySacredWeaponFeature.Reference.Get().GetComponents<AddFeatureIfHasFact>();
            foreach (var deity in list)
            {
                if (unit.HasFact(deity.CheckedFact))
                {
                    var weapon = deity.Feature?.GetComponent<SacredWeaponFavoriteDamageOverride>()?.Category;
                    if (weapon != null)
                    {
                        cat.Add((WeaponCategory)weapon);
                    }
                }
            }
            return cat;
        }
    }
}
