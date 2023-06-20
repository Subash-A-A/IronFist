using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private float nextAttackAfter = 10f;
    [SerializeField] private float feintProbability = 0.2f;
    [SerializeField] private float maxHealth = 20f;

    private Animator anim;
    private float hitDir = 0;
    private float currentHitDir = 0;

    private bool playerDodged = false;

    public float currentHealth;
    private bool isDead;

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        InvokeRepeating(nameof(Attack), nextAttackAfter, nextAttackAfter);
    }

    private void Update()
    {
        if (currentHealth <= 0f && !isDead)
        {
            isDead = true;
            Die();
        }

        currentHitDir = Mathf.Lerp(currentHitDir, hitDir, 20 * Time.deltaTime);
        anim.SetFloat("HitDir", currentHitDir);
    }

    private void Attack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"))
        {
            int randomDir = Random.Range(0, 2);
            anim.SetFloat("SwipeDir", randomDir);
            anim.SetTrigger("Swipe");
        }
    }

    public bool CanFeint() 
    {
        bool notSwiping = !anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.SwipeBlend");
        bool notInIntro= !anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.MutantIntro");
        bool notInLanding= !anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Landing");
        float random = Random.Range(0f, 1f);
        bool canFeint = random < feintProbability;
        return canFeint && notInIntro && notInLanding && notSwiping;
    }
    public void PerformFeint(float feintDir)
    {
        IntroBegin();
        anim.SetFloat("FeintDir", feintDir);
        anim.SetTrigger("Feint");
    }
    public void TakeDamage(int punchDir)
    {
        currentHealth--;
        hitDir = punchDir;
        anim.SetTrigger("Hit");
        PlayEnemyBossSoundRandomPitch("BossHit");
    }

    public void DealDamage()
    {
        PlayEnemyBossSoundRandomPitch("BossHit");
        if (playerDodged) 
        {
            return;
        }
        WorldManager.Instance.GetPlayer().TakeDamage();
        WorldManager.Instance.EndSwipe(false);
    }
    public void ImpactEffect()
    {
        WorldManager.Instance.CameraShake();
    }

    private void PlayEnemyBossSound(string soundName)
    {
        AudioManager.Instance.Play(soundName);
    }
    private void PlayEnemyBossSoundRandomPitch(string soundName)
    {
        AudioManager.Instance.PlayRandomPitch(soundName);
    }

    private void IntroBegin()
    {
        WorldManager.Instance.PlayerWaitBegin();
    }
    private void IntroEnd()
    {
        WorldManager.Instance.PlayerWaitEnd();
    }

    private void BeginSwipe(int swipeDir)
    {
        playerDodged = false;
        WorldManager.Instance.InitiateSwipe(swipeDir);
        TimeManager.Instance.StartSlowMotion();
    }

    private void EndSwipe()
    {
        playerDodged = WorldManager.Instance.DidPlayerDodge();
        TimeManager.Instance.StopSlowMotion();
    }

    private void BeginFeint(int feintDir)
    {
        playerDodged = false;
        WorldManager.Instance.InitiateSwipe(feintDir);
        TimeManager.Instance.StartSlowMotion();
    }

    private void EndFeint()
    {
        EndSwipe();
    }

    private void Die()
    {
        IntroBegin();
        anim.SetTrigger("Die");
        Destroy(this);
    }

    public void GameOver()
    {
        Destroy(this);
    }
}
