# 🌱 FutureHub - Plataforma de Ideias Sustentáveis

> **Plataforma colaborativa para compartilhamento, avaliação e ranqueamento de ideias voltadas à sustentabilidade e ESG (Environmental, Social and Governance).**

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Oracle](https://img.shields.io/badge/Oracle-Database-F80000?logo=oracle)](https://www.oracle.com/database/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?logo=jsonwebtokens)](https://jwt.io/)
[![OpenTelemetry](https://img.shields.io/badge/OpenTelemetry-Tracing-3C44D5)](https://opentelemetry.io/)
[![Tests](https://img.shields.io/badge/Tests-7%2F7%20Passing-success)](https://github.com/CarlosCampos84/futurehub-dotnet)

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Funcionalidades](#-funcionalidades)
- [Tecnologias](#-tecnologias)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
- [Como Executar](#-como-executar)
- [Autenticação JWT](#-autenticação-jwt)
- [Endpoints da API](#-endpoints-da-api)
- [Arquitetura](#-arquitetura)
- [Modelo de Dados](#-modelo-de-dados)
- [Testes](#-testes)
- [Observabilidade](#-observabilidade)
- [Troubleshooting](#-troubleshooting)

---

## 🎯 Sobre o Projeto

O **FutureHub** é uma API REST desenvolvida em **.NET 9.0** que permite aos usuários:

- 📝 Criar e compartilhar ideias sustentáveis
- ⭐ Avaliar ideias de outros usuários (1 a 5 estrelas)
- 🏆 Competir em um sistema de ranking baseado em pontuação
- 🔐 Autenticar-se de forma segura com JWT
- 📊 Visualizar estatísticas e top usuários

### Contexto Acadêmico
Projeto desenvolvido para a disciplina de **Advanced Programming** da **FIAP** (2025), demonstrando aplicação de:
- Clean Architecture
- Princípios SOLID
- RESTful API Design
- JWT Authentication
- Distributed Tracing
- Testes Unitários com xUnit

---

## ✨ Funcionalidades

### Para Usuários
- ✅ Cadastro e autenticação com JWT
- ✅ Criação de ideias sustentáveis
- ✅ Avaliação de ideias (1-5 estrelas)
- ✅ Visualização de ranking pessoal
- ✅ Associação com áreas de interesse (Energia Limpa, Reciclagem, etc.)

### Sistema Automatizado
- 🔄 Cálculo automático de ranking baseado em:
  - **10 pontos** por ideia publicada
  - **Média de avaliações × 5** pontos adicionais
- 📊 Atualização de estatísticas (média e total de avaliações)
- 🗓️ Rankings mensais (formato YYYY-MM)

### Para Desenvolvedores
- 📖 Documentação OpenAPI/Swagger completa
- 🔍 Tracing distribuído com OpenTelemetry + Jaeger
- 🧪 7 testes unitários validando regras de negócio
- 📄 HATEOAS em endpoints paginados
- 🔐 Segurança com BCrypt e JWT

---

## 🚀 Tecnologias

### Backend
| Tecnologia | Versão | Propósito |
|-----------|--------|-----------|
| **.NET** | 9.0 | Framework principal |
| **ASP.NET Core Web API** | 9.0 | Camada REST |
| **Entity Framework Core** | 9.0 | ORM para acesso a dados |
| **Oracle.EntityFrameworkCore** | 9.23.80 | Provider Oracle |

### Banco de Dados
| Tecnologia | Detalhes |
|-----------|----------|
| **Oracle Database** | `oracle.fiap.com.br:1521/orcl` |
| **Usuário** | `rm555223` |
| **Tabelas** | 5 tabelas (`T_FH_*`) |

### Segurança
| Tecnologia | Versão | Propósito |
|-----------|--------|-----------|
| **BCrypt.Net-Next** | 4.0.3 | Hash de senhas (work factor 12) |
| **JWT Bearer** | 9.0.0 | Autenticação stateless |
| **System.IdentityModel.Tokens.Jwt** | 8.2.1 | Geração de tokens |

### Observabilidade
| Tecnologia | Versão | Propósito |
|-----------|--------|-----------|
| **OpenTelemetry** | 1.10.0 | Tracing distribuído |
| **OpenTelemetry.Instrumentation.AspNetCore** | 1.10.0 | Instrumentação HTTP |
| **OpenTelemetry.Exporter.OTLP** | 1.10.0 | Export para Jaeger |
| **Jaeger** | latest | Visualização de traces |

### Testes
| Tecnologia | Versão | Propósito |
|-----------|--------|-----------|
| **xUnit** | 2.9.2 | Framework de testes |
| **Moq** | 4.20.72 | Mocking de dependências |
| **coverlet.collector** | 6.0.2 | Cobertura de código |

### Documentação
| Tecnologia | Versão | Propósito |
|-----------|--------|-----------|
| **Swashbuckle.AspNetCore** | 7.2.0 | OpenAPI/Swagger |
| **Microsoft.AspNetCore.Mvc.Versioning** | 5.1.0 | Versionamento de API |

---

## 📦 Pré-requisitos

Antes de começar, você precisará ter instalado:

### Obrigatório
- ✅ [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- ✅ Acesso ao Oracle Database (credenciais FIAP fornecidas)

### Opcional (para desenvolvimento completo)
- 🐳 [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para Jaeger)
- 🔧 [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- 📦 [Postman](https://www.postman.com/) ou [Insomnia](https://insomnia.rest/) (para testar API)

### Verificar Instalação do .NET
```bash
dotnet --version
# Deve retornar: 9.0.x
```

---

## 📥 Instalação

### 1️⃣ Clonar o Repositório
```bash
git clone https://github.com/CarlosCampos84/futurehub-dotnet.git
cd futurehub-dotnet
```

### 2️⃣ Restaurar Dependências
```bash
dotnet restore
```

**Saída esperada:**
```
Restaurando pacotes para FutureHub.Web...
Restaurando pacotes para FutureHub.Testes...
Restaurar concluído em X.X s
```

### 3️⃣ Aplicar Migrations ao Banco de Dados
```bash
cd FutureHub.Web
dotnet ef database update
```

**O que isso faz:**
- Cria as 5 tabelas no Oracle Database
- Aplica todas as migrations versionadas
- Configura chaves primárias, foreign keys e índices

**Tabelas criadas:**
```
✓ T_FH_USUARIOS
✓ T_FH_AREAS
✓ T_FH_IDEIAS
✓ T_FH_AVALIACOES
✓ T_FH_RANKINGS
```

### 4️⃣ (Opcional) Iniciar Jaeger para Observabilidade
```bash
# Voltar para raiz do projeto
cd ..

# Iniciar container Jaeger
docker-compose up -d
```

**Portas do Jaeger:**
- 🌐 **UI**: http://localhost:16686
- 📡 **OTLP HTTP**: localhost:4318
- 📡 **OTLP gRPC**: localhost:4317

---

## ▶️ Como Executar

### Método 1: Via Linha de Comando (Recomendado)

```bash
cd FutureHub.Web
dotnet run
```

**Saída esperada:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5259
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Método 2: Via Visual Studio
1. Abrir `FutureHub.sln`
2. Definir `FutureHub.Web` como projeto de inicialização
3. Pressionar **F5** ou clicar em **▶️ Run**

### Método 3: Via VS Code
1. Abrir pasta `futurehub-dotnet`
2. Pressionar **F5**
3. Selecionar **.NET Core Launch (web)**

---

## 🌐 Acessar a Aplicação

### Swagger UI (Documentação Interativa)
```
🔗 http://localhost:5259
ou
🔗 http://localhost:5259/swagger
```

**O que você verá:**
- ✅ Lista completa de endpoints
- ✅ Schemas de request/response
- ✅ Botão "Authorize" para autenticação JWT
- ✅ Testar endpoints diretamente no navegador

### Jaeger UI (Tracing)
```
🔗 http://localhost:16686
```

**O que você verá:**
- ✅ Traces de todas as requisições HTTP
- ✅ Spans customizados (Login, CreateIdeia, CreateAvaliacao)
- ✅ Tempo de execução de cada operação
- ✅ Tags e eventos customizados

---

## 🔐 Autenticação JWT

A API usa **JWT Bearer Token** para autenticação. Todos os endpoints (exceto login e register) requerem autenticação.

### 1️⃣ Registrar Novo Usuário

**Endpoint:** `POST /api/v1/auth/register`

**Request:**
```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "SenhaSegura123",
  "areaInteresseId": "opcional-guid-da-area"
}
```

**Response (201 Created):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "usuario": {
    "id": "guid-gerado",
    "nome": "João Silva",
    "email": "joao@email.com",
    "pontos": 0,
    "role": "ROLE_USER"
  }
}
```

### 2️⃣ Fazer Login

**Endpoint:** `POST /api/v1/auth/login`

**Request:**
```json
{
  "email": "joao@email.com",
  "senha": "SenhaSegura123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "usuario": { ... }
}
```

### 3️⃣ Usar o Token

#### Via Swagger UI:
1. Clique no botão **🔓 Authorize** no topo
2. Digite: `Bearer SEU_TOKEN_AQUI`
3. Clique em **Authorize**
4. Agora pode testar endpoints protegidos

#### Via curl:
```bash
curl -X GET "http://localhost:5259/api/v1/ideias" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

#### Via Postman:
1. Aba **Authorization**
2. Type: **Bearer Token**
3. Token: `Cole seu token aqui`

### 4️⃣ Informações do Token

O token JWT contém:
- **UserId**: ID do usuário autenticado
- **Nome**: Nome completo
- **Email**: Email do usuário
- **Role**: Papel (ROLE_USER)
- **Validade**: 60 minutos (configurável)

---

## 📡 Endpoints da API

### Visão Geral por Módulo

| Módulo | Endpoints | Descrição |
|--------|-----------|-----------|
| 🔐 **Auth** | 2 | Autenticação e registro |
| 👤 **Usuários** | 5 | CRUD de usuários |
| 🏷️ **Áreas** | 5 | CRUD de áreas de interesse |
| 💡 **Ideias** | 5 | CRUD de ideias sustentáveis |
| ⭐ **Avaliações** | 4 | Criar e listar avaliações |
| 🏆 **Rankings** | 3 | Consultar rankings |

**Total: 24 endpoints**

---

### 🔐 Autenticação (`/api/v1/auth`)

#### `POST /api/v1/auth/register`
Cadastrar novo usuário.

**Request Body:**
```json
{
  "nome": "string (obrigatório, max 200 caracteres)",
  "email": "string (obrigatório, formato email, único)",
  "senha": "string (obrigatório, min 6 caracteres)",
  "areaInteresseId": "string (opcional, GUID válido)"
}
```

**Validações:**
- ❌ Email já cadastrado → `400 Bad Request`
- ❌ Área inexistente → `400 Bad Request`
- ✅ Senha hasheada com BCrypt (work factor 12)

---

#### `POST /api/v1/auth/login`
Autenticar usuário existente.

**Request Body:**
```json
{
  "email": "string (obrigatório)",
  "senha": "string (obrigatório)"
}
```

**Validações:**
- ❌ Usuário não existe → `401 Unauthorized`
- ❌ Senha incorreta → `401 Unauthorized`
- ✅ Retorna token JWT válido por 60 minutos

---

### 💡 Ideias (`/api/v1/ideias`)

#### `GET /api/v1/ideias`
Listar todas as ideias (paginado com HATEOAS).

**Query Parameters:**
```
?page=1              # Número da página (default: 1)
&pageSize=10         # Itens por página (default: 10)
```

**Response (200 OK):**
```json
{
  "data": [
    {
      "id": "guid",
      "titulo": "Sistema de Compostagem Comunitária",
      "descricao": "Criar pontos de compostagem em bairros residenciais...",
      "autorId": "guid",
      "autorNome": "João Silva",
      "mediaNotas": 4.5,
      "totalAvaliacoes": 12,
      "createdAt": "2025-11-23T10:30:00Z"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 45,
  "totalPages": 5,
  "hasPrevious": false,
  "hasNext": true,
  "links": [
    {
      "href": "http://localhost:5259/api/v1/ideias?page=1&pageSize=10",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "http://localhost:5259/api/v1/ideias?page=2&pageSize=10",
      "rel": "next",
      "method": "GET"
    }
  ]
}
```

---

#### `POST /api/v1/ideias` 🔒
Criar nova ideia (requer autenticação).

**Request Body:**
```json
{
  "titulo": "string (obrigatório, max 160 caracteres)",
  "descricao": "string (obrigatório, max 2000 caracteres)"
}
```

**Comportamento:**
- ✅ Autor é extraído automaticamente do token JWT
- ✅ Atualiza ranking do usuário (+10 pontos)
- ✅ Define `createdAt` automaticamente

**Response (201 Created):**
```json
{
  "id": "guid-gerado",
  "titulo": "Título da ideia",
  "descricao": "Descrição completa...",
  "autorId": "guid-do-usuario",
  "autorNome": "João Silva",
  "mediaNotas": 0,
  "totalAvaliacoes": 0,
  "createdAt": "2025-11-23T15:45:00Z"
}
```

---

#### `GET /api/v1/ideias/{id}`
Buscar ideia por ID.

**Response (200 OK):** Objeto `IdeiaDTO`

**Response (404 Not Found):** Se ideia não existir

---

#### `PUT /api/v1/ideias/{id}` 🔒
Atualizar ideia existente.

**Request Body:**
```json
{
  "titulo": "string (opcional)",
  "descricao": "string (opcional)"
}
```

---

#### `DELETE /api/v1/ideias/{id}` 🔒
Deletar ideia.

**Response (204 No Content):** Sucesso

---

### ⭐ Avaliações (`/api/v1/avaliacoes`)

#### `POST /api/v1/avaliacoes` 🔒
Avaliar uma ideia (requer autenticação).

**Request Body:**
```json
{
  "ideiaId": "string (obrigatório, GUID)",
  "nota": 5
}
```

**Validações:**
- ✅ Nota entre 1 e 5
- ❌ Ideia inexistente → `400 Bad Request`

**Comportamento Automatizado:**
1. ✅ Cria a avaliação
2. ✅ Recalcula `mediaNotas` da ideia
3. ✅ Incrementa `totalAvaliacoes` da ideia
4. ✅ Atualiza ranking do autor da ideia (async)

---

#### `GET /api/v1/avaliacoes` 🔒
Listar todas as avaliações (paginado).

---

#### `GET /api/v1/avaliacoes/ideia/{ideiaId}`
Listar avaliações de uma ideia específica (paginado).

---

#### `GET /api/v1/avaliacoes/{id}` 🔒
Buscar avaliação por ID.

---

### 🏆 Rankings (`/api/v1/rankings`)

#### `GET /api/v1/rankings`
Listar top usuários por pontuação (paginado).

**Query Parameters:**
```
?page=1
&pageSize=10
```

**Response (200 OK):**
```json
{
  "data": [
    {
      "usuarioId": "guid",
      "usuarioNome": "João Silva",
      "pontuacaoTotal": 250,
      "ideiasPublicadas": 15,
      "mediaAvaliacoes": 4.5
    }
  ],
  ...
}
```

---

#### `GET /api/v1/rankings/usuario/{usuarioId}` 🔒
Buscar ranking de um usuário específico.

---

### 🏷️ Áreas (`/api/v1/areas`)

#### `POST /api/v1/areas`
Criar nova área de interesse (público).

**Request Body:**
```json
{
  "nome": "string (obrigatório, max 100 caracteres)",
  "descricao": "string (opcional, max 500 caracteres)"
}
```

**Exemplos de Áreas:**
- 🌿 Energia Limpa
- ♻️ Reciclagem e Gestão de Resíduos
- 💧 Preservação da Água
- 🌱 Agricultura Sustentável
- 🚗 Mobilidade Urbana

---

## 🏗️ Arquitetura

### Clean Architecture (3 Camadas)

```
┌─────────────────────────────────────────┐
│         CONTROLLERS (Apresentação)       │
│  - Recebe HTTP Requests                 │
│  - Valida DTOs                          │
│  - Retorna HTTP Responses               │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│         SERVICES (Lógica de Negócio)    │
│  - Regras de negócio                    │
│  - Validações complexas                 │
│  - Orquestração                         │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│       REPOSITORIES (Acesso a Dados)     │
│  - Queries ao banco (EF Core)           │
│  - CRUD operations                      │
│  - Eager/Lazy loading                   │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│          ORACLE DATABASE                │
│  - 5 Tabelas (T_FH_*)                  │
│  - Constraints e Índices                │
└─────────────────────────────────────────┘
```

### Princípios SOLID Aplicados

| Princípio | Implementação |
|-----------|---------------|
| **S** - Single Responsibility | Cada service tem uma responsabilidade única (ex: `RankingService` só gerencia rankings) |
| **O** - Open/Closed | Extensível via interfaces sem modificar código existente |
| **L** - Liskov Substitution | Implementações podem ser substituídas pelas interfaces |
| **I** - Interface Segregation | Interfaces específicas (`IUsuarioRepository`, `IIdeiaRepository`) |
| **D** - Dependency Inversion | Dependências via abstrações (DI container) |

### Injeção de Dependências

**Lifetimes utilizados:**

```csharp
// Scoped - Uma instância por requisição HTTP
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// DbContext - Uma instância por requisição
builder.Services.AddDbContext<OracleDbContext>();

// Singleton - Uma instância para toda aplicação
// (Usado para ML.NET quando implementado)
```

---

## 🗄️ Modelo de Dados

### Diagrama ER

```
┌─────────────────┐
│   T_FH_AREAS    │
│─────────────────│
│ ID (PK)         │
│ NOME            │
│ DESCRICAO       │
└────────┬────────┘
         │
         │ 1:N
         │
         ▼
┌─────────────────┐         1:N        ┌─────────────────┐
│ T_FH_USUARIOS   │────────────────────│  T_FH_IDEIAS    │
│─────────────────│                    │─────────────────│
│ ID (PK)         │                    │ ID (PK)         │
│ NOME            │                    │ TITULO          │
│ EMAIL (UNIQUE)  │                    │ DESCRICAO       │
│ SENHA_HASH      │                    │ AUTOR_ID (FK)   │
│ AREA_INT_ID(FK) │                    │ MEDIA_NOTAS     │
│ PONTOS          │                    │ TOTAL_AVALIACOES│
│ ROLE            │                    │ CREATED_AT      │
└────────┬────────┘                    └────────┬────────┘
         │                                      │
         │ 1:N                                  │ 1:N
         │                                      │
         ▼                                      ▼
┌─────────────────┐                    ┌─────────────────┐
│ T_FH_RANKINGS   │                    │T_FH_AVALIACOES  │
│─────────────────│                    │─────────────────│
│ ID (PK)         │                    │ ID (PK)         │
│ USUARIO_ID (FK) │                    │ IDEIA_ID (FK)   │
│ PONTUACAO_TOTAL │                    │ NOTA (1-5)      │
│ PERIODO (YYYY-MM)│                   │ DATA_AVALIACAO  │
└─────────────────┘                    │ CREATED_AT      │
                                       └─────────────────┘
```

### Regras de Negócio do Modelo

#### Usuario
- ✅ Email único (UNIQUE constraint)
- ✅ Senha hasheada com BCrypt (nunca armazenar texto plano)
- ✅ Pontos atualizados automaticamente
- ✅ Área de interesse opcional

#### Ideia
- ✅ Sempre possui um autor (FK obrigatória)
- ✅ `MediaNotas` e `TotalAvaliacoes` recalculados automaticamente
- ✅ Ordenação padrão: mais recentes primeiro

#### Avaliacao
- ✅ Nota entre 1 e 5 (validação via DataAnnotations)
- ✅ Integridade referencial: ideia deve existir
- ✅ Dispara atualização de estatísticas da ideia

#### Ranking
- ✅ Um ranking por usuário por período (YYYY-MM)
- ✅ Recalculado quando ideia é criada ou avaliada
- ✅ Fórmula: `(nº ideias × 10) + Σ(média avaliações × 5)`

---

## 🧪 Testes

### Executar Todos os Testes

```bash
dotnet test
```

**Saída esperada:**
```
Aprovado!  – Com falha: 0, Aprovado: 7, Ignorado: 0, Total: 7
```

### Executar com Detalhes

```bash
dotnet test --verbosity normal
```

### Ver Cobertura de Código

```bash
dotnet test /p:CollectCoverage=true
```

---

### Testes Implementados (7/7 ✅)

#### 1️⃣ RankingServiceTests

**Teste:** `AtualizarRankingAsync_DeveCacularPontuacaoCorretamente_ComUmaIdeiaSemAvaliacoes`
- **Cenário:** Usuário com 1 ideia sem avaliações
- **Esperado:** 10 pontos (1 × 10 + 0 × 5)
- **Valida:** Cálculo básico de pontuação

**Teste:** `AtualizarRankingAsync_DeveCacularPontuacaoCorretamente_ComMultiplasIdeiasAvaliadas`
- **Cenário:** 3 ideias com médias 4.5, 3.0, 5.0
- **Esperado:** 92 pontos (30 base + 22 + 15 + 25)
- **Valida:** Cálculo complexo com múltiplas ideias

**Teste:** `AtualizarRankingAsync_DeveAtualizarRankingExistente_QuandoJaExisteParaPeriodo`
- **Cenário:** Ranking já existe para o mês atual
- **Esperado:** UPDATE em vez de INSERT
- **Valida:** Unicidade de ranking por período

---

#### 2️⃣ AuthServiceTests

**Teste:** `RegisterAsync_DeveLancarExcecao_QuandoEmailJaExiste`
- **Cenário:** Cadastro com email duplicado
- **Esperado:** `InvalidOperationException`
- **Valida:** Unicidade de email

**Teste:** `LoginAsync_DeveLancarExcecao_QuandoSenhaIncorreta`
- **Cenário:** Login com senha errada
- **Esperado:** `UnauthorizedAccessException`
- **Valida:** Verificação BCrypt

---

#### 3️⃣ AvaliacaoServiceTests

**Teste:** `CreateAsync_DeveLancarExcecao_QuandoIdeiaInexistente`
- **Cenário:** Avaliar ideia que não existe
- **Esperado:** `InvalidOperationException`
- **Valida:** Integridade referencial

**Teste:** `CreateAsync_DeveAtualizarMediaETotal_QuandoAvaliacaoValida`
- **Cenário:** Criar avaliação válida
- **Esperado:** Ideia atualizada com nova média
- **Valida:** Recalculo de estatísticas

---

### Padrão AAA (Arrange-Act-Assert)

```csharp
[Fact]
public async Task NomeDoTeste()
{
    // 1️⃣ ARRANGE - Preparar dados
    var mockRepo = new Mock<IRepository>();
    var service = new Service(mockRepo.Object);
    
    // 2️⃣ ACT - Executar ação
    var result = await service.MetodoTestado();
    
    // 3️⃣ ASSERT - Verificar resultado
    Assert.NotNull(result);
    mockRepo.Verify(r => r.Metodo(), Times.Once);
}
```

---

## 📊 Observabilidade

### OpenTelemetry + Jaeger

#### Iniciar Jaeger

```bash
docker-compose up -d
```

#### Acessar Jaeger UI

```
🔗 http://localhost:16686
```

#### Visualizar Traces

1. Selecione o serviço: **FutureHub.Api**
2. Clique em **Find Traces**
3. Veja a lista de requisições
4. Clique em um trace para ver detalhes

#### Spans Customizados

Os seguintes métodos possuem tracing manual:

**AuthService.LoginAsync**
- Tags: `user.email`, `user.id`
- Eventos: "Usuário não encontrado", "Senha incorreta", "Login bem-sucedido"

**IdeiaService.CreateAsync**
- Tags: `autor.id`, `ideia.titulo`, `ideia.id`
- Eventos: "Ideia criada"

**AvaliacaoService.CreateAsync**
- Tags: `user.id`, `ideia.id`, `avaliacao.nota`, `avaliacao.id`
- Eventos: "Ideia não encontrada", "Avaliação criada"

#### Exemplo de Trace Completo

```
POST /api/v1/ideias
  ├─ AuthService.LoginAsync (30ms)
  │   ├─ SQL: SELECT usuarios (12ms)
  │   └─ BCrypt.Verify (15ms)
  ├─ IdeiaService.CreateAsync (45ms)
  │   ├─ SQL: INSERT ideias (20ms)
  │   └─ RankingService.AtualizarRankingAsync (18ms)
  │       └─ SQL: UPDATE rankings (10ms)
  └─ Total: 75ms
```

---

## 🔧 Troubleshooting

### Problema: "Could not find Oracle.EntityFrameworkCore"

**Solução:**
```bash
dotnet restore --force
dotnet build
```

---

### Problema: "Database connection failed"

**Verificar:**
1. ✅ Acesso à rede FIAP (VPN se remoto)
2. ✅ Credenciais corretas em `appsettings.json`
3. ✅ Oracle Database online

**Connection String:**
```json
"OracleConnection": "User Id=rm555223;Password=190606;Data Source=oracle.fiap.com.br:1521/orcl;"
```

---

### Problema: "JWT token invalid"

**Causas comuns:**
- ❌ Token expirado (validade: 60 minutos)
- ❌ Formato incorreto (deve ser: `Bearer TOKEN`)
- ❌ SecretKey alterada no servidor

**Solução:**
1. Fazer novo login
2. Usar token retornado
3. Verificar formato: `Authorization: Bearer eyJhbGc...`

---

### Problema: "Port already in use"

**Solução:**
```bash
# Matar processo na porta 5259
lsof -ti:5259 | xargs kill -9

# Ou mudar porta em launchSettings.json
```

---

### Problema: Migrations não aplicam

**Solução:**
```bash
# Deletar pasta Migrations
rm -rf FutureHub.Web/Migrations

# Recriar migrations
dotnet ef migrations add InitialCreate --project FutureHub.Web
dotnet ef database update --project FutureHub.Web
```

---

## 📚 Estrutura do Projeto

```
futurehub-dotnet/
│
├── FutureHub.Web/                    # Projeto principal da API
│   ├── Controllers/                  # 6 controllers REST
│   │   ├── AuthController.cs
│   │   ├── UsuariosController.cs
│   │   ├── IdeiasController.cs
│   │   ├── AvaliacoesController.cs
│   │   ├── RankingsController.cs
│   │   └── AreasController.cs
│   │
│   ├── Services/                     # 6 services (lógica de negócio)
│   │   ├── Interfaces/
│   │   ├── AuthService.cs
│   │   ├── UsuarioService.cs
│   │   ├── IdeiaService.cs
│   │   ├── AvaliacaoService.cs
│   │   ├── RankingService.cs
│   │   └── AreaService.cs
│   │
│   ├── Repositories/                 # 5 repositories (acesso a dados)
│   │   ├── Interfaces/
│   │   ├── UsuarioRepository.cs
│   │   ├── IdeiaRepository.cs
│   │   ├── AvaliacaoRepository.cs
│   │   ├── RankingRepository.cs
│   │   └── AreaRepository.cs
│   │
│   ├── Models/                       # Entidades e DTOs
│   │   ├── Usuario.cs
│   │   ├── Ideia.cs
│   │   ├── Avaliacao.cs
│   │   ├── Ranking.cs
│   │   ├── Area.cs
│   │   ├── Configuration/
│   │   │   └── JwtSettings.cs
│   │   ├── DTOs/
│   │   │   ├── AuthDTOs.cs
│   │   │   ├── UsuarioDTOs.cs
│   │   │   ├── IdeiaDTOs.cs
│   │   │   ├── AvaliacaoDTOs.cs
│   │   │   ├── RankingDTOs.cs
│   │   │   └── AreaDTOs.cs
│   │   └── Pagination/
│   │       ├── PagedResult.cs
│   │       ├── Link.cs
│   │       └── PaginationQuery.cs
│   │
│   ├── Data/                         # DbContext e Mappings
│   │   ├── OracleDbContext.cs
│   │   └── Mappings/
│   │       ├── UsuarioMapping.cs
│   │       ├── IdeiaMapping.cs
│   │       ├── AvaliacaoMapping.cs
│   │       ├── RankingMapping.cs
│   │       └── AreaMapping.cs
│   │
│   ├── Helpers/                      # Classes auxiliares
│   │   └── HateoasLinks.cs
│   │
│   ├── Observability/                # Tracing
│   │   └── Tracing.cs
│   │
│   ├── Migrations/                   # Migrations do EF Core
│   │
│   ├── Program.cs                    # Startup e DI
│   ├── appsettings.json             # Configurações
│   └── FutureHub.Web.csproj         # Arquivo do projeto
│
├── FutureHub.Testes/                 # Projeto de testes
│   ├── RankingServiceTests.cs        # 3 testes
│   ├── AuthServiceTests.cs           # 2 testes
│   ├── AvaliacaoServiceTests.cs      # 2 testes
│   ├── README.md                     # Documentação dos testes
│   └── FutureHub.Testes.csproj
│
├── docker-compose.yml                # Jaeger container
├── .gitignore
├── FutureHub.sln                     # Solution file
└── README.md                         # Este arquivo
```

---

## 📄 Configuração (appsettings.json)

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=rm555223;Password=190606;Data Source=oracle.fiap.com.br:1521/orcl;"
  },
  "JwtSettings": {
    "SecretKey": "chave-secreta-super-segura-com-pelo-menos-32-caracteres-minimo",
    "Issuer": "FutureHub",
    "Audience": "FutureHub-Users",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

---

## 👥 Equipe

- Carlos Ferraz | RM555223
- Antonio Junior | RM554518
- Caio Henrique | RM554600


