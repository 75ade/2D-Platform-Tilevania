using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class StateDrivenCameraManager : MonoBehaviour
{
    [SerializeField] List<Animator> animators;
    [SerializeField] CinemachineCamera idleCamera;
    [SerializeField] CinemachineCamera runCamera;
    [SerializeField] CinemachineCamera ladderCamera;

    const string LAYERNAME = "Base Layer";
    const string IDLEKEYWORD = "idling";
    const string RUNKEYWORD = "running";
    const string CLIMBKEYWORD = "climbing";

    CinemachineStateDrivenCamera cinemachineSDC;
    CharacterSelectManager characterSelectManager;
    int currentCharIndex;
    Animator targetAnimator;
    string clipName;
    CinemachineCamera targetCamera;
    float waitTime = 0f;
    float minDuration = 0f;

    void Awake()
    {
        cinemachineSDC = GetComponent<CinemachineStateDrivenCamera>();
        characterSelectManager = FindFirstObjectByType<CharacterSelectManager>();
    }

    void Start()
    {
        SetAnimatedTarget();
        SetInstructions();
    }

    void SetAnimatedTarget()
    {
        currentCharIndex = characterSelectManager.GetCurrentCharacterIndex();
        targetAnimator = animators[currentCharIndex];
        cinemachineSDC.AnimatedTarget = targetAnimator;
    }

    CinemachineStateDrivenCamera.Instruction CreateInstruction()
    {
        string layerNameFormat = LAYERNAME + "." + clipName;
        return new CinemachineStateDrivenCamera.Instruction
        {
            FullHash = Animator.StringToHash(layerNameFormat),
            Camera = targetCamera,
            ActivateAfter = waitTime,
            MinDuration = minDuration
        };
    }

    void SetInstructionProperties(string nameOfClip)
    {
        clipName = nameOfClip;

        if (clipName.Contains(IDLEKEYWORD, System.StringComparison.OrdinalIgnoreCase))
        {
            targetCamera = idleCamera;
            waitTime = 1f;
            minDuration = 0f;
        }
        else if (clipName.Contains(RUNKEYWORD, System.StringComparison.OrdinalIgnoreCase))
        {
            targetCamera = runCamera;
            waitTime = 0f;
            minDuration = 0f;
        }
        else if (clipName.Contains(CLIMBKEYWORD, System.StringComparison.OrdinalIgnoreCase))
        {
            targetCamera = ladderCamera;
            waitTime = 0.5f;
            minDuration = 0f;
        }
    }

    void SetInstructions()
    {
        AnimationClip[] clips = targetAnimator.runtimeAnimatorController.animationClips;
        CinemachineStateDrivenCamera.Instruction[] instructions = new CinemachineStateDrivenCamera.Instruction[clips.Length - 1];
        CinemachineStateDrivenCamera.Instruction instruction;

        for(int i = 0; i < clips.Length - 1; i++)
        {
            SetInstructionProperties(clips[i].name);
            instruction = CreateInstruction();
            instructions[i] = instruction;
        }

        cinemachineSDC.Instructions = instructions;
    }
}
