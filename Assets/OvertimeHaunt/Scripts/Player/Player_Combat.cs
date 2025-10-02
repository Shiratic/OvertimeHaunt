using UnityEngine;

public class Player_Combat : MonoBehaviour
{

    public Transform attackPoint;
    public float weaponRange = 1;
    public float knockbackForce = 20;
    public float knockbackTime = .15f;
    public float stunTime = .3f;
    public LayerMask enemyLayer;
    public int damage = 1;
    [SerializeField] AudioClip _attackSound = null;

    public Animator anim;

    public void Attack()
    {
        AudioHelper.PlayClip2D(_attackSound, 0.1f);
        anim.SetBool("isAttacking", true);

      
    }

    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<Enemy_Health>().ChangeHealth(-damage);
            enemies[0].GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
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
