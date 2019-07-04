using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
    [SerializeField] private List<BaseController> controllers;

    private void Awake() {
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
        foreach (BaseController controller in controllers) {
            controller.Tick(Time.deltaTime);
        }
    }
}
