# React + TypeScript + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Babel](https://babeljs.io/) (or [oxc](https://oxc.rs) when used in [rolldown-vite](https://vite.dev/guide/rolldown)) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh

## React Compiler

The React Compiler is not enabled on this template because of its impact on dev & build performances. To add it, see [this documentation](https://react.dev/learn/react-compiler/installation).

## Expanding the ESLint configuration

If you are developing a production application, we recommend updating the configuration to enable type-aware lint rules:

```js
export default defineConfig([
  globalIgnores(['dist']),
  {
    # ExpenseControl

    Projeto desenvolvido como resposta ao desafio técnico: um sistema de controle de gastos residenciais dividido entre backend (.NET) e frontend (React + TypeScript).

    ## Resumo
    - Backend em C# / .NET
    - Frontend em React + TypeScript (Vite)
    - Persistência configurável (SQLite / SQL Server / PostgreSQL conforme ambiente)

    ## Regras de negócio principais
    - Cadastro de Pessoas: criar, listar e deletar. Ao deletar uma pessoa, todas as transações vinculadas são removidas.
    - Cadastro de Categorias: criar e listar. Cada categoria tem uma finalidade: `Despesa`, `Receita` ou `Ambas`.
    - Cadastro de Transações: criar e listar com validações:
      - Menores de 18 anos só podem ter transações do tipo `Despesa`.
      - Categoria deve ser compatível com o tipo da transação (despesa/receita), conforme o campo `purpose` da categoria.
    - Relatórios: totais por pessoa (receita, despesa, saldo) e totais por categoria (se implementado).

    ## Estrutura esperada do repositório
    - `backend/` — código do WebApi em .NET (C#)
    - `web/` — frontend React + TypeScript (já presente neste workspace)

    - ## Pré-requisitos
    - .NET SDK 8.0+ (ou a versão usada no projeto backend)
    -
    -> Se o backend usa Entity Framework Core migrations, instale/ative as ferramentas do EF Core:

    - ```bash
    - dotnet tool install --global dotnet-ef --version 8.*
    - # ou, se o projeto usa ferramentas locais
    - dotnet tool restore
    - ```
    - Node.js 16+ e npm/yarn

    ## Executando o backend (exemplo)
    1. Entre na pasta do backend (ajuste o caminho se estiver diferente):

    ```bash
    cd backend
    ```

    2. Ajuste a string de conexão em `appsettings.json` ou através de variáveis de ambiente. Para execução local rápida, recomendo SQLite.

    3. Restaurar dependências e executar:

    ```bash
    dotnet restore
    dotnet run
    ```

    4. Caso o projeto use EF Core migrations, aplique-as (ex.: criar e aplicar uma migration):

    ```bash
    # criar uma migration (se necessário)
    dotnet ef migrations add InitialCreate

    # aplicar migrations no banco
    dotnet ef database update
    ```

    Se a migration já existe no repositório, apenas execute `dotnet ef database update`.

    Observações:
    - A API costuma expor a rota base `https://localhost:7002/api` (ver `launchSettings.json`).

    ## Executando o frontend
    1. Entre na pasta `web`:

    ```bash
    cd web
    ```

    2. Instale dependências e rode em modo dev:

    ```bash
    npm install
    npm run dev
    ```

    3. O frontend consome a API em `https://localhost:7002/api` por padrão. Altere `src/services/api.ts` se necessário.

    ## Como testar rapidamente as regras de negócio
    - Crie uma pessoa maior de 18 anos e uma categoria com finalidade `Income` (ex.: `Salário`). No formulário de transação, selecione `Tipo: Receita` para ver a categoria.
    - Se a pessoa for menor de 18 anos, o frontend impede a seleção de `Receita`.
    - As categorias exibidas no formulário são filtradas pela finalidade compatível com o tipo selecionado.

    ## Decisões e notas técnicas
    - Tipos (`TransactionType`, `CategoryPurpose`) são representados como constantes de string no frontend para manter compatibilidade com o backend.
    - O frontend permite criar transações desde que exista pelo menos uma pessoa e alguma categoria cadastrada (não exige que haja categorias compatíveis até que o usuário escolha o tipo).

    ## Endpoints principais
    - `GET /api/persons` — listar pessoas
    - `POST /api/persons` — criar pessoa
    - `DELETE /api/persons/{id}` — deletar pessoa (e transações relacionadas)
    - `GET /api/categories` — listar categorias
    - `POST /api/categories` — criar categoria
    - `GET /api/transactions` — listar transações
    - `POST /api/transactions` — criar transação
    - `PUT /api/transactions/{id}` — atualizar transação
    - `GET /api/persons/totals` — totais por pessoa
    - `GET /api/categories/totals` — totais por categoria (se implementado)

    ## Possíveis melhorias
    - Selecionar automaticamente o `Tipo` ao escolher uma categoria ou exibir categorias incompatíveis desabilitadas.
    - Adicionar script de seed para dados de exemplo.
    - Adicionar testes automatizados, CI/CD e Docker.

    ## Contribuição
    - Abra uma issue ou PR com a mudança proposta.

    ## Licença
    - (Adicione aqui a licença desejada, ex.: MIT)

    ## Contato
    - Se quiser, informo caminhos exatos do backend no repo e ajusto o README para comandos/migrations específicos.
