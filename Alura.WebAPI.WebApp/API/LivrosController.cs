﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Mvc;

namespace Alura.WebAPI.WebApp.API
{
    [ApiController]
    [Route("[controller]")]
    public class LivrosController: ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public LivrosController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IActionResult ListaDeLivros()
        {
            var lista = _repo.All.Select(l => l.ToModel()).ToList();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Recuperar(int id)
        {
            var model = _repo.Find(id);
            if(model == null)
            {
                return NotFound();
            }

            return Ok(model.ToModel());
        }
        
        [HttpPost]
        public IActionResult Incluir([FromBody]LivroUpload model)
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
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var livro = _repo.Find(id);
            if(livro == null)
            {
                return NotFound();
            }
            _repo.Excluir(livro);
            return NoContent();
        }
        [HttpPut]
        public IActionResult Alterar([FromBody]LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if (model.Capa == null)
                {
                    livro.ImagemCapa = _repo.All
                        .Where(l => l.Id == livro.Id)
                        .Select(l => l.ImagemCapa)
                        .FirstOrDefault();
                }
                _repo.Alterar(livro);
                return Ok();
            }
            return BadRequest();
        }
    }
}
