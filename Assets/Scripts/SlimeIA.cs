using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeIA : MonoBehaviour {
  private GameManager _gameManager;

    private Animator animator;
    public int hitPoints;
    private bool isDead = false;
    public enemyState state;
    
    // IA
    private bool isWalk;
    private bool isAlert;
    private bool isAttacking;
    private bool isPlayerVisible;
    private NavMeshAgent agent;
    private Vector3 destination;
    private int wayPointId;

    // Start is called before the first frame update
    void Start() {
      _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

      animator = GetComponent<Animator>();
      agent = GetComponent<NavMeshAgent>();

      // ChangeState(state);
    }

    // Update is called once per frame
    void Update() {
      ManageState();
      isWalk = agent.desiredVelocity.magnitude >= 0.1f;
      animator.SetBool("isWalk", isWalk);
      animator.SetBool("isAlert", isAlert);
    }

    IEnumerator Died() {
      isDead = true;
      yield return new WaitForSeconds(2.3f);
      Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other) {
      if (other.gameObject.tag == "Player" && (state == enemyState.IDLE || state == enemyState.PATROL)) {
        isPlayerVisible = true;
        ChangeState(enemyState.ALERT);
      }
    }

    private void OnTriggerExit(Collider other) {
      if (other.gameObject.tag == "Player") {
        isPlayerVisible = false;
      }
    }

    #region MEUS METODOS
    void GetHit(int amount) {
        if (isDead == true) {return;}

        hitPoints -= amount;

        if (hitPoints > 0) {
            ChangeState(enemyState.FURY);
            animator.SetTrigger("GetHit");
        } else {
            animator.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }

    void ManageState() {
      switch(state) {
        case enemyState.FOLLOW:
          destination = _gameManager.player.position;
          agent.destination = destination;

          if (agent.remainingDistance <= agent.stoppingDistance) {
            Attack();
          }

          break;

        case enemyState.FURY:
          destination = _gameManager.player.position;
          agent.destination = destination;

          if (agent.remainingDistance <= agent.stoppingDistance) {
            Attack();
          }
          
          break;

        case enemyState.PATROL:
          break;
      }
    }

    void ChangeState(enemyState newState) {

      StopAllCoroutines();
      isAlert = false;

      switch(newState) {
        case enemyState.IDLE:
          agent.stoppingDistance = 0;
          destination = transform.position;
          agent.destination = destination;
          StartCoroutine("IDLE");
          break;

        case enemyState.ALERT:
          agent.stoppingDistance = 0;
          destination = transform.position;
          agent.destination = destination;
          isAlert = true;
          StartCoroutine("ALERT");
          break;

        case enemyState.PATROL:
          agent.stoppingDistance = 0;
          wayPointId = Random.Range(0, _gameManager.slimeWayPoints.Length);
          destination = _gameManager.slimeWayPoints[wayPointId].position;
          agent.destination = destination;
          StartCoroutine("PATROL");
          break;

        case enemyState.FOLLOW:
          agent.stoppingDistance = _gameManager.slimeDistanceToAttack;
          StartCoroutine("FOLLOW");
          break;

        case enemyState.FURY:
          destination = transform.position;
          agent.stoppingDistance = _gameManager.slimeDistanceToAttack;
          agent.destination = destination;
        break;
      }

      state = newState;
    }

    IEnumerator IDLE() {
      yield return new WaitForSeconds(_gameManager.slimeIdleWaitTime);
      StayStill(50); // 50% de chance de ficar parado ou entrar em patrulha
    }

    IEnumerator PATROL() {
      yield return new WaitUntil(() => agent.remainingDistance <= 0);
      StayStill(30); // 30% de chance de ficar parado e 70% de ficar em patrulha
    }

    IEnumerator ALERT() {
      yield return new WaitForSeconds(_gameManager.slimeAlertTime);

      if (isPlayerVisible) {
        ChangeState(enemyState.FOLLOW);
      } else {
        StayStill(10);
      }
    }

    IEnumerator FOLLOW() {
      yield return new WaitUntil(() => !isPlayerVisible);
      print("perdi voce");

      yield return new WaitForSeconds(_gameManager.slimeAlertTime);
      StayStill(50);
    }

    IEnumerator ATTACK() {
      yield return new WaitForSeconds(_gameManager.slimeAttackDelay);
      isAttacking = false;
    }

    void StayStill(int yes) {
      if (Rand() < yes) {
        ChangeState(enemyState.IDLE);
      } else {
        ChangeState(enemyState.PATROL);
      }
    }

    int Rand() {
      return Random.Range(0, 100); //0...99
    }

    void Attack() {
      if (!isAttacking) {
        isAttacking = true;
        animator.SetTrigger("Attack");
      }
    }

    void AttackIsDone() {
      StartCoroutine("ATTACK");
    }

    #endregion
}
