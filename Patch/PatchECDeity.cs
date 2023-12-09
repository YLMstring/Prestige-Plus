using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class PatchECDeity
    {
        public static WeaponCategory? Find(UnitEntityData unit)
        {
            Lpack = LocalizationManager.LoadPack(Kingmaker.Localization.Shared.Locale.enGB); 
            return FindSelection(unit, FeatureSelectionRefs.DeitySelection.Reference);
        }

        public static WeaponCategory? FindSelection(UnitEntityData unit, BlueprintFeatureSelection selection)
        {
            if (selection?.AllFeatures.Any() != true) return null;
            foreach (var feature in selection.AllFeatures)
            {
                var weapon = FindFeature(unit, feature);
                if (weapon != null) return weapon;
            }
            return null;
        }

        public static WeaponCategory? FindFeature(UnitEntityData unit, BlueprintFeature feat)
        {
            var selection = feat as BlueprintFeatureSelection;
            if (selection != null) 
            { 
                var weapon = FindSelection(unit, selection);
                if (weapon != null) return weapon;
                return null;
            }
            var des = feat.m_Description.LoadString(Lpack, Kingmaker.Localization.Shared.Locale.enGB);
            
            return null;
        }

        private static LocalizationPack Lpack;
    }
}
