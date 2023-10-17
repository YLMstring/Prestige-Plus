# Prestige Plus v1.6.0 for WoTR 
## Requirements: [ModMenu](https://github.com/WittleWolfie/ModMenu). [TTT-Core](https://github.com/Vek17/TabletopTweaks-Core).

- Add Grapple Mechanic!
- Add Agent of the Grave, Arcane Archer, Deadeye Devotee, Asavir, Chevalier, Halfling Opportunist, Hinterlander, Horizon Walker, Inheritor’s Crusader, Mammoth Rider, Sanguine Angel, Scar Seeker, Shadowdancer, Umbral Agent, Dragon Fury prestige class.
- Apart from some minor tweaks, all the homebrew stuff are optional.

## Featuring

### Combat Maneuver+
- You can Disarm, Sunder, or Trip in lieu of an melee attack, like on tabletop!
- [Seize the Opportunity](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/seize-the-opportunity-combat/): From Path of War
- [Strength Surge](https://www.d20pfsrd.com/classes/core-classes/barbarian/rage-powers/paizo-rage-powers/strength-surge-ex/)
- [Ki Throw](https://www.d20pfsrd.com/feats/combat-feats/ki-throw-combat/) and its mythic version!
- [Improved Ki Throw](https://www.d20pfsrd.com/feats/combat-feats/improved-ki-throw-combat/)
- [Binding Throw](https://www.d20pfsrd.com/feats/combat-feats/binding-throw-combat/)
- [Enhanced Ki Throw](https://www.d20pfsrd.com/feats/combat-feats/enhanced-ki-throw-combat/)
- [Smashing Style](https://www.d20pfsrd.com/feats/combat-feats/smashing-style-combat-style/)
- [Crush Armor](https://www.d20pfsrd.com/feats/combat-feats/weapon-trick-combat/)
- [Sunder Storm](https://www.d20pfsrd.com/alternative-rule-systems/mythic/mythic-heroes/mythic-paths-paizo-inc/champion/champion-path-abilities/sunder-storm-ex/)
### [Grapple Mechanic](https://www.d20pfsrd.com/gamemastering/Combat/#Grapple)
- This is how I implement it:
1. There are 3 kinds of grapples in the game. My Grapple Mechanic is very close to tabletop. Shifter grapples stay owlcat-brew but once the shifter takes Grapple (Improved Grapple) feat, their grapple mechanics would be turned into mine. Finally, monster grapples stay the way it is in vanilla.
2. If you are grappled, you can attempt to break the grapple by making a combat maneuver check (DC equal to your opponent’s CMD; this does not provoke an attack of opportunity) or Thievery check (with a DC equal to your opponent’s CMD). If you succeed, you break the grapple as a standard action. However, if you fail nothing happens, you don't use your standard action.
3. Tied Up creatures still need to be grappled. The rope would be bursted if you release grapple, because you don't have time to tie someone up properly in combat. They are not helpless but can become the target of Coup De Grace. Additionally, grapple actions against them auto-succeed, they're auto pinned by the rope and their Thievery check DC to escape is higher.
- Added Feats: Grapple (Improved Grapple), Greater Grapple, Rapid Grappler, Unfair Grip, Pinning Knockout, Pinning Rend, Savage Slam, Dramatic Slam, Hamatula Strike, Throat Slicer
- Added Mythic Feats and Abilities (inspired by tabletop abilities of the same name): Grapple (Mythic), Uncanny Grapple, Aerial Assault, Knot Expert, Meat Shield, Maneuver Expert
- Added Familiar: Crab King
- Added Alchemist Discovery: Tentacle
### [Agent of the Grave Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/a-b/agent-of-the-grave/)
- The class has only 5 levels.
- Agent of the Grave lose spell progression at level 1. I have no idea why my code can make you lose spell progression, but it just works.
- Hide your alignment!
#### Homebrew Options:  
- At level 5 you can learn all necromancy spells (require 13 int, full caster)
- Or become a ghoul! (you're undead so incompatible with lichdom)
- Or become a vampire! (require blood drinker, incompatible with lichdom)
### [Anchorite of Dawn Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/a-b/anchorite/)
- Focused Sacred Weapon can't be turned off once chosen (it will become very complicated for me otherwise)
#### Homebrew Options:  
- Blossoming Light: a mythic ability to use Solar Invocation for free!
### [Arcane Archer Class](https://www.d20pfsrd.com/classes/prestige-classes/core-rulebook/arcane-archer/)   
- A True Imbue Arrow is always with a quickened spell XD
- And we have [Deadeye Devotee](https://aonprd.com/ArchetypeDisplay.aspx?FixedName=Arcane%20Archer%20Deadeye%20Devotee)!
#### Homebrew Options:  
- Storm of Arrows: a mythic ability to increase the number of uses of Hail of Arrows!
### [Asavir Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/a-b/asavir/)   
- Thanks to bittercranberry for his aid another and camaraderie (in [Expanded Content](https://github.com/ka-dyn/ExpandedContent) which it requires)!
- Share your pet with Djinni’s Blessing!
- Trample (Mythic): a mythic feat inspired by tabletop abilities of the same name! (notice that owlcat trample works differently from tabletop)
#### Homebrew Options:  
- Bond of Genies: a mythic ability to mount any party member!
### [Chevalier Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/c-d/chevalier/)
- The class has only 3 levels.
### [Halfling Opportunist Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/e-h/halfling-opportunist/)
- Thanks to bittercranberry for his aid another and Excellent Aid (in next update of [Expanded Content](https://github.com/ka-dyn/ExpandedContent) which it requires)!
#### Homebrew Options:  
- Alternative access to the class for non-halflings!
- A mythic feat to use another combat maneuver with Exploitive Maneuver.
### [Horizon Walker Class](https://www.d20pfsrd.com/classes/prestige-classes/apg/horizon-walker/)
- The Terrain Mastery abilities of Astral Plane and Abyss are actually useless in the module but they are prerequisites for their Terrain Dominance.
- There isn't a GM available telling which creature is native to where, so ONLY ABYSS's favored enemy bonus works, for demons (which is written explicitly in the its description). And you guys probably won't focus on other creatures anyway lol
### [Hinterlander Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/e-h/hinterlander/)
- Master Archer gives access to [Deadeye’s Blessing in TTT](https://github.com/Vek17/TabletopTweaks-Base) and [Pinpoint Targeting in ExpandedContent](https://github.com/ka-dyn/ExpandedContent), but you'll need the respective mod active to select them. If you don't want these feats, my mod doesn't require anything, as the original feature will still be there.
- Gain a new Favored Terrain at level 2, 6, and an upgrade at level 9, 10. No need to study for 24 hours!
### [Inheritor’s Crusader Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/i-m/inheritor-s-crusader/)
- The class has only 3 levels.
#### Homebrew Options:  
- Spells per day/Destroyer of Tyranny/Sword Against Injustice: Now warpriest-friendly!
### [Mammoth Rider](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/i-m/mammoth-rider/)
- Gigantic Steed gives stackable bonus instead of size bonus. It just doesn't make any sense that this can't stack with magical enlarge effect!
- Steed’s Reach is togglable, because longer reach might have negative impact in the game.
#### Homebrew Options:  
- Gigantic Assault: a mythic ability to charge like a beast!
### [Sanguine Angel Class (Lite)](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/s-z/sanguine-angel/)
- Require [Dark Codex](https://github.com/Truinto/DarkCodex), or you won't gain Prodigious Two-Weapon Fighting.
- The class has 10 levels, but I only implement the first 3 levels because it doesn't have good stuff at higher levels.
#### Homebrew Options:  
- Become an Angel of Death early! (require precise shot)
### [Scar Seeker Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/s-z/scar-seeker/)   
- Gain spell progression at level 2, 4, 6, 8, 10.
### [Shadowdancer Class](https://www.d20pfsrd.com/classes/prestige-classes/core-rulebook/shadowdancer/)   
#### Homebrew Options:  
- Summon Shadow: You can choose an animal companian instead and make it undead!
- Shadow Jump: You can choose any feat from the dimentional feat chain instead! This requires [Microscopic Content Expansion mod](https://github.com/alterasc/MicroscopicContentExpansion/), which requires [TabletopTweaks-Core](https://github.com/Vek17/TabletopTweaks-Core). If you don't want these feats, my mod doesn't require anything, as the original feature will still be there.
- Extra Shadow Jump: This is a homebrew feat that gives you 4 extra uses of Shadow Jump ability and you can take it as soon as shadowdancer level 1!
### [Umbral Agent Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/s-z/umbral-agent/)   
#### Homebrew Options:  
- Endless Gaze: a mythic ability to use Unnerving Gaze for free!
- Shadow Jump: You can choose any feat from the dimentional feat chain instead! This requires [Microscopic Content Expansion mod](https://github.com/alterasc/MicroscopicContentExpansion/), which requires [TabletopTweaks-Core](https://github.com/Vek17/TabletopTweaks-Core). If you don't want these feats, my mod doesn't require anything, as the original feature will still be there.
- Extra Shadow Jump: This is a homebrew feat that gives you 4 extra uses of Shadow Jump ability and you can take it as soon as umbral agent level 1!
### [Dragon Fury Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/s-z/scar-seeker/)   
- Learn 3 Thrashing Dragon stances!
- The class has more stuff on tabletop but it's enough of a power house atm :)
- From Path of War

### New Archetypes
#### Fighter:  
- [Eldritch Guardian](https://www.aonprd.com/ArchetypeDisplay.aspx?FixedName=Fighter%20Eldritch%20Guardian)
- Share Training with your pets!
- [Lore Warden](https://www.aonprd.com/ArchetypeDisplay.aspx?FixedName=Fighter%20Lore%20Warden) (preserving pros of [this version](https://www.aonprd.com/ArchetypeDisplay.aspx?FixedName=Fighter%20Lore%20Warden%20(PFS%20Field%20Guide)))
- [Warlord](https://www.aonprd.com/ArchetypeDisplay.aspx?FixedName=Fighter%20Warlord)

### New Style Feats

- [Black Seraph Style](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraph-style-combat-style/): From Path of War
- [Black Seraph’s Malevolence](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraphs-malevolence-combat/)
- [Black Seraph Annihilation](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraph-annihilation-combat/)
- [Grabbing Style](https://www.d20pfsrd.com/feats/combat-feats/grabbing-style-combat-style/)
- Grabbing Drag (homebrew: drag effect)
- Grabbing Master (homebrew: bypass immunity)
- [Charging Stag Style](https://www.d20pfsrd.com/feats/combat-feats/charging-stag-style-combat-style/)
- [Stag Horns](https://www.d20pfsrd.com/feats/combat-feats/stag-horns-combat-style/)
- [Stag Submission](https://www.d20pfsrd.com/feats/combat-feats/stag-submission-combat-style/)
- [Snapping Turtle Style](https://www.d20pfsrd.com/feats/combat-feats/snapping-turtle-style-combat-style/)
- [Snapping Turtle Clutch](https://www.d20pfsrd.com/feats/combat-feats/snapping-turtle-clutch-combat/)
- [Snapping Turtle Shell](https://www.d20pfsrd.com/feats/combat-feats/snapping-turtle-shell-combat/)
- [Kraken Style](https://www.d20pfsrd.com/feats/combat-feats/kraken-style-combat-style/)
- [Kraken Wrack](https://www.d20pfsrd.com/feats/combat-feats/kraken-wrack-combat-style/)
- [Medusa’s Wrath](https://www.d20pfsrd.com/feats/combat-feats/medusa-s-wrath-combat/)
- Use Kraken Style Feats as prerequisites since they are all tentacle-like.
### New Feats

- [Ripple in Still Water](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/ripple-in-still-water-combat/): From Path of War
- [Lesser Spell Synthesis](https://www.d20pfsrd.com/feats/general-feats/lesser-spell-synthesis/)
- Where is Spell Synthesis? In [Mystical Mayhem](https://gitgud.io/Kreaddy/mysticalmayhem/)!
- [Surprising Strategy](https://www.d20pfsrd.com/feats/combat-feats/surprising-strategy-combat/)
### New Rogue Talents

- [Another Day](https://www.d20pfsrd.com/classes/core-classes/rogue/rogue-talents/paizo-rogue-advanced-talents/another-day-ex/)
### New Mythic Prestige Class Features

- Swift Death: Your Death Attack becomes a standard action. At 10th assassin level, it becomes a move action.
- Greater Swift Death: Your Death Attack becomes a move action. At 10th assassin level, it becomes a swift action.
- Unbreakable Defense: You no longer have a limited amount of Defensive Stance rounds per day.
- Draconic Wings: The disciple gains two primary wing attacks that deal 1d4 damage.
- Endless Breath: You have 1/3 chance to recover one use of your breath weapon at the start of your turn. Additionally, your Gold Dragon Breath (Mythic) becomes a move action.
- Riposte (Mythic): The duelist adds her mythic tier as a bonus on her attack roll when attempting a parry and riposte. This bonus does not stack with Ever Ready mythic feat.
- Metaphysical Sneak Attack: If the student of war deals sneak attack damage to a target, he can study that target, allowing him to apply his Know Your Enemy effects. Additionally, his sneak attack no longer deals precision damage.
- Mystic Catalyst: You can use Lesser Spell Synthesis ability at will.

## Thanks to  
-   WittleWolfie for his [Blueprint-Core](https://wittlewolfie.github.io/WW-Blueprint-Core/index.html), which makes modding easy and enjoyable.
-   Bubbles (factsubio) for BubblePrints, saving me from going mad.   
-   All the Owlcat modders who came before me, wrote documents, and open sourced their code.
-   Pathfinder Wrath of The Righteous Discord channel members.
-   Join our [Discord](https://discord.com/invite/wotr)
