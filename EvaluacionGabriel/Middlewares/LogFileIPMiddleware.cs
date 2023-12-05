namespace EvaluacionGabriel.Middlewares
{
    public class LogFileIPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        // Equipamos al middleware con lo que va a necesitar. Para que un middleware de paso al siguiente, debemos inyectar RequestDelegate y llamar
        // a la propiedad que lo coge como _next (podría tener otro nombre)
        // En este caso necesitamos IWebHostEnvironment para poder acceder a información del sistema de carpetas del servidor
        public LogFileIPMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var IP = httpContext.Connection.RemoteIpAddress.ToString();


            // PARA BLOQUEAR METODOS, EN ESTE CASO BLOQUEAMOS LOS GET
            // GET

            //if (httpContext.Request.Method == "GET")
            //{
            //    httpContext.Response.StatusCode = 400;
            //    return;
            //}



            var ruta = httpContext.Request.Path.ToString();
            var metodo = httpContext.Request.Method;

            //var path = $@"{_env.ContentRootPath}\wwwroot\consultas.txt";



            if (metodo.ToUpper() == "GET")
            {
                var path = Path.Combine(_env.ContentRootPath, "wwwroot", "consultas.txt");
                using (StreamWriter writer = new StreamWriter(path, append: true))
                {
                    writer.WriteLine($@"{IP} - {DateTime.Now} -  {ruta} - {metodo}");
                    writer.WriteLine();

                }
            }

            await _next(httpContext);
        }
    }
}
