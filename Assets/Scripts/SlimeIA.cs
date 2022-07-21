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

    public const float idleWaitTime = 3f;
    public const float patrolWaitTime = 5f;

    // IA
    private NavMeshAgent agent;
    private Vector3 destination;
    private int wayPointId;

    // Start is called before the first frame update
    void Start() {
      _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

      animator = GetComponent<Animator>();
      agent = GetComponent<NavMeshAgent>();

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
          destination = transform.position;
          agent.destination = destination;
          StartCoroutine("IDLE");
          break;

        case enemyState.ALERT:
          break;

        case enemyState.PATROL:
          wayPointId = Random.Range(0, _gameManager.slimeWayPoints.Length);
          destination = _gameManager.slimeWayPoints[wayPointId].position;
          agent.destination = destination;
          StartCoroutine("PATROL");
          break;
      }
    }

    IEnumerator IDLE() {
      yield return new WaitForSeconds(idleWaitTime);
      StayStill(50); // 50% de chance de ficar parado ou entrar em patrulha
    }

    IEnumerator PATROL() {
      yield return new WaitForSeconds(patrolWaitTime);
      StayStill(30); // 30% de chance de ficar parado e 70% de ficar em patrulha
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

    #endregion
}
