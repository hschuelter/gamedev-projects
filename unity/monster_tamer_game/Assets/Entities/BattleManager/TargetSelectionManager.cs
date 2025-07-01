using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetSelectionManager : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    
    [HideInInspector] public bool isSelectingEnemy = false;
    private bool isReady = false;

    private List<Stats> enemiesList;
    private int currentPosition = 0;
    private int verticalInput = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isSelectingEnemy) return;

        if (!isReady && !Input.GetButtonDown("Submit"))
        {
            Debug.Log($"Soltei");
            isReady = true;
            currentPosition = 0;
        }

        if (isReady)
        {
            float rawVerticalInput = Input.GetAxis("Vertical");
            int currVerticalInput = 0;

            if (rawVerticalInput > 0) currVerticalInput = 1;
            if (rawVerticalInput < 0) currVerticalInput = -1;

            if (Input.GetButtonDown("Submit"))
            {
                isSelectingEnemy = false;
                isReady = false;
                battleManager.hudManager.EnableActionMenu();
                battleManager.ConfirmTarget(enemiesList.ElementAt(currentPosition));
                Debug.Log($"{enemiesList.ElementAt(currentPosition).name}");
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
    }

    private void MoveUp()
    {
        currentPosition = (currentPosition + enemiesList.Count - 1) % enemiesList.Count;
    }
    private void MoveDown()
    {
        currentPosition = (currentPosition + enemiesList.Count + 1) % enemiesList.Count;
    }
}
