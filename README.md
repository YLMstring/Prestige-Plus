# Prestige Plus v1.7.3 for WoTR 
## Requirements: [ModMenu](https://github.com/WittleWolfie/ModMenu). [TTT-Core](https://github.com/Vek17/TabletopTweaks-Core).

- Add grapple mechanic and a bunch of prestige classes!
- Apart from some minor tweaks, all the homebrew stuff are optional.

## Featuring

### Combat Maneuver+
- You can Disarm, Sunder, or Trip in lieu of an melee attack, like on tabletop!
- [Seize the Opportunity](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/seize-the-opportunity-combat/): From Path of War
- [Surprise Maneuver](https://www.d20pfsrd.com/feats/general-feats/surprise-maneuver/)
- [Strength Surge](https://www.d20pfsrd.com/classes/core-classes/barbarian/rage-powers/paizo-rage-powers/strength-surge-ex/)
- [Ki Throw](https://www.d20pfsrd.com/feats/combat-feats/ki-throw-combat/) and its mythic version!
- [Improved Ki Throw](https://www.d20pfsrd.com/feats/combat-feats/improved-ki-throw-combat/)
- [Binding Throw](https://www.d20pfsrd.com/feats/combat-feats/binding-throw-combat/)
- [Enhanced Ki Throw](https://www.d20pfsrd.com/feats/combat-feats/enhanced-ki-throw-combat/)
- [Vindictive Fall](https://www.d20pfsrd.com/feats/general-feats/vindictive-fall/)
- [Ranged Trip](https://www.d20pfsrd.com/feats/combat-feats/ranged-trip-combat-targeting)
- [Ace Trip](https://www.d20pfsrd.com/feats/weapon-mastery-feats/ace-trip-targeting-weapon-mastery/): You don’t take the –2 penalty for making trip attempts with a ranged weapon using Ranged Trip, and you can attempt trip combat maneuver checks against anyone, even if the creature is immune to trip.
- [Cleaving Sweep](https://www.d20pfsrd.com/feats/combat-feats/cleaving-sweep-combat/)
- [Down Like Dominoes](https://www.d20pfsrd.com/alternative-rule-systems/mythic/mythic-heroes/mythic-paths-paizo-inc/trickster/): When you successfully trip a foe, as a free action you can attempt an additional trip attack against an adjacent creature at a –4 penalty. If this second trip is successful, you immediately move to its square and may continue to attempt to trip other creatures, taking a cumulative –4 penalty on each trip attempt after the first. In a round, you can trip a number of creatures equal to 1 + half your tier in this way.
- [Crush Armor](https://www.d20pfsrd.com/feats/combat-feats/weapon-trick-combat/)
- [Sunder Storm](https://www.d20pfsrd.com/alternative-rule-systems/mythic/mythic-heroes/mythic-paths-paizo-inc/champion/champion-path-abilities/sunder-storm-ex/): When you succeed at a sunder combat maneuver check, you drive the shattered pieces of the item into the flesh of its wearer, dealing 1d6 + a number of points equal to your tier of divine damage. Additionally, as a full-round action, you can attempt a sunder combat maneuver against each opponent within reach.
- [Tear Apart](https://www.d20pfsrd.com/alternative-rule-systems/mythic/mythic-heroes/mythic-paths-paizo-inc/champion/champion-path-abilities/tear-apart-ex/): Your sunder combat maneuver no longer disables armor, but rends it. If the check is successful, deal your weapon damage to the target and reduce the target’s armor bonus, natural armor bonus, or shield bonus by half your tier (minimum 1). If the creature has an enhancement bonus to the bonus you chose, reduce the normal bonus first, then apply any leftover reduction to the enhancement bonus. You can’t reduce the bonus below 0.
- [Quick Bull Rush](https://www.d20pfsrd.com/feats/combat-feats/quick-bull-rush-combat/)
- [Raging Throw](https://www.d20pfsrd.com/feats/general-feats/raging-throw/)
- [Hurricane Punch](https://www.d20pfsrd.com/feats/combat-feats/hurricane-punch-combat/)
- [Knockback](https://www.d20pfsrd.com/classes/core-classes/barbarian/rage-powers/paizo-rage-powers/knockback-ex/)
- [Drive Back](https://www.d20pfsrd.com/alternative-rule-systems/mythic/mythic-heroes/mythic-paths-paizo-inc/guardian/): As a full-round action, you can make one melee attack at your highest base attack bonus against each opponent within reach. You must make a separate attack roll against each opponent, then attempt a free bull rush combat maneuver check against each foe that you hit.
- [Combat Trickery](https://www.d20pfsrd.com/alternative-rule-systems/mythic/mythic-heroes/mythic-paths-paizo-inc/trickster/): Through buffoonery and deceit, you can trick opponents into moving where you want them. You can make a bull rush combat maneuver check to all adjacent opponents as a standard action, using your Bluff check modifier in place of your CMB.
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
- Credence gives access to [Dervish in TTT](https://github.com/Vek17/TabletopTweaks-Base).
#### Homebrew Options:  
- Blossoming Light: a mythic ability to use Solar Invocation for free!
### [Arcane Archer Class](https://www.d20pfsrd.com/classes/prestige-classes/core-rulebook/arcane-archer/)   
- A True Imbue Arrow is always with a quickened spell XD
- And we have [Deadeye Devotee](https://aonprd.com/ArchetypeDisplay.aspx?FixedName=Arcane%20Archer%20Deadeye%20Devotee)!
#### Homebrew Options:  
- Storm of Arrows: a mythic ability to increase the number of uses of Hail of Arrows!
### [Asavir Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/a-b/asavir/)   
- Share your pet with Djinni’s Blessing!
- Trample (Mythic): a mythic feat inspired by tabletop abilities of the same name! (notice that owlcat trample works differently from tabletop)
#### Homebrew Options:  
- Bond of Genies: a mythic ability to mount any party member!
### [Chevalier Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/c-d/chevalier/)
- The class has only 3 levels.
### [Esoteric Knight Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/e-h/esoteric-knight/)
#### Homebrew Options:  
- Dvine Psychic: continue divine spellcasting progression every 2 levels!
- Kinetic Esoterica: more versatile kineticist progression!
### [Halfling Opportunist Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/e-h/halfling-opportunist/)
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
- Feat: Extra Tyrant’s Discipline
### [Scar Seeker Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/s-z/scar-seeker/)   
- Gain spell progression at level 2, 4, 6, 8, 10.
### [Shadowdancer Class](https://www.d20pfsrd.com/classes/prestige-classes/core-rulebook/shadowdancer/)   
#### Homebrew Options:  
- Summon Shadow: You can choose an animal companian instead and make it undead!
- Shadow Jump: You can choose any feat from the dimentional feat chain instead! This requires [Microscopic Content Expansion mod](https://github.com/alterasc/MicroscopicContentExpansion/). If you don't want these feats, my mod doesn't require anything, as the original feature will still be there.
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
#### Magus:  
- [Spire Defender](https://www.d20pfsrd.com/classes/base-classes/magus/archetypes/paizo-magus-archetypes/spire-defender/)
- New Magus Arcana: [Maneuver Mastery](https://www.d20pfsrd.com/classes/base-classes/magus/magus-arcana/paizo-magus-arcana/maneuver-mastery-ex)
#### Witch:  
- [White-Haired Witch](https://www.d20pfsrd.com/classes/base-classes/witch/archetypes/paizo-witch-archetypes/white-haired-witch/)
- This is absolutely my favorite.

### New Style Feats

- [Black Seraph Style](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraph-style-combat-style/): From Path of War
- [Black Seraph’s Malevolence](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraphs-malevolence-combat/)
- [Black Seraph Annihilation](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraph-annihilation-combat/)
- [Grabbing Style](https://www.d20pfsrd.com/feats/combat-feats/grabbing-style-combat-style/)
- Grabbing Drag (homebrew): You can use a move action to attempt a grapple combat maneuver against a creature within 30 feet to pull it towards you. This movement as well as the ability itself provoke attack of opportunity.
- Grabbing Master (homebrew): Your grapple may target anyone, even if the creature is immune to grapple.
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
- [Smashing Style](https://www.d20pfsrd.com/feats/combat-feats/smashing-style-combat-style/)
- [Stick-Fighting Counter](https://www.d20pfsrd.com/feats/combat-feats/stick-fighting-counter-combat/)
- [Stick-Fighting Maneuver](https://www.d20pfsrd.com/feats/combat-feats/stick-fighting-maneuver-combat/)
### New Feats

- [Ripple in Still Water](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/ripple-in-still-water-combat/): From Path of War
- [Lesser Spell Synthesis](https://www.d20pfsrd.com/feats/general-feats/lesser-spell-synthesis/)
- Where is Spell Synthesis? In [Mystical Mayhem](https://gitgud.io/Kreaddy/mysticalmayhem/)!
- [Surprising Strategy](https://www.d20pfsrd.com/feats/combat-feats/surprising-strategy-combat/)
- [Bodyguard](https://www.d20pfsrd.com/feats/combat-feats/bodyguard-combat) (owlcat-brew)
- [In Harm’s Way](https://www.d20pfsrd.com/feats/combat-feats/in-harm-s-way-combat/) (owlcat-brew)
- [Divine Defiance](https://www.d20pfsrd.com/feats/general-feats/divine-defiance) 
- [Iconoclast](https://www.d20pfsrd.com/feats/combat-feats/iconoclast-combat/)
- [Rhino Charge](https://www.d20pfsrd.com/feats/combat-feats/rhino-charge-combat/)
- [Minotaur’s Charge](https://www.d20pfsrd.com/feats/general-feats/minotaur-s-charge/)
### New Rogue Talents

- [Another Day](https://www.d20pfsrd.com/classes/core-classes/rogue/rogue-talents/paizo-rogue-advanced-talents/another-day-ex/)
- [Cloying Shades](https://www.d20pfsrd.com/classes/core-classes/rogue/rogue-talents/paizo-rogue-talents/cloying-shades-su)
- [Quick Shot](https://www.d20pfsrd.com/classes/unchained-classes/rogue-unchained/rogue-talents-advanced/paizo-rogue-talents-advanced/quick-shot)
- [Sneaky Maneuver](https://www.d20pfsrd.com/classes/core-classes/rogue/rogue-talents/paizo-rogue-talents/sneaky-maneuver-ex/)
### New Mythic Prestige Class Features

- Swift Death: Your Death Attack becomes a standard action. At 10th assassin level, it becomes a move action.
- Greater Swift Death: Your Death Attack becomes a move action. At 10th assassin level, it becomes a swift action.
- Unbreakable Defense: You no longer have a limited amount of Defensive Stance rounds per day.
- Draconic Wings: The disciple gains two primary wing attacks that deal 1d4 damage.
- Endless Breath: You have 1/3 chance to recover one use of your breath weapon at the start of your turn. Additionally, your Gold Dragon Breath (Mythic) becomes a move action.
- Riposte (Mythic): The duelist adds her mythic tier as a bonus on her attack roll when attempting a parry and riposte. This bonus does not stack with Ever Ready mythic feat.
- Metaphysical Sneak Attack: If the student of war deals sneak attack damage to a target, he can study that target, allowing him to apply his Know Your Enemy effects. Additionally, his sneak attack no longer deals precision damage.
- Mystic Catalyst: You can use Lesser Spell Synthesis ability at will.
### Quality of Life Features

- Estimated THC: Show hit chance and spell (not) saved chance in turn based mode. This is not precise because it doesn't take into consideration certain effects such as concealment, spell penetration or conditional save bonus, but still better than nothing, eh? (default off)
- Infernal Healing: You can fully heal outside of combat by spending gold, as if you buy a wand of infernal healing and use it! (gold : hp = 3:2)

## Mod Combo

### [Expanded Content](https://github.com/ka-dyn/ExpandedContent)
- Bitter adds [aid another](https://www.aonprd.com/Rules.aspx?Name=Aid%20Another&Category=Special%20Attacks) related stuff to my Asavir and Halfling Opportunist!
### [Holy Vindicator](https://github.com/SpencerMycek/WoTR-HolyVindicator)
- Spellbook Progression - Fixed
- Divine Wrath, Divine Judgment, Divine Retribution!

## Thanks to  
-   WittleWolfie for his [Blueprint-Core](https://wittlewolfie.github.io/WW-Blueprint-Core/index.html), which makes modding easy and enjoyable.
-   Bubbles (factsubio) for BubblePrints, saving me from going mad.   
-   All the Owlcat modders who came before me, wrote documents, and open sourced their code.
-   Pathfinder Wrath of The Righteous Discord channel members.
-   Join our [Discord](https://discord.com/invite/wotr)
