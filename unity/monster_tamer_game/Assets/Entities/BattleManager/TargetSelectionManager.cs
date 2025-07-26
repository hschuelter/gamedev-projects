using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetSelectionManager : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private GameObject selectionCursor;

    [HideInInspector] public bool isSelectingEnemy = false;
    private bool isReady = false;

    private List<Stats> enemiesList;
    private int currentPosition = 0;
    private int verticalInput = 0;
    private bool isHovering = false;

    private void Start()
    {
        selectionCursor.SetActive(false);
    }

    void Update()
    {
        if (!isSelectingEnemy) return;
        if (!isReady && !Input.GetButtonDown("Submit"))
        {
            isReady = true;
            currentPosition = 0;
            selectionCursor.SetActive(true);
            var position = enemiesList.Where(ene => ene.currentHealth > 0).ElementAt(currentPosition).transform.position;
            float yOffset = -0.05f;
            selectionCursor.transform.position = new Vector3(position.x, position.y + yOffset, position.z);
        }

        if (isReady)
        {
            float rawVerticalInput = Input.GetAxis("Vertical");
            int currVerticalInput = 0;

            if (rawVerticalInput > 0) currVerticalInput = 1;
            if (rawVerticalInput < 0) currVerticalInput = -1;
            
            if (Input.GetButtonDown("Submit") ||
                (Input.GetMouseButtonDown(0) && isHovering))
            {

                isHovering = false;
                DisableComponent();
                battleManager.ConfirmTarget(enemiesList, currentPosition);
            }

            if (currVerticalInput != 0 && currVerticalInput != verticalInput)
            {
                if (currVerticalInput ==  1) MoveUp();
                if (currVerticalInput == -1) MoveDown();
            }
            verticalInput = currVerticalInput;
        }
    }

    public void SetTargets(List<Stats> enemiesList)
    {
        this.enemiesList = enemiesList;
        selectionCursor.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }
    public void SetTargetsAlly(List<Stats> enemiesList)
    {
        this.enemiesList = enemiesList;
        selectionCursor.transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
    }

    private void MoveUp()
    {
        currentPosition = (currentPosition + enemiesList.Count - 1) % enemiesList.Count;

        var position = enemiesList.ElementAt(currentPosition).transform.position;
        float yOffset = -0.05f;
        selectionCursor.transform.position = new Vector3(position.x, position.y + yOffset, position.z);

    }
    private void MoveDown()
    {
        currentPosition = (currentPosition + enemiesList.Count + 1) % enemiesList.Count;

        var position = enemiesList.ElementAt(currentPosition).transform.position;
        float yOffset = -0.05f;
        selectionCursor.transform.position = new Vector3(position.x, position.y + yOffset, position.z);
    }

    public void SelectOnMouseHover(string enemyName)
    {
        if (!isSelectingEnemy) return;
        isHovering = true;

        currentPosition = enemiesList.FindIndex(enemy => enemy.gameObject.name == enemyName);
        var position = enemiesList.ElementAt(currentPosition).transform.position;
        float yOffset = -0.05f;
        selectionCursor.transform.position = new Vector3(position.x, position.y + yOffset, position.z);
    }

    public void MouseExit()
    {
        isHovering = false;
    }

    public void Enable()
    {

        isSelectingEnemy = true;
    }

    public void DisableComponent()
    {
        isSelectingEnemy = false;
        isReady = false;
        selectionCursor.SetActive(false);
    }

    public void InvertCursor(bool value)
    {
        int x = value ? -1 : 1;
        selectionCursor.transform.localScale = new Vector3(0.7f * x, 0.7f, 0.7f);
    }
}
