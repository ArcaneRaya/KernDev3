using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController {

    public Player player;

    protected override void OnInitialize(List<BaseController> controllers) {
        player.Initialize();
    }

    protected override void OnSetup() {

    }

    protected override void OnTick(float deltaTime) {
        player.Tick(deltaTime);
    }
}
