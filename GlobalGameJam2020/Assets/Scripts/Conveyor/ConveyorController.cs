using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    
    [SerializeField] GameObject[] m_objectToSpawn = new GameObject[1];
    [SerializeField] Transform m_spawnRoot;

    [Header("Positions")]
    public Transform m_startPos;
    public Transform m_endPos;
    public Transform m_startPlayerPos;

    [Header("End Pot Anim Positions")]
    public Transform m_endAnimZPos;
    public Transform m_endAnimYPos;

    [Space]
    [SerializeField] float m_waitTimeToSpawnPot = 1;
    [Space]
    public float m_timeToMovePot = 3;
    [Space]
    float m_speed;
    public float Speed { get => m_speed; set => m_speed = value; }

    Pot m_actualRepairpot;
    PlayerController m_player;


    void Start()
    {
        On_SpawnPot();

        Speed = Vector3.Distance(m_startPos.position, m_endPos.position) / m_timeToMovePot;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            On_PotIsRepair();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            On_PotIsBreak();
        }
    }

    IEnumerator WaitTimeToSpawnPot()
    {
        yield return new WaitForSeconds(m_waitTimeToSpawnPot);
        AddPot();
    }
    void AddPot()
    {
        int potToSpawn = Random.Range(0, m_objectToSpawn.Length);
        m_actualRepairpot = Instantiate(m_objectToSpawn[potToSpawn], m_startPos.position, m_startPos.rotation, m_spawnRoot).GetComponent<Pot>();
        m_actualRepairpot.StartToMovePot(this);
        m_player.On_StartToFollowPot(true);
    }

    public void SetPlayerToConveyor(PlayerController player)
    {
        m_player = player;
        m_player.transform.position = m_startPlayerPos.transform.position;
    }
    public Pot GetRepairPot()
    {
        return m_actualRepairpot;
    }
    public void On_PotArrivedAtTheEndOfConveyor(Pot arrivedPot)
    {
        if(arrivedPot == m_actualRepairpot)
        {
            m_player.On_StartToFollowPot(false);
            m_player.MoveToStartPos();
        }
    }

    public void On_PotIsRepair()
    {
        m_actualRepairpot.On_PotIsRepair();
        m_player.On_StartToFollowPot(false);
        m_player.MoveToStartPos();
    }
    public void On_PotIsBreak()
    {
        m_actualRepairpot.On_PotIsBreak();
        m_player.On_StartToFollowPot(false);
        m_player.MoveToStartPos();
    }

    public void On_SpawnPot()
    {
        StartCoroutine(WaitTimeToSpawnPot());
    }

}
