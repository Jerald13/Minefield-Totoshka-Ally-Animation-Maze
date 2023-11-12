# Minefield-Totoshka-Ally-Animation-Maze

![image](https://github.com/Jerald13/Minefield-Totoshka-Ally-Animation-Maze/assets/72396726/d5a85e00-d800-44d2-92da-45366d19decb)

Bug that exist:

1) when "T" get surround by moved position before. "T" stop searching new path.

![image](https://github.com/Jerald13/Minefield-Totoshka-Ally-Animation-Maze/assets/72396726/3d763b3e-abbe-4786-b4c2-098074c00c55)

2) when "T" get blocked by surround "X" and "A". "T" stop searching new path.

Current ways to fix it:
1) "T" should record every available move surround itself that haven't check before. (Question mention "T" can smell any adjacent(Around itself) field has a bomb)
   When "T" get stuck by one of the bug, "T" able to reversed back to search for position that recorded avaialble move but haven't check yet.
2) when "T" get blocked by surround "X" and "A". "A" should reversed back position that "T" check before so that allow "T" to search for a recorded available move that haven't check before and continue at there.

There will be 2 List to record
    static List<Tuple<int, int>> totoPath = new List<Tuple<int, int>>();  <- allow reversed back and "A" follow "T"
    static List<Tuple<int, int>> availableMoves = new List<Tuple<int, int>>(); <- record down surround "T" have a "." 

here is the priority move "T" will be moving. for cases that have 2 path and "T" have to decide one path to check. (The question did not mention "T" know the path, so i make "T" check one of the path"
        { 1, 0 },     // Down
        { 1, 1 },     // Down-Right
        { 1, -1 },    // Down-Left
        { 0, 1 },     // Right
        { -1, 0 },    // Up
        { 0, -1 } ,    // Left
        { -1, 1 },    // Up-Right
        { -1, -1 }   // Up-Left
