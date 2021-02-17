using Application;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApi_Test.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UsuarioController : ControllerBase
    {
        string sId = "0";
        string sDados = "";
        string sStatus = "001";
        string sDescricao = "";
        string sRequestType = "";
        string sRequestObject = "";

        [HttpPost]
        [Route("Api/Usuario")]
        public IActionResult Post([FromBody] ObjectRequest oRequest)
        {
            sStatus = "001";
            Usuario oUsuario = null;
            ConfigInfo oInfor = Util.ProjectInfo();
            JObject _DsJson = new JObject();
            try
            {
                string ApiToken = Util.ValidarEntrada(HttpContext.Request.Headers["Authorization"].ToString());
                if (Util.IsNull(ApiToken))
                {
                    sStatus = "901";
                    sDescricao = "Atributo do Headers 'Authorization' é obrigatório.";
                }
                else
                {
                    if (ApiToken != oInfor.Api_Key)
                    {
                        sStatus = "902";
                        sDescricao = "Atributo do Headers 'Authorization' não é válido.";
                    }
                }
            }
            catch
            {
                sStatus = "999";
                sDescricao = "Erro inesperado.";
            }

            if (sStatus == "001")
            {
                try
                {
                    sRequestType = oRequest.RequestType.ToUpper();
                }
                catch
                {
                    sStatus = "903";
                    sDescricao = "Atributo do Body 'RequestType' é obrigatório.";
                }
            }

            if (sStatus == "001")
            {
                try { sRequestObject = oRequest.RequestObject; } catch { }
            }

            if (sStatus == "001")
            {
                try
                {
                    if (!Util.IsNull(sRequestObject))
                    {
                        sRequestObject = Util.FormatarHtml(sRequestObject);
                        oUsuario = JsonConvert.DeserializeObject<Usuario>(sRequestObject);
                    }
                }
                catch
                {
                    sStatus = "904";
                    sDescricao = "Atributo do Body 'RequestObject' é obrigatório.";
                }
            }

            if (sStatus == "001")
            {
                sId = "0";
                try
                {
                    switch (sRequestType)
                    {
                        case "CADASTRAR":
                            {
                                _DsJson = Proc.UsuarioCadastrar(oUsuario);
                                try { sId = _DsJson["Retorno"]["Id"].ToString(); } catch { }
                                try { sStatus = _DsJson["Retorno"]["Status"].ToString(); } catch { }
                                try { sDescricao = _DsJson["Retorno"]["Descricao"].ToString(); } catch { }
                            }
                            break;

                        case "ATUALIZAR":
                            {
                                _DsJson = Proc.UsuarioAtualizar(oUsuario);
                                try { sId = _DsJson["Retorno"]["Id"].ToString(); } catch { }
                                try { sStatus = _DsJson["Retorno"]["Status"].ToString(); } catch { }
                                try { sDescricao = _DsJson["Retorno"]["Descricao"].ToString(); } catch { }
                            }
                            break;

                        case "LISTAR":
                            {
                                _DsJson = Proc.UsuarioListar(oUsuario);
                                try { sStatus = _DsJson["Retorno"]["Status"].ToString(); } catch { }
                                try { sDescricao = _DsJson["Retorno"]["Descricao"].ToString(); } catch { }
                                try { sDados = _DsJson["Dados"].ToString(); } catch { }
                            }
                            break;

                        default:
                            {
                                sStatus = "904";
                                sDescricao = "Atributo do Body 'RequestType' não é válido.";
                            }
                            break;
                    }
                }
                catch
                {
                    sStatus = "999";
                    sDescricao = "Erro inesperado.";
                }
            }

            //=========================================================================================
            if ((sStatus != "000") && (sStatus != "001")) { sDados = ""; }
            //=========================================================================================
            string sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"Retorno\": { ";
            if (sId != "0") { sjReturn += "\"Id\": \"" + sId + "\", "; }
            sjReturn += "\"Status\": \"" + sStatus + "\", ";
            sjReturn += "\"Descricao\": \"" + sDescricao + "\" ";
            if (!Util.IsNull(sDados))
            {
                sjReturn += "}, ";
                sjReturn += "\"Dados\": " + sDados;
            }
            else
                sjReturn += "} ";
            sjReturn += "} ";
            //=========================================================================================
            return Content(sjReturn);
            //=========================================================================================
        }
    }
}
