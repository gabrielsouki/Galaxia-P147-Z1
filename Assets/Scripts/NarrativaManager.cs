using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ListaDeEventos
{
    inicioJuego = 0,
    cabinaPrimerEncuentro = 1,
    encontrarWallet = 2,
    usarBitcoinsSinCurso = 3,
    usarBitcoinsConCurso = 4,
    usarHackeoSinCurso = 5,
    usarHackeoConCurso = 6,
    entrandoSalaDeControl = 7,
    usandoSalaDeControl = 8,
    usandoSalaDeControl_InterfazCursos = 9,
    usandoSalaDeControl_AcabandoCurso = 10,
    enNaveSinSuficientesPiezas = 11,
    enNaveConTodasLasPiezas = 12
};
public class NarrativaManager : MonoBehaviour
{
    public static NarrativaManager sharedInstance;
    public Canvas canvasNarrativa;
    public TMP_Text textoAMostrar;
    public Button BotonPasarSiguiente;
    public Animator m_Animator;
    public GameObject player;
    public List<string[]> dialogos;
    public ListaDeEventos eventoActual;
    
    public bool[] objetosClave = {false,false,false};
    short dialogoActual = 0;
    public bool[] eventosActivados = new bool[13];
private bool gamedone=false;

    public float transitionTime = 3f;
    public Animator screenTransition;
    
    void Awake()
    {

        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        dialogos = new List<string[]>();
        AsignarDialogos();

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
    }

   public void ShowNarrativa()
   {
       switch(eventoActual){
                    case ListaDeEventos.usarBitcoinsConCurso:
                    case ListaDeEventos.usarBitcoinsSinCurso:
                    case ListaDeEventos.usarHackeoConCurso:
                    case ListaDeEventos.usarHackeoSinCurso:
                    case ListaDeEventos.usandoSalaDeControl_InterfazCursos:
                    case ListaDeEventos.usandoSalaDeControl_AcabandoCurso:
                        hideNarrativa();
                        break;
                    case ListaDeEventos.enNaveSinSuficientesPiezas:
                    case ListaDeEventos.enNaveConTodasLasPiezas:
                        if (objetosClave[0] && objetosClave[1] && objetosClave[2])
                            StartCoroutine(LoadLevel("Game Over-ganaste"));
                        break; 
                    default:
                        hideNarrativa();
                        GameManager.sharedInstance.StartGame();
                    break;
                }

      if(!eventosActivados[(int) eventoActual]){
         if((eventoActual == (ListaDeEventos)3 || eventoActual == (ListaDeEventos)4)
         && CursosManager.sharedInstance.cursosTomados[1])
            eventoActual = ListaDeEventos.usarBitcoinsConCurso;

         if((eventoActual == (ListaDeEventos)5 || eventoActual == (ListaDeEventos)6)
            && CursosManager.sharedInstance.cursosTomados[2])
            eventoActual = ListaDeEventos.usarHackeoConCurso;

         if((eventoActual == (ListaDeEventos)11 )
            && (objetosClave[0] && objetosClave[1] && objetosClave[2]))
            eventoActual = ListaDeEventos.enNaveConTodasLasPiezas;

        m_Animator.SetBool("isRunning", false);
        dialogoActual = 0;
        canvasNarrativa.enabled = true;
        MostrarTextoDelDialogo();

         canvasNarrativa.sortingOrder = 2;
      }
      else{
         hideNarrativa();

      }
   }

    public void hideNarrativa()
    {
        dialogoActual = 0;
        canvasNarrativa.enabled = false;
    }

