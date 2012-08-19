using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HerbovoreScript : LiveScript
{

    public GameObject yougOne;
    public ParticleSystem lvlUpParticle;

    protected float energyRefill = 0.01f;
    protected float attackDist;

    public int myExp;
    public int nextLvl;
    protected float nextLvlRatio = 1.01f;
    public int currLvl;


    protected new void Start()
    {
        minDefStrenght = 2.0f;
        maxDefStrenght = 3.0f;

        minEatingDist = 2.0f;
        maxEatingDist = 3.0f;

        base.Start();
        SetStats();
        currLvl = 0;
        myExp = 0;
        nextLvl = 30;
        whatEats = TYPE.HERB;
        attackDist = safeDistance + 1;
        affraidOfEnemy = 1.5f;

        ParticleSystem ps = Instantiate(lvlUpParticle, transform.position, Quaternion.identity) as ParticleSystem;
        ps.Play();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (myExp >= nextLvl)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        myExp -= nextLvl;
        nextLvl = (int)(nextLvl * nextLvlRatio);

        minAttackStrenght *= nextLvlRatio;
        maxAttackStrenght *= nextLvlRatio;

        minDefStrenght *= nextLvlRatio;
        maxEatingDist *= nextLvlRatio;

        minEatingDist *= nextLvlRatio;
        maxEatingDist *= nextLvlRatio;

        maxHP *= nextLvlRatio;
        currHp = maxHP;

        affraidOfEnemy /= nextLvlRatio;
        attackDist *= nextLvlRatio;

        nextLvlRatio *= nextLvlRatio;

        currLvl++;

        myGame.playerLvl = Mathf.Max(currLvl, myGame.playerLvl);

        moveForce *= nextLvlRatio;
        SetStats();
    }

    protected new void FixedUpdate()
    {
        if (currEnergy > 40)
        {
            currHp += energyRefill / 10f;
        }
        if (currEnergy > 50f && enemyDist < +attackDist && enemy != null)
        {
            Destroy(enemy);
            currEnergy -= 50;
            myGame.score += 200;
        }
        TrySplit();
        base.FixedUpdate();
        currEnergy += energyRefill;
    }

    protected void TrySplit()
    {
        if (currEnergy > 70)
        {
            Vector3 position = transform.position;
            position.y += .5f;

            GameObject newOne = (GameObject)Instantiate(yougOne, position, Quaternion.identity);
            myGame.hernivores.Add(newOne);
            HerbovoreScript script = (HerbovoreScript)newOne.GetComponent("HerbovoreScript");
            Vector3 dir = -targetDir;
            script.Move(dir * 10);
            currEnergy -= 60;
            myGame.score += 50;
        }
    }

    protected new void LookForFood()
    {
        target = null;
        List<GameObject> possibleTargets = new List<GameObject>();

        GameObject[] tmp = GameObject.FindGameObjectsWithTag("Plant");
        foreach (GameObject obj in tmp)
        {
            possibleTargets.Add(obj.transform.parent.gameObject);
        }


        foreach (GameObject obj in possibleTargets)
        {
            Vector3 tmpDir = obj.collider.bounds.center - collider.bounds.center;
            Ray ray = new Ray(collider.bounds.center, tmpDir);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == obj.collider)
                {
                    Debug.DrawRay(collider.bounds.center, tmpDir);
                    if (hit.distance < targetDist || target == null)
                    {
                        target = obj;
                        targetDist = hit.distance;
                        targetDir = ray.direction;
                    }
                }
            }
        }
    }


    internal void AddExp(int newExp)
    {
        myExp += newExp;
    }
}
