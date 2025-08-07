# 📝 **GerenciadorDeTarefas: Sistema de Gerenciamento de Tarefas**
Este é um projeto de **Web API ASP.NET Core** desenvolvido em C# que implementa um sistema simples de gerenciamento de tarefas. Ele demonstra operações **CRUD (Create, Read, Update, Delete)** utilizando **Entity Framework Core** para persistência de dados em um banco de dados **SQL Server Express**. Este projeto foi criado como parte de um **Bootcamp da Digital Innovation One (DIO)**.

## 🚀 Como Executar o Projeto
Siga os passos abaixo para configurar e executar a aplicação ``GerenciadorDeTarefas`` em sua máquina.

### Pré-requisitos
Certifique-se de ter os seguintes softwares instalados:

  - **SQL Server Express**: Uma versão gratuita do SQL Server, ideal para desenvolvimento.

    - Download SQL Server Express

  - **SQL Server Management Studio (SSMS)**: Ferramenta gráfica essencial para gerenciar bancos de dados SQL Server.

    - Download SSMS

  - **.NET SDK 9.0 (ou 8.0 LTS)**: O SDK necessário para construir e executar aplicações ASP.NET Core.

    - Download .NET SDK

  - **IDE (JetBrains Rider ou Visual Studio)**: Ambiente de Desenvolvimento Integrado.

    - JetBrains Rider (Recomendado)

    - Visual Studio Community (para Windows)

## 📦 **Configuração do Projeto**
1. **Crie o Projeto Web API**:

  - Abra o **JetBrains Rider** (ou Visual Studio).

  - Crie uma "**New Solution**" (Nova Solução).

  - Selecione o tipo "**ASP.NET Core Web Application**" e o template "**API**".

  - Defina o "**Project name**" e "**Solution name**" como ``GerenciadorDeTarefas``.

  - Certifique-se de que a linguagem é C# e o "Target framework" é ``net9.0`` (ou ``net8.0``).

  - Selecione "**Use controllers**" (e desmarque "Use minimal APIs" se visível).

2. **Instale os Pacotes NuGet**:

  - No "**Solution Explorer**" do seu IDE, clique com o botão direito no projeto ``GerenciadorDeTarefas``.

  - Selecione "Manage NuGet Packages..." e instale os seguintes pacotes para o seu projeto:

    - ``Microsoft.EntityFrameworkCore.SqlServer`` (Instale a versão estável compatível com seu ``Target Framework``, ex: 9.0.x para net9.0 ou 8.0.x para net8.0).

    - ``Microsoft.EntityFrameworkCore.Tools`` (Instale a mesma versão do SqlServer para compatibilidade).

    - ``Swashbuckle.AspNetCore`` (Instale a versão estável compatível para habilitar o Swagger UI).

  - Alternativamente, via **Terminal** na pasta raiz do projeto (``GerenciadorDeTarefas``):
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore
```

## 📖 Modelagem de Dados
Crie a pasta ``Models`` na raiz do seu projeto e adicione os seguintes arquivos:

1. **``Models/EnumStatusTarefa.cs``**:
```
namespace GerenciadorDeTarefas.Models
{
    public enum EnumStatusTarefa
    {
        Pendente,
        Finalizado
    }
}
```
2. **``Models/Tarefa.cs``**:
```
using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeTarefas.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public EnumStatusTarefa Status { get; set; }
    }
}
```
3. **``Models/TarefaContext.cs``**:
```
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Models
{
    public class TarefaContext : DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options) : base(options)
        {
        }

        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Titulo)
                .IsRequired();

            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Status)
                .HasConversion<string>(); // Armazena o enum como string no DB

            base.OnModelCreating(modelBuilder);
        }
    }
}
```
## 💾 **Configuração do Banco de Dados e Migrações**
1. **Configure a String de Conexão** (``appsettings.json``):

- Abra o arquivo ``appsettings.json`` na raiz do projeto.
- Adicione a seção ``ConnectionStrings`` (ou modifique se já existir):
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ConexaoPadrao": "Server=localhost\\SQLEXPRESS;Database=TarefasDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;"
  }
}
```
- **Observação**: O nome do banco de dados ``TarefasDB`` será criado automaticamente pelo Entity Framework Core.

2. **Configure** o ``Program.cs``:

  - Substitua o conteúdo completo do arquivo ``Program.cs`` pelo código abaixo, que configura o DbContext e o Swagger:
```
using Microsoft.EntityFrameworkCore;
using GerenciadorDeTarefas.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TarefaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```
3. **Adicione a Migração Inicial**:

- Abra o Terminal no Rider (na pasta raiz do projeto ``GerenciadorDeTarefas``):
```
dotnet ef migrations add InitialCreate
```
- Isso criará a pasta ``Migrations`` com o script para criar a tabela ``Tarefas``.

4. **Aplique a Migração ao Banco de Dados**:

- Ainda no Terminal:
```
dotnet ef database update
```
- Este comando criará o banco de dados ``TarefasDB`` (se não existir) e a tabela ``dbo.Tarefas`` com base no seu modelo.

## ▶️ Executando e Testando a API
1. **Execute a Aplicação**:

- No **JetBrains Rider**, clique no botão **"Run"** (triângulo verde) na barra de ferramentas superior.

- A aplicação será compilada e uma janela do navegador será aberta automaticamente com o **Swagger UI**.

2. **Utilize o Swagger UI para Testar**:

- Na interface do Swagger UI, você verá os endpoints do ``TarefaController``.

- **POST/tarefa**: Para **criar** novas tarefas.

    - Clique em "Try it out" e edite o "Request body" (ex: ``{"titulo": "Estudar .NET", "descricao": "Revisar Entity Framework", "data": "2025-08-10T14:00:00", "status": 0}``). O ``Id`` será gerado automaticamente.

- **GET/tarefa**: Para listar todas as tarefas.

- **GET/tarefa/{id}**: Para buscar uma tarefa específica por seu ID.

- **GET/tarefa/status/{status}**: Para buscar tarefas por status (``0`` para Pendente, ``1`` para Finalizado).

- **PUT/tarefa/{id}**: Para atualizar uma tarefa existente (certifique-se de passar o ID correto na URL e no corpo da requisição).

- **DELETE/tarefa/{id}**: Para excluir uma tarefa por seu ID.

------
Sinta-se à vontade para explorar e modificar o código!
Desenvolvido por Eric Lopes Winkelmann para um Bootcamp da Digital Innovation One (DIO).

