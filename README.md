# API de Gerenciamento de Vendas - DeveloperStore

Este projeto é uma API construída em .NET para gerenciar registros de vendas, incluindo funcionalidades de CRUD completo, aplicação de regras de negócio e publicação de eventos simulados. Ele utiliza uma stack moderna com bibliotecas e frameworks que aumentam a produtividade e garantem robustez ao código.

---

## 🚀 Tecnologias Utilizadas

### Principais Tecnologias

- **Backend**: [.NET 6](https://dotnet.microsoft.com/)
- **Banco de Dados**: [Entity Framework Core](https://learn.microsoft.com/en-us/ef/)
- **Testes**: [xUnit](https://xunit.net/), [Moq](https://github.com/moq)
- **Validação**: [FluentValidation](https://fluentvalidation.net/)
- **Mediador**: [MediatR](https://github.com/jbogard/MediatR)
- **Mapeamento de Objetos**: [AutoMapper](https://automapper.org/)
- **Geração de Dados Falsos**: [Bogus](https://github.com/bchavez/Bogus)

---

## 📋 Requisitos do Projeto

### Funcionalidades da API

1. **CRUD completo para vendas**:
   - Criar, ler, atualizar e cancelar vendas.
   - Gerenciamento detalhado de produtos, quantidades, preços, descontos e estado (cancelada/não cancelada).

2. **Regras de negócio**:
   - Compras acima de 4 itens idênticos possuem 10% de desconto.
   - Compras entre 10 e 20 itens idênticos possuem 20% de desconto.
   - Não é permitido vender mais de 20 itens idênticos.
   - Compras abaixo de 4 itens não podem ter desconto.

3. **Publicação de eventos simulados**:
   - VendaCriada, VendaModificada, VendaCancelada, ItemCancelado (registrados no log).

4. **Documentação da API**:
   - A API é documentada usando Swagger.

---

## 🔧 O que cada pacote faz?

### **AutoMapper**
- Facilita o mapeamento entre objetos de diferentes camadas (ex.: entidades e DTOs).
- Permite transformar objetos de forma simples e eficiente.

### **Bogus**
- Gera dados falsos para testes e desenvolvimento.
- Útil para simular vendas, clientes, produtos e outros dados.

### **FluentValidation**
- Biblioteca para validação de objetos e dados.
- Ajuda a garantir que os dados enviados para a API estejam corretos, aplicando regras de validação de forma declarativa.

### **MediatR**
- Implementa o padrão Mediator para desacoplar a lógica de negócios.
- Permite a comunicação entre diferentes componentes da aplicação via comandos e eventos.

### **Entity Framework Core (EF Core)**
- ORM (Object-Relational Mapping) para interagir com o banco de dados.
- Simplifica operações como criação, leitura, atualização e exclusão (CRUD).

### **xUnit**
- Framework para testes unitários.
- Facilita a escrita e execução de testes automatizados.

### **Moq**
- Biblioteca para criação de mocks (objetos simulados) em testes unitários.
- Útil para simular dependências e testar componentes isoladamente.
