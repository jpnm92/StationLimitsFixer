[b][color=gold]IMPORTANT FOR 1.4.1 UPDATE[/color][/b]
----------------------
If you are updating to version 1.2.2, I recommend you to delete the old config file.
It's not necessarily needed but the new default config file represents better default behavior for smelters, kilns, mills.

[size=4][b]Station Limits Fixer (FAFO Edition)[/b][/size]

[b]Disclaimer:[/b] This is my first mod. Usually I would say idgaf if your pc explodes but now I think I can safely say it won't. 
Also this mod is way better optimized now than it was.

[size=3][b]Why did I make this?[/b][/size]
Because I got tired of this dumbass game telling me there's no space when very obviously I HAVE THE FUCKING SPACE.

[size=3][b]What this actually does:[/b][/size]
[list]
[*][b]Universal Mod Compatibility:[/b] The mod dynamically scans every single loaded object in the game's memory. If you download another mod that adds a "Magic Bullshit Forge", this mod will automatically fix its range and hitboxes without me having to write a patch for it.
[*][b]Surgical Hitbox Shrinking:[/b] The mod makes workbench "improvements" physically tiny so you can stack them like sardines. However, it now features a "Smart Filter" that ignores trigger colliders, meaning you can still actually look at them and press 'E' to interact.
[*][b]The Escape Hatch Config:[/b] Sometimes shrinking a hitbox makes a piece look visually broken. Instead of me constantly updating the mod to fix edge cases, I added an ExcludedHitboxPieces config. If a piece breaks, just type its prefab name in the config, and the mod will ignore it. (The Black Forge Cooler and the Tanning Rack are excluded by default).
[*][b]Brute Force Smelters (FAFO Override):[/b] Enabled by default. Tired of the game telling you a Blast Furnace or Windmill needs perfectly flat dirt? This overrides the physics engine so you can place them anywhere. Bonus: It actually respects the game's Ward system, so you can't use this to bypass restrictions and drop a Smelter in the middle of your friend's living room.
[*][b]Brain-Dead Range:[/b] Extensions now connect from 25+ meters away. Put your upgrades in the basement, the attic, the garden or your mother—you know, as long as she's between the set distance in the config, the workbench doesn't care.
[*][b]Roof Needed (Optional):[/b] You can toggle whether your main crafting stations need a roof or not. The default is vanilla behavior (because sometimes you want to stay a little bit of a Viking).
[/list]

[size=3][b]Installation:[/b][/size]
1. Extract the DLL to your BepInEx/plugins folder or just install with r2modman like a normal person.

[size=3][b]Why am I so rude?[/b][/size]
I am honestly pissed off I couldn't find any mod that does what I just did. Also I kind of enjoy being an ass. It is what it is.

----------------------
[b]Source & Issues:[/b] [url=https://github.com/jpnm92/StationLimitsFixer]https://github.com/jpnm92/StationLimitsFixer[/url]

[size=3][b]Check out my other mod[/b][/size]
[b]InventoryYeeter:[/b] Instantly yeet your entire inventory with a single keypress. Panic button (Default: G) to jettison your heaviest non-equipped items until you are no longer encumbered. Perfect for rage-quitting or just cleaning up fast.

[list]
[*][url=https://thunderstore.io/c/valheim/p/jpnm92/InventoryYeeter/]InventoryYeeter on Thunderstore[/url]
[*][url=https://www.nexusmods.com/valheim/mods/2592]InventoryYeeter on NexusMods[/url]
[/list]