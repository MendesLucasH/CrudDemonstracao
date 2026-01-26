using CrudDemonstracao.Data;
using CrudDemonstracao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using static CrudDemonstracao.Data.Db;

namespace CrudDemonstracao.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        public List<Cliente> ListaClientes { get; set; } = new List<Cliente>();
        public List<string> Estados { get; } = new List<string>
{
    "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG",
    "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
};

        [BindProperty]
        public Cliente NovoCliente { get; set; } = new Cliente();

        public void OnGet()
        {
            ListaClientes = Db.ListarClientes();
        }
        public IActionResult OnPostSalvarCliente()
        {
            limparmascaras();

            // Validação de CPF/CNPJ Real
            if (!ValidacaoUtils.IsValid(NovoCliente.CpfCnpj))
            {
                TempData["MensagemErro"] = "O documento (CPF/CNPJ) digitado é inválido!";
                return RedirectToPage();
            }

            if (NovoCliente.Id > 0)
            {
                try
                {
                    //verifica se o ID é maior que 0 e se for quando eu for editar o cliente ele manda o dropdown (PF ou PJ) com os valores deles mesmo, sem isso ele mandaria nULO
                    var clienteNoBanco = Db.ObterClientePorId(NovoCliente.Id);
                    NovoCliente.TipoPessoa = clienteNoBanco.TipoPessoa;
                    Db.AtualizarCliente(NovoCliente);
                    TempData["MensagemSucesso"] = "Cliente atualizado com sucesso!";
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = "Erro ao atualizar: " + ex.Message;
                }
            }
            else
            {
                if (Db.ExisteClienteComCpf(NovoCliente.CpfCnpj))
                {
                    TempData["MensagemErro"] = "Este CPF/CNPJ já está cadastrado!";
                    return RedirectToPage();
                }

                Db.InsertCliente(NovoCliente);
                TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostExcluirCliente(int id)
        {
            try
            {
                Db.ExcluirCliente(id);
                TempData["MensagemSucesso"] = "Cliente removido com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao excluir: " + ex.Message;
            }
            return RedirectToPage();
        }

        private void limparmascaras()
        {
            //função para limpar as mascaras de cpf, cpf, e telefone para enviar pro banco sem os pontos e traços
            if (!string.IsNullOrEmpty(NovoCliente.CpfCnpj))
                NovoCliente.CpfCnpj = NovoCliente.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (!string.IsNullOrEmpty(NovoCliente.Telefone))
                NovoCliente.Telefone = NovoCliente.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            if (!string.IsNullOrEmpty(NovoCliente.Cep))
                NovoCliente.Cep = NovoCliente.Cep.Replace("-", "");
        }

        public IActionResult OnPostCancelar()
        {
            //quando eu clico no botão de cancelar, ou no X ele limpa os campos
            NovoCliente = new Cliente();
            ModelState.Clear();
            return RedirectToPage();
        }

        public void OnPostCarregarParaEdicao(int id)
        {
            //para carregar o modeal e trazer os campos preenchidos
            NovoCliente = Db.ObterClientePorId(id);
            TempData["AbrirModalEdicao"] = "true";
        }
    }
}