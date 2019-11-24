using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController {

    [SerializeField] private List<Whisp> whisps = new List<Whisp>();

    public List<Whisp> GetEnemiesInRange(Vector3 position, float range) {
        List<Whisp> whispsInRange = new List<Whisp>();
        foreach (Whisp whisp in whisps) {
            if ((whisp.transform.position - position).sqrMagnitude < range * range) {
                whispsInRange.Add(whisp);
            }
        }
        return whispsInRange;
    }

    protected override void OnInitialize() {
        FragmentController fragmentController = MainController.GetControllerOfType(typeof(FragmentController)) as FragmentController;
        foreach (Whisp whisp in whisps) {
            whisp.Initialize(fragmentController);
        }
    }

    protected override void OnSetup() {
        foreach (Whisp whisp in whisps) {
            whisp.Setup();
        }
    }

    protected override void OnTick(float deltaTime) {
        foreach (Whisp whisp in whisps) {
            whisp.Tick(deltaTime);
        }
    }

    protected override void OnTerminate() {
        foreach (Whisp whisp in whisps) {
            whisp.Terminate();
        }
    }
}
