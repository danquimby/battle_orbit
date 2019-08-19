using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherDot : ActorBase 
{
    public GameObject EnemyMotherDot;
    [SerializeField]
    private int healBegin;
    private int heal;
    [SerializeField]
    private string tagDot;
    [SerializeField]
    private float timeout;
    private bool stop;
    [SerializeField]
    private int max_active_dots;

    [SerializeField]
    private List<Dot> dots = new List<Dot>(); // дотики на орбите))

    private int active_count;
    private bool isSelected;

    public void Init()
    {
        heal = healBegin;
        dots.Clear();
        stop = false;
        StartCoroutine(Respawn());

    }
    public override void Hit()
    {
        heal -= 1;
        if (heal == 0)
        {
            Stop(); // останавливаем спавн dot
            Release();
            GameManager.instance.EndGame();
        }
    }
    IEnumerator Respawn()
    {
        while (!stop)
        {
            yield return new WaitForSeconds(timeout);
            //бавает такое что спутники сбивают друг друга на орбите это фикс(костыль)
            for (int i = 0; i < dots.Count; i++)
            {
                Dot dot = dots[i];
                if (!dot.gameObject.activeSelf)
                    dots.RemoveAt(i);
            }
            if (max_active_dots >= dots.Count)
            {
                try
                {
                    GameObject _obj = PoolManager.instance.getPoolObject(GameObjectType.Dot);
                    _obj.transform.position = new Vector3(-300, 0, 0);
                    _obj.SetActive(true);
                    _obj.tag = tagDot;
                    Dot dot = CastToObj<Dot>(_obj);
                    dot.Init(this);
                    dots.Add(dot);
                }
                catch (Exception e){
                    Console.error(e.ToString());                
                }
            }
        }
    }
    // клик на материнскую планету
    private void OnMouseDown()
    {
        isSelected = !isSelected;
        if (isSelected)
            UpdateDots();
        else
            AttackDots();
    }
    private void AttackDots()
    {
        // в атаку !!
        foreach (Dot dot in dots)
        {
            if (dot.isSelected)
                dot.Attack(EnemyMotherDot.transform);
        }
        // убираем из списка кто вылетел с орбиты они нам не нужны
        for (int i = 0; i < dots.Count; i++)
        {
            Dot dot = dots[i];
            if (dot.isSelected)
                dots.RemoveAt(i);
        }
    }
    private void UpdateDots()
    {
        foreach (Dot dot in dots)
        {
            dot.UpdateColor(isSelected);
        }
    }

    public override void Stop()
    {
        stop = true;
        // все игра закончилась )) все кто не в полете тю тю
        for (int i = 0; i < dots.Count; i++)
        {
            Dot dot = dots[i];
            dots.RemoveAt(i);
            dot.Release();
        }

    }
    public static T CastToObj<T>(GameObject _object)
    {
        return _object.gameObject.GetComponent<T>();
    }

}