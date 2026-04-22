using UnityEngine;
using System.Collections;

public class CharacterAnimator : MonoBehaviour
{
    public Animator Animator;

    public void PlayAttackAnimation()
    {
        StartCoroutine(IAttack());
    }

    private IEnumerator IAttack()
    {
        Animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.5f);
        Animator.SetBool("Attack", false);
    }

}
