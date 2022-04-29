﻿using Banking.Account.Query.Domain;
using MediatR;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountById
{
    public class FindAcccountByIdQuery : IRequest<BankAccount>
    {
        public string Identifier { get; set; } = string.Empty;
    }
}
