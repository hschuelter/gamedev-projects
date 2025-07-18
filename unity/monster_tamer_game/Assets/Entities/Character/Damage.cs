using UnityEngine;

public class Damage
{
    public int value;
    public bool isCritical;

    public Damage(int value, bool isCritical = false)
    {
        this.value = value;
        this.isCritical = isCritical;
    }
}
