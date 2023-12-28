using Godot;

namespace CustomSchemePracticeForGodot;

public partial class Main : Node2D
{
    private Label _label;
    
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _label.Text = "Message received:\n";
        SingletonLimitHelper.MessageFromCustomSchemeEvent += message =>
        {
            _label.Text += message + "\n";
        };
    }
}
