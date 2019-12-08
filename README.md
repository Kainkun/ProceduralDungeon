# ProceduralDungeon

This is a coding experiment in procedural dungeon generation where every room has three doors that lead to rooms that also has three doors and so on. I wanted to work with seed-based procedural generation where each room is always the same even if you leave and return, similar to Minecraft or No Man's Sky. This system creates a system of millions of unique, interconnected rooms that stay consistent.
I made a Room Data class which stores where the room is in the world. The Room Data class has a custom get hash code function that is used for the seed that is used for consistent generation.
Rooms were generated with a random walk algorithm. I also experimented with making a 2D explosive force.
