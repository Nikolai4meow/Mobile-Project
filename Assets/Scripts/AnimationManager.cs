using UnityEngine;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance;

    [System.Serializable]
    public class AnimatorReference
    {
        public string id;  
        public Animator animator;
    }

    [SerializeField] List<AnimatorReference> animators = new List<AnimatorReference>();
    public Dictionary<string, Animator> _animatorDict;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        _animatorDict = new Dictionary<string, Animator>();
        foreach (var reff in animators)
        {
            if (!_animatorDict.ContainsKey(reff.id))
            {
                _animatorDict.Add(reff.id, reff.animator);
            }
        }
    }

    public void PlayAnimation(string stateName, string animatorObjectName, int layerIndex = 0, float transitionTime = 0.1f)
    {
        // Find the GameObject with the animator
        GameObject animatorObject = GameObject.Find(animatorObjectName);
        if (animatorObject == null)
        {
            Debug.LogError($"GameObject '{animatorObjectName}' not found");
            return;
        }

        // Get the Animator component
        Animator animator = animatorObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"Animator component not found on '{animatorObjectName}'");
            return;
        }

        // Validate layer index
        if (layerIndex < 0 || layerIndex >= animator.layerCount)
        {
            Debug.LogError($"Invalid layer index: {layerIndex} for animator '{animatorObjectName}'");
            return;
        }

        // Check if state exists
        if (!DoesStateExist(animator, stateName, layerIndex))
        {
            Debug.LogError($"State '{stateName}' not found in layer {layerIndex} of animator '{animatorObjectName}'");
            return;
        }

        // Play the animation
        animator.CrossFade(stateName, transitionTime, layerIndex);
    }
    private bool DoesStateExist(Animator animator, string stateName, int layerIndex)
    {
        AnimatorControllerParameter[] parameters = animator.parameters;
        foreach (var param in parameters)
        {
            if (param.name == stateName) return true;
        }

        // Alternative check using state hashes
        int stateHash = Animator.StringToHash(stateName);
        if (animator.HasState(layerIndex, stateHash))
        {
            return true;
        }

        return false;
    }


    public void RegisterAnimator(string id, Animator animator)
    {
        if (!_animatorDict.ContainsKey(id))
        {
            _animatorDict.Add(id, animator);
        }
    }
}