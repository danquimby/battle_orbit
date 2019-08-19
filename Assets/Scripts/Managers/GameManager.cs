using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private MotherDot[] MotherDots;
    public static GameManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this; // Задаем ссылку на экземпляр объекта
        }
        else if (instance == this)
        {
            Destroy(gameObject); // Удаляем объект
        }

        DontDestroyOnLoad(gameObject);
    }
    public void NewGame()
    {
        panel.SetActive(false);
        foreach (MotherDot mather in MotherDots)
        {
            mather.gameObject.SetActive(true);
            mather.Init();
        }
    }
    public void EndGame()
    {
        panel.SetActive(true);
        foreach (MotherDot mather in MotherDots)
        {
            mather.Stop();
            mather.gameObject.SetActive(false);
        }
    }

}
