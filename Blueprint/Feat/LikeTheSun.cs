using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using UnityEngine.Serialization;
using UnityEngine;
using PrestigePlus.Blueprint.Spell;

namespace PrestigePlus.Blueprint.Feat
{
    internal class LikeTheSun
    {
        private static readonly string FeatName = "FeatLikeTheSun";
        public static readonly string FeatGuid = "{CC2064B7-5EA0-4DAE-A667-0FCBD44C2767}";

        private static readonly string DisplayName = "FeatLikeTheSun.Name";
        private static readonly string Description = "FeatLikeTheSun.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 1)
                    .AddComponent<PrerequisiteSpellKnown>(c => { c.m_Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.MageLight.ToString()); c.RequireSpellbook = true; })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddAuraFeatureComponent(AnchoriteofDawn.SunBladeBuffGuid)
                    .AddComponent<PPIncreaseSpellLv>()
                    .Configure();
        }
    }
    public class PPIncreaseSpellLv : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (evt.Spell == AbilityRefs.MageLight.Reference.Get() || evt.Spell == AbilityRefs.FlareBurst.Reference.Get()
                || evt.Spell == AbilityRefs.ChainsOfLight.Reference.Get() || evt.Spell == AbilityRefs.Sunbeam.Reference.Get()
                || evt.Spell == AbilityRefs.Sunburst.Reference.Get() || evt.Spell == BurstRadiance1.Get()
                || evt.Spell == DarkLight1.Get() || evt.Spell == Mydriatic.Get()
                || evt.Spell == JudgmentLight.Get() || evt.Spell == MassMydriatic.Get())
            {
                if (evt.SpellLevel < 9)
                {
                    evt.ReplaceSpellLevel = evt.SpellLevel + 1;
                }
            }
        }

        // Token: 0x0600E996 RID: 59798 RVA: 0x003C114A File Offset: 0x003BF34A
        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {

        }

        private static readonly BlueprintAbilityReference BurstRadiance1 = BlueprintTool.GetRef<BlueprintAbilityReference>(BurstRadiance.BurstRadianceAbilityGuid);
        private static readonly BlueprintAbilityReference DarkLight1 = BlueprintTool.GetRef<BlueprintAbilityReference>(DarkLight.DarkLightAbilityGuid);
        private static readonly BlueprintAbilityReference Mydriatic = BlueprintTool.GetRef<BlueprintAbilityReference>("fd75c304-9a13-41e1-8d76-ebee9de92831");
        private static readonly BlueprintAbilityReference JudgmentLight = BlueprintTool.GetRef<BlueprintAbilityReference>("9fe907d3-0833-40a2-9fb6-730f213a29c1");
        private static readonly BlueprintAbilityReference MassMydriatic = BlueprintTool.GetRef<BlueprintAbilityReference>("c60dbab6-2ea3-47c9-9f0b-f1335052ee30");
    }
}
