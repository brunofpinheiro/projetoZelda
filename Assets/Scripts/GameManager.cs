using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyState {IDLE, ALERT, EXPLORE, PATROL, FOLLOW, FURY}

public class GameManager : MonoBehaviour {

  [Header("Slime IA")]
  public Transform[] slimeWayPoints;

  // Start is called before the first frame update
  void Start()
  {
      
  }

  // Update is called once per frame
  void Update()
  {
      
  }
}
