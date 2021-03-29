#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class AnimationRecorder : MonoBehaviour
{
    [SerializeField]
    KeyCode keyToStartStopRecording  = KeyCode.Space;

    [SerializeField]
    bool startRecordingUponEnteringPlayMode = false;

    [SerializeField]
    private AnimationClip clipToOverwrite;

    private GameObjectRecorder goRecorder;

    bool canRecord = false;

    void Start()
    {
        if (startRecordingUponEnteringPlayMode)
        {
            canRecord = !canRecord;
        }
        // Create recorder and record the script GameObject.
        goRecorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        goRecorder.BindComponentsOfType<Transform>(gameObject, true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(keyToStartStopRecording))
        {
            canRecord = !canRecord;
            if(!canRecord)
            {
                if (clipToOverwrite == null)
                {
                    return;
                }
                else if (goRecorder.isRecording)
                {
                    // Save the recorded session to the clip.
                    goRecorder.SaveToClip(clipToOverwrite);
                    goRecorder.ResetRecording();
                }
            }
        }
    }

    void LateUpdate()
    {
        if (clipToOverwrite == null || !canRecord)
        {
            return;
        }

        // Take a snapshot and record all the bindings values for this frame.
        goRecorder.TakeSnapshot(Time.deltaTime);
    }

    void OnDisable()
    {
        if (clipToOverwrite == null)
        {
            return;
        }
        else if (goRecorder.isRecording)
        {
            // Save the recorded session to the clip.
            goRecorder.SaveToClip(clipToOverwrite);
        }
    }
}
#endif