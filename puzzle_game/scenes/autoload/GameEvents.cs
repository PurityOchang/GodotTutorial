using Game.Component;
using Godot;


namespace Game.Autoload;  // since the GameEvents Node is an auto node it could be easily access by any other node int the scene tree
                          // anyother node we want to listen to the building places signal would be able to easily reference the game events
                          // node and connect to that signa

//Godot is going to load this node when the game first starts
public partial class GameEvents : Node
{

    public static GameEvents Instance { get; private set; } // public access of reading the instance static method but it would only 
                                                            // allow set within the GameEvent class.

    // we are defining signals here
    [Signal] // a way to define a custom signal in godot
    public delegate void BuildingPlacedEventHandler(BuildingComponent buildingComponent);   // you need the words EventHandler it is a
                                                                                            // godot specific requirement

    public override void _Notification(int what) // int what represents and integer to what the notification responts to. got to object or 
    {                                            // node on godot doc. you cannot know this without looking at the doc
        if (what == NotificationSceneInstantiated)
        {
            Instance = this;
        }

    }

    public static void EmitBuildingPlaced(BuildingComponent buildingComponent)
    {
        // because we are in a static method, we have to reference instance in cSharp
        Instance.EmitSignal(SignalName.BuildingPlaced, buildingComponent);  // emitsigals' first arguement is the name of the signal,and godots source generators 
                                                                            // automatically generates it. signal name is a class that contains a bunch of constants 
                                                                            //that corresponds to all the names of the signals that are available that are available 
                                                                            //on the current node we are in. while we can use a raw string it would be hard to figure 
                                                                            // out why your code does not work
    }
}
