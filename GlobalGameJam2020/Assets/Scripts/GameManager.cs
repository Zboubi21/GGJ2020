using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
	public static GameManager s_instance;

	// [SerializeField] float m_waitTimeToStartGame = 3;
	[SerializeField] float m_gameDuration = 90;

	[Header("Conveyor")]
	[SerializeField] ConveyorController[] m_conveyors;

	[Header("Timer")]
	[SerializeField] TextMeshProUGUI m_timerTextValue;

	float m_actualTimer;
	bool m_partyIsFinished = false;

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

		On_PartyIsStarted();
	}

	void Update()
	{
		m_actualTimer -= Time.deltaTime;
		UpdateTimerText();
		if(m_actualTimer < 0 && !m_partyIsFinished)
		{
			m_partyIsFinished = true;
			On_PartyIsFinished();
		}
	}

	public void UpdateTimerText()
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
		}
	}

	void On_PartyIsStarted()
	{
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

	}

}
