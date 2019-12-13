using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoSingleton<MainController> {
    public float GameTime { get; private set; }

    [SerializeField] private List<BaseController> controllers = new List<BaseController>();
    [SerializeField] private GameObject pausedUIOverlay = null;
    [SerializeField] private GameObject gameUIOverlay = null;
    [SerializeField] private GameUIUpdater gameUIUpdater = null;

    private bool isGameRunning;

    public void StartGame() {
        isGameRunning = true;
        pausedUIOverlay.SetActive(false);
        gameUIOverlay.SetActive(true);
    }

    public void PauseGame() {
        isGameRunning = false;
        pausedUIOverlay.SetActive(true);
        gameUIOverlay.SetActive(false);
    }

    public static BaseController GetControllerOfType(Type type) {
        if (type.IsSubclassOf(typeof(BaseController)) == false) {
            throw new ArgumentException("Requested type has to be subclass of type: " + typeof(BaseController));
        }

        foreach (BaseController controller in Instance.controllers) {
            if (controller.GetType() == type) {
                return controller;
            }
        }

        throw new ArgumentException("Could not find controller of type: " + type);
    }

    private void Awake() {
        PauseGame();
        foreach (BaseController controller in controllers) {
            controller.Initialize();
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

        GameTime += Time.deltaTime;
        gameUIUpdater.Tick(Time.deltaTime);

        foreach (BaseController controller in controllers) {
            controller.Tick(Time.deltaTime);
        }
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGameRunning) {
                PauseGame();
            } else {
                StartGame();
            }
        }
    }
}
