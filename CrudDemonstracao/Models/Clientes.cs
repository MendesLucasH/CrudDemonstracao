using System;
using System.ComponentModel.DataAnnotations;

namespace CrudDemonstracao.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; }
        public string TipoPessoa { get; set; } 
        [Required(ErrorMessage = "O CPF/CNPJ é obrigatório.")]
        [StringLength(18)]
        public string CpfCnpj { get; set; }
        [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }
        public string? Cidade { get; set; }
        [StringLength(2, ErrorMessage = "Use a sigla (ex: SP).")]
        public string? Estado { get; set; }
        public string? Cep { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}