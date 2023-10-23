using System;
namespace GestionTareasBS
{
    public class Tarea
    {
        public String Descripcion { get; set; }
        public DateTime Vencimiento { get; set; }
        public Boolean Completado { get; set; }

        public Tarea(String Descripcion, DateTime Vencimiento, Boolean Completado)
        {
            this.Descripcion = Descripcion;
            this.Vencimiento = Vencimiento;
            this.Completado = Completado;
        }
    }
}