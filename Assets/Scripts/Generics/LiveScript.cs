using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveScript : BasicScript
{

    public GameObject[] tags;
    public ParticleSystem particle;
    public AudioClip hitClip;

    #region Eating

    protected GameObject target = null;
    protected float targetDist;
    protected Vector3 targetDir;
    protected float searchFoodRatio = 0.4f;
    protected bool canSearchFood = true;

    protected TYPE whatEats;
    protected bool canEat = true;
    protected float eatingTime = 0.5f;
    protected float eatingDist = 1.5f;

    #endregion

    #region Enemy

    protected GameObject enemy = null;
    protected float enemyDist;
    protected Vector3 enemyDir;
    protected float searchEnemyRatio = 0.5f;
    protected bool canSearchEnemy = true;

    protected float safeDistance = 4f;
    protected float affraidOfEnemy = 2f;

    #endregion

    #region Movement

    protected float runCost = 10f;
    protected float runCooldown = 2.0f;
    protected float runFactor = 100f;
    protected bool canRun;

    protected float moveForce = 40f;

    #endregion

    #region Abilities

    protected float attackStrenght;

    public float currEnergy;
    protected float energyRatio = 1.0f;
    protected float energyUp = 2.0f;
    protected bool canEnertyUp;

    #endregion

    protected new void Start()
    {
        base.Start();

        attackStrenght = Random.Range(0.1f, 1f);

        currEnergy = Random.Range(10f, 30f);
        type = TYPE.CRITTER | TYPE.MEAT;
        ChooseTags();
    }

    private void ChooseTags()
    {
        GameObject tag;
        if (attackStrenght <= 3)
        {
            tag = (GameObject)Instantiate(tags[0]);
        }
        else
        {
            tag = (GameObject)Instantiate(tags[1]);
        }
        canEat = true;
        tag.transform.parent = transform;
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        Eat();
        LookForStuff();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        Move();

    }

    private void LookForStuff()
    {
        if (canSearchFood)
            StartCoroutine(LookForFood());
        if (canSearchEnemy)
            StartCoroutine(LookForEnemy());
    }

    protected IEnumerator LookForFood()
    {
        target = null;
        canSearchFood = false;

        List<GameObject> possibleTargets = new List<GameObject>();
        if (whatEats == TYPE.HERB)
        {
            GameObject[] tmp = GameObject.FindGameObjectsWithTag("Plant");
            if(tmp != null)
            foreach (GameObject obj in tmp)
            {
                possibleTargets.Add(obj.transform.parent.gameObject);
            }
        } if (whatEats == TYPE.MEAT)
        {
            GameObject[] tmp = GameObject.FindGameObjectsWithTag("Critter");
            foreach (GameObject obj in tmp)
            {
                possibleTargets.Add(obj.transform.parent.gameObject);
            }
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
                    Debug.DrawRay(collider.bounds.center, tmpDir, Color.green);
                    if (hit.distance < targetDist || target == null)
                    {
                        target = obj;
                        targetDist = hit.distance;
                        targetDir = ray.direction;
                    }
                }
            }
        }
        yield return new WaitForSeconds(searchFoodRatio);
        canSearchFood = true;
    }

    protected IEnumerator LookForEnemy()
    {
        enemy = null;
        canSearchEnemy = false;

        List<GameObject> possibleEnemys = new List<GameObject>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Critter");
        foreach (GameObject obj in temp)
        {
            GameObject isEnemy = obj.transform.parent.gameObject;
            CarnivoreScript scripts = (CarnivoreScript)isEnemy.GetComponent("CarnivoreScript");
            if (scripts)
            {
                possibleEnemys.Add(isEnemy);
            }

        }

        foreach (GameObject en in possibleEnemys)
        {
            Vector3 tempDir = en.collider.bounds.center - collider.bounds.center;
            Ray ray = new Ray(collider.bounds.center, tempDir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == en.collider)
                {
                    Debug.DrawRay(collider.bounds.center, tempDir);
                    if (enemy == null || enemyDist > hit.distance)
                    {
                        enemy = en;
                        enemyDist = hit.distance;
                        enemyDir = ray.direction;
                    }

                }
            }
        }
        yield return new WaitForSeconds(searchEnemyRatio);
        canSearchEnemy = true;
    }

    private void Eat()
    {
        if (targetDist <= eatingDist && target != null && canEat)
        {
            if (whatEats == TYPE.HERB)
            {
                PlantScript plantScript = (PlantScript)target.GetComponent("PlantScript");
                StartCoroutine(EatTarget(plantScript.GetHurt(attackStrenght)));
                myGame.score += 10;
            }
            else if (whatEats == BasicScript.TYPE.MEAT)
            {
                BasicScript critterScript = (BasicScript)target.GetComponent("HerbovoreScript");
                if (critterScript == null)
                    critterScript = (BasicScript)target.GetComponent("CarnivoreScript");

                if (critterScript != null)
                {
                    StartCoroutine(EatTarget(critterScript.GetHurt(attackStrenght)));
                }
                else
                {
                    Debug.Log("WOOT!! No such script in " + gameObject.name);
                }
            }
        }
    }

    private void Move()
    {

        if (enemy != null && enemyDist <= safeDistance * affraidOfEnemy)
        {
            if (Vector3.Dot(enemyDir.normalized, targetDir.normalized) < 0)
            {
                Debug.Log("On my BAAAAACK");
                if (enemyDist < safeDistance )
                {
                    Vector3 tryDir = Vector3.Cross(Vector3.up, transform.position - enemy.transform.position);
                    Debug.DrawRay(transform.position, tryDir, Color.magenta, 2.0f);

                    Ray ray = new Ray(transform.position, tryDir);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, safeDistance))
                    {
                        Debug.Log("Right clean, GO!");
                        if (canRun)
                            StartCoroutine(Run(tryDir));
                        else
                            Move(tryDir);
                    }
                    else
                    {

                        Debug.Log("Left clean, GO!");
                        if (canRun)
                            StartCoroutine(Run(tryDir));
                        else
                            Move(tryDir);
                    }
                }
                else if (canRun)
                    StartCoroutine(Run(targetDir));
                else
                    Move(targetDir);
            }
            else
            {
                if (canRun)
                    StartCoroutine(Run(-enemyDir));
                else
                    Move(-enemyDir);
            }
        }
        else
        {
            Move(targetDir);
        }
    }


    protected IEnumerator EatTarget(float amount)
    {
        canEat = false;
        audio.clip = hitClip;
        audio.Play();
        Instantiate(particle, target.transform.position, Quaternion.identity);
        currEnergy += amount;
        yield return new WaitForSeconds(eatingTime);
        canEat = true;
    }

    public TYPE WhatEats()
    {
        return whatEats;
    }


    protected void Move(Vector3 dir)
    {
        if (rigidbody != null)
        {
            rigidbody.AddForce(dir * moveForce * Time.deltaTime);
        }
    }

    protected IEnumerator Run(Vector3 dir)
    {
        if (rigidbody != null)
        {
            canRun = false;
            rigidbody.AddForce(dir * moveForce * runFactor * Time.deltaTime);
            currEnergy -= runCost;
            yield return new WaitForSeconds(runCooldown);
            canRun = true;
        }
    }

}