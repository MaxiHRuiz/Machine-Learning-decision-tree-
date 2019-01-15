using System;
using System.Collections.Generic;

namespace TrabajoIntegrador
{
    public class Pregunta
	{
		private int columna;
		private string valor;
		private string encabezado;
		
		public Pregunta(int columna, string valor, string encabezado)
		{
			this.columna=columna;
			this.valor=valor;
			this.encabezado = encabezado;
		}

        public int Columna { get => columna; set => columna = value; }
        public string Valor { get => valor; set => valor = value; }
        public string Encabezado { get => encabezado; set => encabezado = value; }

        public bool coincide(IList<string> ejemplo){
			// Compare the feature value in a parameter to the
			// feature value in this question.
			string val = ejemplo[this.columna];
			double vals;
            //TODO && this.valor != string.Empty
            if (Double.TryParse(val, out vals) )
				return Double.Parse(val) >= Double.Parse(this.valor);
			else
				return val.Equals(this.valor);
		}

		public bool coincide(ConjuntoDeDatos dataset, int fila){
			// Compare the feature value in a parameter to the
			// feature value in this question.
			string val = dataset.obtenerFila(fila)[this.columna];
			double vals;
			if (Double.TryParse(val, out vals))
				return Double.Parse(val) >= Double.Parse(this.valor);
			else
				return val.Equals(this.valor);
		}

     

        public override string ToString(){
			double vals;
			string condition = " Corresponde ";
			if (Double.TryParse(this.valor, out vals))
				condition = " Mayor o igual a ";
			
			return ("El " + this.encabezado + condition + this.valor) + "?";

		}

   
    }
}
