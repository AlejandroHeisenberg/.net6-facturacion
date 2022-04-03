using Microsoft.AspNetCore.Mvc;
using ProyectoVenta.Datos;
using ProyectoVenta.Models;


using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ProyectoVenta.Controllers
{
    public class AccesoController : Controller
    {
        //creamos una instancia para poder acceder y utilizar los metodos basicos de un crud, estos se encuentran en la carpeta Datos/DA_Usuario.cs
        DA_Usuario _daUsuario = new DA_Usuario();

        public IActionResult Index()
        {
            //retornamos a la vista de inicio de sesion- nuestro index
            return View();
        }

        //metodo de tipo post
        [HttpPost]
        //metodo que se va a sincronizar con el metodo SignInAsync
        public async Task<IActionResult> Index(string correo, string clave)
        {
            Usuario ouser = new Usuario();
            //validamos si el  correo y clave son los correctos para dejarlo ingresar al sistema
            ouser = _daUsuario.Listar().Where(u => u.Correo == correo && u.Clave == clave).FirstOrDefault();

            if (ouser == null) {
                //si en la variable ourser no encuentra ningun usuario con esas credenciales arrojara una alerta que se la pasamos a la vista  
                //por medio de ViewData["mensaje"] y seguimos en la vista de inicio de sesion.
                ViewData["mensaje"] = "Usuario no encontrado";
                //rtornamos a la vista principal(inicio de sesion)
                return View();
            }


            //si existe el usuario entonces creamos una lista de claims que contiene los datos que se envian por solicitud http para iniciar sesion que basicamente es por asi decirlo
            //nombre y valor y es con esta clase que podemos guardar informacion del ususario o virtualmente podemos guardar lo que queramos para posteriormente
            //acceder a esta info asi:
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, ouser.Correo),
                new Claim("NombreCompleto", ouser.NombreCompleto),
                new Claim(ClaimTypes.Role, "Administrador")
            };

            //const InsufficientExecutionStackException linea de codigo obtenemos el tipo de autenticacion que se udo en este caso la autenticacion fue por coockies
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //esperamos a que el codigo se ejecuta mientras esperamos a que termine el proceso de inicio de sesion asincronamente y pasamos como parametros:
            //el esquema de autenticacion que serian las cookies que es el tipo de autenticacion usado por usuario *
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));

            //SESIONES
            //redireccionamos a la pagina de aterrizaje despues de inciar sesion y nos vamos al metdo index del controlador Home
            //HttpContext.Session.SetString("correo", correo);
            //            RedirectToAction("string? actionName", "string? controllerName")
            return RedirectToAction("Index", "Home");
        }


        //este método se quiere sincronizar con métodos que se ejecutarán de forma asíncrona es decir esperar a que termine el metodo asincrono para poder sincronizarse.
        public async Task<IActionResult> Salir()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync( CookieAuthenticationDefaults.AuthenticationScheme);
            //devuelve al inicio de sesion
            return RedirectToAction("Index","Acceso");
        }
    }
}
