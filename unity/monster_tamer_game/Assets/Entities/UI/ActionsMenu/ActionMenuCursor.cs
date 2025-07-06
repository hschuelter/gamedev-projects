using UnityEngine;

public class ActionMenuCursor : MonoBehaviour
{
    private ActionOption currentOption;
    private SpriteRenderer spriteRenderer;
    private RuntimeAnimatorController animatorController;
    private Animator animator;

    bool holdPosition = false;

    Color active = new Color(1f, 1f, 1f);
    Color unactive = new Color(0.6f, 0.6f, 0.6f);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animatorController = GetComponent<RuntimeAnimatorController>();
        animator = GetComponent<Animator>();
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        holdPosition = false;
        spriteRenderer.color = active;
        animator.enabled = true;
    }
    public void Deactivate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        holdPosition = true;
        spriteRenderer.color = unactive;
        animator.enabled = false;
    }
}
