using CustomSchemePracticeForGodot.Utils;
using Godot;

namespace CustomSchemePracticeForGodot;

public partial class Main : Node2D
{
    private Label _counterText;
    private Button _counterButton;
    private Button _startReceivedButton;
    private Label _receivedMessage;
    private int _count;

    public override void _Ready()
    {
        if (CallbackChecker.Check())
        {
            GetTree().Quit();
            return;
        }

        _counterText = GetNode<Label>("VBoxContainer/CounterText");
        _counterButton = GetNode<Button>("VBoxContainer/CounterButton");
        _startReceivedButton = GetNode<Button>("VBoxContainer/StartReceiveButton");
        _receivedMessage = GetNode<Label>("VBoxContainer/ReceivedMessage");

        _startReceivedButton.Pressed += async () =>
            _receivedMessage.Text += "Callback received: " + await CallbackReceiver.ReceiveANewMessage() + "\n";
        _counterButton.Pressed += () => _counterText.Text = $"Count: {++_count}";
    }
}
