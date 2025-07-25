using Game.Autoload;
using Godot;

namespace Game.Component;

// This building component has functionality and data configuration related to properties of a building
public partial class BuildingComponent : Node2D
{
    [Export]
    public int BuildableRadius { get; private set; } // this getter setter method is public for retrieving 
                                                     // a method, but private for setting an attribute.

    public override void _Ready()
    {
        // Know what adding to group does. the name of function. takes the string name of building component and 
        // adds the string name of building component do a group. It is also tide to the node we are trying to create
        AddToGroup(nameof(BuildingComponent));  // you can do this on godot but is is better here since you do not have 
                                                // to remmember all the group names.
        GameEvents.EmitBuildingPlaced(this); // we are passing a reference to ourselves so game events knows when to emit signals
    }

    public Vector2I GetGridCellPosition()
    {
        var gridPosition = GlobalPosition / 64;  // since BuildingComponent is a node 2d is does not have a mouse postion, but a global
                                                // postion as an attribute.
        gridPosition = gridPosition.Floor();   // so we do not get fractional positions
        return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
    }

}
