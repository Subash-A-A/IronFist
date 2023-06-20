using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] Image healthSlider;
    [SerializeField] float smooth = 10f;
    [SerializeField] Animator damageAnim;
    private Animator anim;

    private float currentHealth; 

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        healthSlider.fillAmount = Mathf.Lerp(healthSlider.fillAmount, currentHealth/maxHealth, smooth * Time.unscaledDeltaTime);
    }

    public void TakeDamage()
    {
        currentHealth--;
        if(currentHealth <= 0)
        {
            Die();
            return;
        }

        int dir = Random.Range(0, 3);
        anim.SetFloat("HitDir", dir);
        anim.SetTrigger("Hit");
        damageAnim.SetTrigger("Hit");
    }

    public void Dodge(int dir)
    {
        anim.SetFloat("DodgeDir", dir);
        anim.SetTrigger("Dodge");
    }

    public void Die()
    {
        WorldManager.Instance.GameOver();
        anim.SetTrigger("Die");
        damageAnim.SetTrigger("Dead");
    }
}
