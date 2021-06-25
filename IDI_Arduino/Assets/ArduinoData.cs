using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using DG.Tweening;

public class ArduinoData : MonoBehaviour
{
    SerialPort serial = new SerialPort("COM3", 9600); // Puerto serial

    int datoSonido; // Datos sensor de sonido
    int datoFuego;  // Datos sensor de fuego
    int datoPhotosensor1; // Datos sensor de luz1
    int datoPhotosensor2; // Datos sensor de luz2
    int datoSensorHumedad; //Datos del sensor de humedad
    int datoBoton;  // Datos boton

    bool hayLuz;
    bool activo1,activo2,activo3,activo4,activo5,activo6,activo7,activo8,activo9;
    bool crecio;

    [SerializeField] float duration = 1.5f;

    [SerializeField] Animator anim;

    [SerializeField] SpriteRenderer petaloSOMBRABASE,petaloLUZBASE,pistilloSOMBRABASE,pistilloLUZBASE,petaloMarcoLUZ,pistilloMarcoLUZ,petaloMarcoSombra,pistilloMarcoSombra;


    [SerializeField] List<SpriteRenderer> petalosAguaLUZ, petalosAireLUZ, petalosFuegoLUZ, petalosTierraLUZ, petalosAguaSOMBRA, petalosAireSOMBRA, petalosFuegoSOMBRA, petalosTierraSOMBRA;
    [SerializeField] List<SpriteRenderer> pistilloAguaLUZ, pistilloAireLUZ, pistilloFuegoLUZ, pistilloTierraLUZ, pistilloAguaSOMBRA, pistilloAireSOMBRA, pistilloFuegoSOMBRA, pistilloTierraSOMBRA;
    [SerializeField] List<SpriteRenderer> tresElementosPetalos, tresElementosPistillo, cuatroElementosPetalos, cuatroElementosPistillo;

    [SerializeField] List<GameObject> luces, sombras;

    void Start()
    {
        if (!serial.IsOpen) // si es que el puerto esta cerrado
        {
            serial.Open();      // se abre
        }
        serial.ReadTimeout = 30; // Tiempo de lectura
        InvokeRepeating("Serial_Data",0f,.01f);
    }

    void Serial_Data()
    {
        try
        {
            if (serial.IsOpen)
            {
                //Lectura de datos
                string[] datos = serial.ReadLine().Split(',');
                datoSonido = int.Parse(datos[0]);
                datoFuego = int.Parse(datos[1]);
                datoPhotosensor1 = int.Parse(datos[2]);
                datoPhotosensor2 = int.Parse(datos[3]);
                datoSensorHumedad = int.Parse(datos[4]);
                datoBoton = int.Parse(datos[5]);
            }
        }

        catch (System.Exception ex)
        {
            ex = new System.Exception();
        }

    }

    void Update()       // PAUSAR ANIMACIÓN Y ESPERAR A QUE LA PLANTA CREZCA DE NUEVO (LUZ Y SOMBRA) Y MEZCLAR POR LO MENOS UN PAR DE WEAS //
    {
        SensorDeSonido();
        SensorDeFuego();
        SensorDeLuz1();
        SensorDeLuz2();
        SensorDeHumedad();
        Boton();

        //fusion
        //Test();
        //
    }

    void Test()     // Mezclas 
    {
        if (datoBoton == 1 && datoSensorHumedad == 1)
        {
            activo7 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                petalosAguaLUZ[2].DOFade(1f,duration);
                pistilloAguaLUZ[2].DOFade(1f,duration);
            }

            else
            {
                ActivarMarcoSombra();
                petalosAguaSOMBRA[2].DOFade(1f,duration);
                pistilloAguaSOMBRA[2].DOFade(1f,duration);
            }

        }

        //else if (datoBoton == 1 && datoSensorHumedad == 1 && datoFuego == 1) //va a chocar con el if de arriba
        //{
        //    activo8 = true;
        //    if (hayLuz)
        //    {
        //        ActivarMarcoLuz();
        //        tresElementosPetalos[0].DOFade(1f,duration);
        //        tresElementosPistillo[0].DOFade(1f,duration);
        //    }

        //    else
        //    {
        //        ActivarMarcoSombra();
        //        tresElementosPetalos[1].DOFade(1f, duration);
        //        tresElementosPistillo[1].DOFade(1f, duration);
        //    }
        //}

        //else if ()
        //{

        //}

