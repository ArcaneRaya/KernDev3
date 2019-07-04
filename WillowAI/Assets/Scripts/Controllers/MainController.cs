using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
    [SerializeField] private List<BaseController> controllers;
    [SerializeField] private GameObject pausedUIOverlay;

    private bool isGameRunning;

    public void StartGame() {
        isGameRunning = true;
        pausedUIOverlay.SetActive(false);
    }

    public void PauseGame() {
        isGameRunning = false;
        pausedUIOverlay.SetActive(true);
    }

    private void Awake() {
        isGameRunning = false;
        foreach (BaseController controller in controllers) {
            controller.Initialize(controllers);
        }
    }

    private void Start() {
        foreach (BaseController controller in controllers) {
            controller.Setup();
        }
    }

    private void Update() {
        HandleInput();

        if (isGameRunning == false) { return; }

        foreach (BaseController controller in controllers) {
            controller.Tick(Time.deltaTime);
        }
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGameRunning) {
                PauseGame();
            }
            else {
                StartGame();
            }
        }
    }
}
