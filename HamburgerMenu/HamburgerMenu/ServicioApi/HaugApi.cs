using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using HamburgerMenu.Models;
using System.Threading;
using System.IO;
using System.Net.Http.Headers;

namespace HamburgerMenu.ServicioApi
{
    public class HaugApi
    {
        public static HaugApi Metodo = new HaugApi();
        public const string urlapi = "http://ti.haug.com.pe/WebApiPersonalTareo/Api/PERSONAL";
        public const string urlapiTareo = "http://ti.haug.com.pe/WebApiPersonalTareo/Api/TAREOPERSONAL";
        public const string urlapiToken = "http://ti.haug.com.pe/WebApiPersonalTareo/Api/TAREADOR_DISPOSITIVOS";
        public const string urlapiHorario = "http://ti.haug.com.pe/WebApiPersonalTareo/Api/HORARIO";
        public const string urlapiSucursal = "http://ti.haug.com.pe/WebApiPersonalTareo/Api/SUCURSAL";

        public const string urlapiS10 = "http://ti.haug.com.pe/WATareoS10/Api/Listar_Personal_S10";
        public const string urlapiTareoS10 = "http://ti.haug.com.pe/WATareoS10/Api/MARCACION_PERSONAL";

        public async Task<List<TareadorDispositivosApi>> GetToken(string ParamCelular, string ParamTareador)
        {
            List<TareadorDispositivosApi> LstTask = new List<TareadorDispositivosApi>();
            try
            {
                HttpClient client = new HttpClient
                {
                    MaxResponseContentBufferSize = 256000
                };
                var uri = new Uri(urlapiToken + "?celular=" + ParamCelular + "&tareador=" + ParamTareador);
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LstTask = JsonConvert.DeserializeObject<List<TareadorDispositivosApi>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return LstTask;
        }
        public async Task<List<PersonalTareoApi>> GetAllPersonalTareadorAsync(string ParamTareador)
        {
            List<PersonalTareoApi> LstTask = new List<PersonalTareoApi>();
            try
            {
                HttpClient client = new HttpClient
                {
                    MaxResponseContentBufferSize = 256000
                };
                var uri = new Uri(urlapi + "?tareador=" + ParamTareador);
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LstTask = JsonConvert.DeserializeObject<List<PersonalTareoApi>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return LstTask;
        }
        public async Task<List<HorarioApi>> GetAllHorarioApiAsync()
        {
            List<HorarioApi> LstTask = new List<HorarioApi>();
            try
            {
                HttpClient client = new HttpClient
                {
                    MaxResponseContentBufferSize = 256000
                };
                var uri = new Uri(urlapiHorario);
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LstTask = JsonConvert.DeserializeObject<List<HorarioApi>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return LstTask;
        }
        public async Task<List<SucursalApi>> GetAllSucursalApiAsync()
        {
            List<SucursalApi> LstTask = new List<SucursalApi>();
            try
            {
                HttpClient client = new HttpClient
                {
                    MaxResponseContentBufferSize = 256000
                };
                var uri = new Uri(urlapiSucursal);
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LstTask = JsonConvert.DeserializeObject<List<SucursalApi>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return LstTask;
        }
        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
        //public async Task PostJsonHttpClient(object content, CancellationToken cancellationToken)
        public async Task PostJsonHttpClient(string ID_TAREADOR, string PROYECTO, string CODOBRERO, string PERSONAL, string DNI,string TIPO_MARCACION, DateTime FECHA_TAREO,string HORA,DateTime FECHA_REGISTRO,
           Guid NroEsquemaPlanilla, string CodInsumo, string Insumo, string CodOcupacion, string Ocupacion)
        {
            try
            {
                TareoPersonalS10Api varTareo = new TareoPersonalS10Api
                {
                    ID = 1,
                    ID_TAREADOR = ID_TAREADOR,
                    PROYECTO = PROYECTO,
                    CODOBRERO = CODOBRERO,
                    PERSONAL = PERSONAL,
                    DNI = DNI,
                    TIPO_MARCACION = Convert.ToInt32(TIPO_MARCACION),
                    FECHA_MARCACION = FECHA_TAREO,
                    HORA = HORA,
                    FECHA_REGISTRO = FECHA_REGISTRO,
                    SINCRONIZADO = 0,
                    FECHA_SINCRONIZADO = DateTime.Now.Date,
                    TOKEN = App.Token,
                    ID_SUCURSAL = App.Sucursal,
                    ORIGEN = "1",
                    NroEsquemaPlanilla = NroEsquemaPlanilla,
                    CodInsumo = CodInsumo,
                    Insumo = Insumo,
                    CodOcupacion = CodOcupacion,
                    Ocupacion = Ocupacion
                };

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(varTareo);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PostAsync(urlapiTareoS10, httpContent).ConfigureAwait(false);
                string respuesta = response.RequestMessage.ToString();
                string status_code = response.StatusCode.ToString();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<String> CrearTareo(TareoPersonalApi entidad)
        {
            String codigoMovimientoEspecial = String.Empty;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var informacionAMandar = Newtonsoft.Json.JsonConvert.SerializeObject(entidad,
                                    Newtonsoft.Json.Formatting.None,
                                    new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore
                                    });

                    client.BaseAddress = new Uri(urlapiTareoS10);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromMinutes(10);


                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "")
                    {
                        Content = new StringContent(informacionAMandar, Encoding.UTF8, "application/json")
                    };
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonPuro = await response.Content.ReadAsStringAsync();
                        var jsonDesarializado = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonPuro);

                        codigoMovimientoEspecial = jsonDesarializado.ToString();
                    }
                }

                return codigoMovimientoEspecial;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PersonalS10Api>> GetAllPersonalS10TareadorAsync(string ParamTareador)
        {
            List<PersonalS10Api> LstTask = new List<PersonalS10Api>();
            try
            {
                HttpClient client = new HttpClient
                {
                    MaxResponseContentBufferSize = 256000
                };
                var uri = new Uri(urlapiS10 + "?proyecto=01002001&&tareador=" + ParamTareador + "&planilla=89BE31CF-5A9D-43A2-BD1F-6E025DAF8F12&activo=true");
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LstTask = JsonConvert.DeserializeObject<List<PersonalS10Api>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return LstTask;
        }

    }
}
