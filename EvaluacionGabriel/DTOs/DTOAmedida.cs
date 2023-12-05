namespace EvaluacionGabriel.DTOs
{
    public class DTOAmedida
    {
        public int IdEquipo { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public double PromedioEdad { get; set; }
        public List<DTOAmedida2> ListaJugadores { get; set; }
    }

    public class DTOAmedida2
    {
        public int IdJugador { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public decimal Sueldo { get; set; }
    }

}
