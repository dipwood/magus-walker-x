using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    public WarUnitController[] playerUnitPrefabs;
    public WarUnitController[] enemyUnitPrefabs;

    public List<WarUnitController> playerBattleUnits = new List<WarUnitController>();
    public List<WarUnitController> enemyBattleUnits = new List<WarUnitController>();

    public GameObject movementSquare;
    public GameObject attackSquare;
    public List<GameObject> moveAttackTiles;

    readonly float movementTileTransparency = 0.5f;

    public Grid grid;
    private bool movementTilesDisplayed;

    public int playerUnitsMoved = 0;
    public int enemyUnitsMoved = 0;

    public int totalPlayerUnits;
    public int totalEnemyUnits;

    public bool isPlayerTurn;
    public bool isEnemyTurn;
    public bool nextTurn;

    public bool AITurnActive { get; private set; }
    public bool AIAction { get; private set; }
    public bool AINextStep;

    private int currentAIUnit = -1;
    private bool instructions;
    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        instructions = true;

        foreach (var battleUnit in playerUnitPrefabs)
        {
            WarUnitController unit = Instantiate(battleUnit);
            unit.isPlayerUnit = true;
            playerBattleUnits.Add(unit);
        }

        foreach (var battleUnit in enemyUnitPrefabs)
        {
            WarUnitController unit = Instantiate(battleUnit);
            unit.isPlayerUnit = false;
            enemyBattleUnits.Add(unit);
        }

        totalPlayerUnits = playerBattleUnits.Count;
        totalEnemyUnits = enemyBattleUnits.Count;

        // TODO: externalize later
        playerBattleUnits[0].transform.position = new Vector3(0f, 0f, 0f);
        playerBattleUnits[1].transform.position = new Vector3(1f, 1f, 0f);
        playerBattleUnits[2].transform.position = new Vector3(1f, 2f, 0f);
        playerBattleUnits[3].transform.position = new Vector3(2f, 1f, 0f);
        playerBattleUnits[4].transform.position = new Vector3(2f, 2f, 0f);

        enemyBattleUnits[0].transform.position = new Vector3(-2f, -2f, 0f);
        enemyBattleUnits[1].transform.position = new Vector3(-5f, -4f, 0f);
        enemyBattleUnits[2].transform.position = new Vector3(-4f, -5f, 0f);
        enemyBattleUnits[3].transform.position = new Vector3(-6f, -5f, 0f);
        enemyBattleUnits[4].transform.position = new Vector3(-5f, -6f, 0f);

        moveAttackTiles = new List<GameObject>();

        CalculatePositionAndCreateMoveAttackTiles(playerBattleUnits, enemyBattleUnits, true);
        CalculatePositionAndCreateMoveAttackTiles(enemyBattleUnits, playerBattleUnits, false);
        isPlayerTurn = true;
        isEnemyTurn = false;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (instructions)
        {
            DialogManager.instance.ShowDialog(GetLines(), false, false);
            instructions = false;
        }

        //// for testing
        //if (Input.GetKey(KeyCode.U))
        //{
        //    isPlayerTurn = false;
        //    isEnemyTurn = false;
        //    playerUnitsMoved = 0;
        //    enemyUnitsMoved = 0;
        //}

        //// for testing
        //if (Input.GetKey(KeyCode.Y))
        //{
        //    isPlayerTurn = false;
        //    isEnemyTurn = true;
        //    playerUnitsMoved = 0;
        //    enemyUnitsMoved = 0;
        //}

        if (Input.GetKey(KeyCode.T))
        {
            AudioManager.instance.StopMusic();
            SceneManager.LoadScene("GridTest");
        }

        if (!gameOver)
        {
            CalculateTurn();
            PerformAction();
        }
    }

    private void PerformAction()
    {
        if (isPlayerTurn)
        {
            AITurnActive = false;
            CalculatePositionAndCreateMoveAttackTiles(playerBattleUnits, enemyBattleUnits, true);
            ActionOnClick(playerBattleUnits, ref playerUnitsMoved, ref totalEnemyUnits, ref enemyBattleUnits);

            if (totalEnemyUnits == 0)
            {
                DialogManager.instance.ShowDialog(GetLinesWin(), false, false);
                AudioManager.instance.PlayBGM(8);
            }
        }
        else if (isEnemyTurn)
        {
            AITurnActive = true;
            CalculatePositionAndCreateMoveAttackTiles(enemyBattleUnits, playerBattleUnits, false);

            // uncomment next line if you want to control enemy units, TODO: control via public bool
            // ActionOnClick(enemyBattleUnits, ref enemyUnitsMoved, ref totalPlayerUnits, ref playerBattleUnits);

            AITurn();
        }
    }

    private void CalculateTurn()
    {
        if (!isPlayerTurn && !isEnemyTurn)
        {
            NextTurn(playerBattleUnits);
            NextTurn(enemyBattleUnits);
            isPlayerTurn = true;
        }

        if (isPlayerTurn && playerUnitsMoved == totalPlayerUnits)
        {
            playerUnitsMoved = 0;
            isPlayerTurn = false;
            isEnemyTurn = true;
        }
        if (isEnemyTurn && enemyUnitsMoved == totalEnemyUnits)
        {
            enemyUnitsMoved = 0;
            isEnemyTurn = false;
        }
    }

    private void NextTurn(List<WarUnitController> battleUnits)
    {
        foreach (WarUnitController unit in battleUnits)
        {
            unit.PlayMovementAnimation();
            unit.unitMoved = false;
        }
    }

    private void AITurn()
    {
        Vector3Int coordinate = new Vector3Int(0, 0, 0);

        if (!AIAction)
        {
            for (int i = 0; i < enemyBattleUnits.Count; i++)
            {
                if (!enemyBattleUnits[i].unitMoved)
                {
                    enemyBattleUnits[i].selected = true;
                    AIAction = true;
                    currentAIUnit = i;
                    movementTilesDisplayed = false;
                    break;
                }
            }
        }

        if (AIAction && moveAttackTiles.Count > 0)
        {
            coordinate = new Vector3Int(enemyBattleUnits[currentAIUnit].unitCellPosition.x,
                enemyBattleUnits[currentAIUnit].unitCellPosition.y + 1,
                enemyBattleUnits[currentAIUnit].unitCellPosition.z);

            int removeAtIndex = -1;

            StartCoroutine(AIWaitCo(0.1f, coordinate, removeAtIndex));
        }
    }

    private void ActionOnClick(List<WarUnitController> battleUnits, ref int unitsMoved, ref int totalUnits, ref List<WarUnitController> opposingBattleUnits)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);

            int removeAtIndex = -1;
            foreach (WarUnitController unit in battleUnits)
            {
                if (!unit.unitMoved)
                {
                    if (coordinate.x == unit.unitCellPosition.x && coordinate.y == unit.unitCellPosition.y)
                    {
                        unit.selected = true;
                        movementTilesDisplayed = false;
                    }
                    else if (unit.selected)
                    {
                        removeAtIndex = MoveOrAttack(ref unitsMoved, opposingBattleUnits, coordinate, ref removeAtIndex, unit);
                        DeselectUnitAndDestroyMoveAttackTiles(unit);
                    }
                }
            }

            totalUnits = RemoveUnit(totalUnits, opposingBattleUnits, removeAtIndex);
        }
    }

    private int RemoveUnit(int totalUnits, List<WarUnitController> opposingBattleUnits, int removeAtIndex)
    {
        if (removeAtIndex > -1)
        {
            opposingBattleUnits[removeAtIndex].gameObject.SetActive(false);
            opposingBattleUnits.RemoveAt(removeAtIndex);
            totalUnits--;
        }

        return totalUnits;
    }

    private void DeselectUnitAndDestroyMoveAttackTiles(WarUnitController unit)
    {
        unit.selected = false;
        foreach (var movementTile in moveAttackTiles)
        {
            GameObject.Destroy(movementTile);
        }
        moveAttackTiles.Clear();
    }

    private int MoveOrAttack(ref int unitsMoved, List<WarUnitController> opposingBattleUnits, Vector3Int coordinate, ref int removeAtIndex, WarUnitController unit)
    {
        foreach (var movementTile in moveAttackTiles)
        {
            Vector3Int movementTileCoordinate = grid.WorldToCell(movementTile.transform.position);
            if (coordinate.x == movementTileCoordinate.x && coordinate.y == movementTileCoordinate.y)
            {
                if (movementTile.gameObject.CompareTag("Movement Square"))
                {
                    unit.transform.position = movementTile.transform.position;
                    if ((coordinate.x == -2 && coordinate.y == 7) ||
                        (coordinate.x == -3 && coordinate.y == 7) ||
                        (coordinate.x == -4 && coordinate.y == 7) ||
                        (coordinate.x == -5 && coordinate.y == 7) ||
                        (coordinate.x == -6 && coordinate.y == 7))
                    {
                        DialogManager.instance.ShowDialog(GetLinesLose(), false, false);
                        gameOver = true;
                    }
                }
                else if (movementTile.gameObject.CompareTag("Attack Square"))
                {
                    for (int i = 0; i < opposingBattleUnits.Count; i++)
                    {
                        if (opposingBattleUnits[i].unitCellPosition == movementTileCoordinate)
                        {
                            removeAtIndex = i;
                        }
                    }
                }

                unit.unitMoved = true;
                unit.PlayIdleAnimation();
                unitsMoved++;
            }
        }

        return removeAtIndex;
    }

    private void CalculatePositionAndCreateMoveAttackTiles(List<WarUnitController> battleUnits, List<WarUnitController> opposingBattleUnits, bool isPlayerTurn)
    {
        List<WarUnitController> combinedBattleUnits = new List<WarUnitController>();

        combinedBattleUnits.AddRange(battleUnits);
        combinedBattleUnits.AddRange(opposingBattleUnits);

        for (int i = 0; i < combinedBattleUnits.Count; i++)
        {
            combinedBattleUnits[i].unitCellPosition = grid.WorldToCell(combinedBattleUnits[i].transform.position);
            combinedBattleUnits[i].transform.position = grid.GetCellCenterLocal(combinedBattleUnits[i].unitCellPosition);

            if (combinedBattleUnits[i].selected && !movementTilesDisplayed)
            {
                foreach (var movementTile in moveAttackTiles)
                {
                    GameObject.Destroy(movementTile);
                }
                moveAttackTiles.Clear();

                movementTilesDisplayed = true;
                CreateMovementOrAttackTiles(battleUnits[i].unitCellPosition, battleUnits, opposingBattleUnits, isPlayerTurn);
            }
        }
    }

    private void CreateMovementOrAttackTiles(Vector3Int unitCellPosition, List<WarUnitController> battleUnits, List<WarUnitController> opposingBattleUnits, bool isPlayerTurn)
    {
        bool allowMovementTileLeft = true;
        bool allowMovementTileUp = true;
        bool allowMovementTileRight = true;
        bool allowMovementTileDown = true;

        bool allowAttackTileLeft = false;
        bool allowAttackTileUp = false;
        bool allowAttackTileRight = false;
        bool allowAttackTileDown = false;

        List<WarUnitController> combinedBattleUnits = new List<WarUnitController>();

        combinedBattleUnits.AddRange(battleUnits);
        combinedBattleUnits.AddRange(opposingBattleUnits);

        foreach (WarUnitController unit in combinedBattleUnits)
        {
            if (unit.unitCellPosition.x == (unitCellPosition.x + 1) && (unit.unitCellPosition.y == unitCellPosition.y))
            {
                allowMovementTileRight = false;
                if ((isPlayerTurn && !unit.isPlayerUnit) || (!isPlayerTurn && unit.isPlayerUnit))
                {
                    allowAttackTileRight = true;
                }
            }
            if (unit.unitCellPosition.x == (unitCellPosition.x - 1) && (unit.unitCellPosition.y == unitCellPosition.y))
            {
                allowMovementTileLeft = false;
                if ((isPlayerTurn && !unit.isPlayerUnit) || (!isPlayerTurn && unit.isPlayerUnit))
                {
                    allowAttackTileLeft = true;
                }
            }
            if (unit.unitCellPosition.y == (unitCellPosition.y + 1) && (unit.unitCellPosition.x == unitCellPosition.x))
            {
                allowMovementTileUp = false;
                if ((isPlayerTurn && !unit.isPlayerUnit) || (!isPlayerTurn && unit.isPlayerUnit))
                {
                    allowAttackTileUp = true;
                }
            }
            if (unit.unitCellPosition.y == (unitCellPosition.y - 1) && (unit.unitCellPosition.x == unitCellPosition.x))
            {
                allowMovementTileDown = false;
                if ((isPlayerTurn && !unit.isPlayerUnit) || (!isPlayerTurn && unit.isPlayerUnit))
                {
                    allowAttackTileDown = true;
                }
            }
        }

        if (allowMovementTileUp)
        {
            CreateMovementOrAttackTile(unitCellPosition, 0, 1, true);
        }
        if (allowMovementTileRight)
        {
            CreateMovementOrAttackTile(unitCellPosition, 1, 0, true);
        }
        if (allowMovementTileDown)
        {
            CreateMovementOrAttackTile(unitCellPosition, 0, -1, true);
        }
        if (allowMovementTileLeft)
        {
            CreateMovementOrAttackTile(unitCellPosition, -1, 0, true);
        }

        if (allowAttackTileUp)
        {
            CreateMovementOrAttackTile(unitCellPosition, 0, 1, false);
        }
        if (allowAttackTileRight)
        {
            CreateMovementOrAttackTile(unitCellPosition, 1, 0, false);
        }
        if (allowAttackTileDown)
        {
            CreateMovementOrAttackTile(unitCellPosition, 0, -1, false);
        }
        if (allowAttackTileLeft)
        {
            CreateMovementOrAttackTile(unitCellPosition, -1, 0, false);
        }
    }

    private void CreateMovementOrAttackTile(Vector3Int unitCellPosition, int horizontalOffset, int verticalOffet, bool isMovementTile)
    {
        GameObject movementOrAttackTile;

        if (isMovementTile)
        {
            movementOrAttackTile = Instantiate(movementSquare);
        }
        else
        {
            movementOrAttackTile = Instantiate(attackSquare);
        }
        movementOrAttackTile.transform.position = grid.GetCellCenterLocal(new Vector3Int(unitCellPosition.x + horizontalOffset, unitCellPosition.y + verticalOffet, unitCellPosition.z));
        Color color = movementOrAttackTile.GetComponent<Renderer>().material.color;
        color.a = movementTileTransparency;
        movementOrAttackTile.SetActive(true);
        moveAttackTiles.Add(movementOrAttackTile);
    }

    public IEnumerator AIWaitCo(float timeToWait, Vector3Int coordinate, int removeAtIndex)
    {
        yield return new WaitForSeconds(timeToWait);
        removeAtIndex = MoveOrAttack(ref enemyUnitsMoved, playerBattleUnits, coordinate, ref removeAtIndex, enemyBattleUnits[currentAIUnit]);
        DeselectUnitAndDestroyMoveAttackTiles(enemyBattleUnits[currentAIUnit]);
        totalPlayerUnits = RemoveUnit(totalPlayerUnits, playerBattleUnits, removeAtIndex);
        yield return new WaitForSeconds(timeToWait);
        AIAction = false;
    }

    private string[] GetLines()
    {
        string[] lines = new string[4];

        lines[0] = "NULL-";
        lines[1] = "BONUS MINIGAME!";
        lines[2] = "Click on the top dudes to move them. Touching an enemy unit destroys it, but the same is true if they touch yours.";
        lines[3] = "Stop the Corone Empire from reaching the village! It's those 5 houses at the top. Also, press 'T' if you want to restart this minigame.";

        return lines;
    }

    private string[] GetLinesLose()
    {
        string[] lines = new string[2];

        lines[0] = "NULL-";
        lines[1] = "You lose! Press 'T' to try again!";

        return lines;
    }

    private string[] GetLinesWin()
    {
        string[] lines = new string[2];

        lines[0] = "NULL-";
        lines[1] = "You win! Press 'T' to restart!";

        return lines;
    }
}
