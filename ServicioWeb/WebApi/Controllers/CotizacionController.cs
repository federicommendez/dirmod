using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    public class CotizacionController : ApiController
    {
        public Models.TipoCambio GetCotizacion(string change)
        {
            Models.TipoCambio typeOfChange = new Models.TipoCambio();
            switch (change)
            {
                case "dolar":
                {
                    typeOfChange = GetCotizationOfDolar();
                    break;
                }
                case "euro":
                {
                    typeOfChange = GetCotizationOfEuro();
                    break;
                }
                case "real":
                {
                    typeOfChange = GetCotizationOfReal();
                    break;
                }
                default:
                {
                    typeOfChange.moneda = "No se pudo obtener cambio.";
                    typeOfChange.precio = 0;
                    break;
                }
            }

            return typeOfChange;
        }

        [Route("api/Cotizacion/Dolar")]
        [HttpGet]
        public Models.TipoCambio GetCotizationOfDolar()
        {
            return GetCotization("Dolar");
        }
        [Route("api/Cotizacion/Euro")]
        [HttpGet]
        public Models.TipoCambio GetCotizationOfEuro()
        {
            return GetCotization("Euro");
        }
        [Route("api/Cotizacion/Real")]
        [HttpGet]
        public Models.TipoCambio GetCotizationOfReal()
        {
            return GetCotization("Real");
        }

        private Models.TipoCambio GetCotization(string typeOfChange)
        {
            Models.TipoCambio cotization = new Models.TipoCambio();
            string acronym = ObtainAcronymOfTypeOfChange(typeOfChange);
            string url = "https://api.cambio.today/v1/quotes/ARS/" + acronym + "/json?quantity=1&key=2496|jhe4CVDxDGBSLunwLKXVpp^ip~33u4KO";
            var json = new WebClient().DownloadString(url);
            dynamic DeserealizedTypeOfChange = JsonConvert.DeserializeObject(json);
            cotization.moneda = typeOfChange;
            cotization.precio = DeserealizedTypeOfChange.result.value;
            return cotization;
        }

        private string ObtainAcronymOfTypeOfChange(string typeOfChange)
        {
            string acronym = "";
            switch (typeOfChange)
            {
                case "Dolar":
                    acronym = "USD";
                    break;
                case "Euro":
                    acronym = "EUR";
                    break;
                case "Real":
                    acronym = "BRL";
                    break;
                default:
                    acronym = "USD";
                    break;
            }
            return acronym;
        }
    }
}
