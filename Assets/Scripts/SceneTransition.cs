using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator shadowAnimator;
    [SerializeField] private float transitionDuration = 0.1f;
    private const string FADE_OUT = "FadeOut";
    private bool isTransitionPlaying = false;
    private float transitionTimer;

    private void Start()
    {
        Loader.OnSceneLoad += Loader_OnSceneLoad;
    }

    private void OnDestroy()
    {
        Loader.OnSceneLoad -= Loader_OnSceneLoad;
    }

    private void Update()
    {
        if (isTransitionPlaying)
        {
            transitionTimer += Time.deltaTime;
            if (transitionTimer >= transitionDuration)
            {
                isTransitionPlaying = false;
                Loader.LoadLoadingScene();
            }
        }
    }

    private void Loader_OnSceneLoad(object sender, System.EventArgs e)
    {
        shadowAnimator.SetTrigger(FADE_OUT);
        isTransitionPlaying = true;
        transitionTimer = 0f;
    }
}
