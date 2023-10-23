using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using Kingmaker.UnitLogic;
using Kingmaker.Designers.Mechanics.Buffs;
using BlueprintCore.Utils;
using Kingmaker.RuleSystem;
using PrestigePlus.Grapple;

namespace PrestigePlus.CustomAction.GrappleThrow
{
    internal class HamatulaStrikeGrab : ContextAction
    {
        public override string GetCaption()
        {
            return "Hamatula Grapple";
        }

        public override void RunAction()
        {
            if (!IsHamatula()) { return; }
            RunGrapple();
        }

        private bool IsHamatula()
        {
            UnitEntityData maybeCaster = Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return false;
            }
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = maybeCaster.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP != null) { return false; }
            bool isHamatula = false;
            var Weapon = maybeCaster.GetThreatHandMelee();
            if (Weapon == null) { return false; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Dagger) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.PunchingDagger) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Shortspear) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Spear) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Longspear) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Trident) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.LightPick) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Shortsword) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Starknife) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.HeavyPick) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Rapier) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Siangham) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.DuelingSword) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Estoc) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Tongi) { isHamatula = true; }
            if (Weapon.Weapon.Blueprint.Category == WeaponCategory.Urgrosh) { isHamatula = true; }
            return isHamatula;
        }

        public void RunGrapple()
        {
            UnitEntityData unit = Target.Unit;
            if (unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData maybeCaster = Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (unit == maybeCaster)
            {
                PFLog.Default.Error("Unit can't grapple themselves", Array.Empty<object>());
                return;
            }
            if (maybeCaster.Get<UnitPartGrappleInitiatorPP>())
            {
                PFLog.Default.Error("Caster already grapple", Array.Empty<object>());
                return;
            }
            if (unit.Get<UnitPartGrappleTargetPP>())
            {
                PFLog.Default.Error("Target already grappled", Array.Empty<object>());
                return;
            }
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(maybeCaster, unit, CombatManeuver.Grapple, null);
            ruleCombatManeuver = (Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            if (ruleCombatManeuver.Success)
            {
                maybeCaster.Ensure<UnitPartGrappleInitiatorPP>().Init(unit, CasterBuff, Context);
                unit.Ensure<UnitPartGrappleTargetPP>().Init(maybeCaster, TargetBuff, Context);
            }
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D6D08842-8E03-4A9D-81B8-1D9FB2245649}");
        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");
        public bool isAway;
    }
}
