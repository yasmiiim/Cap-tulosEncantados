using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portaltuto : MonoBehaviour
{
    public string nomeDaFase; // Nome da próxima fase
    public Animator fadeAnimator; // Animator do Canvas de fade
    private bool isTransitioning = false; // Para evitar múltiplas ativações

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true; // Impede ativações repetidas
            StartCoroutine(TransicaoCena());
        }
    }

    private IEnumerator TransicaoCena()
    {
        // Ativa o fade out
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("End");
        }

        // Aguarda o tempo da animação de fade out
        float fadeOutDuration = GetAnimationDuration("levelend"); // Nome do clipe de fade out no Animator
        yield return new WaitForSeconds(fadeOutDuration);

        // Carrega a próxima cena
        SceneManager.LoadScene(nomeDaFase);
    }

    private float GetAnimationDuration(string animationName)
    {
        if (fadeAnimator == null) return 0f;

        AnimationClip[] clips = fadeAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }

        return 0f; // Retorna 0 caso não encontre a animação
    }
}
