using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{

    private class BoneTransform
    {
        public Vector3 position { get; set; }

        public Quaternion Rotation { get; set; }
    }

    private enum ZombieState
    {
        Walking,
        Ragdoll,
        StandingUp,
        ResettingBones,
        Impaled,
        Attacking,
        PlayingDead
    }

    [SerializeField] 
    private Camera _camera;

    [SerializeField]
    private string _faceUpStandUpStateName;

    [SerializeField]
    private string _faceDownstandUpStateName;

    public string _AttackingStateName;

    [SerializeField]
    private string _faceUpStandUpClipName;

    [SerializeField]
    private string _faceDownStandUpClipName;

    public string _AttackingClipName;

    [SerializeField]
    private float _timeToResetBones;

    private Rigidbody[] _ragdollRigidbodies;

    [SerializeField]
    private ZombieState _currentState = ZombieState.Walking;

    [SerializeField]
    private float AnimatorSpeedTracker;

    private Animator _animator;
    // Allows slope climbing but messes with gun knockback
    private CharacterController _characterController;
    private float _timeToWakeUp;
    private Transform _hipsBone;

    private BoneTransform[] _faceUpStandUpBoneTransforms;
    private BoneTransform[] _faceDownStandUpBoneTransforms;
    private BoneTransform[] _ragdollBoneTransforms;
    private Transform[] _bones;
    private float _elapsedResetBonesTime;
    private bool _isFacingUp;
    public Collider ZombieAttackTriggerCollider;

    public float attackingRotationSpeed;
    public float attackAnimationSpeed = 1;
    public float attackMoveSpeed;
    public float StandUpAnimationSpeed = 1;
    public float walkingRotationSpeed;

    private bool moveTowardsPlayer = false;

    public AttackCollider myDamageCollider;

    Animator animator;
    NavMeshAgent myAgent = null;
    public bool isZombieWalking()
    {
        if (_currentState == ZombieState.Walking)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
        myDamageCollider = GetComponentInChildren<AttackCollider>();
        ZombieAttackTriggerCollider = GetComponent<BoxCollider>();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _hipsBone = _animator.GetBoneTransform(HumanBodyBones.Hips);

        _bones = _hipsBone.GetComponentsInChildren<Transform>();
        _faceUpStandUpBoneTransforms = new BoneTransform[_bones.Length];
        _faceDownStandUpBoneTransforms = new BoneTransform[_bones.Length];
        _ragdollBoneTransforms = new BoneTransform[_bones.Length];

        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            _faceUpStandUpBoneTransforms[boneIndex] = new BoneTransform();
            _faceDownStandUpBoneTransforms[boneIndex] = new BoneTransform();
            _ragdollBoneTransforms[boneIndex] = new BoneTransform();
        }

        PopulateAnimationStartBoneTransforms(_faceUpStandUpClipName, _faceUpStandUpBoneTransforms);
        PopulateAnimationStartBoneTransforms(_faceDownStandUpClipName, _faceDownStandUpBoneTransforms);

        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorSpeedTracker = _animator.speed;
        switch (_currentState)
        {
            case ZombieState.Walking:
                _animator.applyRootMotion = true;
                WalkingBehaviour();
                break;
            case ZombieState.Ragdoll:
                RagdollBehaviour(); 
                break;
            case ZombieState.StandingUp:
                StandingUpBehaviour();
                break;
            case ZombieState.ResettingBones:
                ResettingBonesBehaviour();
                break;
            case ZombieState.Attacking:
                _animator.applyRootMotion = false;
                Attacking();
                break;
            case ZombieState.PlayingDead:
                PlayingDeadBehaviour();
                break;  
        }
    }

    private void PlayingDeadBehaviour()
    {
        // Keep the zombie in ragdoll state
        EnableRagdoll();
        //_timeToWakeUp = 0;
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _camera.transform.position);
        if (distanceToPlayer < 5.0f) // Adjust the distance threshold as needed
        {
            SetHips();
            _currentState = ZombieState.ResettingBones; // Change state to wake up
            _timeToWakeUp = 1; // Force wake up
        }
    }

    // temp work in progress from last session (trying to avoid animation replaying from start if already in state)
    private void Attacking()
    {
        
        // Check if current state is not equal to Attacking
        if (_currentState != ZombieState.Attacking)
        {
            ZombieAttackTriggerCollider.enabled = false;
            _animator.Play(_AttackingStateName, 0, 0f);
            _animator.speed = attackAnimationSpeed;
            AnimatorSpeedTracker = _animator.speed;

            _currentState = ZombieState.Attacking;
        }
        else if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_AttackingStateName))
        {
            _currentState = ZombieState.Walking;
            ZombieAttackTriggerCollider.enabled = true;
        }

        // Move towards the player during the attack
        Vector3 direction = _camera.transform.position - transform.position;
        direction.y = 0; // Keep the movement horizontal
        direction.Normalize();

        // You can adjust the speed of the zombie's movement towards the player during the attack
        if (moveTowardsPlayer == true)
        {
            transform.position += transform.forward * attackMoveSpeed * Time.deltaTime;
        }

        // Rotate the zombie to face the player during the attack
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, attackingRotationSpeed * Time.deltaTime);
    }

    public void StartMoving()
    {
        myAgent.enabled = false;
        moveTowardsPlayer = true;
    }

    public void StopMovingTowardsPlayer()
    {
        myAgent.enabled = true;
        myAgent.Warp(transform.position);
        moveTowardsPlayer = false;
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        if (_currentState == ZombieState.Impaled)
        {
            return;
        }

        _characterController.enabled = false;
        EnableRagdoll();

        //Rigidbody hitRigidbody = _ragdollRigidbodies.OrderBy(Rigidbody => Vector3.Distance(Rigidbody.position, hitPoint)).First();

        //hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

        _currentState = ZombieState.Ragdoll;
        _timeToWakeUp = Random.Range(2, 4);
    }

    public void Impale()
    {
        myAgent.enabled = false;
        _currentState = ZombieState.Impaled;
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
       _characterController.enabled = true;
    }


    private void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        myAgent.enabled = false;
        _animator.enabled = false;
        ZombieAttackTriggerCollider.enabled = false;
        //_characterController.enabled = false;
    }
    private void WalkingBehaviour()
    {
        myAgent.enabled = true;
        if (_currentState == ZombieState.Walking)
        {
            if (myAgent != null)
            {
                animator.speed = myAgent.speed;
                AnimatorSpeedTracker = animator.speed;
                myAgent.destination = _camera.transform.position; // or any target position
            }
        }
    }

    private void RagdollBehaviour()
    {
        _animator.speed = StandUpAnimationSpeed;
        moveTowardsPlayer = false;
        _timeToWakeUp -= Time.deltaTime;

        if (_timeToWakeUp <= 0)
        {
            SetHips();
        }
    }

    private void SetHips()
    {
        _isFacingUp = _hipsBone.forward.y > 0;

        AlignRotationToHips();
        AlignPositionToHips();

        PopulateBoneTransforms(_ragdollBoneTransforms);

        _currentState = ZombieState.ResettingBones;
        _elapsedResetBonesTime = 0;
    }

    private void StandingUpBehaviour()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(GetStandUpStateName()) == false)
        {
            _currentState = ZombieState.Walking;
            ZombieAttackTriggerCollider.enabled = true;
        }
    }

    private void ResettingBonesBehaviour()
    {
        _elapsedResetBonesTime += Time.deltaTime;
        float elapsedPercentage = _elapsedResetBonesTime / _timeToResetBones;

        BoneTransform[] standUpBoneTransforms = GetStandUpBoneTransforms();

        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            _bones[boneIndex].localPosition = Vector3.Lerp(
                _ragdollBoneTransforms[boneIndex].position,
                standUpBoneTransforms[boneIndex].position,
                elapsedPercentage );

            _bones[boneIndex].localRotation = Quaternion.Lerp(
                _ragdollBoneTransforms[boneIndex].Rotation,
                standUpBoneTransforms[boneIndex].Rotation,
                elapsedPercentage);
        }

        if (elapsedPercentage >=1)
        {
            _currentState = ZombieState.StandingUp;
            DisableRagdoll();

            _animator.Play(GetStandUpStateName(), 0, 0f);
        }
    }

    private void AlignRotationToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        Quaternion originalHipsRotation = _hipsBone.rotation;

        Vector3 desiredDirection = _hipsBone.up;

        if (_isFacingUp)
        {
            desiredDirection *= -1;
        }

        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
        transform.rotation *= fromToRotation;

        _hipsBone.position = originalHipsPosition;
        _hipsBone.rotation = originalHipsRotation;
    }

    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        transform.position = _hipsBone.position;

        Vector3 positionOffset = GetStandUpBoneTransforms()[0].position;
        positionOffset.y = 0;
        positionOffset = transform.rotation * positionOffset;
        transform.position -= positionOffset;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }

        _hipsBone.position = originalHipsPosition;
    }

    private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
    {
        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            boneTransforms[boneIndex].position = _bones[boneIndex].localPosition;
            boneTransforms[boneIndex].Rotation = _bones[boneIndex].localRotation; 
        }
    }

    private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
    {
        Vector3 positionBeforeSampling = transform.position;
        Quaternion rotationBeforeSampling = transform.rotation;


        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                clip.SampleAnimation(gameObject, 0);
                PopulateBoneTransforms(boneTransforms);
                break;
            }
        }

        transform.position = positionBeforeSampling;
        transform.rotation = rotationBeforeSampling;
    }


    private string GetStandUpStateName()
    {
       // return _isFacingUp ? _faceUpStandUpStateName : _faceDownstandUpStateName;
        if (_isFacingUp == true)
        {
            return _faceUpStandUpStateName;
        }
        else
        {
            return _faceDownstandUpStateName;
        }
    }

    private BoneTransform[] GetStandUpBoneTransforms()
    {
        if (_isFacingUp == true)
        {
            return _faceUpStandUpBoneTransforms;
        }
        else
        {
            return _faceDownStandUpBoneTransforms;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Attacking();
        }
    }

    public void EnableDamage()
    {
        myDamageCollider.canDamagePlayer = true;
    }
    public void DisableDamage()
    {
        myDamageCollider.canDamagePlayer = false;
    }
}
