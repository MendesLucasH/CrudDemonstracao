using MySqlConnector;
using CrudDemonstracao.Models;
using System.Collections.Generic;
using System;

namespace CrudDemonstracao.Data
{
    public static class Db
    {
        public static string ConnectionString { get; set; } = "";
        //LISTAR PARA A GRID
        public static List<Cliente> ListarClientes()
        {
            var lista = new List<Cliente>();
            using (var conexao = new MySqlConnection(ConnectionString))
            {
                conexao.Open();
                string sql = "SELECT * FROM clientes ORDER BY id DESC";
                using (var comando = new MySqlCommand(sql, conexao))
                using (var leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        lista.Add(new Cliente
                        {
                            Id = leitor.GetInt32("id"),
                            Nome = leitor.GetString("nome"),
                            CpfCnpj = leitor.GetString("cpf_cnpj"),
                            Email = leitor.IsDBNull(leitor.GetOrdinal("email")) ? "" : leitor.GetString("email"),
                            TipoPessoa = leitor.GetString("tipo_pessoa")
                        });
                    }
                }
            }
            return lista;
        }
        // INSERIR CLIENTE
        public static void InsertCliente(Cliente c)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var sql = @"INSERT INTO clientes (nome, tipo_pessoa, cpf_cnpj, email, telefone, endereco, cidade, estado, cep, data_criacao) 
                            VALUES (@nome, @tipo, @cpf, @email, @tel, @end, @cid, @uf, @cep, NOW())";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", c.Nome);
                    cmd.Parameters.AddWithValue("@tipo", c.TipoPessoa);
                    cmd.Parameters.AddWithValue("@cpf", c.CpfCnpj);
                    cmd.Parameters.AddWithValue("@email", c.Email ?? "");
                    cmd.Parameters.AddWithValue("@tel", c.Telefone ?? "");
                    cmd.Parameters.AddWithValue("@end", c.Endereco ?? "");
                    cmd.Parameters.AddWithValue("@cid", c.Cidade ?? "");
                    cmd.Parameters.AddWithValue("@uf", c.Estado ?? "");
                    cmd.Parameters.AddWithValue("@cep", c.Cep ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // EXCLUIR CLIENTE
        public static void ExcluirCliente(int id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var sql = "DELETE FROM clientes WHERE id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //VERIFICAÇÃO PARA CASO QUEIRA CADASTRAR CLIENTE COM CPF DUPLICADO
        public static bool ExisteClienteComCpf(string cpf)
        {
            using (var conexao = new MySqlConnection(ConnectionString))
            {
                conexao.Open();
                // Verifica se existe algum registro com esse CPF
                string sql = "SELECT COUNT(*) FROM clientes WHERE cpf_cnpj = @cpf";

                using (var comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@cpf", cpf);


                    long quantidade = (long)comando.ExecuteScalar();

                    return quantidade > 0;
                }
            }
        }

        //SELECT DE CLIENTE POR ID PARA ATUALIZAR
        public static Cliente ObterClientePorId(int id)
        {
            var cliente = new Cliente();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM clientes WHERE id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente.Id = Convert.ToInt32(reader["id"]);
                            cliente.Nome = reader["nome"].ToString();
                            cliente.CpfCnpj = reader["cpf_cnpj"].ToString();
                            cliente.Email = reader["email"].ToString();
                            cliente.TipoPessoa = reader["tipo_pessoa"].ToString();
                            cliente.Telefone = reader["telefone"].ToString();
                            cliente.Endereco = reader["endereco"].ToString();
                            cliente.Cidade = reader["cidade"].ToString();
                            cliente.Estado = reader["estado"].ToString();
                            cliente.Cep = reader["cep"].ToString();
                            
                            
                        }
                    }
                }
            }
            return cliente;
        }

        //UPDATE 
        public static void AtualizarCliente(Cliente c)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();                
                string sql = "UPDATE clientes SET nome=@nome, email=@email, telefone=@tel, tipo_pessoa=@tipo, cep=@cep, endereco=@end, cidade=@cid, estado=@est WHERE id=@id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", c.Nome);
                    cmd.Parameters.AddWithValue("@email", c.Email ?? "");
                    cmd.Parameters.AddWithValue("@tel", c.Telefone ?? "");
                    cmd.Parameters.AddWithValue("@tipo", c.TipoPessoa);
                    cmd.Parameters.AddWithValue("@cep", c.Cep ?? "");
                    cmd.Parameters.AddWithValue("@end", c.Endereco ?? "");
                    cmd.Parameters.AddWithValue("@cid", c.Cidade ?? "");
                    cmd.Parameters.AddWithValue("@est", c.Estado ?? "");
                    cmd.Parameters.AddWithValue("@id", c.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}