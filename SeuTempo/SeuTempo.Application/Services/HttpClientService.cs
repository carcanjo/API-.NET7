using Newtonsoft.Json;
using SeuTempo.Application.Interfaces;
using SeuTempo.Application.ViewModel;
using SeuTempo.Core.Enuns;
using SeuTempo.Core.Exceptions;
using System;
using System.Text;

namespace SeuTempo.Application.Services
{
    public class HttpClientService<T> : IHttpClientService<T>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiConfigService _apiConfig;

        public HttpClientService(IHttpClientFactory httpClientFactory, IApiConfigService apiConfig)
        {
            _httpClientFactory = httpClientFactory;
            _apiConfig = apiConfig;
        }

        public async Task<ResponseHttpViewModel<T>> RequestHttpAsync(string url, bool autenticacao, ProtocoloHttp protocoloHttp,
            dynamic payload, string aplicacao, long? id = null)
        {
            ResponseHttpViewModel<T> resultado = new();

            try
            {
                if (string.IsNullOrEmpty(aplicacao))
                    throw new DomainException("Aplicação é obrigatório");

                using var request = await GetHttpRequestMessageAsync(url, protocoloHttp, payload, id);
                var client = _httpClientFactory.CreateClient(aplicacao);

                if (autenticacao)
                    client.DefaultRequestHeaders.Add("Authorization", await AutenticacaoAsync(aplicacao));

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    resultado = JsonConvert.DeserializeObject<ResponseHttpViewModel<T>>(responseStream);
                }
                else
                {
                    if ((int)response.StatusCode == 404)
                        throw new DomainException($"url {url} não existe");

                    var responseStream = await response.Content.ReadAsStreamAsync();
                    resultado = JsonConvert.DeserializeObject<ResponseHttpViewModel<T>>(responseStream);
                }

                return resultado;
            }
            catch (DomainException ex)
            {
                throw new DomainException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<HttpRequestMessage>? GetHttpRequestMessageAsync(string url, ProtocoloHttp protocoloHttp, dynamic payload, long? id)
        {
            try
            {
                var requestMessage = new HttpRequestMessage();

                switch (protocoloHttp)
                {
                    case ProtocoloHttp.Post:
                        requestMessage = new(HttpMethod.Post, url);
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, _apiConfig.AplicationJson);
                        break;
                    case ProtocoloHttp.Put:
                        requestMessage = new(HttpMethod.Put, url);
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, _apiConfig.AplicationJson);
                        break;
                    case ProtocoloHttp.Get:
                        var urlRequest = id is null ? url : $"{url}/{id}";
                        requestMessage = new(HttpMethod.Get, urlRequest);
                        break;
                    case ProtocoloHttp.Delete:
                        requestMessage = new(HttpMethod.Delete, $"{url}/{id}");
                        break;
                    default:
                        break;
                }

                return requestMessage;
            }
            catch (DomainException ex)
            {
                throw new DomainException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        private async Task<string?> AutenticacaoAsync(string aplicacao)
        {
            try
            {
                var resultado = new TokenViewModel();
                var client = _httpClientFactory.CreateClient(aplicacao);
                var payload = new { clientId = "", clientSecret = "" };

                using HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, _apiConfig.AplicationJson);

                var response = await client.PutAsync("_apiConfig.UrlToken", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    resultado = JsonConvert.DeserializeObject<TokenViewModel>(responseStream);
                }
                else
                {
                    if ((int)response.StatusCode == 404)
                        throw new DomainException($"url não existe");

                    var responseStream = await response.Content.ReadAsStringAsync();
                    resultado = JsonConvert.DeserializeObject<TokenViewModel>(responseStream);
                }

                return $"Bearer {resultado.Token}";
            }
            catch (DomainException ex)
            {
                throw new DomainException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
