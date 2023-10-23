using System;
using System.Globalization;
using System.Text.Json;
using System.Threading;
using System.Xml.Serialization;

namespace GestionTareasBS
{
    public class ListaTareas
    {
        static List<Tarea> tareas;
        int opcion;
        static void AgregarTarea(Tarea tarea)
        {
            tareas.Add(tarea);
        }
        
        public void EliminarTarea(string descripcion)
        {
            Tarea tarea = tareas.Find(t => t.Descripcion == descripcion);
            if (tarea != null)
            {
                tareas.Remove(tarea);
            }
        }

        static List<Tarea> ConsultarTareas()
        {
            return tareas;
        }



        public void Menu()
        {
            do
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("|   Aplicación de Gestión de Tareas   |");
                Console.WriteLine("=======================================");
                Console.WriteLine("Selecciona una opción:");
                Console.WriteLine("1. Listar todas las tareas");
                Console.WriteLine("2. Listar tareas incompletas");
                Console.WriteLine("3. Listar tareas por fecha de vencimiento");
                Console.WriteLine("4. Agregar nueva tarea");
                Console.WriteLine("5. Marcar tarea como completada");
                Console.WriteLine("6. Guardar tareas en archivo");
                Console.WriteLine("7. Cargar tareas desde archivo");
                Console.WriteLine("8. Salir");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        ListarTodasLasTareas();
                        break;
                    case "2":
                        ListarTareasIncompletas();
                        break;
                    case "3":
                        ListarTareasPorFechaDeVencimiento();
                        break;
                    case "4":
                        AgregarNuevaTarea();
                        break;
                    case "5":
                        MarcarTareaComoCompletada();
                        break;
                    case "6":
                        GuardarTareasEnArchivoAsync();
                        break;
                    case "7":
                        CargarTareasDesdeArchivoAsync();
                        break;
                    case "8":
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, elige una opción válida.");
                        break;
                }
            } while (opcion != 8);
        }
        static void ListarTodasLasTareas()
        {
            Console.Clear();
            Console.WriteLine("Listado de todas las tareas:");
            List<Tarea> tareas = ConsultarTareas();
            MostrarTareas(tareas);
            Console.WriteLine("Presiona Enter para volver al menú principal.");
            Console.ReadLine();
        }

        static void ListarTareasIncompletas()
        {
            var tareasIncompletas =
                from tarea in tareas
                where tarea.Completado == false
                select tarea;
            foreach (var tarea in tareasIncompletas)
            {
                Console.WriteLine("Descripcion" + tarea.Descripcion + " fecha vencimiento" + tarea.Vencimiento);
            }
        }

        static void ListarTareasPorFechaDeVencimiento()
        {
            Console.WriteLine("Listar tareas por vencimiento");
            var tareasPorVencimiento =
                from tarea in tareas
                orderby tarea.Vencimiento descending
                select tarea;
            foreach (var tarea in tareasPorVencimiento)
            {
                Console.WriteLine("Descripcion" + tarea.Descripcion + " fecha vencimiento" + tarea.Vencimiento + " estado completado" + tarea.Completado);
            }
        }

        static void AgregarNuevaTarea()
        {
            Console.Clear();
            Console.WriteLine("Agrega una nueva tarea:");
            Console.Write("Descripción: ");
            string Descripcion = Console.ReadLine();
            Console.Write("Fecha de Vencimiento (yyyy/MM/dd): ");
            string fechaVencimiento = Console.ReadLine();
            string formatoFecha = "yyyy/mm/dd";
            DateTime Vencimiento = DateTime.ParseExact(fechaVencimiento, formatoFecha, CultureInfo.InvariantCulture);
            if (Vencimiento.ToString().Length > 0)
            {
                Boolean Completado = false;
                Tarea nuevaTarea = new Tarea(Descripcion, Vencimiento, Completado);
                tareas.Add(nuevaTarea);
                Console.WriteLine("Tarea agregada exitosamente!.");
            }
            else
            {
                Console.WriteLine("Fecha de vencimiento no válida. La tarea no se ha agregado.");
            }
            Console.WriteLine("Presiona Enter para volver al menú principal.");
        }

        static void MarcarTareaComoCompletada()
        {
            Console.WriteLine("Marcar tarea como completada");
            Console.WriteLine("Escribe la descripcion de la tarea");
            String Descripcion = Console.ReadLine();
            foreach ( var tarea in tareas)
            {
                if (tarea.Descripcion.Contains(Descripcion))
                {
                    Console.WriteLine("Descripcion" + tarea.Descripcion + " fecha vencimiento" + tarea.Vencimiento + " estado completado" + tarea.Completado);
                    Console.WriteLine("Estado de completado cambiado");
                    tarea.Completado = true;
                }
            }
            
        }

        static void MostrarTareas(List<Tarea> tareas)
        {
            foreach (Tarea tarea in tareas)
            {
                Console.WriteLine(tarea);
            }
        }

        static void AgregarTareas()
        {
            Console.WriteLine("Nueva tarea");
            Console.WriteLine("Descripcion");
            String Descripcion = Console.ReadLine();
            Console.WriteLine("Fecha vencimiento. Formato yyyy/mm/dd");
            String Fecha = Console.ReadLine();
            String Formato = "yyyy/mm/dd";
            DateTime FechaVencimiento = DateTime.ParseExact(Fecha, Formato, CultureInfo.InvariantCulture);
            Boolean EstadoCompletado = false;
            Tarea nuevaTarea = new Tarea(Descripcion, FechaVencimiento, EstadoCompletado);
            tareas.Add(nuevaTarea);
        }

        static async void GuardarTareasEnArchivoAsync()
        {
            Console.WriteLine("Introduce como quieres guardarlo, como json o como xml");
            String modo = Console.ReadLine();
            modo = modo.ToLower();
            string archivo = "download";
            if (modo.Contains("json"))
            {
                string serializar = JsonSerializer.Serialize(tareas);
                await File.WriteAllTextAsync(archivo, serializar);
            }
            else if (modo.Contains("xml"))
            {
                XmlSerializer serializador = new XmlSerializer(typeof(List<Tarea>));
                using (TextWriter tw = new StreamWriter(archivo))
                {
                    serializador.Serialize(tw, tareas);
                }
            }

        }

        static async void CargarTareasDesdeArchivoAsync()
        {
            Console.WriteLine("Introduce la extension de tu archivo");
            String extension = Console.ReadLine();
            Console.WriteLine("Introduce el archivo");
            String archivo = Console.ReadLine();

            if (extension == "json")
            {
                string leer = await File.ReadAllTextAsync(archivo);
                tareas = JsonSerializer.Deserialize<List<Tarea>>(leer);
            }
            
            else
            {
                XmlSerializer serializador = new XmlSerializer(typeof(List<Tarea>));
                using (FileStream fs = new FileStream(archivo, FileMode.Open))
                {
                    tareas = (List<Tarea>)serializador.Deserialize(fs);
                }
            }
        }
    }
}