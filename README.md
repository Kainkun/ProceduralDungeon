# ProceduralDungeon

This is a coding experiment in procedural dungeon generation where every room has three doors that lead to rooms that also has three doors and so on. I wanted to work with seed based procedural generation where each room is always the same even if you leave and return, similar to minecraft or no man's sky. This system creates a system of millions of unique, interconnected rooms that stay consistent.
I made a Room Data class which stores where the room is in the world. The Room Data class has a custom get hash code function that is used for the seed that is used for consistent generation. Rooms where generated with random walk.
