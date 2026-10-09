using UnityEngine;

public class SistemActivation : MonoBehaviour
{
        [Header("Comportamiento de los 5 Agentes")]
        public GameObject agenteSeek; //Tecla 1: Seek
    public GameObject agenteArrival; //Tecla 1: Arrival
    public GameObject agenteFlee; //Tecla 2: Flee
        public GameObject agenteWander; //Tecla 3: Wander
        public GameObject agentePursuit; //Tecla 4: Pursuit
        public GameObject agenteEvade; //Tecla 5: Evade


    void Start()
    {
        //Opcional: Apagamos todos al inicio para cumplir con que "no deben estar activos todo el tiempo"

        if (agenteSeek != null) agenteSeek.SetActive(false);
        if (agenteSeek != null) agenteArrival.SetActive(false);
        if (agenteFlee != null) agenteFlee.SetActive(false);
        if (agenteWander != null) agenteWander.SetActive(false);
        if (agentePursuit != null) agentePursuit.SetActive(false);
        if (agenteEvade != null) agenteEvade.SetActive(false);
    }

    void Update()
    {
        //Alpha1 es el número 1 arriba de las teclas (QWERTY)
        //Keypad1 es el número 1 del teclado numérico derecho
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            ToggleAgente(agenteSeek);
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            ToggleAgente(agenteArrival);

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            ToggleAgente(agenteFlee);

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            ToggleAgente(agenteWander);

        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            ToggleAgente(agentePursuit);

        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            ToggleAgente(agenteEvade);


    }

    //Función para cambiar el estado del mono: si está prendido lo apaga y viceversa
    void ToggleAgente(GameObject agente)
    {
        if (agente != null)
        {
            agente.SetActive(!agente.activeSelf);
        }
    }
}