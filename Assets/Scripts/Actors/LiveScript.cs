using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveScript : BasicScript
{

    public GameObject[] tags;

    public GameObject hitParticle;
    protected AudioClip hitSound;

    #region Eating

    public GameObject target = null;
    protected float targetDist;
    protected Vector3 targetDir;
    protected float searchFoodRatio = 0.4f;
    protected bool canSearchFood = true;

    protected TYPE whatEats;
    protected bool canEat = true;
    protected float eatingTime = 0.5f;
    protected float eatingDist = 1.5f;
    protected float minEatingDist = 1.0f;
    protected float maxEatingDist = 2.0f;

    #endregion

    #region Enemy

    protected GameObject enemy = null;
    protected float enemyDist;
    protected Vector3 enemyDir;
    protected float searchEnemyRatio = 0.5f;
    protected bool canSearchEnemy = true;

    public float safeDistance = 4f;
    public float affraidOfEnemy = 2f;
    float panicFactor = 1.5f;

    #endregion

    #region Movement

    protected float runCost = 10f;
    protected float runCooldown = 2.0f;
    protected float runFactor = 100f;
    protected bool canRun;

    public float moveForce = 40f;

    #endregion

    #region Abilities

    public float attackStrenght;
    protected float minAttackStrenght = 1.0f;
    protected float maxAttackStrenght = 1.5f;

    public float currEnergy;
    protected float energyRatio = 1.0f;
    protected float energyUp = 2.0f;
    protected bool canEnergyUp;

    #endregion

    protected new void Start()
    {
        base.Start();

        hitParticle = Resources.Load("Particles/Heal Particles") as GameObject;
        hitSound = Resources.Load("Sounds/eat") as AudioClip;

        currEnergy = Random.Range(10f, 30f);
        type = TYPE.CRITTER | TYPE.MEAT;
        SetStats();
        ChooseTags();
    }

    protected new void SetStats()
    {
        base.SetStats();

        attackStrenght = Random.Range(minAttackStrenght, maxAttackStrenght);
        eatingDist = Random.Range(minEatingDist, maxEatingDist);
    }

    private void ChooseTags()
    {
        canEat = true;
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
        targetDist = float.MaxValue;
        canSearchFood = false;

        List<GameObject> possibleTargets = new List<GameObject>();
        if (whatEats == TYPE.HERB)
        {
            GameObject[] tmp = GameObject.FindGameObjectsWithTag("Plant");
            if (tmp != null)
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
        enemyDist = float.MaxValue;
        enemyDir = Vector3.zero;
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
                StartCoroutine(EatTarget(plantScript.GetHurt(attackStrenght, gameObject)));
                myGame.score += 10;
            }
            else if (whatEats == BasicScript.TYPE.MEAT)
            {
                BasicScript critterScript = (BasicScript)target.GetComponent("HerbovoreScript");
                if (critterScript == null)
                    critterScript = (BasicScript)target.GetComponent("CarnivoreScript");

                if (critterScript != null)
                {
                    StartCoroutine(EatTarget(critterScript.GetHurt(attackStrenght, gameObject)));
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
        DecideWhereToMove();
    }

    private void DecideWhereToMove()
    {
        if (enemyDist <= safeDistance / panicFactor && enemy) // Panic moce on!!
        {
            Vector3 tryDir = Vector3.Cross(Vector3.up, enemy.collider.bounds.center - collider.bounds.center).normalized;

            Ray ray = new Ray(collider.bounds.center, -enemyDir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(collider.bounds.center, hit.point, Color.blue, 1.0f);
                if (hit.distance > safeDistance)
                {
                    TryRun(-targetDir);
                    return;
                }
            }

            float leftDistance = float.MinValue, rightDistance = float.MinValue;
            Vector3 leftDir = -tryDir, rightDir = tryDir;

            ray = new Ray(collider.bounds.center, rightDir);
            if (Physics.Raycast(ray, out hit))
            {
                rightDistance = hit.distance;
                Debug.DrawLine(collider.bounds.center, hit.point, Color.yellow, 1.0f);
            }

            ray = new Ray(collider.bounds.center, leftDir);
            if (Physics.Raycast(ray, out hit))
            {
                leftDistance = hit.distance;
                Debug.DrawLine(collider.bounds.center, hit.point, Color.gray, 1.0f);
            }

            float res = Mathf.Max(new float[] { leftDistance, rightDistance, enemyDist });

            Debug.Log("PANIC PANIC PANIC");
            TryRun(GetPanicDir(leftDistance, rightDistance, ref leftDir, ref rightDir, res));
        }
        else if (enemyDist < safeDistance && enemy) // Avoid enemy
        {
            Debug.Log("Just avoid");
            TryRun(-targetDir);
        }
        else if (target) // Get food
        {
            Debug.Log("Getting food");
            Move(targetDir);
        }
        else // JUMP ! :D
        {
            Move(Vector3.up);
        }
    }

    private Vector3 GetPanicDir(float leftDistance, float rightDistance, ref Vector3 leftDir, ref Vector3 rightDir, float max)
    {
        if (leftDistance == max)
            return leftDir;
        else if (rightDistance == max)
            return rightDir;
        else
            return -targetDir;
    }


    protected IEnumerator EatTarget(float amount)
    {
        canEat = false;
        audio.clip = hitSound;
        audio.Play();
        GameObject ps = Instantiate(hitParticle, target.transform.position, Quaternion.identity) as GameObject;
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

    protected void TryRun(Vector3 dir)
    {
        if (currEnergy >= runCost && canRun)
        {
            Run(dir);
        }
        else
        {
            Move(dir);
        }
    }

    void Destroy()
    {
    }

}