   public void MostrarTextoDelDialogo()
    {
        switch(GameManager.sharedInstance.currentGameState){
        case GameState.inEvent:
        case GameState.inShop:
        case GameState.courseMenu:
            if (dialogos[(int)eventoActual].Length > dialogoActual &&
                !eventosActivados[(int)eventoActual])
            {
                textoAMostrar.text = dialogos[(int)eventoActual][dialogoActual];
                dialogoActual++;
                
            }
            else
            {
                eventosActivados[(int)eventoActual] = true;
                switch(eventoActual){
                    case ListaDeEventos.usarHackeoSinCurso:
                    case ListaDeEventos.usarBitcoinsSinCurso:
                    case ListaDeEventos.enNaveSinSuficientesPiezas:
                        eventosActivados[(int)eventoActual] = false;
                        hideNarrativa();
                        GameManager.sharedInstance.StartGame();                        
                        break;

                    case ListaDeEventos.usarBitcoinsConCurso:
                    case ListaDeEventos.usarHackeoConCurso:
                    case ListaDeEventos.usandoSalaDeControl_InterfazCursos:
                    case ListaDeEventos.usandoSalaDeControl_AcabandoCurso:
                        hideNarrativa();
                        break;
                    case ListaDeEventos.enNaveConTodasLasPiezas:
                            if(gamedone){
                                StartCoroutine(LoadLevel("Game Over-ganaste"));
                            }
                        break; 
                    default:
                        hideNarrativa();
                        GameManager.sharedInstance.StartGame();
                    break;
                }
            }
            break;
        }
    }

    IEnumerator LoadLevel(string levelName)
    {
        //musicAnim.SetTrigger("musicFadeOut");
        screenTransition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }

