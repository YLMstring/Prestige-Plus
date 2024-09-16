using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;
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
            try
            {
                if (__result is not BlueprintScriptableObject blueprint) return;
                if (blueprint.ComponentsArray.Count() == 0) return;
                for (int i = 0; i < blueprint.ComponentsArray.Count(); i++)
                {
                    var c = blueprint.ComponentsArray[i];
                    if (c.OwnerBlueprint != blueprint)
                    {
                        if (ErrorComponentTypes.Any(t => t.IsAssignableFrom(c.GetType())))
                        {
                            var ms = new MemoryStream();

                            var writer = new StreamWriter(ms);
                            var jsonWriter = new JsonTextWriter(writer);
                            Json.Serializer.Serialize(jsonWriter, c);

                            ms.Position = 0;

                            var reader = new StreamReader(ms);
                            var jsonReader = new JsonTextReader(reader);
                            var newc = (BlueprintComponent)Json.Serializer.Deserialize(jsonReader, c.GetType()) ?? Helpers.CreateCopy(c);
                            if (newc != null)
                            {
                                blueprint.ComponentsArray[i] = newc;
                                blueprint.ComponentsArray[i].OwnerBlueprint = blueprint;
                            }
                        }
                    }
                }
            } catch (Exception ex) { Main.Logger.Error("Failed to joypatch " + guid.ToString(), ex); }
        }
    }
}