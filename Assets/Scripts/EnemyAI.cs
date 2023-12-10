using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyPathfinding))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    private enum State {
        Roaming
    }

    private Rigidbody2D rb;
    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake() {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        rb = GetComponent<Rigidbody2D>();
        state = State.Roaming;
    }

    private void Start() {
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine() {
        while (state == State.Roaming) {
            Vector2 roamDestination = GetRoamingDestination();
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingDestination() {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}