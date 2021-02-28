using Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Application
{
    public static class Proc
    {
        private static string _Id = "0";
        private static string _SQL = "";
        private static string _Status = "";
        private static string _Descricao = "";
        private static JObject _DsJson = new JObject();

        //===================================================================================================================

        public static JObject ProdutoCadastrar(TB_Produtos obj = null)
        {
            obj.DataCadastro = Util.FormatarData("", "UST");
            obj.CriadoPor = Util.ValidarEntrada(obj.CriadoPor, 20, "");
            obj.Nome = Util.ValidarEntrada(obj.Nome, 250, "");
            obj.CodigoDeBarras = Util.ValidarEntrada(obj.CodigoDeBarras, 100, "");
            obj.Descricao = Util.ValidarEntrada(obj.Descricao, 10000, "");
            obj.Preco = Util.FormatarValor(obj.Preco, ".", 2);
            obj.Status = Util.ValidarEntrada(obj.Status, 1, "F");

            _SQL = "";
            _SQL += "EXEC SP_ProdutoCadastrar ";
            _SQL += "'" + obj.DataCadastro + "', ";
            _SQL += "'" + obj.CriadoPor + "', ";
            _SQL += "'" + obj.Nome + "', ";
            _SQL += "'" + obj.CodigoDeBarras + "', ";
            _SQL += "'" + obj.Descricao + "', ";
            _SQL += "'" + obj.Preco + "', ";
            _SQL += "'" + obj.Status + "' ";
            _DsJson = Util.JObjectDS("BD_002", _SQL);
            try { _Id = _DsJson["Dados"][0]["Id"].ToString(); } catch { _Id = "0"; }
            try { _Status = _DsJson["Dados"][0]["Status"].ToString(); } catch { _Status = ""; }
            try { _Descricao = _DsJson["Dados"][0]["Descricao"].ToString(); } catch { _Descricao = ""; }

            string sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"Retorno\": { ";
            sjReturn += "\"Id\": \"" + _Id + "\", ";
            sjReturn += "\"Status\": \"" + _Status + "\", ";
            sjReturn += "\"Descricao\": \"" + _Descricao + "\" ";
            sjReturn += "}} ";
            return JObject.Parse(sjReturn);
        }

        //===================================================================================================================

        public static JObject ProdutoAtualizar(TB_Produtos obj = null)
        {
            obj.UltimaModificacaoPor = Util.ValidarEntrada(obj.UltimaModificacaoPor, 20, "");
            obj.UltimaModificacao = Util.FormatarData("", "US");
            obj.Nome = Util.ValidarEntrada(obj.Nome, 250, "");
            obj.Descricao = Util.ValidarEntrada(obj.Descricao, 10000, "");
            obj.Preco = Util.FormatarValor(obj.Preco, ".", 2);
            obj.Status = Util.ValidarEntrada(obj.Status, 1, "F");

            _SQL = "";
            _SQL += "EXEC SP_ProdutoAtualizar ";
            _SQL += "'" + Util.SomenteNumeros(obj.Id.ToString()) + "', ";
            _SQL += "'" + obj.UltimaModificacaoPor + "', ";
            _SQL += "'" + obj.UltimaModificacao + "', ";
            _SQL += "'" + obj.Nome + "', ";
            _SQL += "'" + obj.Descricao + "', ";
            _SQL += "'" + obj.Preco + "', ";
            _SQL += "'" + obj.Status + "' ";
            _DsJson = Util.JObjectDS("BD_002", _SQL);
            try { _Id = _DsJson["Dados"][0]["Id"].ToString(); } catch { _Id = "0"; }
            try { _Status = _DsJson["Dados"][0]["Status"].ToString(); } catch { _Status = ""; }
            try { _Descricao = _DsJson["Dados"][0]["Descricao"].ToString(); } catch { _Descricao = ""; }

            string sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"Retorno\": { ";
            sjReturn += "\"Id\": \"" + _Id + "\", ";
            sjReturn += "\"Status\": \"" + _Status + "\", ";
            sjReturn += "\"Descricao\": \"" + _Descricao + "\" ";
            sjReturn += "}} ";
            return JObject.Parse(sjReturn);
        }

        //===================================================================================================================

        public static JObject ProdutoListar(TB_Produtos obj = null)
        {
            _Id = "0";
            try { _Id = Util.SomenteNumeros(obj.Id.ToString()); } catch { }
            obj.UltimaModificacaoPor = Util.ValidarEntrada(obj.UltimaModificacaoPor, 20, "");
            obj.Nome = Util.ValidarEntrada(obj.Nome, 250, "");
            obj.Descricao = Util.ValidarEntrada(obj.Descricao, 10000, "");
            obj.Preco = Util.FormatarValor(obj.Preco, ".", 2);
            obj.Status = Util.ValidarEntrada(obj.Status, 1, "A");

            _SQL = "";
            _SQL += "EXEC SP_ProdutoListar ";
            _SQL += "'" + _Id + "', ";
            _SQL += "'" + obj.UltimaModificacaoPor + "', ";
            _SQL += "'" + obj.Nome + "', ";
            _SQL += "'" + obj.Descricao + "', ";
            _SQL += "'" + obj.Preco + "', ";
            _SQL += "'" + obj.Status + "' ";
            _DsJson = Util.JObjectDS("BD_002", _SQL);
            try { _Status = _DsJson["Retorno"]["Status"].ToString(); } catch { _Status = ""; }
            try { _Descricao = _DsJson["Retorno"]["Descricao"].ToString(); } catch { _Descricao = ""; }

            string sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"Retorno\": { ";
            sjReturn += "\"Status\": \"" + _Status + "\", ";
            sjReturn += "\"Descricao\": \"" + _Descricao + "\" ";
            try { sjReturn += "}, \"Dados\": " + _DsJson["Dados"].ToString() + " } "; } catch { sjReturn += "}} "; }
            return JObject.Parse(sjReturn);
        }

        //===================================================================================================================

        public static JObject UsuarioCadastrar(TB_Usuario obj = null)
        {
            obj.DataCadastro = Util.FormatarData("", "UST");
            obj.DataNascimento = Util.FormatarData(obj.DataNascimento, "US");
            obj.Nome = Util.ValidarEntrada(obj.Nome, 50, "");
            obj.SobreNome = Util.ValidarEntrada(obj.SobreNome, 20, "");
            obj.Email = Util.ValidarEntrada(obj.Email, 50, "");
            obj.Telefone = Util.ValidarEntrada(obj.Telefone, 20, "");
            obj.Perfil = Util.ValidarEntrada(obj.Perfil, 20, "Cliente");
            obj.Password = Util.Encrypt(obj.Password);
            obj.Status = Util.ValidarEntrada(obj.Status, 1, "N");

            _SQL = "";
            _SQL += "EXEC SP_UsuarioCadastrar ";
            _SQL += "'" + obj.DataCadastro + "', ";
            _SQL += "'" + obj.DataNascimento + "', ";
            _SQL += "'" + obj.Nome + "', ";
            _SQL += "'" + obj.SobreNome + "', ";
            _SQL += "'" + obj.Email + "', ";
            _SQL += "'" + obj.Telefone + "', ";
            _SQL += "'" + obj.Perfil + "', ";
            _SQL += "'" + obj.Password + "', ";
            _SQL += "'" + obj.Status + "' ";
            _DsJson = Util.JObjectDS("BD_001", _SQL);
            try { _Id = _DsJson["Dados"][0]["Id"].ToString(); } catch { _Id = "0"; }
            try { _Status = _DsJson["Dados"][0]["Status"].ToString(); } catch { _Status = ""; }
            try { _Descricao = _DsJson["Dados"][0]["Descricao"].ToString(); } catch { _Descricao = ""; }

            string sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"Retorno\": { ";
            sjReturn += "\"Id\": \"" + _Id + "\", ";
            sjReturn += "\"Status\": \"" + _Status + "\", ";
            sjReturn += "\"Descricao\": \"" + _Descricao + "\" ";
            sjReturn += "}} ";
            return JObject.Parse(sjReturn);
        }

        //===================================================================================================================

        public static JObject UsuarioAtualizar(TB_Usuario obj = null)
        {
            obj.DataNascimento = Util.FormatarData(obj.DataNascimento, "US");
            obj.Nome = Util.ValidarEntrada(obj.Nome, 50, "");
            obj.SobreNome = Util.ValidarEntrada(obj.SobreNome, 20, "");
            obj.Email = Util.ValidarEntrada(obj.Email, 50, "");
            obj.Telefone = Util.ValidarEntrada(obj.Telefone, 20, "");
            obj.Perfil = Util.ValidarEntrada(obj.Perfil, 20, "");
            obj.Status = Util.ValidarEntrada(obj.Status, 1, "N");

            _SQL = "";
            _SQL += "EXEC SP_UsuarioAtualizar ";
            _SQL += "'" + Util.SomenteNumeros(obj.Id.ToString()) + "', ";
            _SQL += "'" + obj.DataNascimento + "', ";
            _SQL += "'" + obj.Nome + "', ";
            _SQL += "'" + obj.SobreNome + "', ";
            _SQL += "'" + obj.Email + "', ";
            _SQL += "'" + obj.Telefone + "', ";
            _SQL += "'" + obj.Perfil + "', ";
            _SQL += "'" + obj.Status + "' ";
            _DsJson = Util.JObjectDS("BD_001", _SQL);
            try { _Id = _DsJson["Dados"][0]["Id"].ToString(); } catch { _Id = "0"; }
            try { _Status = _DsJson["Dados"][0]["Status"].ToString(); } catch { _Status = ""; }
            try { _Descricao = _DsJson["Dados"][0]["Descricao"].ToString(); } catch { _Descricao = ""; }

            string sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"Retorno\": { ";
            sjReturn += "\"Id\": \"" + _Id + "\", ";
            sjReturn += "\"Status\": \"" + _Status + "\", ";
            sjReturn += "\"Descricao\": \"" + _Descricao + "\" ";
            sjReturn += "}} ";
            return JObject.Parse(sjReturn);
        }

        //===================================================================================================================

        public static JObject UsuarioListar(TB_Usuario obj = null)
        {
            _Id = "0";
            try { _Id = Util.SomenteNumeros(obj.Id.ToString()); } catch { }
            obj.Nome = Util.ValidarEntrada(obj.Nome, 50, "");
            obj.SobreNome = Util.ValidarEntrada(obj.SobreNome, 20, "");
            obj.Email = Util.ValidarEntrada(obj.Email, 50, "");
            obj.Telefone = Util.ValidarEntrada(obj.Telefone, 20, "");
            obj.Perfil = Util.ValidarEntrada(obj.Perfil, 20, "");
            obj.Status = Util.ValidarEntrada(obj.Status, 1, "A");

            _SQL = "";
            _SQL += "EXEC SP_UsuarioListar ";
            _SQL += "'" + _Id + "', ";
            _SQL += "'" + obj.Nome + "', ";
            _SQL += "'" + obj.SobreNome + "', ";
            _SQL += "'" + obj.Telefone + "', ";
            _SQL += "'" + obj.Perfil + "', ";
            _SQL += "'" + obj.Status + "' ";
            _DsJson = Util.JObjectDS("BD_001", _SQL);
            try { _Status = _DsJson["Retorno"]["Status"].ToString(); } catch { _Status = ""; }
            try { _Descricao = _DsJson["Retorno"]["Descricao"].ToString(); } catch { _Descricao = ""; }

            string sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"Retorno\": { ";
            sjReturn += "\"Status\": \"" + _Status + "\", ";
            sjReturn += "\"Descricao\": \"" + _Descricao + "\" ";
            try { sjReturn += "}, \"Dados\": " + _DsJson["Dados"].ToString() + " } "; } catch { sjReturn += "}} "; }
            return JObject.Parse(sjReturn);
        }

        //===================================================================================================================

        public static JObject UsuarioLogin(TB_Usuario obj = null)
        {
            _Id = "0";
            obj.Email = Util.ValidarEntrada(obj.Email, 50, "");
            obj.Password = Util.Encrypt(obj.Password);

            _SQL = "";
            _SQL += "EXEC SP_UsuarioLogin ";
            _SQL += "'" + obj.Email + "', ";
            _SQL += "'" + obj.Password + "' ";
            _DsJson = Util.JObjectDS("BD_001", _SQL);
            try { _Status = _DsJson["Retorno"]["Status"].ToString(); } catch { _Status = ""; }
            try { _Descricao = _DsJson["Retorno"]["Descricao"].ToString(); } catch { _Descricao = ""; }

            string sjReturn = "";
            sjReturn += "{ ";
            sjReturn += "\"Retorno\": { ";
            sjReturn += "\"Status\": \"" + _Status + "\", ";
            sjReturn += "\"Descricao\": \"" + _Descricao + "\" ";
            try { sjReturn += "}, \"Dados\": " + _DsJson["Dados"].ToString() + " } "; } catch { sjReturn += "}} "; }
            return JObject.Parse(sjReturn);
        }

        //===================================================================================================================
    }
}
