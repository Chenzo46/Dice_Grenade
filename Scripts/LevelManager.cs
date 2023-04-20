using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private int checksToCollect = 0;
    [SerializeField] private TMP_Text checkText;
    [SerializeField] private Animator scenTransition;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private string levelName;
    private int checksCollected = 0;

    private void Awake()
    {
        instance = this;
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings-1)
        {
            levelText.text = "level: " + SceneManager.GetActiveScene().buildIndex.ToString() + "/" + (SceneManager.sceneCountInBuildSettings - 2).ToString();
            nameText.text = levelName;
            checkText.text = checksCollected.ToString() + "/" + checksToCollect.ToString();
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Fail();
        }
    }

    public void advance()
    {
        scenTransition.SetTrigger("end");
        StartCoroutine(waitForNextScene(SceneManager.GetActiveScene().buildIndex+1));
    }

    public void incChecks()
    {
        checksCollected++;
        checkText.text = checksCollected.ToString() + "/" + checksToCollect.ToString();
    }

    public void Fail()
    {
        scenTransition.SetTrigger("end");
        StartCoroutine(waitForNextScene(SceneManager.GetActiveScene().buildIndex));
    }
    private IEnumerator waitForNextScene(int level)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(level);
    }

    public bool allChecksCollected()
    {
        return checksCollected >= checksToCollect;
    }

}
