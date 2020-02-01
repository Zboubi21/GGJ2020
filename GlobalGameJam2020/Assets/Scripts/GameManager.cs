using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
	public static GameManager s_instance;

	[SerializeField] float m_waitTimeToStartGame = 4;
	[SerializeField] float m_gameDuration = 90;
	[SerializeField] float m_criticalTime= 70;

    [Header("Countdown")]
	[SerializeField] Animator m_animator;

	[Header("Conveyor")]
	[SerializeField] ConveyorController[] m_conveyors;

	[Header("Timer")]
	[SerializeField] Animator m_showTimerAnim;
	[SerializeField] TextMeshProUGUI m_timerTextValue;

	[Header("Score")]
	[SerializeField] TextMeshProUGUI m_scoreTxtP1;
	[SerializeField] TextMeshProUGUI m_scoreTxtP2;
	[Space]
	[SerializeField] int m_potScoreValue = 1;

    [Header("Patern Var")]
    public float punishTimeForFaillingPatern = 0.5f;

	[Header("Camera")]
	[SerializeField] CameraController m_camera;

    [Header("Mise en scene")]
    [SerializeField] Animator m_stagingAnim;

    bool m_partyIsStarted = false;

	float m_actualTimer;
	bool m_partyIsFinished = false;

	int m_actualScoreP1 = 0;
	int m_actualComboNbrP1 = 1;
	int m_actualScoreP2 = 0;
	int m_actualComboNbrP2 = 1;

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
		StartCoroutine(WaitTimeToStartGame());

		UpdateTimerText();
	}

	void Update()
	{
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
		if(m_actualTimer < 0 && !m_partyIsFinished)
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
            }
        }
	}

	void CalculateScore(int playerId)
	{
		if(playerId == 1)
		{
			m_actualScoreP1 += m_potScoreValue * m_actualComboNbrP1;
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
			}
		}
		else
		{
			if(m_scoreTxtP2 != null)
			{
				m_scoreTxtP2.text = m_actualScoreP2.ToString();
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

		// Arrêter de faire spawn les pots et bouger le conveyor !
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
		if(playerId == 1 && m_actualComboNbrP1 > 1)
			m_actualComboNbrP1 --;

		if(playerId == 2 && m_actualComboNbrP2 > 1)
			m_actualComboNbrP2 --;
	}
	public void On_PotArrivedAtTheEndOfConveyor(int playerId)
	{
		if(playerId == 1 && m_actualComboNbrP1 > 1)
			m_actualComboNbrP1 --;

		if(playerId == 2 && m_actualComboNbrP2 > 1)
			m_actualComboNbrP2 --;
	}

}
