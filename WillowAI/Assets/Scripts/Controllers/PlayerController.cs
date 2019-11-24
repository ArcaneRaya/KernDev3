using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController {

    public Player Player;

    protected override void OnInitialize(MainController mainController) {
        Player.Initialize(mainController.GetControllerOfType(typeof(FragmentController)) as FragmentController);
    }

    protected override void OnSetup() {
        Player.Setup();
    }

    protected override void OnTick(float deltaTime) {
        Player.Tick(deltaTime);
    }

    protected override void OnTerminate() {
        Player.Terminate();
    }
}
