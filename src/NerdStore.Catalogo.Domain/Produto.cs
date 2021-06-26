﻿using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace NerdStore.Catalogo.Domain
{
    public class Produto : Entity, IAggregateRoot
    {
        public Guid CategoriaId { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public Dimensoes Dimensoes { get; private set; }
        public Categoria Categoria { get; private set; }

        protected Produto() { }
        public Produto(string nome, string descricao, bool ativo, decimal valor, Guid categoriaId, DateTime dataCadastro, string imagem, Dimensoes dimensoes)
        {
            CategoriaId = categoriaId;
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            DataCadastro = dataCadastro;
            Imagem = imagem;
            Dimensoes = dimensoes;

            Validar();
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        public void AlterarDescricao(string descricao)
        {
            Validacoes.ValidarSeVazio(descricao, $"O campo {nameof(Descricao)} do produto não pode estar vazio.");
            Descricao = descricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new DomainException("Estoque insuficiente.");
            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
        }

        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, $"O campo {nameof(Nome)} do produto não pode estar vazio.");
            Validacoes.ValidarSeVazio(Descricao, $"O campo {nameof(Descricao)} do produto não pode estar vazio.");
            Validacoes.ValidarSeIgual(CategoriaId, Guid.Empty, $"O campo {nameof(CategoriaId)} do produto não pode estar vazio.");
            Validacoes.ValidarSeMenorQue(Valor, 1, $"O campo {nameof(Valor)} do produto não pode se menor igual a zero.");
            Validacoes.ValidarSeVazio(Imagem, $"O campo {nameof(Imagem)} do produto não pode estar vazio.");
        }
    }

    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }

        //EF Relation
        public ICollection<Produto> Produtos { get; set; }

        protected Categoria() { }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;
            Validar();
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, $"O campo {nameof(Nome)} da categoria não pode estar vazio.");
            Validacoes.ValidarSeIgual(Codigo, 0, $"O campo {nameof(Codigo)} da categoria não pode ser igual a zero.");
            Validacoes.ValidarSeMenorQue(Codigo, 0, $"O campo {nameof(Codigo)} da categoria não pode ser menor que zero.");
        }
    }
}
