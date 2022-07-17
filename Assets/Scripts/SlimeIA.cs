 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIA : MonoBehaviour
{
    private Animator animator;
    public int hitPoints;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Died() {
        isDead = true;
        yield return new WaitForSeconds(2.3f);
        Destroy(this.gameObject);
    }

    #region MEUS METODOS
    void GetHit(int amount) {
        if (isDead == true) {return;}

        hitPoints -= amount;

        if (hitPoints > 0) {
            animator.SetTrigger("GetHit");
        } else {
            animator.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }
    #endregion
}
