using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProceduralInputGenerationController : MonoBehaviour
{
    [SerializeField] PlayerController m_playerController;
    [SerializeField] ConveyorController m_conveyorController;
    [Space]
    public Image timeBar;
    [Space]
    public GameObject[] allInputs;
    [Space]
    public Transform inputParent;
    [Space]
    public int nbrOfInputToSpawn;
    [Space]
    public float timeForPatternWithoutCombo;
    float _currentPassingTime;

    List<GameObject> instantiatedInput = new List<GameObject>();
    public List<GameObject> InstantiatedInput { get => instantiatedInput; }

    int inputCount;
    bool activateTimer;
    public bool ActivateTimer { get => activateTimer; set => activateTimer = value; }

    int m_playerID;

    private void Start()
    {
        m_playerID = m_playerController.m_playerId;
        OnResetTimer();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnSpriteGeneration();
            OnResetTimer();
            ActivateTimer = true;
        }
        else if(ActivateTimer)
        {
            OnPassingTime(timeForPatternWithoutCombo);
        }
    }

    public void OnResetTimer()
    {
        _currentPassingTime = timeForPatternWithoutCombo;
        inputCount = 0;
    }


    public void OnPassingTime(float time)
    {
        if(_currentPassingTime > 0)
        {
            _currentPassingTime -= Time.deltaTime;
            timeBar.fillAmount = Mathf.InverseLerp(0f, time, _currentPassingTime);
        }
        else
        {
            OnResetTimer();
            OnSpriteGeneration();
        }
    }

    public void OnSpriteGeneration()
    {
        DestroyInputsOnLists(InstantiatedInput);

        for (int i = 0; i < nbrOfInputToSpawn; ++i)
        {
            int random = Random.Range(0, allInputs.Length);
            GameObject go = Instantiate(allInputs[random], inputParent);
            InstantiatedInput.Add(go);
            InputScript input = go.GetComponent<InputScript>();
            input.Convoyer = m_conveyorController;
            input.Controller = this;
            input.PlayerID = m_playerID;
        }
        InstantiatedInput[0].GetComponent<InputScript>().IsCheckable = true;
    }

    public void DestroyInputsOnLists(List<GameObject> OnActiveList)
    {
        for (int i = 0, l = OnActiveList.Count; i < l; ++i)
        {
            Destroy(OnActiveList[i].gameObject);
        }
        OnActiveList.Clear();
    }

    public void ActivateCheckableOnNextInput()
    {
        inputCount++;

        if(inputCount < InstantiatedInput.Count)
        {
            InstantiatedInput[inputCount].GetComponent<InputScript>().IsCheckable = true;
        }
        else if(inputCount == InstantiatedInput.Count)
        {
            OnSpriteGeneration();
            OnResetTimer();
            m_conveyorController.On_PotIsRepair();
        }

    }
}
