using System.Collections;
using UnityEngine;

public class ShatteredCube : MonoBehaviour
{
    private Animator animatorComponent;

    private void Awake()
    {
        animatorComponent = GetComponent<Animator>();
    }

    public void PlayShatterAnimation()
    {
        animatorComponent.enabled = true;

        StartCoroutine(DisableAfterAnimation(1f));
    }

    private IEnumerator DisableAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
