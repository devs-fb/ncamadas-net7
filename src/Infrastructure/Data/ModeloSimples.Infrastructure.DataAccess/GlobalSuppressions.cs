﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2016:Forward the 'CancellationToken' parameter to methods", Justification = "<Pending>", Scope = "member", Target = "~M:ModeloSimples.Infrastructure.DataAccess.EfRepository`1.GetByIdAsync(System.Guid,System.Threading.CancellationToken)~System.Threading.Tasks.Task{`0}")]
[assembly: SuppressMessage("Usage", "CA2253:Named placeholders should not be numeric values", Justification = "<Pending>", Scope = "member", Target = "~M:ModeloSimples.Infrastructure.DataAccess.Queries.PessoaBuscarQuery.BuscarPessoasAsync(System.String,System.String)~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{ModeloSimples.Infrastructure.Shared.DTO.PessoaModel}}")]
[assembly: SuppressMessage("Usage", "CA2253:Named placeholders should not be numeric values", Justification = "<Pending>", Scope = "member", Target = "~M:ModeloSimples.Infrastructure.DataAccess.Queries.PessoaBuscarQuery.BuscarPessoasAsync(ModeloSimples.Infrastructure.Shared.Common.DataInfo,ModeloSimples.Infrastructure.Shared.DTO.PessoaModel)~System.Threading.Tasks.Task{ModeloSimples.Infrastructure.Shared.Common.ResultadoConsulta{System.Collections.Generic.IEnumerable{ModeloSimples.Infrastructure.Shared.DTO.PessoaModel}}}")]
[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>", Scope = "member", Target = "~M:ModeloSimples.Infrastructure.DataAccess.Queries.PessoaBuscarQuery.BuscarPessoasAsync(ModeloSimples.Infrastructure.Shared.Common.DataInfo,ModeloSimples.Infrastructure.Shared.DTO.PessoaModel)~System.Threading.Tasks.Task{ModeloSimples.Infrastructure.Shared.Common.ResultadoConsulta{System.Collections.Generic.IEnumerable{ModeloSimples.Infrastructure.Shared.DTO.PessoaModel}}}")]
[assembly: SuppressMessage("Usage", "CA2253:Named placeholders should not be numeric values", Justification = "<Pending>", Scope = "member", Target = "~M:ModeloSimples.Infrastructure.DataAccess.Queries.ObterPessoaQuery.ObterPessoaAsync(System.Guid)~System.Threading.Tasks.Task{ModeloSimples.Infrastructure.Shared.Common.ResultadoConsulta{ModeloSimples.Infrastructure.Shared.DTO.PessoaModel}}")]
[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>", Scope = "member", Target = "~M:ModeloSimples.Infrastructure.DataAccess.Queries.ObterPessoaQuery.ObterPessoaAsync(System.Guid)~System.Threading.Tasks.Task{ModeloSimples.Infrastructure.Shared.Common.ResultadoConsulta{ModeloSimples.Infrastructure.Shared.DTO.PessoaModel}}")]
[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>", Scope = "member", Target = "~M:ModeloSimples.Infrastructure.DataAccess.Queries.PessoaAuditarQuery.AuditarPessoaAsync(System.Guid)~System.Threading.Tasks.Task{ModeloSimples.Infrastructure.Shared.Common.ResultadoConsulta{System.Collections.Generic.List{ModeloSimples.Infrastructure.Shared.DTO.PessoaAuditadaModel}}}")]
[assembly: SuppressMessage("Usage", "CA2253:Named placeholders should not be numeric values", Justification = "<Pending>", Scope = "member", Target = "~M:ModeloSimples.Infrastructure.DataAccess.Queries.PessoaAuditarQuery.AuditarPessoaAsync(System.Guid)~System.Threading.Tasks.Task{ModeloSimples.Infrastructure.Shared.Common.ResultadoConsulta{System.Collections.Generic.List{ModeloSimples.Infrastructure.Shared.DTO.PessoaAuditadaModel}}}")]