    public void AsignarDialogos()
    {
        // Al momneto de ir creando los dialogos para cada escena, se van agregando a la lista de dialogos
        string[] inicioJuego = {"Uhh, no puede ser.",
                "Que mala suerte la m??a que mi nave se estropear?? justo cuando regresaba de mis vacaciones, Elon Musk Jr me oir?? muy bien cuando regrese??? .",
                "Si es que regreso. ",
                "??? Este lugar parece desierto, ojal?? me equivoque y haya alguna colonia cerca que me pueda ayudar??? si no, estoy muerta.",
                "Lo bueno que esta nave me puedo proporcionar suficiente ox??geno, pero mi Jet pack no tiene combustible.",
                "Tendr?? que consegu?? un poco en alguna cabina o en algun lugar.",
                "A empezar a buscar."
        };
        dialogos.Add(inicioJuego);

        string[] cabinaPrimerEncuentro ={ "Oh por dios, es una cabina de recarga???. Espera, si hay una cabina de recarga, eso significa que??? ",
                "??Siii!, no estoy sola en este planeta, alguien debi?? de instalar esta cabina, por lo que debe de haber un asentamiento.??Si!...",
                "Ahora solo espero que no est??n muy lejos."
        };
        dialogos.Add(cabinaPrimerEncuentro);

        string[] encontrarWallet ={ "??Que es esto???? oh, es una wallet f??sica de bitcoin??? l??stima que no s?? nada de criptomonedas. Nada mal para alguien del siglo XXlll, eh.",
                "Igual la tomo, tal vez alguien la perdi?? y me de una recompensa por regresarse la??? me conformar??a si me llevara a casa"
        };
        dialogos.Add(encontrarWallet);

        string[] usarBitcoinsSinCurso = { "Oh, una maquina expendedora que usa cripto monedas??? a ver si puedo usarla",
                "...",
                "Nop, mucha tecnolog??a para m??, ??que eso de la llave publica y la llave privada?,nop, na ha",
                "??Y cual es esa criptomoneda???? PlatziCoin, ??ser?? esta una colonia suya para usar esa divisa ende vez del bitcoin?",
        };
        dialogos.Add(usarBitcoinsSinCurso);

        string[] usarBitcoinsConCurso = {
                "Ahora si, gracias a ese curso de criptodivisas que tome, podre dominar las altas finanzas criptotasticas??? y sabre como utilizar esta m??quina expendedora."
        };
        dialogos.Add(usarBitcoinsConCurso);

        string[] usarHackeoSinCurso = {
                "Esta puerta esta cerrada, se ve que quisieron hackearla por sus cables salidos y sus circuitos expuestos??? ojal?? yo tambi??n pudiera hackearla."
        };
        dialogos.Add(usarHackeoSinCurso);

        string[] usarHackeoConCurso = {
                "Esta puerta est?? cerrada, se ve que quisieron hackearla por sus cables salidos y sus circuitos expuestos??? ",
                "Pero ahora con mis nuevas habilidades hacker, hasta podr??a hackear cualquier cuenta de metaFacebook que quisiera???. o tal vez no."
        };
        dialogos.Add(usarHackeoConCurso);

        string[] entrandoSalaDeControl = {
                "No??? no hay nadie??? c??mo es posible que si haya una colonia??? pero no haya nadie.",
                "??? tal vez pueda pedir ayuda por esa computadora central."
        };
        dialogos.Add(entrandoSalaDeControl);

        string[] usandoSalaDeControl = {
                "A ver, espero que pueda comunicarme con alguien que pueda ayudarme a salir",
                "??? ??? ???",
                "N??? No hay comunicaci??n. No puede ser ??Ahora que hare? ??podre sobrevivir en este planeta yo sola? ",
                "??Tengo las suficientes provisiones para resistir?????y el ox??geno!?, no se cuanto haya en esta colonia y no veo ning??n generador de ox??geno funcional???",
                "??Y si reparo yo misma la nave???? pero no s?? nada de naves.",
                "Si, en los ??ltimos a??os, las naves se han vuelto m??s baratas y sencillas de manejar, no por nada ya hay turismo espacial entre galaxias a galaxias (qu?? diablos, yo soy una turista), pero ??podr??a repararla?...",
                "Talvez haya un manual digital de instrucciones en esta computadora???",
                "...",
                "Mmmm no hay nada, pero hay un neura link a la mano??? bueno, no otra opci??n, ya si me da un virus cerebral, vere que hacer al momento."
        };
        dialogos.Add(usandoSalaDeControl);

        string[] usandoSalaDeControl_InterfazCursos = {
                "Mmm ??qu?? es esto?, ??MetaPlatzi? Creo haber escuchado de esto, es una plataforma de ense??anza con cursos online muy popular (ahora con la versi??n NeuraPlatzi).",
                "Pero ??c??mo? Sino hay comunicaciones ni acceso al internet, ser?? como lo que hacen en mi trabajo, un servido privado, pero para toda la colonia.",
                "Hab??a escuchado que tenia miles de cursos, pero ahora solo veo 3??? igual est??n da??ados los servidores. ",
                "Pero eso no importa porque veo que tienen los cursos que necesito para reparar mi nave, ??que suerte!",
        };
        dialogos.Add(usandoSalaDeControl_InterfazCursos);

        string[] usandoSalaDeControl_AcabandoCurso = {
                "Bien, fue muy instructivo y r??pido de aprender, seg??n el curso, debo de tener tres objetos, los planos de la nave, las piezas de respuesta y las herramientas adecuadas.",
                "Debo de buscar en los alrededores los objetos que necesito y regresar a mi nave para repararla.",
                "??Venga, si se puede!"
        };
        dialogos.Add(usandoSalaDeControl_AcabandoCurso);

        string[] enNaveSinSuficientesPiezas = {
                "mmmm, aun no tengo todas las pieza para reparar mi nave, pero gracias al curso de MetaPlatzi siento que puedo reparar la nave yo solito",
                "Si salgo vivo de esta, comprare subcripciones para toda mi desendencia, lo juro"
        };
        dialogos.Add(enNaveSinSuficientesPiezas);

        string[] enNaveConTodasLasPiezas = {
                "Muy bien, hora de reparar mi nave, que mi diploma digital haga su magia.",
                "...",
                "...",
                "Listo, no fue tan dif??cil. Todo listo para partir.",
                "Aunque me pregunto, ??Por qu?? estar?? deshabitada esta colonia? acaso hubo un desastre o algo que espantara a sus habitantes.",
                "Digo, para que una colonia tenga los permisos necesarios para establecerse en un planeta, debe de haber un estudio de viabilidad de 3 a??os o m??s.",
                "??Por qu?? abandonar??an una colonia si tanto esfuerzo requiere? Si hasta dejaron los recursos necesarios para poder sobrevivir a un simple turista espacial como yo...",
                "...",
                "Argh, No lo s??, tal vez vuelva a aqu?? en el futuro, con los conocimientos y recursos suficientes para quedarme una temporada y tratar de descubrir este enigma.",
                "Quiero decir, sino fuera por ellos que dejaron su colonia aqu??, yo habr??a quedado atrapada y sin esperanzas.",
                "Seguro ser?? emocionante."
                };
        dialogos.Add(enNaveConTodasLasPiezas);

    }


}
