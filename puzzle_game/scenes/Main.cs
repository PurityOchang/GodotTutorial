// NOTE: YOU CAN ALWAYS GO TO GODOT HELP FOR HOW AN OBJECT FUNCTIONS 
using Godot;  // This is importing everything from the Godot name space. similar to a module
using System;

// A c sharp namespace is a way of categorizing or organizing different files in the solution
// a namespace is used when you are importing using the using keyword
namespace Game;  // The main namespace we use for our own files that are separate or extend godot functionality


public partial class Main : Node2D
{
    private Sprite2D cursor;
    private PackedScene buildingScene;   // a packed scene is the date requiered to instantiate a scene.
    private Button placeBuildingButton;

    //Called when the node enters the scene tree for the first time.
    // This ready method is called after the ready methods of all its children have been called. all decendents get called first b4 root basically saying they are configured and ready to go
    public override void _Ready()
    {
        buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");  // Load is for loading any type of node
        cursor = GetNode<Sprite2D>("Cursor");  // Rememeber in heritance form 270 you cannot equate a more specific type to a less
                                               // specific one. Getnode returns a generic node sprite is a Sprite2D. Remember the heirarchy.
                                               // The angular brackets supply Sprite2D as the generic type.  Cursor/Node2D -this is the node path of a child of cursor.
                                               // You can also get node path by right clicking the desired node on godot and clicking copy path.
                                               // the string over there is the node path. This is the string representation of how get node can locate
                                               // the node we are trying to target in the scene tree. GetNode always happens relative to the current node we are calling it from.
                                               // when there is no leading forward slash. Saying in relatioin to main find the node called sprite 2d.
        placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

        cursor.Visible = false;

        placeBuildingButton.Pressed += OnButtonPressed;   // a way we can connect to signals godot ensourages but there are problems associated with it.
        //placeBuildingButton.Connect(Buttton.SignalName.Pressed, Callable.From(OnButtonPressed));   a more foolproof name to connect to signals
    }

    public override void _UnhandledInput(InputEvent evt) // overide not untop like in jave but after public can't use event as a variable name because it is a cSharp keyword
    {
        if (cursor.Visible && evt.IsActionPressed("left_click"))
        {
            PlaceBuildingAtMousePosition();
            cursor.Visible = false;
        }
    }

    // Called every frame. 'delta' is the elapsed tiime since the previous frame.
    public override void _Process(double delta)   // This delta passes the amount of time since the last frame
    {
        var gridPosition = GetMouseGridCellPosition();
        cursor.GlobalPosition = gridPosition * 64;  // GlobalPostion is the postion of the node 2d in the world.
                                                    // there is another property called postion which is its relative postion to its parent.
                                                    // We are doing this (multipltying by 64) so the pixel positon of the sprite is directly aligned with the grid.

    }

    private Vector2 GetMouseGridCellPosition() // This method create so we do not repeat ourselves, when getting the mouse posion in the grid cell
    {
        var mousePosition = GetGlobalMousePosition();  // returns a vector2 that is the x and y postion of the mouse
        var gridPosition = mousePosition / 64;
        gridPosition = gridPosition.Floor();   // so we do not get fractional positions
        return gridPosition;
    }

    private void PlaceBuildingAtMousePosition()
    {
        var building = buildingScene.Instantiate<Node2D>(); // this takes the data representation of building scene (building.tscn from earlier)and Instantiate would go through all that data
                                                            // and create every single node such that we now have a node we could add to the scene tree. hovering over building, we see has a type of Node
        AddChild(building); // Add child adds the child of whatever node we are currently on. and in this case, it is main. As a child of main, we are adding the building node.
                            // building contains the root node and sprite 2d. However, by default, when we add to our scene, it has a postion 0,0
        var gridPosition = GetMouseGridCellPosition();
        building.GlobalPosition = gridPosition * 64; // NOTE when instantiating variables/objects always pass in the most specific type as a generic type so certain methods can be used on them. 
                                                     // find out if the above is a c# thing or a godot thing ???
    }


    private void OnButtonPressed()
    {
        cursor.Visible = true;
    }


}
