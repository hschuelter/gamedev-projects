using UnityEngine;

public class PlayerControllerOne : IPlayerController
{
    public float GetInput()
    {
        return (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
    }
}
