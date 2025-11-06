using UnityEngine;

public class Player_Combat : MonoBehaviour
{

    public Transform attackPoint;
    public float weaponRange = 1;
    public float knockbackForce = 20;
    public float knockbackTime = .15f;
    public float stunTime = .3f;
    public LayerMask enemyLayer;
    public LayerMask breakableLayer;
    public int damage = 1;
    [SerializeField] AudioClip _attackSound = null;

    public Animator anim;
    public float cooldown = 0.5f;
    private float timer;



    private void Update()
    {
        if(timer >0)
        {
            timer -= Time.deltaTime;
        }
    }
    public void Attack()
    {
        if (timer <= 0)
        {
            AudioHelper.PlayClip2D(_attackSound, 0.1f);
            anim.SetBool("isAttacking", true);
            timer = cooldown;
        }
      
    }

    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.isTrigger) continue;


            if (enemies.Length > 0)
            {
                enemies[0].GetComponent<Enemy_Health>().ChangeHealth(-damage);
                enemies[0].GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
            }
        }

        Collider2D[] breakables = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, breakableLayer);
        foreach (Collider2D breakable in breakables)
        {
            Enemy_Health breakableHealth = breakable.GetComponent<Enemy_Health>();
            if (breakableHealth != null)
                breakableHealth.ChangeHealth(-damage);
        }
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}
