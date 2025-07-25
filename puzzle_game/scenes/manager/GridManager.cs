using System.Collections.Generic;
using Godot;

// all grid related items would go to grid items.
// Our ultimate goal of the grid manager is to provide the data about the current state of the grid as well as providing grid highlights.
// Though we could separate them we just want to put them together for now

namespace Game.Manager;  // this is a way not to clutter your namespaces

public partial class GridManager : Node
{
    private HashSet<Vector2> occupiedCells = new();  // new() is an alternate way of saying new Hashset<Vector2>

    [Export] // export attribute exposes the member we define in the godot inspector for the grid manager scene. You see this on godot 
             // as part of its metadata
    private TileMapLayer highlightTilemapLayer; // assigning this in grid manager in godot is basically an alternate way of getting a node
                                                // Get node is really only safe to do when you are guaranteed the structure of the scene.
    [Export]
    private TileMapLayer baseTerrainTilemapLayer; // this is the tile map layer that contains our terrain information. our sand our top most 
                                                  // tile map layer which we will get information about other tile map layers from here. 
                                                  // that is why it is our base.

    public override void _Ready()
    {

    }

    // all logic is implemented here
    public bool IsTilePositionValid(Vector2 tilePosition) //c sharp the method return bool not boolean 
    {
        // returns true if a building can be placed on tile postion
        return !occupiedCells.Contains(tilePosition);
    }

    public void MarkTileAsOccupied(Vector2 tilePosition)
    {
        occupiedCells.Add(tilePosition);
    }


    public void HighlightValidTilesInRadius(Vector2 rootCell, int radius)
    {
        // rootCell will always be a valid value so no need to check if it is null
        highlightTilemapLayer.Clear();

        for (var x = rootCell.X - radius; x <= rootCell.X + radius; x++)
        {
            for (var y = rootCell.Y - radius; y <= rootCell.Y + radius; y++)
            {
                if (!IsTilePositionValid(new Vector2(x, y))) continue;  // This would skip out on highlighting invalid tiles. 
                                                                        // continue just means to skip the current for loop iteration but go one with the rest of the for loop
                highlightTilemapLayer.SetCell(new Vector2I((int)x, (int)y), 0, Vector2I.Zero);
            }
        }
    }

    public void ClearHighlightedTiles()
    {
        highlightTilemapLayer.Clear();
    }

    public Vector2 GetMouseGridCellPosition() // This method create so we do not repeat ourselves, when getting the mouse posion in the grid cell
    {
        var mousePosition = highlightTilemapLayer.GetGlobalMousePosition();  // returns a vector2 that is the x and y postion of the mouse
        var gridPosition = mousePosition / 64;
        gridPosition = gridPosition.Floor();   // so we do not get fractional positions
        return gridPosition;
    }
}