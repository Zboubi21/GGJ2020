using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
    [Space]
    public TextMeshProUGUI _tmp;
    [Space]
    public CanvasGroup canvasGroup;
    [Space]
    public AudioSource source;
    public AudioClip[] inputSound;
    float _currentPassingTime;

    List<GameObject> instantiatedInput = new List<GameObject>();
    public List<GameObject> InstantiatedInput { get => instantiatedInput; }

    int inputCount;
    bool activateTimer;
    public bool ActivateTimer { get => activateTimer; set => activateTimer = value; }

    int m_playerID;

    int comboCount = 1;

    private void Start()
    {
        m_playerID = m_playerController.m_playerId;
        OnResetTimer();
    }
    void Update()
    {

        //transform.LookAt(Camera.main.transform);

        if (Input.GetKeyDown(KeyCode.T))
        {
            OnSpriteGeneration();
            OnResetTimer();
        }

        OnPassingTime(timeForPatternWithoutCombo);

    }

    public void OnResetTimer()
    {
        _currentPassingTime = timeForPatternWithoutCombo;
        inputCount = 0;
    }


    public void OnPassingTime(float time)
    {
        if (ActivateTimer)
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
        else
        {
            timeBar.fillAmount = 0;
        }
    }

    public void OnSpriteGeneration()
    {
        StartCoroutine(OnShowCanvasGroupe());
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
        if (source != null && inputSound[0] != null)
        {
            int random = Random.Range(0, inputSound.Length);
            source.clip = inputSound[random];
            source.Play();
        }
        if (inputCount < InstantiatedInput.Count)
        {
            InstantiatedInput[inputCount].GetComponent<InputScript>().IsCheckable = true;
        }
        else if(inputCount == InstantiatedInput.Count)
        {
            OnSpriteGeneration();
            OnResetTimer();
            m_conveyorController.On_PotIsRepair();
            OnWinCombo();
        }

    }

    public void OnLoseCombo()
    {
        if(comboCount - 1 >= 1)
        {
            comboCount--;
            _tmp.text = string.Format("X {0}", comboCount);
        }
        StartCoroutine(OnHideCanvasGroupe());

        if (source != null && inputSound[0] != null)
        {
            int random = Random.Range(0, inputSound.Length);
            source.clip = inputSound[random];
            source.Play();
        }
    }

    void OnWinCombo()
    {
        comboCount++;
        _tmp.text = string.Format("X {0}", comboCount);
        StartCoroutine(OnHideCanvasGroupe());
    }


    IEnumerator OnHideCanvasGroupe()
    {
        while (canvasGroup.alpha > 0)
        {
            Debug.Log("hop");
            yield return new WaitForSeconds(0.01f);
            canvasGroup.alpha -= 0.1f;
        }
    }

    IEnumerator OnShowCanvasGroupe()
    {
        while (canvasGroup.alpha < 1)
        {
            yield return new WaitForSeconds(0.01f);
            canvasGroup.alpha += 0.1f;
        }
    }
}
