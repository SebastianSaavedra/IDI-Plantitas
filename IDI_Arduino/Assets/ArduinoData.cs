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
    [SerializeField] int interacciones = 0;

    bool hayLuz;
    bool activo1,activo2,activo3,activo4,activo5,activo6,activo7,activo8,activo9;
    bool crecio;
    bool interaccionMultiple = false;
    bool sePrendio;

    [SerializeField] float duration = 1.5f;

    [SerializeField] Animator anim;

    [SerializeField] List<AudioSource> audios = new List<AudioSource>();

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

    void Update()       
    {
        if (datoPhotosensor1 == 0 && datoPhotosensor2 == 0 && sePrendio)
        {
            sePrendio = false;
            crecio = false;
            interacciones = 0;
            anim.Play("Base Layer.FlorEncoje");
            StartCoroutine("ActivarTodo");
            foreach (AudioSource item in audios)
            {
                item.DOFade(0f, 1f);
            }
        }
        InteraccionMultiple();

        SensorDeSonido();
        SensorDeFuego();
        SensorDeLuz1();
        SensorDeLuz2();
        SensorDeHumedad();
        Boton();
    }

    void InteraccionMultiple()     // Mezclas 
    {
        if (interacciones >= 5)
        {
            interaccionMultiple = true;
            audios[4].DOFade(1f, 1f);
            activo7 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                cuatroElementosPetalos[0].DOFade(1f,duration);
                cuatroElementosPistillo[0].DOFade(1f,duration);
            }

            else
            {
                ActivarMarcoSombra();
                cuatroElementosPetalos[1].DOFade(1f, duration);
                cuatroElementosPistillo[1].DOFade(1f, duration);
            }
            Debug.Log("Mezcla cuadruple ON");
        }

        else if (interacciones == 4)
        {
            interaccionMultiple = true;
            audios[3].DOFade(1f, 1f);
            activo8 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                tresElementosPetalos[0].DOFade(1f, duration);
                tresElementosPistillo[0].DOFade(1f, duration);
            }

            else
            {
                ActivarMarcoSombra();
                tresElementosPetalos[1].DOFade(1f, duration);
                tresElementosPistillo[1].DOFade(1f, duration);
            }
            Debug.Log("Mezcla Triple ON");
        }

        #region la pesadilla de cualquier programador, else ifs infinitos

        #region Agua
        else if (datoBoton == 1 && datoSensorHumedad == 1)
        {
            audios[2].DOFade(1f, 1f);
            activo9 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                petalosAguaLUZ[2].DOFade(1f, duration);
                pistilloAguaLUZ[2].DOFade(1f, duration);
            }

            else
            {
                ActivarMarcoSombra();
                petalosAguaSOMBRA[2].DOFade(1f, duration);
                pistilloAguaSOMBRA[2].DOFade(1f, duration);
            }
            Debug.Log("Agua tierra");
        }

        else if (datoFuego == 1 && datoSensorHumedad == 1)
        {
            audios[2].DOFade(1f, 1f);
            activo9 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                petalosAguaLUZ[1].DOFade(1f, duration);
                pistilloAguaLUZ[1].DOFade(1f, duration);
            }

            else
            {
                ActivarMarcoSombra();
                petalosAguaSOMBRA[1].DOFade(1f, duration);
                pistilloAguaSOMBRA[1].DOFade(1f, duration);
            }
            Debug.Log("Agua Fuego");
        }

        else if (datoSonido == 1 && datoSensorHumedad == 1)
        {
            audios[2].DOFade(1f, 1f);
            activo9 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                petalosAguaLUZ[3].DOFade(1f, duration);
                pistilloAguaLUZ[3].DOFade(1f, duration);
            }

            else
            {
                ActivarMarcoSombra();
                petalosAguaSOMBRA[3].DOFade(1f, duration);
                pistilloAguaSOMBRA[3].DOFade(1f, duration);
            }
            Debug.Log("Agua Aire");
        }
        #endregion
        #region Tierra
        else if (datoBoton == 1 && datoFuego == 1)
        {
            audios[2].DOFade(1f, 1f);
            activo9 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                petalosTierraLUZ[2].DOFade(1f, duration);
                pistilloFuegoLUZ[1].DOFade(1f, duration);
            }

            else
            {
                ActivarMarcoSombra();
                petalosTierraSOMBRA[1].DOFade(1f, duration);
                pistilloFuegoSOMBRA[1].DOFade(1f, duration);
            }
            Debug.Log("Tierra Fuego");
        }

        else if (datoBoton == 1 && datoSonido == 1)
        {
            audios[2].DOFade(1f, 1f);
            activo9 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                petalosTierraLUZ[1].DOFade(1f, duration);
                pistilloAireLUZ[2].DOFade(1f, duration);
            }

            else
            {
                ActivarMarcoSombra();
                petalosTierraSOMBRA[3].DOFade(1f, duration);
                pistilloAireSOMBRA[2].DOFade(1f, duration);
            }
            Debug.Log("Tierra Aire");
        }
        #endregion
        #region Fuego
        else if (datoFuego == 1 && datoSonido == 1)
        {
            audios[2].DOFade(1f, 1f);
            activo9 = true;
            if (hayLuz)
            {
                ActivarMarcoLuz();
                petalosFuegoLUZ[3].DOFade(1f, duration);
                pistilloAireLUZ[1].DOFade(1f, duration);
            }

            else
            {
                ActivarMarcoSombra();
                petalosFuegoSOMBRA[3].DOFade(1f, duration);
                pistilloAireSOMBRA[2].DOFade(1f, duration);
            }
            Debug.Log("Fuego Aire");
        }
        #endregion

        #endregion

        else
        {
            if (activo7 || activo8 || activo9)
            {
                //petalosAguaLUZ[2].DOFade(0f, duration);
                //pistilloAguaLUZ[2].DOFade(0f, duration);
                //petalosAguaSOMBRA[2].DOFade(0f, duration);
                //pistilloAguaSOMBRA[2].DOFade(0f, duration);
                tresElementosPetalos[0].DOFade(0f, duration);
                tresElementosPistillo[0].DOFade(0f, duration);
                tresElementosPetalos[1].DOFade(0f, duration);
                tresElementosPistillo[1].DOFade(0f, duration);
                //petalosTierraSOMBRA[1].DOFade(0f, duration);
                //pistilloFuegoSOMBRA[1].DOFade(0f, duration);
                //petalosTierraLUZ[2].DOFade(0f, duration);
                //pistilloFuegoLUZ[1].DOFade(0f, duration);

                //agua
                foreach (SpriteRenderer item in petalosAguaLUZ)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in pistilloAguaLUZ)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in petalosAguaSOMBRA)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in pistilloAguaSOMBRA)
                {
                    item.DOFade(0f, duration);
                }

                //tierra
                foreach (SpriteRenderer item in petalosTierraLUZ)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in pistilloTierraLUZ)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in petalosTierraSOMBRA)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in pistilloTierraSOMBRA)
                {
                    item.DOFade(0f, duration);
                }

                //Fuego
                foreach (SpriteRenderer item in petalosFuegoLUZ)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in pistilloFuegoLUZ)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in petalosFuegoSOMBRA)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in pistilloFuegoSOMBRA)
                {
                    item.DOFade(0f, duration);
                }

                //Aire
                foreach (SpriteRenderer item in petalosAireLUZ)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in pistilloAireLUZ)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in petalosAireSOMBRA)
                {
                    item.DOFade(0f, duration);
                }
                foreach (SpriteRenderer item in pistilloAireSOMBRA)
                {
                    item.DOFade(0f, duration);
                }
                audios[2].DOFade(0f,1f);
                audios[3].DOFade(0f,1f);
                audios[4].DOFade(0f,1f);
                activo7 = false;
                activo8 = false;
                activo9 = false;
                interaccionMultiple = false;
                sePrendio = false;
                interacciones--;
                Debug.Log("Interacciones Multiples desactivadas");
            }
        }
    }

    void SensorDeSonido()
    {
        if (datoSonido == 1) // si microfono activado
        {
            if (!interaccionMultiple)
            {
                audios[1].DOFade(1f, 1f);
                if (!activo1)
                {
                    interacciones++;
                    activo1 = true;
                }

                if (hayLuz && datoFuego == 0 && datoBoton == 0 && datoSensorHumedad == 0)   // 1 sensor activado
                {
                    ActivarMarcoLuz();
                    petalosAireLUZ[0].DOFade(1f, duration);
                    pistilloAireLUZ[0].DOFade(1f, duration);
                }
                else if (!hayLuz && datoFuego == 0 && datoBoton == 0 && datoSensorHumedad == 0)   // Oscuridad
                {
                    ActivarMarcoSombra();
                    petalosAireSOMBRA[0].DOFade(1f, duration);
                    pistilloAireSOMBRA[0].DOFade(1f, duration);
                }
                Debug.Log("Microfono prendido");
            }
        }

        else // si microfono apagado
        {
            if(activo1)
            {
                petalosAireLUZ[0].DOFade(0f, duration);
                pistilloAireLUZ[0].DOFade(0f, duration);
                petalosAireSOMBRA[0].DOFade(0f, duration);
                pistilloAireSOMBRA[0].DOFade(0f, duration);
                audios[1].DOFade(0f, 1f);
                activo1 = false;
                interacciones--;
                Debug.Log("Microfono apagado");
            }
        }
    }
    void SensorDeFuego()
    {
        if (datoFuego == 1)
        {
            if (!interaccionMultiple)
            {
                audios[1].DOFade(1f, 1f);
                if (!activo2)
                {
                    interacciones++;
                    activo2 = true;
                }

                if (hayLuz && datoSonido == 0 && datoBoton == 0 && datoSensorHumedad == 0)   // 1 sensor activado
                {
                    ActivarMarcoLuz();
                    petalosFuegoLUZ[0].DOFade(1f, duration);
                    pistilloFuegoLUZ[0].DOFade(1f, duration);
                }
                else if (!hayLuz && datoSonido == 0 && datoBoton == 0 && datoSensorHumedad == 0)  // Oscuridad
                {
                    ActivarMarcoSombra();
                    petalosFuegoSOMBRA[0].DOFade(1f, duration);
                    pistilloFuegoSOMBRA[0].DOFade(1f, duration);
                }
                Debug.Log("Fuego Cerca");
            }
        }

        else
        {
            if (activo2)
            {
                petalosFuegoLUZ[0].DOFade(0f, duration);
                pistilloFuegoLUZ[0].DOFade(0f, duration);
                petalosFuegoSOMBRA[0].DOFade(0f, duration);
                pistilloFuegoSOMBRA[0].DOFade(0f, duration);
                audios[1].DOFade(0f, 1f);
                activo2 = false;
                interacciones--;
                Debug.Log("No hay Fuego");
            }
        }
    }

    void SensorDeLuz1()
    {
        if (datoPhotosensor1 == 1)
        {
            if (!interaccionMultiple)
            {
                audios[0].DOFade(1f, 1f);
                if (!sePrendio)
                {
                    interacciones++;
                    sePrendio = true;
                }
                if (datoSonido == 0 && datoSensorHumedad == 0 && datoFuego == 0 && datoBoton == 0)   // 1 sensor activado
                {
                    if (!hayLuz && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_1E"))
                    {
                        StartCoroutine("CambioLuz");
                    }

                    else if (!crecio)
                    {

                        if (sombras[0].activeSelf == true)  // Desactiva los gameobjects de las sombras
                        {
                            foreach (GameObject go in sombras)
                            {
                                go.SetActive(false);
                            }
                        }

                        anim.Play("Base Layer.FlorCrece");
                        hayLuz = true;
                        crecio = true;
                    }
                    ActivarMarcoLuz();
                    Debug.Log("Recibe luz");
                }
            }
        }
    }
    void SensorDeLuz2()
    {
        if (datoPhotosensor2 == 1)
        {
            if (!interaccionMultiple)
            {
                audios[0].DOFade(1f, 1f);
                if (!sePrendio)
                {
                    interacciones++;
                    sePrendio = true;
                }

                if (datoSonido == 0 && datoSensorHumedad == 0 && datoFuego == 0 && datoBoton == 0)   // 1 sensor activado
                {
                    if (hayLuz && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_1E"))
                    {
                        StartCoroutine("CambioSombra");
                    }

                    else if (!crecio)
                    {

                        if (luces[0].activeSelf == true)
                        {
                            foreach (GameObject go in luces)
                            {
                                go.SetActive(false);
                            }
                        }

                        anim.Play("Base Layer.FlorCrece");
                        crecio = true;
                        hayLuz = false;
                        Debug.Log("Esta muy oscuro");
                    }
                    ActivarMarcoSombra();
                }
            }
        }
    }

    void SensorDeHumedad()
    {
        if (datoSensorHumedad == 1)
        {
            if (!interaccionMultiple)
            {
                audios[1].DOFade(1f, 1f);
                if (!activo5)
                {
                    interacciones++;
                    activo5 = true;
                }

                if (hayLuz && datoBoton == 0 && datoSonido == 0 && datoFuego == 0)   // 1 sensor activado
                {
                    ActivarMarcoLuz();
                    petalosAguaLUZ[0].DOFade(1f, duration);
                    pistilloAguaLUZ[0].DOFade(1f, duration);
                }
                else if (!hayLuz && datoSonido == 0 && datoFuego == 0 && datoBoton == 0)   // Oscuridad
                {
                    ActivarMarcoSombra();
                    petalosAguaSOMBRA[0].DOFade(1f, duration);
                    pistilloAguaSOMBRA[0].DOFade(1f, duration);
                }
                Debug.Log("Esta mojado");
            }
        }
        else
        {
            if (activo5)
            {
                petalosAguaLUZ[0].DOFade(0f, duration);
                pistilloAguaLUZ[0].DOFade(0f, duration);
                petalosAguaSOMBRA[0].DOFade(0f, duration);
                pistilloAguaSOMBRA[0].DOFade(0f, duration);
                audios[1].DOFade(0f, 1f);
                activo5 = false;
                interacciones--;
                Debug.Log("No ta mojao");
            }
        }
    }

    void Boton()
    {
        if (datoBoton == 1)
        {
            if (!interaccionMultiple)
            {
                audios[1].DOFade(1f, 1f);
                if (!activo6)
                {
                    interacciones++;
                    activo6 = true;
                }

                if (hayLuz && datoSonido == 0 && datoSensorHumedad == 0 && datoFuego == 0)   // 1 sensor activado
                {
                    //ActivarMarcoLuz();
                    petalosTierraLUZ[0].DOFade(1f, duration);
                    pistilloTierraLUZ[0].DOFade(1f, duration);
                }
                else if (!hayLuz && datoSonido == 0 && datoSensorHumedad == 0 && datoFuego == 0) // Oscuridad
                {
                    //ActivarMarcoSombra();
                    petalosTierraSOMBRA[0].DOFade(1f, duration);
                    pistilloTierraSOMBRA[0].DOFade(1f, duration);
                }
                Debug.Log("Hay tacto");
            }
        }

        else
        {
            if (activo6)
            {
                petalosTierraLUZ[0].DOFade(0f, duration);
                pistilloTierraLUZ[0].DOFade(0f, duration);
                petalosTierraSOMBRA[0].DOFade(0f, duration);
                pistilloTierraSOMBRA[0].DOFade(0f, duration);
                audios[1].DOFade(0f, 1f);
                activo6 = false;
                interacciones--;
                Debug.Log("No hay tacto");
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

    IEnumerator CambioLuz()
    {
        anim.Play("Base Layer.FlorEncoje");
        yield return new WaitForSeconds(duration);

        if (luces[0].activeSelf == false) // activa todos los GameObjects de luz de vuelta.
        {
            foreach (GameObject go in luces)
            {
                go.SetActive(true);
            }
        }

        if (sombras[0].activeSelf == true)  // Desactiva los gameobjects de las sombras
        {
            foreach (GameObject go in sombras)
            {
                go.SetActive(false);
            }
        }
        anim.Play("Base Layer.FlorCrece");
        yield return new WaitForSeconds(duration);

        hayLuz = true;
        Debug.Log(hayLuz);
        yield break;
    }

    IEnumerator CambioSombra()
    {
        anim.Play("Base Layer.FlorEncoje");
        yield return new WaitForSeconds(duration);

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
        anim.Play("Base Layer.FlorCrece");
        yield return new WaitForSeconds(duration);

        hayLuz = false;
        Debug.Log(hayLuz);
        yield break;
    }

    IEnumerator ActivarTodo()
    {
        yield return new WaitForSeconds(duration);
        if (sombras[0].activeSelf == false) // activa todos los GameObjects de luz de vuelta.
        {
            foreach (GameObject go in sombras)
            {
                go.SetActive(true);
            }
        }
        if (luces[0].activeSelf == false) // activa todos los GameObjects de luz de vuelta.
        {
            foreach (GameObject go in luces)
            {
                go.SetActive(true);
            }
        }
        yield return new WaitForEndOfFrame();

        yield break;
    }
}
