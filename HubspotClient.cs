using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HubspotClient.Clases;

namespace HubspotClient
{
    public class HubSpotClient
    {
        private readonly string token; // Aqui se asigna el api key
        private readonly string baseurl = "https://api.hubapi.com/crm/v3/objects/contacts";

        // Constructor, recibe el api key como parametro
        public HubSpotClient(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("La API key está vacía."); // lanza excepcion si la api key está vacia.
            this.token = token;
        }

        public string getToken()
        {
            return this.token;
        }
        /*********************************************************/
        /***********FUNCIONES CON EL API DE HUBSPOT***************/
        /*********************************************************/

        /* Recibe email como parametro y devuelve el id del cliente */

        /// <summary>
        /// Obtiene el id del contacto por email
        /// </summary>
        /// <param name="email">Email del contacto</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetContactID(string email)
        {
            var url = $"{baseurl}/{email}?idProperty=email";
            var response = await MakeRequest(HttpMethod.Get, url);
            EnsureSuccessResponse(response);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Contact>(body);
            EnsureSuccessResponse(response);
            return result.id;
        }

        /// <summary>
        /// Obtiene el contacto por id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contactProperties"></param>
        /// <returns>Contact</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Contact> GetContact(string id, List<string> contactProperties = null)
        {
            if (contactProperties == null)
            {
                contactProperties = DefaultContactProperties();
            }
            var url = $"{baseurl}/{id}?properties={string.Join(",", contactProperties)}";
            var response = await MakeRequest(HttpMethod.Get, url);
            EnsureSuccessResponse(response);
            var body = await response.Content.ReadAsStringAsync();
            var contactResult = JsonSerializer.Deserialize<Contact>(body);

            if (contactResult == null || contactResult.archived)
            {
                throw new Exception($"El contacto con id '{id}' no existe o está archivado.");
            }

            return contactResult;
        }

        /// <summary>
        /// Elimina el contacto pasandole el parametro ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteContact(string id)
        {
            var url = $"{baseurl}/{id}";
            var response = await MakeRequest(HttpMethod.Delete, url);
            EnsureSuccessResponse(response);
        }

        /// <summary>
        /// Obtiene todos los contactos
        /// </summary>
        /// <param name="contactProperties"></param>
        /// <returns>Lista de contact</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Contact>> GetAllContacts(List<string> contactProperties = null)
        {
            string? after = null;
            var allContactList = new List<Contact>();
            if (contactProperties == null)
            {
                contactProperties = DefaultContactProperties();
            }

            // Realizamos petición e iteramos los datos
            do
            {
                var url = $"{baseurl}?limit=100&properties={string.Join(",", contactProperties)}";
                if (after != null) url += $"&after={after}";
                using var response = await MakeRequest(HttpMethod.Get, url);
                EnsureSuccessResponse(response);
                var body = await response.Content.ReadAsStringAsync();

                Contacts? apiresult = JsonSerializer.Deserialize<Contacts>(body);
                if (apiresult?.contactResults != null)
                {
                    allContactList.AddRange(apiresult.contactResults);
                }
                after = apiresult?.paging?.next?.after;
            } while (after != null);

            return allContactList;
        }

        /// <summary>
        /// Crea un contacto
        /// </summary>  
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CreateContact(string firstname, string lastname, string email, string phone)
        {
            var contactData = new
            {
                properties = new
                {
                    firstname = firstname,
                    lastname = lastname,
                    email = email,
                    phone = phone
                }
            };

            var response = await MakeRequest(HttpMethod.Post, $"{baseurl}", JsonContent.Create(contactData));
            EnsureSuccessResponse(response);
        }

        /// <summary>
        /// Actualiza la informaicon de contacto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task UpdateContact(string id, string? firstname = null, string? lastname = null, string? email = null, string? phone = null)
        {
            var properties = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(firstname)) properties["firstname"] = firstname;
            if (!string.IsNullOrWhiteSpace(lastname)) properties["lastname"] = lastname;
            if (!string.IsNullOrWhiteSpace(email)) properties["email"] = email;
            if (!string.IsNullOrWhiteSpace(phone)) properties["phone"] = phone;

            var payload = new { properties = properties };

            var url = $"{baseurl}/{id}";

            var response = await MakeRequest(HttpMethod.Patch, url, new
                StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            EnsureSuccessResponse(response, responseContent);
        }

        private static void EnsureSuccessResponse(HttpResponseMessage response, string? responseContent = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}\n{responseContent}");
            }
        }

        private static List<string> DefaultContactProperties()
        {
            return new List<string>() {
                    "firstname",
                    "lastname",
                    "email",
                    "phone"
                };
        }

        private async Task<HttpResponseMessage> MakeRequest(HttpMethod method, string url, HttpContent? contentBody = null)
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(url),
                Headers = {
                    { "Host", "api.hubapi.com" },
                    { "authorization", $"Bearer {token}" },
                },
            };
            if (contentBody != null)
            {
                request.Content = contentBody;
            }
            return await client.SendAsync(request);
        }
    }
}