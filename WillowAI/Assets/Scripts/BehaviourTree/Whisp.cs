using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Whisp : MonoBehaviour, IAgent {
    private Node Root;
    private NavMeshAgent myNavMeshAgent;
    private Rigidbody myRigidbody;
    [SerializeField] private float ViewRange;

    public bool targetSet = false;

    public float distanceWalk = 5f;
    public Vector3 targetPosition;

    public Vector3 Position {
        get {
            return transform.position;
        }
    }

    public float Speed {
        get {
            return speed;
        }
    }

    [SerializeField] private PathfindingAgent pathfindingAgent = null;
    [SerializeField] private float speed = 5;

    public void Initialize() {
        pathfindingAgent.Initialize(this);
        pathfindingAgent.OnDestinationReachedAction += DestinationReached;
    }

    public void OnSetup() {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myRigidbody = GetComponent<Rigidbody>();

        ActionNode<Whisp> SetExploringTargetAction = new Exploring(this);

        Selector RootSelector = new Selector(new List<Node>() { SetExploringTargetAction });
        Sequence RootSequence = new Sequence(new List<Node>() { RootSelector });

        Root = RootSequence;
    }

    public void Tick(float deltaTime) {
        pathfindingAgent.Tick(deltaTime);
        Root.Evaluate(deltaTime);
        MoveTowards(this, deltaTime);
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    private void OnDestroy() {
        pathfindingAgent.OnDestinationReachedAction -= DestinationReached;
    }

    private void DestinationReached() {
        targetSet = false;
    }

    private NodeStates SetExploringTarget(Whisp obj, float deltaTime) {
        if (targetSet)
            return NodeStates.RUNNING;
        else {
            targetPosition = new Vector3(UnityEngine.Random.Range(-distanceWalk, distanceWalk), transform.position.y, UnityEngine.Random.Range(-distanceWalk, distanceWalk));
            targetSet = true;
            return NodeStates.SUCCESS;
        }
    }

    private NodeStates MoveTowards(Whisp obj, float deltaTime) {
        pathfindingAgent.MoveTowards(targetPosition);
        return NodeStates.RUNNING;
    }

    private NodeStates Flee(object obj, float deltaTime) {
        if ((Player.Instance.transform.position - transform.position).sqrMagnitude < ViewRange * ViewRange)
            return NodeStates.RUNNING;

        return NodeStates.FAILURE;
    }

    /*
    NodeStates SetPlayerTarget() {
        Vector3 heading = player.transform.position - transform.position;
        float distPlayer = heading.sqrMagnitude;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, heading.normalized, out hit, ViewRange)) {
            if(hit.collider.gameObject == player) {
                float distPlayerFromLight = Vector3.Distance(GameManager.Instance.LightObject.transform.position, player.transform.position);
                if ((distPlayerFromLight > LightManager.Instance.light.GetComponent<HDAdditionalLightData>().intensity * LightManager.Instance.lightRange || GameManager.Instance.LightMovementScript.Height > GameManager.Instance.LightMovementScript.maxHeight) && distPlayer <= ViewRange * ViewRange) {
                    Target = player.GetComponent<PlayerBehaviour>();
                    return NodeStates.SUCCESS;
                }
            }
        }
        return NodeStates.FAILURE;
    }

    NodeStates SetLightTarget() {
        Vector3 heading = GameManager.Instance.LightObject.transform.position - transform.position;
        float distLight = heading.sqrMagnitude;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, heading.normalized, out hit, ViewRange)) {
            if (hit.collider.gameObject == GameManager.Instance.LightObject) {
                if (distLight <= ViewRange * ViewRange) {
                    Target = GameManager.Instance.LightMovementScript;
                    return NodeStates.SUCCESS;
                }
            }
        }

        return NodeStates.FAILURE;
    }

    public void EndAttack() {
        attacking = false;
        midAttack = false;
    }

    public void MidAttack() {
        midAttack = true;
    }

    NodeStates AttackTarget() {
        myAnimator.SetBool("Scared", false);
        if (jumping)
            return NodeStates.FAILURE;
        if ((heightRemoved - transform.position).sqrMagnitude < stageInformation[AngerStage].AttackRange * stageInformation[AngerStage].AttackRange) {
            if (!attacking) {
                myAnimator.SetTrigger("Attack");
                myAnimator.SetFloat("Speed", 0f);
                targetSpeed = 0f;
                transform.LookAt(heightRemoved);
                transform.rotation = Quaternion.LookRotation(transform.right);
                attacking = true;
            }

            //Debug.Log("Attacking");
            transform.LookAt(heightRemoved);
            transform.Rotate(new Vector3(0f, 50f, 0f));
            return NodeStates.RUNNING;
        }
        return NodeStates.FAILURE;
    }

    NodeStates CheckIfScared() {
        heightRemoved = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);

        if (jumping || preparingJump)
            return NodeStates.FAILURE;
        float speedCalculation = 0f;
        float distShadow = Vector3.Distance(transform.position, LightManager.Instance.AveragePos);
        if (LightManager.Instance.ShadowFound) {
            if (distShadow > 1.0f)
                speedCalculation = stageInformation[AngerStage].Size * distShadow - LightManager.Instance.SizeShadow;
            else
                speedCalculation = stageInformation[AngerStage].Size - LightManager.Instance.SizeShadow;
        }
        else {
            speedCalculation = 50f;
        }
        speedCalculation = Mathf.Clamp(speedCalculation / 50, -stageInformation[AngerStage].Speed, stageInformation[AngerStage].Speed);
        bool positive = speedCalculation >= 0;

        if (positive) {
            scaredTimer = 0.0f;
            return NodeStates.FAILURE;
        }

            return NodeStates.SUCCESS;
    }

    NodeStates MoveToTarget() {
        targetSpeed = stageInformation[AngerStage].Speed;
        myNavMeshAgent.SetDestination(heightRemoved);
        return NodeStates.RUNNING;
    }

    NodeStates Escape() {
        if (scaredTimer <= stageInformation[AngerStage].ScaredTime) { //Play scared animation for ScaredTime amt of seconds
            myAnimator.SetBool("Scared", true);
            targetSpeed = 0f;
            scaredTimer += Time.deltaTime;
        }
        else { //After being scared start running away
            var targetHeading = LightManager.Instance.AveragePos - transform.position;
            var targetDirection = targetHeading / (targetHeading.magnitude);
            targetSpeed = stageInformation[AngerStage].Speed;
            myNavMeshAgent.SetDestination(transform.position - targetDirection);
        }
        return NodeStates.RUNNING;
    }

    NodeStates CircleAroundTarget() {
        Vector3 start = heightRemoved - transform.position;
        if(start.sqrMagnitude <= stageInformation[AngerStage].DistanceToCircleAt * stageInformation[AngerStage].DistanceToCircleAt) { //If the Target is close enough to start circling around it
            if(walkAroundTimer >= attackTime) { //If this entity has been walking around for longer than the attackTime variable
                return NodeStates.FAILURE;
            }
            else {
                //Debug.Log("Circle around target");
                walkAroundTimer += Time.deltaTime;
                Vector3 target = (Quaternion.AngleAxis(10f, Vector3.up) * start.normalized).normalized;

                Vector3 pos = heightRemoved - target * (stageInformation[AngerStage].DistanceToCircleAt - 1f);

                RaycastHit hit;
                if (Physics.Raycast(heightRemoved, target, out hit, stageInformation[AngerStage].DistanceToCircleAt - 1f, layerObstructions)) {
                    pos = hit.point;
                }
                myNavMeshAgent.speed = stageInformation[AngerStage].Speed;
                myAnimator.SetFloat("Speed", myNavMeshAgent.speed);
                myNavMeshAgent.SetDestination(pos);
            }

            return NodeStates.RUNNING;
        }

        return NodeStates.FAILURE;
    }

    public void IncreaseAnger(float amt) {
        AngerLevel += amt;
        if (stageInformation.Length - 1 > AngerStage) {
            if (AngerLevel > stageInformation[AngerStage+1].AngerToStage) {
                AngerStage++;
                AngerLevel = 0f;
                
                if(AngerStage == StageToAddJump) {
                    Root = RootSequenceStageTwo;
                }
                else {
                    Root = RootSequenceStageOne;
                }
            }
        }
    }

    public void JumpOrNotJump() {
        jumping = !jumping;
        preparingJump = false;
        //Debug.Log("Currently jumping: " + jumping.ToString());

        if (!jumping) {
            //End jump
            //Debug.Log("End jump");
            jumpTimer = 0f;
            jumpstart = true;
            transform.position = targetPosition;
            myNavMeshAgent.enabled = true;
        }
        else {
            Vector3 heading = transform.position - heightRemoved;
            Vector3 direction = heading.normalized;
            Vector3 target = Vector3.zero;
            if (heading.magnitude - 1f <= stageInformation[AngerStage].MaxJumpLength) {
                target = heightRemoved + direction * 1f;
            }
            else {
                target = transform.position - direction * stageInformation[AngerStage].MaxJumpLength;
            }
            targetPosition = target;
            jumpTargetSet = true;
            myNavMeshAgent.enabled = false;
        }
    }

    NodeStates Jump() {
        if(jumpTimer == 0f) {
            jumpTimer = Random.Range(stageInformation[AngerStage].MinJumpTime, stageInformation[AngerStage].MaxJumpTime);
        }
        else if(jumpTimer < 0f) {
            if (jumpstart && (transform.position - heightRemoved).magnitude >= stageInformation[AngerStage].MinJumpLength) {
                //Start jump preparing animation
                myAnimator.SetTrigger("Jump");
                targetSpeed = 0f;
                myNavMeshAgent.speed = 0f;

                jumpstart = false;
                preparingJump = true;
                jumpAnimationTimer = 0f;
                jumpStartingPosition = transform.position;
                transform.LookAt(heightRemoved);
                //Debug.Log("Start jump");
            }
            else if (jumping) {                
                //Play jumping animation
                transform.LookAt(heightRemoved);
                jumpAnimationTimer += Time.deltaTime;
                jumpAnimationTimer = Mathf.Clamp(jumpAnimationTimer, 0f, jumpAnimationLength);
                transform.position = Vector3.Lerp(jumpStartingPosition, targetPosition, jumpAnimationTimer / jumpAnimationLength);
                //Debug.Log("Currently jumping");
            }
            else if(!preparingJump && !jumping){
                return NodeStates.FAILURE;
            }
            return NodeStates.RUNNING;
        }
        else {
            jumpTimer -= Time.deltaTime;
        }

        return NodeStates.FAILURE;
    }

    private float GetClipLength(string clipName) {
        RuntimeAnimatorController ac = myAnimator.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == clipName)        //If it has the same name as your clip
            {
                return ac.animationClips[i].length;
            }
        }

        return 0f;
    }

    private void OnTriggerEnter(Collider other) {
        if (Target != null) {
            if (other.gameObject == Target.gameObject && midAttack) {
                Target.TakeDamage(stageInformation[AngerStage].Damage);
                if (Target.CompareTag("Light")) {
                    Destroy(gameObject);
                }
            }
        }
    }*/
}
