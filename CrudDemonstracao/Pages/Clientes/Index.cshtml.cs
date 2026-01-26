using CrudDemonstracao.Models;
using CrudDemonstracao.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrudDemonstracao.Pages.Clientes
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;      
        public List<Cliente> ListaClientes { get; set; } = new List<Cliente>();
        [BindProperty]
        public Cliente NovoCliente { get; set; }
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;           
        }
        public void OnGet()
        {           
            ListaClientes = Db.ListarClientes();
        }
        public IActionResult OnPostSalvarCliente()
        {
            // Se o Id for maior que 0, quer dizer  "Editar"
            if (NovoCliente.Id > 0)
            {
                try
                {
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
                    TempData["MensagemErro"] = "Este CPF já está cadastrado!";
                    return RedirectToPage();
                }

                Db.InsertCliente(NovoCliente);
                TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";
            }

            return RedirectToPage();
        }
        public IActionResult OnPostExcluir(int id)
        {
            try
            {                
                Db.ExcluirCliente(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir: " + ex.Message);
                return RedirectToPage();
            }
        }      
        //private void LimparMascaras()
        //{
        //    if (!string.IsNullOrEmpty(NovoCliente.CpfCnpj))
        //        NovoCliente.CpfCnpj = NovoCliente.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
        //    if (!string.IsNullOrEmpty(NovoCliente.Telefone))
        //        NovoCliente.Telefone = NovoCliente.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
        //    if (!string.IsNullOrEmpty(NovoCliente.Cep))
        //        NovoCliente.Cep = NovoCliente.Cep.Replace("-", "");
        //}
        public IActionResult OnPostCancelar()
        {           
            NovoCliente = new Cliente();            
            ModelState.Clear();            
            return RedirectToPage();
        }       
        public IActionResult OnPostExcluirCliente(int id)
        {
            try
            {
                // CHAMA A FUNÇÃO QUE EU CRIEI NO DB.CS
                Db.ExcluirCliente(id);

               // MENSAGEM AMIGAVEL SWEETALERT
                TempData["MensagemSucesso"] = "Cliente removido com sucesso!";
            }
            catch (Exception ex)
            {
               //CASO ACONTECA ALGUM ERRINHO
                TempData["MensagemErro"] = "Erro ao excluir: " + ex.Message;
            }            
            return RedirectToPage();
        }

        public void OnPostCarregarParaEdicao(int id)
        {          
            NovoCliente = Db.ObterClientePorId(id);            
            TempData["AbrirModalEdicao"] = "true";
        }
    }
}