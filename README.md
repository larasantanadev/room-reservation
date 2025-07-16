# RoomReservation API
Sistema de gerenciamento de **reserva de salas**, desenvolvido com arquitetura **Clean Architecture**, utilizando **.NET 9, CQRS, MediatR e FluentValidation**.  
Permite o cadastro de salas e criação de reservas com validações de capacidade e conflito de horário.

---

## 📌 Descrição do Projeto

O **RoomReservation** é uma API RESTful que resolve o problema de **gestão de reservas de salas corporativas**.  
Empresas com múltiplas salas enfrentam desafios como:
- Conflitos de agendamento.
- Excesso de ocupação.
- Falta de controle e histórico.

Esta API permite:
- Cadastrar salas com capacidade máxima.
- Criar, listar e deletar reservas com validação automática de conflitos e capacidade.
- Consultas por status, sala ou ID.

---

## 🚀 Instruções de Execução

### ✅ Pré-requisitos
- [.NET SDK 9+](https://dotnet.microsoft.com/en-us/download)
- [SQLite](https://www.sqlite.org/download.html) (ou utilize o banco local incluso)

### ⚙️ Configuração

1. Clone o repositório:
```bash
git clone https://github.com/larasantanadev/room-reservation.git
cd RoomReservation
```

2. Restaure os pacotes:
```bash
dotnet restore
```

3. Execute as migrações (opcional):
```bash
dotnet ef database update --project RoomReservation.Infrastructure
```

4. Rode a API:
```bash
dotnet run --project RoomReservation.API
```

5. Acesse o Swagger:  
https://localhost:7143/swagger ou http://localhost:5167/swagger
---

## 🧪 Executando os Testes

Para rodar todos os testes:
```bash
dotnet test
```

A estrutura dos testes segue os princípios da Clean Architecture:
- ApplicationTests: Testes de Handlers, Queries e Validators.
- ApiTests: Testes de endpoints e integração.
- InfrastructureTests: Testes de repositórios e serviços externos.

---

## 🧰 Tecnologias Utilizadas

- .NET 9  
- Entity Framework Core  
- CQRS + MediatR  
- FluentValidation  
- Polly (resiliência com retry e circuit breaker)  
- SQLite (banco local e em memória para testes)  
- Swagger / OpenAPI  
- xUnit + Moq  
- Clean Architecture  
- DDD (Domain-Driven Design)

---

## 📄 Documentação Técnica

📘 Documentação completa em PDF:  
📎 [Clique aqui para acessar](https://drive.google.com/file/d/1Y8Bw2YhYm_ShjfztQjo7EEzthcH4qH9g/view?usp=sharing)

---

## 👤 Autor

Desenvolvido por **@larasantanadev**  

