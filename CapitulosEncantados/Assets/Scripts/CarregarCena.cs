using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CarregarCena : MonoBehaviour
{
    public string cenaCarregar;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(cenaCarregar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
