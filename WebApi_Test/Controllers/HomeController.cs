﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Security.Claims;
using System;

namespace WebApi_Test.Controllers
{
    public class HomeController : Controller
    {
        public AppUser CurrentUser { get { return new AppUser(this.User as ClaimsPrincipal); } }

        [HttpGet]
        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Cadastro")]
        public IActionResult Cadastro()
        {
            ViewData["Status"] = "001";
            return View();
        }

        // Cadastro na TB_Usuario, através de uma requisição ao API tratado com método de POST
        // Cadastro na TB_Usuario, através de uma requisição ao API tratado com método de POST
        // Cadastro na TB_Usuario, através de uma requisição ao API tratado com método de POST

        [HttpPost]
        [Route("Cadastro")]
        public IActionResult Cadastro(Usuario obj)
        {
            ConfigInfo oInfor = Util.ProjectInfo();
            ViewBag.Dados = null;
            try
            {
                if ((obj.Nome != null) && (obj.Email != null) && (obj.Password != null))
                {
                    string _Status = "", _Descricao = "", jsonSTRINGResult = JsonConvert.SerializeObject(obj);
                    JObject jObj = Util.FormatarObjeto("CADASTRAR", jsonSTRINGResult);
                    jObj = Util.GetRequest("POST", oInfor.Api_Url_Base + "Usuario", "Authorization", oInfor.Api_Key, jObj);
                    ViewBag.Dados = jObj["Dados"];
                    _Status = jObj["Dados"][0]["Status"].ToString();
                    _Descricao = jObj["Dados"][0]["Descricao"].ToString();
                    if (_Status == "000")
                    {
                        ViewData["Status"] = _Status;
                        ViewData["Card_Title"] = "Parabéns!";
                        ViewData["Card_Text"] = _Descricao;
                    }
                    else
                    {
                        ViewData["Status"] = "901";
                        ViewData["Descricao"] = _Descricao;
                        ViewData["Card_Title"] = "Ops!";
                        ViewData["Card_Text"] = _Descricao;
                    }
                }
            }
            catch { }
            return View("_View_Alert");
        }

        [HttpGet]
        [Route("Acessar/{Id=1}")]
        [Route("Entrar/{Id=2}")]
        [Route("Login/{Id=3}")]
        public IActionResult LogIn(string Id)
        {
            ViewData["Status"] = "001";
            switch (Id)
            {
                case "1": ViewData["Title"] = "Acessar"; break;
                case "2": ViewData["Title"] = "Entrar"; break;
                case "3": ViewData["Title"] = "Login"; break;
            }
            return View();
        }

        // LogIn na TB_Usuario_Login com TB_Usuario, através de uma Procedure
        // LogIn na TB_Usuario_Login com TB_Usuario, através de uma Procedure
        // LogIn na TB_Usuario_Login com TB_Usuario, através de uma Procedure

