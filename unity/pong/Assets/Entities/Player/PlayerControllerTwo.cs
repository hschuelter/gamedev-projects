using UnityEngine;

public class PlayerControllerTwo : IPlayerController
{
    public float GetInput()
    {
        return (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
    }
}
