﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    public class ApiDocumentacaoController : Controller
    {
        [HttpGet]
        //[Authorize]
        [Route("Api/Documentação/Usuário")]
        public IActionResult DocApiUsuario()
        {
            ViewData["Mstr_Layout"] = "_Layout_Docmentacao";
            return View();
        }

        [HttpGet]
        //[Authorize]
        [Route("Api/Documentação/Usuário/Cadastar")]
        public IActionResult DocApiUsuarioCadastar()
        {
            ViewData["Mstr_Layout"] = "_Layout_Docmentacao";
            return View();
        }

        [HttpGet]
        //[Authorize]
        [Route("Api/Documentação/Usuário/Atualizar")]
        public IActionResult DocApiUsuarioAtualizar()
        {
            ViewData["Mstr_Layout"] = "_Layout_Docmentacao";
            return View();
        }

        [HttpGet]
        //[Authorize]
        [Route("Api/Documentação/Usuário/Listar")]
        public IActionResult DocApiUsuarioListar()
        {
            ViewData["Mstr_Layout"] = "_Layout_Docmentacao";
            return View();
        }

        [HttpGet]
        //[Authorize]
        [Route("Api/Documentação/Produto")]
        public IActionResult DocApiProduto()
        {
            ViewData["Mstr_Layout"] = "_Layout_Docmentacao";
            return View();
        }

        [HttpGet]
        //[Authorize]
        [Route("Api/Documentação/Produto/Cadastar")]
        public IActionResult DocApiProdutoCadastar()
        {
            ViewData["Mstr_Layout"] = "_Layout_Docmentacao";
            return View();
        }

        [HttpGet]
        //[Authorize]
        [Route("Api/Documentação/Produto/Atualizar")]
        public IActionResult DocApiProdutoAtualizar()
        {
            ViewData["Mstr_Layout"] = "_Layout_Docmentacao";
            return View();
        }

        [HttpGet]
        //[Authorize]
        [Route("Api/Documentação/Produto/Listar")]
        public IActionResult DocApiProdutoListar()
        {
            ViewData["Mstr_Layout"] = "_Layout_Docmentacao";
            return View();
        }
    }
}
