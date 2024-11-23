# Projeto de API de Gerenciamento de Tarefas

## Detalhamento da Solução

Neste projeto, utilizei:

- **Linguagem de programação**: C# com ASP.NET Core para o desenvolvimento da API.
- **Biblioteca MediatR**: Para implementar o padrão CQRS, facilitando o desenvolvimento do CRUD.
- **ORM**: Entity Framework para gerenciar a persistência dos dados.
- **Banco de Dados**: SQL Server, devido à familiaridade com essa tecnologia.

---

## Instruções para Execução da Aplicação

Para executar a aplicação, siga os passos abaixo:

1. Navegue até a raiz do projeto da API, onde estão localizados os arquivos `Dockerfile` e `docker-compose.yml`.
2. Execute o seguinte comando no terminal:

   ```bash
   docker-compose up --build

## Perguntas ao PO

Não ficou claro se o relatório com a media de tarefas concluídas por usuário seria por projeto ou seria um relatório geral, se for um relatório geral, um relatório por projeto seria uma possível melhoria?

É importante para os usuários do sistema a criação de dashboards ou relatórios mais complexos?
Se sim poderia ser incluído no sistema a funcionalidade de gerar esse tipo de relatório

## Possíveis melhorias

- Uma possível melhoria seria criar um repository genérico e utilizar injeção de dependência para reduzir o acoplamento
- Também pode ser possível usar cache nas queries
