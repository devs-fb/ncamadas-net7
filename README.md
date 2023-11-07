# singlesoluction
Modelo de solução simples

## Objetivo

O objetivo é criar uma fonte colaborativa de estudos e inspiração entre colegas de trabalho, possibilitando a troca de experiências e acelerando a aquisição de maior proficiência nas técnicas profissionais. 

## Sobre

Este modelo foi desenvolvido em .NET Core 7 C#, seguindo princípios de Clean Code, SOLID e padrões de boas práticas em arquitetura, como DDD, Eventos de Domínio, Mediação, CQRS, Repositórios, entre outros.

## Ferramentas / Tecnologias

O modelo utiliza SQL Server, RabbitMQ, Redis, MassTransit como orquestrador do barramento de eventos, MediatR para mediação, Entity Framework e Dapper como principais ferramentas e tecnologias integradas no projeto.

*Recomendo* criar as seguintes contas (free):

- RabbitMQ em: https://www.cloudamqp.com/
- Redis em: https://app.redislabs.com/
- SQL Server em: https://learn.microsoft.com/pt-br/azure/azure-sql/database/free-offer?view=azuresql

## Instruções de Instalação

*Pré-requisitos:* Certifique-se de ter o .NET Core 7 instalado na sua máquina. Você pode baixá-lo em dotnet.microsoft.com.

### Clonando o Repositório

git clone https://github.com/fabiobraganet/singlesoluction.git
cd singlesoluction

### Configurando o Banco de Dados

Configure a conexão com o SQL Server no arquivo appsettings.json.

Crie uma base de dados com o nome ModeloSimples

Execute a migração para o banco de dados:

Para criar novo migration
dotnet dotnet-ef migrations add 'suaversao' --project "../../../Infrastructure/Data/ModeloSimples.Infrastructure.DataAccess/ModeloSimples.Infrastructure.DataAccess.csproj"

Para atualizar (necessario na instalacao)
dotnet dotnet-ef database update PrimeiraVersao --project "../../../Infrastructure/Data/ModeloSimples.Infrastructure.DataAccess/ModeloSimples.Infrastructure.DataAccess.csproj"

Para remover
dotnet dotnet-ef migrations remove --project "../../../Infrastructure/Data/ModeloSimples.Infrastructure.DataAccess/ModeloSimples.Infrastructure.DataAccess.csproj"

### Como Contribuir

1. Faça um fork do repositório.
2. Crie uma branch para sua contribuição: git checkout -b minha-contribuicao.
3. Faça suas alterações e commit: git commit -m 'Adicionando funcionalidade incrível'.
4. Envie suas alterações para o seu fork: git push origin minha-contribuicao.
5. Abra um pull request para revisão.
