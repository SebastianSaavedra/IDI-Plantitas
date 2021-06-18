using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoData : MonoBehaviour
{
    SerialPort serial = new SerialPort("COM3", 9600); // Puerto serial

    int datoSonido; // Datos sensor de sonido
    int datoFuego;  // Datos sensor de fuego
    int datoPhotosensor1; // Datos sensor de luz1
    int datoPhotosensor2; // Datos sensor de luz2
    int datoBoton;  // Datos boton

    [SerializeField] GameObject aire, fuego, tierra, luz, sombra;

    void Start()
    {
        if (!serial.IsOpen) // si es que el puerto esta cerrado
        {
            serial.Open();      // se abre
        }
        serial.ReadTimeout = 50; // Tiempo de lectura

    }

    void Update()
    {
        try
        {
            if (serial.IsOpen)
            {
                string[] datos = serial.ReadLine().Split(',');
                datoSonido = int.Parse(datos[0]);
                datoFuego = int.Parse(datos[1]);
                datoPhotosensor1 = int.Parse(datos[2]);
                datoPhotosensor2 = int.Parse(datos[3]);
                datoBoton = int.Parse(datos[4]);

                //Funciones de lectura
                SensorDeSonido();
                SensorDeFuego();
                SensorDeLuz1();
                SensorDeLuz2();
                Boton();
            }
        }

        catch (System.Exception ex)
        {
            ex = new System.Exception();
        }
    }

    void SensorDeSonido()
    {
        if (datoSonido == 1) // si microfono activado
        {
            aire.SetActive(true);
            Debug.Log("Microfono prendido");
        }

        else // si microfono apagado
        {
            aire.SetActive(false);
            Debug.Log("Microfono apagado");
        }
    }
    void SensorDeFuego()
    {
        if (datoFuego == 1)
        {
            fuego.SetActive(true);
            Debug.Log("Fuego Cerca");
        }

        else
        {
            fuego.SetActive(false);
            Debug.Log("No hay Fuego");
        }
    }

    void SensorDeLuz1()
    {
        if (datoPhotosensor1 == 1)
        {
            luz.SetActive(true);
            sombra.SetActive(false);
            Debug.Log("Recibe luz");
        }
        else
        {
            luz.SetActive(false);
            Debug.Log("No recibe Luz");
        }
    }
    void SensorDeLuz2()
    {
        if (datoPhotosensor2 == 1)
        {
            sombra.SetActive(true);
            Debug.Log("Esta muy oscuro");
        }
        else
        {
            sombra.SetActive(false);
            Debug.Log("No esta tan oscuro");
        }
    }
    void Boton()
    {
        if (datoBoton == 1)
        {
            tierra.SetActive(false);
            Debug.Log("No hay tacto");
        }

        else
        {
            tierra.SetActive(true);
            Debug.Log("Hay tacto");
        }

    }
}
