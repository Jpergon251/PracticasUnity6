using System;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private GameObject enemy;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    public void mainAction()
    {
        spawnEnemy();
    }

    private void spawnEnemy()
    {
        if (enemy.active)
        {
            enemy.SetActive(false);
        }
        else
        {
            enemy.SetActive(true);
        }
    }
}
