using UnityEngine;
using System.Collections;

public class BasicScript : MonoBehaviour
{
    public GameObject death;

    public enum TYPE
    {
        UNKNOWN = 0,
        HERB = 1,
        MEAT = HERB << 1,
        CRITTER = MEAT << 1,
    };

    public ParticleSystem healParticle;

    protected TYPE type;

    #region Health

    public float currHp;
    public float maxHP = 15;
    protected float healRatio = 5.0f;
    protected float healUp = 2.0f;
    protected bool canHeal = true;

    #endregion

    public float defendStrenght;
    protected float minDefStrenght = 0.1f;
    protected float maxDefStrenght = 1.0f;
    protected int exp;

    protected void Start()
    {
        exp = 10;
        currHp = maxHP / 2;

        SetStats();
    }

    protected void SetStats()
    {
        defendStrenght = Random.Range(minDefStrenght, maxDefStrenght);
    }

    protected void Update()
    {
        TryHeal();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Death"))
        {
            Debug.Log("Falling damage");
            myGame.hernivores.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    protected void FixedUpdate()
    {
    }

    public float GetHurt(float dmg, GameObject src)
    {
        float damageTaken = dmg - defendStrenght;
        currHp -= dmg;
        if (currHp <= 0)
        {
            damageTaken -= currHp;
            if (src.transform.FindChild("TagPlayer"))
            {

                HerbovoreScript hs = src.GetComponent<HerbovoreScript>();
                Debug.Log(hs.name);
                hs.AddExp(exp);
            }

            myGame.hernivores.Remove(gameObject);
            myGame.carnivores.Remove(gameObject);
            myGame.plants.Remove(gameObject);

            Destroy(gameObject);
        }
        return damageTaken;
    }

    protected void TryHeal()
    {
        if (canHeal)
        {
            StartCoroutine(Heal());
        }
    }

    protected IEnumerator Heal()
    {
        return Heal(healUp);
    }

    public IEnumerator Heal(float amnt)
    {
        return Heal(amnt, healUp);
    }

    public IEnumerator Heal(float amnt, float cooldown)
    {

        canHeal = false;
        Instantiate(healParticle, transform.position, Quaternion.identity);
        currHp += amnt;
        if (currHp > maxHP)
            currHp = maxHP;
        yield return new WaitForSeconds(cooldown);
        canHeal = true;
    }

    protected void OnDestroy()
    {
        Instantiate(death, transform.position, Quaternion.identity);
    }

}
