using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController {

    [SerializeField] private List<Whisp> whisps = new List<Whisp>();
    [SerializeField] private List<Willow> willows = new List<Willow>();

    protected override void OnInitialize(List<BaseController> controllers) {
        foreach (Whisp whisp in whisps) {
            whisp.Initialize();
        }
        foreach (Willow willow in willows) {
            willow.Initialize();
        }
    }

    protected override void OnSetup() {

    }

    protected override void OnTick(float deltaTime) {
        foreach (Whisp whisp in whisps) {
            whisp.Tick(deltaTime);
        }
        foreach (Willow willow in willows) {
            willow.Tick(deltaTime);
        }
    }
}
