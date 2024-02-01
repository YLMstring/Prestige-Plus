using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using System.Security.AccessControl;
using BlueprintCore.Blueprints.Configurators.Root;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class FakeAlignedClass
    {
        private const string ArchetypeName = "AlignedClass";
        public static readonly string ArchetypeGuid = "{6C2F8B9B-059B-403D-BF05-9A2841D18B18}";

        private const string ClassProgressName = "AlignedPrestigeClass";
        private static readonly string ClassProgressGuid = "{35DA22FE-4D9C-4729-AE82-01EB6BBB1138}";

        public static void Configure()
        {
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .SetDisplayName("")
            .SetDescription("")
            .SetRanks(1)
            .SetIsClassFeature(true)
            .Configure();

          CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
            .SetLocalizedName("")
            .SetLocalizedDescription("")
            .SetSkillPoints(0)
            .SetHitDie(DiceType.Zero)
            .SetPrestigeClass(true)
            .SetBaseAttackBonus(StatProgressionRefs.ZeroProgression.ToString())
            .SetFortitudeSave(StatProgressionRefs.ZeroProgression.ToString())
            .SetReflexSave(StatProgressionRefs.ZeroProgression.ToString())
            .SetWillSave(StatProgressionRefs.ZeroProgression.ToString())
            .SetProgression(progression)
            .SetClassSkills(new StatType[] {  })
            .Configure();
        }

        public static void AddtoMenu(BlueprintCharacterClass clazz)
        {
            void act(ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var clazzref = clazz.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = clazzref;
                i.m_CharacterClasses = result;
            }

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }
    }
}
