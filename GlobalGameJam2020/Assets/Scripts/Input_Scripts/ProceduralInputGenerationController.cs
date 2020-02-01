using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProceduralInputGenerationController : MonoBehaviour
{
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

    List<GameObject> InstantiatedInput = new List<GameObject>();

    private void Start()
    {
        OnResetTimer();
    }
    bool go;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnSpriteGeneration(nbrOfInputToSpawn, false);
            OnResetTimer();
            go = true;
        }
        else if(go)
        {
            OnPassingTime(timeForPatternWithoutCombo);
        }
    }

    public void OnResetTimer()
    {
        _currentPassingTime = timeForPatternWithoutCombo;
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
        }
    }

    public void OnSpriteGeneration(int nbrOfSpawn, bool hastoAddInputs)
    {
        if (!hastoAddInputs)
        {
            DestroyInputsOnLists(InstantiatedInput);
        }

        for (int i = 0; i < nbrOfSpawn; ++i)
        {
            int random = Random.Range(0, allInputs.Length);
            GameObject go = Instantiate(allInputs[random], inputParent);
            InstantiatedInput.Add(go);
            InputScript input = go.GetComponent<InputScript>();
            input.Controller = this;
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

    int inputCount;
    public void ActivateCheckableOnNextInput()
    {
        if(inputCount < InstantiatedInput.Count)
        {
            inputCount++;
            InstantiatedInput[inputCount].GetComponent<InputScript>().IsCheckable = true;
        }
        
    }
}
