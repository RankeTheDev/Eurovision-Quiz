using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    //VARIABLES
    public TextMeshProUGUI questionText; //referencia al componente de texto para mostrar las preguntas
    public TextMeshProUGUI scoreText; //referencia al componente de texto para mostrar la puntuacion
    public TextMeshProUGUI finalScore; //referencia para contener con que mostrar la score final 
    public Button[] replyButtons; //referencia a las respuestas del player
    public DatosPreguntas datosPreguntas; //referencia al scriptable object
    public GameObject right; //referencia al pane de acierto
    public GameObject wrong; //referencia al pane de fallo 
    public GameObject gameFinished; //referencia al pane de juego terminado

    int currentQuestion = 0; //para saber en que pregunta estamos
    [Space]
    public int score = 0; //para almacenar la score del player


    //METHODS

    // Start is called before the first frame update
    void Start()
    {
        SetQuestion(currentQuestion); //Llamo a SetQuestion con el index de la pregunta inicial y desactivo los paneles e right, wrong y game finished
        right.SetActive(false);
        wrong.SetActive(false);
        gameFinished.SetActive(false);
    }

    void SetQuestion(int questionIndex) //defino el metodo setQuestion para preparalo para una pregunta
    {
        questionText.text = datosPreguntas.questions[questionIndex].questionText; //set the text of the question UI

        foreach (Button r in replyButtons) //elimino listeners previos de cada boton de respuesta
        {
            r.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < replyButtons.Length; i++) //añado un loop para ir pasando por los botones de respuesta. Pongo un texto y listener en cada boton.
        {
            replyButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = datosPreguntas.questions[questionIndex].replies[i];
            int replyIndex = i;
            replyButtons[i].onClick.AddListener(() =>
            {
                CheckReply(replyIndex);
            });
        }
    }

    void CheckReply(int replayIndex) //metodo para compobar si la guess es correcta. Sumar score y mostrar paneles a conveniencia.
    {
        if(replayIndex == datosPreguntas.questions[currentQuestion].correctReplyIndex)
        {
            score++;
            scoreText.text = " " + score;

            right.SetActive(true); //activo panel de correcto

            foreach (Button r in replyButtons) //Desactivo todos los botones de respuesta
            {
                r.interactable = false;
            }

            StartCoroutine(Next()); //Siguiente pregunta
        }
        else
        {
            wrong.SetActive(true); //activo panel de fallo

            foreach (Button r in replyButtons) //Desactivo todos los botones de respuesta
            {
                r.interactable = false;
            }

            StartCoroutine(Next()); //Siguiente pregunta
        }
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(2);

        currentQuestion++;

        if (currentQuestion < datosPreguntas.questions.Length)
        {
            Reset(); //Resetea la UI y activa todos los botones de respuestas
        }
        else
        {
            gameFinished.SetActive(true); //Game over

            float scorePercentage = (float)score / datosPreguntas.questions.Length * 100;

            finalScore.text = "Obtuviste un porcentaje de aciertos del " + scorePercentage.ToString("F0") + "%";

            if(scorePercentage < 50)
            {
                finalScore.text += "\n\nFin del Juego, más suerte la próxima vez";
            }
            else if (scorePercentage < 60)
            {
                finalScore.text += "\n\nNo está mal, ¡sigue intentándolo!";
            }
            else if (scorePercentage < 70)
            {
                finalScore.text += "\n\n¡Nada Mal!";
            }
            else if (scorePercentage < 80)
            {
                finalScore.text += "\n\n¡Bien hecho!";
            }
            else
            {
                finalScore.text += "\n\nNi yo podría hacerlo mejor, aún siendo quien lo programó";
            }
        }
    }

    public void Reset() //Desactivo paneles de acierto/error
    {
        right.SetActive(false);
        wrong.SetActive(false);

        foreach (Button r in replyButtons) //Reactivo todos los botones de respuesta
        {
            r.interactable = true;
        }

        SetQuestion(currentQuestion);
    }
}
