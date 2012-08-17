using UnityEngine;
using System.Collections;

public class BasicScript : MonoBehaviour
{

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
    protected float maxHP = 15;
    protected float healRatio = 5.0f;
    protected float healUp = 2.0f;
    protected bool canHeal = true;

    #endregion

    protected float defendStrenght;

    protected void Start()
    {
        currHp = maxHP / 2;
        defendStrenght = Random.Range(0.1f, 1f);
    }

    protected void Update()
    {
        TryHeal();
    }

    protected void FixedUpdate()
    {
    }

    public float GetHurt(float dmg)
    {
        float damageTaken = dmg - defendStrenght;
        currHp -= dmg;
        if (currHp <= 0)
        {
            damageTaken -= currHp;
            myGame.hernivores.Remove(gameObject);
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

}
