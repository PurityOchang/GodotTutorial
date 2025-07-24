using Godot;
using System;

public partial class Main : Node2D
{
    //Called wehn the node enters the scene tree for the first time./
    public override void _Ready()
    {
        GD.Print("Hello world!!!");
    }

    // Called every fram. 'delta' is the elapsed tiime since the previous frame.
    public override void _Process(double delta)
    {
        
    }
}
