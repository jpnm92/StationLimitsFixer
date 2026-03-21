# Station Limits Fixer (FAFO Edition)

**Disclaimer:** This is my first mod. It is the direct result of me "fucking around and finding out" because Valheim's building restrictions are..*thinking of a nice way..* bullshit! If it breaks your game, you probably shouldn't have trusted a guy who just learned what a BoxCollider was yesterday. 

### Why did I make this?
Because I got tired of this dumbass game telling me there's no space when very obviously I HAVE THE FUCKING SPACE.
I also found variables with typos—literally `m_continousConnection` (missing the 'u')—and hitboxes that make absolutely no sense. I got tired of the game telling me I "need more space" for an Adze or a chopping tree block or 90% of the other improvements when I have an entire fucking continent to work with.

### What this actually does:
* **Surgical Hitbox Shrinking:** Instead of making objects invisible (which I did by accident, you're welcome), this mod makes workbench improvements" "skinny." They keep their height and depth so they snap to walls/floors, but their width is tiny so you can stack them like sardines.
* **Brain-Dead Range:** Extensions now connect from 25+ meters away. Put your upgrades in the basement, the attic, the garden or your mother, you know as long as she's between the set distance in the config the workbench doesn't care.
* **Roof needed optional** Made this optional in the config. The default is vanilla behavior.
* **Brute Force Placement (FAFO Edition):** Added a toggle that tells the physics engine to go touch grass. Allows placing Smelters, Kilns, Blast Furnaces, and Windmills ANYWHERE, ignoring height and terrain restrictions.
* **Universal Support:** This doesn't just fix the vanilla game. It uses a "Universal Logic" to find and fix addons from other mods (like OdinShip or CoreWoodPieces) automatically.

### Installation:
1. Extract the DLL to your `BepInEx/plugins` folder or just install with r2modman

### Config:
*Use R2Modman to edit the config or notepad++ if you're a nerd or notepad if you're a degenerate or just use some ConfigurationManager mod if you're a normal person*

**ImprovementHitboxSize**
	Size of the physical footprint. 0.1 is tiny.
	Setting type: Single
	Default value: 0.6
**MaxRange**
	Maximum distance between a crafting station and its improvements.
	Setting type: Single
	Default value: 25
**NoRoofRequired**
	If true, main crafting stations work even without a roof or in the rain.
	Setting type: Boolean
	Default value: false
**ForceSmelterPlacement**
	If true, Smelters, Kilns, and Windmills can be placed ANYWHERE, ignoring terrain rules.
	Setting type: Boolean
	Default value: true