using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Application
{
    public static class Util
    {
        //===================================================================================================================

        public static string Label(string ToolId, string ToolValue, string ArrayIds, string ArrayLabels, string Title = "")
        {
            string sHtml = "";
            string[] aIds = ArrayIds.Split('|');
            string[] aLabels = ArrayLabels.Split('|');
            //----------------------------------------
            if (Util.IsNull(Title)) { sHtml += "<label id='" + ToolId + "' name='" + ToolId + "'>"; }
            else { sHtml += "<label id='" + ToolId + "' name='" + ToolId + "'><b>" + Title + ": </b>"; }
            for (int i = 0; i < aIds.Length; i++)
            {
                if (ToolValue == aIds[i])
                {
                    try { sHtml += aLabels[i]; } catch { }
                    break;
                }
            }
            sHtml += "</label>";
            //----------------------------------------
            return sHtml;
        }

        //===================================================================================================================

        public static string Select(string ToolId, string ToolClass, string ToolValue, string ArrayIds, string ArrayLabels, string ToolOnChangeValue = "")
        {
            string sHtml = "";
            string[] aIds = ArrayIds.Split('|');
            string[] aLabels = ArrayLabels.Split('|');
            //----------------------------------------
            if (Util.IsNull(ToolOnChangeValue)) { sHtml += "<select class=\"" + ToolClass + "\" required id='" + ToolId + "' name='" + ToolId + "'>"; }
            else { sHtml += "<select class=\"" + ToolClass + "\" required id='" + ToolId + "' name='" + ToolId + "' onchange=\"javascript: " + ToolOnChangeValue + "(this.value);\">"; }
            for (int i = 0; i < aIds.Length; i++)
            {
                if (ToolValue != aIds[i])
                    try { sHtml += "<option value=\"" + aIds[i] + "\">" + aLabels[i] + "</option>"; } catch { }
                else
                    try { sHtml += "<option value=\"" + aIds[i] + "\" selected=\"selected\">" + aLabels[i] + "</option>"; } catch { }
            }
            sHtml += "</select>";
            //----------------------------------------
            return sHtml;
        }

        //===================================================================================================================

        public static bool IsNull(string Texto)
        {
            bool IsStatus = false;
            if ((Texto == null) || (Texto.ToUpper() == "NULL") || (Texto == ""))
                IsStatus = true;
            return IsStatus;
        }

        //===================================================================================================================

        public static string Encrypt(string Texto)
        {
            byte[] by_key = System.Text.ASCIIEncoding.UTF8.GetBytes(Texto);
            string encrypt_key = Convert.ToBase64String(by_key);
            for (int i = 0; i < 5; i++) { encrypt_key = encrypt_key.Replace("=", "!@"); }
            return encrypt_key;
        }

        //===================================================================================================================

        public static string Descrypt(string Texto)
        {
            for (int i = 0; i < 5; i++) { Texto = Texto.Replace("!@", "="); }
            byte[] by_key = Convert.FromBase64String(Texto);
            string descrypt_key = System.Text.ASCIIEncoding.UTF8.GetString(by_key);
            return descrypt_key;
        }

        //===================================================================================================================

        private static string ValidaTexto(string Texto)
        {
            string TxTeste = "";
            try { if (Util.IsNull(Texto)) { Texto = ""; } } catch { Texto = ""; }
            try { Texto = Texto.Replace(Environment.NewLine, ""); } catch { }
            try { Texto = Regex.Replace(Texto, @"[\r\n]+", ""); } catch { }
            try { Texto = Regex.Replace(Texto, @"[\t\r\n]+", ""); } catch { }
            try { Texto = Regex.Replace(Texto, @"[\v]+", ""); } catch { }
            Texto = Texto.Trim();
            Texto = Texto.Replace("  ", "&nbsp;&nbsp;");
            Texto = Texto.Replace("=", "%3d");
            Texto = Texto.Replace("'", "&squo;");
            Texto = Texto.Replace("\"", "&quot;");
            Texto = Texto.Replace("\\", "&#92;");
            TxTeste = Texto.ToUpper();
            if (TxTeste.IndexOf("UNION ") > -1) { Texto = ""; }
            if (TxTeste.IndexOf("SELECT ") > -1) { Texto = ""; }
            if (TxTeste.IndexOf("INSERT ") > -1) { Texto = ""; }
            if (TxTeste.IndexOf("DELETE ") > -1) { Texto = ""; }
            if (TxTeste.IndexOf("UPDATE ") > -1) { Texto = ""; }
            if (TxTeste.IndexOf("DROP ") > -1) { Texto = ""; }
            return Texto;
        }

        //===================================================================================================================

        public static string ValidarEntrada(string Texto, int iLength = 0, string sDefault = "")
        {
            try { Texto = ValidaTexto(Texto); } catch { }
            try
            {
                if ((iLength > 0) && (Texto.Length > iLength)) { Texto = Texto.Substring(0, iLength); }
            }
            catch { }
            try
            {
                if ((!Util.IsNull(sDefault)) && (Texto.Length < 1)) { Texto = sDefault; }
            }
            catch { }
            return Texto;
        }

        //===================================================================================================================

        public static string SomenteNumeros(string Texto)
        {
            Texto = ValidaTexto(Texto);
            string Numeros = "";
            try
            {
                Regex regexObj = new Regex(@"[^\d]");
                Numeros = regexObj.Replace(Texto, "");
            }
            catch { Numeros = ""; }
            return Numeros;
        }

        //===================================================================================================================

        public static string FormatarHtml(string Texto)
        {
            Texto = Texto.Replace("&#92;", "\\");
            Texto = Texto.Replace("&quot;", "\"");
            Texto = Texto.Replace("&squo;", "'");
            Texto = Texto.Replace("%3d", "=");
            Texto = Texto.Replace("&nbsp;&nbsp;", "  ");
            return Texto;
        }

        //===================================================================================================================

        public static string FormatarData(string sDataHoar, string sTipo)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            sDataHoar = ValidaTexto(sDataHoar);
            if (Util.IsNull(sDataHoar)) { sDataHoar = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
            try
            {
                if (sTipo == "BR") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("dd/MM/yyyy"); }
                if (sTipo == "BRR") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("dd/MM/yy"); }
                if (sTipo == "BRT") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("dd/MM/yyyy HH:mm:ss"); }
                if (sTipo == "BRTR") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("dd/MM/yy HH:mm:ss"); }
                if (sTipo == "US") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("yyyy-MM-dd"); }
                if (sTipo == "USR") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("yy-MM-dd"); }
                if (sTipo == "UST") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("yyyy-MM-dd HH:mm:ss"); }
                if (sTipo == "USTR") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("yy-MM-dd HH:mm:ss"); }
                if (sTipo == "T_HMS") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("HH:mm:ss"); }
                if (sTipo == "T_HM") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("HH:mm"); }
                if (sTipo == "T_H") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("HH"); }
                if (sTipo == "T_M") { sDataHoar = Convert.ToDateTime(sDataHoar).ToString("mm"); }
            }
            catch { sDataHoar = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
            return sDataHoar;
        }

        //===================================================================================================================

        public static string FormatarValor(string sValor, string sRep = ",", int iDec = 2)
        {
            //---------------------------------------------------------------------------------
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            //---------------------------------------------------------------------------------
            string vRet = "";
            if (Util.IsNull(sValor)) { sValor = "0,00"; }
            sValor = sValor.Replace(" ", "").Trim();
            sValor = sValor.Replace("\\T", "");
            sValor = sValor.Replace("\t", "");
            sValor = sValor.Replace("\\R", "");
            sValor = sValor.Replace("\r", "");
            sValor = sValor.Replace("\\N", "");
            sValor = sValor.Replace("\n", "");
            sValor = sValor.Replace("R", "");
            sValor = sValor.Replace("$", "");
            if ((sValor.IndexOf(".") > -1) && (sValor.IndexOf(",") > -1))
                sValor = sValor.Replace(".", "");
            if ((sValor.IndexOf(".") > -1) && (sValor.IndexOf(",") < 0))
                sValor = sValor.Replace(".", "");
            double dValue = Convert.ToDouble(sValor.Replace(".", ","));
            //---------------------------------------------------------------------------------
            vRet = String.Format("{0:N" + iDec + "}", dValue).Replace(".", "");
            //---------------------------------------------------------------------------------
            if (sRep == ".") { vRet = vRet.Replace(",", "."); }
            if (sRep == "") { vRet = vRet.Replace(",", ""); }
            //---------------------------------------------------------------------------------
            return vRet;
            //---------------------------------------------------------------------------------
        }

        //===================================================================================================================

        public static JObject FormatarObjeto(string sRequestType, string sJson = "")
        {
            string sjReturn = "";
            sJson = Util.ValidaTexto(sJson);
            sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"RequestType\": \"" + sRequestType + "\" ";
            if (!Util.IsNull(sJson)) { sjReturn += ", \"RequestObject\": \"" + sJson + "\" "; }
            sjReturn += "} ";
            return JObject.Parse(sjReturn);
        }

        //===================================================================================================================
        //===================================================================================================================

        public static JObject GetRequest(string Metodo, string ApiUrl, string ApiHeaderChave, string ApiHeaderValor, JObject obj = null, string IdUsuario = "0", string Titulo = "Titulo da requisição")
        {
            string Result = "", RequisicaoObj = "";
            var client = new RestClient(ApiUrl);
            client.Timeout = -1;
            var request = new RestRequest();
            switch (Metodo)
            {
                case "DELETE": request = new RestRequest(Method.DELETE); break;
                case "GET": request = new RestRequest(Method.GET); break;
                case "POST": request = new RestRequest(Method.POST); break;
                case "PUT": request = new RestRequest(Method.PUT); break;
            }
            request.AddHeader("Accept", "application/json");
            if ((!Util.IsNull(ApiHeaderChave)) && (!Util.IsNull(ApiHeaderValor))) { request.AddHeader(ApiHeaderChave, ApiHeaderValor); }
            if (obj != null)
                request.Parameters.Add(new Parameter("application/json;charset=UTF-8", obj, ParameterType.RequestBody));
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var Response = client.Execute(request);
            if (Response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(Response.Content);
            else
                Result = Response.Content;

            try { RequisicaoObj = obj.ToString(); } catch { }

            return JObject.Parse(Result);
        }

        //===================================================================================================================
        //===================================================================================================================

        public static ConfigInfo ProjectInfo()
        {
            string StConfig = "111";
            ConfigInfo oDados = new ConfigInfo();
            try { oDados.appDirectory = AppDomain.CurrentDomain.BaseDirectory; } catch { oDados.appDirectory = ""; };
            try { oDados.projectPathBin = oDados.appDirectory.Substring(0, oDados.appDirectory.IndexOf("\\bin")); } catch { oDados.projectPathBin = ""; };
            try { oDados.projectPathRoot = oDados.appDirectory.Substring(0, oDados.appDirectory.IndexOf("\\wwwroot")); } catch { oDados.projectPathRoot = ""; };

            string EndConfig = "";
            oDados.Code = "111";
            if (oDados.projectPathRoot != "")
            {
                EndConfig = Path.Combine(oDados.projectPathRoot, "appsettings.json");
                if (File.Exists(EndConfig)) { StConfig = "000"; }
            }

            if ((oDados.projectPathBin != "") && (StConfig == "111"))
            {
                EndConfig = Path.Combine(oDados.projectPathBin, "appsettings.json");
                if (File.Exists(EndConfig)) { StConfig = "000"; }
            }

            if ((oDados.appDirectory != "") && (StConfig == "111"))
            {
                EndConfig = Path.Combine(oDados.appDirectory, "appsettings.json");
                if (File.Exists(EndConfig)) { StConfig = "000"; }
            }

            if ((EndConfig != "") && (StConfig == "000"))
            {
                string oJson = "";
                using (StreamReader r = new StreamReader(EndConfig))
                {
                    oJson = r.ReadToEnd();
                    JObject oObject = JObject.Parse(oJson);
                    //----------------------------------------------------------------------------------------------------------------------------------------
                    try { oDados.DefaultConnection = oObject["ConnectionStrings"]["DefaultConnection"].ToString(); } catch { oDados.DefaultConnection = ""; }
                    //----------------------------------------------------------------------------------------------------------------------------------------
                    try { oDados.appUrl = oObject["Url.Base"].ToString(); } catch { oDados.appUrl = "./"; }
                    try { oDados.Api_Url_Base = oObject["API"]["Url.Base"].ToString(); } catch { oDados.Api_Url_Base = ""; }
                    try { oDados.Api_Key = oObject["API"]["Key"].ToString(); } catch { oDados.Api_Key = ""; }
                    //----------------------------------------------------------------------------------------------------------------------------------------
                }
            }

            return oDados;
        }

        private static SqlConnection sConn;
        private static SqlCommand cmd;
        private static SqlDataAdapter da;
        private static DataSet dsRet;

        private static void ConnectionString()
        {
            ConfigInfo oDados = ProjectInfo();
            sConn = new SqlConnection(oDados.DefaultConnection);
            cmd = new SqlCommand
            {
                Connection = sConn,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 1000000 // 10 minutos
            };
        }

        private static void Open()
        {
            ConnectionString();

            try
            {
                if (sConn.State == ConnectionState.Closed)
                    sConn.Open();
            }
            catch { Close(); }
        }

        private static void Close()
        {
            try
            {
                if (sConn.State == ConnectionState.Open)
                    sConn.Close();
            }
            catch { }

            try
            {
                if (sConn.State != 0)
                { sConn.Close(); }
            }
            catch { }
        }

        private static void ExecuteNonQuery(string SQL)
        {
            try
            {
                Open();
                cmd = new SqlCommand
                {
                    CommandText = SQL,
                    CommandType = CommandType.Text,
                    Connection = sConn
                };
                cmd.ExecuteNonQuery();
            }
            catch { Close(); }
            finally { Close(); }
        }

        private static DataSet ExecutaDataSet(string SQL)
        {
            dsRet = new DataSet();
            try
            {
                Open();
                da = new SqlDataAdapter(SQL, sConn);
                da.Fill(dsRet, "Dados");
            }
            catch { Close(); }
            finally { Close(); }
            return dsRet;
        }

        private static DataSet getDataSet(string SQL, int Pagina = 0, int Itens = 0)
        {
            DataSet dsDS = new DataSet();
            DataSet pgDS = new DataSet();
            DataSet dsRet = new DataSet();
            DataTable dtPag = new DataTable { TableName = "Paginacao" };
            DataTable dtDat = new DataTable { TableName = "Dados" };
            DataRow dtRow = dtPag.NewRow();
            long RetInt_TotalItens = 0;
            long RetInt_TotalPagina = 1;
            try
            {
                string SqlSub = SQL;
                SqlSub = SqlSub.Replace(" ", "");
                SqlSub = SqlSub.ToUpper();
                SqlSub = SqlSub.Substring(0, 4);

                switch (SqlSub)
                {
                    case "SELE":
                    case "EXEC":
                        {
                            try
                            {
                                if ((Pagina > 0) && (Itens > 0))
                                {
                                    Open();
                                    da = new SqlDataAdapter(SQL, sConn);
                                    da.Fill(dsDS, "Dados");
                                    RetInt_TotalItens = dsDS.Tables["Dados"].Rows.Count;
                                    if (Itens > 0)
                                    {
                                        if (RetInt_TotalItens > Itens)
                                        {
                                            int iIndex = 0;
                                            if ((Pagina - 1) > 0)
                                                iIndex = ((Pagina - 1) * Itens);
                                            da.Fill(pgDS, iIndex, Itens, "Dados");
                                            try
                                            {
                                                double dTotalItens = Convert.ToDouble(RetInt_TotalItens);
                                                double dItens = Convert.ToDouble(Itens);
                                                double dTtlPages = Math.Ceiling(dTotalItens / dItens);
                                                RetInt_TotalPagina = Convert.ToInt32(dTtlPages);
                                            }
                                            catch { }
                                        }
                                    }

                                    dtPag.Columns.Add("Paginas", typeof(Int64));
                                    dtPag.Columns.Add("Itens", typeof(Int64));
                                    dtRow = dtPag.NewRow();
                                    dtRow[0] = RetInt_TotalPagina;
                                    dtRow[1] = RetInt_TotalItens;
                                    dtPag.Rows.Add(dtRow);

                                    if (RetInt_TotalPagina < 2)
                                        dsRet = dsDS;
                                    else
                                        dsRet = pgDS;

                                    dsRet.Tables.Add(dtPag);
                                }
                                else
                                    dsRet = ExecutaDataSet(SQL);
                            }
                            catch
                            {
                                Close();
                                throw;
                            }
                            finally
                            {
                                Close();
                            }
                        }
                        break;
                    case "INSE":
                        { ExecuteNonQuery(SQL); }
                        break;
                    case "UPDA":
                    case "DELE":
                        {
                            if (SQL.ToUpper().IndexOf("WHERE") > 0)
                                ExecuteNonQuery(SQL);
                        }
                        break;
                }
            }
            catch { }
            return dsRet;
        }

        public static JObject JObjectDS(string SQL, int Pagina = 0, int Itens = 0)
        {
            JObject _DsJson = new JObject();
            DataSet Ret_DS = new DataSet();
            string sDados = "";
            string sjReturn = "";
            string Status = "";
            string Descricao = "";

            try
            {
                Ret_DS = getDataSet(SQL, Pagina, Itens);

                try
                {
                    sDados = JsonConvert.SerializeObject(Ret_DS, Formatting.Indented);
                    sDados = sDados.Replace("\r\n", "");
                    sDados = sDados.Trim();
                    sDados = sDados.Substring(1, (sDados.Length - 2));
                    Status = "000";
                    Descricao = "Requisição efetuada com sucesso.";
                }
                catch (Exception ex)
                {
                    Status = "999";
                    Descricao = "Erro inesperado.";
                }

                sjReturn = "";
                sjReturn += "{ ";
                sjReturn += "\"Retorno\": { ";
                sjReturn += "\"Status\": \"" + Status + "\", ";
                sjReturn += "\"Descricao\": \"" + Descricao + "\" ";
                if (sDados != "")
                {
                    sjReturn += "}, ";
                    sjReturn += sDados;
                }
                else
                    sjReturn += "} ";
                sjReturn += "} ";

                _DsJson = JObject.Parse(sjReturn);
            }
            catch (Exception ex) { }
            return _DsJson;
        }

        //===================================================================================================================
    }

    public class ConfigInfo
    {
        //----------------------------------------------------
        public string appUrl { get; set; }
        public string appDirectory { get; set; }
        public string projectAmbient { get; set; }
        public string projectPathBin { get; set; }
        public string projectPathRoot { get; set; }
        public string DefaultConnection { get; set; }
        //----------------------------------------------------
        public string Api_Url_Base { get; set; }
        public string Api_Key { get; set; }
        //----------------------------------------------------
        public string Code { get; set; }
        public string Description { get; set; }
        //----------------------------------------------------
    }

    public class ObjectRequest
    {
        public string RequestType { get; set; }
        public string RequestObject { get; set; }
    }

}
