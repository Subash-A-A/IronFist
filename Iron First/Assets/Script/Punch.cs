using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Punch : MonoBehaviour
{
    [SerializeField] float maxStamina = 5f;
    [SerializeField] float staminaWaitTime = 4f;
    [SerializeField] private float sliderSmooth = 10f;
    [SerializeField] Image staminaSlider;

    public bool playerWait = false;
    private Animator anim;
    private bool isPunching = false;
    private int punchDir = 0;

    private float currentStamina;
    private Coroutine regen;

    private void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.fillAmount = currentStamina;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoPunch(0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            DoPunch(1);
        }

        staminaSlider.fillAmount = Mathf.Lerp(staminaSlider.fillAmount, currentStamina/maxStamina, sliderSmooth * Time.unscaledDeltaTime);
    }
    private void DoPunch(int dir)
    {
        if(!isPunching && !playerWait && currentStamina > 0)
        {
            isPunching = true;
            punchDir = dir;
            anim.SetFloat("PunchDir", punchDir);
            anim.SetTrigger("Punch");
        }
    }

    public void PunchHit(int punchDir)
    {
        EnemyBoss boss = WorldManager.Instance.GetActiveEnemyBoss();

        if (boss != null)
        {
            currentStamina--;

            if(regen != null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(ResetStamina());

            if (boss.CanFeint())
            {
                boss.PerformFeint(punchDir);
            }

            else
            {
                TimeManager.Instance.StopSlowMotion();
                boss.TakeDamage(punchDir);
                AudioManager.Instance.PlayRandomPitch("Punch");
            }
        }
    }

    public void PunchEnd()
    {
        isPunching = false;
    }
    public void PlayerWaitEnd()
    {
        playerWait = false;
    }

    IEnumerator ResetStamina()
    {
        yield return new WaitForSecondsRealtime(staminaWaitTime);
        currentStamina = maxStamina;
        regen = null;
    }
}
