using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendlyController : BaseController {

    [SerializeField] private List<Robot> robots = new List<Robot>();

    public List<Robot> GetFriendliesInRange(Vector3 position, float range) {
        List<Robot> robotsInRange = new List<Robot>();
        foreach (Robot robot in robots) {
            if ((robot.transform.position - position).sqrMagnitude < range * range) {
                robotsInRange.Add(robot);
            }
        }
        return robotsInRange;
    }

    protected override void OnInitialize() {
        Player player = (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;
        FragmentController fragmentController = MainController.GetControllerOfType(typeof(FragmentController)) as FragmentController;
        foreach (var robot in robots) {
            robot.Initialize(player, fragmentController);
        }
    }

    protected override void OnSetup() {
        foreach (var robot in robots) {
            robot.Setup();
        }
    }

    protected override void OnTick(float deltaTime) {
        foreach (var robot in robots) {
            robot.Tick(deltaTime);
        }
    }

    protected override void OnTerminate() {
        foreach (var robot in robots) {
            robot.Terminate();
        }
    }
}
