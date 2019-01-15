using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrabajoIntegrador
{
    class Program
    {
        #region constantes
        const string opciones = "1) Administración.\n" +
                                "2) Consultas.\n" +
                                "3) ¡Jugar!.\n" +
                                "4) Salir.";

        const string menuAdmin = "1) Construir arbol a partir de CSV.\n" +
                                  "2) Probar Arbol.\n" +
                                  "3) Salir.";

        const string menuConsulta = "1) Imprimir toda las predicciones.\n" +
                                      "2) Seleccione caracteristicas.\n" +
                                      "3) Seleccione profundidad.\n" +
                                      "4) Salir.";

        const string respuestas = "1) Si.\n" +
                                  "2) No.\n" +
                                  "Seleccione: ";

        const string titulo = "Machine Learning";
        const string nombreAlumno = "Alumno: Ruiz, Maximiliano";
        #endregion constantes

        static void Main(string[] args)
        {
            Titulo();
            ArbolDecision<object> arbol = null;
            Clasificador clasificador = null;
            ConjuntoDeDatos conjuntoDeDatos = null;
            int opcion = 0;

            do
            {
                Console.WriteLine(opciones);
                Console.Write("Seleccione opción: ");
                opcion = SafeParseOpcion(Console.ReadLine());
                while (opcion > 4)
                {
                    Console.WriteLine("\nOpción no valida.");
                    PressKey();
                    Console.WriteLine(opciones);
                    Console.Write("Seleccione opción: ");
                    opcion = SafeParseOpcion(Console.ReadLine());
                }
                switch (opcion)
                {
                    //MÓDULO: ADMINISTRACIÓN
                    case 1:
                        int opcionMenu;
                        do
                        {
                            Console.Clear();
                            Titulo();
                            Console.WriteLine(menuAdmin);
                            Console.Write("Seleccione opción: ");
                            opcionMenu = SafeParseOpcion(Console.ReadLine());
                            while (opcionMenu > 3)
                            {
                                Console.WriteLine("\nOpción no valida.");
                                PressKey();
                                Console.WriteLine(menuAdmin);
                                Console.Write("Seleccione opción: ");
                                opcionMenu = SafeParseOpcion(Console.ReadLine());

                            }
                            switch (opcionMenu)
                            {
                                case 1:
                                    conjuntoDeDatos = ArmarConjuntoDeDatos(args);
                                    clasificador = ArmarClasificador(conjuntoDeDatos);

                               
                                    if (clasificador != null)
                                    {
                                        arbol = ArmarArbol(clasificador);
                                    }
                                    if (arbol != null)
                                    {
                                        Console.WriteLine("\n¡Él Árbol se ha generado correctamente!");
                                        PressKey();
                                    }
                                    break;
                                case 2:
                                    Probar(arbol);
                                    PressKey();
                                    break;
                                case 3:
                                    Console.Clear();
                                    Titulo();
                                    break;
                            }
                        }
                        while (opcionMenu != 3);
                        break;

                    //MÓDULO: CONSULTA
                    case 2:
                        int opcionMenuConsulta;
                        do
                        {
                            Console.Clear();
                            Titulo();
                            Console.WriteLine(menuConsulta);
                            Console.Write("Seleccione opción: ");
                            opcionMenuConsulta = SafeParseOpcion(Console.ReadLine());
                            while (opcionMenuConsulta > 4)
                            {
                                Console.WriteLine("\nOpción no valida.");
                                PressKey();
                                Console.WriteLine(menuConsulta);
                                Console.Write("Seleccione opción: ");
                                opcionMenuConsulta = SafeParseOpcion(Console.ReadLine());

                            }
                            switch (opcionMenuConsulta)
                            {
                                case 1:
                                    ImprimirArbol(arbol);
                                    PressKey();
                                    break;
                                case 2:
                                    SelectCaracteristicas(conjuntoDeDatos, clasificador, arbol);
                                    PressKey();
                                    break;
                                case 3:
                                    ImprimirProfundidadArbol(arbol);
                                    PressKey();
                                    break;
                                case 4:
                                    Console.Clear();
                                    Titulo();
                                    break;
                            }

                        } while (opcionMenuConsulta != 4);
                        break;
                    case 3:
                        Jugar(arbol);
                        PressKey();
                        break;
                }
            } while (opcion < 4);

            Timer(3000);
        }

        #region Administracion
        public static ConjuntoDeDatos ArmarConjuntoDeDatos(string[] args)
        {
            try
            {
                string ruta = args[0];
                char delimitador = ',';
                ConjuntoDeDatos datos = new ConjuntoDeDatos(ruta, delimitador);

                return datos;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                PressKey();
                return null;
            }
        }

        public static Clasificador ArmarClasificador(ConjuntoDeDatos datos)
        {
            if (datos == null || datos.Filas.Count == 0)
            {
                Console.WriteLine("El archivo CSV seleccionado está corrupto o vacío.");
                PressKey();
                return null;
            }
            else
            {
                Clasificador clasificador = new Clasificador(datos);

                return clasificador;
            }
        }

        public static ArbolDecision<object> ArmarArbol(Clasificador clasificador)
        {
            if (clasificador != null)
            {
                ArbolDecision<object> arbolBinario = new ArbolDecision<object>();
                arbolBinario.MapArbol(clasificador);

                return arbolBinario;
            }
            else
                return null;
        }

        public static void Probar(ArbolDecision<object> arbol)
        {
            if (arbol == null)
            {
                Console.WriteLine("Aún no se ha construido un árbol o el CSV seleccionado está vacio.");
            }
            else
            {
                Dictionary<string, int> lista = new Dictionary<string, int>();
                arbol.ProbabilidadPrediccion();
            }

        }
        #endregion Administracion

        #region metodos/funciones 
        public static void Titulo()
        {
            Console.Write(new String('*', Console.WindowWidth));
            Console.Write("**");
            Console.SetCursorPosition((Console.WindowWidth - titulo.Length) / 2, Console.CursorTop);
            Console.Write(titulo);
            Console.SetCursorPosition((Console.WindowWidth - 2), Console.CursorTop);
            Console.Write("**");
            Console.Write("**");
            Console.SetCursorPosition((Console.WindowWidth - nombreAlumno.Length) / 2, Console.CursorTop);
            Console.Write(nombreAlumno);
            Console.SetCursorPosition((Console.WindowWidth - 2), Console.CursorTop);
            Console.Write("**");
            Console.WriteLine(new String('*', Console.WindowWidth));
        }

        public static void PressKey()
        {
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadLine();
            Console.Clear();
            Titulo();
        }

        public static int SafeParseOpcion(string dato)
        {
            if (Int32.TryParse(dato, out int opcion))
                return opcion;
            else
            {
                Console.WriteLine("\nEl valor '{0}' es incorrecto. Solo debe ingresar valores númericos.", dato);
                PressKey();
                return 0;
            }
        }

        public static int Timer(double tiempo)
        {
            if (tiempo == 0)
            {
                return 0;
            }
            else
            {
                Console.Clear();
                Titulo();
                Console.WriteLine(opciones);
                Console.WriteLine("Saliendo en {0}...", (tiempo * 0.001));
                Thread.Sleep(1000);

                return Timer(tiempo - 1000);
            }
        }

        public static int ValidateOpcion(string respu, string pregunta)
        {
            int respuesta = SafeParseOpcion(respu);
            while (respuesta != 1 && respuesta != 2)
            {

                Console.WriteLine("Opción incorrecta.");
                Console.WriteLine(pregunta);
                Console.Write(respuestas);
                respuesta = SafeParseOpcion(Console.ReadLine());
            }
            return respuesta;
        }

        public static void Jugar(ArbolDecision<object> arbol)
        {
            if (arbol == null)
            {
                Console.WriteLine("Aún no se ha construido un árbol o el CSV seleccionado está vacio.");
            }
            else
            {
                //Consigna va a ser el árbol de referencia que se va a utilizar al jugar, una vez que se termine y quiera volver a jugar se vuelve a generar dicho árbol.
                ArbolDecision<object> consigna = arbol;

                Console.Write("¿Ya estas listo para jugar?\n");
                Console.Write(respuestas);
                //Valido y convierto lo que ingrese por consola a un número. Sino vuelvo a preguntar
                int respuesta = ValidateOpcion(Console.ReadLine(), "¿Ya estas listo para jugar?\n");

                switch (respuesta)
                {
                    case 1:
                        Console.Clear();
                        Titulo();
                        Console.WriteLine("¡¡Juguemos!!");
                        while (respuesta == 1)
                        {

                            //Si no es hoja imprimo la siguiente pregunta.
                            if (!consigna.EsHoja())
                            {
                                Console.WriteLine(consigna.Dato);
                                Console.Write(respuestas);

                                int respConsigna = ValidateOpcion(Console.ReadLine(), consigna.Dato.ToString());

                                switch (respConsigna)
                                {
                                    case 1:
                                        consigna = JugarRespuesta("si", consigna);
                                        break;
                                    case 2:
                                        consigna = JugarRespuesta("no", consigna);
                                        break;
                                }
                            }
                            else
                            {
                                //Si es una hoja, hago una conversión explicita del tipo object a dictionary para poder recorrerlo
                                Dictionary<string, int> etiquetas = (Dictionary<string, int>)consigna.Dato;

                                Console.WriteLine("Estoy pensando en/los:");
                                foreach (var hoja in etiquetas)
                                {
                                    Console.WriteLine(hoja.Key);
                                }

                                Console.WriteLine("¿Deseas seguir jugando?");
                                Console.Write(respuestas);
                                respuesta = ValidateOpcion(Console.ReadLine(), "¿Deseas seguir jugando?");
                                //Si quiere volver a jugar vuelvo a instanciar el arbol de decisión original.
                                if (respuesta == 1)
                                {
                                    Jugar(arbol);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("\n:(\n" +
                                         "¡Hasta la próxima!");
                                    break;
                                }
                            }
                        }
                        break;

                    case 2:
                        Console.WriteLine("\n:(\n" +
                                          "¡Hasta la próxima!");
                        break;
                }
            }
        }

        public static ArbolDecision<object> JugarRespuesta(string resp, ArbolDecision<object> datos)
        {
            switch (resp)
            {
                case "si":
                    if (!datos.EsHoja())
                        return datos.HijoDerecho;
                    else
                        return datos;

                case "no":
                    if (!datos.EsHoja())
                        return datos.HijoIzquierdo;
                    else
                        return datos;

                default:
                    return null;
            }
        }

        public static void ImprimirArbol(ArbolDecision<object> arbol)
        {
            if (arbol == null)
                Console.WriteLine("Aún no se ha construido un árbol o el CSV seleccionado está vacio.");
            else
            {
                Console.WriteLine("\nEl árbol puede predecir los siguientes elementos:");

                List<string> listado = new List<string>();
                listado = arbol.PrintHojas(listado);
                //ordeno el árbol para imprimirlo alfabeticamente
                listado.Sort();
                foreach (var item in listado)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();

            }
        }

        public static void ImprimirProfundidadArbol(ArbolDecision<object> arbol)
        {
            if (arbol != null)
            {
                int nivelesArbol = 0;
                nivelesArbol = arbol.ContarNiveles(ref nivelesArbol);

                Console.WriteLine("Seleccione profunidad del árbol:");
                int profunArbol = SafeParseOpcion(Console.ReadLine());

                if (profunArbol > nivelesArbol)
                {
                    Console.WriteLine("La profunidad seleccionada es mayor a la del árbol que es: {0}", nivelesArbol);
                }
                else
                {
                    arbol.PrintNiveles(profunArbol);
                    Console.WriteLine();
                }
            }
            else
                Console.WriteLine("Aún no se ha construido un árbol o el CSV seleccionado está vacio.");
            //todo: tratar de sacar los titulos duplicados para las hojas

        }

        public static void SelectCaracteristicas(ConjuntoDeDatos conjuntoDeDatos, Clasificador clasificador, ArbolDecision<object> arbol)
        {
            if (arbol == null)
            {
                Console.WriteLine("Aún no se ha construido un árbol o el CSV seleccionado está vacio.");
            }
            else
            {
                //todo corregir nombres de variables
                List<Pregunta> caracteristicas = new List<Pregunta>();

                Console.WriteLine("\nSeleccione caracteristicas:");
                for (int i = 0; i < clasificador.Encabezados.Count - 1; i++)
                {
                    Console.Write("{0}: ", clasificador.Encabezados[i]);
                    string resp = Console.ReadLine();
                    caracteristicas.Add(new Pregunta(i, resp, clasificador.Encabezados[i]));
                }

                List<string> recorridoPreguntasRespuestas = new List<string>();
                bool respuestaCarac = false;
                bool finArbol = false;

                while (!finArbol)
                {
                    if (!arbol.EsHoja())
                    {
                        Pregunta pregunta = (Pregunta)arbol.Dato;
                        recorridoPreguntasRespuestas.Add(pregunta.ToString());


                        for (int j = 0; j < caracteristicas.Count; j++)
                        {
                            if (pregunta.ToString().Contains(" Mayor o igual a ") && caracteristicas[j].ToString().Contains(" Mayor o igual a ") && pregunta.Columna == caracteristicas[j].Columna)
                            {
                                if (Convert.ToDouble(caracteristicas[j].Valor) >= Convert.ToDouble(pregunta.Valor) && pregunta.Columna == caracteristicas[j].Columna)
                                {
                                    respuestaCarac = true;
                                    recorridoPreguntasRespuestas.Add("Si");
                                    break;
                                }
                                else
                                {
                                    respuestaCarac = false;
                                    recorridoPreguntasRespuestas.Add("No");
                                    break;
                                }
                            }
                            else
                            {
                                if (pregunta.Valor == caracteristicas[j].Valor && pregunta.Columna == caracteristicas[j].Columna)
                                {
                                    respuestaCarac = true;
                                    recorridoPreguntasRespuestas.Add("Si");
                                    break;
                                }
                                else if (pregunta.Valor != caracteristicas[j].Valor && pregunta.Columna == caracteristicas[j].Columna)
                                {
                                    respuestaCarac = false;
                                    recorridoPreguntasRespuestas.Add("No");
                                    break;
                                }
                            }
                        }

                        switch (respuestaCarac)
                        {
                            case true:
                                arbol = JugarRespuesta("si", arbol);
                                break;
                            case false:
                                arbol = JugarRespuesta("no", arbol);
                                break;
                        }
                    }
                    else
                    {
                        finArbol = true;
                        Dictionary<string, int> etiquetas = (Dictionary<string, int>)arbol.Dato;

                        Console.WriteLine("\nPreguntas y respuestas hechas en base a las caracteristicas brindadas:");
                        foreach (string item in recorridoPreguntasRespuestas)
                        {
                            Console.WriteLine(item);
                        }
                        Console.WriteLine("\nDatos Encontrados:");
                        foreach (var hoja in etiquetas)
                        {
                            Console.WriteLine(hoja.Key);
                        }
                    }
                }

            }
        }
        #endregion metodos/funciones  

    }
}
