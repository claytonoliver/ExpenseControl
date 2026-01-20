# ExpenseControl 💰

> Sistema full-stack para gerenciamento de gastos residenciais desenvolvido como resposta a um desafio técnico.

O **ExpenseControl** é uma aplicação completa que demonstra a implementação de uma API com regras de negócio específicas e um frontend moderno e reativo para controle financeiro pessoal.

---

## 🚀 Tecnologias Utilizadas

### Backend
- **C# / .NET 8** (Web API)
- **Entity Framework Core** (ORM)
- **SQL Server 22** (Banco de Dados)

### Frontend
- **React** + **TypeScript**
- **Vite** (Build Tool)
- **Gerenciamento de Estado & Hooks**
- **HTTP Client** para integração com API

---
### Screens
<img width="1454" height="801" alt="image" src="https://github.com/user-attachments/assets/b5accf73-3ad9-4525-8afd-0aaa36ea89c6" />

---

## 📋 Regras de Negócio e Funcionalidades

O sistema garante a integridade e lógica dos dados através das seguintes implementações:

### 1. Gestão de Pessoas
- **Cadastro Completo:** Criação, listagem e remoção de pessoas
- **Deleção em Cascata:** Ao remover uma pessoa, todas as transações vinculadas são excluídas automaticamente
- **Regra de Menor de Idade:**
  - Pessoas menores de 18 anos **não podem** ter receitas (Income)
  - O sistema valida a idade e restringe as operações apenas a despesas (Expense)

### 2. Gestão de Categorias
- **Finalidade Definida:** Cada categoria possui um propósito específico:
  - `Despesa` (Expense)
  - `Receita` (Income)
  - `Ambas` (Both)
- **Filtro Contextual:** Ao criar uma transação, apenas categorias compatíveis com o tipo selecionado são exibidas

### 3. Gestão de Transações
- **Validação Cruzada:** O sistema impede que uma transação de "Receita" seja salva com uma categoria de "Despesa" e vice-versa
- **Relatórios:** Visualização de totais consolidados por pessoa (Total Receitas, Total Despesas e Saldo)

---

## 📂 Estrutura do Repositório

```text
ExpenseControl/
├── backend/              # API .NET Core e Lógica de Negócios
│   └── src/
├── web/                  # Frontend React + TypeScript + Vite
│   └── src/
└── README.md            # Documentação do projeto
```

---

## 🛠️ Pré-requisitos

Certifique-se de ter as seguintes ferramentas instaladas:

- **.NET SDK 8.0+**
- **Node.js 20+** (com npm)
- **SQL Server** (LocalDB ou instância completa)
- **EF Core Tools** (para rodar migrações):

```bash
dotnet tool install --global dotnet-ef
```

---

## ⚡ Como Executar o Projeto

### 1. Backend (API)

Acesse o diretório do backend:

```bash
cd backend/src
```

**(Opcional)** Verifique o arquivo `appsettings.json` para configurar a string de conexão do SQL Server. Por padrão, o sistema utiliza `//LOCALHOST`.

Restaure as dependências e atualize o banco de dados:

```bash
dotnet restore
dotnet ef database update
```

> **Nota:** Se não houver migrations criadas, execute antes:
> ```bash
> dotnet ef migrations add InitialCreate
> ```

Execute a aplicação:

```bash
dotnet run
```

A API iniciará (por padrão) em:  
👉 **https://localhost:7002** (ou na porta definida em `launchSettings.json`)

---

### 2. Frontend (Web)

Em um novo terminal, acesse o diretório do frontend:

```bash
cd web
```

Instale as dependências:

```bash
npm install
```

**(Opcional)** Verifique o arquivo de serviço da API (ex: `src/services/api.ts`) para garantir que a URL base corresponde à porta do backend (ex: `https://localhost:7002/api`).

Inicie o servidor de desenvolvimento:

```bash
npm run dev
```

O projeto estará acessível no navegador em:  
👉 **http://localhost:5173**

---

## 🔌 Documentação da API

Principais endpoints disponíveis:

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/persons` | Lista todas as pessoas cadastradas |
| `POST` | `/api/persons` | Cadastra uma nova pessoa |
| `DELETE` | `/api/persons/{id}` | Remove uma pessoa e suas transações |
| `GET` | `/api/categories` | Lista todas as categorias |
| `POST` | `/api/categories` | Cadastra uma nova categoria |
| `GET` | `/api/transactions` | Lista todas as transações |
| `POST` | `/api/transactions` | Cria uma nova transação (com validações) |
| `PUT` | `/api/transactions/{id}` | Atualiza uma transação existente |
| `GET` | `/api/persons/totals` | Retorna o balanço financeiro por pessoa |

---

## 🧪 Validando as Regras de Negócio (Teste Manual)

Para verificar as regras implementadas no desafio:

### 🔹 Teste de Idade
1. Cadastre uma pessoa com idade inferior a 18 anos
2. Tente criar uma transação para ela
3. O campo **Tipo** não deve permitir selecionar **Receita** (Income)

### 🔹 Teste de Categoria
1. Cadastre uma categoria "Salário" com finalidade **Receita**
2. Ao tentar criar uma **Despesa**, a categoria "Salário" não deve aparecer na lista

---

## 📝 Decisões Técnicas

- **Tipagem:** No frontend, tipos como `TransactionType` e `CategoryPurpose` são tratados como strings constantes, mantendo alinhamento direto com os Enums/Strings do backend
- **UX:** O formulário de transação é dinâmico; ele filtra as categorias em tempo real com base no tipo de transação selecionado pelo usuário
- **Integridade de Dados:** Validações tanto no frontend quanto no backend garantem consistência e previnem estados inválidos

---

## 🔜 Melhorias Futuras (Roadmap)

- [ ] Implementação de **Docker** e **Docker Compose** para orquestração
- [ ] Adição de **Testes Unitários** (Backend e Frontend)
- [ ] Configuração de pipeline de **CI/CD**
- [ ] Script de **Seed** para popular o banco com dados fictícios automaticamente
- [ ] Autenticação e autorização de usuários
- [ ] Dashboard com gráficos e visualizações financeiras
- [ ] Exportação de relatórios (PDF/Excel)

---

## 📞 Suporte

Se encontrar algum problema ou tiver sugestões, sinta-se à vontade para abrir uma **issue** no repositório.
