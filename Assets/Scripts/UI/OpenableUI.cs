using UnityEngine;

public class OpenableUI : MonoBehaviour
{
    private const string IN = "In";
    private const string OUT = "Out";

    [SerializeField] private float outAnimationDuration = 0.3f;
    private Animator animator;
    private float outAnimatintimer = 0;
    private bool isPlayingOutAnimation = false;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (isPlayingOutAnimation)
        {
            outAnimatintimer += Time.deltaTime;
            if (outAnimatintimer >= outAnimationDuration)
            {
                gameObject.SetActive(false);
                isPlayingOutAnimation = false;
                outAnimatintimer = 0;
            }
        }
    }

    protected void Show()
    {
        gameObject.SetActive(true);
        animator.Play(IN);
    }

    protected void Hide()
    {
        animator.Play(OUT);
        isPlayingOutAnimation = true;
    }
}
