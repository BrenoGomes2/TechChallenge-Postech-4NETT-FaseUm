**# Postech.PhaseOne.GroupEight.TechChallenge**

**## Descrição**
Este projeto é uma API mínima (Minimal API) que inclui funcionalidades para gerenciamento de contatos desenvolvido para o TechChallenge - Fase 1 da Pós-Tech FIAP para o curso de ARQUITETURA DE SISTEMAS .NET COM AZURE.

Ele está dividido em várias camadas para facilitar a organização e a manutenção do código.

**## Funcionalidades**
- Registrar contatos via endpoint `/contacts`.
- Tratamento de exceções customizadas para diferentes tipos de erros.
- Uso do MiniProfiler para profiling em ambiente de desenvolvimento.
- Documentação da API com Swagger.

**## Estrutura do Projeto**
O projeto está dividido nos seguintes subprojetos:

- **src**
  - `Postech.PhaseOne.GroupEight.TechChallenge.Api`
  - `Postech.PhaseOne.GroupEight.TechChallenge.Domain`
  - `Postech.PhaseOne.GroupEight.TechChallenge.Infra`
- **tests**
  - `Postech.PhaseOne.GroupEight.TechChallenge.FunctionalTests`
  - `Postech.PhaseOne.GroupEight.TechChallenge.IntegrationTests`
  - `Postech.PhaseOne.GroupEight.TechChallenge.UnitTests`

**## Pré-requisitos**
- .NET 8.0 ou superior
- Visual Studio 2022 ou superior / Visual Studio Code
- SQL Server (ou outro banco de dados configurado no `appsettings.json`)

**## Instalação e Execução do Projeto**
**1.** Clone o repositório
   git clone https://github.com/lucasdirani/TechChallenge-Postech-4NETT-FaseUm.git

**2.** Navegue até o diretório do projeto:
  cd TechChallenge-Postech-4NETT-FaseUm

**3.** Restaure as dependências do projeto:
  dotnet restore

**4.** Inicialize as imagens Docker:
Ainda no diretório principal '..\TechChallenge-Postech-4NETT-FaseUm', execute o comando abaixo para criar as imagens e subir os containers Docker:

docker-compose up -d

**5.** Criação das Migrations:
Para criar as migrations, execute o comando a partir do diretório raiz do projeto:

dotnet ef migrations add Initial -p .\Postech.PhaseOne.GroupEight.TechChallenge.Infra\Postech.PhaseOne.GroupEight.TechChallenge.Infra.csproj --startup-project .\Postech.PhaseOne.GroupEight.TechChallenge.Api\Postech.PhaseOne.GroupEight.TechChallenge.Api.csproj

**6.** Atualização do Banco de Dados:
Para aplicar as migrations ao banco de dados, execute o comando:

dotnet ef database update -p .\Postech.PhaseOne.GroupEight.TechChallenge.Infra\Postech.PhaseOne.GroupEight.TechChallenge.Infra.csproj --startup-project .\Postech.PhaseOne.GroupEight.TechChallenge.Api\Postech.PhaseOne.GroupEight.TechChallenge.Api.csproj

**7.**   Execute o projeto:
dotnet run --project src/Postech.PhaseOne.GroupEight.TechChallenge.Api

**## Uso**
Para registrar um novo contato, faça uma requisição POST para o endpoint /contacts com um corpo de requisição no seguinte formato:

Request:
{
  "name": "Nome do Contato",
  "email": "email@exemplo.com",
  "phone": "123456789"
}

**## Contribuição**
Faça um fork do projeto.
Crie uma nova branch para sua feature (git checkout -b feature/MinhaFeature).
Commit suas mudanças (git commit -m 'Adiciona minha feature').
Faça o push para a branch (git push origin feature/MinhaFeature).
Abra um Pull Request.
Licença
Este projeto está licenciado sob a Licença MIT - veja o arquivo LICENSE.md para mais detalhes.

**## Autores**
Breno Jhefferson Gomes
Lucas Ruiz Dirani
Lucas Montarroyos
Ricardo Fulgencio Alves
Tatiana Lima

**## Contato**
Para mais informações, entre em contato com os autores através de seus perfis no GitHub.
Breno Jhefferson Gomes - https://github.com/Brenojgomes
Lucas Ruiz Dirani - https://github.com/lucasdirani
Lucas Montarroyos Pinho - https://github.com/lucasmrpinho
Ricardo Fulgencio Alves - https://github.com/rfulgencio3
Tatiana Cardoso Lima - https://github.com/tatianacardosolima

