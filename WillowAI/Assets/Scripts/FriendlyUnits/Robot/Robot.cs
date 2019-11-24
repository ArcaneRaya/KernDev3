using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour, IAgent {

    public Vector3 Position { get { return transform.position; } }
    public float Speed { get { return speed; } }

    [SerializeField] private float speed = 5f;
    private Player targetPlayer;

    public void Initialize(Player targetPlayer) {
        this.targetPlayer = targetPlayer;
    }

    public void Setup() {

    }

    public void Tick(float elapsedTime) {

    }

    public void Terminate() {

    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }
}
