using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class ActionMenuCursor : MonoBehaviour
{
    private ActionOption currentOption;
    private SpriteRenderer spriteRenderer;

    bool holdPosition = false;

    Color active = new Color(1f, 1f, 1f);
    Color unactive = new Color(0.6f, 0.6f, 0.6f);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Activate();
    }
    private void MoveToTarget()
    {
        if (currentOption == null) return;
        
        var optionPosition = currentOption.transform.position;
        var cursorPosition = this.transform.position;

        this.transform.position = new Vector3(cursorPosition.x, optionPosition.y, cursorPosition.z);
    }

    public void ChangeTarget(ActionOption actionOption)
    {
        if (holdPosition) return;

        currentOption = actionOption;
        MoveToTarget();
    }

    public void Activate()
    {
        holdPosition = false;
        spriteRenderer.color = active;

    }
    public void Deactivate()
    {
        holdPosition = true;
        spriteRenderer.color = unactive;

    }
}