        [HttpPost]
        [Route("Acessar/{Id=1}")]
        [Route("Entrar/{Id=2}")]
        [Route("Login/{Id=3}")]
        public IActionResult LogIn(string Id, Usuario obj)
        {
            ConfigInfo oInfor = Util.ProjectInfo();
            ViewBag.Dados = null;
            string _IdUsuario = "0", _Descricao = "";
            switch (Id)
            {
                case "1": ViewData["Title"] = "Acessar"; break;
                case "2": ViewData["Title"] = "Entrar"; break;
                case "3": ViewData["Title"] = "Login"; break;
            }
            try
            {
                JObject jObj = Proc.UsuarioLogin(obj);
                ViewBag.Dados = jObj["Dados"];
                Thread.Sleep(300);
                try { _IdUsuario = jObj["Dados"][0]["Id"].ToString(); } catch { _IdUsuario = "0"; }
                try { _Descricao = jObj["Dados"][0]["Descricao"].ToString(); } catch { _Descricao = ""; }

                if (_IdUsuario != "0")
                {
                    var claims = new[]
                    {
                        new Claim("Key", Util.Encrypt(_IdUsuario)),
                        new Claim("Perfil", Util.Encrypt(jObj["Dados"][0]["Perfil"].ToString())),
                        new Claim("Nome", Util.Encrypt(jObj["Dados"][0]["SobreNome"].ToString())),
                        new Claim("Email", Util.Encrypt(jObj["Dados"][0]["Email"].ToString())),
                        new Claim("Status", Util.Encrypt(jObj["Dados"][0]["Status"].ToString()))
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                        IsPersistent = true,
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                }
            }
            catch { }
            if (_IdUsuario != "0")
            {
                return Redirect("Minha-Conta");
            }
            else
            {
                ViewData["Card_Title"] = "Ops!";
                ViewData["Card_Text"] = _Descricao;
                return View("_View_Alert");
            }
        }

        // Listagem na TB_Produtos, através de uma requisição ao API tratado com método de POST
        // Listagem na TB_Produtos, através de uma requisição ao API tratado com método de POST
        // Listagem na TB_Produtos, através de uma requisição ao API tratado com método de POST

        [HttpGet]
        [HttpPost]
        [Route("Produtos")]
        public IActionResult Produtos(Produto obj)
        {
            ConfigInfo oInfor = Util.ProjectInfo();
            ViewBag.Dados = null;
            ViewData["Perfil"] = null;
            try { ViewData["Perfil"] = CurrentUser.Perfil; } catch { ViewData["Perfil"] = "Cliente"; }
            try
            {
                string jsonSTRINGResult = JsonConvert.SerializeObject(obj);
                JObject jObj = Util.FormatarObjeto("LISTAR", jsonSTRINGResult);
                jObj = Util.GetRequest("POST", oInfor.Api_Url_Base + "Produto", "Authorization", oInfor.Api_Key, jObj);
                ViewBag.Dados = jObj["Dados"];
            }
            catch { }
            return View();
        }

        // Cadastrar na TB_Produtos, através de uma requisição ao API tratado com método de POST
        // Cadastrar na TB_Produtos, através de uma requisição ao API tratado com método de POST
        // Cadastrar na TB_Produtos, através de uma requisição ao API tratado com método de POST

        [HttpGet]
        [Authorize]
        [Route("Cadastrar-Produto")]
        public IActionResult CadastrarProduto()
        {
            Produto obj = new Produto();
            return View(obj);
        }

        [HttpPost]
        [Authorize]
        [Route("Cadastrar-Produto")]
        public IActionResult CadastrarProduto(Produto obj)
        {
            ConfigInfo oInfor = Util.ProjectInfo();
            ViewBag.Dados = null;
            try { obj.CriadoPor = CurrentUser.Nome; } catch { }
            try
            {
                if ((obj.CodigoDeBarras != null) && (obj.Nome != null) && (obj.Descricao != null) && (obj.Preco != null))
                {
                    string _Status = "", _Descricao = "", jsonSTRINGResult = JsonConvert.SerializeObject(obj);
                    JObject jObj = Util.FormatarObjeto("CADASTRAR", jsonSTRINGResult);
                    jObj = Util.GetRequest("POST", oInfor.Api_Url_Base + "Produto", "Authorization", oInfor.Api_Key, jObj);
                    _Status = jObj["Retorno"]["Status"].ToString();
                    _Descricao = jObj["Retorno"]["Descricao"].ToString();
                    if (_Status == "000")
                    {
                        ViewData["Status"] = _Status;
                        ViewData["Card_Title"] = "Parabéns!";
                        ViewData["Card_Text"] = _Descricao;
                    }
                    else
                    {
                        ViewData["Status"] = "901";
                        ViewData["Descricao"] = _Descricao;
                        ViewData["Card_Title"] = "Ops!";
                        ViewData["Card_Text"] = _Descricao;
                    }
                }
            }
            catch { }
            return View("_View_Alert");
        }

        // Atualizar na TB_Produtos, através de uma requisição ao API tratado com método de POST
        // Atualizar na TB_Produtos, através de uma requisição ao API tratado com método de POST
        // Atualizar na TB_Produtos, através de uma requisição ao API tratado com método de POST

        [HttpGet]
        [Authorize]
        [Route("Atualizar-Produto/{Id?}")]
        public IActionResult AtualizarProduto(string Id)
        {
            ConfigInfo oInfor = Util.ProjectInfo();
            Produto obj = new Produto();
            ViewBag.Dados = null;
            try { obj.Id = Convert.ToInt32(Util.Descrypt(Id)); } catch { }
            try
            {
                if (obj.Id > 0)
                {
                    string jsonSTRINGResult = JsonConvert.SerializeObject(obj);
                    JObject jObj = Util.FormatarObjeto("LISTAR", jsonSTRINGResult);
                    jObj = Util.GetRequest("POST", oInfor.Api_Url_Base + "Produto", "Authorization", oInfor.Api_Key, jObj);
                    ViewBag.Dados = jObj["Dados"];
                    try { obj = JsonConvert.DeserializeObject<Produto>(jObj["Dados"][0].ToString()); } catch { }
                }
            }
            catch { }
            return View(obj);
        }

        [HttpPost]
        [Authorize]
        [Route("Atualizar-Produto")]
        public IActionResult AtualizarProduto(Produto obj)
        {
            ConfigInfo oInfor = Util.ProjectInfo();
            ViewBag.Dados = null;
            try { obj.UltimaModificacaoPor = CurrentUser.Nome; } catch { }
            try
            {
                if ((obj.CodigoDeBarras != null) && (obj.Nome != null) && (obj.Descricao != null) && (obj.Preco != null))
                {
                    string _Status = "", _Descricao = "", jsonSTRINGResult = JsonConvert.SerializeObject(obj);
                    JObject jObj = Util.FormatarObjeto("ATUALIZAR", jsonSTRINGResult);
                    jObj = Util.GetRequest("POST", oInfor.Api_Url_Base + "Produto", "Authorization", oInfor.Api_Key, jObj);
                    _Status = jObj["Retorno"]["Status"].ToString();
                    _Descricao = jObj["Retorno"]["Descricao"].ToString();
                    if (_Status == "000")
                    {
                        ViewData["Status"] = _Status;
                        ViewData["Card_Title"] = "Parabéns!";
                        ViewData["Card_Text"] = _Descricao;
                    }
                    else
                    {
                        ViewData["Status"] = "901";
                        ViewData["Descricao"] = _Descricao;
                        ViewData["Card_Title"] = "Ops!";
                        ViewData["Card_Text"] = _Descricao;
                    }
                }
            }
            catch { }
            return View("_View_Alert");
        }

        [HttpGet]
        [HttpPost]
        [Authorize]
        [Route("Minha-Conta")]
        public IActionResult MinhaConta()
        {
            return View();
        }

        // 1ª parte Listar dados
        // Atualizar dados na TB_Usuario, através de uma requisição ao API tratado com método de POST
        // Atualizar dados na TB_Usuario, através de uma requisição ao API tratado com método de POST
        // Atualizar dados na TB_Usuario, através de uma requisição ao API tratado com método de POST

        [HttpGet]
        [Authorize]
        [Route("Meus-Dados")]
        public IActionResult MeusDados()
        {
            ConfigInfo oInfor = Util.ProjectInfo();
            Usuario obj = new Usuario();
            ViewBag.Dados = null;
            try { obj.Id = CurrentUser.Key; } catch { }
            try { obj.Status = CurrentUser.Status; } catch { }
            try
            {
                if (obj.Id > 0)
                {
                    string jsonSTRINGResult = JsonConvert.SerializeObject(obj);
                    JObject jObj = Util.FormatarObjeto("LISTAR", jsonSTRINGResult);
                    jObj = Util.GetRequest("POST", oInfor.Api_Url_Base + "Usuario", "Authorization", oInfor.Api_Key, jObj);
                    ViewBag.Dados = jObj["Dados"];
                    try { obj = JsonConvert.DeserializeObject<Usuario>(jObj["Dados"][0].ToString()); } catch { }
                }
            }
            catch { }
            return View(obj);
        }

        // 2ª parte
        // Atualizar dados na TB_Usuario, através de uma requisição ao API tratado com método de POST
        // Atualizar dados na TB_Usuario, através de uma requisição ao API tratado com método de POST
        // Atualizar dados na TB_Usuario, através de uma requisição ao API tratado com método de POST

        [HttpPost]
        [Authorize]
        [Route("Meus-Dados")]
        public IActionResult MeusDados(Usuario obj)
        {
            ConfigInfo oInfor = Util.ProjectInfo();
            ViewBag.Dados = null;
            try { obj.Id = CurrentUser.Key; } catch { }
            try { obj.Perfil = CurrentUser.Perfil; } catch { }
            try { obj.Status = CurrentUser.Status; } catch { }
            string _Status = "", _Descricao = "", jsonSTRINGResult = JsonConvert.SerializeObject(obj);
            try
            {
                if (obj.Id > 0)
                {
                    JObject jObj = Util.FormatarObjeto("ATUALIZAR", jsonSTRINGResult);
                    jObj = Util.GetRequest("POST", oInfor.Api_Url_Base + "Usuario", "Authorization", oInfor.Api_Key, jObj);
                    _Status = jObj["Retorno"]["Status"].ToString();
                    _Descricao = jObj["Retorno"]["Descricao"].ToString();
                    if (_Status == "000")
                    {
                        ViewData["Status"] = _Status;
                        ViewData["Card_Title"] = "Parabéns!";
                        ViewData["Card_Text"] = _Descricao;
                    }
                    else
                    {
                        ViewData["Status"] = "901";
                        ViewData["Descricao"] = _Descricao;
                        ViewData["Card_Title"] = "Ops!";
                        ViewData["Card_Text"] = _Descricao;
                    }
                }
            }
            catch { }
            return View("_View_Alert");
        }

        [HttpGet]
        [HttpPost]
        [Authorize]
        [Route("Sair")]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("Início");
        }
    }
}
