using System;
using System.Collections;
using System.Collections.Generic;

namespace TrabajoIntegrador

{
    /// <summary>
    /// Description of NodoBinario.
    /// </summary>
    /// 
    public class ArbolDecision<T>
    {
        public Object Dato { get; set; }
        public ArbolDecision<T> HijoIzquierdo { get; set; }
        public ArbolDecision<T> HijoDerecho { get; set; }

        public ArbolDecision<T> MapArbol(Clasificador clasificador)
        {
            bool datosEnd = false;

            while (!datosEnd)
            {
                if (Dato == null)
                {
                    Dato = clasificador.obtenerPregunta();
                }

                //invierto los datos del clasificador para que en el árbol las respuestas positivas vayan a la derecha.
                Clasificador hijoDer = new Clasificador(clasificador.obtenerDatosIzquierdo());
                //Si es hoja, almaceno las etiquetas
                if (hijoDer.crearHoja() && HijoDerecho == null)
                {
                    HijoDerecho = new ArbolDecision<T>();
                    Dictionary<string, int> itemHoja = hijoDer.obtenerDatoHoja();
                    HijoDerecho.Dato = itemHoja;
                    datosEnd = true;
                }
                //Si no es una hoja almaceno pregunta y vuelvo a llamar al método y pregunto de vuelta.
                else
                {
                    if (HijoDerecho == null)
                    {
                        HijoDerecho = new ArbolDecision<T>();
                        HijoDerecho.Dato = hijoDer.obtenerPregunta();
                        HijoDerecho.MapArbol(hijoDer);
                    }
                    else
                        datosEnd = true;


                }

                Clasificador hijoIzq = new Clasificador(clasificador.obtenerDatosDerecho());
                if (hijoIzq.crearHoja() && HijoIzquierdo == null)
                {
                    HijoIzquierdo = new ArbolDecision<T>();
                    Dictionary<string, int> itemHoja = hijoIzq.obtenerDatoHoja();
                    HijoIzquierdo.Dato = itemHoja;
                    datosEnd = true;
                }
                else
                {
                    //Si no es una hoja vuelvo a llamar al método y pregunto de vuelta.
                    if (HijoIzquierdo == null)
                    {
                        HijoIzquierdo = new ArbolDecision<T>();
                        HijoIzquierdo.Dato = hijoIzq.obtenerPregunta();
                        HijoIzquierdo.MapArbol(hijoIzq);
                    }
                    else
                        datosEnd = true;
                }
            }

            return this;
        }

        public bool EsHoja()
        {
            return Dato != null && HijoIzquierdo == null && HijoDerecho == null;
        }

        /// <summary>
        /// Recorro en preorden el arbol
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public List<string> PrintHojas(List<string> lista)
        {
            List<string> listadoHojas = lista;

            if (EsHoja())
            {

                Dictionary<string, int> etiquetas = (Dictionary<string, int>)Dato;
                foreach (var hoja in etiquetas)
                {
                    listadoHojas.Add(hoja.Key);
                }
            }
            else
            {
                HijoIzquierdo.PrintHojas(listadoHojas);
                HijoDerecho.PrintHojas(listadoHojas);
            }
            return listadoHojas;
        }


        public Dictionary<string, int> ProbabilidadPrediccion()
        {
            Dictionary<string, int> listadoHojas = new Dictionary<string, int>();
        
            if (EsHoja())
            {
                Dictionary<string, int> etiquetas = (Dictionary<string, int>)Dato;
                foreach (var hoja in etiquetas)
                {
                    Console.WriteLine("Probabilidad de predicción: {0} - seguridad: {1}%", hoja.Key, (( hoja.Value * 100) / etiquetas.Count ));
                }
            }
            else
            {   
                HijoIzquierdo.ProbabilidadPrediccion();
                HijoDerecho.ProbabilidadPrediccion();
            }
            return listadoHojas;
        }

        /// <summary>
        /// Imprimo tanto hojas como nodos dependiendo el nodo que se pase por parametro.
        /// </summary>
        /// <param name="n"></param>
        public void PrintNiveles(int n)
        {
            Cola<ArbolDecision<T>> nodo = new Cola<ArbolDecision<T>>();

            nodo.encolar(this);
            nodo.encolar(null);
            int NivelActual = 0;

            while (!nodo.esVacia() && NivelActual <= n)
            {

                if (nodo.tope() != null)
                {
                    if (NivelActual >= n)
                    {
                        if (nodo.tope().EsHoja())
                        {
                            Console.WriteLine("Predicciones de profundida {0} encontradas:", NivelActual);
                            Dictionary<string, int> etiquetas = (Dictionary<string, int>)nodo.tope().Dato;
                            foreach (var hoja in etiquetas)
                            {
                                //listadoHojas.Add(hoja.Key);
                                Console.WriteLine(hoja.Key + ", ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Pregunta de profundida {0} encontradas:", NivelActual);
                            Console.WriteLine(nodo.tope().Dato);
                        }

                    }

                    if (!nodo.tope().EsHoja())
                    {
                        nodo.encolar(nodo.tope().HijoIzquierdo);
                        nodo.encolar(nodo.tope().HijoDerecho);
                        nodo.encolar(null);

                    }

                    nodo.desencolar();
                }
                else
                {
                    nodo.desencolar();
                    NivelActual++;
                }
            }
        }

        /// <summary>
        /// Devuelvo la cantidad de niveles que posee el árbol para referenciar en el módulo 2-3.
        /// </summary>
        /// <param name="niv"></param>
        /// <returns></returns>
        public int ContarNiveles(ref int niv)
        {
            Cola<ArbolDecision<T>> nodo = new Cola<ArbolDecision<T>>();

            nodo.encolar(this);
            nodo.encolar(null);
            while (!nodo.esVacia())
            {
                if (nodo.tope() != null)
                {
                    if (!nodo.tope().EsHoja())
                    {
                        nodo.encolar(nodo.tope().HijoIzquierdo);
                        nodo.encolar(nodo.tope().HijoDerecho);
                        nodo.encolar(null);

                    }
                    nodo.desencolar();
                }
                else
                {
                    nodo.desencolar();
                    //Mientras haya elementos incremento la variable para obtener la profundidad
                    if (!nodo.esVacia())
                        niv++;
                }
            }
            return niv;
        }

    }
}
