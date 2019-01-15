using System.IO;
using System.Collections.Generic;
using System;

namespace TrabajoIntegrador
{
    public class ConjuntoDeDatos
    {

        private List<IList<string>> filas;
        private IList<string> encabezados;

        public List<IList<string>> Filas
        {
            get
            {
                return filas;
            }
            set
            {
                {
                    filas = value;
                }
            }
        }
        public IList<string> Encabezados
        {
            get
            {
                return encabezados;
            }
            set
            {
                {
                    encabezados = value;
                }
            }
        }


        public ConjuntoDeDatos(IList<IList<string>> filas, IList<string> encabezados)
        {

            this.filas = (List<IList<string>>)filas;
            this.encabezados = encabezados;
        }

        public ConjuntoDeDatos(string pathCsvFile, char delimitador)
        {
            try { 
            filas = new List<IList<string>>();

            using (var stream = File.OpenRead(pathCsvFile))
            using (var reader = new StreamReader(stream))
            {
                var data = CsvParser.ParseHeadAndTail(reader, delimitador, '"');

                encabezados = data.Item1;

                foreach (var line in data.Item2)
                {
                    filas.Add(line);
                }
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        public string obtenerEtiquetaFila(int index)
        {
            return this.filas[index][filas[index].Count - 1];
        }

        public IList<string> obtenerFila(int index)
        {
            return this.filas[index];
        }


        public int cantidadFilas()
        {
            return this.filas.Count;
        }


    }
}
