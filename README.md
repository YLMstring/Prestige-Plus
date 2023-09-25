# Prestige Plus v1.2.0 for WoTR 
## Requirements: [ModMenu](https://github.com/WittleWolfie/ModMenu). [TTT-Core](https://github.com/Vek17/TabletopTweaks-Core).

- Add Grapple Mechanic!
- Add Agent of the Grave, Arcane Archer, Asavir, Deadeye Devotee, Chevalier, Hinterlander, Horizon Walker, Inheritor’s Crusader, Mammoth Rider, Sanguine Angel, Scar Seeker, Shadowdancer, Umbral Agent prestige class. More coming soon!
- Apart from some minor tweaks, all the homebrew stuff are optional.
- And they are togglable now!

## Featuring

### [Grapple Mechanic](https://www.d20pfsrd.com/gamemastering/Combat/#Grapple)
- This is how I implement it:
1. There are 3 kinds of grapples in the game. My Grapple Mechanic is very close to tabletop. Shifter grapples stay owlcat-brew but once the shifter takes Grapple (Improved Grapple) feat, their grapple mechanics would be turned into mine. Finally, monster grapples stay the way it is in Vanilla.
2. If you are grappled, you can attempt to break the grapple by making a combat maneuver check (DC equal to your opponent’s CMD; this does not provoke an attack of opportunity) or Thievery check (with a DC equal to your opponent’s CMD). If you succeed, you break the grapple as a standard action. However, if you fail nothing happens, you don't use your standard action.
3. Tied Up creatures still need to be grappled. The rope would be bursted if you release grapple, because you don't have time to tie someone up properly in combat. They are not helpless but can become the target of Coup De Grace. Additionally, grapple actions against them auto-succeed, they're auto pinned by the rope and their Thievery check DC to escape is higher.
- Added Feats: Grapple (Improved Grapple), Greater Grapple, Rapid Grappler, Unfair Grip, Pinning Knockout, Pinning Rend, Savage Slam, Dramatic Slam, Hamatula Strike
- Added Mythic Feats and Abilities (inspired by tabletop abilities of the same name): Grapple (Mythic), Uncanny Grapple, Aerial Assault, Knot Expert, Meat Shield, Maneuver Expert
### [Agent of the Grave Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/a-b/agent-of-the-grave/)
- The class has only 5 levels.
- Agent of the Grave lose spell progression at level 1. I have no idea why my code can make you lose spell progression, but it just works.
- Hide your alignment!
#### Homebrew Options:  
- At level 5 you can learn all necromancy spells (require 13 int, full caster)
- Or become a ghoul! (you're undead so incompatible with lichdom)
- Or become a vampire! (require blood drinker, incompatible with lichdom)
### [Arcane Archer Class](https://www.d20pfsrd.com/classes/prestige-classes/core-rulebook/arcane-archer/)   
- A True Imbue Arrow is always with a quickened spell XD
- And we have [Deadeye Devotee](https://aonprd.com/ArchetypeDisplay.aspx?FixedName=Arcane%20Archer%20Deadeye%20Devotee)!
#### Homebrew Options:  
- Storm of Arrows: a mythic ability to increase the number of uses of Hail of Arrows!
### [Asavir Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/a-b/asavir/)   
- Thanks to bittercranberry for his aid another and camaraderie (in the next [EC update](https://github.com/ka-dyn/ExpandedContent) which it requires)!
- Share your pet with Djinni’s Blessing!
- Trample (Mythic): a mythic feat inspired by tabletop abilities of the same name! (notice that owlcat trample works differently from tabletop)
#### Homebrew Options:  
- Bond of Genies: a mythic ability to mount any party member!
### [Chevalier Class](https://www.d20pfsrd.com/classes/prestige-classes/other-paizo/c-d/chevalier/)
- The class has only 3 levels.
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
  
### New Style Feats

- [Black Seraph Style](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraph-style-combat-style/)
- [Black Seraph’s Malevolence](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraphs-malevolence-combat/)
- [Black Seraph Annihilation](https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/feats/black-seraph-annihilation-combat/)
- From Path of War, Make them Fear!
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
### New Rogue Talents

- [Defensive Roll](https://www.d20pfsrd.com/classes/core-classes/rogue/rogue-talents/paizo-rogue-advanced-talents/defensive-roll-ex/)
- I homebrew it to work like a one-time last stand, so at least it looks like an advanced talent :)

## Thanks to  
-   WittleWolfie for his [Blueprint-Core](https://wittlewolfie.github.io/WW-Blueprint-Core/index.html), which makes modding easy and enjoyable.
-   Bubbles (factsubio) for BubblePrints, saving me from going mad.   
-   All the Owlcat modders who came before me, wrote documents, and open sourced their code.
-   Pathfinder Wrath of The Righteous Discord channel members.
-   Join our [Discord](https://discord.com/invite/wotr)
