using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    
    [SerializeField] ProceduralInputGenerationController m_inputGenerationController;
    [Space]
    [SerializeField] GameObject[] m_objectToSpawn = new GameObject[1];
    [SerializeField] Transform m_spawnRoot;

    [Header("Positions")]
    public Transform m_startPos;
    public Transform m_endPos;
    public Transform m_startPlayerPos;

    [Header("Rotators")]
    [SerializeField] ConveyorRotator[] m_rotators;

    [Header("End Pot Anim Positions")]
    public Transform m_endAnimZPos;
    public Transform m_endAnimYPos;

    [Space]
    [SerializeField] float m_waitTimeToSpawnPot = 1;
    [Space]
    public float m_startTimeToMovePot = 5;

    [Tooltip("The value who decrease conveyor time to move when a pot is repair")]
    [SerializeField] float m_decreaseConveyorTimeToMove = 0.25f;
    [SerializeField] float m_minConveyorTimeToMoveValue = 2.5f;

    [Header("Debug")]
    [SerializeField] bool m_showGizmos = false;
    [SerializeField] float m_gizmosSize = 1;
    [SerializeField] Color m_gizmosColor = Color.magenta;

    float m_actualTimeToMovePot;
    public float ActualTimeToMovePot { get => m_actualTimeToMovePot; }

    float m_actualSpeed;
    public float ActualSpeed { get => m_actualSpeed; }

    Pot m_actualRepairPot;
    public Pot ActualRepairPot { get => m_actualRepairPot;}

    PlayerController m_player;
    List<Pot> m_potsOnConveyor = new List<Pot>();
    GameManager m_gameManager;

    void Awake()
    {
        m_actualTimeToMovePot = m_startTimeToMovePot;
        SetConveyorSpeed();

        for (int i = 0, l = m_rotators.Length; i < l; ++i)
        {
            m_rotators[i].Conveyor = this;
        }
    }

    void Start()
    {
        m_gameManager = GameManager.s_instance;
    }

    void OnDrawGizmos()
    {
        if (!m_showGizmos)
            return;

        Gizmos.color = m_gizmosColor;
        Gizmos.DrawSphere(m_startPos.position, m_gizmosSize);
        Gizmos.DrawSphere(m_endPos.position, m_gizmosSize);
        Gizmos.DrawSphere(m_startPlayerPos.position, m_gizmosSize);
        Gizmos.DrawSphere(m_endAnimZPos.position, m_gizmosSize);
        Gizmos.DrawSphere(m_endAnimYPos.position, m_gizmosSize);
    }

    IEnumerator WaitTimeToSpawnPot()
    {
        yield return new WaitForSeconds(m_waitTimeToSpawnPot);
        AddPot();
    }
    void AddPot()
    {
        int potToSpawn = Random.Range(0, m_objectToSpawn.Length);
        m_actualRepairPot = Instantiate(m_objectToSpawn[potToSpawn], m_startPos.position, m_startPos.rotation, m_spawnRoot).GetComponent<Pot>();
        m_potsOnConveyor.Add(m_actualRepairPot);
        m_actualRepairPot.StartToMovePot(this);
        m_player.On_StartToFollowPot(true);

        m_inputGenerationController.timeForPatternWithoutCombo = m_actualTimeToMovePot;
        m_inputGenerationController.OnSpriteGeneration();
        m_inputGenerationController.OnResetTimer();
        m_inputGenerationController.ActivateTimer = true;
    }

    void SetConveyorSpeed()
    {
        m_actualSpeed = Vector3.Distance(m_startPos.position, m_endPos.position) / m_actualTimeToMovePot;
    }
    void On_IncreaseConveyorSpeed()
    {
        m_actualTimeToMovePot -= m_decreaseConveyorTimeToMove;
        if (m_actualTimeToMovePot < m_minConveyorTimeToMoveValue)
            m_actualTimeToMovePot = m_minConveyorTimeToMoveValue;
        SetConveyorSpeed();
    }
    void On_ResetConveyorSpeed()
    {
        m_actualTimeToMovePot = m_startTimeToMovePot;
        SetConveyorSpeed();
        SetPotsSpeed();
    }

    void SetPotsSpeed()
    {
        if (m_potsOnConveyor.Count == 0)
            return;

        for (int i = 0, l = m_potsOnConveyor.Count; i < l; ++i)
        {
            m_potsOnConveyor[i].GetConveyorValues();
        }
    }

    void StopTimerInputGeneration()
    {
        m_inputGenerationController.DestroyInputsOnLists(m_inputGenerationController.InstantiatedInput);
        m_inputGenerationController.ActivateTimer = false;
    }

    public void SetPlayerToConveyor(PlayerController player)
    {
        m_player = player;
        m_player.transform.position = m_startPlayerPos.transform.position;
    }
    public Pot GetRepairPot()
    {
        return m_actualRepairPot;
    }
    public void On_PotArrivedAtTheEndOfConveyor(Pot arrivedPot)
    {
        if(arrivedPot == m_actualRepairPot)
        {
            m_actualRepairPot = null; // En test

            m_player.On_StartToFollowPot(false);
            m_player.MoveToStartPos();
            On_ResetConveyorSpeed();

            m_gameManager.On_PotArrivedAtTheEndOfConveyor(m_player.m_playerId);

            StopTimerInputGeneration();
        }
    }

    public void On_PotIsRepair()
    {
        m_actualRepairPot.On_PotIsRepair();
        m_actualRepairPot = null; // En test
        m_player.On_StartToFollowPot(false);
        m_player.MoveToStartPos();
        On_IncreaseConveyorSpeed();
        SetPotsSpeed();

        m_gameManager.On_PotIsRepaired(m_player.m_playerId);

        StopTimerInputGeneration();
    }
    public void On_PotIsBreak()
    {
        m_actualRepairPot.On_PotIsBreak();
        m_actualRepairPot = null; // En test
        m_player.On_StartToFollowPot(false);
        m_player.MoveToStartPos();
        On_ResetConveyorSpeed();

        m_gameManager.On_PotIsBroken(m_player.m_playerId);

        StopTimerInputGeneration();
    }

    public void On_PotIsDestroy(Pot pot)
    {
        m_potsOnConveyor.Remove(pot);
    }

    public void On_SpawnPot()
    {
        StartCoroutine(WaitTimeToSpawnPot());
    }
    public void On_ConveyorStop()
    {
        m_actualTimeToMovePot = 0;
        SetConveyorSpeed();
        SetPotsSpeed();
    }

}
