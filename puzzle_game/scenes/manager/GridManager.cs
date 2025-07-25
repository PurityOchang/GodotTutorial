using System.Collections.Generic;
using System.Linq;      // Linq is a set of helper functioinallity for iterating over and manipulating collections of data built into c#
using Game.Autoload;
using Game.Component;   // so use stuff from the building component file
using Godot;

// all grid related items would go to grid items.
// Our ultimate goal of the grid manager is to provide the data about the current state of the grid as well as providing grid highlights.
// Though we could separate them we just want to put them together for now

namespace Game.Manager;  // this is a way not to clutter your namespaces

public partial class GridManager : Node
{
    private HashSet<Vector2I> occupiedCells = new();  // new() is an alternate way of saying new Hashset<Vector2>

    [Export] // export attribute exposes the member we define in the godot inspector for the grid manager scene. You see this on godot 
             // as part of its metadata
    private TileMapLayer highlightTilemapLayer; // assigning this in grid manager in godot is basically an alternate way of getting a node
                                                // Get node is really only safe to do when you are guaranteed the structure of the scene.
    [Export]
    private TileMapLayer baseTerrainTilemapLayer; // this is the tile map layer that contains our terrain information. our sand our top most 
                                                  // tile map layer which we will get information about other tile map layers from here. 
                                                  // that is why it is our base.



    // overides the ready method
    public override void _Ready()
    {
        // when we emit a building placed signal, we are listenign for that always and then mark a tile a occupied
        GameEvents.Instance.BuildingPlaced += OnBuildingPlaced;  // we are configuring grid manager to listen the OnBuildingPlaced event
        //var gameEvents = GetNode<GameEvents>("/root/GameEvents");  //This is a valid way of getting an autoload node
    }


    // all logic is implemented here
    public bool IsTilePositionValid(Vector2I tilePosition) //c sharp the method return bool not boolean 
    {
        var customData = baseTerrainTilemapLayer.GetCellTileData(tilePosition);
        if (customData == null) return false;
        if (!(bool)customData.GetCustomData("buildable")) return false; // NOTE the type returned from GetCustomData is a variant which is a catch 
                                                                        // all type Godot makes use of if it doesn't know what type is supposed to be returned.
                                                                        // that is why we have to cast it ourselves.

        // returns true if a building can be placed on tile postion
        return !occupiedCells.Contains(tilePosition);
    }

    public void MarkTileAsOccupied(Vector2I tilePosition)
    {
        occupiedCells.Add(tilePosition);
    }


    // We fetch all our building components from this method.
    public void HighlightBuildableTiles()
    {
        ClearHighlightedTiles();
        // since get tree returns a node we cast is using the case method that takes a generic type
        // .cast is basically iterating over The nodes collection on converting each node in the collection to a building component
        // the variable buildingComponents now have a type IEnumerable<BuildingComponent>. basically a list of BuildingComponents
        var buildingComponents = GetTree().GetNodesInGroup(nameof(BuildingComponent)).Cast<BuildingComponent>();

        // We are iterating over the building components
        foreach (var buildingComponent in buildingComponents)
        {
            HighlightValidTilesInRadius(buildingComponent.GetGridCellPosition(), buildingComponent.BuildableRadius);
        }
    }

    public void ClearHighlightedTiles()
    {
        highlightTilemapLayer.Clear();
    }

    public Vector2I GetMouseGridCellPosition() // This method create so we do not repeat ourselves, when getting the mouse posion in the grid cell
    {
        var mousePosition = highlightTilemapLayer.GetGlobalMousePosition();
        var gridPosition = mousePosition / 64;
        gridPosition = gridPosition.Floor();
        return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
    }

    // by convention, but private methods below the public
    private void HighlightValidTilesInRadius(Vector2I rootCell, int radius)   //changed from public to private because it is being used as an internal method.
    {
        // rootCell will always be a valid value so no need to check if it is null
        for (var x = rootCell.X - radius; x <= rootCell.X + radius; x++)
        {
            for (var y = rootCell.Y - radius; y <= rootCell.Y + radius; y++)
            {
                var tilePosition = new Vector2I(x, y);
                if (!IsTilePositionValid(tilePosition)) continue;  // This would skip out on highlighting invalid tiles. 
                                                                   // continue just means to skip the current for loop iteration but go one with the rest of the for loop
                highlightTilemapLayer.SetCell(tilePosition, 0, Vector2I.Zero);
            }
        }
    }


    private void OnBuildingPlaced(BuildingComponent buildingComponent)
    {
        MarkTileAsOccupied(buildingComponent.GetGridCellPosition());
    }
}