        else
        {
            if (activo7 || activo8 || activo9)
            {
                petalosAguaLUZ[2].DOFade(0f, duration);
                pistilloAguaLUZ[2].DOFade(0f, duration);
                petalosAguaSOMBRA[2].DOFade(0f, duration);
                pistilloAguaSOMBRA[2].DOFade(0f, duration);
                tresElementosPetalos[0].DOFade(0f, duration);
                tresElementosPistillo[0].DOFade(0f, duration);
                Desactivar();

            }
        }
    }

    void SensorDeSonido()
    {
        if (datoSonido == 1) // si microfono activado
        {
            activo1 = true;
            if (hayLuz && datoFuego == 0 && datoBoton == 0 && datoSensorHumedad == 0)   // 1 sensor activado
            {
                ActivarMarcoLuz();
                petalosAireLUZ[0].DOFade(1f,duration);
                pistilloAireLUZ[0].DOFade(1f,duration);
            }
            else if(!hayLuz && datoFuego == 0 && datoBoton == 0 && datoSensorHumedad == 0)   // Oscuridad
            {
                ActivarMarcoSombra();
                petalosAireSOMBRA[0].DOFade(1f,duration);
                pistilloAireSOMBRA[0].DOFade(1f,duration);
            }
            Debug.Log("Microfono prendido");
        }

        else // si microfono apagado
        {
            if(activo1)
            {
                petalosAireLUZ[0].DOFade(0f, duration);
                pistilloAireLUZ[0].DOFade(0f, duration);
                petalosAireSOMBRA[0].DOFade(0f, duration);
                pistilloAireSOMBRA[0].DOFade(0f, duration);
                Desactivar();
                Debug.Log("Microfono apagado");
            }
        }
    }
    void SensorDeFuego()
    {
        if (datoFuego == 1)
        {
            activo2 = true;
            if(hayLuz && datoSonido == 0 && datoBoton == 0 && datoSensorHumedad == 0)   // 1 sensor activado
            {
                ActivarMarcoLuz();
                petalosFuegoLUZ[0].DOFade(1f,duration);
                pistilloFuegoLUZ[0].DOFade(1f,duration);
            }
            else if(!hayLuz && datoSonido == 0 && datoBoton == 0 && datoSensorHumedad == 0)  // Oscuridad
            {
                ActivarMarcoSombra();
                petalosFuegoSOMBRA[0].DOFade(1f,duration);
                pistilloFuegoSOMBRA[0].DOFade(1f,duration);
            }
            Debug.Log("Fuego Cerca");
        }

        else
        {
            if (activo2)
            {
                petalosFuegoLUZ[0].DOFade(0f, duration);
                pistilloFuegoLUZ[0].DOFade(0f, duration);
                petalosFuegoSOMBRA[0].DOFade(0f, duration);
                pistilloFuegoSOMBRA[0].DOFade(0f, duration);
                Desactivar();
                Debug.Log("No hay Fuego");
            }
        }
    }

    void SensorDeLuz1()
    {
        if (datoPhotosensor1 == 1)
        {
            if(!crecio)
            {

                anim.Play("Base Layer.FlorCrece");
                crecio = true;
            }

            if (datoSonido == 0 && datoSensorHumedad == 0 && datoFuego == 0 && datoBoton == 0)   // 1 sensor activado
            {
                if (luces[0].activeSelf == false) // activa todos los GameObjects de luz de vuelta.
                {
                    foreach (GameObject go in luces)
                    {
                        go.SetActive(true);
                    }
                }

                if (sombras[0].activeSelf == true)
                {
                    foreach (GameObject go in sombras)
                    {
                        go.SetActive(false);
                    }
                }

                hayLuz = true;
                //activo3 = true;
                ActivarMarcoLuz();

                petaloLUZBASE.DOFade(1f, duration);
                pistilloLUZBASE.DOFade(1f, duration);
                Debug.Log("Recibe luz");
            }
        }
        //else
        //{
        //    if (activo3)
        //    {
        //        //petaloLUZBASE.DOFade(0f, duration);
        //        //pistilloLUZBASE.DOFade(0f, duration);
        //        //petaloMarcoLUZ.DOFade(0f,duration);
        //        //pistilloMarcoLUZ.DOFade(0f,duration);
        //        activo3 = false;
        //        Debug.Log("No recibe Luz");
        //    }
        //}
    }
    void SensorDeLuz2()
    {
        if (datoPhotosensor2 == 1)
        {
            if (!crecio)
            {

                anim.Play("Base Layer.FlorCrece");
                crecio = true;
            }

            if (datoSonido == 0 && datoSensorHumedad == 0 && datoFuego == 0 && datoBoton == 0)   // 1 sensor activado
            {
                if (sombras[0].activeSelf == false) // activa todos los GameObjects de luz de vuelta.
                {
                    foreach (GameObject go in sombras)
                    {
                        go.SetActive(true);
                    }
                }

                if (luces[0].activeSelf == true)
                {
                    foreach (GameObject go in luces)
                    {
                        go.SetActive(false);
                    }
                }

                hayLuz = false;
                activo4 = true;
                ActivarMarcoSombra();

                petaloSOMBRABASE.DOFade(1f, duration);
                pistilloSOMBRABASE.DOFade(1f, duration);
            }
            Debug.Log("Esta muy oscuro");
        }
        //else
        //{
        //    if(activo4)
        //    {
        //        //petaloSOMBRABASE.DOFade(0f, duration);
        //        //pistilloSOMBRABASE.DOFade(0f, duration);
        //        //petaloMarcoLUZ.DOFade(0f,duration);
        //        //pistilloMarcoLUZ.DOFade(0f,duration);
        //        Debug.Log("No esta tan oscuro");
        //        activo4 = false;
        //    }
        //}
    }

    void SensorDeHumedad()
    {
        if (datoSensorHumedad == 1)
        {
            activo5 = true;
            if (hayLuz && datoBoton == 0 && datoSonido == 0 && datoFuego == 0)   // 1 sensor activado
            {
                ActivarMarcoLuz();
                petalosAguaLUZ[0].DOFade(1f, duration);
                pistilloAguaLUZ[0].DOFade(1f,duration);
            }
            else if(!hayLuz && datoSonido == 0 && datoFuego == 0 && datoBoton == 0)   // Oscuridad
            {
                ActivarMarcoSombra();
                petalosAguaSOMBRA[0].DOFade(1f,duration);
                pistilloAguaSOMBRA[0].DOFade(1f,duration);
            }
            Debug.Log("Esta mojado");
        }
        else
        {
            if (activo5)
            {
                petalosAguaLUZ[0].DOFade(0f, duration);
                pistilloAguaLUZ[0].DOFade(0f, duration);
                petalosAguaSOMBRA[0].DOFade(0f, duration);
                pistilloAguaSOMBRA[0].DOFade(0f, duration);
                Debug.Log("No ta mojao");
                Desactivar();
            }
        }
    }

    void Boton()
    {
        if (datoBoton == 1)
        {
            activo6 = true;
            if(hayLuz && datoSonido == 0 && datoSensorHumedad == 0 && datoFuego == 0)   // 1 sensor activado
            {
                //ActivarMarcoLuz();
                petalosTierraLUZ[0].DOFade(1f,duration);
                pistilloTierraLUZ[0].DOFade(1f,duration);
            }
            else if(!hayLuz && datoSonido == 0 && datoSensorHumedad == 0 && datoFuego == 0) // Oscuridad
            {
                //ActivarMarcoSombra();
                petalosTierraSOMBRA[0].DOFade(1f,duration);
                pistilloTierraSOMBRA[0].DOFade(1f, duration);
            }
            Debug.Log("Hay tacto");
        }

        else
        {
            if (activo6)
            {
                petalosTierraLUZ[0].DOFade(0f, duration);
                pistilloTierraLUZ[0].DOFade(0f, duration);
                petalosTierraSOMBRA[0].DOFade(0f, duration);
                pistilloTierraSOMBRA[0].DOFade(0f, duration);
                Debug.Log("No hay tacto");
                Desactivar();
            }
        }

    }

    void ActivarMarcoLuz()
    {
        petaloMarcoLUZ.DOFade(1f, duration);
        pistilloMarcoLUZ.DOFade(1f, duration);
    }
    void ActivarMarcoSombra()
    {
        petaloMarcoSombra.DOFade(1f, duration);
        pistilloMarcoSombra.DOFade(1f, duration);
    }
    void Desactivar()
    {
        //petaloMarcoLUZ.DOFade(0f, duration);
        //pistilloMarcoLUZ.DOFade(0f, duration);
        //petaloMarcoSombra.DOFade(0f, duration);
        //pistilloMarcoSombra.DOFade(0f, duration);
        activo1 = false;
        activo2 = false;
        activo3 = false;
        activo4 = false;
        activo5 = false;
        activo6 = false;
        activo7 = false;
        activo8 = false;
        activo9 = false;
    }
}
