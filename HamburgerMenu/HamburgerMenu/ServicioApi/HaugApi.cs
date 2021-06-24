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
        public async Task PostJsonHttpClient(string ID_TAREADOR, string ID_PERSONAL, string PERSONAL, string ID_PROYECTO,
            string ID_SITUACION, string ID_CLASE_TRABAJADOR, DateTime FECHA_TAREO, string TIPO_MARCACION, string HORA,
            DateTime FECHA_REGISTRO, string dni)
        {
            try
            {
                TareoPersonalApi varTareo = new TareoPersonalApi
                {
                    ID = 1,
                    ID_TAREADOR = ID_TAREADOR,
                    ID_PERSONAL = Convert.ToInt32(ID_PERSONAL),
                    PERSONAL = PERSONAL,
                    ID_PROYECTO = ID_PROYECTO,
                    ID_SITUACION = Convert.ToInt32(ID_SITUACION),
                    ID_CLASE_TRABAJADOR = Convert.ToInt32(ID_CLASE_TRABAJADOR),
                    FECHA_TAREO = FECHA_TAREO,
                    TIPO_MARCACION = Convert.ToInt32(TIPO_MARCACION),
                    HORA = HORA,
                    FECHA_REGISTRO = FECHA_REGISTRO,
                    SINCRONIZADO = 0,
                    FECHA_SINCRONIZADO = DateTime.Now.Date,
                    TOKEN = App.Token,
                    NUMERO_DOCUIDEN = dni,
                    ORIGEN = 1,
                    ID_SUCURSAL = App.Sucursal
                    
                };

                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(varTareo);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.PostAsync(urlapiTareo, httpContent).ConfigureAwait(false);
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

                    client.BaseAddress = new Uri(urlapiTareo);
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

    }
}
