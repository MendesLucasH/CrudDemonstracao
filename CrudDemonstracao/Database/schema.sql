CREATE TABLE IF NOT EXISTS clientes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    tipo_pessoa ENUM('PF','PJ') NULL,
    cpf_cnpj VARCHAR(18) NOT NULL,
    email VARCHAR(150),
    telefone VARCHAR(20),
    endereco VARCHAR(255),
    cidade VARCHAR(100),
    estado CHAR(2),
    cep VARCHAR(9),
    data_criacao DATETIME DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_cpf_cnpj (cpf_cnpj) 
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
