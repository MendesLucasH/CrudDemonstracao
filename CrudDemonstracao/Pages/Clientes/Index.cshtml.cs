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
            if (!ModelState.IsValid)
            {
                ListaClientes = Db.ListarClientes();
                return Page();
            }
            LimparMascaras();            
            if (Db.ExisteClienteComCpf(NovoCliente.CpfCnpj))
            {                
                TempData["MensagemErro"] = "Este CPF/CNPJ já está cadastrado no sistema!";
                ListaClientes = Db.ListarClientes();
                return Page(); 
            }
            try
            {
                Db.InsertCliente(NovoCliente);                
                TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao salvar: " + ex.Message;
                ListaClientes = Db.ListarClientes();
                return Page();
            }
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
        private void LimparMascaras()
        {
            if (!string.IsNullOrEmpty(NovoCliente.CpfCnpj))
                NovoCliente.CpfCnpj = NovoCliente.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (!string.IsNullOrEmpty(NovoCliente.Telefone))
                NovoCliente.Telefone = NovoCliente.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            if (!string.IsNullOrEmpty(NovoCliente.Cep))
                NovoCliente.Cep = NovoCliente.Cep.Replace("-", "");
        }
        public IActionResult OnPostCancelar()
        {           
            NovoCliente = new Cliente();            
            ModelState.Clear();            
            return RedirectToPage();
        }
    }
}