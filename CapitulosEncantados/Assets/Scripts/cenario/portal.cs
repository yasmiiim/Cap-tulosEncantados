using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class portal : MonoBehaviour
{
    public string sceneToLoad;
    public bool hasStone; // Variável para verificar se o jogador tem a Pedra
    public MensagemPedra mensagemManager; // Referência ao script do gerenciador de mensagem
    public Animator fadeAnimator; // Animator do Canvas de fade
    private bool isTransitioning = false; // Para evitar múltiplas ativações

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            if (hasStone) // Verifica se o jogador tem a Pedra
            {
                isTransitioning = true; // Impede ativações repetidas
                StartCoroutine(TransicaoCena());
            }
            else
            {
                Debug.Log("Você precisa da Pedra para avançar!");
                mensagemManager.MostrarMensagem(); // Mostra a mensagem de texto
            }
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
        SceneManager.LoadScene(sceneToLoad);
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
