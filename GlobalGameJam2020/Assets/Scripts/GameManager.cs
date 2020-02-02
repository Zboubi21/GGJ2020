using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
	public static GameManager s_instance;

	[SerializeField] float m_waitTimeToStartGame = 4;
	[SerializeField] float m_gameDuration = 90;
	[SerializeField] float m_criticalTime= 70;

    [Header("Countdown")]
	[SerializeField] Animator m_animator;
    public GameObject m_CountdownSFX;
    

    [Header("Conveyor")]
	[SerializeField] ConveyorController[] m_conveyors;
    public int UltraCombo = 15;

	[Header("Timer")]
	[SerializeField] Animator m_showTimerAnim;
	[SerializeField] TextMeshProUGUI m_timerTextValue;
    public GameObject m_EndgameSFX;

    [Header("Score")]
	[SerializeField] TextMeshProUGUI m_scoreTxtP1;
	[SerializeField] TextMeshProUGUI m_scoreTxtP2;
    public Animator scoreP1;
    public Animator scoreP2;
    [Space]
	[SerializeField] int m_potScoreValue = 1;

    [Header("Patern Var")]
    public float punishTimeForFaillingPatern = 0.5f;

	[Header("Camera")]
	[SerializeField] CameraController m_camera;

    [Header("Mise en scene")]
	[SerializeField] Animator[] m_traps = new Animator[2];

    public bool GameOver;

    bool m_partyIsStarted = false;

	float m_actualTimer;
	bool m_partyIsFinished = false;

	int m_actualScoreP1 = 0;
	int m_actualComboNbrP1 = 1;
    public int ActualComboNbrP1 { get => m_actualComboNbrP1; }
	int m_actualScoreP2 = 0;
	int m_actualComboNbrP2 = 1;
    public int ActualComboNbrP2 { get => m_actualComboNbrP2; }

    bool m_canResetGame = false;
    [SerializeField] Animator m_endGameMenu;

    void Awake()
    {
        if(s_instance == null){
			s_instance = this;
		}else{
			Debug.LogError("Two instance of GameManager");
		}
    }

	void Start()
	{
		m_actualTimer = m_gameDuration;

		UpdateScoreText(1);
		UpdateScoreText(2);

		m_animator.SetTrigger("Start");
        Level.AddFX(m_CountdownSFX, Vector3.zero, Quaternion.identity);
        StartCoroutine(WaitTimeToStartGame());

		StartCoroutine(OpenTraps(m_waitTimeToStartGame - 0.5f));

		UpdateTimerText();
	}

	void Update()
	{
        if(m_canResetGame && Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

		if(!m_partyIsStarted)
			return;

		if(m_actualTimer - Time.deltaTime > 0)
		{
			m_actualTimer -= Time.deltaTime;
		}
		else
		{
			m_actualTimer = 0;
		}
		UpdateTimerText();
		if(m_actualTimer == 0 && !m_partyIsFinished)
		{
			m_partyIsFinished = true;
			On_PartyIsFinished();
		}
	}

	void UpdateTimerText()
	{
		int minutes = Mathf.FloorToInt(m_actualTimer / 60f);
		int seconds = Mathf.RoundToInt(m_actualTimer % 60f);

		string formatedSeconds = seconds.ToString();

		if (seconds == 60)
		{
			seconds = 0;
			minutes += 1;
		}

		if(m_timerTextValue != null)
        {
			m_timerTextValue.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            if(m_actualTimer < m_criticalTime)
            {
                m_timerTextValue.color = Color.red;
                m_showTimerAnim.SetTrigger("Critical");
                if(m_actualTimer == 0)
                {
                    m_showTimerAnim.SetTrigger("StopMoving");
                }
            }
        }
	}

	void CalculateScore(int playerId)
	{
		if(playerId == 1)
		{
			m_actualScoreP1 += m_potScoreValue * ActualComboNbrP1;
		}
		else
		{
			m_actualScoreP2 += m_potScoreValue * m_actualComboNbrP2;
		}
		UpdateScoreText(playerId);
	}
	void UpdateScoreText(int playerId)
	{
		if(playerId == 1)
		{
			if(m_scoreTxtP1 != null)
			{
				m_scoreTxtP1.text = m_actualScoreP1.ToString();
                scoreP1.SetTrigger("EarnScoreP1");
            }
		}
		else
		{
			if(m_scoreTxtP2 != null)
			{
				m_scoreTxtP2.text = m_actualScoreP2.ToString();
                scoreP2.SetTrigger("EarnScoreP2");
            }
        }
	}

	IEnumerator WaitTimeToStartGame()
	{
		yield return new WaitForSeconds(m_waitTimeToStartGame);
		On_PartyIsStarted();
	}
	void On_PartyIsStarted()
	{
		m_showTimerAnim.SetTrigger("Start");
		m_partyIsStarted = true;
		if(m_conveyors != null)
		{
			for (int i = 0, l = m_conveyors.Length; i < l; ++i)
			{
				if(m_conveyors[i] != null)
				{
					m_conveyors[i].On_SpawnPot();
				}
			}
		}
	}
	void On_PartyIsFinished()
	{
		m_camera.SwitchCameraToEndGame();
        GameOver = true;
        Level.AddFX(m_EndgameSFX, Vector3.zero, Quaternion.identity);
        if (m_conveyors != null)
		{
			for (int i = 0, l = m_conveyors.Length; i < l; ++i)
			{
				if(m_conveyors[i] != null)
				{
					m_conveyors[i].On_ConveyorStop();
				}
			}
		}

        if(m_actualScoreP1 > m_actualScoreP2)
        {
            scoreP1.SetTrigger("EndGameP1");
        }
        else if(m_actualScoreP1 < m_actualScoreP2)
        {
            scoreP2.SetTrigger("EndGameP2");
        }

        m_canResetGame = true;
        m_endGameMenu.SetTrigger("Start");
    }

	IEnumerator OpenTraps(float waitTimeToOpen)
	{
		yield return new WaitForSeconds(waitTimeToOpen);
		if(m_traps != null)
		{
			for (int i = 0, l = m_traps.Length; i < l; ++i)
			{
				if(m_traps[i] != null)
					m_traps[i].SetTrigger("Start");
			}
		}
	}

	public void On_PotIsRepaired(int playerId)
	{
		CalculateScore(playerId);

		if(playerId == 1)
			m_actualComboNbrP1 ++;

		if(playerId == 2)
			m_actualComboNbrP2 ++;
	}
	public void On_PotIsBroken(int playerId)
	{
		if(playerId == 1 && ActualComboNbrP1 > 1)
        {
            if(ActualComboNbrP1 <= UltraCombo)
            {
			    m_actualComboNbrP1 --;
            }
            else
            {
                m_actualComboNbrP1 = UltraCombo;
            }
        }

		if(playerId == 2 && m_actualComboNbrP2 > 1)
        {
            if (m_actualComboNbrP2 <= UltraCombo)
            {
                m_actualComboNbrP2--;
            }
            else
            {
                m_actualComboNbrP2 = UltraCombo;
            }
        }
	}
	public void On_PotArrivedAtTheEndOfConveyor(int playerId)
	{
        if (playerId == 1 && ActualComboNbrP1 > 1)
        {
            if (ActualComboNbrP1 <= UltraCombo)
            {
                m_actualComboNbrP1--;
            }
            else
            {
                m_actualComboNbrP1 = UltraCombo;
            }
        }

        if (playerId == 2 && m_actualComboNbrP2 > 1)
        {
            if (m_actualComboNbrP2 <= UltraCombo)
            {
                m_actualComboNbrP2--;
            }
            else
            {
                m_actualComboNbrP2 = UltraCombo;
            }
        }
    }

}
