using UnityEngine;

public class ControllsManager 
{
    //Movement
    public static KeyCode GoUpKey() => KeyCode.W;
    public static KeyCode GoDownKey() => KeyCode.S;
    public static KeyCode GoLeftKey() => KeyCode.A;
    public static KeyCode GoRightKey() => KeyCode.D;

    //Settings
    public static KeyCode RestartGameKey() => KeyCode.Space;
    public static KeyCode GetHelpWindow() => KeyCode.H;
    public static KeyCode ExitGameKey() => KeyCode.Escape;
    public static KeyCode TurnOffOnSounds() => KeyCode.M;
}
