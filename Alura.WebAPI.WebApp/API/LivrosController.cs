﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Mvc;

namespace Alura.WebAPI.WebApp.API
{
    public class LivrosController: Controller
    {
        private readonly IRepository<Livro> _repo;

        public LivrosController(IRepository<Livro> repository)
        {
            _repo = repository;
        }
        [HttpGet]
        public IActionResult Recuperar(int id)
        {
            var model = _repo.Find(id);
            if(model == null)
            {
                return NotFound();
            }

            return Json(model.ToModel());
        }
        
        [HttpPost]
        public IActionResult Incluir(LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                _repo.Incluir(livro);
                var uri = Url.Action("Recuperar", new { id = livro.Id });
                return Created(uri,livro);
            }
            return BadRequest();

        }

        public IActionResult Deletar(int id)
        {
            var livro = _repo.Find(id);
            if(livro == null)
            {
                return NotFound();
            }
            _repo.Excluir(livro);
            return 0;
        }
    }
}