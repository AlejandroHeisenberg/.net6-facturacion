using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoVenta.Datos;
using ProyectoVenta.Models;

namespace ProyectoVenta.Controllers
{
    //decorador para decir que esta clase se puede utilizar solo si se esta autorizado en la autenticacion
    [Authorize]
    public class ReporteController : Controller
    {
        DA_Reporte _daReporte = new DA_Reporte();
        //metodo para redireccionar a la vista de ventas
        public IActionResult Ventas()
        {
            //reireccionamos a la vista index de reportes
            return View();
        }
        //metodo get
        [HttpGet]
        //metodo publico que retorna un jeson como resultado ty solicita como parametros la
        //fecha de inicio y fecha fin para genersr el reporte de ventas entre esos dias de intervalo
        public JsonResult ReporteVenta(string fechaInicio, string fechaFin)
        {
            //creamos una instancia de tipo lista pero esta lista sera de tipo reporte            
            List<Reporte> oLista = new List<Reporte>();
            //en la variable oLista accedemos por medio de la instancia al metodo listar y le pasamos las
            //fechas inicio y fin
            oLista = _daReporte.Listar(fechaInicio, fechaFin);
            //retornamos un json con una nueva data, relacionada a reporte de ventas entre 2 fechas
            return Json(new { data = oLista });
        }
    }
}
