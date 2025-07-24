using Godot;  // This is importing everything from the Godot name space. similar to a module
using System;

// A c sharp namespace is a way of categorizing or organizing different files in the solution
// a namespace is used when you are importing using the using keyword
namespace Game;  // The main namespace we use for our own files that are separate or extend godot functionality


public partial class Main : Node2D
{
    private Sprite2D sprite;

    //Called when the node enters the scene tree for the first time.
    // This ready method is called after the ready methods of all its children have been called. all decendents get called first b4 root basically saying they are configured and ready to go
    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Cursor");  // Rememeber in heritance form 270 you cannot equate a more specific type to a less
                                               // specific one. Getnode returns a generic node sprite is a Sprite2D. Remember the heirarchy.
                                               // The angular brackets supply Sprite2D as the generic type.  Cursor/Node2D -this is the node path of a child of cursor.
                                               // You can also get node path by right clicking the desired node on godot and clicking copy path.
                                               // the string over there is the node path. This is the string representation of how get node can locate
                                               // the node we are trying to target in the scene tree. GetNode always happens relative to the current node we are calling it from.
                                               // when there is no leading forward slash. Saying in relatioin to main find the node called sprite 2d.
    }

    // Called every frame. 'delta' is the elapsed tiime since the previous frame.
    public override void _Process(double delta)   // This delta passes the amount of time since the last frame
    {
        var mousePosition = GetGlobalMousePosition();  // returns a vector2 that is the x and y postion of the mouse
        var gridPosition = mousePosition / 64;
        gridPosition = gridPosition.Floor();   // so we do not get fractional positions
        sprite.GlobalPosition = gridPosition * 64;  // GlobalPostion is the postion of the node 2d in the world.
                                                    // there is another property called postion which is its relative postion to its parent.
                                                    // We are doing this (multipltying by 64) so the pixel positon of the sprite is directly aligned with the grid.

    }
}
