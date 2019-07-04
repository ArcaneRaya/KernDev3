using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float speed = 5;

    public void Tick(float deltaTime) {
        Vector3 inputDirection = HandleInput(deltaTime);
        HandleMovement(inputDirection);
    }

    private void HandleMovement(Vector3 inputDirection) {
        transform.position += inputDirection * Time.deltaTime * speed;
    }

    private Vector3 HandleInput(float deltaTime) {
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) {
            moveDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDirection -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDirection -= transform.right;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDirection += transform.right;
        }
        return moveDirection.normalized;
    }
}
