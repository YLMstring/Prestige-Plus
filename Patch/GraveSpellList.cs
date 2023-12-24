using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Blueprint.PrestigeClass;
using System.Security.Claims;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace PrestigePlus.Patch
{
    internal class GraveSpellList
    {
        private const string SecretDeath = "AgentoftheGrave.SecretDeath";
        public static readonly string SecretDeathGuid = "{406A2DD4-AF4D-45F9-BD96-D78E218DD3AC}";
        internal const string SecretDeathDisplayName = "AgentoftheGraveSecretDeath.Name";
        private const string SecretDeathDescription = "AgentoftheGraveSecretDeath.Description";

        private const string spelllist = "AgentoftheGrave.spelllist";
        private static readonly string spelllistguid = "{72869416-FA1F-4864-BB0A-AAAFD05D7177}";

        private static readonly BlueprintSpellList WizardNecromancySpells = SpellListRefs.WizardNecromancySpellList.Reference.Get();
        private static readonly BlueprintSpellList ClericSpells = SpellListRefs.ClericSpellList.Reference.Get();

        public static void CreateSecretDeath()
        {
            var icon = AbilityRefs.AnimateDead.Reference.Get().Icon;

            var firstLevelSpells = new SpellLevelList(1)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var secondLevelSpells = new SpellLevelList(2)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var thirdLevelSpells = new SpellLevelList(3)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var fourthLevelSpells = new SpellLevelList(4)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var fifthLevelSpells = new SpellLevelList(5)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var sixthLevelSpells = new SpellLevelList(6)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };
            var sevenththLevelSpells = new SpellLevelList(7)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var eighthLevelSpells = new SpellLevelList(8)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var ninthLevelSpells = new SpellLevelList(9)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };


            var spellList = SpellListConfigurator.New(spelllist, spelllistguid)
              .AddToSpellsByLevel(
                new(0),
                firstLevelSpells,
                secondLevelSpells,
                thirdLevelSpells,
                fourthLevelSpells,
                fifthLevelSpells,
                sixthLevelSpells,
                sevenththLevelSpells,
                eighthLevelSpells,
                ninthLevelSpells)
              .SetFilterByMaxLevel(9)
              .Configure();

            foreach (var level in spellList.SpellsByLevel)
            {
                foreach (var spell in ClericSpells.SpellsByLevel[level.SpellLevel].Spells)
                {
                    if (spell.School == SpellSchool.Necromancy)
                    {
                        level.m_Spells.Add(spell.ToReference<BlueprintAbilityReference>());
                    }
                }
                foreach (var spell in WizardNecromancySpells.SpellsByLevel[level.SpellLevel].Spells)
                {
                    level.m_Spells.Add(spell.ToReference<BlueprintAbilityReference>());
                }
            }

            FeatureConfigurator.New(SecretDeath, SecretDeathGuid)
              .SetDisplayName(SecretDeathDisplayName)
              .SetDescription(SecretDeathDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteStatValue(StatType.Intelligence, 13)
              .AddLearnSpellList(spellList: spelllistguid, characterClass: AgentoftheGrave.ArchetypeGuid)
              .Configure();
        }

        private const string spelllist2 = "CreateMiracle.spelllist2";
        public static readonly string spelllist2guid = "{DEEF40E3-22A2-4BD0-AB9A-3194A344EEC4}";

        public static void CreateMiracleList()
        {
            var firstLevelSpells = new SpellLevelList(1)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var secondLevelSpells = new SpellLevelList(2)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var thirdLevelSpells = new SpellLevelList(3)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var fourthLevelSpells = new SpellLevelList(4)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var fifthLevelSpells = new SpellLevelList(5)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var sixthLevelSpells = new SpellLevelList(6)
            {
                m_Spells = new List<BlueprintAbilityReference>() { }
            };

            var spellList = SpellListConfigurator.New(spelllist2, spelllist2guid)
              .AddToSpellsByLevel(
                new(0),
                firstLevelSpells,
                secondLevelSpells,
                thirdLevelSpells,
                fourthLevelSpells,
                fifthLevelSpells,
                sixthLevelSpells)
              .SetFilterByMaxLevel(6)
              .Configure();

            var spells = new List<BlueprintAbility>() { };

            foreach (var level in spellList.SpellsByLevel)
            {
                if (level.SpellLevel > 6) continue;
                foreach (var spell in ClericSpells.SpellsByLevel[level.SpellLevel].Spells)
                {
                    level.m_Spells.Add(spell.ToReference<BlueprintAbilityReference>());
                    spells.Add(spell);
                }
            }

            var list = new List<BlueprintSpellList>() { SpellListRefs.AlchemistSpellList.Reference.Get(), SpellListRefs.BardSpellList.Reference.Get(), SpellListRefs.BloodragerSpellList.Reference.Get(), SpellListRefs.DruidSpellList.Reference.Get(), SpellListRefs.HunterSpelllist.Reference.Get(), SpellListRefs.InquisitorSpellList.Reference.Get(), SpellListRefs.MagusSpellList.Reference.Get(), SpellListRefs.PaladinSpellList.Reference.Get(), SpellListRefs.RangerSpellList.Reference.Get(), SpellListRefs.ShamanSpelllist.Reference.Get(), SpellListRefs.WarpriestSpelllist.Reference.Get(), SpellListRefs.WitchSpellList.Reference.Get(), SpellListRefs.WizardSpellList.Reference.Get() };
            
            foreach (var level in spellList.SpellsByLevel)
            {
                if (level.SpellLevel > 5) continue;
                foreach (var clazz in list)
                {
                    foreach (var spell in clazz.SpellsByLevel[level.SpellLevel].Spells)
                    {
                        if (spells.Contains(spell)) continue;
                        level.m_Spells.Add(spell.ToReference<BlueprintAbilityReference>());
                        spells.Add(spell);
                    }
                }
            }
        }
    }
}
