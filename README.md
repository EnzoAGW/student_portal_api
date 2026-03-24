# Portal Escolar — ASP.NET Core API

API REST para gerenciamento de um portal escolar com dois módulos: **Portal do Aluno** e **Portal do Empregado**.

## Tecnologias

- .NET 10 / ASP.NET Core
- Entity Framework Core + PostgreSQL (Npgsql)
- JWT Bearer Authentication
- Swagger / OpenAPI
- xUnit + Moq (testes)

---

## Como rodar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL rodando em `localhost:5432`

### 1. Banco de dados

Crie o banco e aplique as migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

A string de conexão está em `infra/ConnectionContext.cs`. Ajuste usuário/senha se necessário.

### 2. Rodar a API

```bash
dotnet run
```

Acesse o Swagger em: `https://localhost:{porta}/swagger`

### 3. Rodar os testes

```bash
cd ../WebApplication1.Tests
dotnet test
```

---

## Autenticação

Todos os endpoints exigem um token JWT. Obtenha-o via:

```
POST /api/v1/auth?username={usuario}&password={senha}
```

| Usuário      | Senha    | Papel        |
|--------------|----------|--------------|
| `admin`      | `123`    | admin        |
| `rh`         | `rh123`  | rh           |
| `secretaria` | `sec123` | secretaria   |
| `professor`  | `prof123`| professor    |

Use o token no header: `Authorization: Bearer {token}`

---

## Módulos e Endpoints

### Autenticação

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/auth` | Gera token JWT |

---

### Portal do Aluno

#### Alunos

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/student` | qualquer | Cadastrar aluno (com foto opcional) |
| GET | `/api/v1/student?pageNumber=0&pageQtd=10` | qualquer | Listar alunos paginado |
| GET | `/api/v1/student/{id}` | qualquer | Buscar aluno por ID |
| POST | `/api/v1/student/{id}/download` | qualquer | Download da foto do aluno |

#### Matérias

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/subject` | secretaria, admin | Criar matéria |
| GET | `/api/v1/subject` | qualquer | Listar matérias |
| GET | `/api/v1/subject/{id}` | qualquer | Buscar matéria por ID |

#### Matrícula em Matéria

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/enrollment` | secretaria, admin | Matricular aluno em matéria |
| GET | `/api/v1/enrollment/student/{id}` | qualquer | Matérias de um aluno |
| GET | `/api/v1/enrollment/subject/{id}` | qualquer | Alunos de uma matéria |

#### Notas

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/grade` | professor, admin | Lançar nota |
| GET | `/api/v1/grade/student/{id}` | qualquer | Notas de um aluno |
| GET | `/api/v1/grade/student/{studentId}/subject/{subjectId}/average` | qualquer | Média do aluno na matéria |

#### Frequência

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/attendance` | professor, admin | Registrar chamada (presente/falta) |
| GET | `/api/v1/attendance/student/{studentId}/subject/{subjectId}` | qualquer | Histórico de chamadas |
| GET | `/api/v1/attendance/student/{studentId}/subject/{subjectId}/percentage` | qualquer | % de presença (aprovado/reprovado) |

---

### Portal do Empregado

#### Funcionários

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/employee` | qualquer | Cadastrar funcionário |
| GET | `/api/v1/employee?pageNumber=0&pageQtd=10` | qualquer | Listar funcionários |
| POST | `/api/v1/employee/{id}/download` | qualquer | Download da foto |

#### Professor → Matéria

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/subject-teacher` | rh, admin | Atribuir professor a matéria |
| GET | `/api/v1/subject-teacher/employee/{id}` | qualquer | Matérias de um professor |
| GET | `/api/v1/subject-teacher/subject/{id}` | qualquer | Professores de uma matéria |

#### Turnos

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/workshift` | rh, admin | Criar turno (Manhã, Tarde, Noite...) |
| GET | `/api/v1/workshift` | qualquer | Listar turnos |

#### Escala

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/schedule` | rh, admin | Escalar funcionário em um dia |
| GET | `/api/v1/schedule/employee/{id}` | qualquer | Escala de um funcionário |
| GET | `/api/v1/schedule/date/{date}` | qualquer | Quem trabalha em determinada data |

#### Ponto

| Método | Rota | Papel | Descrição |
|--------|------|-------|-----------|
| POST | `/api/v1/timerecord/employee/{id}/clockin` | qualquer | Bater entrada |
| POST | `/api/v1/timerecord/employee/{id}/breakstart` | qualquer | Saída para almoço |
| POST | `/api/v1/timerecord/employee/{id}/breakend` | qualquer | Retorno do almoço |
| POST | `/api/v1/timerecord/employee/{id}/clockout` | qualquer | Bater saída (calcula horas) |
| GET | `/api/v1/timerecord/employee/{id}/today` | qualquer | Ponto de hoje |
| GET | `/api/v1/timerecord/employee/{id}/month?year=2025&month=3` | qualquer | Espelho do mês |

---

## Arquitetura

```
Controllers/      ← recebem requisições HTTP, delegam para repositories
Model/            ← entidades do banco + interfaces dos repositories
infra/            ← implementações dos repositories (falam com o banco)
ViewModel/        ← dados que chegam nas requisições (formulários)
Services/         ← serviços auxiliares (geração de token JWT)
Storage/          ← arquivos de foto dos usuários
```

### Fluxo de uma requisição

```
HTTP Request
    │
    ▼
Controller         ← valida permissão (JWT role), chama repository
    │
    ▼
IRepository        ← interface (contrato)
    │
    ▼
Repository         ← acessa o banco via ConnectionContext (EF Core)
    │
    ▼
PostgreSQL
```

### Injeção de Dependência

O `Program.cs` registra todos os repositórios. O ASP.NET injeta automaticamente nos controllers:

```csharp
// Program.cs
builder.Services.AddDbContext<ConnectionContext>(...);
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
// etc.
```

---

## Testes

O projeto `WebApplication1.Tests` contém 28 testes divididos em:

| Categoria | O que testa |
|-----------|-------------|
| **Models** | Lógica de cálculo de horas trabalhadas (`TimeRecord`) |
| **Repositories** | Média de notas, percentual de presença, ordenação (banco InMemory) |
| **Controllers** | Respostas HTTP: 200, 404, 400, 409 (com repositórios mockados via Moq) |

```bash
dotnet test
# Aprovado! – Com falha: 0, Aprovado: 28, Total: 28
```
