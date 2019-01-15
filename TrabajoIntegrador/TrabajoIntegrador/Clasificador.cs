using System;
using System.Collections.Generic;

namespace TrabajoIntegrador
{

    public class Clasificador
    {

        private Dictionary<Pregunta, double> resultados;
        private List<Pregunta> preguntas;
        private double ganancia;
        private Pregunta pregunta;
        private IList<IList<string>> filas;
        private IList<string> encabezados;
        private IList<IList<IList<string>>> datosDeHijos;

        public IList<string> Encabezados { get => encabezados; set => encabezados = value; }
        public IList<IList<string>> Filas { get => filas; set => filas = value; }

        public Clasificador(ConjuntoDeDatos dataset)
        {
            this.filas = dataset.Filas;
            this.encabezados = dataset.Encabezados;
            resultados = Particionador.find_best_split(filas, encabezados);
            preguntas = new List<Pregunta>(resultados.Keys);
            pregunta = preguntas[0];
            ganancia = resultados[pregunta];
            //TODO Si es hijo no avanzo
            if (ganancia > 0)
                datosDeHijos = Particionador.partition(filas, pregunta);
        }

        public bool crearHoja()
        {
            return (ganancia == 0.0);
        }

        public Dictionary<string, int> obtenerDatoHoja()
        {
            return Particionador.class_counts(filas);
        }

        public Pregunta obtenerPregunta()
        {
            return pregunta;
        }

        public ConjuntoDeDatos obtenerDatosIzquierdo()
        {
            return new ConjuntoDeDatos(datosDeHijos[0], this.encabezados);
        }

        public ConjuntoDeDatos obtenerDatosDerecho()
        {
            return new ConjuntoDeDatos(datosDeHijos[1], this.encabezados);
        }

    }
}
