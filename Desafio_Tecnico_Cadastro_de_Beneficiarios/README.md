Visão geral
Este projeto tem como objetivo implementar uma aplicação completa para gerenciar informações de beneficiários e seus respectivos planos de saúde. A solução foi construída utilizando C#, seguindo boas práticas de desenvolvimento, organização em camadas, validações consistentes e integração com banco de dados. O sistema permite realizar operações de criação, edição, listagem e desativação de registros, garantindo controle, integridade dos dados e facilidade de manutenção.

Stack utilizada
Arquitetura em camadas.

Como rodar (local e via Docker, se houver)
Ter instalado o Visual Studio 2022 e baixar o projeto do Git Hub.

No projeto “Desafio_Tecnico.Infraestructure”, adicionei os seguintes pacotes para gerenciar o banco de dados localmente no Visual Studio - SQL Server:
	EntityFramework.SqlServer – versão 8.0
	Microsoft.EntityFrameworkCore.Design 


Como rodar testes
Em Desafio.Teste, clicar com o botão direito e selecionar "Executar Testes".


Decisões de projeto (trade-offs)
Este projeto tem como objetivo implementar uma aplicação completa para gerenciar informações de beneficiários e seus respectivos planos de saúde. A solução foi desenvolvida utilizando C# e estruturada com o conceito de arquitetura em camadas, separando claramente as responsabilidades entre Application, Core, Infraestrutura e API (Controllers).

Essa abordagem promove uma melhor organização do código, facilita a manutenção, melhora a escalabilidade e reforça a separação de responsabilidades entre regras de negócio, serviços de aplicação, acesso a dados e exposição de endpoints.

O sistema permite cadastrar, atualizar, listar e gerenciar beneficiários e seus planos de saúde, garantindo integridade dos dados, clareza nas operações e aderência a boas práticas de desenvolvimento de software.


1) Organização do código em camadas

A aplicação foi estruturada seguindo uma arquitetura em camadas para garantir organização, flexibilidade e facilidade de manutenção. Cada camada possui responsabilidades bem definidas:



Desafio_Tecnico.Core

Nesta camada ficam os elementos fundamentais da aplicação:

Enums: definições de estados e classificações utilizadas no domínio.

Models (Modelos): classes que representam as entidades principais, como Beneficiário e Plano de Saúde.



Desafio_Tecnico.Application

Responsável pela lógica de aplicação e pelo fluxo de dados entre as camadas:

DTOs: objetos de transferência de dados utilizados para entrada e saída nas operações.

Profiles (AutoMapper): mapeamentos entre Models e DTOs.

Services: regras de negócio e orquestração das operações.

Interfaces: contratos que definem o comportamento esperado dos serviços.



Desafio_Tecnico.Infraestrutura

Camada destinada a recursos técnicos e infraestrutura da aplicação:

AppContext: contexto do Entity Framework Core para acesso ao banco de dados.

Repositórios e configurações: implementação do acesso aos dados e demais recursos tecnológicos.



Desafio_Tecnico_Cadastro_de_Beneficiarios (API)

Camada responsável pela comunicação externa, onde ficam:

Controllers: gerenciamento das rotas e endpoints HTTP.

Chamadas HTTP: ações de cadastro, listagem, atualização e demais operações.

Exemplo de chamada: http://localhost:5102/api/Beneficiario


Testes Unitários
Os testes unitários foram organizados no projeto Desafio.Tests, no arquivo UnitTest1.cs.

Nessa camada de testes foram implementados cenários completos para validar o comportamento da aplicação, incluindo:

Criar Beneficiário: validação do fluxo de cadastro, regras de negócio e persistência.

Editar Beneficiário: verificação de atualizações corretas e tratamento de dados.

Remover Beneficiário: confirmação de exclusão e proteção contra inconsistências.

Pesquisar Beneficiários: testes de filtros, retorno esperado e integridade dos dados.
