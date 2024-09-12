using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using Kingmaker.AreaLogic.Capital;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Armies.Components;
using Kingmaker.Armies.TacticalCombat.LeaderSkills;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Craft;
using Kingmaker.Designers.EventConditionActionSystem.Events;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Dungeon.FactLogic;
using Kingmaker.Kingdom.Settlements.BuildingComponents;
using Kingmaker.Tutorial;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace PrestigePlus
{
    [HarmonyPatch]
    public class OwnerBlueprintWarning
    {
        public static readonly IEnumerable<Type> ErrorComponentTypes =
        [
        typeof(CapitalCompanionLogic),
        typeof(EtudeBracketMusic),
        typeof(EtudeBracketSetCompanionPosition),
        typeof(ArmyUnitComponent),
        typeof(LeaderPercentAttributeBonus),
        typeof(MaxArmySquadsBonusLeaderComponent),
        typeof(SquadsActionOnTacticalCombatStart),
        typeof(Experience),
        typeof(PrerequisiteArchetypeLevel),
        typeof(PrerequisiteClassLevel),
        typeof(PrerequisiteFeature),
        typeof(CraftInfoComponent),
        typeof(EvaluatedUnitCombatTrigger),
        typeof(ControlledProjectileHolder),
        typeof(DungeonAddLootToVendor),
        typeof(BuildingUpgradeBonus),
        typeof(TutorialPage),
        typeof(AbilityCustomDimensionDoor),
        typeof(AbilityDeliverProjectileOnGrid),
        typeof(AbilityIsBomb),
        typeof(AbilityDeliverEffect),
        typeof(MarkUsableWhileCan),
        typeof(ActivatableAbilitySet),
        typeof(ActivatableAbilitySetItem),
        typeof(AddAbilityUseTrigger),
        typeof(AddFeaturesFromSelectionToDescription),
        typeof(AddTriggerOnActivationChanged),
        typeof(AddVendorItems),
        typeof(NenioSpecialPolymorphWhileEtudePlaying),
        typeof(ChangeSpellElementalDamage),
        typeof(ContextCalculateAbilityParams),
        typeof(ContextRankConfig),
        typeof(ContextSetAbilityParams),
        typeof(UnitPropertyComponent)
        ];

        [HarmonyBefore("WrathPatches")]
        [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Load))]
        [HarmonyPostfix]
        static void Postfix(SimpleBlueprint __result, BlueprintGuid guid)
        {
            if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("joypatch"))) return;

            if (__result is not BlueprintScriptableObject blueprint) return;

            HashSet<(BlueprintScriptableObject, BlueprintComponent, BlueprintComponent)> set = [];

            foreach (var c in blueprint.ComponentsArray)
            {
                if (c.OwnerBlueprint != blueprint)
                {
                    if (ErrorComponentTypes.Any(t => t.IsAssignableFrom(c.GetType())))
                    {
                        var newc = Helpers.CreateCopy(c);
                        newc.OwnerBlueprint = blueprint;
                        set.Add((blueprint, c, newc));
                        Main.Logger.Info("Replace Owner for " + c.name + " in " + blueprint.name);
                    }
                }
            }
            foreach (var tuple in set)
            {
                ReplaceComp(tuple.Item1, tuple.Item2, tuple.Item3);
            }
        }
        private static void ReplaceComp(BlueprintScriptableObject blueprint, BlueprintComponent c, BlueprintComponent newc)
        {
            blueprint.RemoveComponent(c);
            blueprint.AddComponent(newc);
        }

    }
}