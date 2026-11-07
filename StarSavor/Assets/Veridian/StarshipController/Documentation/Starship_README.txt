

====================================== Starship Controller
This asset contains the core controller scripts (Player, AI, and Core) used in the Starship demo scenes. 
It is designed to be a central dependency for several related assets.

Documentation
The complete and most up-to-date documentation for the Starship Controller system, including component breakdowns and setup guides, is available online.

Link: https://docs.google.com/document/d/1bK8_wqpfBkHQ05B-Yrv6QLEylCNeUeeQzIWG-dmVrLk/edit?usp=sharing

Assembly Structure
The scripts are organized into three distinct assembly definitions (.asmdef) with a clear dependency structure to ensure modularity.

Player: Contains scripts for the player-controlled ship (e.g., PlayerShipDriver). It references the Core assembly.

AI: Contains scripts for the AI-piloted ships (e.g., AiBrain, SimpleAiPilot). It also references the Core assembly.

Core: Contains the central logic, physics, and interfaces (e.g., AtmosphericStarshipController, IShipDriver). It has no other dependencies in this asset.

The Player and AI assemblies do not reference each other.

Dependencies & Demo Scenes
This controller asset is self-contained. If you are using other assets from this collection (e.g., environment packs, ship models) but not the demo scenes, 
this controller asset can be safely deleted without causing errors.

IMPORTANT: The included demo scene(s) require this asset to function. 
If you delete this Starship Controller asset, the demo scene(s) will fail to compile and should also be deleted.


Contact me if you have (reasonable) questions about this free asset, or if you find bugs or encounter errors. 
trevor.keiber@gmail.com