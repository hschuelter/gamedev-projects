using UnityEngine;

public class Damage
{
    public int value;
    public bool isCritical;
    public bool isMiss;

    public Damage(int value, bool isCritical = false, bool isMiss = false)
    {
        this.value = value;
        this.isCritical = isCritical;
        this.isMiss = isMiss;
    }
}
