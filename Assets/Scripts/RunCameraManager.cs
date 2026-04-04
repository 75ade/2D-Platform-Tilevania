using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class RunCameraManager : MonoBehaviour
{
    [SerializeField] List<Transform> listTransforms;
    CinemachineCamera cinemachineCamera;
    CharacterSelectManager characterSelectManager;


    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        characterSelectManager = FindFirstObjectByType<CharacterSelectManager>();
    }

    void Start()
    {
        SetTrackingTarget();
    }

    void SetTrackingTarget()
    {
        int currentCharIndex = characterSelectManager.GetCurrentCharacterIndex();
        Transform targetTransform = listTransforms[currentCharIndex];
        cinemachineCamera.Target.TrackingTarget = targetTransform;
    }
}
