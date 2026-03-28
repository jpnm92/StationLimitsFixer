
# Station Limits Fixer (FAFO Edition)

**Disclaimer:** This is my first mod. It is the direct result of me "fucking around and finding out" because Valheim's building restrictions are... bullshit! If it breaks your game, you probably shouldn't have trusted a guy who just learned what a BoxCollider was yesterday (It's probably okay though, my mod doesn't add anything, only edits, so even if you uninstall mid playthrough maybe you'll have to replace some workstation or improvement of the same).

## Why did I make this?
Because I got tired of this dumbass game telling me there's no space when very obviously I HAVE THE FUCKING SPACE.

I also found variables with typos—literally `m_continousConnection` (missing the 'u')—and hitboxes that make absolutely no sense. I got tired of the game telling me I "need more space" for an Adze or a chopping tree block or 90% of the other improvements when I have an entire fucking continent to work with.

## What this actually does:
- **Surgical Hitbox Shrinking:** Instead of making objects invisible (which I did by accident, you're welcome), this mod makes workbench "improvements" skinny. They keep their height and depth so they snap to walls/floors, but their width is tiny so you can stack them like sardines.
- **Brain-Dead Range:** Extensions now connect from 25+ meters away. Put your upgrades in the basement, the attic, the garden or your mother—you know, as long as she's between the set distance in the config, the workbench doesn't care.
- **Roof Needed (Optional):** Made this optional in the config. The default is vanilla behavior (because sometimes you want to stay a little bit of a Viking).
- **Targeted Support:** Fixes the core vanilla pieces that fight you the most (Workbench, Forge, Black Forge, Galdr Table, and all their annoying friends).
- **2.1 Change:** The Piece Workbench Ext2 is now excluded from hitbox shrinking for better visuals and compatibility.

## Installation:
1. Extract the DLL to your BepInEx/plugins folder or just install with r2modman like a normal person.

## Why am I so rude?
I am honestly pissed off I couldn't find any mod that does what I just did. Also I kind of enjoy being an ass. It is what it is.

---
**Source & Issues:** [https://github.com/jpnm92/StationLimitsFixer](https://github.com/jpnm92/StationLimitsFixer)

## Check out my other mod

**InventoryYeeter**: Instantly yeet your entire inventory with a single keypress. Perfect for rage-quitting or just cleaning up fast.

- [InventoryYeeter on Thunderstore](https://thunderstore.io/c/valheim/p/jpnm92/InventoryYeeter/)
- [InventoryYeeter on NexusMods](https://www.nexusmods.com/valheim/mods/2592)