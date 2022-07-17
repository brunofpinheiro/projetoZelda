 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIA : MonoBehaviour {
    private Animator animator;
    public int hitPoints;
    private bool isDead = false;
    public enemyState state;

    public const float idleWaitTime = 3f;
    public const float patrolWaitTime = 5f;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        ChangeState(state);
    }

    // Update is called once per frame
    void Update() {
      ManageState();
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

    void ManageState() {
      switch(state) {
        case enemyState.IDLE:
          break;

        case enemyState.ALERT:
          break;

        case enemyState.EXPLORE:
          break;

        case enemyState.FOLLOW:
          break;

        case enemyState.FURY:
          break;

        case enemyState.PATROL:
          break;
      }
    }

    void ChangeState(enemyState newState) {

      StopAllCoroutines();
      state = newState;
      print(newState);

      switch(state) {
        case enemyState.IDLE:
          StartCoroutine("IDLE");
          break;

        case enemyState.ALERT:
          break;

        case enemyState.PATROL:
          StartCoroutine("PATROL");
          break;
      }
    }

    IEnumerator IDLE() {
      yield return new WaitForSeconds(idleWaitTime);
      if (Rand() < 50) {
        ChangeState(enemyState.IDLE);
      } else {
        ChangeState(enemyState.PATROL);
      }
    }

    IEnumerator PATROL() {
      yield return new WaitForSeconds(patrolWaitTime);
      ChangeState(enemyState.IDLE);
    }

    int Rand() {
      return Random.Range(0, 100); //0...99
    }

    #endregion
}
