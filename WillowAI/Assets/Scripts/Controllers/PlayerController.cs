using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController {

    public Player Player;

    protected override void OnInitialize() {
        FragmentController fragmentController = MainController.GetControllerOfType(typeof(FragmentController)) as FragmentController;
        FriendlyController friendlyController = MainController.GetControllerOfType(typeof(FriendlyController)) as FriendlyController;
        Player.Initialize(fragmentController, friendlyController);
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
