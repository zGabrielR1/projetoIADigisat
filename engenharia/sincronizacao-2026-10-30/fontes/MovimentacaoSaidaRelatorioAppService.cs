using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
using DigisatServer.Application.Core.Contracts.AuxiliaresModulo;
using DigisatServer.Application.Core.Projections.CadastrosProjection.Objetos;
using DigisatServer.Application.Core.Projections.CadastrosProjection.ProdutosServicos;
using DigisatServer.Application.Core.Projections.CadastrosProjection.TiposPersonalizado;
using DigisatServer.Application.Core.Projections.MovimentacoesProjection;
using DigisatServer.Application.Main.Dto.Cadastros.ProdutosServicos;
using DigisatServer.Application.Main.Dto.Movimentacoes.Relatorios;
using DigisatServer.Domain.Core;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Abastecimentos;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Sngpcs;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Cardapios;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Grupos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Linhas;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Marcas;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.OperacoesFiscal;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Pessoas;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Pessoas.Cashback;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Pessoas.Fisicas;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Pessoas.Juridicas;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Pessoas.Telefones;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Estoques;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Estoques.EstoquesMovimentacao;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Precos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Produtos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Produtos.ClassesTerapeutica;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Servicos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Promocoes;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesProdutoFornecedor;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.TiposSegmentacao;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.TributacoesEstadual.Percentuais.Interfaces;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.UnidadesMedida;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Usuarios;
using DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PagamentosRecebimentos;
using DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PagamentosRecebimentos.Parcelas;
using DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PagamentosRecebimentos.Parcelas.Recebimentos;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.DocumentosFiscaisReferenciados;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Entradas;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Itens;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.PessoasHistorico;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.ProdutosServicosHistorico;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.StatusDavOs;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Transacoes;
using DigisatServer.Infrastructure.Crosscutting;
using DigisatServer.Infrastructure.Crosscutting.Attribute;
using DigisatServer.Infrastructure.Crosscutting.Dto;
using DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios;
using DigisatServer.Infrastructure.Crosscutting.Enum;
using DigisatServer.Infrastructure.Crosscutting.Function;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Model.Atendimentos;
using DigisatServer.Infrastructure.Crosscutting.Model.ClassesTerapeuticas;
using DigisatServer.Infrastructure.Crosscutting.Model.OrigensPesquisaMovimentacao;
using DigisatServer.Infrastructure.Crosscutting.Model.StatusDavOs;
using DigisatServer.Infrastructure.Crosscutting.Model.TiposFiltroData;
using DigisatServer.Infrastructure.Crosscutting.ValueObject.CadastrosVO.CsFOP;
using DigisatServer.Infrastructure.Crosscutting.ValueObject.CadastrosVO.Municipios;
using DigisatServer.Infrastructure.Crosscutting.ValueObject.CadastrosVO.UFs;
using DigisatServer.Infrastructure.Data.Core.Interfaces;
using DigisatServer.Infrastructure.Data.Core.Specification;
using MongoDB.Bson;

namespace DigisatServer.Application.Main.AuxiliaresModulo.Services
{
	// Token: 0x020001D1 RID: 465
	public class MovimentacaoSaidaRelatorioAppService : IMovimentacaoSaidaRelatorioAppService
	{
		// Token: 0x060011BB RID: 4539 RVA: 0x0007C9B0 File Offset: 0x0007ABB0
		public MovimentacaoSaidaRelatorioAppService(IMovimentacaoRepositoryRead movimentacaoRepositoryRead, IProdutoServicoEmpresaRepositoryRead produtoServicoEmpresaRepositoryRead, IProdutoServicoRepositoryRead produtoServicoRepositoryRead, IUsuarioRepositoryRead usuarioRepositoryRead, IRepositoryRead<RegistroAbastecimento> abastecimentoRepositoryRead, IPessoaRepositoryRead pessoaRepositoryRead, ICardapioRepositoryRead cardapioRepositoryRead, IRepositoryRead<Receita> receitaRepositoryRead, IRepositoryRead<Estoque> estoqueRepositoryRead, IPrecoRepositoryRead precoRepositoryRead, IRepositoryRead<ItemCreditoDebitoCashback> itemDebitoCreditoCashbackRepositoryRead, IRepositoryRead<ItemRelacaoProdutoServicoEmpresaFornecedor> itemRelacaoProdutoServicoEmpresaFornecedorRead, IRepositoryRead<Promocao> promocaoRepositoryRead, IRepositoryRead<SubGrupo> subGrupoRepositoryRead, IRepositoryRead<Marca> marcaRepositoryRead, IRepositoryRead<Grupo> grupoRepositoryRead)
		{
			this._movimentacaoRepositoryRead = movimentacaoRepositoryRead;
			this._produtoServicoEmpresaRepositoryRead = produtoServicoEmpresaRepositoryRead;
			this._produtoServicoRepositoryRead = produtoServicoRepositoryRead;
			this._usuarioRepositoryRead = usuarioRepositoryRead;
			this._abastecimentoRepositoryRead = abastecimentoRepositoryRead;
			this._pessoaRepositoryRead = pessoaRepositoryRead;
			this._cardapioRepositoryRead = cardapioRepositoryRead;
			this._receitaRepositoryRead = receitaRepositoryRead;
			this._estoqueRepositoryRead = estoqueRepositoryRead;
			this._precoRepositoryRead = precoRepositoryRead;
			this._itemDebitoCreditoCashbackRepositoryRead = itemDebitoCreditoCashbackRepositoryRead;
			this._itemRelacaoProdutoServicoEmpresaFornecedorRead = itemRelacaoProdutoServicoEmpresaFornecedorRead;
			this._promocaoRepositoryRead = promocaoRepositoryRead;
			this._subGrupoRepositoryRead = subGrupoRepositoryRead;
			this._marcaRepositoryRead = marcaRepositoryRead;
			this._grupoRepositoryRead = grupoRepositoryRead;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0007CA40 File Offset: 0x0007AC40
		public async Task<IEnumerable<DadosRelatorioVendasPorAdministradoraCartaoDto>> ObterVendasPorAdministradoraCartao(DateTime dataInicial, DateTime dataFinal, int origem, string administradoraCartaoReferencia, string idEmpresa)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass17_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass17_0();
			CS$<>8__locals1.administradoraCartaoReferencia = administradoraCartaoReferencia;
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(idEmpresa).PorDataEmissao(dataInicial, dataFinal).PorOrigem(origem)
				.PorSituacao(2)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindAsync(specification);
			List<DadosRelatorioVendasPorAdministradoraCartaoDto> list = new List<DadosRelatorioVendasPorAdministradoraCartaoDto>();
			foreach (Movimentacao movimentacao in enumerable)
			{
				if (!movimentacao.IsNot<IMovimentacaoPagamentoRecebimento>())
				{
					IEnumerable<IParcelaRecebimentoTaxa> enumerable2 = (movimentacao as IMovimentacaoPagamentoRecebimento).PagamentoRecebimento.Parcelas.OfType<IParcelaRecebimentoTaxa>().ToListEmptyWhenNull<IParcelaRecebimentoTaxa>();
					Func<IParcelaRecebimentoTaxa, bool> func;
					if ((func = CS$<>8__locals1.<>9__0) == null)
					{
						MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass17_0 CS$<>8__locals2 = CS$<>8__locals1;
						Func<IParcelaRecebimentoTaxa, bool> func2 = (IParcelaRecebimentoTaxa x) => x.PessoaReferencia.Equals(CS$<>8__locals1.administradoraCartaoReferencia.TryParseReferencia());
						CS$<>8__locals2.<>9__0 = func2;
						func = func2;
					}
					List<IParcelaRecebimentoTaxa> list2 = enumerable2.Where<IParcelaRecebimentoTaxa>(func).ToListEmptyWhenNull<IParcelaRecebimentoTaxa>();
					if (list2.Any<IParcelaRecebimentoTaxa>())
					{
						List<DadosRelatorioVendasPorAdministradoraCartaoDto> list3 = list;
						DadosRelatorioVendasPorAdministradoraCartaoDto dadosRelatorioVendasPorAdministradoraCartaoDto = new DadosRelatorioVendasPorAdministradoraCartaoDto();
						dadosRelatorioVendasPorAdministradoraCartaoDto.Origem = movimentacao.Tipo.RetornarDescricao();
						dadosRelatorioVendasPorAdministradoraCartaoDto.Numero = movimentacao.Numero;
						DocumentoFiscal documentoFiscal = movimentacao.To<DocumentoFiscal>();
						dadosRelatorioVendasPorAdministradoraCartaoDto.Serie = ((documentoFiscal != null) ? documentoFiscal.Serie.ToStringEmptyWhenNull() : null) ?? string.Empty;
						dadosRelatorioVendasPorAdministradoraCartaoDto.DataEmissao = movimentacao.DataHoraEmissao;
						DocumentoFiscalEletronico documentoFiscalEletronico = movimentacao.To<DocumentoFiscalEletronico>();
						dadosRelatorioVendasPorAdministradoraCartaoDto.ChaveAcesso = ((documentoFiscalEletronico != null) ? documentoFiscalEletronico.ChaveAcesso.ToStringEmptyWhenNull() : null) ?? string.Empty;
						dadosRelatorioVendasPorAdministradoraCartaoDto.PessoaNome = movimentacao.Pessoa.Nome.ToStringEmptyWhenNull();
						dadosRelatorioVendasPorAdministradoraCartaoDto.PessoaDocumento = movimentacao.Pessoa.CpfCnpjFormatado.ToStringEmptyWhenNull();
						dadosRelatorioVendasPorAdministradoraCartaoDto.TotalMovimentacao = movimentacao.Total;
						dadosRelatorioVendasPorAdministradoraCartaoDto.TotalCartao = list2.Sum<IParcelaRecebimentoTaxa>((IParcelaRecebimentoTaxa parcela) => parcela.Valor);
						list3.Add(dadosRelatorioVendasPorAdministradoraCartaoDto);
					}
				}
			}
			return list;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0007CAB0 File Offset: 0x0007ACB0
		public async Task<IEnumerable<DadosRelatorioTotalMovimentacoesPorMesPorVendedorDto>> ObterTotalMovimentacoesPorMesPorVendedor(int ano, CodigoDescricaoDto origem, CodigoDescricaoDto situacao, string idVendedor, string idEmpresa)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass18_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass18_0();
			CS$<>8__locals1.idVendedor = idVendedor;
			DateTime dateTime = new DateTime(ano, 1, 1);
			DateTime dateTime2 = new DateTime(ano, 12, 31);
			ISpecification<Movimentacao> neSpec = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(idEmpresa).PorDataEmissao(dateTime, dateTime2).PorSituacao(situacao.Codigo)
				.PorOrigem(origem.Codigo)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			if (!string.IsNullOrEmpty(CS$<>8__locals1.idVendedor))
			{
				neSpec = neSpec.PorVendedor(CS$<>8__locals1.idVendedor);
			}
			CS$<>8__locals1.acumuladoGlobal = new ConcurrentDictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase);
			int skip = 0;
			List<Movimentacao> list;
			do
			{
				list = (await this._movimentacaoRepositoryRead.FindPagedAsync(neSpec.SatisfiedBy(), skip, 500)).ToList<Movimentacao>();
				skip += 500;
				if (!list.Any<Movimentacao>())
				{
					break;
				}
				IEnumerable<Movimentacao> enumerable = list;
				ParallelOptions parallelOptions = new ParallelOptions();
				parallelOptions.MaxDegreeOfParallelism = Math.Max(2, Environment.ProcessorCount / 2);
				Parallel.ForEach<Movimentacao, Dictionary<string, decimal[]>>(enumerable, parallelOptions, () => new Dictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase), delegate(Movimentacao movimentacao, ParallelLoopState loopState, Dictionary<string, decimal[]> localDict)
				{
					foreach (IItem item2 in movimentacao.Itens.Where<IItem>(delegate(IItem item)
					{
						if (!item.Cancelado)
						{
							IItemDocumentoAuxiliar itemDocumentoAuxiliar = item.To<IItemDocumentoAuxiliar>();
							if (itemDocumentoAuxiliar == null || itemDocumentoAuxiliar.OperacaoFiscal.MovimentaEstoque)
							{
								IItemDocumentoAuxiliar itemDocumentoAuxiliar2 = item.To<IItemDocumentoAuxiliar>();
								return itemDocumentoAuxiliar2 == null || itemDocumentoAuxiliar2.OperacaoFiscal.MovimentaFinanceiro;
							}
						}
						return false;
					}))
					{
						string text = item2.VendedorReferencia.ToStringEmptyWhenDefault();
						if (string.IsNullOrEmpty(CS$<>8__locals1.idVendedor) || !(text != CS$<>8__locals1.idVendedor))
						{
							int month = movimentacao.DataHoraEmissao.Month;
							decimal[] array;
							if (!localDict.TryGetValue(text, out array))
							{
								array = new decimal[12];
								localDict[text] = array;
							}
							array[month - 1] += item2.TotalLiquido;
						}
					}
					return localDict;
				}, delegate(Dictionary<string, decimal[]> localDict)
				{
					using (Dictionary<string, decimal[]>.Enumerator enumerator2 = localDict.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<string, decimal[]> kvp2 = enumerator2.Current;
							CS$<>8__locals1.acumuladoGlobal.AddOrUpdate(kvp2.Key, kvp2.Value, delegate(string key, decimal[] existing)
							{
								for (int i = 0; i < 12; i++)
								{
									existing[i] += kvp2.Value[i];
								}
								return existing;
							});
						}
					}
				});
			}
			while (list.Any<Movimentacao>());
			IEnumerable<DadosRelatorioTotalMovimentacoesPorMesPorVendedorDto> enumerable2;
			if (!CS$<>8__locals1.acumuladoGlobal.Any<KeyValuePair<string, decimal[]>>())
			{
				enumerable2 = Enumerable.Empty<DadosRelatorioTotalMovimentacoesPorMesPorVendedorDto>();
			}
			else
			{
				IEnumerable<ObjectId> enumerable3 = from id in CS$<>8__locals1.acumuladoGlobal.Keys
					where !string.IsNullOrEmpty(id)
					select id into x
					select x.TryParseReferencia();
				MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
				IRepositoryRead<Pessoa> pessoaRepositoryRead = this._pessoaRepositoryRead;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Pessoa), "x");
				CS$<>8__locals2.vendedores = pessoaRepositoryRead.GetIn<ObjectId>(Expression.Lambda<Func<Pessoa, ObjectId>>(Expression.Property(parameterExpression, methodof(EntityBase.get_Id())), new ParameterExpression[] { parameterExpression }), enumerable3).ToDictionary<Pessoa, ObjectId, string>((Pessoa v) => v.Id, (Pessoa v) => v.Nome.ToStringEmptyWhenNull());
				enumerable2 = CS$<>8__locals1.acumuladoGlobal.Select<KeyValuePair<string, decimal[]>, DadosRelatorioTotalMovimentacoesPorMesPorVendedorDto>(delegate(KeyValuePair<string, decimal[]> kvp)
				{
					string key = kvp.Key;
					decimal[] value = kvp.Value;
					string text2 = CS$<>8__locals1.vendedores.GetValueOrDefault(key.TryParseReferencia()) ?? "Não informado";
					return new DadosRelatorioTotalMovimentacoesPorMesPorVendedorDto
					{
						VendedorId = key,
						VendedorNome = text2,
						Janeiro = value[0],
						Fevereiro = value[1],
						Marco = value[2],
						Abril = value[3],
						Maio = value[4],
						Junho = value[5],
						Julho = value[6],
						Agosto = value[7],
						Setembro = value[8],
						Outubro = value[9],
						Novembro = value[10],
						Dezembro = value[11]
					};
				});
			}
			return enumerable2;
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0007CB20 File Offset: 0x0007AD20
		public async Task<IEnumerable<DadosRelatorioItensMovimentadosPorVendedorDto>> ObterItensMovimentadosPorVendedor(DateTime dataInicial, DateTime dataFinal, CodigoDescricaoDto origem, CodigoDescricaoDto situacao, string idVendedor, string idEmpresa)
		{
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(idEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorSituacao(situacao.Codigo)
				.PorOrigem(origem.Codigo)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			if (!string.IsNullOrEmpty(idVendedor))
			{
				specification = specification.PorVendedor(idVendedor);
			}
			IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindAsync(specification);
			List<DadosRelatorioItensMovimentadosPorVendedorDto> list = enumerable.SelectMany<Movimentacao, DadosRelatorioItensMovimentadosPorVendedorDto>((Movimentacao movimentacao) => movimentacao.Itens.Where<IItem>((IItem item) => !item2.VendedorReferencia.IsDefaultOrEmpty() && !item2.Cancelado).Select<IItem, DadosRelatorioItensMovimentadosPorVendedorDto>(delegate(IItem item)
			{
				DadosRelatorioItensMovimentadosPorVendedorDto dadosRelatorioItensMovimentadosPorVendedorDto = new DadosRelatorioItensMovimentadosPorVendedorDto();
				dadosRelatorioItensMovimentadosPorVendedorDto.Origem = movimentacao.Tipo.RetornarDescricao();
				DocumentoFiscalEletronico documentoFiscalEletronico = movimentacao.To<DocumentoFiscalEletronico>();
				dadosRelatorioItensMovimentadosPorVendedorDto.NaturezaOperacao = ((documentoFiscalEletronico != null) ? documentoFiscalEletronico.NaturezaOperacao.Descricao : null) ?? string.Empty;
				dadosRelatorioItensMovimentadosPorVendedorDto.Data = movimentacao.DataHoraEmissao;
				DocumentoFiscal documentoFiscal = movimentacao.To<DocumentoFiscal>();
				dadosRelatorioItensMovimentadosPorVendedorDto.Modelo = ((documentoFiscal != null) ? documentoFiscal.Modelo.Modelo : null) ?? string.Empty;
				dadosRelatorioItensMovimentadosPorVendedorDto.NumeroMovimentacao = movimentacao.Numero;
				DocumentoFiscal documentoFiscal2 = movimentacao.To<DocumentoFiscal>();
				dadosRelatorioItensMovimentadosPorVendedorDto.Serie = ((documentoFiscal2 != null) ? documentoFiscal2.Serie : null) ?? string.Empty;
				PessoaHistorico pessoa2 = movimentacao.Pessoa;
				dadosRelatorioItensMovimentadosPorVendedorDto.Pessoa = ((pessoa2 != null) ? pessoa2.Nome.ToStringEmptyWhenNull() : null) ?? string.Empty;
				dadosRelatorioItensMovimentadosPorVendedorDto.VendedorId = item2.VendedorReferencia.ToStringEmptyWhenDefault();
				dadosRelatorioItensMovimentadosPorVendedorDto.NumeroItem = item2.Numero;
				dadosRelatorioItensMovimentadosPorVendedorDto.Codigo = item2.ProdutoServico.CodigoInterno.ToStringEmptyWhenNull();
				dadosRelatorioItensMovimentadosPorVendedorDto.Descricao = item2.ProdutoServico.Descricao.ToStringEmptyWhenNull();
				dadosRelatorioItensMovimentadosPorVendedorDto.Quantidade = item2.Quantidade;
				dadosRelatorioItensMovimentadosPorVendedorDto.PrecoUnitario = item2.PrecoUnitario;
				dadosRelatorioItensMovimentadosPorVendedorDto.Desconto = item2.TotalDesconto;
				dadosRelatorioItensMovimentadosPorVendedorDto.Outras = item2.TotalOutrasDespesas;
				dadosRelatorioItensMovimentadosPorVendedorDto.TotalLiquido = item2.TotalLiquido;
				dadosRelatorioItensMovimentadosPorVendedorDto.Cancelado = item2.Cancelado;
				return dadosRelatorioItensMovimentadosPorVendedorDto;
			})).ToListEmptyWhenNull<DadosRelatorioItensMovimentadosPorVendedorDto>();
			IEnumerable<DadosRelatorioItensMovimentadosPorVendedorDto> enumerable2;
			if (!list.Any<DadosRelatorioItensMovimentadosPorVendedorDto>())
			{
				enumerable2 = Enumerable.Empty<DadosRelatorioItensMovimentadosPorVendedorDto>();
			}
			else
			{
				IEnumerable<ObjectId> enumerable3 = enumerable.SelectMany<Movimentacao, ObjectId>((Movimentacao m) => m.Itens.Select<IItem, ObjectId>((IItem item) => item2.VendedorReferencia));
				IRepositoryRead<Pessoa> pessoaRepositoryRead = this._pessoaRepositoryRead;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Pessoa), "x");
				IEnumerable<Pessoa> @in = pessoaRepositoryRead.GetIn<ObjectId>(Expression.Lambda<Func<Pessoa, ObjectId>>(Expression.Property(parameterExpression, methodof(EntityBase.get_Id())), new ParameterExpression[] { parameterExpression }), enumerable3);
				using (List<DadosRelatorioItensMovimentadosPorVendedorDto>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DadosRelatorioItensMovimentadosPorVendedorDto item2 = enumerator.Current;
						Pessoa pessoa = @in.FirstOrDefault<Pessoa>((Pessoa vendedor) => vendedor.Id.Equals(item2.VendedorId.TryParseReferencia()));
						if (!pessoa.IsNull())
						{
							item2.VendedorNome = pessoa.Nome.ToStringEmptyWhenNull();
						}
					}
				}
				IEnumerable<DadosRelatorioItensMovimentadosPorVendedorDto> enumerable4;
				if (string.IsNullOrEmpty(idVendedor))
				{
					enumerable4 = list.OrderBy<DadosRelatorioItensMovimentadosPorVendedorDto, DateTime>((DadosRelatorioItensMovimentadosPorVendedorDto item) => item.Data);
				}
				else
				{
					enumerable4 = from item in list
						where item.VendedorId.Equals(idVendedor)
						orderby item.Data
						select item;
				}
				enumerable2 = enumerable4;
			}
			return enumerable2;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0007CB98 File Offset: 0x0007AD98
		public async Task<IEnumerable<LucroMovimentacaoDto>> LucroMovimentacao(DateTime dataInicial, DateTime dataFinal, CodigoDescricaoDto opcaoPreco, string pessoaReferencia, string vendedorReferencia, int origem, int codigoSituacao, int codigoNaturezaOperacao, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorSituacao(codigoSituacao).PorOrigem(origem)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
				.PorVendedor(vendedorReferencia)
				.PorPessoa(pessoaReferencia)
				.PorData(new Emissao(), dataInicial, dataFinal);
			if (codigoNaturezaOperacao > 0)
			{
				specification = specification.PorNaturezaOperacao(codigoNaturezaOperacao);
			}
			IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindSemXmlHistoricosAsync(specification);
			IEnumerable<ObjectId> enumerable2 = enumerable.SelectMany<Movimentacao, ObjectId>((Movimentacao x) => x.ItensBase.Select<ItemBase, ObjectId>((ItemBase i) => i.ProdutoServicoEmpresaReferencia)).Distinct<ObjectId>();
			IRepositoryRead<ProdutoServicoEmpresa> produtoServicoEmpresaRepositoryRead = this._produtoServicoEmpresaRepositoryRead;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(ProdutoServicoEmpresa), "x");
			IEnumerable<ProdutoServicoEmpresa> @in = produtoServicoEmpresaRepositoryRead.GetIn<ObjectId>(Expression.Lambda<Func<ProdutoServicoEmpresa, ObjectId>>(Expression.Property(parameterExpression, methodof(EntityBase.get_Id())), new ParameterExpression[] { parameterExpression }), enumerable2);
			IDictionary<ObjectId, ProdutoServicoEmpresa> dictionary = MovimentacaoSaidaRelatorioAppService.PorId(@in);
			IDictionary<ObjectId, List<PrecoHistorico>> dictionary2 = ((opcaoPreco.Codigo == 4) ? this.ObterHistoricosPrecoCusto(@in) : new Dictionary<ObjectId, List<PrecoHistorico>>());
			List<LucroMovimentacaoDto> list = new List<LucroMovimentacaoDto>();
			foreach (Movimentacao movimentacao in enumerable)
			{
				if (!movimentacao.Itens.OfType<IItemDocumentoAuxiliar>().All<IItemDocumentoAuxiliar>(delegate(IItemDocumentoAuxiliar x)
				{
					OperacaoFiscal operacaoFiscal = x.OperacaoFiscal;
					if (operacaoFiscal == null || !operacaoFiscal.MovimentaEstoque)
					{
						OperacaoFiscal operacaoFiscal2 = x.OperacaoFiscal;
						return operacaoFiscal2 == null || !operacaoFiscal2.MovimentaFinanceiro;
					}
					return false;
				}))
				{
					List<LucroMovimentacaoItemDto> list2 = new List<LucroMovimentacaoItemDto>();
					foreach (IItemDocumentoAuxiliar itemDocumentoAuxiliar in from i in movimentacao.Itens.OfType<IItemDocumentoAuxiliar>()
						where !i.Cancelado
						select i)
					{
						if (!string.IsNullOrEmpty(vendedorReferencia))
						{
							IItemSaidaProduto itemSaidaProduto = itemDocumentoAuxiliar as IItemSaidaProduto;
							if (itemSaidaProduto.Vendedor.IsNull() || !itemSaidaProduto.Vendedor.PessoaReferencia.Equals(vendedorReferencia.TryParseReferencia()))
							{
								continue;
							}
						}
						ProdutoServicoEmpresa produtoServicoEmpresa = MovimentacaoSaidaRelatorioAppService.ObterProdutoServico(dictionary, itemDocumentoAuxiliar.ProdutoServicoEmpresaReferencia);
						decimal num = ((produtoServicoEmpresa is ServicoEmpresa) ? produtoServicoEmpresa.ValorCusto : ((opcaoPreco.Codigo == 1) ? produtoServicoEmpresa.ValorCusto : ((opcaoPreco.Codigo == 4) ? MovimentacaoSaidaRelatorioAppService.RetornarPrecoCustoAnteriorADataMovimentacao(dictionary2, itemDocumentoAuxiliar.DataHora, produtoServicoEmpresa.Preco.Id) : ((opcaoPreco.Codigo == 2) ? produtoServicoEmpresa.To<ProdutoEmpresa>().ValorCustoMedio : produtoServicoEmpresa.To<ProdutoEmpresa>().PrecoUltimaCompra))));
						decimal totalLiquido = itemDocumentoAuxiliar.TotalLiquido;
						decimal num2 = num * itemDocumentoAuxiliar.Quantidade;
						list2.Add(new LucroMovimentacaoItemDto
						{
							Codigo = itemDocumentoAuxiliar.ProdutoServico.CodigoInterno.ToStringEmptyWhenNull(),
							Descricao = itemDocumentoAuxiliar.ProdutoServico.Descricao.ToStringEmptyWhenNull(),
							Quantidade = itemDocumentoAuxiliar.Quantidade,
							Desconto = itemDocumentoAuxiliar.TotalDesconto,
							Acrescimo = itemDocumentoAuxiliar.TotalOutrasDespesas,
							Preco = itemDocumentoAuxiliar.PrecoUnitario,
							VendaTotal = totalLiquido,
							CustoTotal = num2,
							ValorLucro = totalLiquido - num2,
							PercentualLucro = ((totalLiquido > 0m) ? ((totalLiquido - num2) * 100m / totalLiquido) : 0m)
						});
					}
					List<LucroMovimentacaoDto> list3 = list;
					LucroMovimentacaoDto lucroMovimentacaoDto = new LucroMovimentacaoDto();
					lucroMovimentacaoDto.Origem = movimentacao.Tipo.RetornarDescricao();
					lucroMovimentacaoDto.Numero = movimentacao.Numero;
					DocumentoFiscal documentoFiscal = movimentacao.To<DocumentoFiscal>();
					lucroMovimentacaoDto.Modelo = ((documentoFiscal != null) ? documentoFiscal.Modelo.Modelo : null) ?? string.Empty;
					DocumentoFiscal documentoFiscal2 = movimentacao.To<DocumentoFiscal>();
					lucroMovimentacaoDto.Serie = ((documentoFiscal2 != null) ? documentoFiscal2.Serie : null) ?? string.Empty;
					lucroMovimentacaoDto.Data = movimentacao.DataHoraEmissao;
					lucroMovimentacaoDto.Pessoa = movimentacao.Pessoa.Nome.ToStringEmptyWhenNull().RemoverCaracteresNaoImprimiveis();
					IMovimentacaoVendedor movimentacaoVendedor = movimentacao.To<IMovimentacaoVendedor>();
					string text;
					if (movimentacaoVendedor == null)
					{
						text = null;
					}
					else
					{
						PessoaHistorico vendedor = movimentacaoVendedor.Vendedor;
						text = ((vendedor != null) ? vendedor.Nome.ToStringEmptyWhenNull().RemoverCaracteresNaoImprimiveis() : null);
					}
					lucroMovimentacaoDto.Vendedor = text ?? string.Empty;
					lucroMovimentacaoDto.Itens = list2.ToListEmptyWhenNull<LucroMovimentacaoItemDto>();
					list3.Add(lucroMovimentacaoDto);
				}
			}
			return list;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0007CC2C File Offset: 0x0007AE2C
		public async Task<IEnumerable<SaldoEstoqueEntregaFuturaDto>> ObterSaldoEstoqueEntregasFuturas(List<int> cfops, int codigoNaturezaOperacao, bool filtrarPorDataEmissao, DateTime dataEmissaoInicial, DateTime dataEmissaoFinal, string produtoServicoEmpresaReferencia, string grupoReferencia, int codigoSituacaoCobertura, IUsuarioIdentity usuarioIdentity)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_0();
			CS$<>8__locals1.cfops = cfops;
			CS$<>8__locals1.grupoReferencia = grupoReferencia;
			CS$<>8__locals1.produtoFiltro = (string.IsNullOrEmpty(produtoServicoEmpresaReferencia) ? ObjectId.Empty : produtoServicoEmpresaReferencia.TryParseReferencia());
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
				.PorCfopNosItens(CS$<>8__locals1.cfops);
			if (filtrarPorDataEmissao)
			{
				specification = specification.PorDataEmissao(dataEmissaoInicial, dataEmissaoFinal);
			}
			if (codigoNaturezaOperacao > 0)
			{
				specification = specification.PorNaturezaOperacao(codigoNaturezaOperacao);
			}
			if (CS$<>8__locals1.produtoFiltro != ObjectId.Empty)
			{
				specification = specification.PorProdutoServicoEmpresaReferencia(new ObjectId[] { CS$<>8__locals1.produtoFiltro });
			}
			List<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida> list = (await this._movimentacaoRepositoryRead.FindAsync(specification)).OfType<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>().ToListEmptyWhenNull<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>();
			IEnumerable<SaldoEstoqueEntregaFuturaDto> enumerable;
			if (!list.Any<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>())
			{
				enumerable = Enumerable.Empty<SaldoEstoqueEntregaFuturaDto>();
			}
			else
			{
				List<SaldoEstoqueEntregaFuturaDto> linhas = new List<SaldoEstoqueEntregaFuturaDto>();
				Dictionary<string, DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida> vendasPorChave = new Dictionary<string, DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>();
				Dictionary<string, HashSet<ObjectId>> produtosPorChave = new Dictionary<string, HashSet<ObjectId>>();
				foreach (DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida notaFiscalEletronicaSaida in list)
				{
					IEnumerable<IItemDocumentoFiscalProduto> enumerable2 = notaFiscalEletronicaSaida.Itens.OfType<IItemDocumentoFiscalProduto>();
					Func<IItemDocumentoFiscalProduto, bool> func;
					if ((func = CS$<>8__locals1.<>9__0) == null)
					{
						MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_0 CS$<>8__locals2 = CS$<>8__locals1;
						Func<IItemDocumentoFiscalProduto, bool> func2 = delegate(IItemDocumentoFiscalProduto i)
						{
							if (!i.Cancelado)
							{
								List<int> cfops2 = CS$<>8__locals1.cfops;
								OperacaoFiscal operacaoFiscal3 = i.OperacaoFiscal;
								int? num6;
								if (operacaoFiscal3 == null)
								{
									num6 = null;
								}
								else
								{
									CFOP cfop = operacaoFiscal3.Cfop;
									num6 = ((cfop != null) ? new int?(cfop.Codigo) : null);
								}
								return cfops2.Contains(num6 ?? 0);
							}
							return false;
						};
						CS$<>8__locals2.<>9__0 = func2;
						func = func2;
					}
					IEnumerable<IItemDocumentoFiscalProduto> enumerable3 = enumerable2.Where<IItemDocumentoFiscalProduto>(func);
					Func<IItemDocumentoFiscalProduto, bool> func3;
					if ((func3 = CS$<>8__locals1.<>9__1) == null)
					{
						MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_0 CS$<>8__locals3 = CS$<>8__locals1;
						Func<IItemDocumentoFiscalProduto, bool> func4 = (IItemDocumentoFiscalProduto i) => CS$<>8__locals1.produtoFiltro == ObjectId.Empty || i.ProdutoServicoEmpresaReferencia.Equals(CS$<>8__locals1.produtoFiltro);
						CS$<>8__locals3.<>9__1 = func4;
						func3 = func4;
					}
					List<IItemDocumentoFiscalProduto> list2 = enumerable3.Where<IItemDocumentoFiscalProduto>(func3).ToListEmptyWhenNull<IItemDocumentoFiscalProduto>();
					if (list2.Any<IItemDocumentoFiscalProduto>())
					{
						string text = notaFiscalEletronicaSaida.ChaveAcesso.ToStringEmptyWhenNull();
						if (!string.IsNullOrEmpty(text) && !vendasPorChave.ContainsKey(text))
						{
							vendasPorChave.Add(text, notaFiscalEletronicaSaida);
							produtosPorChave.Add(text, new HashSet<ObjectId>(list2.Select<IItemDocumentoFiscalProduto, ObjectId>((IItemDocumentoFiscalProduto i) => i.ProdutoServicoEmpresaReferencia)));
						}
						foreach (IItemDocumentoFiscalProduto itemDocumentoFiscalProduto in list2)
						{
							linhas.Add(MovimentacaoSaidaRelatorioAppService.CriarLinhaEntregaFutura(notaFiscalEletronicaSaida, notaFiscalEletronicaSaida, itemDocumentoFiscalProduto, "Venda", itemDocumentoFiscalProduto.Quantidade, 0m));
						}
					}
				}
				CS$<>8__locals1.idsVendas = new HashSet<ObjectId>(list.Select<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida, ObjectId>((DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida v) => v.Id));
				CS$<>8__locals1.vendaPorProdutoDaRemessa = new Dictionary<string, Dictionary<ObjectId, DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>>();
				HashSet<string> devolucoesContabilizadas = new HashSet<string>();
				if (vendasPorChave.Any<KeyValuePair<string, DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>>())
				{
					ISpecification<Movimentacao> specification2 = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorTipo<NotaFiscalEletronica>()
						.PorDfeReferenciado(vendasPorChave.Keys.ToList<string>());
					foreach (NotaFiscalEletronica notaFiscalEletronica in (from x in (await this._movimentacaoRepositoryRead.FindAsync(specification2)).OfType<NotaFiscalEletronica>()
						where !CS$<>8__locals1.idsVendas.Contains(x.Id)
						select x).ToListEmptyWhenNull<NotaFiscalEletronica>())
					{
						bool flag = notaFiscalEletronica is DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida;
						if (flag || notaFiscalEletronica is DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Entradas.NotaFiscalEletronicaEntrada)
						{
							List<string> list3 = (from r in notaFiscalEletronica.DocumentosFiscaisReferenciados.ToListEmptyWhenNull<IDocumentoFiscalReferenciado>().OfType<ChaveAcessoReferenciada>()
								select r.ChaveAcesso).Where<string>(new Func<string, bool>(vendasPorChave.ContainsKey)).Distinct<string>().ToListEmptyWhenNull<string>();
							foreach (IItemDocumentoFiscalProduto itemDocumentoFiscalProduto2 in from i in notaFiscalEletronica.Itens.OfType<IItemDocumentoFiscalProduto>()
								where !i.Cancelado
								select i)
							{
								if (!(CS$<>8__locals1.produtoFiltro != ObjectId.Empty) || itemDocumentoFiscalProduto2.ProdutoServicoEmpresaReferencia.Equals(CS$<>8__locals1.produtoFiltro))
								{
									if (flag)
									{
										OperacaoFiscal operacaoFiscal = itemDocumentoFiscalProduto2.OperacaoFiscal;
										if (operacaoFiscal == null || !operacaoFiscal.MovimentaEstoque)
										{
											continue;
										}
									}
									DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida notaFiscalEletronicaSaida2 = MovimentacaoSaidaRelatorioAppService.ObterVendaAlvoEntregaFutura(itemDocumentoFiscalProduto2, list3, vendasPorChave, produtosPorChave);
									if (!notaFiscalEletronicaSaida2.IsNull())
									{
										if (flag)
										{
											linhas.Add(MovimentacaoSaidaRelatorioAppService.CriarLinhaEntregaFutura(notaFiscalEletronicaSaida2, notaFiscalEletronica, itemDocumentoFiscalProduto2, "Remessa", 0m, itemDocumentoFiscalProduto2.Quantidade));
											string text2 = notaFiscalEletronica.ChaveAcesso.ToStringEmptyWhenNull();
											if (!string.IsNullOrEmpty(text2))
											{
												if (!CS$<>8__locals1.vendaPorProdutoDaRemessa.ContainsKey(text2))
												{
													CS$<>8__locals1.vendaPorProdutoDaRemessa.Add(text2, new Dictionary<ObjectId, DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>());
												}
												if (!CS$<>8__locals1.vendaPorProdutoDaRemessa[text2].ContainsKey(itemDocumentoFiscalProduto2.ProdutoServicoEmpresaReferencia))
												{
													CS$<>8__locals1.vendaPorProdutoDaRemessa[text2].Add(itemDocumentoFiscalProduto2.ProdutoServicoEmpresaReferencia, notaFiscalEletronicaSaida2);
												}
											}
										}
										else
										{
											linhas.Add(MovimentacaoSaidaRelatorioAppService.CriarLinhaEntregaFutura(notaFiscalEletronicaSaida2, notaFiscalEletronica, itemDocumentoFiscalProduto2, "Devolução de venda", itemDocumentoFiscalProduto2.Quantidade * -1m, 0m));
											devolucoesContabilizadas.Add(string.Format("{0}-{1}", notaFiscalEletronica.Id, itemDocumentoFiscalProduto2.Numero));
										}
									}
								}
							}
						}
					}
				}
				if (CS$<>8__locals1.vendaPorProdutoDaRemessa.Any<KeyValuePair<string, Dictionary<ObjectId, DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>>>())
				{
					ISpecification<Movimentacao> specification3 = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Entradas.NotaFiscalEletronicaEntrada>()
						.PorDfeReferenciado(CS$<>8__locals1.vendaPorProdutoDaRemessa.Keys.ToList<string>());
					foreach (DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Entradas.NotaFiscalEletronicaEntrada notaFiscalEletronicaEntrada in (await this._movimentacaoRepositoryRead.FindAsync(specification3)).OfType<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Entradas.NotaFiscalEletronicaEntrada>().ToListEmptyWhenNull<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Entradas.NotaFiscalEletronicaEntrada>())
					{
						List<string> list4 = (from r in notaFiscalEletronicaEntrada.DocumentosFiscaisReferenciados.ToListEmptyWhenNull<IDocumentoFiscalReferenciado>().OfType<ChaveAcessoReferenciada>()
							select r.ChaveAcesso).Where<string>(new Func<string, bool>(CS$<>8__locals1.vendaPorProdutoDaRemessa.ContainsKey)).Distinct<string>().ToListEmptyWhenNull<string>();
						using (IEnumerator<IItemDocumentoFiscalProduto> enumerator4 = (from i in notaFiscalEletronicaEntrada.Itens.OfType<IItemDocumentoFiscalProduto>()
							where !i.Cancelado
							select i).GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								IItemDocumentoFiscalProduto item = enumerator4.Current;
								if ((!(CS$<>8__locals1.produtoFiltro != ObjectId.Empty) || item.ProdutoServicoEmpresaReferencia.Equals(CS$<>8__locals1.produtoFiltro)) && !devolucoesContabilizadas.Contains(string.Format("{0}-{1}", notaFiscalEletronicaEntrada.Id, item.Numero)))
								{
									OperacaoFiscal operacaoFiscal2 = item.OperacaoFiscal;
									if (operacaoFiscal2 != null && operacaoFiscal2.MovimentaEstoque)
									{
										string text3 = item.ChaveAcessoDfeReferenciado.ToStringEmptyWhenNull();
										string text4 = ((!string.IsNullOrEmpty(text3) && CS$<>8__locals1.vendaPorProdutoDaRemessa.ContainsKey(text3)) ? text3 : (list4.FirstOrDefault<string>((string c) => CS$<>8__locals1.vendaPorProdutoDaRemessa[c].ContainsKey(item.ProdutoServicoEmpresaReferencia)) ?? list4.FirstOrDefault<string>()));
										if (!string.IsNullOrEmpty(text4))
										{
											DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida notaFiscalEletronicaSaida3;
											if (!CS$<>8__locals1.vendaPorProdutoDaRemessa[text4].TryGetValue(item.ProdutoServicoEmpresaReferencia, out notaFiscalEletronicaSaida3))
											{
												notaFiscalEletronicaSaida3 = CS$<>8__locals1.vendaPorProdutoDaRemessa[text4].Values.FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>();
											}
											if (!notaFiscalEletronicaSaida3.IsNull())
											{
												linhas.Add(MovimentacaoSaidaRelatorioAppService.CriarLinhaEntregaFutura(notaFiscalEletronicaSaida3, notaFiscalEletronicaEntrada, item, "Devolução de remessa", 0m, item.Quantidade * -1m));
											}
										}
									}
								}
							}
						}
					}
				}
				if (!linhas.Any<SaldoEstoqueEntregaFuturaDto>())
				{
					enumerable = Enumerable.Empty<SaldoEstoqueEntregaFuturaDto>();
				}
				else
				{
					CS$<>8__locals1.referenciasProduto = linhas.Select<SaldoEstoqueEntregaFuturaDto, string>((SaldoEstoqueEntregaFuturaDto l) => l.ProdutoServicoEmpresaReferencia).Distinct<string>().Select<string, ObjectId>(new Func<string, ObjectId>(ObjectId.Parse))
						.ToListEmptyWhenNull<ObjectId>();
					IProdutoServicoEmpresaRepositoryRead produtoServicoEmpresaRepositoryRead = this._produtoServicoEmpresaRepositoryRead;
					ParameterExpression parameterExpression = Expression.Parameter(typeof(ProdutoServicoEmpresa), "x");
					List<ProdutoServicoEmpresa> list5 = produtoServicoEmpresaRepositoryRead.Find(new DirectSpecification<ProdutoServicoEmpresa>(Expression.Lambda<Func<ProdutoServicoEmpresa, bool>>(Expression.Call(Expression.Field(Expression.Constant(CS$<>8__locals1, typeof(MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_0)), fieldof(MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_0.referenciasProduto)), methodof(List<ObjectId>.Contains(T)), new Expression[] { Expression.Property(parameterExpression, methodof(EntityBase.get_Id())) }), new ParameterExpression[] { parameterExpression })), usuarioIdentity).ToListEmptyWhenNull<ProdutoServicoEmpresa>();
					CS$<>8__locals1.produtoEmpresaPorReferencia = (from x in list5
						group x by x.Id.ToString()).ToDictionary<IGrouping<string, ProdutoServicoEmpresa>, string, ProdutoServicoEmpresa>((IGrouping<string, ProdutoServicoEmpresa> g) => g.Key, (IGrouping<string, ProdutoServicoEmpresa> g) => g.First<ProdutoServicoEmpresa>());
					if (!string.IsNullOrEmpty(CS$<>8__locals1.grupoReferencia))
					{
						MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_2 CS$<>8__locals5 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_2();
						CS$<>8__locals5.CS$<>8__locals2 = CS$<>8__locals1;
						IRepositoryRead<Grupo> grupoRepositoryRead = this._grupoRepositoryRead;
						parameterExpression = Expression.Parameter(typeof(Grupo), "x");
						Grupo grupo = grupoRepositoryRead.Find(new DirectSpecification<Grupo>(Expression.Lambda<Func<Grupo, bool>>(Expression.Call(Expression.Property(parameterExpression, methodof(EntityBase.get_Id())), methodof(ObjectId.Equals(ObjectId)), new Expression[] { Expression.Call(null, methodof(ObjectId.Parse(string)), new Expression[] { Expression.Field(Expression.Field(Expression.Constant(CS$<>8__locals5, typeof(MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_2)), fieldof(MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_2.CS$<>8__locals2)), fieldof(MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass21_0.grupoReferencia)) }) }), new ParameterExpression[] { parameterExpression }))).FirstOrDefault<Grupo>();
						CS$<>8__locals5.subGruposDoGrupo = new HashSet<ObjectId>(grupo.IsNull() ? Enumerable.Empty<ObjectId>() : grupo.SubGruposReferencia);
						linhas = linhas.Where<SaldoEstoqueEntregaFuturaDto>(delegate(SaldoEstoqueEntregaFuturaDto l)
						{
							ProdutoServicoEmpresa produtoServicoEmpresa2;
							return CS$<>8__locals5.CS$<>8__locals2.produtoEmpresaPorReferencia.TryGetValue(l.ProdutoServicoEmpresaReferencia, out produtoServicoEmpresa2) && CS$<>8__locals5.subGruposDoGrupo.Contains(produtoServicoEmpresa2.ProdutoServico.SubGrupoReferencia);
						}).ToListEmptyWhenNull<SaldoEstoqueEntregaFuturaDto>();
					}
					List<SaldoEstoqueEntregaFuturaDto> list6 = new List<SaldoEstoqueEntregaFuturaDto>();
					foreach (IGrouping<string, SaldoEstoqueEntregaFuturaDto> grouping in from l in linhas
						group l by l.ProdutoServicoEmpresaReferencia)
					{
						decimal num = grouping.Sum<SaldoEstoqueEntregaFuturaDto>((SaldoEstoqueEntregaFuturaDto l) => l.QuantidadeVendida) - grouping.Sum<SaldoEstoqueEntregaFuturaDto>((SaldoEstoqueEntregaFuturaDto l) => l.QuantidadeEntregue);
						if (!(CS$<>8__locals1.produtoFiltro == ObjectId.Empty) || !(num <= 0m))
						{
							ProdutoServicoEmpresa produtoServicoEmpresa;
							CS$<>8__locals1.produtoEmpresaPorReferencia.TryGetValue(grouping.Key, out produtoServicoEmpresa);
							decimal? num2;
							if (produtoServicoEmpresa == null)
							{
								num2 = null;
							}
							else
							{
								ProdutoEmpresa produtoEmpresa = produtoServicoEmpresa.To<ProdutoEmpresa>();
								if (produtoEmpresa == null)
								{
									num2 = null;
								}
								else
								{
									Estoque estoque = produtoEmpresa.Estoque;
									num2 = ((estoque != null) ? new decimal?(estoque.QuantidadeSaldo) : null);
								}
							}
							decimal num3 = num2 ?? 0m;
							decimal num4 = num3 - num;
							if ((codigoSituacaoCobertura != 2 || !(num4 >= 0m)) && (codigoSituacaoCobertura != 3 || !(num4 < 0m)))
							{
								foreach (IEnumerable<SaldoEstoqueEntregaFuturaDto> enumerable4 in from l in grouping
									group l by l.PessoaReferencia)
								{
									decimal num5 = 0m;
									foreach (SaldoEstoqueEntregaFuturaDto saldoEstoqueEntregaFuturaDto in from l in enumerable4
										orderby l.DataEmissao, l.Numero
										select l)
									{
										num5 += saldoEstoqueEntregaFuturaDto.SaldoLinha;
										saldoEstoqueEntregaFuturaDto.SaldoCorrente = num5;
										if (produtoServicoEmpresa.IsNotNull())
										{
											saldoEstoqueEntregaFuturaDto.CodigoInterno = produtoServicoEmpresa.ProdutoServico.CodigoInterno.ToStringEmptyWhenNull();
											saldoEstoqueEntregaFuturaDto.DescricaoProduto = produtoServicoEmpresa.ProdutoServico.Descricao.ToStringEmptyWhenNull().RemoverCaracteresNaoImprimiveis();
											UnidadeMedida unidadeMedida = produtoServicoEmpresa.ProdutoServico.UnidadeMedida;
											saldoEstoqueEntregaFuturaDto.Unidade = ((unidadeMedida != null) ? unidadeMedida.Sigla : null) ?? saldoEstoqueEntregaFuturaDto.Unidade;
										}
										saldoEstoqueEntregaFuturaDto.EstoqueDisponivel = num3;
										saldoEstoqueEntregaFuturaDto.SaldoCobertura = num4;
										list6.Add(saldoEstoqueEntregaFuturaDto);
									}
								}
							}
						}
					}
					enumerable = list6;
				}
			}
			return enumerable;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0007CCC0 File Offset: 0x0007AEC0
		private static SaldoEstoqueEntregaFuturaDto CriarLinhaEntregaFutura(DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida venda, Movimentacao documento, IItemDocumentoFiscalProduto item, string tipoMovimento, decimal quantidadeVendida, decimal quantidadeEntregue)
		{
			SaldoEstoqueEntregaFuturaDto saldoEstoqueEntregaFuturaDto = new SaldoEstoqueEntregaFuturaDto();
			PessoaHistorico pessoa = venda.Pessoa;
			saldoEstoqueEntregaFuturaDto.PessoaReferencia = ((pessoa != null) ? pessoa.PessoaReferencia.ToString() : null) ?? string.Empty;
			PessoaHistorico pessoa2 = venda.Pessoa;
			saldoEstoqueEntregaFuturaDto.NomePessoa = ((pessoa2 != null) ? pessoa2.Nome.ToStringEmptyWhenNull().RemoverCaracteresNaoImprimiveis() : null) ?? string.Empty;
			PessoaHistorico pessoa3 = venda.Pessoa;
			saldoEstoqueEntregaFuturaDto.CpfCnpjPessoa = ((pessoa3 != null) ? pessoa3.CpfCnpjFormatado : null) ?? string.Empty;
			saldoEstoqueEntregaFuturaDto.ProdutoServicoEmpresaReferencia = item.ProdutoServicoEmpresaReferencia.ToString();
			ProdutoServicoHistorico produtoServico = item.ProdutoServico;
			saldoEstoqueEntregaFuturaDto.CodigoInterno = ((produtoServico != null) ? produtoServico.CodigoInterno.ToStringEmptyWhenNull() : null) ?? string.Empty;
			ProdutoServicoHistorico produtoServico2 = item.ProdutoServico;
			saldoEstoqueEntregaFuturaDto.DescricaoProduto = ((produtoServico2 != null) ? produtoServico2.Descricao.ToStringEmptyWhenNull().RemoverCaracteresNaoImprimiveis() : null) ?? string.Empty;
			ProdutoServicoHistorico produtoServico3 = item.ProdutoServico;
			string text;
			if (produtoServico3 == null)
			{
				text = null;
			}
			else
			{
				UnidadeMedidaHistorico unidadeMedida = produtoServico3.UnidadeMedida;
				text = ((unidadeMedida != null) ? unidadeMedida.Sigla : null);
			}
			saldoEstoqueEntregaFuturaDto.Unidade = text ?? string.Empty;
			saldoEstoqueEntregaFuturaDto.DataEmissao = documento.DataHoraEmissao;
			saldoEstoqueEntregaFuturaDto.Numero = documento.Numero;
			DocumentoFiscal documentoFiscal = documento.To<DocumentoFiscal>();
			saldoEstoqueEntregaFuturaDto.Serie = ((documentoFiscal != null) ? documentoFiscal.Serie : null) ?? string.Empty;
			saldoEstoqueEntregaFuturaDto.TipoMovimento = tipoMovimento;
			OperacaoFiscal operacaoFiscal = item.OperacaoFiscal;
			int? num;
			if (operacaoFiscal == null)
			{
				num = null;
			}
			else
			{
				CFOP cfop = operacaoFiscal.Cfop;
				num = ((cfop != null) ? new int?(cfop.Codigo) : null);
			}
			saldoEstoqueEntregaFuturaDto.Cfop = num ?? 0;
			saldoEstoqueEntregaFuturaDto.Quantidade = quantidadeVendida - quantidadeEntregue;
			saldoEstoqueEntregaFuturaDto.QuantidadeVendida = quantidadeVendida;
			saldoEstoqueEntregaFuturaDto.QuantidadeEntregue = quantidadeEntregue;
			return saldoEstoqueEntregaFuturaDto;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0007CE94 File Offset: 0x0007B094
		private static DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida ObterVendaAlvoEntregaFutura(IItemDocumentoFiscalProduto item, IList<string> chavesCabecalho, IDictionary<string, DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida> vendasPorChave, IDictionary<string, HashSet<ObjectId>> produtosPorChave)
		{
			string text = item.ChaveAcessoDfeReferenciado.ToStringEmptyWhenNull();
			if (string.IsNullOrEmpty(text))
			{
				return (from c in chavesCabecalho
					where produtosPorChave[c].Contains(item.ProdutoServicoEmpresaReferencia)
					select vendasPorChave[c] into v
					orderby v.DataHoraEmissao, v.Numero
					select v).FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>();
			}
			if (!vendasPorChave.ContainsKey(text))
			{
				return null;
			}
			return vendasPorChave[text];
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0007CF64 File Offset: 0x0007B164
		public async Task<IEnumerable<VendaComLucroDto>> VendasComLucro(DateTime dataInicial, DateTime dataFinal, string pessoaReferencia, CodigoDescricaoDto opcaoPreco, IUsuarioIdentity usuarioIdentity)
		{
			return await Task.Factory.StartNew<List<VendaComLucroDto>>(delegate
			{
				ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorPessoa(pessoaReferencia).PorSituacaoConcluido()
					.PorOrigem(101)
					.PorFinalidadeEmissaoNormal()
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
					.PorNaturezaOperacao(1)
					.PorData(new Emissao(), dataInicial, dataFinal);
				IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.FindSemXmlHistoricos(specification);
				IEnumerable<ObjectId> enumerable2 = enumerable.SelectMany<Movimentacao, ObjectId>((Movimentacao x) => x.ItensBase.Select<ItemBase, ObjectId>((ItemBase i) => i.ProdutoServicoEmpresaReferencia)).Distinct<ObjectId>();
				IEnumerable<ProdutoServicoEmpresa> @in = this._produtoServicoEmpresaRepositoryRead.GetIn<ObjectId>((ProdutoServicoEmpresa x) => x.Id, enumerable2);
				IDictionary<ObjectId, ProdutoServicoEmpresa> dictionary = MovimentacaoSaidaRelatorioAppService.PorId(@in);
				IDictionary<ObjectId, List<PrecoHistorico>> dictionary3;
				if (opcaoPreco.Codigo != 4)
				{
					IDictionary<ObjectId, List<PrecoHistorico>> dictionary2 = new Dictionary<ObjectId, List<PrecoHistorico>>();
					dictionary3 = dictionary2;
				}
				else
				{
					dictionary3 = this.ObterHistoricosPrecoCusto(@in);
				}
				IDictionary<ObjectId, List<PrecoHistorico>> dictionary4 = dictionary3;
				List<VendaComLucroDto> list = new List<VendaComLucroDto>();
				foreach (Movimentacao movimentacao in enumerable)
				{
					decimal num = 0m;
					decimal num2 = 0m;
					if (!movimentacao.Itens.OfType<IItemDocumentoFiscal>().All<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => !x.OperacaoFiscal.MovimentaFinanceiro))
					{
						foreach (IItemDocumentoFiscal itemDocumentoFiscal in from x in movimentacao.Itens.OfType<IItemDocumentoFiscal>()
							where !x.Cancelado && x.OperacaoFiscal.MovimentaEstoque && x.OperacaoFiscal.MovimentaFinanceiro
							select x)
						{
							ProdutoServicoEmpresa produtoServicoEmpresa = MovimentacaoSaidaRelatorioAppService.ObterProdutoServico(dictionary, itemDocumentoFiscal.ProdutoServicoEmpresaReferencia);
							if (produtoServicoEmpresa != null)
							{
								decimal num3 = ((produtoServicoEmpresa is ServicoEmpresa) ? produtoServicoEmpresa.ValorCusto : ((opcaoPreco.Codigo == 1) ? produtoServicoEmpresa.ValorCusto : ((opcaoPreco.Codigo == 4) ? MovimentacaoSaidaRelatorioAppService.RetornarPrecoCustoAnteriorADataMovimentacao(dictionary4, itemDocumentoFiscal.DataHora, produtoServicoEmpresa.Preco.Id) : ((opcaoPreco.Codigo == 2) ? produtoServicoEmpresa.To<ProdutoEmpresa>().ValorCustoMedio : produtoServicoEmpresa.To<ProdutoEmpresa>().PrecoUltimaCompra))));
								num += itemDocumentoFiscal.TotalLiquido;
								num2 += num3 * itemDocumentoFiscal.Quantidade;
							}
						}
						VendaComLucroDto dto = new VendaComLucroDto
						{
							Cnpj = movimentacao.Empresa.CpfCnpjFormatado.ToStringEmptyWhenNull(),
							Empresa = movimentacao.Empresa.Nome.ToStringEmptyWhenNull().RemoverCaracteresNaoImprimiveis(),
							Data = movimentacao.DataHoraEmissao,
							Pessoa = movimentacao.Pessoa.Nome.ToStringEmptyWhenNull().RemoverCaracteresNaoImprimiveis(),
							Numero = movimentacao.Numero,
							Origem = movimentacao.Tipo.RetornarDescricao(),
							CustoTotal = num2,
							VendaTotal = num,
							ValorLucro = num - num2,
							PercentualLucro = ((num > 0m) ? ((num - num2) * 100m / ((num2 == 0m) ? 1m : num2)) : 0m)
						};
						movimentacao.ToDo<DocumentoFiscal>(delegate(DocumentoFiscal documentoFiscal)
						{
							dto.Data = documentoFiscal.ObterDataEfetivacao();
							dto.Modelo = documentoFiscal.Modelo.Modelo.ToStringEmptyWhenNull();
							dto.Serie = documentoFiscal.Serie.ToStringEmptyWhenNull();
						});
						list.Add(dto);
					}
				}
				return list;
			});
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0007CFD4 File Offset: 0x0007B1D4
		public async Task<IEnumerable<LucroProdutoVendidoDto>> LucroProdutoVendido(DateTime dataInicial, DateTime dataFinal, string produtoServicoEmpresaReferencia, CodigoDescricaoDto opcaoPreco, IUsuarioIdentity usuarioIdentity)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass25_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.usuarioIdentity = usuarioIdentity;
			CS$<>8__locals1.dataInicial = dataInicial;
			CS$<>8__locals1.dataFinal = dataFinal;
			CS$<>8__locals1.produtoServicoEmpresaReferencia = produtoServicoEmpresaReferencia;
			CS$<>8__locals1.opcaoPreco = opcaoPreco;
			return await Task.Factory.StartNew<List<LucroProdutoVendidoDto>>(delegate
			{
				MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass25_1 CS$<>8__locals2 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass25_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(CS$<>8__locals1.usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorOrigem(101)
					.PorData(new Emissao(), CS$<>8__locals1.dataInicial, CS$<>8__locals1.dataFinal);
				if (!string.IsNullOrEmpty(CS$<>8__locals1.produtoServicoEmpresaReferencia))
				{
					specification = specification.PorProdutoServicoEmpresaReferencia(new List<ObjectId> { CS$<>8__locals1.produtoServicoEmpresaReferencia.TryParseReferencia() });
				}
				IEnumerable<Movimentacao> enumerable = CS$<>8__locals1.<>4__this._movimentacaoRepositoryRead.FindSemXmlHistoricos(specification);
				IEnumerable<ObjectId> enumerable2 = enumerable.SelectMany<Movimentacao, ObjectId>((Movimentacao x) => x.ItensBase.Select<ItemBase, ObjectId>((ItemBase i) => i.ProdutoServicoEmpresaReferencia)).Distinct<ObjectId>();
				IEnumerable<ProdutoServicoEmpresa> @in = CS$<>8__locals1.<>4__this._produtoServicoEmpresaRepositoryRead.GetIn<ObjectId>((ProdutoServicoEmpresa x) => x.Id, enumerable2);
				CS$<>8__locals2.produtosServicosPorId = MovimentacaoSaidaRelatorioAppService.PorId(@in);
				MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass25_1 CS$<>8__locals3 = CS$<>8__locals2;
				IDictionary<ObjectId, List<PrecoHistorico>> dictionary2;
				if (CS$<>8__locals1.opcaoPreco.Codigo != 4)
				{
					IDictionary<ObjectId, List<PrecoHistorico>> dictionary = new Dictionary<ObjectId, List<PrecoHistorico>>();
					dictionary2 = dictionary;
				}
				else
				{
					dictionary2 = CS$<>8__locals1.<>4__this.ObterHistoricosPrecoCusto(@in);
				}
				CS$<>8__locals3.historicosPrecoCusto = dictionary2;
				CS$<>8__locals2.subGrupos = CS$<>8__locals1.<>4__this._subGrupoRepositoryRead.GetIn<ObjectId>((SubGrupo x) => x.Id, @in.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa p) => p.ProdutoServico.SubGrupoReferencia));
				IEnumerable<ProdutoEmpresa> enumerable3 = @in.OfType<ProdutoEmpresa>();
				CS$<>8__locals2.marcas = CS$<>8__locals1.<>4__this._marcaRepositoryRead.GetIn<ObjectId?>((Marca x) => (ObjectId?)x.Id, enumerable3.Select<ProdutoEmpresa, ObjectId?>(delegate(ProdutoEmpresa p)
				{
					Marca marca = p.Produto.Marca;
					if (marca == null)
					{
						return null;
					}
					return new ObjectId?(marca.Id);
				}));
				List<LucroProdutoVendidoDto> list = new List<LucroProdutoVendidoDto>();
				using (IEnumerator<Movimentacao> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Movimentacao documentoFiscal = enumerator.Current;
						if (!documentoFiscal.Itens.OfType<IItemDocumentoFiscal>().All<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => !x.OperacaoFiscal.MovimentaFinanceiro))
						{
							List<LucroProdutoVendidoDto> list2 = list;
							IEnumerable<IItem> itens = documentoFiscal.Itens;
							var func;
							if ((func = CS$<>8__locals2.<>9__6) == null)
							{
								func = (CS$<>8__locals2.<>9__6 = (IItem item) => new
								{
									item = item,
									produtoServico = MovimentacaoSaidaRelatorioAppService.ObterProdutoServico(CS$<>8__locals2.produtosServicosPorId, item.ProdutoServicoEmpresaReferencia)
								});
							}
							var enumerable4 = itens.Select(func);
							var func2;
							if ((func2 = CS$<>8__locals2.<>9__7) == null)
							{
								func2 = (CS$<>8__locals2.<>9__7 = <>h__TransparentIdentifier0 => new
								{
									<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0,
									grupo = CS$<>8__locals2.subGrupos.FirstOrDefault<SubGrupo>(delegate(SubGrupo x)
									{
										ObjectId id = x.Id;
										ProdutoServicoEmpresa produtoServico = <>h__TransparentIdentifier0.produtoServico;
										return id.Equals((produtoServico != null) ? produtoServico.ProdutoServico.SubGrupoReferencia : default(ObjectId));
									})
								});
							}
							var enumerable5 = from <>h__TransparentIdentifier1 in enumerable4.Select(func2)
								select new
								{
									<>h__TransparentIdentifier1 = <>h__TransparentIdentifier1,
									produto = (<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.produtoServico.ProdutoServico as Produto)
								};
							var func3;
							if ((func3 = CS$<>8__locals2.<>9__10) == null)
							{
								func3 = (CS$<>8__locals2.<>9__10 = <>h__TransparentIdentifier2 => new
								{
									<>h__TransparentIdentifier2 = <>h__TransparentIdentifier2,
									marca = CS$<>8__locals2.marcas.FirstOrDefault<Marca>(delegate(Marca x)
									{
										ObjectId id2 = x.Id;
										Produto produto = <>h__TransparentIdentifier2.produto;
										ObjectId? objectId;
										if (produto == null)
										{
											objectId = null;
										}
										else
										{
											Marca marca2 = produto.Marca;
											objectId = ((marca2 != null) ? new ObjectId?(marca2.Id) : null);
										}
										return id2.Equals(objectId ?? default(ObjectId));
									})
								});
							}
							var enumerable6 = enumerable5.Select(func3);
							var func4;
							if ((func4 = CS$<>8__locals2.<>9__12) == null)
							{
								func4 = (CS$<>8__locals2.<>9__12 = <>h__TransparentIdentifier3 => new
								{
									<>h__TransparentIdentifier3 = <>h__TransparentIdentifier3,
									precoEntrada = ((<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.produtoServico is ServicoEmpresa) ? <>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.produtoServico.ValorCusto : ((CS$<>8__locals2.CS$<>8__locals1.opcaoPreco.Codigo == 1) ? <>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.produtoServico.ValorCusto : ((CS$<>8__locals2.CS$<>8__locals1.opcaoPreco.Codigo == 4) ? MovimentacaoSaidaRelatorioAppService.RetornarPrecoCustoAnteriorADataMovimentacao(CS$<>8__locals2.historicosPrecoCusto, <>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.DataHora, <>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.produtoServico.Preco.Id) : ((CS$<>8__locals2.CS$<>8__locals1.opcaoPreco.Codigo == 2) ? <>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.produtoServico.To<ProdutoEmpresa>().ValorCustoMedio : <>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.produtoServico.To<ProdutoEmpresa>().PrecoUltimaCompra))))
								});
							}
							var enumerable7 = from <>h__TransparentIdentifier4 in enumerable6.Select(func4)
								where <>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.produtoServico != null && !<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.Cancelado
								select <>h__TransparentIdentifier4;
							var func5;
							if ((func5 = CS$<>8__locals1.<>9__14) == null)
							{
								func5 = (CS$<>8__locals1.<>9__14 = <>h__TransparentIdentifier4 => string.IsNullOrEmpty(CS$<>8__locals1.produtoServicoEmpresaReferencia) || <>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.ProdutoServicoEmpresaReferencia.Equals(CS$<>8__locals1.produtoServicoEmpresaReferencia.TryParseReferencia()));
							}
							list2.AddRange((from <>h__TransparentIdentifier4 in enumerable7.Where(func5)
								let venda = item.TotalLiquido
								let custo = precoEntrada * item.Quantidade
								select new
								{
									<>h__TransparentIdentifier6 = <>h__TransparentIdentifier6,
									valorLucro = venda - custo
								}).Select(delegate(<>h__TransparentIdentifier7)
							{
								LucroProdutoVendidoDto lucroProdutoVendidoDto = new LucroProdutoVendidoDto();
								lucroProdutoVendidoDto.Cnpj = documentoFiscal.Empresa.CpfCnpjFormatado.ToStringEmptyWhenNull();
								lucroProdutoVendidoDto.Empresa = documentoFiscal.Empresa.Nome.ToStringEmptyWhenNull().RemoverCaracteresNaoImprimiveis();
								lucroProdutoVendidoDto.CustoTotal = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.custo;
								lucroProdutoVendidoDto.VendaTotal = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.venda;
								lucroProdutoVendidoDto.ValorLucro = <>h__TransparentIdentifier7.valorLucro;
								lucroProdutoVendidoDto.Descricao = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.ProdutoServico.Descricao.ToStringEmptyWhenNull();
								lucroProdutoVendidoDto.Quantidade = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.Quantidade;
								lucroProdutoVendidoDto.CodigoInterno = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.ProdutoServico.CodigoInterno.ToStringEmptyWhenNull();
								lucroProdutoVendidoDto.PrecoUnitarioVenda = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.PrecoUnitario;
								lucroProdutoVendidoDto.PrecoUnitarioCusto = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.precoEntrada;
								lucroProdutoVendidoDto.TipoProdutoServico = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.ProdutoServico.Tipo;
								SubGrupo grupo = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.grupo;
								lucroProdutoVendidoDto.Subgrupo = ((grupo != null) ? grupo.Descricao : null) ?? string.Empty;
								SubGrupo grupo2 = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.grupo;
								lucroProdutoVendidoDto.SubgrupoReferencia = ((grupo2 != null) ? grupo2.Id.ToStringEmptyWhenDefault() : null) ?? string.Empty;
								Marca marca3 = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.marca;
								lucroProdutoVendidoDto.Marca = ((marca3 != null) ? marca3.Descricao : null) ?? string.Empty;
								Marca marca4 = <>h__TransparentIdentifier7.<>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.marca;
								lucroProdutoVendidoDto.MarcaReferencia = ((marca4 != null) ? marca4.Id.ToStringEmptyWhenDefault() : null) ?? string.Empty;
								return lucroProdutoVendidoDto;
							}));
						}
					}
				}
				return (from g in list
					group g by new
					{
						g.Cnpj, g.Empresa, g.CodigoInterno, g.Descricao, g.PrecoUnitarioVenda, g.PrecoUnitarioCusto, g.TipoProdutoServico, g.Marca, g.MarcaReferencia, g.Subgrupo,
						g.SubgrupoReferencia
					}).Select(delegate(arg)
				{
					LucroProdutoVendidoDto lucroProdutoVendidoDto2 = new LucroProdutoVendidoDto();
					lucroProdutoVendidoDto2.CodigoInterno = arg.Key.CodigoInterno;
					lucroProdutoVendidoDto2.Descricao = arg.Key.Descricao;
					lucroProdutoVendidoDto2.TipoProdutoServico = arg.Key.TipoProdutoServico;
					lucroProdutoVendidoDto2.PrecoUnitarioVenda = arg.Key.PrecoUnitarioVenda;
					lucroProdutoVendidoDto2.PrecoUnitarioCusto = arg.Key.PrecoUnitarioCusto;
					lucroProdutoVendidoDto2.Quantidade = arg.Sum<LucroProdutoVendidoDto>((LucroProdutoVendidoDto x) => x.Quantidade);
					lucroProdutoVendidoDto2.CustoTotal = arg.Sum<LucroProdutoVendidoDto>((LucroProdutoVendidoDto x) => x.CustoTotal);
					lucroProdutoVendidoDto2.VendaTotal = arg.Sum<LucroProdutoVendidoDto>((LucroProdutoVendidoDto x) => x.VendaTotal);
					lucroProdutoVendidoDto2.ValorLucro = arg.Sum<LucroProdutoVendidoDto>((LucroProdutoVendidoDto x) => x.ValorLucro);
					lucroProdutoVendidoDto2.Empresa = arg.Key.Empresa;
					lucroProdutoVendidoDto2.Cnpj = arg.Key.Cnpj;
					lucroProdutoVendidoDto2.Marca = arg.Key.Marca;
					lucroProdutoVendidoDto2.MarcaReferencia = arg.Key.MarcaReferencia;
					lucroProdutoVendidoDto2.Subgrupo = arg.Key.Subgrupo;
					lucroProdutoVendidoDto2.SubgrupoReferencia = arg.Key.SubgrupoReferencia;
					return lucroProdutoVendidoDto2;
				}).ToListEmptyWhenNull<LucroProdutoVendidoDto>();
			});
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0007D044 File Offset: 0x0007B244
		public async Task<ResumoTotalizacaoEspecieDto> TotalizacaoPorEspecie(DateTime dataInicial, DateTime dataFinal, string usuarioReferencia, int origemMovimentacao, int situacaoMovimentacao, bool agruparPorDia, string centroCustoReferencia, IUsuarioIdentity usuarioIdentity)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass26_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass26_0();
			CS$<>8__locals1.usuarioReferencia = usuarioReferencia;
			CS$<>8__locals1.centroCustoReferencia = centroCustoReferencia;
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass26_0 CS$<>8__locals2 = CS$<>8__locals1;
			List<UsuarioBase> list;
			if (string.IsNullOrEmpty(CS$<>8__locals1.usuarioReferencia))
			{
				list = this._usuarioRepositoryRead.Find(UsuarioSpecifications.AtivoExcetoUsuarioSuporteERevendaEPedirMenu()).ToListEmptyWhenNull<UsuarioBase>();
			}
			else
			{
				(list = new List<UsuarioBase>()).Add(this._usuarioRepositoryRead.GetById(ObjectId.Parse(CS$<>8__locals1.usuarioReferencia)));
			}
			CS$<>8__locals2.usuarios = list;
			CS$<>8__locals1.usuarioFiltroMovimentacao = CS$<>8__locals1.usuarios.FirstOrDefault<UsuarioBase>((UsuarioBase x) => x.Id.Equals(CS$<>8__locals1.usuarioReferencia.TryParseReferencia()));
			IEnumerable<Movimentacao> enumerable3;
			if (situacaoMovimentacao == 99)
			{
				ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
					.PorNaturezaOperacao(1)
					.PorFinalidadeEmissaoNormal()
					.PorSituacao(14)
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
				IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindAsync(specification);
				IEnumerable<Movimentacao> resultadoConcluido = enumerable;
				specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
					.PorNaturezaOperacao(1)
					.PorFinalidadeEmissaoNormal()
					.PorSituacao(2)
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
				IEnumerable<Movimentacao> enumerable2 = await this._movimentacaoRepositoryRead.FindAsync(specification);
				enumerable3 = resultadoConcluido.Union<Movimentacao>(enumerable2);
				resultadoConcluido = null;
			}
			else
			{
				ISpecification<Movimentacao> specification2 = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
					.PorNaturezaOperacao(1)
					.PorFinalidadeEmissaoNormal()
					.PorSituacao(situacaoMovimentacao)
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
				enumerable3 = await this._movimentacaoRepositoryRead.FindAsync(specification2);
			}
			IEnumerable<Movimentacao> enumerable4 = ((!string.IsNullOrEmpty(CS$<>8__locals1.usuarioReferencia)) ? enumerable3.Where<Movimentacao>(delegate(Movimentacao x)
			{
				IEnumerable<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico> historicos = x.Historicos;
				Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> func5;
				if ((func5 = CS$<>8__locals1.<>9__2) == null)
				{
					func5 = (CS$<>8__locals1.<>9__2 = (DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico h) => (h.NomeUsuario.ToLower().Equals(CS$<>8__locals1.usuarioFiltroMovimentacao.Nome.ToLower()) || h.NomeUsuario.ToLower().Equals(CS$<>8__locals1.usuarioFiltroMovimentacao.AcessoAlternativo.ToLower())) && h.SituacaoMovimentacao.Codigo == 1);
				}
				return historicos.FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>(func5) != null;
			}) : enumerable3);
			CS$<>8__locals1.lista = new List<TotalizacaoEspecieDto>();
			List<TotalizacaoEspecieDto> list2 = new List<TotalizacaoEspecieDto>();
			using (IEnumerator<Movimentacao> enumerator = enumerable4.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Movimentacao movimentacao2 = enumerator.Current;
					if (!movimentacao2.IsNot<IMovimentacaoPagamento>())
					{
						if (movimentacao2.Is<IMovimentacaoPagamentoRecebimento>() && !CS$<>8__locals1.centroCustoReferencia.IsNullOrEmpty())
						{
							IEnumerable<Parcela> parcelas = movimentacao2.To<IMovimentacaoPagamentoRecebimento>().PagamentoRecebimento.Parcelas;
							Func<Parcela, bool> func;
							if ((func = CS$<>8__locals1.<>9__3) == null)
							{
								MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass26_0 CS$<>8__locals4 = CS$<>8__locals1;
								Func<Parcela, bool> func2 = (Parcela x) => x.CentroCustoCodigo.Equals(CS$<>8__locals1.centroCustoReferencia);
								CS$<>8__locals4.<>9__3 = func2;
								func = func2;
							}
							if (!parcelas.Any<Parcela>(func))
							{
								continue;
							}
						}
						List<ItemPagamento> list3 = movimentacao2.To<IMovimentacaoPagamento>().Pagamentos.ToListEmptyWhenNull<ItemPagamento>();
						IEnumerable<ItemPagamento> enumerable5 = list3.Where<ItemPagamento>((ItemPagamento x) => !(x is IItemPagamentoCartao) && !(x is ItemPagamentoCarteiraDigital) && !(x is ItemPagamentoTerminalPos));
						CS$<>8__locals1.lista.AddRange(enumerable5.Select<ItemPagamento, TotalizacaoEspecieDto>((ItemPagamento p) => new TotalizacaoEspecieDto
						{
							Codigo = p.EspeciePagamento.Codigo,
							Descricao = p.EspeciePagamento.Descricao,
							Valor = p.Valor,
							Usuario = MovimentacaoSaidaRelatorioAppService.RetornarUsuario(CS$<>8__locals1.usuarios, movimentacao2),
							Data = movimentacao2.DataHoraEmissao.Date
						}).ToListEmptyWhenNull<TotalizacaoEspecieDto>());
						list2.AddRange((from parcela in list3.Where<ItemPagamento>((ItemPagamento x) => x is IItemPagamentoCartao).OfType<IItemPagamentoCartao>()
							from tef in parcela.InformacoesTransacaoRelatorio
							select new
							{
								<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0,
								creditoDebito = (tef.Debito ? "Débito" : "Crédito")
							}).Select(delegate(<>h__TransparentIdentifier1)
						{
							TotalizacaoEspecieDto totalizacaoEspecieDto = new TotalizacaoEspecieDto();
							totalizacaoEspecieDto.Codigo = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.parcela.EspeciePagamento.Codigo;
							totalizacaoEspecieDto.Descricao = string.Format("{0} {1}", <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.parcela.EspeciePagamento.Descricao, <>h__TransparentIdentifier1.creditoDebito);
							totalizacaoEspecieDto.DescricaoBandeira = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.DescricaoRelatorio;
							totalizacaoEspecieDto.Valor = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Valor;
							totalizacaoEspecieDto.Usuario = MovimentacaoSaidaRelatorioAppService.RetornarUsuario(CS$<>8__locals1.usuarios, movimentacao2);
							totalizacaoEspecieDto.Data = movimentacao2.DataHoraEmissao;
							totalizacaoEspecieDto.Debito = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito;
							int num;
							if (!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.IsExactlyObject<InformacaoTransacaoTef>())
							{
								if (!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito)
								{
									Taxa taxa = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
									if (taxa == null || !taxa.Is<Debito>())
									{
										Taxa taxa2 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
										if (taxa2 != null && taxa2.Is<CreditoAVista>())
										{
											num = 1;
											goto IL_01FE;
										}
										Taxa taxa3 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
										if (taxa3 == null || !taxa3.Is<Credito>())
										{
											DebitoParcelado debitoParcelado = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<DebitoParcelado>();
											num = ((debitoParcelado != null) ? debitoParcelado.NumeroParcelas : 0);
											goto IL_01FE;
										}
										Credito credito = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<Credito>();
										if (credito != null && credito.NumeroParcelas != 0)
										{
											Credito credito2 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<Credito>();
											num = ((credito2 != null) ? credito2.NumeroParcelas : 0);
											goto IL_01FE;
										}
										num = 1;
										goto IL_01FE;
									}
								}
								num = 0;
							}
							else
							{
								num = ((!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito && <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.To<InformacaoTransacaoTef>().NumeroParcelas == 0) ? 1 : <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.To<InformacaoTransacaoTef>().NumeroParcelas);
							}
							IL_01FE:
							totalizacaoEspecieDto.NumeroParcelas = num;
							return totalizacaoEspecieDto;
						}));
						list2.AddRange(list3.Where<ItemPagamento>((ItemPagamento x) => x is ItemPagamentoCarteiraDigital).OfType<ItemPagamentoCarteiraDigital>().SelectMany<ItemPagamentoCarteiraDigital, InformacaoTransacaoCarteiraDigital, TotalizacaoEspecieDto>((ItemPagamentoCarteiraDigital parcela) => parcela.InformacoesTransacao, delegate(ItemPagamentoCarteiraDigital parcela, InformacaoTransacaoCarteiraDigital tef)
						{
							TotalizacaoEspecieDto totalizacaoEspecieDto2 = new TotalizacaoEspecieDto();
							totalizacaoEspecieDto2.Codigo = parcela.EspeciePagamento.Codigo;
							totalizacaoEspecieDto2.Descricao = parcela.EspeciePagamento.Descricao;
							totalizacaoEspecieDto2.DescricaoBandeira = tef.Tipo;
							totalizacaoEspecieDto2.Valor = tef.Valor;
							totalizacaoEspecieDto2.Usuario = MovimentacaoSaidaRelatorioAppService.RetornarUsuario(CS$<>8__locals1.usuarios, movimentacao2);
							totalizacaoEspecieDto2.Data = movimentacao2.DataHoraEmissao;
							totalizacaoEspecieDto2.Debito = tef.Debito;
							int num2;
							if (!tef.IsExactlyObject<InformacaoTransacaoTef>())
							{
								if (!tef.Debito)
								{
									Taxa taxa4 = tef.Taxa;
									if (taxa4 == null || !taxa4.Is<Debito>())
									{
										Taxa taxa5 = tef.Taxa;
										if (taxa5 != null && taxa5.Is<CreditoAVista>())
										{
											num2 = 1;
											goto IL_0142;
										}
										Taxa taxa6 = tef.Taxa;
										if (taxa6 == null || !taxa6.Is<Credito>())
										{
											DebitoParcelado debitoParcelado2 = tef.Taxa.To<DebitoParcelado>();
											num2 = ((debitoParcelado2 != null) ? debitoParcelado2.NumeroParcelas : 0);
											goto IL_0142;
										}
										Credito credito3 = tef.Taxa.To<Credito>();
										if (credito3 != null && credito3.NumeroParcelas != 0)
										{
											Credito credito4 = tef.Taxa.To<Credito>();
											num2 = ((credito4 != null) ? credito4.NumeroParcelas : 0);
											goto IL_0142;
										}
										num2 = 1;
										goto IL_0142;
									}
								}
								num2 = 0;
							}
							else
							{
								num2 = ((!tef.Debito && tef.To<InformacaoTransacaoTef>().NumeroParcelas == 0) ? 1 : tef.To<InformacaoTransacaoTef>().NumeroParcelas);
							}
							IL_0142:
							totalizacaoEspecieDto2.NumeroParcelas = num2;
							return totalizacaoEspecieDto2;
						}));
						list2.AddRange((from parcela in list3.Where<ItemPagamento>((ItemPagamento x) => x is ItemPagamentoTerminalPos).OfType<ItemPagamentoTerminalPos>()
							from tef in parcela.PagamentosTransacoes
							select new
							{
								<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0,
								creditoDebito = (tef.Debito ? "Débito" : "Crédito")
							}).Select(delegate(<>h__TransparentIdentifier1)
						{
							TotalizacaoEspecieDto totalizacaoEspecieDto3 = new TotalizacaoEspecieDto();
							totalizacaoEspecieDto3.Codigo = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.parcela.EspeciePagamento.Codigo;
							totalizacaoEspecieDto3.Descricao = string.Format("{0} {1}", <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.parcela.EspeciePagamento.Descricao, <>h__TransparentIdentifier1.creditoDebito);
							totalizacaoEspecieDto3.DescricaoBandeira = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.DescricaoRelatorio;
							totalizacaoEspecieDto3.Valor = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Valor;
							totalizacaoEspecieDto3.Usuario = MovimentacaoSaidaRelatorioAppService.RetornarUsuario(CS$<>8__locals1.usuarios, movimentacao2);
							totalizacaoEspecieDto3.Data = movimentacao2.DataHoraEmissao;
							totalizacaoEspecieDto3.Debito = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito;
							int num3;
							if (!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.IsExactlyObject<PagamentoTransacao>())
							{
								if (!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito)
								{
									Taxa taxa7 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
									if (taxa7 == null || !taxa7.Is<Debito>())
									{
										Taxa taxa8 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
										if (taxa8 != null && taxa8.Is<CreditoAVista>())
										{
											num3 = 1;
											goto IL_01FE;
										}
										Taxa taxa9 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
										if (taxa9 == null || !taxa9.Is<Credito>())
										{
											DebitoParcelado debitoParcelado3 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<DebitoParcelado>();
											num3 = ((debitoParcelado3 != null) ? debitoParcelado3.NumeroParcelas : 0);
											goto IL_01FE;
										}
										Credito credito5 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<Credito>();
										if (credito5 != null && credito5.NumeroParcelas != 0)
										{
											Credito credito6 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<Credito>();
											num3 = ((credito6 != null) ? credito6.NumeroParcelas : 0);
											goto IL_01FE;
										}
										num3 = 1;
										goto IL_01FE;
									}
								}
								num3 = 0;
							}
							else
							{
								num3 = ((!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito && <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.To<PagamentoTransacao>().NumeroParcelas == 0) ? 1 : <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.To<PagamentoTransacao>().NumeroParcelas);
							}
							IL_01FE:
							totalizacaoEspecieDto3.NumeroParcelas = num3;
							return totalizacaoEspecieDto3;
						}));
					}
				}
			}
			CS$<>8__locals1.lista.AddRange(list2);
			using (IEnumerator<Movimentacao> enumerator = enumerable4.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Movimentacao movimentacao = enumerator.Current;
					if (!movimentacao.IsNot<IMovimentacaoPagamentoRecebimento>())
					{
						IMovimentacaoPagamento movimentacaoPagamento = movimentacao.To<IMovimentacaoPagamento>();
						if (movimentacaoPagamento == null || !movimentacaoPagamento.Pagamentos.ToListEmptyWhenNull<ItemPagamento>().Any<ItemPagamento>())
						{
							if (movimentacao.Is<IMovimentacaoPagamentoRecebimento>() && !CS$<>8__locals1.centroCustoReferencia.IsNullOrEmpty())
							{
								IEnumerable<Parcela> parcelas2 = movimentacao.To<IMovimentacaoPagamentoRecebimento>().PagamentoRecebimento.Parcelas;
								Func<Parcela, bool> func3;
								if ((func3 = CS$<>8__locals1.<>9__19) == null)
								{
									MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass26_0 CS$<>8__locals6 = CS$<>8__locals1;
									Func<Parcela, bool> func4 = (Parcela x) => x.CentroCustoCodigo.Equals(CS$<>8__locals1.centroCustoReferencia);
									CS$<>8__locals6.<>9__19 = func4;
									func3 = func4;
								}
								if (!parcelas2.Any<Parcela>(func3))
								{
									continue;
								}
							}
							IMovimentacaoPagamentoRecebimento movimentacaoPagamentoRecebimento = movimentacao as IMovimentacaoPagamentoRecebimento;
							IEnumerable<Parcela> enumerable6 = movimentacaoPagamentoRecebimento.PagamentoRecebimento.Parcelas.Where<Parcela>((Parcela x) => !(x is IParcelaRecebimentoTaxa));
							CS$<>8__locals1.lista.AddRange(enumerable6.Select<Parcela, TotalizacaoEspecieDto>((Parcela p) => new TotalizacaoEspecieDto
							{
								Codigo = p.EspeciePagamento.Codigo,
								Descricao = p.EspeciePagamento.Descricao,
								Valor = p.Valor,
								Usuario = MovimentacaoSaidaRelatorioAppService.RetornarUsuario(CS$<>8__locals1.usuarios, movimentacao),
								Data = movimentacao.DataHoraEmissao
							}));
							IEnumerable<ParcelaRecebimentoCartao> enumerable7 = movimentacaoPagamentoRecebimento.PagamentoRecebimento.Parcelas.OfType<ParcelaRecebimentoCartao>();
							CS$<>8__locals1.lista.AddRange(enumerable7.Select<ParcelaRecebimentoCartao, TotalizacaoEspecieDto>((ParcelaRecebimentoCartao p) => new TotalizacaoEspecieDto
							{
								Codigo = p.EspeciePagamento.Codigo,
								Descricao = p.EspeciePagamento.Descricao,
								DescricaoBandeira = string.Format("{0} {1}", p.DescricaoRede, p.ObterEspecieTipoCartao),
								Valor = p.Valor,
								Usuario = MovimentacaoSaidaRelatorioAppService.RetornarUsuario(CS$<>8__locals1.usuarios, movimentacao),
								Data = movimentacao.DataHoraEmissao
							}));
							IEnumerable<ParcelaRecebimentoCarteiraDigital> enumerable8 = movimentacaoPagamentoRecebimento.PagamentoRecebimento.Parcelas.OfType<ParcelaRecebimentoCarteiraDigital>();
							CS$<>8__locals1.lista.AddRange(enumerable8.Select<ParcelaRecebimentoCarteiraDigital, TotalizacaoEspecieDto>((ParcelaRecebimentoCarteiraDigital p) => new TotalizacaoEspecieDto
							{
								Codigo = p.EspeciePagamento.Codigo,
								Descricao = p.EspeciePagamento.Descricao,
								DescricaoBandeira = p.ObterEspecieTipoCartao,
								Valor = p.Valor,
								Usuario = MovimentacaoSaidaRelatorioAppService.RetornarUsuario(CS$<>8__locals1.usuarios, movimentacao),
								Data = movimentacao.DataHoraEmissao
							}));
						}
					}
				}
			}
			List<TotalizacaoEspecieDto> list4;
			if (!agruparPorDia)
			{
				list4 = (from g in CS$<>8__locals1.lista
					group g by new { g.Codigo, g.Descricao, g.DescricaoBandeira, g.Usuario }).Select(delegate(s)
				{
					TotalizacaoEspecieDto totalizacaoEspecieDto4 = new TotalizacaoEspecieDto();
					totalizacaoEspecieDto4.Codigo = s.Key.Codigo;
					totalizacaoEspecieDto4.Descricao = s.Key.Descricao;
					totalizacaoEspecieDto4.DescricaoBandeira = s.Key.DescricaoBandeira;
					totalizacaoEspecieDto4.Valor = s.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor);
					totalizacaoEspecieDto4.Quantidade = s.Count<TotalizacaoEspecieDto>();
					totalizacaoEspecieDto4.Participacao = s.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor) * 100m / CS$<>8__locals1.lista.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor);
					totalizacaoEspecieDto4.Usuario = s.Key.Usuario;
					return totalizacaoEspecieDto4;
				}).ToListEmptyWhenNull<TotalizacaoEspecieDto>();
			}
			else
			{
				list4 = (from g in CS$<>8__locals1.lista
					group g by new
					{
						g.Codigo,
						g.Descricao,
						g.DescricaoBandeira,
						g.Usuario,
						g.Data.Date
					}).Select(delegate(s)
				{
					TotalizacaoEspecieDto totalizacaoEspecieDto5 = new TotalizacaoEspecieDto();
					totalizacaoEspecieDto5.Codigo = s.Key.Codigo;
					totalizacaoEspecieDto5.Descricao = s.Key.Descricao;
					totalizacaoEspecieDto5.DescricaoBandeira = s.Key.DescricaoBandeira;
					totalizacaoEspecieDto5.Valor = s.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor);
					totalizacaoEspecieDto5.Quantidade = s.Count<TotalizacaoEspecieDto>();
					totalizacaoEspecieDto5.Participacao = s.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor) * 100m / CS$<>8__locals1.lista.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor);
					totalizacaoEspecieDto5.Usuario = s.Key.Usuario;
					totalizacaoEspecieDto5.Data = s.Key.Date;
					return totalizacaoEspecieDto5;
				}).ToListEmptyWhenNull<TotalizacaoEspecieDto>();
			}
			List<TotalizacaoEspecieDto> list5 = list4;
			return new ResumoTotalizacaoEspecieDto
			{
				Lista = list5,
				ListaCartoes = list2
			};
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0007D0D0 File Offset: 0x0007B2D0
		public async Task<ResumoTotalizacaoEspecieDto> TotalizacaoPorEspeciePorVendedor(DateTime dataInicial, DateTime dataFinal, string vendedorReferencia, int origemMovimentacao, int situacaoMovimentacao, bool agruparPorDia, string centroCustoReferencia, IUsuarioIdentity usuarioIdentity)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass27_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass27_0();
			CS$<>8__locals1.centroCustoReferencia = centroCustoReferencia;
			IEnumerable<Movimentacao> enumerable3;
			if (situacaoMovimentacao == 99)
			{
				ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
					.PorNaturezaOperacao(1)
					.PorFinalidadeEmissaoNormal()
					.PorSituacao(14)
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
					.PorVendedor(vendedorReferencia);
				IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindAsync(specification);
				IEnumerable<Movimentacao> resultadoContingencia = enumerable;
				specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
					.PorNaturezaOperacao(1)
					.PorFinalidadeEmissaoNormal()
					.PorSituacao(2)
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
					.PorVendedor(vendedorReferencia);
				IEnumerable<Movimentacao> enumerable2 = await this._movimentacaoRepositoryRead.FindAsync(specification);
				enumerable3 = resultadoContingencia.Union<Movimentacao>(enumerable2);
				resultadoContingencia = null;
			}
			else
			{
				ISpecification<Movimentacao> specification2 = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
					.PorNaturezaOperacao(1)
					.PorFinalidadeEmissaoNormal()
					.PorSituacao(situacaoMovimentacao)
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
					.PorVendedor(vendedorReferencia);
				enumerable3 = await this._movimentacaoRepositoryRead.FindAsync(specification2);
			}
			List<Movimentacao> list = enumerable3.ToListEmptyWhenNull<Movimentacao>();
			Dictionary<ObjectId, string> dictionary = this.ObterNomesVendedores(list);
			CS$<>8__locals1.lista = new List<TotalizacaoEspecieDto>();
			List<TotalizacaoEspecieDto> list2 = new List<TotalizacaoEspecieDto>();
			using (List<Movimentacao>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Movimentacao movimentacao2 = enumerator.Current;
					if (!movimentacao2.IsNot<IMovimentacaoPagamento>())
					{
						if (movimentacao2.Is<IMovimentacaoPagamentoRecebimento>() && !CS$<>8__locals1.centroCustoReferencia.IsNullOrEmpty())
						{
							IEnumerable<Parcela> parcelas = movimentacao2.To<IMovimentacaoPagamentoRecebimento>().PagamentoRecebimento.Parcelas;
							Func<Parcela, bool> func;
							if ((func = CS$<>8__locals1.<>9__0) == null)
							{
								MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass27_0 CS$<>8__locals4 = CS$<>8__locals1;
								Func<Parcela, bool> func2 = (Parcela x) => x.CentroCustoCodigo.Equals(CS$<>8__locals1.centroCustoReferencia);
								CS$<>8__locals4.<>9__0 = func2;
								func = func2;
							}
							if (!parcelas.Any<Parcela>(func))
							{
								continue;
							}
						}
						IList<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie> rateios2 = MovimentacaoSaidaRelatorioAppService.ObterRateiosPorVendedor(movimentacao2, dictionary, vendedorReferencia);
						if (rateios2.Any<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie>())
						{
							List<ItemPagamento> list3 = movimentacao2.To<IMovimentacaoPagamento>().Pagamentos.ToListEmptyWhenNull<ItemPagamento>();
							IEnumerable<ItemPagamento> enumerable4 = list3.Where<ItemPagamento>((ItemPagamento x) => !(x is IItemPagamentoCartao) && !(x is ItemPagamentoCarteiraDigital) && !(x is ItemPagamentoTerminalPos));
							CS$<>8__locals1.lista.AddRange(enumerable4.Select<ItemPagamento, TotalizacaoEspecieDto>((ItemPagamento p) => new TotalizacaoEspecieDto
							{
								Codigo = p.EspeciePagamento.Codigo,
								Descricao = p.EspeciePagamento.Descricao,
								Valor = p.Valor,
								Data = movimentacao2.DataHoraEmissao.Date
							}).SelectMany<TotalizacaoEspecieDto, TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => MovimentacaoSaidaRelatorioAppService.RatearPorVendedor(x, rateios2)).ToListEmptyWhenNull<TotalizacaoEspecieDto>());
							list2.AddRange((from parcela in list3.Where<ItemPagamento>((ItemPagamento x) => x is IItemPagamentoCartao).OfType<IItemPagamentoCartao>()
								from tef in parcela.InformacoesTransacaoRelatorio
								select new
								{
									<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0,
									creditoDebito = (tef.Debito ? "Débito" : "Crédito")
								}).Select(delegate(<>h__TransparentIdentifier1)
							{
								TotalizacaoEspecieDto totalizacaoEspecieDto = new TotalizacaoEspecieDto();
								totalizacaoEspecieDto.Codigo = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.parcela.EspeciePagamento.Codigo;
								totalizacaoEspecieDto.Descricao = string.Format("{0} {1}", <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.parcela.EspeciePagamento.Descricao, <>h__TransparentIdentifier1.creditoDebito);
								totalizacaoEspecieDto.DescricaoBandeira = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.DescricaoRelatorio;
								totalizacaoEspecieDto.Valor = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Valor;
								totalizacaoEspecieDto.Data = movimentacao2.DataHoraEmissao;
								totalizacaoEspecieDto.Debito = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito;
								int num;
								if (!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.IsExactlyObject<InformacaoTransacaoTef>())
								{
									if (!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito)
									{
										Taxa taxa = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
										if (taxa == null || !taxa.Is<Debito>())
										{
											Taxa taxa2 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
											if (taxa2 != null && taxa2.Is<CreditoAVista>())
											{
												num = 1;
												goto IL_01E2;
											}
											Taxa taxa3 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
											if (taxa3 == null || !taxa3.Is<Credito>())
											{
												DebitoParcelado debitoParcelado = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<DebitoParcelado>();
												num = ((debitoParcelado != null) ? debitoParcelado.NumeroParcelas : 0);
												goto IL_01E2;
											}
											Credito credito = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<Credito>();
											if (credito != null && credito.NumeroParcelas != 0)
											{
												Credito credito2 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<Credito>();
												num = ((credito2 != null) ? credito2.NumeroParcelas : 0);
												goto IL_01E2;
											}
											num = 1;
											goto IL_01E2;
										}
									}
									num = 0;
								}
								else
								{
									num = ((!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito && <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.To<InformacaoTransacaoTef>().NumeroParcelas == 0) ? 1 : <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.To<InformacaoTransacaoTef>().NumeroParcelas);
								}
								IL_01E2:
								totalizacaoEspecieDto.NumeroParcelas = num;
								return totalizacaoEspecieDto;
							}).SelectMany<TotalizacaoEspecieDto, TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => MovimentacaoSaidaRelatorioAppService.RatearPorVendedor(x, rateios2)));
							list2.AddRange(list3.Where<ItemPagamento>((ItemPagamento x) => x is ItemPagamentoCarteiraDigital).OfType<ItemPagamentoCarteiraDigital>().SelectMany<ItemPagamentoCarteiraDigital, InformacaoTransacaoCarteiraDigital, TotalizacaoEspecieDto>((ItemPagamentoCarteiraDigital parcela) => parcela.InformacoesTransacao, delegate(ItemPagamentoCarteiraDigital parcela, InformacaoTransacaoCarteiraDigital tef)
							{
								TotalizacaoEspecieDto totalizacaoEspecieDto2 = new TotalizacaoEspecieDto();
								totalizacaoEspecieDto2.Codigo = parcela.EspeciePagamento.Codigo;
								totalizacaoEspecieDto2.Descricao = parcela.EspeciePagamento.Descricao;
								totalizacaoEspecieDto2.DescricaoBandeira = tef.Tipo;
								totalizacaoEspecieDto2.Valor = tef.Valor;
								totalizacaoEspecieDto2.Data = movimentacao2.DataHoraEmissao;
								totalizacaoEspecieDto2.Debito = tef.Debito;
								int num2;
								if (!tef.IsExactlyObject<InformacaoTransacaoTef>())
								{
									if (!tef.Debito)
									{
										Taxa taxa4 = tef.Taxa;
										if (taxa4 == null || !taxa4.Is<Debito>())
										{
											Taxa taxa5 = tef.Taxa;
											if (taxa5 != null && taxa5.Is<CreditoAVista>())
											{
												num2 = 1;
												goto IL_0126;
											}
											Taxa taxa6 = tef.Taxa;
											if (taxa6 == null || !taxa6.Is<Credito>())
											{
												DebitoParcelado debitoParcelado2 = tef.Taxa.To<DebitoParcelado>();
												num2 = ((debitoParcelado2 != null) ? debitoParcelado2.NumeroParcelas : 0);
												goto IL_0126;
											}
											Credito credito3 = tef.Taxa.To<Credito>();
											if (credito3 != null && credito3.NumeroParcelas != 0)
											{
												Credito credito4 = tef.Taxa.To<Credito>();
												num2 = ((credito4 != null) ? credito4.NumeroParcelas : 0);
												goto IL_0126;
											}
											num2 = 1;
											goto IL_0126;
										}
									}
									num2 = 0;
								}
								else
								{
									num2 = ((!tef.Debito && tef.To<InformacaoTransacaoTef>().NumeroParcelas == 0) ? 1 : tef.To<InformacaoTransacaoTef>().NumeroParcelas);
								}
								IL_0126:
								totalizacaoEspecieDto2.NumeroParcelas = num2;
								return totalizacaoEspecieDto2;
							})
								.SelectMany<TotalizacaoEspecieDto, TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => MovimentacaoSaidaRelatorioAppService.RatearPorVendedor(x, rateios2)));
							list2.AddRange((from parcela in list3.Where<ItemPagamento>((ItemPagamento x) => x is ItemPagamentoTerminalPos).OfType<ItemPagamentoTerminalPos>()
								from tef in parcela.PagamentosTransacoes
								select new
								{
									<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0,
									creditoDebito = (tef.Debito ? "Débito" : "Crédito")
								}).Select(delegate(<>h__TransparentIdentifier1)
							{
								TotalizacaoEspecieDto totalizacaoEspecieDto3 = new TotalizacaoEspecieDto();
								totalizacaoEspecieDto3.Codigo = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.parcela.EspeciePagamento.Codigo;
								totalizacaoEspecieDto3.Descricao = string.Format("{0} {1}", <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.parcela.EspeciePagamento.Descricao, <>h__TransparentIdentifier1.creditoDebito);
								totalizacaoEspecieDto3.DescricaoBandeira = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.DescricaoRelatorio;
								totalizacaoEspecieDto3.Valor = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Valor;
								totalizacaoEspecieDto3.Data = movimentacao2.DataHoraEmissao;
								totalizacaoEspecieDto3.Debito = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito;
								int num3;
								if (!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.IsExactlyObject<PagamentoTransacao>())
								{
									if (!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito)
									{
										Taxa taxa7 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
										if (taxa7 == null || !taxa7.Is<Debito>())
										{
											Taxa taxa8 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
											if (taxa8 != null && taxa8.Is<CreditoAVista>())
											{
												num3 = 1;
												goto IL_01E2;
											}
											Taxa taxa9 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa;
											if (taxa9 == null || !taxa9.Is<Credito>())
											{
												DebitoParcelado debitoParcelado3 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<DebitoParcelado>();
												num3 = ((debitoParcelado3 != null) ? debitoParcelado3.NumeroParcelas : 0);
												goto IL_01E2;
											}
											Credito credito5 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<Credito>();
											if (credito5 != null && credito5.NumeroParcelas != 0)
											{
												Credito credito6 = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Taxa.To<Credito>();
												num3 = ((credito6 != null) ? credito6.NumeroParcelas : 0);
												goto IL_01E2;
											}
											num3 = 1;
											goto IL_01E2;
										}
									}
									num3 = 0;
								}
								else
								{
									num3 = ((!<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.Debito && <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.To<PagamentoTransacao>().NumeroParcelas == 0) ? 1 : <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.tef.To<PagamentoTransacao>().NumeroParcelas);
								}
								IL_01E2:
								totalizacaoEspecieDto3.NumeroParcelas = num3;
								return totalizacaoEspecieDto3;
							}).SelectMany<TotalizacaoEspecieDto, TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => MovimentacaoSaidaRelatorioAppService.RatearPorVendedor(x, rateios2)));
						}
					}
				}
			}
			CS$<>8__locals1.lista.AddRange(list2);
			using (List<Movimentacao>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Movimentacao movimentacao = enumerator.Current;
					if (!movimentacao.IsNot<IMovimentacaoPagamentoRecebimento>())
					{
						IMovimentacaoPagamento movimentacaoPagamento = movimentacao.To<IMovimentacaoPagamento>();
						if (movimentacaoPagamento == null || !movimentacaoPagamento.Pagamentos.ToListEmptyWhenNull<ItemPagamento>().Any<ItemPagamento>())
						{
							if (movimentacao.Is<IMovimentacaoPagamentoRecebimento>() && !CS$<>8__locals1.centroCustoReferencia.IsNullOrEmpty())
							{
								IEnumerable<Parcela> parcelas2 = movimentacao.To<IMovimentacaoPagamentoRecebimento>().PagamentoRecebimento.Parcelas;
								Func<Parcela, bool> func3;
								if ((func3 = CS$<>8__locals1.<>9__20) == null)
								{
									MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass27_0 CS$<>8__locals7 = CS$<>8__locals1;
									Func<Parcela, bool> func4 = (Parcela x) => x.CentroCustoCodigo.Equals(CS$<>8__locals1.centroCustoReferencia);
									CS$<>8__locals7.<>9__20 = func4;
									func3 = func4;
								}
								if (!parcelas2.Any<Parcela>(func3))
								{
									continue;
								}
							}
							IList<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie> rateios = MovimentacaoSaidaRelatorioAppService.ObterRateiosPorVendedor(movimentacao, dictionary, vendedorReferencia);
							if (rateios.Any<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie>())
							{
								IMovimentacaoPagamentoRecebimento movimentacaoPagamentoRecebimento = movimentacao as IMovimentacaoPagamentoRecebimento;
								IEnumerable<Parcela> enumerable5 = movimentacaoPagamentoRecebimento.PagamentoRecebimento.Parcelas.Where<Parcela>((Parcela x) => !(x is IParcelaRecebimentoTaxa));
								CS$<>8__locals1.lista.AddRange(enumerable5.Select<Parcela, TotalizacaoEspecieDto>((Parcela p) => new TotalizacaoEspecieDto
								{
									Codigo = p.EspeciePagamento.Codigo,
									Descricao = p.EspeciePagamento.Descricao,
									Valor = p.Valor,
									Data = movimentacao.DataHoraEmissao
								}).SelectMany<TotalizacaoEspecieDto, TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => MovimentacaoSaidaRelatorioAppService.RatearPorVendedor(x, rateios)));
								IEnumerable<ParcelaRecebimentoCartao> enumerable6 = movimentacaoPagamentoRecebimento.PagamentoRecebimento.Parcelas.OfType<ParcelaRecebimentoCartao>();
								CS$<>8__locals1.lista.AddRange(enumerable6.Select<ParcelaRecebimentoCartao, TotalizacaoEspecieDto>((ParcelaRecebimentoCartao p) => new TotalizacaoEspecieDto
								{
									Codigo = p.EspeciePagamento.Codigo,
									Descricao = p.EspeciePagamento.Descricao,
									DescricaoBandeira = string.Format("{0} {1}", p.DescricaoRede, p.ObterEspecieTipoCartao),
									Valor = p.Valor,
									Data = movimentacao.DataHoraEmissao
								}).SelectMany<TotalizacaoEspecieDto, TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => MovimentacaoSaidaRelatorioAppService.RatearPorVendedor(x, rateios)));
								IEnumerable<ParcelaRecebimentoCarteiraDigital> enumerable7 = movimentacaoPagamentoRecebimento.PagamentoRecebimento.Parcelas.OfType<ParcelaRecebimentoCarteiraDigital>();
								CS$<>8__locals1.lista.AddRange(enumerable7.Select<ParcelaRecebimentoCarteiraDigital, TotalizacaoEspecieDto>((ParcelaRecebimentoCarteiraDigital p) => new TotalizacaoEspecieDto
								{
									Codigo = p.EspeciePagamento.Codigo,
									Descricao = p.EspeciePagamento.Descricao,
									DescricaoBandeira = p.ObterEspecieTipoCartao,
									Valor = p.Valor,
									Data = movimentacao.DataHoraEmissao
								}).SelectMany<TotalizacaoEspecieDto, TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => MovimentacaoSaidaRelatorioAppService.RatearPorVendedor(x, rateios)));
							}
						}
					}
				}
			}
			List<TotalizacaoEspecieDto> list4;
			if (!agruparPorDia)
			{
				list4 = (from g in CS$<>8__locals1.lista
					group g by new { g.Codigo, g.Descricao, g.DescricaoBandeira, g.Vendedor }).Select(delegate(s)
				{
					TotalizacaoEspecieDto totalizacaoEspecieDto4 = new TotalizacaoEspecieDto();
					totalizacaoEspecieDto4.Codigo = s.Key.Codigo;
					totalizacaoEspecieDto4.Descricao = s.Key.Descricao;
					totalizacaoEspecieDto4.DescricaoBandeira = s.Key.DescricaoBandeira;
					totalizacaoEspecieDto4.Valor = s.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor);
					totalizacaoEspecieDto4.Quantidade = s.Count<TotalizacaoEspecieDto>();
					totalizacaoEspecieDto4.Participacao = s.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor) * 100m / CS$<>8__locals1.lista.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor);
					totalizacaoEspecieDto4.Vendedor = s.Key.Vendedor;
					return totalizacaoEspecieDto4;
				}).ToListEmptyWhenNull<TotalizacaoEspecieDto>();
			}
			else
			{
				list4 = (from g in CS$<>8__locals1.lista
					group g by new
					{
						g.Codigo,
						g.Descricao,
						g.DescricaoBandeira,
						g.Vendedor,
						g.Data.Date
					}).Select(delegate(s)
				{
					TotalizacaoEspecieDto totalizacaoEspecieDto5 = new TotalizacaoEspecieDto();
					totalizacaoEspecieDto5.Codigo = s.Key.Codigo;
					totalizacaoEspecieDto5.Descricao = s.Key.Descricao;
					totalizacaoEspecieDto5.DescricaoBandeira = s.Key.DescricaoBandeira;
					totalizacaoEspecieDto5.Valor = s.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor);
					totalizacaoEspecieDto5.Quantidade = s.Count<TotalizacaoEspecieDto>();
					totalizacaoEspecieDto5.Participacao = s.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor) * 100m / CS$<>8__locals1.lista.Sum<TotalizacaoEspecieDto>((TotalizacaoEspecieDto x) => x.Valor);
					totalizacaoEspecieDto5.Vendedor = s.Key.Vendedor;
					totalizacaoEspecieDto5.Data = s.Key.Date;
					return totalizacaoEspecieDto5;
				}).ToListEmptyWhenNull<TotalizacaoEspecieDto>();
			}
			List<TotalizacaoEspecieDto> list5 = list4;
			return new ResumoTotalizacaoEspecieDto
			{
				Lista = list5,
				ListaCartoes = list2
			};
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0007D15C File Offset: 0x0007B35C
		private Dictionary<ObjectId, string> ObterNomesVendedores(IEnumerable<Movimentacao> movimentacoes)
		{
			List<ObjectId> list = (from x in movimentacoes.SelectMany<Movimentacao, IItem>((Movimentacao x) => x.Itens.ToListEmptyWhenNull<IItem>())
				select x.VendedorReferencia into x
				where !x.IsDefaultOrEmpty()
				select x).Distinct<ObjectId>().ToListEmptyWhenNull<ObjectId>();
			if (list.Any<ObjectId>())
			{
				return (from x in this._pessoaRepositoryRead.GetIn<ObjectId>((Pessoa x) => x.Id, list)
					group x by x.Id).ToDictionary<IGrouping<ObjectId, Pessoa>, ObjectId, string>((IGrouping<ObjectId, Pessoa> x) => x.Key, (IGrouping<ObjectId, Pessoa> x) => x.First<Pessoa>().Nome.ToStringEmptyWhenNull());
			}
			return new Dictionary<ObjectId, string>();
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0007D2A0 File Offset: 0x0007B4A0
		private static IList<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie> ObterRateiosPorVendedor(Movimentacao movimentacao, Dictionary<ObjectId, string> nomesVendedores, string vendedorReferencia)
		{
			List<IItem> list = (from x in movimentacao.Itens.ToListEmptyWhenNull<IItem>()
				where !x.Cancelado
				select x).ToListEmptyWhenNull<IItem>();
			var list2 = (from x in list
				group x by x.VendedorReferencia into x
				select new
				{
					Referencia = x.Key,
					Nome = MovimentacaoSaidaRelatorioAppService.ObterNomeVendedor(nomesVendedores, x.Key, x.Select<IItem, string>(delegate(IItem y)
					{
						IItemSaidaProduto itemSaidaProduto = y.To<IItemSaidaProduto>();
						if (itemSaidaProduto == null)
						{
							return null;
						}
						PessoaHistorico vendedor = itemSaidaProduto.Vendedor;
						if (vendedor == null)
						{
							return null;
						}
						return vendedor.Nome;
					}).FirstOrDefault<string>((string y) => !string.IsNullOrEmpty(y))),
					Total = x.Sum<IItem>((IItem y) => y.TotalLiquido)
				}).ToListEmptyWhenNull();
			int quantidadeVendedores = list2.Count;
			if (!string.IsNullOrEmpty(vendedorReferencia))
			{
				list2 = list2.Where(x => x.Referencia.Equals(vendedorReferencia.TryParseReferencia())).ToListEmptyWhenNull();
			}
			if (!list2.Any())
			{
				if (!string.IsNullOrEmpty(vendedorReferencia))
				{
					return new List<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie>();
				}
				return new List<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie>
				{
					new MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie
					{
						Vendedor = "Não informado",
						Fator = 1m
					}
				};
			}
			else
			{
				decimal totalItens = list.Sum<IItem>((IItem x) => x.TotalLiquido);
				if (!(totalItens <= 0m))
				{
					return list2.Select(x => new MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie
					{
						Vendedor = x.Nome,
						Fator = x.Total / totalItens
					}).ToListEmptyWhenNull<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie>();
				}
				return list2.Select(x => new MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie
				{
					Vendedor = x.Nome,
					Fator = 1m / quantidadeVendedores
				}).ToListEmptyWhenNull<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie>();
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0007D414 File Offset: 0x0007B614
		private static string ObterNomeVendedor(Dictionary<ObjectId, string> nomesVendedores, ObjectId vendedorReferencia, string nomeHistorico)
		{
			if (vendedorReferencia.IsDefaultOrEmpty())
			{
				return "Não informado";
			}
			string text;
			if (nomesVendedores.TryGetValue(vendedorReferencia, out text) && !string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (!string.IsNullOrEmpty(nomeHistorico))
			{
				return nomeHistorico;
			}
			return "Não informado";
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0007D454 File Offset: 0x0007B654
		private static IEnumerable<TotalizacaoEspecieDto> RatearPorVendedor(TotalizacaoEspecieDto especie, IList<MovimentacaoSaidaRelatorioAppService.RateioVendedorEspecie> rateios)
		{
			List<TotalizacaoEspecieDto> list = new List<TotalizacaoEspecieDto>();
			decimal num = 0m;
			for (int i = 0; i < rateios.Count; i++)
			{
				decimal num2 = ((rateios.Count > 1 && i == rateios.Count - 1) ? (especie.Valor - num) : (especie.Valor * rateios[i].Fator).ArredondarParaDuasCasasDecimais());
				num += num2;
				list.Add(new TotalizacaoEspecieDto
				{
					Codigo = especie.Codigo,
					Descricao = especie.Descricao,
					DescricaoBandeira = especie.DescricaoBandeira,
					Valor = num2,
					Vendedor = rateios[i].Vendedor,
					Data = especie.Data,
					Debito = especie.Debito,
					NumeroParcelas = especie.NumeroParcelas
				});
			}
			return list;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0007D53C File Offset: 0x0007B73C
		public async Task<DadosRelatorioResumoVendasDto> ResumoVendas(DateTime dataInicial, DateTime dataFinal, int origemMovimentacao, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specificationMovimentacao = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
				.PorNaturezaOperacao(1)
				.PorFinalidadeEmissaoNormal()
				.PorSituacaoConcluido()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindAsync(specificationMovimentacao);
			List<ResumoVendasDto> list = new List<ResumoVendasDto>();
			using (IEnumerator<Movimentacao> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Movimentacao movimentacao3 = enumerator.Current;
					if (!movimentacao3.IsNot<IMovimentacaoPagamento>())
					{
						List<ItemPagamento> list2 = movimentacao3.To<IMovimentacaoPagamento>().Pagamentos.ToListEmptyWhenNull<ItemPagamento>();
						list.AddRange((from x in list2
							where !(x is IItemPagamentoCartao)
							select x into p
							select new ResumoVendasDto
							{
								Descricao = p.EspeciePagamento.Descricao,
								Valor = p.Valor,
								Data = movimentacao3.DataHoraEmissao.Date
							}).ToListEmptyWhenNull<ResumoVendasDto>());
						list.AddRange(from parcela in list2.Where<ItemPagamento>((ItemPagamento x) => x is IItemPagamentoCartao).OfType<IItemPagamentoCartao>()
							from tef in parcela.InformacoesTransacaoRelatorio
							let creditoDebito = tef.Debito ? "Débito" : "Crédito"
							select new ResumoVendasDto
							{
								Descricao = string.Format("{0} {1}", parcela.EspeciePagamento.Descricao, creditoDebito),
								Valor = tef.Valor,
								Data = movimentacao3.DataHoraEmissao
							});
					}
				}
			}
			using (IEnumerator<Movimentacao> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Movimentacao movimentacao = enumerator.Current;
					if (!movimentacao.IsNot<IMovimentacaoPagamentoRecebimento>())
					{
						IMovimentacaoPagamento movimentacaoPagamento = movimentacao.To<IMovimentacaoPagamento>();
						if (movimentacaoPagamento == null || !movimentacaoPagamento.Pagamentos.ToListEmptyWhenNull<ItemPagamento>().Any<ItemPagamento>())
						{
							IMovimentacaoPagamentoRecebimento movimentacaoPagamentoRecebimento = movimentacao as IMovimentacaoPagamentoRecebimento;
							list.AddRange(from x in movimentacaoPagamentoRecebimento.PagamentoRecebimento.Parcelas
								where !(x is IParcelaRecebimentoTaxa)
								select x into p
								select new ResumoVendasDto
								{
									Descricao = p.EspeciePagamento.Descricao,
									Valor = p.Valor,
									Data = movimentacao.DataHoraEmissao
								});
							list.AddRange(from p in movimentacaoPagamentoRecebimento.PagamentoRecebimento.Parcelas.OfType<IParcelaRecebimentoTaxa>()
								select new ResumoVendasDto
								{
									Descricao = p.EspeciePagamento.Descricao,
									Valor = p.Valor,
									Data = movimentacao.DataHoraEmissao
								});
						}
					}
				}
			}
			IEnumerable<Movimentacao> enumerable2 = this._movimentacaoRepositoryRead.Find(specificationMovimentacao);
			IEnumerable<IItem> enumerable3 = enumerable2.SelectMany<Movimentacao, IItem>((Movimentacao x) => x.Itens);
			IEnumerable<IItemDocumentoAuxiliarProduto> enumerable4 = enumerable3.Where<IItem>((IItem item) => !item.Cancelado).OfType<IItemDocumentoAuxiliarProduto>();
			IEnumerable<IItemDocumentoAuxiliarServico> enumerable5 = enumerable3.Where<IItem>((IItem item) => !item.Cancelado).OfType<IItemDocumentoAuxiliarServico>();
			List<TotalMovimentadoPorTributacaoItemDto> list3 = (from p in enumerable4
				group p by new
				{
					p.Percentual.SituacaoTributaria.Tipo,
					p.Aliquota,
					p.Percentual.SituacaoTributaria.Codigo
				}).Select(delegate(g)
			{
				TotalMovimentadoPorTributacaoItemDto totalMovimentadoPorTributacaoItemDto = new TotalMovimentadoPorTributacaoItemDto();
				totalMovimentadoPorTributacaoItemDto.Tributacao = g.Key.Tipo.RetornarDescricaoPeloValor<DescricaoAttribute>();
				totalMovimentadoPorTributacaoItemDto.Aliquota = g.Key.Aliquota;
				totalMovimentadoPorTributacaoItemDto.Total = g.Sum<IItemDocumentoAuxiliarProduto>((IItemDocumentoAuxiliarProduto x) => x.Total);
				return totalMovimentadoPorTributacaoItemDto;
			}).ToListEmptyWhenNull<TotalMovimentadoPorTributacaoItemDto>();
			List<TotalMovimentadoPorTributacaoItemDto> list4 = (from s in enumerable5
				group s by new
				{
					s.ISS.CstIss.Tipo,
					s.Aliquota,
					s.ISS.CstIss.Codigo
				}).Select(delegate(g)
			{
				TotalMovimentadoPorTributacaoItemDto totalMovimentadoPorTributacaoItemDto2 = new TotalMovimentadoPorTributacaoItemDto();
				totalMovimentadoPorTributacaoItemDto2.Tributacao = g.Key.Tipo.RetornarDescricaoPeloValor<DescricaoAttribute>();
				totalMovimentadoPorTributacaoItemDto2.Aliquota = g.Key.Aliquota;
				totalMovimentadoPorTributacaoItemDto2.Total = g.Sum<IItemDocumentoAuxiliarServico>((IItemDocumentoAuxiliarServico x) => x.Total);
				return totalMovimentadoPorTributacaoItemDto2;
			}).ToListEmptyWhenNull<TotalMovimentadoPorTributacaoItemDto>();
			List<ItemRelatorioBaseDto> list5 = new List<ItemRelatorioBaseDto>();
			foreach (Movimentacao movimentacao2 in enumerable2)
			{
				foreach (IItemDocumentoFiscal itemDocumentoFiscal in from i in movimentacao2.Itens.OfType<IItemDocumentoFiscal>()
					where !i.Cancelado
					select i)
				{
					list5.Add(new ItemRelatorioBaseDto
					{
						TotalBruto = itemDocumentoFiscal.TotalBruto,
						TotalLiquido = itemDocumentoFiscal.TotalLiquido,
						TotalDesconto = itemDocumentoFiscal.TotalDesconto,
						TotalOutrasDespesas = itemDocumentoFiscal.TotalOutrasDespesas
					});
				}
			}
			return new DadosRelatorioResumoVendasDto
			{
				ResumosVendas = list,
				Produtos = list3,
				Servicos = list4,
				Totais = list5
			};
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0007D5A4 File Offset: 0x0007B7A4
		private static string RetornarUsuario(IEnumerable<UsuarioBase> usuarios, Movimentacao movimentacao)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass35_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass35_0();
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass35_0 CS$<>8__locals2 = CS$<>8__locals1;
			IEnumerable<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico> historicos = movimentacao.Historicos;
			string text;
			if (historicos == null)
			{
				text = null;
			}
			else
			{
				DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico historico = historicos.FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>();
				if (historico == null)
				{
					text = null;
				}
				else
				{
					string nomeUsuario = historico.NomeUsuario;
					text = ((nomeUsuario != null) ? nomeUsuario.ToLower() : null);
				}
			}
			CS$<>8__locals2.usuarioDaMovimentacao = text ?? "Nenhum";
			if (CS$<>8__locals1.usuarioDaMovimentacao.Equals("Nenhum"))
			{
				return usuarios.FirstOrDefault<UsuarioBase>((UsuarioBase x) => x.Nome.ToLower().Equals(CS$<>8__locals1.usuarioDaMovimentacao) || x.AcessoAlternativo.ToLower().Equals(CS$<>8__locals1.usuarioDaMovimentacao)).Nome ?? "Usuário não localizado";
			}
			return CS$<>8__locals1.usuarioDaMovimentacao;
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0007D62C File Offset: 0x0007B82C
		public TotalMovimentadoPorTributacaoDto TotalVendidoPorTributacao(DateTime dataInicial, DateTime dataFinal, string usuarioReferencia, int origemMovimentacao, int situacaoMovimentacao, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<UsuarioBase> enumerable;
			if (string.IsNullOrEmpty(usuarioReferencia))
			{
				enumerable = this._usuarioRepositoryRead.Find(UsuarioSpecifications.AtivoExcetoUsuarioSuporteERevendaEPedirMenu());
			}
			else
			{
				IEnumerable<UsuarioBase> enumerable2 = new List<UsuarioBase> { this._usuarioRepositoryRead.GetById(ObjectId.Parse(usuarioReferencia)) };
				enumerable = enumerable2;
			}
			IEnumerable<UsuarioBase> enumerable3 = enumerable;
			UsuarioBase usuarioFiltroMovimentacao = enumerable3.FirstOrDefault<UsuarioBase>((UsuarioBase x) => x.Id.Equals(usuarioReferencia.TryParseReferencia()));
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
				.PorSituacao(situacaoMovimentacao)
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable4 = this._movimentacaoRepositoryRead.Find(specification);
			object obj;
			if (string.IsNullOrEmpty(usuarioReferencia))
			{
				obj = enumerable4.SelectMany<Movimentacao, IItem>((Movimentacao x) => x.Itens);
			}
			else
			{
				Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> <>9__2;
				obj = enumerable4.Where<Movimentacao>(delegate(Movimentacao m)
				{
					IEnumerable<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico> historicos = m.Historicos;
					Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = (DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico h) => h.NomeUsuario.ToLower().Equals(usuarioFiltroMovimentacao.Nome.ToLower()) || h.NomeUsuario.ToLower().Equals(usuarioFiltroMovimentacao.AcessoAlternativo.ToLower()));
					}
					return historicos.Any<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>(func);
				}).SelectMany<Movimentacao, IItem>((Movimentacao x) => x.Itens);
			}
			object obj2 = obj;
			IEnumerable<IItemDocumentoAuxiliarProduto> enumerable5 = obj2.OfType<IItemDocumentoAuxiliarProduto>();
			IEnumerable<IItemDocumentoAuxiliarServico> enumerable6 = obj2.OfType<IItemDocumentoAuxiliarServico>();
			List<TotalMovimentadoPorTributacaoItemDto> list = (from p in enumerable5
				group p by new
				{
					p.Percentual.SituacaoTributaria.Tipo,
					p.Aliquota,
					p.Percentual.SituacaoTributaria.Codigo
				}).Select(delegate(g)
			{
				TotalMovimentadoPorTributacaoItemDto totalMovimentadoPorTributacaoItemDto = new TotalMovimentadoPorTributacaoItemDto();
				totalMovimentadoPorTributacaoItemDto.Tributacao = g.Key.Tipo.RetornarDescricao();
				totalMovimentadoPorTributacaoItemDto.CST = g.Key.Codigo.ToString("000");
				totalMovimentadoPorTributacaoItemDto.Aliquota = g.Key.Aliquota;
				totalMovimentadoPorTributacaoItemDto.Total = g.Sum<IItemDocumentoAuxiliarProduto>((IItemDocumentoAuxiliarProduto x) => x.Total);
				totalMovimentadoPorTributacaoItemDto.Acrescimo = g.Sum<IItemDocumentoAuxiliarProduto>((IItemDocumentoAuxiliarProduto x) => x.TotalOutrasDespesas);
				totalMovimentadoPorTributacaoItemDto.Desconto = g.Sum<IItemDocumentoAuxiliarProduto>((IItemDocumentoAuxiliarProduto x) => x.TotalDesconto);
				return totalMovimentadoPorTributacaoItemDto;
			}).ToListEmptyWhenNull<TotalMovimentadoPorTributacaoItemDto>();
			List<TotalMovimentadoPorTributacaoItemDto> list2 = (from s in enumerable6
				group s by new
				{
					s.ISS.CstIss.Tipo,
					s.Aliquota,
					s.ISS.CstIss.Codigo
				}).Select(delegate(g)
			{
				TotalMovimentadoPorTributacaoItemDto totalMovimentadoPorTributacaoItemDto2 = new TotalMovimentadoPorTributacaoItemDto();
				totalMovimentadoPorTributacaoItemDto2.Tributacao = g.Key.Tipo.RetornarDescricao();
				totalMovimentadoPorTributacaoItemDto2.CST = g.Key.Codigo.ToString("000");
				totalMovimentadoPorTributacaoItemDto2.Aliquota = g.Key.Aliquota;
				totalMovimentadoPorTributacaoItemDto2.Total = g.Sum<IItemDocumentoAuxiliarServico>((IItemDocumentoAuxiliarServico x) => x.Total);
				totalMovimentadoPorTributacaoItemDto2.Acrescimo = g.Sum<IItemDocumentoAuxiliarServico>((IItemDocumentoAuxiliarServico x) => x.TotalOutrasDespesas);
				totalMovimentadoPorTributacaoItemDto2.Desconto = g.Sum<IItemDocumentoAuxiliarServico>((IItemDocumentoAuxiliarServico x) => x.TotalDesconto);
				return totalMovimentadoPorTributacaoItemDto2;
			}).ToListEmptyWhenNull<TotalMovimentadoPorTributacaoItemDto>();
			return new TotalMovimentadoPorTributacaoDto
			{
				Produtos = list,
				Servicos = list2
			};
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0007D804 File Offset: 0x0007BA04
		public IEnumerable<ComposicaoVendidaDto> ObterComposicoesVendidas(DateTime dataInicial, DateTime dataFinal, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<ProdutoServicoEmpresa> resultado = this._produtoServicoEmpresaRepositoryRead.Find(ProdutoServicoSpecifications.Ativo().Composicao(), usuarioIdentity, null);
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal)
				.PorProdutoServicoEmpresaReferencia(resultado.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa x) => x.Id))
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>());
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			Func<IItemDocumentoFiscal, bool> <>9__2;
			List<<>f__AnonymousType98<ObjectId, string, string, decimal, decimal, decimal, decimal>> itens = (from c in enumerable.SelectMany<Movimentacao, IItemDocumentoFiscal>(delegate(Movimentacao movimentacao)
				{
					IEnumerable<IItemDocumentoFiscal> enumerable2 = movimentacao.ItensBase.OfType<IItemDocumentoFiscal>();
					Func<IItemDocumentoFiscal, bool> func;
					if ((func = <>9__2) == null)
					{
						func = (<>9__2 = delegate(IItemDocumentoFiscal item)
						{
							if (!item.Cancelado && item.OperacaoFiscal.MovimentaEstoque && item.OperacaoFiscal.MovimentaFinanceiro)
							{
								return resultado.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa b) => b.Id).Contains(item.ProdutoServicoEmpresaReferencia);
							}
							return false;
						});
					}
					return enumerable2.Where<IItemDocumentoFiscal>(func);
				})
				select new
				{
					c.ProdutoServicoEmpresaReferencia,
					c.ProdutoServico.CodigoInterno,
					c.ProdutoServico.Descricao,
					c.Quantidade,
					c.PrecoUnitario,
					c.Total,
					c.TotalDesconto
				} into g
				group g by new { g.ProdutoServicoEmpresaReferencia, g.CodigoInterno, g.Descricao, g.PrecoUnitario } into arg
				select new
				{
					ProdutoServicoEmpresaReferencia = arg.Key.ProdutoServicoEmpresaReferencia,
					CodigoInterno = arg.Key.CodigoInterno,
					Descricao = arg.Key.Descricao,
					PrecoUnitario = arg.Key.PrecoUnitario,
					Quantidade = arg.Sum(x => x.Quantidade),
					Total = arg.Sum(x => x.Total),
					TotalDesconto = arg.Sum(x => x.TotalDesconto)
				}).ToList();
			List<ObjectId> list = (from zz in resultado.Where<ProdutoServicoEmpresa>((ProdutoServicoEmpresa x) => x.Id.In(itens.Select(z => z.ProdutoServicoEmpresaReferencia).ToArray<ObjectId>())).SelectMany<ProdutoServicoEmpresa, Componente>((ProdutoServicoEmpresa p) => (p.ProdutoServico as Composicao).Componentes)
				select zz.ProdutoReferencia).ToListEmptyWhenNull<ObjectId>();
			IEnumerable<ProdutoServico> componentes = this._produtoServicoRepositoryRead.GetIn<ObjectId>((ProdutoServico x) => x.Id, list);
			return (from item in itens
				let composicao = resultado.FirstOrDefault<ProdutoServicoEmpresa>((ProdutoServicoEmpresa x) => x.ProdutoServico.CodigoInterno.Equals(item.CodigoInterno)).ProdutoServico as Composicao
				select new ComposicaoVendidaDto
				{
					Codigo = item.CodigoInterno,
					Desconto = item.TotalDesconto,
					Descricao = item.Descricao,
					Preco = item.PrecoUnitario,
					Quantidade = item.Quantidade,
					Total = item.Total,
					Compontes = composicao.Componentes.Select<Componente, DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios.ComponenteVendidoDto>(delegate(Componente c)
					{
						DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios.ComponenteVendidoDto componenteVendidoDto = new DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios.ComponenteVendidoDto();
						ProdutoServico produtoServico = componentes.FirstOrDefault<ProdutoServico>((ProdutoServico x) => x.Id.Equals(c.ProdutoReferencia));
						componenteVendidoDto.Codigo = ((produtoServico != null) ? produtoServico.CodigoInterno : null) ?? c.ProdutoReferencia.ToStringEmptyWhenDefault();
						componenteVendidoDto.Descricao = c.ProdutoDescricao;
						componenteVendidoDto.Quantidade = c.Quantidade * item.Quantidade;
						return componenteVendidoDto;
					}).ToList<DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios.ComponenteVendidoDto>()
				}).ToList<ComposicaoVendidaDto>();
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0007DA14 File Offset: 0x0007BC14
		public IEnumerable<ItemMovimentadoDto> ObterItensVendidos(DateTime dataInicial, DateTime dataFinal, TipoProdutoServico tipo, bool consolidado, bool agruparPorValorUnitario, IUsuarioIdentity usuarioIdentity)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass38_1 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass38_1();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.produtosServicos = this._produtoServicoRepositoryRead.Find(ProdutoServicoSpecifications.PorTipo(tipo));
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacaoConcluido().PorDataHoraEmissao(dataInicial, dataFinal).PorProdutoServicoReferencia(CS$<>8__locals1.produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>())
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass38_0 CS$<>8__locals2 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass38_0();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.empresa = enumerator.Current;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(CS$<>8__locals2.empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> func;
					if ((func = CS$<>8__locals2.CS$<>8__locals1.<>9__4) == null)
					{
						func = (CS$<>8__locals2.CS$<>8__locals1.<>9__4 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoFiscal> enumerable4 = m.Itens.OfType<IItemDocumentoFiscal>();
							Func<IItemDocumentoFiscal, bool> func2;
							if ((func2 = CS$<>8__locals2.CS$<>8__locals1.<>9__5) == null)
							{
								func2 = (CS$<>8__locals2.CS$<>8__locals1.<>9__5 = delegate(IItemDocumentoFiscal i)
								{
									if (!i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro)
									{
										return i.ProdutoServico.ProdutoServicoReferencia.In(CS$<>8__locals2.CS$<>8__locals1.produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable4.Where<IItemDocumentoFiscal>(func2);
						});
					}
					IEnumerable<IItemDocumentoFiscal> enumerable3 = enumerable2.SelectMany<Movimentacao, IItemDocumentoFiscal>(func);
					List<IItemSaidaCombustivel> list3 = enumerable3.OfType<IItemSaidaCombustivel>().ToListEmptyWhenNull<IItemSaidaCombustivel>();
					IEnumerable<RegistroAbastecimento> abastecimentos = this._abastecimentoRepositoryRead.GetIn<string>((RegistroAbastecimento x) => x.Abastecimento.Id, list3.Select<IItemSaidaCombustivel, string>((IItemSaidaCombustivel x) => x.AbastecimentoReferencia));
					List<Estoque> estoques = this._estoqueRepositoryRead.GetIn<ObjectId?>((Estoque x) => (ObjectId?)x.Id, enumerable3.Select<IItemDocumentoFiscal, ObjectId?>(delegate(IItemDocumentoFiscal x)
					{
						IItemDocumentoFiscalProduto itemDocumentoFiscalProduto = x.To<IItemDocumentoFiscalProduto>();
						if (itemDocumentoFiscalProduto == null)
						{
							return null;
						}
						return new ObjectId?(itemDocumentoFiscalProduto.EstoqueMovimentacao.Id);
					})).ToList<Estoque>();
					if (agruparPorValorUnitario)
					{
						list2.AddRange((from g in enumerable3.Select(delegate(IItemDocumentoFiscal item)
							{
								ObjectId produtoServicoEmpresaReferencia = item.ProdutoServicoEmpresaReferencia;
								string codigoInterno = item.ProdutoServico.CodigoInterno;
								string descricao = item.ProdutoServico.Descricao;
								decimal quantidade = item.Quantidade;
								decimal precoUnitario = item.PrecoUnitario;
								decimal totalDesconto = item.TotalDesconto;
								decimal total = item.Total;
								IItemDocumentoFiscalProduto itemDocumentoFiscalProduto2 = item.To<IItemDocumentoFiscalProduto>();
								return new
								{
									ProdutoServicoEmpresaReferencia = produtoServicoEmpresaReferencia,
									CodigoInterno = codigoInterno,
									Descricao = descricao,
									Quantidade = quantidade,
									PrecoUnitario = precoUnitario,
									TotalDesconto = totalDesconto,
									Total = total,
									EstoqueMovimentacao = ((itemDocumentoFiscalProduto2 != null) ? itemDocumentoFiscalProduto2.EstoqueMovimentacao : null),
									TotalOutrasDespesas = item.TotalOutrasDespesas
								};
							})
							group g by new { g.ProdutoServicoEmpresaReferencia, g.CodigoInterno, g.Descricao, g.PrecoUnitario }).Select(delegate(arg)
						{
							ItemMovimentadoDto itemMovimentadoDto = new ItemMovimentadoDto();
							itemMovimentadoDto.CodigoInterno = arg.Key.CodigoInterno;
							itemMovimentadoDto.Descricao = arg.Key.Descricao;
							itemMovimentadoDto.Preco = arg.Key.PrecoUnitario;
							itemMovimentadoDto.Quantidade = arg.Sum(x => x.Quantidade);
							itemMovimentadoDto.Desconto = arg.Sum(x => x.TotalDesconto);
							itemMovimentadoDto.AgruparPorValorUnitario = true;
							itemMovimentadoDto.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
							itemMovimentadoDto.Total = arg.Sum(x => x.Total);
							itemMovimentadoDto.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
							Estoque estoque = estoques.FirstOrDefault<Estoque>(delegate(Estoque x)
							{
								ObjectId id = x.Id;
								var anon = arg.FirstOrDefault();
								ObjectId? objectId;
								if (anon == null)
								{
									objectId = null;
								}
								else
								{
									EstoqueMovimentacao estoqueMovimentacao = anon.EstoqueMovimentacao;
									objectId = ((estoqueMovimentacao != null) ? new ObjectId?(estoqueMovimentacao.Id) : null);
								}
								return id == objectId;
							});
							itemMovimentadoDto.Saldo = ((estoque != null) ? estoque.QuantidadeSaldo : 0m);
							itemMovimentadoDto.Empresa = CS$<>8__locals2.empresa.Nome;
							itemMovimentadoDto.CnpjEmpresa = CS$<>8__locals2.empresa.CpfCnpjFormatado;
							itemMovimentadoDto.Componentes = CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObterComponentesItensVendidos(CS$<>8__locals2.CS$<>8__locals1.produtosServicos.FirstOrDefault<ProdutoServico>((ProdutoServico x) => x.CodigoInterno == arg.Key.CodigoInterno), arg.Sum(x => x.Quantidade));
							itemMovimentadoDto.Abastecimentos = CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObterAbastecimentosItensVendidos(abastecimentos.Where<RegistroAbastecimento>((RegistroAbastecimento x) => x.Abastecimento.IdProduto.Equals(arg.Key.ProdutoServicoEmpresaReferencia.ToString())), arg.Key.PrecoUnitario);
							itemMovimentadoDto.QuantidadeMovimentacoes = arg.Count();
							return itemMovimentadoDto.ProcessarEstoque().AgruparEstoque();
						}));
					}
					else
					{
						list2.AddRange((from g in enumerable3.Select(delegate(IItemDocumentoFiscal item)
							{
								ObjectId produtoServicoEmpresaReferencia2 = item.ProdutoServicoEmpresaReferencia;
								string codigoInterno2 = item.ProdutoServico.CodigoInterno;
								string descricao2 = item.ProdutoServico.Descricao;
								decimal quantidade2 = item.Quantidade;
								decimal precoUnitario2 = item.PrecoUnitario;
								decimal totalDesconto2 = item.TotalDesconto;
								decimal total2 = item.Total;
								decimal totalBruto = item.TotalBruto;
								IItemDocumentoFiscalProduto itemDocumentoFiscalProduto3 = item.To<IItemDocumentoFiscalProduto>();
								return new
								{
									ProdutoServicoEmpresaReferencia = produtoServicoEmpresaReferencia2,
									CodigoInterno = codigoInterno2,
									Descricao = descricao2,
									Quantidade = quantidade2,
									PrecoUnitario = precoUnitario2,
									TotalDesconto = totalDesconto2,
									Total = total2,
									TotalBruto = totalBruto,
									EstoqueMovimentacao = ((itemDocumentoFiscalProduto3 != null) ? itemDocumentoFiscalProduto3.EstoqueMovimentacao : null),
									TotalOutrasDespesas = item.TotalOutrasDespesas
								};
							})
							group g by new { g.ProdutoServicoEmpresaReferencia, g.CodigoInterno, g.Descricao }).Select(delegate(arg)
						{
							ItemMovimentadoDto itemMovimentadoDto2 = new ItemMovimentadoDto();
							itemMovimentadoDto2.CodigoInterno = arg.Key.CodigoInterno;
							itemMovimentadoDto2.Descricao = arg.Key.Descricao;
							itemMovimentadoDto2.Preco = (arg.Sum(x => x.Total) / Math.Max(1m, arg.Sum(x => x.Quantidade))).TruncarDuasCasasDecimais();
							itemMovimentadoDto2.PrecoBruto = (arg.Sum(x => x.TotalBruto) / Math.Max(1m, arg.Sum(x => x.Quantidade))).TruncarDuasCasasDecimais();
							itemMovimentadoDto2.Quantidade = arg.Sum(x => x.Quantidade);
							itemMovimentadoDto2.Desconto = arg.Sum(x => x.TotalDesconto);
							itemMovimentadoDto2.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
							itemMovimentadoDto2.Total = arg.Sum(x => x.Total);
							itemMovimentadoDto2.AgruparPorValorUnitario = false;
							itemMovimentadoDto2.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
							Estoque estoque2 = estoques.FirstOrDefault<Estoque>(delegate(Estoque x)
							{
								ObjectId id2 = x.Id;
								var anon2 = arg.FirstOrDefault();
								ObjectId? objectId2;
								if (anon2 == null)
								{
									objectId2 = null;
								}
								else
								{
									EstoqueMovimentacao estoqueMovimentacao2 = anon2.EstoqueMovimentacao;
									objectId2 = ((estoqueMovimentacao2 != null) ? new ObjectId?(estoqueMovimentacao2.Id) : null);
								}
								return id2 == objectId2;
							});
							itemMovimentadoDto2.Saldo = ((estoque2 != null) ? estoque2.QuantidadeSaldo : 0m);
							itemMovimentadoDto2.Empresa = CS$<>8__locals2.empresa.Nome;
							itemMovimentadoDto2.CnpjEmpresa = CS$<>8__locals2.empresa.CpfCnpjFormatado;
							itemMovimentadoDto2.Componentes = CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObterComponentesItensVendidos(CS$<>8__locals2.CS$<>8__locals1.produtosServicos.FirstOrDefault<ProdutoServico>((ProdutoServico x) => x.CodigoInterno == arg.Key.CodigoInterno), arg.Sum(x => x.Quantidade));
							itemMovimentadoDto2.Abastecimentos = CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObterAbastecimentosItensVendidos(abastecimentos.Where<RegistroAbastecimento>((RegistroAbastecimento x) => x.Abastecimento.IdProduto.Equals(arg.Key.ProdutoServicoEmpresaReferencia.ToString())), (arg.Sum(x => x.Total) / Math.Max(1m, arg.Sum(x => x.Quantidade))).TruncarDuasCasasDecimais());
							itemMovimentadoDto2.QuantidadeMovimentacoes = arg.Count();
							return itemMovimentadoDto2.ProcessarEstoque().AgruparEstoque();
						}));
					}
				}
			}
			return list2;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0007DDEC File Offset: 0x0007BFEC
		public IEnumerable<ItemMovimentadoDto> ObterItensVendidosComDadosPessoa(DateTime dataInicial, DateTime dataFinal, TipoProdutoServico tipo, bool consolidado, CodigoDescricaoDto situacaoMovimentacao, CodigoDescricaoDto origemPesquisaMovimentacao, int naturezaOperacao, string pessoaReferencia, string vendedorReferencia, string produtoServicoReferencia, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<ProdutoServico> specification = ProdutoServicoSpecifications.PorTipo(tipo);
			if (!string.IsNullOrEmpty(produtoServicoReferencia))
			{
				specification = ProdutoServicoSpecifications.PorProdutoServicoId(produtoServicoReferencia);
			}
			IEnumerable<ProdutoServico> produtosServicos = this._produtoServicoRepositoryRead.Find(specification);
			ISpecification<Movimentacao> specification2 = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorPessoa(pessoaReferencia).PorVendedor(vendedorReferencia).PorSituacao(situacaoMovimentacao.Codigo)
				.PorOrigem(origemPesquisaMovimentacao.Codigo)
				.PorDataEmissao(dataInicial, dataFinal)
				.PorProdutoServicoReferencia(produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>())
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			if (naturezaOperacao > 0)
			{
				specification2 = specification2.PorNaturezaOperacao(naturezaOperacao);
			}
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification2);
			List<Pessoa> pessoas = this._pessoaRepositoryRead.GetIn<ObjectId>((Pessoa x) => x.Id, enumerable.Select<Movimentacao, ObjectId>((Movimentacao y) => y.Pessoa.PessoaReferencia)).ToListEmptyWhenNull<Pessoa>();
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoFiscal, bool> <>9__5;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					using (IEnumerator<Movimentacao> enumerator2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia)).GetEnumerator())
					{
						Func<<>f__AnonymousType37<string, string, string, decimal, decimal, string, string, string, string, string, long, DateTime, DateTime?, string>, ItemMovimentadoDto> <>9__9;
						while (enumerator2.MoveNext())
						{
							Movimentacao movimentacao = enumerator2.Current;
							IEnumerable<IItemDocumentoFiscal> enumerable2 = movimentacao.Itens.OfType<IItemDocumentoFiscal>();
							Func<IItemDocumentoFiscal, bool> func;
							if ((func = <>9__5) == null)
							{
								func = (<>9__5 = delegate(IItemDocumentoFiscal i)
								{
									if (!i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro)
									{
										return i.ProdutoServico.ProdutoServicoReferencia.In(produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							IEnumerable<IItemDocumentoFiscal> enumerable3 = enumerable2.Where<IItemDocumentoFiscal>(func);
							List<ItemMovimentadoDto> list3 = list2;
							Func<Pessoa, bool> <>9__8;
							var enumerable4 = enumerable3.Select(delegate(IItemDocumentoFiscal item)
							{
								string codigoInterno = item.ProdutoServico.CodigoInterno;
								ProdutoHistorico produtoHistorico = item.ProdutoServico.To<ProdutoHistorico>();
								string text = ((produtoHistorico != null) ? produtoHistorico.CodigoBarras : null);
								string descricao = item.ProdutoServico.Descricao;
								decimal quantidade = item.Quantidade;
								decimal total = item.Total;
								EnderecoHistorico enderecoPrincipal = movimentacao.Pessoa.EnderecoPrincipal;
								string text2;
								if (enderecoPrincipal == null)
								{
									text2 = null;
								}
								else
								{
									Municipio municipio = enderecoPrincipal.Municipio;
									if (municipio == null)
									{
										text2 = null;
									}
									else
									{
										UF uf = municipio.Uf;
										text2 = ((uf != null) ? uf.Sigla : null);
									}
								}
								EnderecoHistorico enderecoPrincipal2 = movimentacao.Pessoa.EnderecoPrincipal;
								string text3;
								if (enderecoPrincipal2 == null)
								{
									text3 = null;
								}
								else
								{
									Municipio municipio2 = enderecoPrincipal2.Municipio;
									text3 = ((municipio2 != null) ? municipio2.Nome : null);
								}
								string nome = movimentacao.Pessoa.Nome;
								IEnumerable<Pessoa> pessoas2 = pessoas;
								Func<Pessoa, bool> func3;
								if ((func3 = <>9__8) == null)
								{
									func3 = (<>9__8 = (Pessoa x) => x.Id.Equals(movimentacao.Pessoa.PessoaReferencia));
								}
								Pessoa pessoa = pessoas2.FirstOrDefault<Pessoa>(func3);
								string text4;
								if (pessoa == null)
								{
									text4 = null;
								}
								else
								{
									Segmentacao segmentacao = pessoa.Segmentacao;
									if (segmentacao == null)
									{
										text4 = null;
									}
									else
									{
										TipoSegmentacao tipo2 = segmentacao.Tipo;
										text4 = ((tipo2 != null) ? tipo2.Descricao : null);
									}
								}
								string text5 = text4 ?? "Sem segmentação definida";
								string cpfCnpjFormatado = movimentacao.Pessoa.CpfCnpjFormatado;
								long numeroMovimentacao = item.NumeroMovimentacao;
								DateTime dataHoraEmissao = movimentacao.DataHoraEmissao;
								NotaFiscal notaFiscal = movimentacao.To<NotaFiscal>();
								return new
								{
									CodigoInterno = codigoInterno,
									CodigoBarras = text,
									Descricao = descricao,
									Quantidade = quantidade,
									Total = total,
									Uf = text2,
									Municipio = text3,
									NomePessoa = nome,
									Segmentacao = text5,
									DocumentoPessoa = cpfCnpjFormatado,
									NumeroMovimentacao = numeroMovimentacao,
									DataHoraEmissao = dataHoraEmissao,
									DataHoraEntradaSaida = ((notaFiscal != null) ? new DateTime?(notaFiscal.DataHoraEntradaSaida) : null),
									TipoMovimentacao = movimentacao.Tipo.RetornarDescricaoCurta()
								};
							});
							var func2;
							if ((func2 = <>9__9) == null)
							{
								func2 = (<>9__9 = arg => new ItemMovimentadoDto
								{
									CodigoInterno = arg.CodigoInterno.ToStringEmptyWhenNull(),
									CodigoBarras = arg.CodigoBarras.ToStringEmptyWhenNull(),
									Descricao = arg.Descricao.ToStringEmptyWhenNull(),
									Quantidade = arg.Quantidade,
									Total = arg.Total,
									UfPessoa = arg.Uf.ToStringEmptyWhenNull(),
									MunicipioPessoa = arg.Municipio.ToStringEmptyWhenNull(),
									NomePessoa = arg.NomePessoa.ToStringEmptyWhenNull(),
									Segmentacao = arg.Segmentacao.ToStringEmptyWhenNull(),
									CpfCnpjPessoa = arg.DocumentoPessoa.ToStringEmptyWhenNull(),
									Numero = arg.NumeroMovimentacao,
									DataHora = arg.DataHoraEmissao,
									DataHoraSaida = (arg.DataHoraEntradaSaida ?? arg.DataHoraEmissao),
									Empresa = empresa.Nome,
									CnpjEmpresa = empresa.CpfCnpjFormatado,
									TipoMovimentacao = arg.TipoMovimentacao.ToStringEmptyWhenNull()
								});
							}
							list3.AddRange(enumerable4.Select(func2));
						}
					}
				}
			}
			return list2;
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0007E13C File Offset: 0x0007C33C
		public IEnumerable<ItemVendidoDto> ObterItensVendidosTotalizadosPorPessoa(DateTime dataInicial, DateTime dataFinal, bool consolidado, CodigoDescricaoDto situacaoMovimentacao, CodigoDescricaoDto origemPesquisaMovimentacao, int naturezaOperacao, string pessoaReferencia, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorPessoa(pessoaReferencia).PorSituacao(situacaoMovimentacao.Codigo).PorOrigem(origemPesquisaMovimentacao.Codigo)
				.PorDataEmissao(dataInicial, dataFinal)
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			if (naturezaOperacao > 0)
			{
				specification = specification.PorNaturezaOperacao(naturezaOperacao);
			}
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			Dictionary<ObjectId, EmpresaHistorico> empresasDict = (from m in enumerable
				group m by m.Empresa.PessoaReferencia).ToDictionary<IGrouping<ObjectId, Movimentacao>, ObjectId, EmpresaHistorico>((IGrouping<ObjectId, Movimentacao> g) => g.Key, (IGrouping<ObjectId, Movimentacao> g) => g.Last<Movimentacao>().Empresa);
			return (from x in enumerable.SelectMany(delegate(Movimentacao m)
				{
					EmpresaHistorico empresa = empresasDict[m.EmpresaReferencia];
					return (from i in m.Itens.OfType<IItemDocumentoFiscal>()
						where !i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro
						select i).Select(delegate(IItemDocumentoFiscal item)
					{
						string codigoInterno = item.ProdutoServico.CodigoInterno;
						ProdutoHistorico produtoHistorico = item.ProdutoServico.To<ProdutoHistorico>();
						return new
						{
							CodigoInterno = codigoInterno,
							CodigoBarras = ((produtoHistorico != null) ? produtoHistorico.CodigoBarras : null),
							Descricao = item.ProdutoServico.Descricao,
							Quantidade = item.Quantidade,
							Total = item.Total,
							NomePessoa = m.Pessoa.Nome,
							DocumentoPessoa = m.Pessoa.CpfCnpjFormatado,
							EmpresaNome = empresa.Nome,
							EmpresaCnpj = empresa.CpfCnpjFormatado
						};
					});
				})
				group x by new { x.NomePessoa, x.DocumentoPessoa, x.EmpresaNome, x.EmpresaCnpj, x.CodigoInterno }).Select(delegate(g)
			{
				ItemVendidoDto itemVendidoDto = new ItemVendidoDto();
				itemVendidoDto.NomePessoa = g.Key.NomePessoa.ToStringEmptyWhenNull();
				itemVendidoDto.CpfCnpjPessoa = g.Key.DocumentoPessoa.ToStringEmptyWhenNull();
				itemVendidoDto.Empresa = g.Key.EmpresaNome.ToStringEmptyWhenNull();
				itemVendidoDto.CnpjEmpresa = g.Key.EmpresaCnpj.ToStringEmptyWhenNull();
				itemVendidoDto.CodigoInterno = g.Key.CodigoInterno.ToStringEmptyWhenNull();
				itemVendidoDto.CodigoBarras = g.First().CodigoBarras.ToStringEmptyWhenNull();
				itemVendidoDto.Descricao = g.First().Descricao.ToStringEmptyWhenNull();
				itemVendidoDto.Quantidade = g.Sum(x => x.Quantidade);
				itemVendidoDto.Total = g.Sum(x => x.Total);
				decimal num = g.Sum(x => x.Total);
				decimal num2;
				if (!(g.Sum(x => x.Quantidade) == 0m))
				{
					num2 = g.Sum(x => x.Quantidade);
				}
				else
				{
					num2 = 1m;
				}
				itemVendidoDto.Preco = num / num2;
				return itemVendidoDto;
			});
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0007E29C File Offset: 0x0007C49C
		public IEnumerable<ItemMovimentadoDto> ObterItensVendidosPorPessoa(DateTime dataInicial, DateTime dataFinal, TipoProdutoServico tipo, bool consolidado, string pessoaReferencia, int origemMovimentacao, string produtoServicoReferencia, CodigoDescricaoDto situacaoMovimentacao, int naturezaOperacao, IUsuarioIdentity usuarioIdentity)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass41_1 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass41_1();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.produtosServicos = new List<ProdutoServico>();
			if (produtoServicoReferencia.IsNullOrEmpty())
			{
				CS$<>8__locals1.produtosServicos.AddRange(this._produtoServicoRepositoryRead.Find(ProdutoServicoSpecifications.PorTipo(tipo)));
			}
			else
			{
				CS$<>8__locals1.produtosServicos.Add(this._produtoServicoRepositoryRead.GetById(produtoServicoReferencia.TryParseReferencia()));
			}
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorPessoa(pessoaReferencia).PorSituacao(situacaoMovimentacao.Codigo).PorDataHoraEmissao(dataInicial, dataFinal)
				.PorProdutoServicoReferencia(CS$<>8__locals1.produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>())
				.PorOrigem(origemMovimentacao)
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			if (naturezaOperacao > 0)
			{
				specification = specification.PorNaturezaOperacao(naturezaOperacao);
			}
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass41_0 CS$<>8__locals2 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass41_0();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.empresa = enumerator.Current;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(CS$<>8__locals2.empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> func;
					if ((func = CS$<>8__locals2.CS$<>8__locals1.<>9__4) == null)
					{
						func = (CS$<>8__locals2.CS$<>8__locals1.<>9__4 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoFiscal> enumerable4 = m.Itens.OfType<IItemDocumentoFiscal>();
							Func<IItemDocumentoFiscal, bool> func2;
							if ((func2 = CS$<>8__locals2.CS$<>8__locals1.<>9__5) == null)
							{
								func2 = (CS$<>8__locals2.CS$<>8__locals1.<>9__5 = delegate(IItemDocumentoFiscal i)
								{
									if (!i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro)
									{
										return i.ProdutoServico.ProdutoServicoReferencia.In(CS$<>8__locals2.CS$<>8__locals1.produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable4.Where<IItemDocumentoFiscal>(func2);
						});
					}
					IEnumerable<IItemDocumentoFiscal> enumerable3 = enumerable2.SelectMany<Movimentacao, IItemDocumentoFiscal>(func);
					List<IItemSaidaCombustivel> list3 = enumerable3.OfType<IItemSaidaCombustivel>().ToListEmptyWhenNull<IItemSaidaCombustivel>();
					IEnumerable<RegistroAbastecimento> abastecimentos = this._abastecimentoRepositoryRead.GetIn<string>((RegistroAbastecimento x) => x.Abastecimento.Id, list3.Select<IItemSaidaCombustivel, string>((IItemSaidaCombustivel x) => x.AbastecimentoReferencia));
					list2.AddRange((from g in enumerable3.Select(delegate(IItemDocumentoFiscal item)
						{
							ObjectId produtoServicoEmpresaReferencia = item.ProdutoServicoEmpresaReferencia;
							string codigoInterno = item.ProdutoServico.CodigoInterno;
							string descricao = item.ProdutoServico.Descricao;
							decimal quantidade = item.Quantidade;
							decimal precoUnitario = item.PrecoUnitario;
							decimal totalDesconto = item.TotalDesconto;
							decimal total = item.Total;
							IItemDocumentoFiscalProduto itemDocumentoFiscalProduto = item.To<IItemDocumentoFiscalProduto>();
							return new
							{
								ProdutoServicoEmpresaReferencia = produtoServicoEmpresaReferencia,
								CodigoInterno = codigoInterno,
								Descricao = descricao,
								Quantidade = quantidade,
								PrecoUnitario = precoUnitario,
								TotalDesconto = totalDesconto,
								Total = total,
								EstoqueMovimentacao = ((itemDocumentoFiscalProduto != null) ? itemDocumentoFiscalProduto.EstoqueMovimentacao : null),
								TotalOutrasDespesas = item.TotalOutrasDespesas
							};
						})
						group g by new { g.ProdutoServicoEmpresaReferencia, g.CodigoInterno, g.Descricao, g.PrecoUnitario }).Select(delegate(arg)
					{
						ItemMovimentadoDto itemMovimentadoDto = new ItemMovimentadoDto();
						itemMovimentadoDto.CodigoInterno = arg.Key.CodigoInterno;
						itemMovimentadoDto.Descricao = arg.Key.Descricao;
						itemMovimentadoDto.Preco = arg.Key.PrecoUnitario;
						itemMovimentadoDto.Quantidade = arg.Sum(x => x.Quantidade);
						itemMovimentadoDto.Desconto = arg.Sum(x => x.TotalDesconto);
						itemMovimentadoDto.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
						itemMovimentadoDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoDto.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
						itemMovimentadoDto.Empresa = CS$<>8__locals2.empresa.Nome;
						itemMovimentadoDto.CnpjEmpresa = CS$<>8__locals2.empresa.CpfCnpjFormatado;
						itemMovimentadoDto.Componentes = CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObterComponentesItensVendidos(CS$<>8__locals2.CS$<>8__locals1.produtosServicos.FirstOrDefault<ProdutoServico>((ProdutoServico x) => x.CodigoInterno == arg.Key.CodigoInterno), arg.Sum(x => x.Quantidade));
						itemMovimentadoDto.Abastecimentos = CS$<>8__locals2.CS$<>8__locals1.<>4__this.ObterAbastecimentosItensVendidos(abastecimentos.Where<RegistroAbastecimento>((RegistroAbastecimento x) => x.Abastecimento.IdProduto.Equals(arg.Key.ProdutoServicoEmpresaReferencia.ToString())), arg.Key.PrecoUnitario);
						itemMovimentadoDto.QuantidadeMovimentacoes = arg.Count();
						return itemMovimentadoDto.ProcessarEstoque().AgruparEstoque();
					}));
				}
			}
			return list2;
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0007E5B4 File Offset: 0x0007C7B4
		public DocumentosAuxiliaresPorPeriodoDto DavOuPreVendaOuOrcamentoPorNumero(TipoMovimentacao tipoMovimentacao, string numeracoesEspecificas, IUsuarioIdentity usuarioIdentity)
		{
			List<string> list = numeracoesEspecificas.Split(new char[] { ';' }).ToListEmptyWhenNull<string>();
			List<Movimentacao> list2 = new List<Movimentacao>();
			foreach (string text in list)
			{
				long num;
				long.TryParse(text, out num);
				if (!num.Equals(0L))
				{
					ISpecification<Movimentacao> specification = EntityBaseEmpresaSpecification<Movimentacao>.PorEmpresa(usuarioIdentity.IdEmpresa).PorNumero(num);
					if (tipoMovimentacao != TipoMovimentacao.Dav)
					{
						if (tipoMovimentacao != TipoMovimentacao.PreVenda)
						{
							if (tipoMovimentacao == TipoMovimentacao.Orcamento)
							{
								specification = specification.PorOrigem(14);
							}
						}
						else
						{
							specification = specification.PorOrigem(4);
						}
					}
					else
					{
						specification = specification.PorOrigem(1);
					}
					list2.AddRange(this._movimentacaoRepositoryRead.Find(specification));
				}
			}
			List<DocumentoAuxiliarPeriodoMovimentacaoDto> list3 = list2.Select((Movimentacao movimentacao) => new
			{
				movimentacao = movimentacao,
				itens = movimentacao.Itens.Where<IItem>((IItem i) => !i.Cancelado)
			}).Select(delegate(<>h__TransparentIdentifier0)
			{
				DocumentoAuxiliarPeriodoMovimentacaoDto documentoAuxiliarPeriodoMovimentacaoDto = new DocumentoAuxiliarPeriodoMovimentacaoDto();
				documentoAuxiliarPeriodoMovimentacaoDto.Numero = <>h__TransparentIdentifier0.movimentacao.Numero;
				documentoAuxiliarPeriodoMovimentacaoDto.Cliente = <>h__TransparentIdentifier0.movimentacao.Pessoa.Nome.RemoverCaracteresNaoImprimiveis();
				documentoAuxiliarPeriodoMovimentacaoDto.Data = <>h__TransparentIdentifier0.movimentacao.DataHoraEmissao;
				DocumentoAuxiliarVenda documentoAuxiliarVenda = <>h__TransparentIdentifier0.movimentacao.To<DocumentoAuxiliarVenda>();
				documentoAuxiliarPeriodoMovimentacaoDto.DataPrevisao = ((documentoAuxiliarVenda != null) ? documentoAuxiliarVenda.DataHoraPrevisao : <>h__TransparentIdentifier0.movimentacao.DataHoraEmissao);
				documentoAuxiliarPeriodoMovimentacaoDto.TotalProdutos = <>h__TransparentIdentifier0.movimentacao.TotalProdutos;
				documentoAuxiliarPeriodoMovimentacaoDto.Situacao = <>h__TransparentIdentifier0.movimentacao.Situacao.Descricao;
				string text2;
				if (!<>h__TransparentIdentifier0.movimentacao.Is<IMovimentacaoPagamentoRecebimento>())
				{
					text2 = string.Empty;
				}
				else
				{
					text2 = string.Join("/", <>h__TransparentIdentifier0.movimentacao.To<IMovimentacaoPagamentoRecebimento>().PagamentoRecebimento.Parcelas.Select<Parcela, string>((Parcela parcela) => parcela.EspeciePagamento.Descricao).Distinct<string>());
				}
				documentoAuxiliarPeriodoMovimentacaoDto.Pagamento = text2;
				documentoAuxiliarPeriodoMovimentacaoDto.Itens = from item in <>h__TransparentIdentifier0.itens.ToListEmptyWhenNull<IItem>()
					select new DocumentoAuxiliarPeriodoItemDto
					{
						CodigoInterno = item.ProdutoServico.CodigoInterno,
						Descricao = item.ProdutoServico.Descricao,
						Quantidade = item.Quantidade,
						Preco = item.PrecoUnitario,
						Total = item.Total
					};
				documentoAuxiliarPeriodoMovimentacaoDto.Total = <>h__TransparentIdentifier0.movimentacao.Total;
				return documentoAuxiliarPeriodoMovimentacaoDto;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPeriodoMovimentacaoDto>();
			List<DocumentoAuxiliarPorPeriodoItemAcumuladoDto> list4 = (from g in list3.SelectMany<DocumentoAuxiliarPeriodoMovimentacaoDto, DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoMovimentacaoDto x) => x.Itens)
				group g by new { g.CodigoInterno, g.Descricao }).Select(delegate(s)
			{
				DocumentoAuxiliarPorPeriodoItemAcumuladoDto documentoAuxiliarPorPeriodoItemAcumuladoDto = new DocumentoAuxiliarPorPeriodoItemAcumuladoDto();
				documentoAuxiliarPorPeriodoItemAcumuladoDto.CodigoInterno = s.Key.CodigoInterno;
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Descricao = s.Key.Descricao;
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Quantidade = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Quantidade);
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Total = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Total);
				return documentoAuxiliarPorPeriodoItemAcumuladoDto;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoItemAcumuladoDto>();
			List<DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto> list5 = (from g in list3
				group g by new { g.Situacao } into s
				select new DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto
				{
					Situacao = s.Key.Situacao,
					Quantidade = s.Count<DocumentoAuxiliarPeriodoMovimentacaoDto>()
				}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto>();
			List<DocumentosAuxiliaresPorPeriodoResumoPagamentosDto> list6 = (from x in list2.OfType<IMovimentacaoPagamentoRecebimento>().SelectMany<IMovimentacaoPagamentoRecebimento, Parcela>((IMovimentacaoPagamentoRecebimento movimentacao) => movimentacao.PagamentoRecebimento.Parcelas).ToListEmptyWhenNull<Parcela>()
				group x by x.EspeciePagamento.Descricao).Select<IGrouping<string, Parcela>, DocumentosAuxiliaresPorPeriodoResumoPagamentosDto>(delegate(IGrouping<string, Parcela> g)
			{
				DocumentosAuxiliaresPorPeriodoResumoPagamentosDto documentosAuxiliaresPorPeriodoResumoPagamentosDto = new DocumentosAuxiliaresPorPeriodoResumoPagamentosDto();
				documentosAuxiliaresPorPeriodoResumoPagamentosDto.Descricao = g.Key;
				documentosAuxiliaresPorPeriodoResumoPagamentosDto.Total = g.Sum<Parcela>((Parcela x) => x.Valor);
				return documentosAuxiliaresPorPeriodoResumoPagamentosDto;
			}).ToListEmptyWhenNull<DocumentosAuxiliaresPorPeriodoResumoPagamentosDto>();
			return new DocumentosAuxiliaresPorPeriodoDto
			{
				Movimentacoes = list3,
				ItensAcumulados = list4,
				ResumoPagamentos = list6,
				ResumoSituacoes = list5
			};
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0007E834 File Offset: 0x0007CA34
		public IEnumerable<ItemMovimentadoDto> ObterItensVendidosComTributacao(DateTime dataInicial, DateTime dataFinal, TipoProdutoServico tipo, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<ProdutoServico> produtosServicos = this._produtoServicoRepositoryRead.Find(ProdutoServicoSpecifications.PorTipo(tipo));
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal).PorProdutoServicoReferencia(produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>())
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoFiscal, bool> <>9__5;
				Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> <>9__4;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> func;
					if ((func = <>9__4) == null)
					{
						func = (<>9__4 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoFiscal> enumerable4 = m.Itens.OfType<IItemDocumentoFiscal>();
							Func<IItemDocumentoFiscal, bool> func2;
							if ((func2 = <>9__5) == null)
							{
								func2 = (<>9__5 = delegate(IItemDocumentoFiscal i)
								{
									if (!i.Cancelado)
									{
										return i.ProdutoServico.ProdutoServicoReferencia.In(produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable4.Where<IItemDocumentoFiscal>(func2);
						});
					}
					IEnumerable<IItemDocumentoFiscal> enumerable3 = enumerable2.SelectMany<Movimentacao, IItemDocumentoFiscal>(func);
					list2.AddRange((from g in enumerable3.Select(delegate(IItemDocumentoFiscal item)
						{
							ObjectId produtoServicoEmpresaReferencia = item.ProdutoServicoEmpresaReferencia;
							string codigoInterno = item.ProdutoServico.CodigoInterno;
							ProdutoHistorico produtoHistorico = item.ProdutoServico.To<ProdutoHistorico>();
							string text = ((produtoHistorico != null) ? produtoHistorico.CodigoBarras : null);
							DateTime date = item.DataHora.Date;
							string codigo = item.NCMNBS.Codigo;
							IItemDocumentoAuxiliarProduto itemDocumentoAuxiliarProduto = item.To<IItemDocumentoAuxiliarProduto>();
							string text2 = ((itemDocumentoAuxiliarProduto != null) ? itemDocumentoAuxiliarProduto.CodigoEspecificadorSubstituicaoTributaria : null);
							int codigo2 = item.OperacaoFiscal.Cfop.Codigo;
							string descricao = item.ProdutoServico.Descricao;
							decimal quantidade = item.Quantidade;
							decimal precoUnitario = item.PrecoUnitario;
							decimal totalDesconto = item.TotalDesconto;
							decimal total = item.Total;
							IItemDocumentoFiscalProduto itemDocumentoFiscalProduto = item.To<IItemDocumentoFiscalProduto>();
							return new
							{
								ProdutoServicoEmpresaReferencia = produtoServicoEmpresaReferencia,
								CodigoInterno = codigoInterno,
								CodigoBarras = text,
								DataHora = date,
								NcmnbsCodigo = codigo,
								CodigoEspecificadorSubstituicaoTributaria = text2,
								Cfop = codigo2,
								Descricao = descricao,
								Quantidade = quantidade,
								PrecoUnitario = precoUnitario,
								TotalDesconto = totalDesconto,
								Total = total,
								EstoqueMovimentacao = ((itemDocumentoFiscalProduto != null) ? itemDocumentoFiscalProduto.EstoqueMovimentacao : null),
								TotalOutrasDespesas = item.TotalOutrasDespesas
							};
						})
						group g by new { g.ProdutoServicoEmpresaReferencia, g.CodigoInterno, g.CodigoBarras, g.DataHora, g.NcmnbsCodigo, g.CodigoEspecificadorSubstituicaoTributaria, g.Cfop, g.Descricao, g.PrecoUnitario }).Select(delegate(arg)
					{
						ItemMovimentadoDto itemMovimentadoDto = new ItemMovimentadoDto();
						itemMovimentadoDto.CodigoInterno = arg.Key.CodigoInterno;
						itemMovimentadoDto.CodigoBarras = arg.Key.CodigoBarras ?? string.Empty;
						itemMovimentadoDto.DataHora = arg.Key.DataHora;
						itemMovimentadoDto.CodigoNcmNbs = arg.Key.NcmnbsCodigo;
						itemMovimentadoDto.CodigoEspecificadorSubstituicaoTributaria = arg.Key.CodigoEspecificadorSubstituicaoTributaria ?? string.Empty;
						itemMovimentadoDto.Cfop = arg.Key.Cfop;
						itemMovimentadoDto.Descricao = arg.Key.Descricao;
						itemMovimentadoDto.Preco = arg.Key.PrecoUnitario;
						itemMovimentadoDto.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
						itemMovimentadoDto.Quantidade = arg.Sum(x => x.Quantidade);
						itemMovimentadoDto.Desconto = arg.Sum(x => x.TotalDesconto);
						itemMovimentadoDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoDto.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
						itemMovimentadoDto.Empresa = empresa.Nome;
						itemMovimentadoDto.CnpjEmpresa = empresa.CpfCnpjFormatado;
						return itemMovimentadoDto;
					}));
				}
			}
			return list2;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0007EA34 File Offset: 0x0007CC34
		public IEnumerable<ItemMovimentadoTributacaoPisCofinsDto> ObterItensVendidosComTributacaoPisCofins(DateTime dataInicial, DateTime dataFinal, TipoProdutoServico tipo, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<ProdutoServico> produtosServicos = this._produtoServicoRepositoryRead.Find(ProdutoServicoSpecifications.PorTipo(tipo));
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal).PorProdutoServicoReferencia(produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>())
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoTributacaoPisCofinsDto> list2 = new List<ItemMovimentadoTributacaoPisCofinsDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoFiscal, bool> <>9__5;
				Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> <>9__4;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> func;
					if ((func = <>9__4) == null)
					{
						func = (<>9__4 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoFiscal> enumerable4 = m.Itens.OfType<IItemDocumentoFiscal>();
							Func<IItemDocumentoFiscal, bool> func2;
							if ((func2 = <>9__5) == null)
							{
								func2 = (<>9__5 = delegate(IItemDocumentoFiscal i)
								{
									if (!i.Cancelado)
									{
										return i.ProdutoServico.ProdutoServicoReferencia.In(produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable4.Where<IItemDocumentoFiscal>(func2);
						});
					}
					IEnumerable<IItemDocumentoFiscal> enumerable3 = enumerable2.SelectMany<Movimentacao, IItemDocumentoFiscal>(func);
					list2.AddRange((from g in enumerable3.Select(delegate(IItemDocumentoFiscal item)
						{
							string codigoInterno = item.ProdutoServico.CodigoInterno;
							ProdutoHistorico produtoHistorico = item.ProdutoServico.To<ProdutoHistorico>();
							string text = ((produtoHistorico != null) ? produtoHistorico.CodigoBarras : null);
							string descricao = item.ProdutoServico.Descricao;
							DateTime date = item.DataHora.Date;
							string codigo = item.NCMNBS.Codigo;
							IItemDocumentoAuxiliarProduto itemDocumentoAuxiliarProduto = item.To<IItemDocumentoAuxiliarProduto>();
							string text2 = ((itemDocumentoAuxiliarProduto != null) ? itemDocumentoAuxiliarProduto.CodigoEspecificadorSubstituicaoTributaria : null);
							int codigo2 = item.OperacaoFiscal.Cfop.Codigo;
							decimal quantidade = item.Quantidade;
							decimal total = item.Total;
							string text3 = string.Format("{0} - {1}", item.PIS.CstPISCOFINS.Codigo.ToStringLeftZero(2), item.PIS.CstPISCOFINS.Descricao);
							string text4 = string.Format("{0} - {1}", item.COFINS.CstPISCOFINS.Codigo.ToStringLeftZero(2), item.COFINS.CstPISCOFINS.Descricao);
							string text5 = "{0} - {1}";
							IItemDocumentoAuxiliarProduto itemDocumentoAuxiliarProduto2 = item.To<IItemDocumentoAuxiliarProduto>();
							object obj = ((itemDocumentoAuxiliarProduto2 != null) ? itemDocumentoAuxiliarProduto2.Percentual.SituacaoTributaria.Codigo.ToStringOrEmpty() : null) ?? string.Empty;
							IItemDocumentoAuxiliarProduto itemDocumentoAuxiliarProduto3 = item.To<IItemDocumentoAuxiliarProduto>();
							string text6 = string.Format(text5, obj, (itemDocumentoAuxiliarProduto3 != null) ? itemDocumentoAuxiliarProduto3.Percentual.SituacaoTributaria.Descricao : null);
							string text7 = item.PIS.CstPISCOFINS.Codigo.ToStringLeftZero(2);
							string text8 = item.COFINS.CstPISCOFINS.Codigo.ToStringLeftZero(2);
							IItemDocumentoAuxiliarProduto itemDocumentoAuxiliarProduto4 = item.To<IItemDocumentoAuxiliarProduto>();
							return new
							{
								CodigoInterno = codigoInterno,
								CodigoBarras = text,
								Descricao = descricao,
								DataHora = date,
								NcmnbsCodigo = codigo,
								CodigoEspecificadorSubstituicaoTributaria = text2,
								Cfop = codigo2,
								Quantidade = quantidade,
								Total = total,
								CstPisToText = text3,
								CstCofinsToText = text4,
								CstCsosnToText = text6,
								CstPis = text7,
								CstCofins = text8,
								CstCsosn = (((itemDocumentoAuxiliarProduto4 != null) ? itemDocumentoAuxiliarProduto4.Percentual.SituacaoTributaria.Codigo.ToStringOrEmpty() : null) ?? string.Empty)
							};
						})
						group g by new
						{
							g.CodigoInterno, g.CodigoBarras, g.Descricao, g.DataHora, g.NcmnbsCodigo, g.CodigoEspecificadorSubstituicaoTributaria, g.Cfop, g.CstPisToText, g.CstCofinsToText, g.CstCsosnToText,
							g.CstPis, g.CstCofins, g.CstCsosn
						}).Select(delegate(arg)
					{
						ItemMovimentadoTributacaoPisCofinsDto itemMovimentadoTributacaoPisCofinsDto = new ItemMovimentadoTributacaoPisCofinsDto();
						itemMovimentadoTributacaoPisCofinsDto.Empresa = empresa.Nome;
						itemMovimentadoTributacaoPisCofinsDto.CnpjEmpresa = empresa.CpfCnpjFormatado;
						itemMovimentadoTributacaoPisCofinsDto.CodigoInterno = arg.Key.CodigoInterno;
						itemMovimentadoTributacaoPisCofinsDto.CodigoBarras = arg.Key.CodigoBarras ?? string.Empty;
						itemMovimentadoTributacaoPisCofinsDto.Descricao = arg.Key.Descricao;
						itemMovimentadoTributacaoPisCofinsDto.DataHora = arg.Key.DataHora;
						itemMovimentadoTributacaoPisCofinsDto.CodigoNcmNbs = arg.Key.NcmnbsCodigo;
						itemMovimentadoTributacaoPisCofinsDto.CodigoEspecificadorSubstituicaoTributaria = arg.Key.CodigoEspecificadorSubstituicaoTributaria ?? string.Empty;
						itemMovimentadoTributacaoPisCofinsDto.Cfop = arg.Key.Cfop;
						itemMovimentadoTributacaoPisCofinsDto.Quantidade = arg.Sum(x => x.Quantidade);
						itemMovimentadoTributacaoPisCofinsDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoTributacaoPisCofinsDto.CstPisToText = arg.Key.CstPisToText;
						itemMovimentadoTributacaoPisCofinsDto.CstCofinsToText = arg.Key.CstCofinsToText;
						itemMovimentadoTributacaoPisCofinsDto.CstCsosnToText = arg.Key.CstCsosnToText;
						itemMovimentadoTributacaoPisCofinsDto.CstPis = arg.Key.CstPis;
						itemMovimentadoTributacaoPisCofinsDto.CstCofins = arg.Key.CstCofins;
						itemMovimentadoTributacaoPisCofinsDto.CstCsosn = arg.Key.CstCsosn;
						return itemMovimentadoTributacaoPisCofinsDto;
					}));
				}
			}
			return list2;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0007EC34 File Offset: 0x0007CE34
		public IEnumerable<ItemMovimentadoTributacaoFcpDto> ObterItensVendidosComFcp(DateTime dataInicial, DateTime dataFinal, bool consolidado, CodigoDescricaoDto origemDto, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<ProdutoServico> produtosServicos = this._produtoServicoRepositoryRead.Find(EntitySpecification<ProdutoServico>.All());
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo());
			OrigemPesquisaMovimentacao origemPesquisaMovimentacao = OrigensPesquisaMovimentacao.Dados.FirstOrDefault<OrigemPesquisaMovimentacao>((OrigemPesquisaMovimentacao x) => x.Codigo == origemDto.Codigo);
			specification = ((origemPesquisaMovimentacao is Compra) ? specification.PorDataHoraEntradaSaida(dataInicial, dataFinal) : specification.PorDataEmissao(dataInicial, dataFinal));
			ISpecification<Movimentacao> specification2 = specification.PorSituacaoConcluido().PorOrigem(origemPesquisaMovimentacao.Codigo).PorDataEmissao(dataInicial, dataFinal)
				.PorProdutoServicoReferencia(produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>())
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification2);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoTributacaoFcpDto> list2 = new List<ItemMovimentadoTributacaoFcpDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoAuxiliarProduto, bool> <>9__6;
				Func<Movimentacao, IEnumerable<IItemDocumentoAuxiliarProduto>> <>9__5;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoAuxiliarProduto>> func;
					if ((func = <>9__5) == null)
					{
						func = (<>9__5 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoAuxiliarProduto> enumerable4 = m.Itens.OfType<IItemDocumentoAuxiliarProduto>();
							Func<IItemDocumentoAuxiliarProduto, bool> func2;
							if ((func2 = <>9__6) == null)
							{
								func2 = (<>9__6 = delegate(IItemDocumentoAuxiliarProduto i)
								{
									if (!i.Cancelado)
									{
										return i.ProdutoServico.ProdutoServicoReferencia.In(produtosServicos.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable4.Where<IItemDocumentoAuxiliarProduto>(func2);
						});
					}
					IEnumerable<IItemDocumentoAuxiliarProduto> enumerable3 = enumerable2.SelectMany<Movimentacao, IItemDocumentoAuxiliarProduto>(func);
					list2.AddRange((from g in enumerable3.Select(delegate(IItemDocumentoAuxiliarProduto item)
						{
							string codigoInterno = item.ProdutoServico.CodigoInterno;
							string descricao = item.ProdutoServico.Descricao;
							long numeroMovimentacao = item.NumeroMovimentacao;
							int codigo = item.OperacaoFiscal.Cfop.Codigo;
							decimal total = item.Total;
							decimal num = this.ObterPercentualFcp(item);
							decimal num2 = this.ObterValorFcp(item);
							decimal num3 = this.ObterBaseCalculoFcp(item);
							IItemDocumentoAuxiliarProduto itemDocumentoAuxiliarProduto = item.To<IItemDocumentoAuxiliarProduto>();
							decimal? num4;
							if (itemDocumentoAuxiliarProduto == null)
							{
								num4 = null;
							}
							else
							{
								IPercentual percentual = itemDocumentoAuxiliarProduto.Percentual;
								if (percentual == null)
								{
									num4 = null;
								}
								else
								{
									IPercentualTributado percentualTributado = percentual.To<IPercentualTributado>();
									num4 = ((percentualTributado != null) ? new decimal?(percentualTributado.Valor) : null);
								}
							}
							return new
							{
								CodigoInterno = codigoInterno,
								Descricao = descricao,
								NumeroMovimentacao = numeroMovimentacao,
								Cfop = codigo,
								Total = total,
								PercentualFcp = num,
								ValorFcp = num2,
								BaseCalculoFcp = num3,
								ValorIcms = (num4 ?? 0m)
							};
						})
						group g by new { g.CodigoInterno, g.Descricao, g.NumeroMovimentacao, g.Cfop, g.PercentualFcp }).Select(delegate(arg)
					{
						ItemMovimentadoTributacaoFcpDto itemMovimentadoTributacaoFcpDto = new ItemMovimentadoTributacaoFcpDto();
						itemMovimentadoTributacaoFcpDto.Empresa = empresa.Nome;
						itemMovimentadoTributacaoFcpDto.CnpjEmpresa = empresa.CpfCnpjFormatado;
						itemMovimentadoTributacaoFcpDto.CodigoInterno = arg.Key.CodigoInterno;
						itemMovimentadoTributacaoFcpDto.Descricao = arg.Key.Descricao;
						itemMovimentadoTributacaoFcpDto.NumeroMovimentacao = arg.Key.NumeroMovimentacao;
						itemMovimentadoTributacaoFcpDto.Cfop = arg.Key.Cfop;
						itemMovimentadoTributacaoFcpDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoTributacaoFcpDto.PercentualFcp = arg.Key.PercentualFcp;
						itemMovimentadoTributacaoFcpDto.ValorFcp = arg.Sum(x => x.ValorFcp);
						itemMovimentadoTributacaoFcpDto.BaseCalculoFcp = arg.Sum(x => x.BaseCalculoFcp);
						itemMovimentadoTributacaoFcpDto.ValorIcms = arg.Sum(x => x.ValorIcms);
						return itemMovimentadoTributacaoFcpDto;
					}));
				}
			}
			return list2;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0007EE44 File Offset: 0x0007D044
		private decimal ObterPercentualFcp(IItemDocumentoAuxiliarProduto item)
		{
			IPercentual percentual = item.Percentual;
			if (percentual.IsNotNull())
			{
				if (percentual.Is<IPercentualTributado>())
				{
					return percentual.To<IPercentualTributado>().PercentualFundoCombatePobrezaInterno;
				}
				if (percentual.Is<IPercentualSubstituicaoTributaria>())
				{
					return percentual.To<IPercentualSubstituicaoTributaria>().PercentualFundoCombatePobrezaStInterno;
				}
				if (percentual.Is<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>())
				{
					IPercentualCobradoAnteriormentePorSubstituicaoTributaria percentualCobradoAnteriormentePorSubstituicaoTributaria = percentual.To<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>();
					if (percentualCobradoAnteriormentePorSubstituicaoTributaria == null)
					{
						return 0m;
					}
					return percentualCobradoAnteriormentePorSubstituicaoTributaria.PercentualFundoCombatePobrezaStInterno;
				}
			}
			return 0m;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0007EEAC File Offset: 0x0007D0AC
		private decimal ObterValorFcp(IItemDocumentoAuxiliarProduto item)
		{
			IPercentual percentual = item.Percentual;
			if (percentual.IsNotNull())
			{
				if (percentual.Is<IPercentualTributado>())
				{
					return percentual.To<IPercentualTributado>().ValorFundoCombatePobrezaInterno;
				}
				if (percentual.Is<IPercentualSubstituicaoTributaria>())
				{
					return percentual.To<IPercentualSubstituicaoTributaria>().ValorFundoCombatePobrezaStInterno;
				}
				if (percentual.Is<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>())
				{
					IPercentualCobradoAnteriormentePorSubstituicaoTributaria percentualCobradoAnteriormentePorSubstituicaoTributaria = percentual.To<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>();
					if (percentualCobradoAnteriormentePorSubstituicaoTributaria == null)
					{
						return 0m;
					}
					return percentualCobradoAnteriormentePorSubstituicaoTributaria.ValorFundoCombatePobrezaStInterno;
				}
			}
			return 0m;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0007EF14 File Offset: 0x0007D114
		private decimal ObterBaseCalculoFcp(IItemDocumentoAuxiliarProduto item)
		{
			IPercentual percentual = item.Percentual;
			if (!percentual.IsNotNull())
			{
				return 0m;
			}
			if (percentual.Is<IPercentualFundoCombatePobreza>())
			{
				return percentual.To<IPercentualFundoCombatePobreza>().BaseCalculoFundoCombatePobreza;
			}
			if (percentual.Is<IPercentualSubstituicaoTributaria>())
			{
				return percentual.To<IPercentualSubstituicaoTributaria>().ValorBaseCalculoFundoCombatePobrezaStInterno;
			}
			IPercentualTributado percentualTributado = percentual.To<IPercentualTributado>();
			if (percentualTributado == null)
			{
				return 0m;
			}
			return percentualTributado.ValorBaseCalculo;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0007EF74 File Offset: 0x0007D174
		private IEnumerable<DigisatServer.Application.Main.Dto.Movimentacoes.Relatorios.ComponenteVendidoDto> ObterComponentesItensVendidos(ProdutoServico produtoServico, decimal quantidade)
		{
			if (!(produtoServico is Composicao))
			{
				return Enumerable.Empty<DigisatServer.Application.Main.Dto.Movimentacoes.Relatorios.ComponenteVendidoDto>();
			}
			return produtoServico.To<Composicao>().Componentes.Select<Componente, DigisatServer.Application.Main.Dto.Movimentacoes.Relatorios.ComponenteVendidoDto>((Componente c) => new DigisatServer.Application.Main.Dto.Movimentacoes.Relatorios.ComponenteVendidoDto
			{
				Descricao = c.ProdutoDescricao.ToStringEmptyWhenNull(),
				QuantidadeComposicao = c.Quantidade,
				QuantidadeVendida = quantidade * c.Quantidade
			}).ToListEmptyWhenNull<DigisatServer.Application.Main.Dto.Movimentacoes.Relatorios.ComponenteVendidoDto>();
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0007EFC0 File Offset: 0x0007D1C0
		private IEnumerable<AbastecimentoVendidoDto> ObterAbastecimentosItensVendidos(IEnumerable<RegistroAbastecimento> abastecimentos, decimal precoUnitario)
		{
			List<RegistroAbastecimento> list = abastecimentos.ToListEmptyWhenNull<RegistroAbastecimento>();
			if (!list.Any<RegistroAbastecimento>())
			{
				return Enumerable.Empty<AbastecimentoVendidoDto>();
			}
			return (from x in list
				where x.Abastecimento.PrecoUnitario == precoUnitario
				select x into a
				select new AbastecimentoVendidoDto
				{
					Bico = a.Abastecimento.NumeroDoBicoNoSoftware,
					Bomba = a.Abastecimento.NumeroDaBomba,
					Tanque = a.Abastecimento.NumeroDoTanque.ToStringEmptyWhenNull(),
					Volume = a.Abastecimento.Litros
				} into g
				group g by new { g.Bico, g.Bomba, g.Tanque }).Select(delegate(s)
			{
				AbastecimentoVendidoDto abastecimentoVendidoDto = new AbastecimentoVendidoDto();
				abastecimentoVendidoDto.Bico = s.Key.Bico;
				abastecimentoVendidoDto.Tanque = s.Key.Tanque.ToStringEmptyWhenNull();
				abastecimentoVendidoDto.Bomba = s.Key.Bomba;
				abastecimentoVendidoDto.Volume = s.Sum<AbastecimentoVendidoDto>((AbastecimentoVendidoDto x) => x.Volume);
				return abastecimentoVendidoDto;
			}).ToListEmptyWhenNull<AbastecimentoVendidoDto>();
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0007F074 File Offset: 0x0007D274
		public IEnumerable<ItemMovimentadoDto> ObterItensVendidosPorLocalizacao(DateTime dataInicial, DateTime dataFinal, string localizacaoReferencia, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<ProdutoServicoEmpresa> resultado = this._produtoServicoEmpresaRepositoryRead.Find(ProdutoServicoEmpresaSpecifications.PorAtivo().PorLocalizacao(localizacaoReferencia));
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal).PorProdutoServicoEmpresaReferencia(resultado.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa x) => x.Id).ToArray<ObjectId>())
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoFiscal, bool> <>9__5;
				Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> <>9__4;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					List<ItemMovimentadoDto> list3 = list2;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> func;
					if ((func = <>9__4) == null)
					{
						func = (<>9__4 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoFiscal> enumerable3 = m.Itens.OfType<IItemDocumentoFiscal>();
							Func<IItemDocumentoFiscal, bool> func2;
							if ((func2 = <>9__5) == null)
							{
								func2 = (<>9__5 = delegate(IItemDocumentoFiscal i)
								{
									if (!i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro)
									{
										return i.ProdutoServicoEmpresaReferencia.In(resultado.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable3.Where<IItemDocumentoFiscal>(func2);
						});
					}
					list3.AddRange((from g in enumerable2.SelectMany<Movimentacao, IItemDocumentoFiscal>(func).Select(delegate(IItemDocumentoFiscal item)
						{
							string codigoInterno = item.ProdutoServico.CodigoInterno;
							string descricao = item.ProdutoServico.Descricao;
							decimal quantidade = item.Quantidade;
							decimal precoUnitario = item.PrecoUnitario;
							decimal totalDesconto = item.TotalDesconto;
							decimal total = item.Total;
							ItemDocumentoFiscalSaidaProduto itemDocumentoFiscalSaidaProduto = item.To<ItemDocumentoFiscalSaidaProduto>();
							return new
							{
								CodigoInterno = codigoInterno,
								Descricao = descricao,
								Quantidade = quantidade,
								PrecoUnitario = precoUnitario,
								TotalDesconto = totalDesconto,
								Total = total,
								EstoqueMovimentacao = ((itemDocumentoFiscalSaidaProduto != null) ? itemDocumentoFiscalSaidaProduto.EstoqueMovimentacao : null),
								TotalOutrasDespesas = item.TotalOutrasDespesas
							};
						})
						group g by new { g.CodigoInterno, g.Descricao, g.PrecoUnitario }).Select(delegate(arg)
					{
						ItemMovimentadoDto itemMovimentadoDto = new ItemMovimentadoDto();
						itemMovimentadoDto.CodigoInterno = arg.Key.CodigoInterno;
						itemMovimentadoDto.Descricao = arg.Key.Descricao;
						itemMovimentadoDto.Preco = arg.Key.PrecoUnitario;
						itemMovimentadoDto.Quantidade = arg.Sum(x => x.Quantidade);
						itemMovimentadoDto.Desconto = arg.Sum(x => x.TotalDesconto);
						itemMovimentadoDto.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
						itemMovimentadoDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoDto.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
						itemMovimentadoDto.Empresa = empresa.Nome;
						itemMovimentadoDto.CnpjEmpresa = empresa.CpfCnpjFormatado;
						return itemMovimentadoDto.ProcessarEstoque();
					}));
				}
			}
			return list2;
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0007F278 File Offset: 0x0007D478
		public IEnumerable<MovimentacaoPorConvenioDto> ObterMovimentacoesPorConvenio(DateTime dataInicial, DateTime dataFinal, string convenioReferencia, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo());
			ISpecification<Movimentacao> specification2 = (string.IsNullOrEmpty(convenioReferencia) ? specification.ComConvenio() : specification.PorConvenio(convenioReferencia)).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal);
			return (from g in this._movimentacaoRepositoryRead.Find(specification2)
				group g by new
				{
					Nome = g.Pessoa.Nome,
					ConvenioReferencia = g.To<IMovimentacaoConvenio>().Convenio.PessoaReferencia
				}).Select(delegate(arg)
			{
				Movimentacao movimentacao = arg.FirstOrDefault<Movimentacao>();
				return new MovimentacaoPorConvenioDto
				{
					Cliente = arg.Key.Nome.ToStringEmptyWhenNull(),
					Telefone = movimentacao.Pessoa.TelefonePrincipal.ToStringEmptyWhenNull(),
					ClienteReferencia = movimentacao.Pessoa.PessoaReferencia.ToString(),
					Convenio = movimentacao.To<IMovimentacaoConvenio>().Convenio.Nome.RemoverCaracteresNaoImprimiveis(),
					ConvenioDocumento = movimentacao.To<IMovimentacaoConvenio>().Convenio.CpfCnpjFormatado,
					Movimentacoes = arg.ProjectedAsCollectionMovimentacaoRelatorioBaseDto(1, null)
				};
			}).ToListEmptyWhenNull<MovimentacaoPorConvenioDto>();
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0007F31C File Offset: 0x0007D51C
		public IEnumerable<MovimentacaoRelatorioBaseDto> ObterMovimentacoesPorVendedor(DateTime dataInicial, DateTime dataFinal, string vendedorReferencia, CodigoDescricaoDto origemDto, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			OrigemPesquisaMovimentacao origemPesquisaMovimentacao = OrigensPesquisaMovimentacao.Dados.FirstOrDefault<OrigemPesquisaMovimentacao>((OrigemPesquisaMovimentacao x) => x.Codigo == origemDto.Codigo);
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorDataHoraEmissao(dataInicial, dataFinal).PorSituacaoConcluido().PorOrigem(origemPesquisaMovimentacao.Codigo)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			specification = specification.PorVendedor(vendedorReferencia);
			return this._movimentacaoRepositoryRead.Find(specification).ProjectedAsCollectionMovimentacaoRelatorioBaseDto(1, vendedorReferencia);
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0007F3A4 File Offset: 0x0007D5A4
		public IEnumerable<MovimentacaoPorPessoaDto> VendasPorCliente(DateTime dataInicial, DateTime dataFinal, int origem, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specification;
			if (consolidado)
			{
				specification = MovimentacaoSpecification.PorOrigem(origem).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal)
					.PorFinalidadeEmissaoNormal()
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			}
			else
			{
				specification = MovimentacaoSpecification.PorOrigem(origem).PorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido()
					.PorDataEmissao(dataInicial, dataFinal)
					.PorFinalidadeEmissaoNormal()
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			}
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			List<<>f__AnonymousType116<ObjectId, string, DateTime, decimal>> group = (from x in enumerable
				where x.Itens.OfType<IItemDocumentoAuxiliar>().All<IItemDocumentoAuxiliar>(delegate(IItemDocumentoAuxiliar y)
				{
					OperacaoFiscal operacaoFiscal = y.OperacaoFiscal;
					if (operacaoFiscal != null && operacaoFiscal.MovimentaEstoque)
					{
						OperacaoFiscal operacaoFiscal2 = y.OperacaoFiscal;
						return operacaoFiscal2 != null && operacaoFiscal2.MovimentaFinanceiro;
					}
					return false;
				})
				select x into m
				group m by new
				{
					m.Pessoa.PessoaReferencia,
					m.Pessoa.Nome
				} into arg
				select new
				{
					PessoaReferencia = arg.Key.PessoaReferencia,
					Nome = arg.Key.Nome,
					UltimaVenda = arg.Last<Movimentacao>().DataHoraEmissao,
					Total = arg.Sum<Movimentacao>((Movimentacao x) => x.Total)
				}).ToListEmptyWhenNull();
			return group.Select(delegate(g)
			{
				MovimentacaoPorPessoaDto movimentacaoPorPessoaDto = new MovimentacaoPorPessoaDto();
				movimentacaoPorPessoaDto.Total = g.Total;
				movimentacaoPorPessoaDto.Nome = g.Nome;
				movimentacaoPorPessoaDto.Participacao = g.Total * 100m / group.Sum(x => x.Total);
				movimentacaoPorPessoaDto.DataUltimaMovimentacao = g.UltimaVenda;
				return movimentacaoPorPessoaDto;
			});
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0007F4A0 File Offset: 0x0007D6A0
		public IEnumerable<VendasPorProdutoDto> VendasPorProduto(DateTime dataInicial, DateTime dataFinal, CodigoDescricaoDto preco, bool consolidado, string usuarioReferencia, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo());
			specification = specification.PorOrigem(101).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			if (!usuarioReferencia.IsNullOrEmpty())
			{
				UsuarioBase usuario = this._usuarioRepositoryRead.GetById(ObjectId.Parse(usuarioReferencia));
				Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> <>9__1;
				enumerable = enumerable.Where<Movimentacao>(delegate(Movimentacao x)
				{
					IEnumerable<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico> historicos = x.Historicos;
					Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> func2;
					if ((func2 = <>9__1) == null)
					{
						func2 = (<>9__1 = (DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico y) => y.NomeUsuario.ToLower().Equals(usuario.Nome.ToLower()) || y.NomeUsuario.ToLower().Equals(usuario.AcessoAlternativo.ToLower()));
					}
					return historicos.Any<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>(func2);
				}).ToListEmptyWhenNull<Movimentacao>();
			}
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<VendasPorProdutoDto> list2 = new List<VendasPorProdutoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<<>f__AnonymousType118<<>f__AnonymousType117<string, string, string, decimal, decimal, decimal>, ProdutoEmpresa>, <>f__AnonymousType119<<>f__AnonymousType118<<>f__AnonymousType117<string, string, string, decimal, decimal, decimal>, ProdutoEmpresa>, decimal>> <>9__16;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					IEnumerable<IItemDocumentoFiscalProduto> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao movimentacao) => movimentacao.EmpresaReferencia.Equals(empresa.PessoaReferencia)).SelectMany<Movimentacao, IItemDocumentoFiscalProduto>((Movimentacao m) => from x in m.Itens.OfType<IItemDocumentoFiscalProduto>()
						where !x.Cancelado && x.OperacaoFiscal.MovimentaEstoque && x.OperacaoFiscal.MovimentaFinanceiro
						select x);
					IEnumerable<ProdutoServicoEmpresa> produtosEmpresa = this._produtoServicoEmpresaRepositoryRead.GetIn<ObjectId>((ProdutoServicoEmpresa x) => x.Id, enumerable2.Select<IItemDocumentoFiscalProduto, ObjectId>((IItemDocumentoFiscalProduto i) => i.ProdutoServicoEmpresaReferencia), usuarioIdentity);
					var enumerable3 = from g in (from g in enumerable2
							group g by new
							{
								g.ProdutoServico.CodigoInterno,
								g.ProdutoServico.Descricao
							}).Select(delegate(s)
						{
							string codigoInterno = s.Key.CodigoInterno;
							string descricao = s.Key.Descricao;
							IItemDocumentoFiscalProduto itemDocumentoFiscalProduto = s.FirstOrDefault<IItemDocumentoFiscalProduto>((IItemDocumentoFiscalProduto x) => !string.IsNullOrEmpty(x.Produto.CodigoBarras));
							return new
							{
								CodigoInterno = codigoInterno,
								Descricao = descricao,
								Barras = ((itemDocumentoFiscalProduto != null) ? itemDocumentoFiscalProduto.Produto.CodigoBarras : null),
								PrecoUnitario = s.Average<IItemDocumentoFiscalProduto>((IItemDocumentoFiscalProduto x) => x.PrecoUnitario).ArredondarParaDuasCasasDecimais(),
								Quantidade = s.Sum<IItemDocumentoFiscalProduto>((IItemDocumentoFiscalProduto x) => x.Quantidade),
								TotalVenda = s.Sum<IItemDocumentoFiscalProduto>((IItemDocumentoFiscalProduto x) => x.Total)
							};
						}).ToListEmptyWhenNull()
						select new
						{
							g = g,
							produtoEmpresa = produtosEmpresa.OfType<ProdutoEmpresa>().FirstOrDefault<ProdutoEmpresa>((ProdutoEmpresa x) => x.ProdutoServico.CodigoInterno.Equals(g.CodigoInterno))
						};
					var func;
					if ((func = <>9__16) == null)
					{
						func = (<>9__16 = delegate(<>h__TransparentIdentifier0)
						{
							decimal num;
							if (preco.Codigo != 1)
							{
								if (preco.Codigo != 2)
								{
									ProdutoEmpresa produtoEmpresa = <>h__TransparentIdentifier0.produtoEmpresa;
									num = ((produtoEmpresa != null) ? produtoEmpresa.PrecoUltimaCompra : 0m);
								}
								else
								{
									ProdutoEmpresa produtoEmpresa2 = <>h__TransparentIdentifier0.produtoEmpresa;
									num = ((produtoEmpresa2 != null) ? produtoEmpresa2.ValorCustoMedio : 0m);
								}
							}
							else
							{
								ProdutoEmpresa produtoEmpresa3 = <>h__TransparentIdentifier0.produtoEmpresa;
								num = ((produtoEmpresa3 != null) ? produtoEmpresa3.ValorCusto : 0m);
							}
							return new
							{
								<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0,
								precoEntrada = num
							};
						});
					}
					List<VendasPorProdutoDto> list3 = (from <>h__TransparentIdentifier1 in enumerable3.Select(func)
						select new VendasPorProdutoDto
						{
							Descricao = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.Descricao,
							Quantidade = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.Quantidade,
							CodigoInterno = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.CodigoInterno,
							CodigoBarras = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.Barras,
							Preco = <>h__TransparentIdentifier1.precoEntrada,
							Venda = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.PrecoUnitario,
							Lucro = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.TotalVenda - <>h__TransparentIdentifier1.precoEntrada * <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.Quantidade,
							PercentualLucro = ((<>h__TransparentIdentifier1.precoEntrada > 0m) ? (<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.TotalVenda / (<>h__TransparentIdentifier1.precoEntrada * <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.Quantidade) * 100m - 100m) : 0m),
							TotalVenda = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.g.TotalVenda,
							CnpjEmpresa = empresa.CpfCnpjFormatado.ToStringEmptyWhenNull(),
							NomeEmpresa = empresa.Nome.ToStringEmptyWhenNull()
						}).ToList<VendasPorProdutoDto>();
					list2.AddRange(list3);
				}
			}
			return list2;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0007F74C File Offset: 0x0007D94C
		public VendasPorOperadorDto VendasPorOperador(DateTime dataInicial, DateTime dataFinal, string usuarioReferencia, int origemMovimentacao, int situacaoMovimentacao, IUsuarioIdentity usuarioIdentity)
		{
			UsuarioBase usuario = this._usuarioRepositoryRead.GetById(ObjectId.Parse(usuarioReferencia));
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataEmissao(dataInicial, dataFinal).PorOrigem(origemMovimentacao)
				.PorSituacao(situacaoMovimentacao)
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			List<VendasPorOperadorMovimentacaoDto> list = (from movimentacao in this._movimentacaoRepositoryRead.Find(specification)
				where (movimentacao.Historicos.FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>().NomeUsuario.ToLower().Equals(usuario.Nome.ToLower()) || movimentacao.Historicos.FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>().NomeUsuario.ToLower().Equals(usuario.AcessoAlternativo.ToLower())) && movimentacao.Historicos.FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>().SituacaoMovimentacao.Codigo == 1
				select movimentacao).Select<Movimentacao, VendasPorOperadorMovimentacaoDto>(delegate(Movimentacao movimentacao)
			{
				VendasPorOperadorMovimentacaoDto vendasPorOperadorMovimentacaoDto = new VendasPorOperadorMovimentacaoDto();
				vendasPorOperadorMovimentacaoDto.Origem = movimentacao.Tipo.RetornarDescricao();
				vendasPorOperadorMovimentacaoDto.Numero = movimentacao.Numero;
				vendasPorOperadorMovimentacaoDto.Cliente = movimentacao.Pessoa.Nome.RemoverCaracteresNaoImprimiveis();
				vendasPorOperadorMovimentacaoDto.Data = movimentacao.DataHoraEmissao;
				vendasPorOperadorMovimentacaoDto.TotalProdutos = movimentacao.TotalProdutos;
				vendasPorOperadorMovimentacaoDto.TotalDesconto = movimentacao.TotalDescontos;
				vendasPorOperadorMovimentacaoDto.Total = movimentacao.Total;
				vendasPorOperadorMovimentacaoDto.Itens = movimentacao.Itens.Select<IItem, VendasPorOperadorItemDto>((IItem item) => new VendasPorOperadorItemDto
				{
					CodigoInterno = item.ProdutoServico.CodigoInterno,
					Descricao = item.ProdutoServico.Descricao,
					Quantidade = item.Quantidade,
					Preco = item.PrecoUnitario,
					Desconto = item.TotalDesconto,
					Total = item.Total
				});
				vendasPorOperadorMovimentacaoDto.EspeciesRecebimento = movimentacao.To<IMovimentacaoEfetivacao>().PagamentoRecebimento.Parcelas.Select<Parcela, VendasPorOperadorEspecieRecebimentoDto>((Parcela parcela) => new VendasPorOperadorEspecieRecebimentoDto
				{
					Especie = parcela.EspeciePagamento.Descricao,
					Valor = parcela.Valor
				});
				vendasPorOperadorMovimentacaoDto.Situacao = movimentacao.Situacao.Descricao.ToStringEmptyWhenNull();
				return vendasPorOperadorMovimentacaoDto;
			}).ToListEmptyWhenNull<VendasPorOperadorMovimentacaoDto>();
			List<VendasPorOperadorEspecieRecebimentoDto> list2 = (from especie in list.SelectMany<VendasPorOperadorMovimentacaoDto, VendasPorOperadorEspecieRecebimentoDto>((VendasPorOperadorMovimentacaoDto x) => x.EspeciesRecebimento)
				group especie by especie.Especie).Select<IGrouping<string, VendasPorOperadorEspecieRecebimentoDto>, VendasPorOperadorEspecieRecebimentoDto>(delegate(IGrouping<string, VendasPorOperadorEspecieRecebimentoDto> especieGroup)
			{
				VendasPorOperadorEspecieRecebimentoDto vendasPorOperadorEspecieRecebimentoDto = new VendasPorOperadorEspecieRecebimentoDto();
				vendasPorOperadorEspecieRecebimentoDto.Especie = especieGroup.Key;
				vendasPorOperadorEspecieRecebimentoDto.Valor = especieGroup.Sum<VendasPorOperadorEspecieRecebimentoDto>((VendasPorOperadorEspecieRecebimentoDto x) => x.Valor);
				return vendasPorOperadorEspecieRecebimentoDto;
			}).ToListEmptyWhenNull<VendasPorOperadorEspecieRecebimentoDto>();
			return new VendasPorOperadorDto
			{
				Movimentacoes = list,
				ResumoEspecies = list2
			};
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0007F86C File Offset: 0x0007DA6C
		public DadosRelatorioResumoVendasSinteticoDto ResumoVendasSintetico(DateTime dataInicial, DateTime dataFinal, string usuarioReferencia, string pessoaReferencia, string vendedorReferencia, int origemMovimentacao, int situacaoMovimentacao, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorVendedor(vendedorReferencia)
				.PorPessoa(pessoaReferencia)
				.PorOrigem(origemMovimentacao)
				.PorSituacao(situacaoMovimentacao)
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			List<VendasPorOperadorMovimentacaoDto> list;
			if (string.IsNullOrEmpty(usuarioReferencia))
			{
				Func<IItem, bool> <>9__4;
				list = enumerable.Where<Movimentacao>((Movimentacao a) => a.Itens.OfType<IItemDocumentoAuxiliar>().All<IItemDocumentoAuxiliar>(delegate(IItemDocumentoAuxiliar x)
				{
					OperacaoFiscal operacaoFiscal = x.OperacaoFiscal;
					if (operacaoFiscal != null && operacaoFiscal.MovimentaEstoque)
					{
						OperacaoFiscal operacaoFiscal2 = x.OperacaoFiscal;
						return operacaoFiscal2 != null && operacaoFiscal2.MovimentaFinanceiro;
					}
					return false;
				})).Select<Movimentacao, VendasPorOperadorMovimentacaoDto>(delegate(Movimentacao movimentacao)
				{
					VendasPorOperadorMovimentacaoDto vendasPorOperadorMovimentacaoDto = new VendasPorOperadorMovimentacaoDto();
					vendasPorOperadorMovimentacaoDto.TotalProdutos = movimentacao.TotalProdutos;
					vendasPorOperadorMovimentacaoDto.TotalServicos = movimentacao.TotalServicos;
					vendasPorOperadorMovimentacaoDto.TotalDesconto = movimentacao.TotalDescontos;
					decimal num;
					if (!string.IsNullOrEmpty(vendedorReferencia))
					{
						IEnumerable<IItem> itens = movimentacao.Itens;
						Func<IItem, bool> func;
						if ((func = <>9__4) == null)
						{
							func = (<>9__4 = (IItem a) => a.VendedorReferencia.Equals(ObjectId.Parse(vendedorReferencia)));
						}
						num = itens.Where<IItem>(func).Sum<IItem>((IItem y) => y.TotalLiquido);
					}
					else
					{
						num = movimentacao.Itens.Sum<IItem>((IItem y) => y.TotalLiquido);
					}
					vendasPorOperadorMovimentacaoDto.TotalLiquido = num;
					vendasPorOperadorMovimentacaoDto.Total = movimentacao.Total;
					vendasPorOperadorMovimentacaoDto.EspeciesRecebimento = movimentacao.To<IMovimentacaoEfetivacao>().PagamentoRecebimento.Parcelas.Select<Parcela, VendasPorOperadorEspecieRecebimentoDto>((Parcela parcela) => new VendasPorOperadorEspecieRecebimentoDto
					{
						Especie = parcela.EspeciePagamento.Descricao,
						Valor = parcela.Valor
					});
					return vendasPorOperadorMovimentacaoDto;
				}).ToListEmptyWhenNull<VendasPorOperadorMovimentacaoDto>();
			}
			else
			{
				UsuarioBase usuario = this._usuarioRepositoryRead.GetById(ObjectId.Parse(usuarioReferencia));
				Func<IItem, bool> <>9__13;
				Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> <>9__10;
				list = enumerable.Where<Movimentacao>((Movimentacao a) => a.Itens.OfType<IItemDocumentoAuxiliar>().All<IItemDocumentoAuxiliar>(delegate(IItemDocumentoAuxiliar x)
				{
					OperacaoFiscal operacaoFiscal3 = x.OperacaoFiscal;
					if (operacaoFiscal3 != null && operacaoFiscal3.MovimentaEstoque)
					{
						OperacaoFiscal operacaoFiscal4 = x.OperacaoFiscal;
						return operacaoFiscal4 != null && operacaoFiscal4.MovimentaFinanceiro;
					}
					return false;
				})).Where<Movimentacao>(delegate(Movimentacao movimentacao)
				{
					IEnumerable<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico> historicos = movimentacao.Historicos;
					Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> func2;
					if ((func2 = <>9__10) == null)
					{
						func2 = (<>9__10 = (DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico x) => (x.NomeUsuario.ToLower().Equals(usuario.Nome.ToLower()) || x.NomeUsuario.ToLower().Equals(usuario.AcessoAlternativo.ToLower())) && x.SituacaoMovimentacao.Codigo == 1);
					}
					return historicos.FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>(func2) != null;
				}).Select<Movimentacao, VendasPorOperadorMovimentacaoDto>(delegate(Movimentacao movimentacao)
				{
					VendasPorOperadorMovimentacaoDto vendasPorOperadorMovimentacaoDto2 = new VendasPorOperadorMovimentacaoDto();
					vendasPorOperadorMovimentacaoDto2.TotalProdutos = movimentacao.TotalProdutos;
					vendasPorOperadorMovimentacaoDto2.TotalServicos = movimentacao.TotalServicos;
					vendasPorOperadorMovimentacaoDto2.TotalDesconto = movimentacao.TotalDescontos;
					decimal num2;
					if (!string.IsNullOrEmpty(vendedorReferencia))
					{
						IEnumerable<IItem> itens2 = movimentacao.Itens;
						Func<IItem, bool> func3;
						if ((func3 = <>9__13) == null)
						{
							func3 = (<>9__13 = (IItem a) => a.VendedorReferencia.Equals(ObjectId.Parse(vendedorReferencia)));
						}
						num2 = itens2.Where<IItem>(func3).Sum<IItem>((IItem y) => y.TotalLiquido);
					}
					else
					{
						num2 = movimentacao.Itens.Sum<IItem>((IItem y) => y.TotalLiquido);
					}
					vendasPorOperadorMovimentacaoDto2.TotalLiquido = num2;
					vendasPorOperadorMovimentacaoDto2.Total = movimentacao.Total;
					vendasPorOperadorMovimentacaoDto2.EspeciesRecebimento = movimentacao.To<IMovimentacaoEfetivacao>().PagamentoRecebimento.Parcelas.Select<Parcela, VendasPorOperadorEspecieRecebimentoDto>((Parcela parcela) => new VendasPorOperadorEspecieRecebimentoDto
					{
						Especie = parcela.EspeciePagamento.Descricao,
						Valor = parcela.Valor
					});
					return vendasPorOperadorMovimentacaoDto2;
				})
					.ToListEmptyWhenNull<VendasPorOperadorMovimentacaoDto>();
			}
			List<VendasPorOperadorEspecieRecebimentoDto> list2 = (from especie in list.SelectMany<VendasPorOperadorMovimentacaoDto, VendasPorOperadorEspecieRecebimentoDto>((VendasPorOperadorMovimentacaoDto x) => x.EspeciesRecebimento)
				group especie by especie.Especie).Select<IGrouping<string, VendasPorOperadorEspecieRecebimentoDto>, VendasPorOperadorEspecieRecebimentoDto>(delegate(IGrouping<string, VendasPorOperadorEspecieRecebimentoDto> especieGroup)
			{
				VendasPorOperadorEspecieRecebimentoDto vendasPorOperadorEspecieRecebimentoDto = new VendasPorOperadorEspecieRecebimentoDto();
				vendasPorOperadorEspecieRecebimentoDto.Especie = especieGroup.Key;
				vendasPorOperadorEspecieRecebimentoDto.Valor = especieGroup.Sum<VendasPorOperadorEspecieRecebimentoDto>((VendasPorOperadorEspecieRecebimentoDto x) => x.Valor);
				return vendasPorOperadorEspecieRecebimentoDto;
			}).ToListEmptyWhenNull<VendasPorOperadorEspecieRecebimentoDto>();
			DadosRelatorioResumoVendasSinteticoTotalDto dadosRelatorioResumoVendasSinteticoTotalDto = new DadosRelatorioResumoVendasSinteticoTotalDto();
			dadosRelatorioResumoVendasSinteticoTotalDto.TotalProdutos = list.Sum<VendasPorOperadorMovimentacaoDto>((VendasPorOperadorMovimentacaoDto x) => x.TotalProdutos);
			dadosRelatorioResumoVendasSinteticoTotalDto.TotalServicos = list.Sum<VendasPorOperadorMovimentacaoDto>((VendasPorOperadorMovimentacaoDto x) => x.TotalServicos);
			dadosRelatorioResumoVendasSinteticoTotalDto.TotalDesconto = list.Sum<VendasPorOperadorMovimentacaoDto>((VendasPorOperadorMovimentacaoDto x) => x.TotalDesconto);
			dadosRelatorioResumoVendasSinteticoTotalDto.TotalLiquido = list.Sum<VendasPorOperadorMovimentacaoDto>((VendasPorOperadorMovimentacaoDto x) => x.TotalLiquido);
			dadosRelatorioResumoVendasSinteticoTotalDto.Total = list.Sum<VendasPorOperadorMovimentacaoDto>((VendasPorOperadorMovimentacaoDto x) => x.Total);
			DadosRelatorioResumoVendasSinteticoTotalDto dadosRelatorioResumoVendasSinteticoTotalDto2 = dadosRelatorioResumoVendasSinteticoTotalDto;
			return new DadosRelatorioResumoVendasSinteticoDto
			{
				DadosRelatorioResumoVendasSinteticoTotalDto = dadosRelatorioResumoVendasSinteticoTotalDto2,
				ResumoEspecies = list2
			};
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0007FAEC File Offset: 0x0007DCEC
		public async Task<IEnumerable<RelatorioTotalMovimentadoPorVendedorDto>> TotalMovimentadoPorVendedor(string idVendedor, DateTime dataInicial, DateTime dataFinal, CodigoDescricaoDto origem, int situacaoMovimentacao, IUsuarioIdentity usuarioIdentity)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass58_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass58_0();
			CS$<>8__locals1.idVendedor = idVendedor;
			ISpecification<Movimentacao> neSpec = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataEmissao(dataInicial, dataFinal).PorSituacao(situacaoMovimentacao)
				.PorOrigem(origem.Codigo)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			if (!string.IsNullOrEmpty(CS$<>8__locals1.idVendedor))
			{
				neSpec = neSpec.PorVendedor(CS$<>8__locals1.idVendedor);
			}
			CS$<>8__locals1.acumuladoGlobal = new ConcurrentDictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase);
			int skip = 0;
			List<Movimentacao> list;
			do
			{
				list = (await this._movimentacaoRepositoryRead.FindPagedAsync(neSpec.SatisfiedBy(), skip, 500)).ToList<Movimentacao>();
				skip += 500;
				if (!list.Any<Movimentacao>())
				{
					break;
				}
				IEnumerable<Movimentacao> enumerable = list;
				ParallelOptions parallelOptions = new ParallelOptions();
				parallelOptions.MaxDegreeOfParallelism = Math.Max(2, Environment.ProcessorCount / 2);
				Parallel.ForEach<Movimentacao, Dictionary<string, decimal[]>>(enumerable, parallelOptions, () => new Dictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase), delegate(Movimentacao movimentacao, ParallelLoopState loopState, Dictionary<string, decimal[]> localDict)
				{
					foreach (IItem item2 in movimentacao.Itens.Where<IItem>(delegate(IItem item)
					{
						if (!item.Cancelado)
						{
							IItemDocumentoAuxiliar itemDocumentoAuxiliar = item.To<IItemDocumentoAuxiliar>();
							if (itemDocumentoAuxiliar == null || itemDocumentoAuxiliar.OperacaoFiscal.MovimentaEstoque)
							{
								IItemDocumentoAuxiliar itemDocumentoAuxiliar2 = item.To<IItemDocumentoAuxiliar>();
								return itemDocumentoAuxiliar2 == null || itemDocumentoAuxiliar2.OperacaoFiscal.MovimentaFinanceiro;
							}
						}
						return false;
					}))
					{
						string text = item2.VendedorReferencia.ToStringEmptyWhenDefault();
						if (string.IsNullOrEmpty(CS$<>8__locals1.idVendedor) || !(text != CS$<>8__locals1.idVendedor))
						{
							int month = movimentacao.DataHoraEmissao.Month;
							decimal[] array;
							if (!localDict.TryGetValue(text, out array))
							{
								array = new decimal[12];
								localDict[text] = array;
							}
							array[month - 1] += item2.TotalLiquido;
						}
					}
					return localDict;
				}, delegate(Dictionary<string, decimal[]> localDict)
				{
					using (Dictionary<string, decimal[]>.Enumerator enumerator2 = localDict.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<string, decimal[]> kvp2 = enumerator2.Current;
							CS$<>8__locals1.acumuladoGlobal.AddOrUpdate(kvp2.Key, kvp2.Value, delegate(string key, decimal[] existing)
							{
								for (int i = 0; i < 12; i++)
								{
									existing[i] += kvp2.Value[i];
								}
								return existing;
							});
						}
					}
				});
			}
			while (list.Any<Movimentacao>());
			IEnumerable<RelatorioTotalMovimentadoPorVendedorDto> enumerable2;
			if (!CS$<>8__locals1.acumuladoGlobal.Any<KeyValuePair<string, decimal[]>>())
			{
				enumerable2 = Enumerable.Empty<RelatorioTotalMovimentadoPorVendedorDto>();
			}
			else
			{
				IEnumerable<ObjectId> enumerable3 = from id in CS$<>8__locals1.acumuladoGlobal.Keys
					where !string.IsNullOrEmpty(id)
					select id into x
					select x.TryParseReferencia();
				MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass58_0 CS$<>8__locals2 = CS$<>8__locals1;
				IRepositoryRead<Pessoa> pessoaRepositoryRead = this._pessoaRepositoryRead;
				ParameterExpression parameterExpression = Expression.Parameter(typeof(Pessoa), "x");
				CS$<>8__locals2.vendedores = pessoaRepositoryRead.GetIn<ObjectId>(Expression.Lambda<Func<Pessoa, ObjectId>>(Expression.Property(parameterExpression, methodof(EntityBase.get_Id())), new ParameterExpression[] { parameterExpression }), enumerable3).ToDictionary<Pessoa, ObjectId, string>((Pessoa v) => v.Id, (Pessoa v) => v.Nome.ToStringEmptyWhenNull());
				enumerable2 = CS$<>8__locals1.acumuladoGlobal.Select<KeyValuePair<string, decimal[]>, RelatorioTotalMovimentadoPorVendedorDto>(delegate(KeyValuePair<string, decimal[]> kvp)
				{
					string key = kvp.Key;
					decimal[] value = kvp.Value;
					string text2 = CS$<>8__locals1.vendedores.GetValueOrDefault(key.TryParseReferencia()) ?? "Não informado";
					RelatorioTotalMovimentadoPorVendedorDto relatorioTotalMovimentadoPorVendedorDto = new RelatorioTotalMovimentadoPorVendedorDto();
					relatorioTotalMovimentadoPorVendedorDto.VendedorId = key;
					relatorioTotalMovimentadoPorVendedorDto.VendedorNome = text2;
					relatorioTotalMovimentadoPorVendedorDto.Total = value.Sum<decimal>((decimal x) => x);
					return relatorioTotalMovimentadoPorVendedorDto;
				});
			}
			return enumerable2;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0007FB64 File Offset: 0x0007DD64
		public VendasPorOperadorDto MovimentacoesPorOperador(DateTime dataInicial, DateTime dataFinal, string usuarioReferencia, CodigoDescricaoDto origem, int situacaoMovimentacao, IUsuarioIdentity usuarioIdentity)
		{
			UsuarioBase usuario = this._usuarioRepositoryRead.GetById(ObjectId.Parse(usuarioReferencia));
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorOrigem(origem.Codigo)
				.PorSituacao(situacaoMovimentacao)
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			List<VendasPorOperadorMovimentacaoDto> list = (from movimentacao in this._movimentacaoRepositoryRead.Find(specification)
				where movimentacao.Historicos.FirstOrDefault<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>((DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico x) => (x.NomeUsuario.ToLower().Equals(usuario.Nome.ToLower()) || x.NomeUsuario.ToLower().Equals(usuario.AcessoAlternativo.ToLower())) && x.SituacaoMovimentacao.Codigo == 1) != null
				select movimentacao).Select<Movimentacao, VendasPorOperadorMovimentacaoDto>(delegate(Movimentacao movimentacao)
			{
				VendasPorOperadorMovimentacaoDto vendasPorOperadorMovimentacaoDto = new VendasPorOperadorMovimentacaoDto();
				vendasPorOperadorMovimentacaoDto.Numero = movimentacao.Numero;
				vendasPorOperadorMovimentacaoDto.Cliente = movimentacao.Pessoa.Nome.RemoverCaracteresNaoImprimiveis();
				vendasPorOperadorMovimentacaoDto.Data = movimentacao.DataHoraEmissao;
				vendasPorOperadorMovimentacaoDto.TotalProdutos = movimentacao.TotalProdutos;
				vendasPorOperadorMovimentacaoDto.TotalDesconto = movimentacao.TotalDescontos;
				vendasPorOperadorMovimentacaoDto.Total = movimentacao.Total;
				vendasPorOperadorMovimentacaoDto.Origem = movimentacao.Tipo.RetornarDescricao();
				vendasPorOperadorMovimentacaoDto.Itens = movimentacao.Itens.Select<IItem, VendasPorOperadorItemDto>((IItem item) => new VendasPorOperadorItemDto
				{
					CodigoInterno = item.ProdutoServico.CodigoInterno,
					Descricao = item.ProdutoServico.Descricao,
					Quantidade = item.Quantidade,
					Preco = item.PrecoUnitario,
					Desconto = item.TotalDesconto,
					Total = item.Total
				});
				vendasPorOperadorMovimentacaoDto.Situacao = movimentacao.Situacao.Descricao.ToStringEmptyWhenNull();
				return vendasPorOperadorMovimentacaoDto;
			}).ToListEmptyWhenNull<VendasPorOperadorMovimentacaoDto>();
			return new VendasPorOperadorDto
			{
				Movimentacoes = list
			};
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0007FC10 File Offset: 0x0007DE10
		public DocumentosAuxiliaresPorPeriodoDto DavOuPreVendaOuOrcamentoPorPeriodo(string marcaReferencia, CodigoDescricaoDto situacaoMovimentacao, TipoMovimentacao tipoMovimentacao, DateTime dataInicial, DateTime dataFinal, string pessoaReferencia, string vendedorReferencia, IUsuarioIdentity usuarioIdentity, int tipoFiltroData = 2)
		{
			ISpecification<Movimentacao> specification = EntitySpecification<Movimentacao>.Ativo();
			if (tipoMovimentacao != TipoMovimentacao.Dav)
			{
				if (tipoMovimentacao != TipoMovimentacao.PreVenda)
				{
					if (tipoMovimentacao == TipoMovimentacao.Orcamento)
					{
						specification = specification.PorVendedorResponsavelOrcamento(vendedorReferencia).PorOrigem(14);
					}
				}
				else
				{
					specification = specification.PorVendedorResponsavelPreVenda(vendedorReferencia).PorOrigem(4);
				}
			}
			else
			{
				specification = specification.PorVendedorResponsavelDav(vendedorReferencia).PorOrigem(1);
			}
			specification = ((tipoFiltroData == 1) ? specification.PorDataPrevisao(dataInicial, dataFinal.ToFinalDia()) : specification.PorDataEmissao(dataInicial, dataFinal.ToFinalDia()));
			specification = specification.PorEmpresa(usuarioIdentity.IdEmpresa).PorPessoa(pessoaReferencia).PorSituacao(situacaoMovimentacao.Codigo);
			IEnumerable<ProdutoServico> produtos = new List<ProdutoServico>();
			if (!string.IsNullOrEmpty(marcaReferencia))
			{
				produtos = this._produtoServicoRepositoryRead.Find(ProdutoServicoSpecifications.AtivoPorMarca(marcaReferencia.TryParseReferencia()));
				if (produtos.Any<ProdutoServico>())
				{
					specification = specification.PorProdutoServicoReferencia(produtos.Select<ProdutoServico, ObjectId>((ProdutoServico p) => p.Id));
				}
			}
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification.PorEmpresa(usuarioIdentity.IdEmpresa));
			List<DocumentoAuxiliarPeriodoMovimentacaoDto> list = enumerable.Select((Movimentacao movimentacao) => new
			{
				movimentacao = movimentacao,
				itens = (string.IsNullOrEmpty(marcaReferencia) ? movimentacao.Itens : movimentacao.Itens.Where<IItem>((IItem i) => i.ProdutoServico.ProdutoServicoReferencia.In(produtos.Select<ProdutoServico, ObjectId>((ProdutoServico p) => p.Id).ToArray<ObjectId>())))
			}).Select(delegate(<>h__TransparentIdentifier0)
			{
				DocumentoAuxiliarPeriodoMovimentacaoDto documentoAuxiliarPeriodoMovimentacaoDto = new DocumentoAuxiliarPeriodoMovimentacaoDto();
				documentoAuxiliarPeriodoMovimentacaoDto.Numero = <>h__TransparentIdentifier0.movimentacao.Numero;
				documentoAuxiliarPeriodoMovimentacaoDto.Cliente = <>h__TransparentIdentifier0.movimentacao.Pessoa.Nome.RemoverCaracteresNaoImprimiveis();
				documentoAuxiliarPeriodoMovimentacaoDto.Data = <>h__TransparentIdentifier0.movimentacao.DataHoraEmissao;
				DocumentoAuxiliarVenda documentoAuxiliarVenda = <>h__TransparentIdentifier0.movimentacao.To<DocumentoAuxiliarVenda>();
				documentoAuxiliarPeriodoMovimentacaoDto.DataPrevisao = ((documentoAuxiliarVenda != null) ? documentoAuxiliarVenda.DataHoraPrevisao : <>h__TransparentIdentifier0.movimentacao.DataHoraEmissao);
				documentoAuxiliarPeriodoMovimentacaoDto.TotalProdutos = <>h__TransparentIdentifier0.movimentacao.TotalProdutos;
				documentoAuxiliarPeriodoMovimentacaoDto.Situacao = <>h__TransparentIdentifier0.movimentacao.Situacao.Descricao;
				string text;
				if (!<>h__TransparentIdentifier0.movimentacao.Is<IMovimentacaoPagamentoRecebimento>())
				{
					text = string.Empty;
				}
				else
				{
					text = string.Join("/", <>h__TransparentIdentifier0.movimentacao.To<IMovimentacaoPagamentoRecebimento>().PagamentoRecebimento.Parcelas.Select<Parcela, string>((Parcela parcela) => parcela.EspeciePagamento.Descricao).Distinct<string>());
				}
				documentoAuxiliarPeriodoMovimentacaoDto.Pagamento = text;
				documentoAuxiliarPeriodoMovimentacaoDto.Itens = <>h__TransparentIdentifier0.itens.Select<IItem, DocumentoAuxiliarPeriodoItemDto>((IItem item) => new DocumentoAuxiliarPeriodoItemDto
				{
					CodigoInterno = item.ProdutoServico.CodigoInterno,
					Descricao = item.ProdutoServico.Descricao,
					Quantidade = item.Quantidade,
					Preco = item.PrecoUnitario,
					Total = item.Total,
					Cancelado = item.Cancelado
				});
				documentoAuxiliarPeriodoMovimentacaoDto.Total = <>h__TransparentIdentifier0.movimentacao.Total;
				return documentoAuxiliarPeriodoMovimentacaoDto;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPeriodoMovimentacaoDto>();
			List<DocumentoAuxiliarPorPeriodoItemAcumuladoDto> list2 = (from y in list.SelectMany<DocumentoAuxiliarPeriodoMovimentacaoDto, DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoMovimentacaoDto x) => x.Itens)
				where !y.Cancelado
				select y into g
				group g by new { g.CodigoInterno, g.Descricao }).Select(delegate(s)
			{
				DocumentoAuxiliarPorPeriodoItemAcumuladoDto documentoAuxiliarPorPeriodoItemAcumuladoDto = new DocumentoAuxiliarPorPeriodoItemAcumuladoDto();
				documentoAuxiliarPorPeriodoItemAcumuladoDto.CodigoInterno = s.Key.CodigoInterno;
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Descricao = s.Key.Descricao;
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Quantidade = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Quantidade);
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Total = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Total);
				return documentoAuxiliarPorPeriodoItemAcumuladoDto;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoItemAcumuladoDto>();
			List<DocumentoAuxiliarPorPeriodoItemAcumuladoDto> list3 = (from y in list.SelectMany<DocumentoAuxiliarPeriodoMovimentacaoDto, DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoMovimentacaoDto x) => x.Itens)
				where y.Cancelado
				select y into g
				group g by new { g.CodigoInterno, g.Descricao }).Select(delegate(s)
			{
				DocumentoAuxiliarPorPeriodoItemAcumuladoDto documentoAuxiliarPorPeriodoItemAcumuladoDto2 = new DocumentoAuxiliarPorPeriodoItemAcumuladoDto();
				documentoAuxiliarPorPeriodoItemAcumuladoDto2.CodigoInterno = s.Key.CodigoInterno;
				documentoAuxiliarPorPeriodoItemAcumuladoDto2.Descricao = s.Key.Descricao;
				documentoAuxiliarPorPeriodoItemAcumuladoDto2.Quantidade = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Quantidade);
				documentoAuxiliarPorPeriodoItemAcumuladoDto2.Total = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Total);
				return documentoAuxiliarPorPeriodoItemAcumuladoDto2;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoItemAcumuladoDto>();
			List<DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto> list4 = (from g in list
				group g by new { g.Situacao } into s
				select new DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto
				{
					Situacao = s.Key.Situacao,
					Quantidade = s.Count<DocumentoAuxiliarPeriodoMovimentacaoDto>()
				}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto>();
			List<DocumentosAuxiliaresPorPeriodoResumoPagamentosDto> list5 = (from x in enumerable.OfType<IMovimentacaoPagamentoRecebimento>().SelectMany<IMovimentacaoPagamentoRecebimento, Parcela>((IMovimentacaoPagamentoRecebimento movimentacao) => movimentacao.PagamentoRecebimento.Parcelas).ToListEmptyWhenNull<Parcela>()
				group x by x.EspeciePagamento.Descricao).Select<IGrouping<string, Parcela>, DocumentosAuxiliaresPorPeriodoResumoPagamentosDto>(delegate(IGrouping<string, Parcela> g)
			{
				DocumentosAuxiliaresPorPeriodoResumoPagamentosDto documentosAuxiliaresPorPeriodoResumoPagamentosDto = new DocumentosAuxiliaresPorPeriodoResumoPagamentosDto();
				documentosAuxiliaresPorPeriodoResumoPagamentosDto.Descricao = g.Key;
				documentosAuxiliaresPorPeriodoResumoPagamentosDto.Total = g.Sum<Parcela>((Parcela x) => x.Valor);
				return documentosAuxiliaresPorPeriodoResumoPagamentosDto;
			}).ToListEmptyWhenNull<DocumentosAuxiliaresPorPeriodoResumoPagamentosDto>();
			return new DocumentosAuxiliaresPorPeriodoDto
			{
				Movimentacoes = list,
				ItensAcumulados = list2,
				ItensAcumuladosCancelados = list3,
				ResumoPagamentos = list5,
				ResumoSituacoes = list4
			};
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0007FFAC File Offset: 0x0007E1AC
		public DocumentosAuxiliaresPorPeriodoDto DavOuDavOsOuOrcamentoPendentePorVencimento(DateTime dataInicial, DateTime dataFinal, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specification = EntityBaseEmpresaSpecification<Movimentacao>.PorEmpresa(usuarioIdentity.IdEmpresa).PorTipo<DocumentoAuxiliarVenda>().OrPorTipo<DocumentoAuxiliarVendaOrdemServico>()
				.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.Orcamento>()
				.PorDataPrevisao(dataInicial, dataFinal)
				.PorSituacaoAguardando();
			List<DocumentoAuxiliarPeriodoMovimentacaoDto> list = this._movimentacaoRepositoryRead.Find(specification).Select<Movimentacao, DocumentoAuxiliarPeriodoMovimentacaoDto>(delegate(Movimentacao movimentacao)
			{
				DocumentoAuxiliarPeriodoMovimentacaoDto documentoAuxiliarPeriodoMovimentacaoDto = new DocumentoAuxiliarPeriodoMovimentacaoDto();
				documentoAuxiliarPeriodoMovimentacaoDto.MovimentacaoReferencia = movimentacao.Id.ToString();
				documentoAuxiliarPeriodoMovimentacaoDto.Numero = movimentacao.Numero;
				documentoAuxiliarPeriodoMovimentacaoDto.Cliente = movimentacao.Pessoa.Nome.RemoverCaracteresNaoImprimiveis();
				documentoAuxiliarPeriodoMovimentacaoDto.Data = movimentacao.DataHoraEmissao;
				DocumentoAuxiliarVenda documentoAuxiliarVenda = movimentacao.To<DocumentoAuxiliarVenda>();
				documentoAuxiliarPeriodoMovimentacaoDto.DataPrevisao = ((documentoAuxiliarVenda != null) ? documentoAuxiliarVenda.DataHoraPrevisao : movimentacao.DataHoraEmissao);
				documentoAuxiliarPeriodoMovimentacaoDto.TotalProdutos = movimentacao.TotalProdutos;
				documentoAuxiliarPeriodoMovimentacaoDto.Situacao = movimentacao.Situacao.Descricao;
				string text;
				if (!movimentacao.Is<IMovimentacaoPagamentoRecebimento>())
				{
					text = string.Empty;
				}
				else
				{
					text = string.Join("/", movimentacao.To<IMovimentacaoPagamentoRecebimento>().PagamentoRecebimento.Parcelas.Select<Parcela, string>((Parcela parcela) => parcela.EspeciePagamento.Descricao).Distinct<string>());
				}
				documentoAuxiliarPeriodoMovimentacaoDto.Pagamento = text;
				documentoAuxiliarPeriodoMovimentacaoDto.Total = movimentacao.Total;
				documentoAuxiliarPeriodoMovimentacaoDto.TipoMovimentacao = movimentacao.Tipo.RetornarDescricao();
				return documentoAuxiliarPeriodoMovimentacaoDto;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPeriodoMovimentacaoDto>();
			return new DocumentosAuxiliaresPorPeriodoDto
			{
				Movimentacoes = list
			};
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00080024 File Offset: 0x0007E224
		public DocumentosAuxiliaresPorPeriodoDto DavOsPorPeriodo(CodigoDescricaoDto situacaoMovimentacao, int tipoFiltroData, DateTime dataInicial, DateTime dataFinal, int codigoStatus, int codigoAtendimento, string pessoaReferencia, string vendedorReferencia, CodigoDescricaoDto tipoDavOs, IUsuarioIdentity usuarioIdentity)
		{
			TipoStatusDocumentoAuxiliarOrdemServico tipoStatusDocumentoAuxiliarOrdemServico = StatusDocumentosAuxiliaresOrdensServicos.ObterPorCodigo(codigoStatus);
			ISpecification<Movimentacao> specification = EntitySpecification<Movimentacao>.Ativo().PorVendedorResponsavelDavOs(vendedorReferencia).PorOrigem(2)
				.PorStatusDavOs(tipoStatusDocumentoAuxiliarOrdemServico)
				.PorPessoa(pessoaReferencia);
			if (!tipoDavOs.Codigo.Equals(99))
			{
				specification = specification.PorTipoDavOs(tipoDavOs.Codigo);
			}
			if (!codigoAtendimento.Equals(-1))
			{
				specification = specification.PorAtendimento(codigoAtendimento);
			}
			if (tipoFiltroData == 1)
			{
				specification = specification.PorDataPrevisao(dataInicial, dataFinal.ToFinalDia());
			}
			if (tipoFiltroData == 2)
			{
				specification = specification.PorDataEmissao(dataInicial, dataFinal.ToFinalDia());
			}
			specification = specification.PorSituacao(situacaoMovimentacao.Codigo);
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification.PorEmpresa(usuarioIdentity.IdEmpresa));
			if (tipoFiltroData == 3)
			{
				enumerable = from x in enumerable.OfType<DocumentoAuxiliarVendaOrdemServico>()
					where x.DataHoraTermino >= dataInicial.Date && x.DataHoraTermino <= dataFinal.ToFinalDia()
					select x;
			}
			List<DocumentoAuxiliarPeriodoMovimentacaoDto> list = enumerable.Select<Movimentacao, DocumentoAuxiliarPeriodoMovimentacaoDto>(delegate(Movimentacao movimentacao)
			{
				DocumentoAuxiliarPeriodoMovimentacaoDto documentoAuxiliarPeriodoMovimentacaoDto = new DocumentoAuxiliarPeriodoMovimentacaoDto();
				documentoAuxiliarPeriodoMovimentacaoDto.Numero = movimentacao.Numero;
				documentoAuxiliarPeriodoMovimentacaoDto.Cliente = movimentacao.Pessoa.Nome.RemoverCaracteresNaoImprimiveis();
				DocumentoAuxiliarVendaBase documentoAuxiliarVendaBase = movimentacao.To<DocumentoAuxiliarVendaBase>();
				string text;
				if (documentoAuxiliarVendaBase == null)
				{
					text = null;
				}
				else
				{
					PessoaHistorico vendedor = documentoAuxiliarVendaBase.Vendedor;
					text = ((vendedor != null) ? vendedor.Nome.RemoverCaracteresNaoImprimiveis() : null);
				}
				documentoAuxiliarPeriodoMovimentacaoDto.Vendedor = text ?? string.Empty;
				documentoAuxiliarPeriodoMovimentacaoDto.Data = movimentacao.DataHoraEmissao;
				DocumentoAuxiliarVendaBase documentoAuxiliarVendaBase2 = movimentacao.To<DocumentoAuxiliarVendaBase>();
				documentoAuxiliarPeriodoMovimentacaoDto.DataPrevisao = ((documentoAuxiliarVendaBase2 != null) ? documentoAuxiliarVendaBase2.DataHoraPrevisao : movimentacao.DataHoraEmissao);
				documentoAuxiliarPeriodoMovimentacaoDto.Situacao = movimentacao.Situacao.Descricao;
				documentoAuxiliarPeriodoMovimentacaoDto.TotalProdutos = movimentacao.TotalProdutos;
				documentoAuxiliarPeriodoMovimentacaoDto.TotalServicos = movimentacao.TotalServicos;
				DocumentoAuxiliarVendaOrdemServico documentoAuxiliarVendaOrdemServico = movimentacao.To<DocumentoAuxiliarVendaOrdemServico>();
				IEnumerable<CodigoDescricaoDto> enumerable2;
				if (documentoAuxiliarVendaOrdemServico == null)
				{
					enumerable2 = null;
				}
				else
				{
					IEnumerable<ObjetoMovimentacao> objetos = documentoAuxiliarVendaOrdemServico.Objetos;
					if (objetos == null)
					{
						enumerable2 = null;
					}
					else
					{
						enumerable2 = objetos.Select<ObjetoMovimentacao, CodigoDescricaoDto>((ObjetoMovimentacao x) => new CodigoDescricaoDto
						{
							Descricao = x.ObjetoDefeitoToText
						});
					}
				}
				documentoAuxiliarPeriodoMovimentacaoDto.DefeitosObjeto = enumerable2 ?? new List<CodigoDescricaoDto>();
				string text2 = ", ";
				DocumentoAuxiliarVendaOrdemServico documentoAuxiliarVendaOrdemServico2 = movimentacao.To<DocumentoAuxiliarVendaOrdemServico>();
				IEnumerable<string> enumerable3;
				if (documentoAuxiliarVendaOrdemServico2 == null)
				{
					enumerable3 = null;
				}
				else
				{
					IEnumerable<ObjetoMovimentacao> objetos2 = documentoAuxiliarVendaOrdemServico2.Objetos;
					if (objetos2 == null)
					{
						enumerable3 = null;
					}
					else
					{
						enumerable3 = from x in objetos2
							select x.PlacasSeriaisToText into x
							where !string.IsNullOrEmpty(x)
							select x;
					}
				}
				documentoAuxiliarPeriodoMovimentacaoDto.PlacaSerialObjeto = string.Join(text2, enumerable3 ?? Enumerable.Empty<string>());
				DocumentoAuxiliarVendaOrdemServico documentoAuxiliarVendaOrdemServico3 = movimentacao.To<DocumentoAuxiliarVendaOrdemServico>();
				string text3;
				if (documentoAuxiliarVendaOrdemServico3 == null)
				{
					text3 = null;
				}
				else
				{
					StatusDocumentoAuxiliarOrdemServico statusDocumentoAuxiliarOrdemServico = documentoAuxiliarVendaOrdemServico3.StatusDocumentoAuxiliarOrdemServico;
					text3 = ((statusDocumentoAuxiliarOrdemServico != null) ? statusDocumentoAuxiliarOrdemServico.Status.Descricao : null);
				}
				documentoAuxiliarPeriodoMovimentacaoDto.Status = text3 ?? string.Empty;
				DocumentoAuxiliarVendaOrdemServico documentoAuxiliarVendaOrdemServico4 = movimentacao.To<DocumentoAuxiliarVendaOrdemServico>();
				string text4;
				if (documentoAuxiliarVendaOrdemServico4 == null)
				{
					text4 = null;
				}
				else
				{
					Atendimento atendimento = documentoAuxiliarVendaOrdemServico4.Atendimento;
					text4 = ((atendimento != null) ? atendimento.Descricao : null);
				}
				documentoAuxiliarPeriodoMovimentacaoDto.Atendimento = text4 ?? string.Empty;
				documentoAuxiliarPeriodoMovimentacaoDto.Itens = movimentacao.Itens.Select<IItem, DocumentoAuxiliarPeriodoItemDto>(delegate(IItem item)
				{
					DocumentoAuxiliarPeriodoItemDto documentoAuxiliarPeriodoItemDto = new DocumentoAuxiliarPeriodoItemDto();
					documentoAuxiliarPeriodoItemDto.CodigoInterno = item.ProdutoServico.CodigoInterno;
					documentoAuxiliarPeriodoItemDto.Descricao = item.ProdutoServico.Descricao;
					ItemDocumentoAuxiliarSaidaProduto itemDocumentoAuxiliarSaidaProduto = item.To<ItemDocumentoAuxiliarSaidaProduto>();
					string text5;
					if (itemDocumentoAuxiliarSaidaProduto == null)
					{
						text5 = null;
					}
					else
					{
						PessoaHistorico vendedor2 = itemDocumentoAuxiliarSaidaProduto.Vendedor;
						text5 = ((vendedor2 != null) ? vendedor2.Nome.RemoverCaracteresNaoImprimiveis() : null);
					}
					documentoAuxiliarPeriodoItemDto.Vendedor = text5 ?? string.Empty;
					documentoAuxiliarPeriodoItemDto.Quantidade = item.Quantidade;
					documentoAuxiliarPeriodoItemDto.Preco = item.PrecoUnitario;
					documentoAuxiliarPeriodoItemDto.Total = item.Total;
					documentoAuxiliarPeriodoItemDto.Cancelado = item.Cancelado;
					return documentoAuxiliarPeriodoItemDto;
				});
				documentoAuxiliarPeriodoMovimentacaoDto.Total = movimentacao.Total;
				return documentoAuxiliarPeriodoMovimentacaoDto;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPeriodoMovimentacaoDto>();
			List<DocumentoAuxiliarPorPeriodoItemAcumuladoDto> list2 = (from y in list.SelectMany<DocumentoAuxiliarPeriodoMovimentacaoDto, DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoMovimentacaoDto x) => x.Itens)
				where !y.Cancelado
				select y into g
				group g by new { g.CodigoInterno, g.Descricao }).Select(delegate(s)
			{
				DocumentoAuxiliarPorPeriodoItemAcumuladoDto documentoAuxiliarPorPeriodoItemAcumuladoDto = new DocumentoAuxiliarPorPeriodoItemAcumuladoDto();
				documentoAuxiliarPorPeriodoItemAcumuladoDto.CodigoInterno = s.Key.CodigoInterno;
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Descricao = s.Key.Descricao;
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Quantidade = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Quantidade);
				documentoAuxiliarPorPeriodoItemAcumuladoDto.Total = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Total);
				return documentoAuxiliarPorPeriodoItemAcumuladoDto;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoItemAcumuladoDto>();
			List<DocumentoAuxiliarPorPeriodoItemAcumuladoDto> list3 = (from y in list.SelectMany<DocumentoAuxiliarPeriodoMovimentacaoDto, DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoMovimentacaoDto x) => x.Itens)
				where y.Cancelado
				select y into g
				group g by new { g.CodigoInterno, g.Descricao }).Select(delegate(s)
			{
				DocumentoAuxiliarPorPeriodoItemAcumuladoDto documentoAuxiliarPorPeriodoItemAcumuladoDto2 = new DocumentoAuxiliarPorPeriodoItemAcumuladoDto();
				documentoAuxiliarPorPeriodoItemAcumuladoDto2.CodigoInterno = s.Key.CodigoInterno;
				documentoAuxiliarPorPeriodoItemAcumuladoDto2.Descricao = s.Key.Descricao;
				documentoAuxiliarPorPeriodoItemAcumuladoDto2.Quantidade = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Quantidade);
				documentoAuxiliarPorPeriodoItemAcumuladoDto2.Total = s.Sum<DocumentoAuxiliarPeriodoItemDto>((DocumentoAuxiliarPeriodoItemDto x) => x.Total);
				return documentoAuxiliarPorPeriodoItemAcumuladoDto2;
			}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoItemAcumuladoDto>();
			List<DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto> list4 = (from g in list
				group g by new { g.Situacao } into s
				select new DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto
				{
					Situacao = s.Key.Situacao,
					Quantidade = s.Count<DocumentoAuxiliarPeriodoMovimentacaoDto>()
				}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto>();
			List<DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto> list5 = (from g in list
				group g by new { g.Status } into s
				select new DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto
				{
					Situacao = s.Key.Status,
					Quantidade = s.Count<DocumentoAuxiliarPeriodoMovimentacaoDto>()
				}).ToListEmptyWhenNull<DocumentoAuxiliarPorPeriodoSituacaoAcumuladoDto>();
			List<DocumentosAuxiliaresPorPeriodoResumoPagamentosDto> list6 = (from x in enumerable.OfType<IMovimentacaoPagamentoRecebimento>().SelectMany<IMovimentacaoPagamentoRecebimento, Parcela>((IMovimentacaoPagamentoRecebimento movimentacao) => movimentacao.PagamentoRecebimento.Parcelas).ToListEmptyWhenNull<Parcela>()
				group x by x.EspeciePagamento.Descricao).Select<IGrouping<string, Parcela>, DocumentosAuxiliaresPorPeriodoResumoPagamentosDto>(delegate(IGrouping<string, Parcela> g)
			{
				DocumentosAuxiliaresPorPeriodoResumoPagamentosDto documentosAuxiliaresPorPeriodoResumoPagamentosDto = new DocumentosAuxiliaresPorPeriodoResumoPagamentosDto();
				documentosAuxiliaresPorPeriodoResumoPagamentosDto.Descricao = g.Key;
				documentosAuxiliaresPorPeriodoResumoPagamentosDto.Total = g.Sum<Parcela>((Parcela x) => x.Valor);
				return documentosAuxiliaresPorPeriodoResumoPagamentosDto;
			}).ToListEmptyWhenNull<DocumentosAuxiliaresPorPeriodoResumoPagamentosDto>();
			return new DocumentosAuxiliaresPorPeriodoDto
			{
				Movimentacoes = list,
				ItensAcumulados = list2,
				ItensAcumuladosCancelados = list3,
				ResumoPagamentos = list6,
				ResumoSituacoes = list4,
				ResumoStatus = list5
			};
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x000803D0 File Offset: 0x0007E5D0
		public IEnumerable<MedicamentoControladoMovimentadoDto> MedicamentosControladosVendidos(DateTime dataInicial, DateTime dataFinal, CodigoDescricaoDto classeTerapeuticaDto, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			ClasseTerapeutica classeTerapeutica = FluentFactory<ClasseTerapeuticaFactory>.Init().Criar(classeTerapeuticaDto.Codigo);
			IEnumerable<ProdutoServico> resultado = this._produtoServicoRepositoryRead.Find(ProdutoServicoSpecifications.Ativo().PorTipo(TipoProdutoServico.Medicamento).PorClasseTerapeutica(classeTerapeutica));
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal).PorProdutoServicoReferencia(resultado.Select<ProdutoServico, ObjectId>((ProdutoServico x) => x.Id).ToArray<ObjectId>())
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>());
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var enumerable2 = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			Func<IItemDocumentoFiscalProduto, bool> <>9__4;
			List<<>f__AnonymousType44<long, string, DateTime, TipoMovimentacao, ObjectId, EmpresaHistorico, List<IItemDocumentoFiscalProduto>>> select = enumerable.OfType<DocumentoFiscal>().Select(delegate(DocumentoFiscal x)
			{
				long numero = x.Numero;
				string serie = x.Serie;
				DateTime dataHoraEmissao = x.DataHoraEmissao;
				TipoMovimentacao tipo = x.Tipo;
				ObjectId empresaReferencia = x.EmpresaReferencia;
				EmpresaHistorico empresa2 = x.Empresa;
				IEnumerable<IItemDocumentoFiscalProduto> enumerable3 = x.Itens.OfType<IItemDocumentoFiscalProduto>();
				Func<IItemDocumentoFiscalProduto, bool> func;
				if ((func = <>9__4) == null)
				{
					func = (<>9__4 = (IItemDocumentoFiscalProduto i) => !i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro && i.EstoqueMovimentacao.Is<EstoqueBaseMovimentacao>() && i.ProdutoServico is MedicamentoHistorico && i.ProdutoServico.To<MedicamentoHistorico>().ClasseTerapeutica.Codigo.Equals(classeTerapeuticaDto.Codigo));
				}
				return new
				{
					Numero = numero,
					Serie = serie,
					DataHoraEmissao = dataHoraEmissao,
					Tipo = tipo,
					EmpresaReferencia = empresaReferencia,
					Empresa = empresa2,
					Itens = enumerable3.Where<IItemDocumentoFiscalProduto>(func).ToListEmptyWhenNull<IItemDocumentoFiscalProduto>()
				};
			}).ToListEmptyWhenNull();
			return (from empresa in enumerable2
				from movimentacao in from x in @select
					where x.EmpresaReferencia.Equals(empresa.PessoaReferencia)
					select x
				let itens = movimentacao.Itens
				from item in itens
				let estoque = item.EstoqueMovimentacao as EstoqueBaseMovimentacao
				from itemEstoque in estoque.Lista
				where itemEstoque.Quantidade > 0m
				let medicamento = resultado.FirstOrDefault<ProdutoServico>((ProdutoServico x) => x.Id.Equals(item.ProdutoServico.ProdutoServicoReferencia)) as Medicamento
				select new
				{
					<>h__TransparentIdentifier5 = <>h__TransparentIdentifier5,
					estoqueLote = (itemEstoque as ItemEstoqueLote)
				}).Select(delegate(<>h__TransparentIdentifier6)
			{
				MedicamentoControladoMovimentadoDto medicamentoControladoMovimentadoDto = new MedicamentoControladoMovimentadoDto();
				medicamentoControladoMovimentadoDto.Numero = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.movimentacao.Numero;
				medicamentoControladoMovimentadoDto.DataVenda = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.movimentacao.DataHoraEmissao;
				medicamentoControladoMovimentadoDto.Origem = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.movimentacao.Tipo.RetornarDescricao();
				medicamentoControladoMovimentadoDto.Serie = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.movimentacao.Serie;
				medicamentoControladoMovimentadoDto.Descricao = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.item.ProdutoServico.Descricao;
				medicamentoControladoMovimentadoDto.Codigo = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.item.ProdutoServico.CodigoInterno;
				medicamentoControladoMovimentadoDto.Quantidade = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.itemEstoque.Quantidade;
				ItemEstoqueLote estoqueLote = <>h__TransparentIdentifier6.estoqueLote;
				medicamentoControladoMovimentadoDto.Lote = ((estoqueLote != null) ? estoqueLote.Numero : null) ?? string.Empty;
				MedicamentoHistorico medicamentoHistorico = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.item.ProdutoServico.To<MedicamentoHistorico>();
				string text;
				if ((text = ((medicamentoHistorico != null) ? medicamentoHistorico.RegistroMinisterioSaude : null)) == null)
				{
					Medicamento medicamento = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.medicamento;
					text = ((medicamento != null) ? medicamento.RegistroMinisterioSaude : null) ?? string.Empty;
				}
				medicamentoControladoMovimentadoDto.RegistroMinisterioSaude = text;
				medicamentoControladoMovimentadoDto.Empresa = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.empresa.Nome;
				medicamentoControladoMovimentadoDto.CnpjEmpresa = <>h__TransparentIdentifier6.<>h__TransparentIdentifier5.<>h__TransparentIdentifier4.<>h__TransparentIdentifier3.<>h__TransparentIdentifier2.<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.empresa.CpfCnpjFormatado;
				return medicamentoControladoMovimentadoDto;
			}).ToList<MedicamentoControladoMovimentadoDto>();
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x000806A4 File Offset: 0x0007E8A4
		public async Task<IEnumerable<MovimentacaoRelatorioBaseDto>> MovimentacoesSemReceita(DateTime dataInicial, DateTime dataFinal, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal).PorItensMedicamentoSngpc()
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>());
			IEnumerable<Movimentacao> movimentacoes = this._movimentacaoRepositoryRead.Find(specification);
			IEnumerable<Receita> enumerable = await this._receitaRepositoryRead.FindAsync(EntityEmpresaSpecification<Receita>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa));
			IEnumerable<ObjectId> ids = movimentacoes.Select<Movimentacao, ObjectId>((Movimentacao x) => x.Id).Except<ObjectId>(enumerable.Select<Receita, ObjectId>((Receita x) => x.MovimentacaoReferencia));
			return movimentacoes.Where<Movimentacao>((Movimentacao x) => ids.Any<ObjectId>((ObjectId y) => y == x.Id)).ProjectedAsCollectionMovimentacaoRelatorioBaseDto(1, null);
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0008070C File Offset: 0x0007E90C
		public IEnumerable<VendasPorProdutoPorOperadorDto> ObterVendasPorProdutoEOperador(DateTime dataInicial, DateTime dataFinal, string idEmpresa, string usuarioReferencia)
		{
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(idEmpresa).PorOrigem(101).PorSituacaoConcluido()
				.PorDataEmissao(dataInicial, dataFinal)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			UsuarioBase usuario = this._usuarioRepositoryRead.GetById(ObjectId.Parse(usuarioReferencia));
			Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> <>9__1;
			IEnumerable<IItemDocumentoFiscal> enumerable2 = enumerable.Where<Movimentacao>(delegate(Movimentacao x)
			{
				IEnumerable<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico> historicos = x.Historicos;
				Func<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico y) => y.NomeUsuario.ToLower().Equals(usuario.Nome.ToLower()) || y.NomeUsuario.ToLower().Equals(usuario.AcessoAlternativo.ToLower()));
				}
				return historicos.Any<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Historico>(func);
			}).ToListEmptyWhenNull<Movimentacao>().SelectMany<Movimentacao, IItemDocumentoFiscal>((Movimentacao m) => from x in m.Itens.OfType<IItemDocumentoFiscal>()
				where !x.Cancelado && x.OperacaoFiscal.MovimentaEstoque && x.OperacaoFiscal.MovimentaFinanceiro
				select x);
			IEnumerable<ObjectId> enumerable3 = enumerable2.Select<IItemDocumentoFiscal, ObjectId>((IItemDocumentoFiscal x) => x.ProdutoServico.ProdutoServicoReferencia).Distinct<ObjectId>();
			IEnumerable<ProdutoServico> produtosServicos = this._produtoServicoRepositoryRead.GetIn<ObjectId>((ProdutoServico x) => x.Id, enumerable3);
			return (from x in enumerable2
				orderby x.DataHora
				group x by new { x.ProdutoServico.ProdutoServicoReferencia }).Select(delegate(g)
			{
				ProdutoServico produtoServico = produtosServicos.FirstOrDefault<ProdutoServico>((ProdutoServico x) => x.Id.Equals(g.Key.ProdutoServicoReferencia));
				VendasPorProdutoPorOperadorDto vendasPorProdutoPorOperadorDto = new VendasPorProdutoPorOperadorDto();
				vendasPorProdutoPorOperadorDto.CodigoInterno = produtoServico.CodigoInterno.ToStringEmptyWhenNull();
				vendasPorProdutoPorOperadorDto.Descricao = produtoServico.Descricao.ToStringEmptyWhenNull();
				vendasPorProdutoPorOperadorDto.Quantidade = g.Sum<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => x.Quantidade);
				vendasPorProdutoPorOperadorDto.TotalVenda = g.Sum<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => x.Total);
				return vendasPorProdutoPorOperadorDto;
			}).ToListEmptyWhenNull<VendasPorProdutoPorOperadorDto>();
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0008087C File Offset: 0x0007EA7C
		public IEnumerable<HistoricoObjetoDto> ObterHistoricoObjeto(string objetoReferencia, FiltroDataHoraDavOs filtroData, DateTime dataInicial, DateTime dataFinal, CodigoDescricaoDto situacaoMovimentacao, string tecnicoReferencia, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			return this._movimentacaoRepositoryRead.ObterHistoricoObjeto(objetoReferencia, filtroData, dataInicial, dataFinal, situacaoMovimentacao, tecnicoReferencia, consolidado, usuarioIdentity);
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x000808A4 File Offset: 0x0007EAA4
		public async Task<IEnumerable<RelatorioVendasCartaoDto>> ObterRelatorioVendasCartao(DateTime dataInicial, DateTime dataFinal, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindAsync(EntityBaseEmpresaSpecification<Movimentacao>.PorEmpresa(usuarioIdentity.IdEmpresa).PorDataHoraEmissao(dataInicial, dataFinal).PorSituacaoConcluido()
				.PorNaturezaOperacao(1)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()));
			List<RelatorioVendasCartaoDto> list = enumerable.OfType<IMovimentacaoPagamento>().SelectMany<IMovimentacaoPagamento, ItemPagamento>((IMovimentacaoPagamento x) => x.Pagamentos.ToListEmptyWhenNull<ItemPagamento>()).OfType<IItemPagamentoCartao>()
				.SelectMany<IItemPagamentoCartao, IInformacaoTransacao>((IItemPagamentoCartao x) => x.InformacoesTransacaoRelatorio)
				.GroupBy((IInformacaoTransacao x) => new { x.DescricaoRelatorio, x.Tipo, x.CodigoAutorizacaoTransacao }, delegate(redeTipo, IEnumerable<IInformacaoTransacao> tefs)
				{
					RelatorioVendasCartaoDto relatorioVendasCartaoDto = new RelatorioVendasCartaoDto();
					relatorioVendasCartaoDto.Rede = redeTipo.DescricaoRelatorio;
					relatorioVendasCartaoDto.Valor = tefs.Sum<IInformacaoTransacao>((IInformacaoTransacao x) => x.Valor);
					relatorioVendasCartaoDto.Tipo = redeTipo.Tipo;
					relatorioVendasCartaoDto.Autorizacao = redeTipo.CodigoAutorizacaoTransacao.ToStringEmptyWhenNull();
					return relatorioVendasCartaoDto;
				})
				.ToListEmptyWhenNull<RelatorioVendasCartaoDto>();
			List<RelatorioVendasCartaoDto> list2 = (from x in enumerable.Where<Movimentacao>(delegate(Movimentacao x)
				{
					if (x.Tipo != TipoMovimentacao.CupomFiscal && x.Tipo != TipoMovimentacao.CupomFiscalEletronico && x.Tipo != TipoMovimentacao.NotaFiscalConsumidorEletronica && x.Tipo != TipoMovimentacao.RegistroVenda)
					{
						IMovimentacaoPagamento movimentacaoPagamento = x.To<IMovimentacaoPagamento>();
						return movimentacaoPagamento == null || !movimentacaoPagamento.Pagamentos.ToListEmptyWhenNull<ItemPagamento>().Any<ItemPagamento>();
					}
					return false;
				}).OfType<IMovimentacaoPagamentoRecebimento>().SelectMany<IMovimentacaoPagamentoRecebimento, Parcela>((IMovimentacaoPagamentoRecebimento x) => x.PagamentoRecebimento.Parcelas)
				where x.EspeciePagamento is EspeciePagamentoCartao
				select x).GroupBy((Parcela x) => new
			{
				x.EspeciePagamento.To<EspeciePagamentoCartao>().DescricaoRelatorio,
				x.EspeciePagamento.To<EspeciePagamentoCartao>().TipoRelatorio,
				x.EspeciePagamento.To<EspeciePagamentoCartao>().CodigoAutorizacaoTransacao
			}, delegate(redeTipo, IEnumerable<Parcela> parcelas)
			{
				RelatorioVendasCartaoDto relatorioVendasCartaoDto2 = new RelatorioVendasCartaoDto();
				relatorioVendasCartaoDto2.Rede = redeTipo.DescricaoRelatorio;
				relatorioVendasCartaoDto2.Valor = parcelas.Sum<Parcela>((Parcela x) => x.Valor);
				relatorioVendasCartaoDto2.Tipo = redeTipo.TipoRelatorio;
				relatorioVendasCartaoDto2.Autorizacao = redeTipo.CodigoAutorizacaoTransacao.ToStringEmptyWhenNull();
				return relatorioVendasCartaoDto2;
			}).ToListEmptyWhenNull<RelatorioVendasCartaoDto>();
			return list.Union<RelatorioVendasCartaoDto>(list2);
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00080904 File Offset: 0x0007EB04
		public async Task<IEnumerable<RelatorioObjetoPessoaDto>> ObterObjetosPorDataGarantia(IUsuarioIdentity usuarioIdentity, DateTime dataInicial, DateTime dataFinal)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass68_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass68_0();
			CS$<>8__locals1.dataInicial = dataInicial;
			CS$<>8__locals1.dataFinal = dataFinal;
			var enumerable = (await this._movimentacaoRepositoryRead.FindAsync(EntityBaseEmpresaSpecification<Movimentacao>.PorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorDataGarantiaObjeto(CS$<>8__locals1.dataInicial, CS$<>8__locals1.dataFinal))).Cast<DocumentoAuxiliarVendaOrdemServico>().SelectMany(delegate(DocumentoAuxiliarVendaOrdemServico davOS)
			{
				IEnumerable<ObjetoMovimentacao> objetos = davOS.Objetos;
				Func<ObjetoMovimentacao, bool> func;
				if ((func = CS$<>8__locals1.<>9__1) == null)
				{
					func = (CS$<>8__locals1.<>9__1 = (ObjetoMovimentacao objeto) => objeto.DataFinalGarantia >= CS$<>8__locals1.dataInicial.Date && objeto.DataFinalGarantia <= CS$<>8__locals1.dataFinal.ToFinalDia());
				}
				return from objetoMovimentacao in objetos.Where<ObjetoMovimentacao>(func)
					select new { objetoMovimentacao, davOS.Pessoa, davOS.Numero };
			});
			IEnumerable<ObjectId> enumerable2 = enumerable.Select(x => x.Pessoa.PessoaReferencia).Distinct<ObjectId>();
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass68_0 CS$<>8__locals2 = CS$<>8__locals1;
			IRepositoryRead<Pessoa> pessoaRepositoryRead = this._pessoaRepositoryRead;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Pessoa), "x");
			CS$<>8__locals2.pessoas = pessoaRepositoryRead.GetIn<ObjectId>(Expression.Lambda<Func<Pessoa, ObjectId>>(Expression.Property(parameterExpression, methodof(EntityBase.get_Id())), new ParameterExpression[] { parameterExpression }), enumerable2);
			return from x in enumerable.GroupBy(x => x.objetoMovimentacao.Objeto.Id, delegate(ObjectId objetoId, itens)
				{
					RelatorioObjetoPessoaDto relatorioObjetoPessoaDto = new RelatorioObjetoPessoaDto();
					relatorioObjetoPessoaDto.Objeto = itens.FirstOrDefault().objetoMovimentacao.Objeto.ProjectedAsDto();
					var func2;
					if ((func2 = CS$<>8__locals1.<>9__6) == null)
					{
						func2 = (CS$<>8__locals1.<>9__6 = delegate(x)
						{
							Pessoa pessoa = CS$<>8__locals1.pessoas.FirstOrDefault<Pessoa>((Pessoa p) => p.Id.Equals(x.Pessoa.PessoaReferencia));
							List<string> list = new List<string>();
							if (pessoa.Carteira.TelefonePrincipal.IsNotNull())
							{
								list.Add(string.Format("Principal: {0}", pessoa.Carteira.TelefonePrincipal.Numero));
							}
							if (pessoa.Carteira.TelefoneWhatsApp.IsNotNull())
							{
								list.Add(string.Format("WhatsApp: {0}", pessoa.Carteira.TelefoneWhatsApp.Numero));
							}
							if (pessoa.Carteira.Celulares.ToListEmptyWhenNull<TelefoneMovel>().Any<TelefoneMovel>())
							{
								list.Add(string.Format("Celulares: {0}", string.Join(",", pessoa.Carteira.Celulares.Select<TelefoneMovel, string>((TelefoneMovel c) => c.Numero))));
							}
							if (!list.Any<string>())
							{
								list.Add("Não informado");
							}
							return new RelatorioObjetoPessoaItemDto
							{
								DataGarantia = x.objetoMovimentacao.DataFinalGarantia,
								Personalizado = x.objetoMovimentacao.CamposPersonalizados.ProjectedAsDto(),
								PessoaNome = pessoa.Nome,
								Telefones = string.Join(" ", list),
								NumeroDavOs = x.Numero
							};
						});
					}
					relatorioObjetoPessoaDto.Itens = from x in itens.Select(func2)
						orderby x.DataGarantia
						select x;
					return relatorioObjetoPessoaDto;
				})
				orderby x.Objeto.ToTextPlacaSerial
				select x;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00080964 File Offset: 0x0007EB64
		public IEnumerable<MovimentacaoPorUfDto> VendasPorUf(DateTime dataInicial, DateTime dataFinal)
		{
			return from g in this._movimentacaoRepositoryRead.Find(MovimentacaoSpecification.PorOrigem(101).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal)
					.PorFinalidadeEmissaoNormal()
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()).GroupBy(delegate(Movimentacao m)
				{
					EnderecoHistorico enderecoPrincipal = m.Pessoa.EnderecoPrincipal;
					string text;
					if (!string.IsNullOrEmpty((enderecoPrincipal != null) ? enderecoPrincipal.CodigoPostal : null))
					{
						text = "EX";
					}
					else
					{
						EnderecoHistorico enderecoPrincipal2 = m.Pessoa.EnderecoPrincipal;
						if ((text = ((enderecoPrincipal2 != null) ? enderecoPrincipal2.Municipio.Uf.Sigla : null)) == null)
						{
							EnderecoHistorico enderecoPrincipal3 = m.Empresa.EnderecoPrincipal;
							if (enderecoPrincipal3 == null)
							{
								text = null;
							}
							else
							{
								Municipio municipio = enderecoPrincipal3.Municipio;
								if (municipio == null)
								{
									text = null;
								}
								else
								{
									UF uf = municipio.Uf;
									text = ((uf != null) ? uf.Sigla : null);
								}
							}
						}
					}
					EnderecoHistorico enderecoPrincipal4 = m.Pessoa.EnderecoPrincipal;
					string text2;
					if (!string.IsNullOrEmpty((enderecoPrincipal4 != null) ? enderecoPrincipal4.CodigoPostal : null))
					{
						text2 = "Exterior";
					}
					else
					{
						EnderecoHistorico enderecoPrincipal5 = m.Pessoa.EnderecoPrincipal;
						if ((text2 = ((enderecoPrincipal5 != null) ? enderecoPrincipal5.Municipio.Uf.Nome : null)) == null)
						{
							EnderecoHistorico enderecoPrincipal6 = m.Empresa.EnderecoPrincipal;
							if (enderecoPrincipal6 == null)
							{
								text2 = null;
							}
							else
							{
								Municipio municipio2 = enderecoPrincipal6.Municipio;
								text2 = ((municipio2 != null) ? municipio2.Uf.Nome : null);
							}
						}
					}
					return new
					{
						Sigla = text,
						Nome = text2
					};
				}).Select(delegate(arg)
				{
					string sigla = arg.Key.Sigla;
					string nome = arg.Key.Nome;
					Movimentacao movimentacao = arg.Last<Movimentacao>();
					return new
					{
						Sigla = sigla,
						Nome = nome,
						UltimaVenda = ((movimentacao != null) ? movimentacao.DataHoraEmissao : DateTime.MinValue),
						Total = arg.Sum<Movimentacao>((Movimentacao x) => x.Total)
					};
				})
					.ToListEmptyWhenNull()
				select new MovimentacaoPorUfDto
				{
					Sigla = g.Sigla,
					Nome = g.Nome,
					Total = g.Total,
					DataUltimaMovimentacao = g.UltimaVenda
				};
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00080A0C File Offset: 0x0007EC0C
		public IEnumerable<VendasPorUfDetalhadoDto> VendasPorUfDetalhado(DateTime dataInicial, DateTime dataFinal, string pessoaReferencia, int uf, bool agrupar, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.FindSemXmlHistoricos(MovimentacaoSpecification.PorOrigem(101).PorSituacaoConcluido().PorPessoa(pessoaReferencia)
				.PorDataEmissao(dataInicial, dataFinal)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()
				.PorUFDestinatario(uf));
			IEnumerable<ObjectId> enumerable2 = enumerable.SelectMany<Movimentacao, ObjectId>((Movimentacao x) => x.ItensBase.Select<ItemBase, ObjectId>((ItemBase i) => i.ProdutoServicoEmpresaReferencia)).Distinct<ObjectId>();
			IEnumerable<ProdutoServicoEmpresa> @in = this._produtoServicoEmpresaRepositoryRead.GetIn<ObjectId>((ProdutoServicoEmpresa x) => x.Id, enumerable2);
			IDictionary<ObjectId, ProdutoServicoEmpresa> dictionary = MovimentacaoSaidaRelatorioAppService.PorId(@in);
			IDictionary<ObjectId, List<PrecoHistorico>> dictionary2 = this.ObterHistoricosPrecoCusto(@in);
			string text = this._pessoaRepositoryRead.FindOne(PessoaSpecifications.PorId(usuarioIdentity.IdEmpresa)).Carteira.EnderecoPrincipalNacional.Municipio.Uf.Sigla.ToStringEmptyWhenNull();
			List<VendasPorUfDetalhadoDto> list = new List<VendasPorUfDetalhadoDto>();
			foreach (Movimentacao movimentacao in enumerable)
			{
				if (!movimentacao.Itens.OfType<IItemDocumentoAuxiliar>().All<IItemDocumentoAuxiliar>(delegate(IItemDocumentoAuxiliar x)
				{
					OperacaoFiscal operacaoFiscal = x.OperacaoFiscal;
					if (operacaoFiscal == null || !operacaoFiscal.MovimentaEstoque)
					{
						OperacaoFiscal operacaoFiscal2 = x.OperacaoFiscal;
						return operacaoFiscal2 == null || !operacaoFiscal2.MovimentaFinanceiro;
					}
					return false;
				}))
				{
					foreach (IItemDocumentoAuxiliar itemDocumentoAuxiliar in from i in movimentacao.Itens.OfType<IItemDocumentoAuxiliar>()
						where !i.Cancelado
						select i)
					{
						ProdutoServicoEmpresa produtoServicoEmpresa = MovimentacaoSaidaRelatorioAppService.ObterProdutoServico(dictionary, itemDocumentoAuxiliar.ProdutoServicoEmpresaReferencia);
						decimal num = ((produtoServicoEmpresa is ServicoEmpresa) ? produtoServicoEmpresa.ValorCusto : MovimentacaoSaidaRelatorioAppService.RetornarPrecoCustoAnteriorADataMovimentacao(dictionary2, itemDocumentoAuxiliar.DataHora, produtoServicoEmpresa.Preco.Id));
						decimal totalLiquido = itemDocumentoAuxiliar.TotalLiquido;
						decimal num2 = num * itemDocumentoAuxiliar.Quantidade;
						string text2 = movimentacao.Pessoa.Nome.ToStringEmptyWhenNull();
						text2 = ((text2.ToUpper() == "CLIENTE CONSUMIDOR" && !movimentacao.Pessoa.Documento.IsNullOrEmpty()) ? "Cliente" : text2);
						EnderecoHistorico enderecoPrincipal = movimentacao.Pessoa.EnderecoPrincipal;
						string text3;
						if (enderecoPrincipal == null)
						{
							text3 = null;
						}
						else
						{
							Municipio municipio = enderecoPrincipal.Municipio;
							if (municipio == null)
							{
								text3 = null;
							}
							else
							{
								UF uf2 = municipio.Uf;
								text3 = ((uf2 != null) ? uf2.Sigla.ToStringEmptyWhenNull() : null);
							}
						}
						string text4 = text3 ?? string.Empty;
						text4 = (text4.IsNullOrEmpty() ? text : text4);
						list.Add(new VendasPorUfDetalhadoDto
						{
							DocumentoPessoa = (movimentacao.Pessoa.Documento.IsNullOrEmpty() ? "" : movimentacao.Pessoa.DocumentoFormatado),
							NomePessoa = text2,
							Uf = text4,
							Codigo = itemDocumentoAuxiliar.ProdutoServico.CodigoInterno.ToStringEmptyWhenNull(),
							Descricao = itemDocumentoAuxiliar.ProdutoServico.Descricao.ToStringEmptyWhenNull(),
							Quantidade = itemDocumentoAuxiliar.Quantidade,
							Desconto = itemDocumentoAuxiliar.TotalDesconto,
							Acrescimo = itemDocumentoAuxiliar.TotalOutrasDespesas,
							Preco = itemDocumentoAuxiliar.PrecoUnitario,
							VendaTotal = totalLiquido,
							CustoTotal = num2,
							ValorLucro = totalLiquido - num2,
							PercentualLucro = ((num2 > 0m) ? ((totalLiquido - num2) / num2 * 100m) : 100m)
						});
					}
				}
			}
			if (!agrupar)
			{
				return list;
			}
			return (from x in list
				group x by new { x.Codigo, x.Uf, x.DocumentoPessoa, x.Descricao }).Select(delegate(g)
			{
				VendasPorUfDetalhadoDto vendasPorUfDetalhadoDto = new VendasPorUfDetalhadoDto();
				vendasPorUfDetalhadoDto.DocumentoPessoa = g.Key.DocumentoPessoa;
				vendasPorUfDetalhadoDto.NomePessoa = g.FirstOrDefault<VendasPorUfDetalhadoDto>().NomePessoa;
				vendasPorUfDetalhadoDto.Uf = g.Key.Uf;
				vendasPorUfDetalhadoDto.Codigo = g.Key.Codigo;
				vendasPorUfDetalhadoDto.Descricao = g.Key.Descricao;
				vendasPorUfDetalhadoDto.Quantidade = g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.Quantidade);
				vendasPorUfDetalhadoDto.Desconto = g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.Desconto);
				vendasPorUfDetalhadoDto.Acrescimo = g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.Acrescimo);
				vendasPorUfDetalhadoDto.Preco = g.Average<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.Preco);
				vendasPorUfDetalhadoDto.VendaTotal = g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.VendaTotal);
				vendasPorUfDetalhadoDto.CustoTotal = g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.CustoTotal);
				vendasPorUfDetalhadoDto.ValorLucro = g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.VendaTotal) - g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.CustoTotal);
				decimal num3;
				if (!(g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.CustoTotal) > 0m))
				{
					num3 = 100m;
				}
				else
				{
					num3 = (g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.VendaTotal) - g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.CustoTotal)) / g.Sum<VendasPorUfDetalhadoDto>((VendasPorUfDetalhadoDto a) => a.CustoTotal) * 100m;
				}
				vendasPorUfDetalhadoDto.PercentualLucro = num3;
				return vendasPorUfDetalhadoDto;
			});
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00080E44 File Offset: 0x0007F044
		public IEnumerable<MovimentacaoPorMunicipioDto> MovimentacoesPorMunicipio(DateTime dataInicial, DateTime dataFinal, string vendedorReferencia, int origem, int situacao)
		{
			return from g in this._movimentacaoRepositoryRead.Find(MovimentacaoSpecification.PorOrigem(origem).PorSituacao(situacao).PorVendedor(vendedorReferencia)
					.PorDataEmissao(dataInicial, dataFinal)
					.PorFinalidadeEmissaoNormal()
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro()).GroupBy(delegate(Movimentacao m)
				{
					string text;
					if (!m.Pessoa.EnderecoPrincipal.IsNull())
					{
						EnderecoHistorico enderecoPrincipal = m.Pessoa.EnderecoPrincipal;
						if (enderecoPrincipal == null)
						{
							text = null;
						}
						else
						{
							Municipio municipio = enderecoPrincipal.Municipio;
							text = ((municipio != null) ? municipio.Nome : null);
						}
					}
					else
					{
						EnderecoHistorico enderecoPrincipal2 = m.Empresa.EnderecoPrincipal;
						if (enderecoPrincipal2 == null)
						{
							text = null;
						}
						else
						{
							Municipio municipio2 = enderecoPrincipal2.Municipio;
							text = ((municipio2 != null) ? municipio2.Nome : null);
						}
					}
					return new
					{
						Municipio = text
					};
				}).Select(delegate(arg)
				{
					string municipio3 = arg.Key.Municipio;
					Movimentacao movimentacao = arg.Last<Movimentacao>();
					return new
					{
						Municipio = municipio3,
						UltimaVenda = ((movimentacao != null) ? movimentacao.DataHoraEmissao : DateTime.MinValue),
						Total = arg.Sum<Movimentacao>((Movimentacao x) => x.Total)
					};
				})
					.ToListEmptyWhenNull()
				select new MovimentacaoPorMunicipioDto
				{
					Nome = g.Municipio,
					Total = g.Total,
					DataUltimaMovimentacao = g.UltimaVenda
				};
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00080EF4 File Offset: 0x0007F0F4
		public IEnumerable<SaidaSubstituicaoTributariaPorProdutoDto> ObterSaidaSubstituicaoTributariaPorProduto(string idEmpresa, DateTime dataInicial, DateTime dataFinal, bool mostrarDetalhes, int codigoOrigem)
		{
			return (from x in this._movimentacaoRepositoryRead.Find(EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(idEmpresa).PorSituacaoConcluido().PorDocumentoFiscal()
					.PorDataEmissao(dataInicial, dataFinal)
					.PorOrigem(codigoOrigem)).SelectMany<Movimentacao, IItem>((Movimentacao x) => x.Itens).OfType<IItemDocumentoAuxiliarProduto>()
				where !x.Cancelado && x.OperacaoFiscal.MovimentaEstoque && (x.Percentual is IPercentualCobradoAnteriormentePorSubstituicaoTributaria || x.Percentual is IPercentualSubstituicaoTributaria)
				select x into item
				group item by item.ProdutoServicoEmpresaReferencia).Select<IGrouping<ObjectId, IItemDocumentoAuxiliarProduto>, SaidaSubstituicaoTributariaPorProdutoDto>(delegate(IGrouping<ObjectId, IItemDocumentoAuxiliarProduto> itemAgrupado)
			{
				IItemDocumentoAuxiliarProduto itemDocumentoAuxiliarProduto = itemAgrupado.FirstOrDefault<IItemDocumentoAuxiliarProduto>();
				SaidaSubstituicaoTributariaPorProdutoDto saidaSubstituicaoTributariaPorProdutoDto = new SaidaSubstituicaoTributariaPorProdutoDto();
				saidaSubstituicaoTributariaPorProdutoDto.CodigoInterno = itemDocumentoAuxiliarProduto.ProdutoServico.CodigoInterno;
				saidaSubstituicaoTributariaPorProdutoDto.Descricao = itemDocumentoAuxiliarProduto.ProdutoServico.Descricao;
				saidaSubstituicaoTributariaPorProdutoDto.Ncm = itemDocumentoAuxiliarProduto.NCMNBS.Codigo;
				saidaSubstituicaoTributariaPorProdutoDto.Quantidade = itemAgrupado.Sum<IItemDocumentoAuxiliarProduto>((IItemDocumentoAuxiliarProduto x) => x.Quantidade);
				saidaSubstituicaoTributariaPorProdutoDto.TotalLiquido = itemAgrupado.Sum<IItemDocumentoAuxiliarProduto>((IItemDocumentoAuxiliarProduto x) => x.TotalLiquido);
				saidaSubstituicaoTributariaPorProdutoDto.ValorBaseCalculoSt = itemAgrupado.Select<IItemDocumentoAuxiliarProduto, IPercentual>((IItemDocumentoAuxiliarProduto x) => x.Percentual).OfType<IPercentualSubstituicaoTributaria>().Sum<IPercentualSubstituicaoTributaria>((IPercentualSubstituicaoTributaria x) => x.ValorBaseCalculoSt);
				saidaSubstituicaoTributariaPorProdutoDto.ValorSt = itemAgrupado.Select<IItemDocumentoAuxiliarProduto, IPercentual>((IItemDocumentoAuxiliarProduto x) => x.Percentual).OfType<IPercentualSubstituicaoTributaria>().Sum<IPercentualSubstituicaoTributaria>((IPercentualSubstituicaoTributaria x) => x.ValorSt);
				saidaSubstituicaoTributariaPorProdutoDto.BaseCalculoStRetido = itemAgrupado.Select<IItemDocumentoAuxiliarProduto, IPercentual>((IItemDocumentoAuxiliarProduto x) => x.Percentual).OfType<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>().Sum<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>((IPercentualCobradoAnteriormentePorSubstituicaoTributaria x) => x.BaseCalculoStRetido);
				saidaSubstituicaoTributariaPorProdutoDto.ValorStRetido = itemAgrupado.Select<IItemDocumentoAuxiliarProduto, IPercentual>((IItemDocumentoAuxiliarProduto x) => x.Percentual).OfType<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>().Sum<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>((IPercentualCobradoAnteriormentePorSubstituicaoTributaria x) => x.ValorIcmsStRetido);
				saidaSubstituicaoTributariaPorProdutoDto.BaseCalculoIcmsEfetivo = itemAgrupado.Select<IItemDocumentoAuxiliarProduto, IPercentual>((IItemDocumentoAuxiliarProduto x) => x.Percentual).OfType<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>().Sum<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>((IPercentualCobradoAnteriormentePorSubstituicaoTributaria x) => x.BaseCalculoIcmsEfetivo);
				saidaSubstituicaoTributariaPorProdutoDto.ValorIcmsEfetivo = itemAgrupado.Select<IItemDocumentoAuxiliarProduto, IPercentual>((IItemDocumentoAuxiliarProduto x) => x.Percentual).OfType<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>().Sum<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>((IPercentualCobradoAnteriormentePorSubstituicaoTributaria x) => x.ValorIcmsEfetivo);
				IEnumerable<ItemMovimentacaoPorProdutoDto> enumerable;
				if (!mostrarDetalhes)
				{
					enumerable = Enumerable.Empty<ItemMovimentacaoPorProdutoDto>();
				}
				else
				{
					IEnumerable<ItemMovimentacaoPorProdutoDto> enumerable2 = (from x in itemAgrupado.Select<IItemDocumentoAuxiliarProduto, ItemMovimentacaoPorProdutoDto>(delegate(IItemDocumentoAuxiliarProduto i)
						{
							ItemMovimentacaoPorProdutoDto itemMovimentacaoPorProdutoDto = new ItemMovimentacaoPorProdutoDto();
							itemMovimentacaoPorProdutoDto.Pessoa = i.Destinatario.Nome.RemoverCaracteresNaoImprimiveis();
							itemMovimentacaoPorProdutoDto.NumeroMovimentacao = i.NumeroMovimentacao;
							itemMovimentacaoPorProdutoDto.Quantidade = i.Quantidade;
							itemMovimentacaoPorProdutoDto.TotalLiquido = i.TotalLiquido;
							IPercentualSubstituicaoTributaria percentualSubstituicaoTributaria = i.Percentual.To<IPercentualSubstituicaoTributaria>();
							itemMovimentacaoPorProdutoDto.ValorBaseCalculoSt = ((percentualSubstituicaoTributaria != null) ? percentualSubstituicaoTributaria.ValorBaseCalculoSt : 0m);
							IPercentualSubstituicaoTributaria percentualSubstituicaoTributaria2 = i.Percentual.To<IPercentualSubstituicaoTributaria>();
							itemMovimentacaoPorProdutoDto.ValorSt = ((percentualSubstituicaoTributaria2 != null) ? percentualSubstituicaoTributaria2.ValorSt : 0m);
							IPercentualCobradoAnteriormentePorSubstituicaoTributaria percentualCobradoAnteriormentePorSubstituicaoTributaria = i.Percentual.To<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>();
							itemMovimentacaoPorProdutoDto.BaseCalculoStRetido = ((percentualCobradoAnteriormentePorSubstituicaoTributaria != null) ? percentualCobradoAnteriormentePorSubstituicaoTributaria.BaseCalculoStRetido : 0m);
							IPercentualCobradoAnteriormentePorSubstituicaoTributaria percentualCobradoAnteriormentePorSubstituicaoTributaria2 = i.Percentual.To<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>();
							itemMovimentacaoPorProdutoDto.ValorStRetido = ((percentualCobradoAnteriormentePorSubstituicaoTributaria2 != null) ? percentualCobradoAnteriormentePorSubstituicaoTributaria2.ValorIcmsStRetido : 0m);
							IPercentualCobradoAnteriormentePorSubstituicaoTributaria percentualCobradoAnteriormentePorSubstituicaoTributaria3 = i.Percentual.To<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>();
							itemMovimentacaoPorProdutoDto.BaseCalculoIcmsEfetivo = ((percentualCobradoAnteriormentePorSubstituicaoTributaria3 != null) ? percentualCobradoAnteriormentePorSubstituicaoTributaria3.BaseCalculoIcmsEfetivo : 0m);
							IPercentualCobradoAnteriormentePorSubstituicaoTributaria percentualCobradoAnteriormentePorSubstituicaoTributaria4 = i.Percentual.To<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>();
							itemMovimentacaoPorProdutoDto.ValorIcmsEfetivo = ((percentualCobradoAnteriormentePorSubstituicaoTributaria4 != null) ? percentualCobradoAnteriormentePorSubstituicaoTributaria4.ValorIcmsEfetivo : 0m);
							IPercentualCobradoAnteriormentePorSubstituicaoTributaria percentualCobradoAnteriormentePorSubstituicaoTributaria5 = i.Percentual.To<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>();
							itemMovimentacaoPorProdutoDto.PercentualBaseCalculoIcmsEfetivo = ((percentualCobradoAnteriormentePorSubstituicaoTributaria5 != null) ? percentualCobradoAnteriormentePorSubstituicaoTributaria5.PercentualBaseCalculoIcmsEfetivo : 0m);
							IPercentualCobradoAnteriormentePorSubstituicaoTributaria percentualCobradoAnteriormentePorSubstituicaoTributaria6 = i.Percentual.To<IPercentualCobradoAnteriormentePorSubstituicaoTributaria>();
							itemMovimentacaoPorProdutoDto.PercentualIcmsEfetivo = ((percentualCobradoAnteriormentePorSubstituicaoTributaria6 != null) ? percentualCobradoAnteriormentePorSubstituicaoTributaria6.PercentualIcmsEfetivo : 0m);
							return itemMovimentacaoPorProdutoDto;
						})
						orderby x.NumeroMovimentacao, x.Pessoa
						select x).ToListEmptyWhenNull<ItemMovimentacaoPorProdutoDto>();
					enumerable = enumerable2;
				}
				saidaSubstituicaoTributariaPorProdutoDto.ItensMovimentacoes = enumerable;
				return saidaSubstituicaoTributariaPorProdutoDto;
			}).ToListEmptyWhenNull<SaidaSubstituicaoTributariaPorProdutoDto>();
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00080FC0 File Offset: 0x0007F1C0
		public IEnumerable<VendasPorProdutoMensalDto> ObterVendasPorProdutoMensal(DateTime dataInicial, DateTime dataFinal, string idEmpresa, IEnumerable<string> idProdutoServico = null)
		{
			IEnumerable<Tuple<DateTime, DateTime>> enumerable = HelperDateTime.QuebrarPeriodoEmDias(dataInicial, dataFinal, 1);
			List<ObjectId> idsProdutoServico = (from id in idProdutoServico ?? Enumerable.Empty<string>()
				where !string.IsNullOrEmpty(id)
				select id into x
				select x.TryParseReferencia()).ToList<ObjectId>();
			ConcurrentBag<List<Parcial>> resultadosParciais = new ConcurrentBag<List<Parcial>>();
			Parallel.ForEach<Tuple<DateTime, DateTime>>(enumerable, new ParallelOptions
			{
				MaxDegreeOfParallelism = Math.Max(2, Environment.ProcessorCount / 2)
			}, delegate(Tuple<DateTime, DateTime> periodo)
			{
				List<Parcial> list2 = new List<Parcial>();
				ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(idEmpresa).PorOrigem(101).PorSituacaoConcluido()
					.PorDataEmissao(periodo.Item1, periodo.Item2)
					.PorFinalidadeEmissaoNormal()
					.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
				ISpecification<Movimentacao> specification2 = (idsProdutoServico.Any<ObjectId>() ? specification.PorProdutoServicoReferencia(idsProdutoServico) : specification);
				int num = 0;
				List<Movimentacao> list3;
				do
				{
					list3 = this._movimentacaoRepositoryRead.FindPaged(specification2.SatisfiedBy(), num, 500).ToList<Movimentacao>();
					num += 500;
					foreach (Movimentacao movimentacao in list3)
					{
						foreach (IItemDocumentoFiscal itemDocumentoFiscal in from x in movimentacao.Itens.OfType<IItemDocumentoFiscal>()
							where !x.Cancelado && x.OperacaoFiscal.MovimentaEstoque && x.OperacaoFiscal.MovimentaFinanceiro
							select x)
						{
							if (!idsProdutoServico.Any<ObjectId>() || idsProdutoServico.Contains(itemDocumentoFiscal.ProdutoServico.ProdutoServicoReferencia))
							{
								list2.Add(new Parcial
								{
									ProdutoServicoReferencia = itemDocumentoFiscal.ProdutoServico.ProdutoServicoReferencia,
									ProdutoServicoEmpresaReferencia = itemDocumentoFiscal.ProdutoServicoEmpresaReferencia,
									Mes = itemDocumentoFiscal.DataHora.Month,
									Ano = itemDocumentoFiscal.DataHora.Year,
									Quantidade = itemDocumentoFiscal.Quantidade,
									Total = itemDocumentoFiscal.Total
								});
							}
						}
					}
				}
				while (list3.Any<Movimentacao>());
				if (list2.Any<Parcial>())
				{
					resultadosParciais.Add(list2);
				}
			});
			var list = (from x in resultadosParciais.SelectMany<List<Parcial>, Parcial>((List<Parcial> r) => r)
				group x by new { x.ProdutoServicoReferencia, x.ProdutoServicoEmpresaReferencia, x.Mes, x.Ano } into g
				select new
				{
					ProdutoServicoReferencia = g.Key.ProdutoServicoReferencia,
					ProdutoServicoEmpresaReferencia = g.Key.ProdutoServicoEmpresaReferencia,
					Mes = g.Key.Mes,
					Ano = g.Key.Ano,
					Quantidade = g.Sum<Parcial>((Parcial x) => x.Quantidade),
					TotalVenda = g.Sum<Parcial>((Parcial x) => x.Total)
				}).ToList();
			IEnumerable<ProdutoServico> @in = this._produtoServicoRepositoryRead.GetIn<ObjectId>((ProdutoServico x) => x.Id, list.Select(x => x.ProdutoServicoReferencia).Distinct<ObjectId>());
			IEnumerable<ProdutoEmpresa> enumerable2 = this._produtoServicoEmpresaRepositoryRead.GetIn<ObjectId>((ProdutoServicoEmpresa x) => x.Id, list.Select(x => x.ProdutoServicoEmpresaReferencia).Distinct<ObjectId>()).OfType<ProdutoEmpresa>();
			IEnumerable<Estoque> in2 = this._estoqueRepositoryRead.GetIn<ObjectId>((Estoque x) => x.Id, enumerable2.Select<ProdutoEmpresa, ObjectId>((ProdutoEmpresa item) => item.EstoqueReferencia).Distinct<ObjectId>());
			Dictionary<ObjectId, ProdutoServico> dictProdutos = @in.ToDictionary<ProdutoServico, ObjectId, ProdutoServico>((ProdutoServico x) => x.Id, (ProdutoServico x) => x);
			Dictionary<ObjectId, ProdutoEmpresa> dictProdutosEmpresa = enumerable2.ToDictionary<ProdutoEmpresa, ObjectId, ProdutoEmpresa>((ProdutoEmpresa x) => x.ProdutoServicoReferencia, (ProdutoEmpresa x) => x);
			Dictionary<ObjectId, Estoque> dictEstoques = in2.ToDictionary<Estoque, ObjectId, Estoque>((Estoque x) => x.Id, (Estoque x) => x);
			return list.Select(delegate(g)
			{
				ProdutoServico valueOrDefault = dictProdutos.GetValueOrDefault(g.ProdutoServicoReferencia);
				ProdutoEmpresa produtoEmpresa = ((valueOrDefault != null) ? dictProdutosEmpresa.GetValueOrDefault(valueOrDefault.Id) : null);
				Estoque estoque = ((produtoEmpresa != null) ? dictEstoques.GetValueOrDefault(produtoEmpresa.EstoqueReferencia) : null);
				return new VendasPorProdutoMensalDto
				{
					CodigoInterno = ((valueOrDefault != null) ? valueOrDefault.CodigoInterno.ToStringEmptyWhenNull() : null),
					Descricao = ((valueOrDefault != null) ? valueOrDefault.Descricao.ToStringEmptyWhenNull() : null),
					MesAno = new DateTime(g.Ano, g.Mes, 1).ToString("Y"),
					Data = new DateTime(g.Ano, g.Mes, 1),
					Quantidade = g.Quantidade,
					TotalVenda = g.TotalVenda,
					EstoqueAtual = ((estoque != null) ? estoque.Quantidade : 0m)
				};
			}).ToList<VendasPorProdutoMensalDto>();
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00081348 File Offset: 0x0007F548
		public IEnumerable<MovimentacaoComDadosDoVeiculoDto> AbastecimentoPorClientePlaca(DateTime dataInicial, DateTime dataFinal, string pessoaReferencia)
		{
			IEnumerable enumerable = this._movimentacaoRepositoryRead.Find(MovimentacaoSpecification.PorOrigem(101).PorSituacaoConcluido().PorPessoa(pessoaReferencia)
				.PorDataEmissao(dataInicial, dataFinal));
			List<MovimentacaoComDadosDoVeiculoDto> list = new List<MovimentacaoComDadosDoVeiculoDto>();
			using (IEnumerator<DocumentoFiscal> enumerator = enumerable.OfType<DocumentoFiscal>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass74_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass74_0();
					CS$<>8__locals1.movimentacao = enumerator.Current;
					string[] dados = CS$<>8__locals1.movimentacao.Observacao.Split(new char[] { '\r' });
					list.AddRange(CS$<>8__locals1.movimentacao.Itens.OfType<IItemSaidaCombustivel>().Select<IItemSaidaCombustivel, MovimentacaoComDadosDoVeiculoDto>(delegate(IItemSaidaCombustivel itemDocumentoFiscalSaidaCombustivel)
					{
						MovimentacaoComDadosDoVeiculoDto movimentacaoComDadosDoVeiculoDto = new MovimentacaoComDadosDoVeiculoDto();
						movimentacaoComDadosDoVeiculoDto.Data = CS$<>8__locals1.movimentacao.DataHoraEmissao;
						movimentacaoComDadosDoVeiculoDto.Numero = CS$<>8__locals1.movimentacao.Numero;
						MovimentacaoComDadosDoVeiculoDto movimentacaoComDadosDoVeiculoDto2 = movimentacaoComDadosDoVeiculoDto;
						string text;
						if (!dados.Any<string>((string x) => x.Contains("Placa")))
						{
							text = string.Empty;
						}
						else
						{
							text = dados.FirstOrDefault<string>((string x) => x.Contains("Placa"));
						}
						movimentacaoComDadosDoVeiculoDto2.Placa = text;
						MovimentacaoComDadosDoVeiculoDto movimentacaoComDadosDoVeiculoDto3 = movimentacaoComDadosDoVeiculoDto;
						string text2;
						if (!dados.Any<string>((string x) => x.Contains("Quilometragem")))
						{
							text2 = string.Empty;
						}
						else
						{
							string text3 = dados.FirstOrDefault<string>((string x) => x.Contains("Quilometragem"));
							text2 = ((text3 != null) ? text3.Split(new char[] { ':' }).LastOrDefault<string>() : null);
						}
						movimentacaoComDadosDoVeiculoDto3.Quilometragem = text2;
						movimentacaoComDadosDoVeiculoDto.DescricaoItem = itemDocumentoFiscalSaidaCombustivel.ProdutoServico.Descricao;
						movimentacaoComDadosDoVeiculoDto.Quantidade = itemDocumentoFiscalSaidaCombustivel.Quantidade;
						movimentacaoComDadosDoVeiculoDto.TotalItem = itemDocumentoFiscalSaidaCombustivel.TotalLiquido;
						return movimentacaoComDadosDoVeiculoDto;
					}));
				}
			}
			return list;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00081424 File Offset: 0x0007F624
		public IEnumerable<ItemMovimentadoDto> ObterItensVendidosPorLinha(DateTime dataInicial, DateTime dataFinal, string linhaReferencia, string vendedorReferencia, bool consolidado, int tipoFiltroData, int codigoOrigem, int situacao, int naturezaOperacao, bool agruparPorPessoa, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<ProdutoServicoEmpresa> resultado = (linhaReferencia.IsNullOrEmpty() ? this._produtoServicoEmpresaRepositoryRead.Find(ProdutoServicoEmpresaSpecifications.PorAtivo()) : this._produtoServicoEmpresaRepositoryRead.Find(ProdutoServicoEmpresaSpecifications.PorAtivo().PorLinha(linhaReferencia)));
			Dictionary<ObjectId, string> linhasPorProdutoServicoEmpresa = resultado.ToDictionary<ProdutoServicoEmpresa, ObjectId, string>((ProdutoServicoEmpresa x) => x.Id, delegate(ProdutoServicoEmpresa x)
			{
				Produto produto = x.ProdutoServico.To<Produto>();
				string text;
				if (produto == null)
				{
					text = null;
				}
				else
				{
					Linha linha = produto.Linha;
					text = ((linha != null) ? linha.Descricao : null);
				}
				return text ?? "Sem linha de produto definida";
			});
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacao(situacao).PorVendedor(vendedorReferencia).PorProdutoServicoEmpresaReferencia(resultado.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa x) => x.Id).ToArray<ObjectId>())
				.PorOrigem(codigoOrigem)
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			if (naturezaOperacao > 0)
			{
				specification = specification.PorNaturezaOperacao(naturezaOperacao);
			}
			if (tipoFiltroData == 1)
			{
				specification = specification.PorDataPrevisao(dataInicial, dataFinal.ToFinalDia());
			}
			else
			{
				specification = specification.PorDataEmissao(dataInicial, dataFinal.ToFinalDia());
			}
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoAuxiliarProduto, bool> <>9__7;
				Func<Movimentacao, IEnumerable<IItemDocumentoAuxiliarProduto>> <>9__6;
				Func<IItemDocumentoAuxiliarProduto, <>f__AnonymousType132<string, string, decimal, decimal, decimal, decimal, EstoqueMovimentacao, decimal, string, ObjectId, string, string>> <>9__9;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					List<ItemMovimentadoDto> list3 = list2;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoAuxiliarProduto>> func;
					if ((func = <>9__6) == null)
					{
						func = (<>9__6 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoAuxiliarProduto> enumerable4 = m.Itens.OfType<IItemDocumentoAuxiliarProduto>();
							Func<IItemDocumentoAuxiliarProduto, bool> func3;
							if ((func3 = <>9__7) == null)
							{
								func3 = (<>9__7 = delegate(IItemDocumentoAuxiliarProduto i)
								{
									if ((vendedorReferencia.IsNullOrEmpty() ? (!i.Cancelado) : i.VendedorReferencia.Equals(vendedorReferencia.TryParseReferencia())) && !i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro)
									{
										return i.ProdutoServicoEmpresaReferencia.In(resultado.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable4.Where<IItemDocumentoAuxiliarProduto>(func3);
						});
					}
					IEnumerable<IItemDocumentoAuxiliarProduto> enumerable3 = enumerable2.SelectMany<Movimentacao, IItemDocumentoAuxiliarProduto>(func);
					var func2;
					if ((func2 = <>9__9) == null)
					{
						func2 = (<>9__9 = (IItemDocumentoAuxiliarProduto item) => new
						{
							CodigoInterno = item.ProdutoServico.CodigoInterno,
							Descricao = item.ProdutoServico.Descricao,
							Quantidade = item.Quantidade,
							PrecoUnitario = item.PrecoUnitario,
							TotalDesconto = item.TotalDesconto,
							Total = item.Total,
							EstoqueMovimentacao = item.EstoqueMovimentacao,
							TotalOutrasDespesas = item.TotalOutrasDespesas,
							Linha = linhasPorProdutoServicoEmpresa[item.ProdutoServicoEmpresaReferencia],
							ProdutoServicoEmpresaReferencia = item.ProdutoServicoEmpresaReferencia,
							Nome = (agruparPorPessoa ? item.Destinatario.Nome : null),
							CpfCnpjFormatado = (agruparPorPessoa ? item.Destinatario.CpfCnpjFormatado : null)
						});
					}
					list3.AddRange((from g in enumerable3.Select(func2)
						group g by new { g.CodigoInterno, g.Descricao, g.Linha, g.ProdutoServicoEmpresaReferencia, g.Nome, g.CpfCnpjFormatado }).Select(delegate(arg)
					{
						ItemMovimentadoDto itemMovimentadoDto = new ItemMovimentadoDto();
						itemMovimentadoDto.CodigoInterno = arg.Key.CodigoInterno;
						itemMovimentadoDto.Descricao = arg.Key.Descricao;
						itemMovimentadoDto.Preco = arg.Average(x => x.PrecoUnitario);
						itemMovimentadoDto.Quantidade = arg.Sum(x => x.Quantidade);
						itemMovimentadoDto.Desconto = arg.Sum(x => x.TotalDesconto);
						itemMovimentadoDto.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
						itemMovimentadoDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoDto.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
						itemMovimentadoDto.Empresa = empresa.Nome;
						itemMovimentadoDto.CnpjEmpresa = empresa.CpfCnpjFormatado;
						itemMovimentadoDto.Linha = arg.Key.Linha.ToStringEmptyWhenNull();
						itemMovimentadoDto.ProdutoServicoEmpresaReferencia = arg.Key.ProdutoServicoEmpresaReferencia.ToStringEmptyWhenDefault();
						itemMovimentadoDto.NomePessoa = (agruparPorPessoa ? arg.Key.Nome.ToStringEmptyWhenNull() : "");
						itemMovimentadoDto.CpfCnpjPessoa = (agruparPorPessoa ? arg.Key.CpfCnpjFormatado.ToStringEmptyWhenNull() : "");
						return itemMovimentadoDto.ProcessarEstoque();
					}));
				}
			}
			this.PreencherSubgrupos(list2);
			return list2;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x000816E4 File Offset: 0x0007F8E4
		public IEnumerable<ItemMovimentadoDto> ObterItensVendidosPorMarca(DateTime dataInicial, DateTime dataFinal, string marcaReferencia, string vendedorReferencia, bool consolidado, int tipoFiltroData, int codigoOrigem, int situacao, int naturezaOperacao, bool agruparPorPessoa, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<ProdutoServicoEmpresa> resultado = (marcaReferencia.IsNullOrEmpty() ? this._produtoServicoEmpresaRepositoryRead.Find(ProdutoServicoEmpresaSpecifications.PorAtivo()) : this._produtoServicoEmpresaRepositoryRead.Find(ProdutoServicoEmpresaSpecifications.PorAtivo().PorMarca(marcaReferencia)));
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacao(situacao).PorVendedor(vendedorReferencia).PorProdutoServicoEmpresaReferencia(resultado.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa x) => x.Id).ToArray<ObjectId>())
				.PorOrigem(codigoOrigem)
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			if (naturezaOperacao > 0)
			{
				specification = specification.PorNaturezaOperacao(naturezaOperacao);
			}
			if (tipoFiltroData == 1)
			{
				specification = specification.PorDataPrevisao(dataInicial, dataFinal.ToFinalDia());
			}
			else
			{
				specification = specification.PorDataEmissao(dataInicial, dataFinal.ToFinalDia());
			}
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoAuxiliarProduto, bool> <>9__5;
				Func<Movimentacao, IEnumerable<IItemDocumentoAuxiliarProduto>> <>9__4;
				Func<IItemDocumentoAuxiliarProduto, <>f__AnonymousType134<string, string, decimal, decimal, decimal, decimal, EstoqueMovimentacao, decimal, string, ObjectId, string, string>> <>9__7;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					List<ItemMovimentadoDto> list3 = list2;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoAuxiliarProduto>> func;
					if ((func = <>9__4) == null)
					{
						func = (<>9__4 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoAuxiliarProduto> enumerable4 = m.Itens.OfType<IItemDocumentoAuxiliarProduto>();
							Func<IItemDocumentoAuxiliarProduto, bool> func3;
							if ((func3 = <>9__5) == null)
							{
								func3 = (<>9__5 = delegate(IItemDocumentoAuxiliarProduto i)
								{
									if ((vendedorReferencia.IsNullOrEmpty() ? (!i.Cancelado) : i.VendedorReferencia.Equals(vendedorReferencia.TryParseReferencia())) && !i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro)
									{
										return i.ProdutoServicoEmpresaReferencia.In(resultado.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa x) => x.Id).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable4.Where<IItemDocumentoAuxiliarProduto>(func3);
						});
					}
					IEnumerable<IItemDocumentoAuxiliarProduto> enumerable3 = enumerable2.SelectMany<Movimentacao, IItemDocumentoAuxiliarProduto>(func);
					var func2;
					if ((func2 = <>9__7) == null)
					{
						func2 = (<>9__7 = delegate(IItemDocumentoAuxiliarProduto item)
						{
							string codigoInterno = item.ProdutoServico.CodigoInterno;
							string descricao = item.ProdutoServico.Descricao;
							decimal quantidade = item.Quantidade;
							decimal precoUnitario = item.PrecoUnitario;
							decimal totalDesconto = item.TotalDesconto;
							decimal total = item.Total;
							EstoqueMovimentacao estoqueMovimentacao = item.EstoqueMovimentacao;
							decimal totalOutrasDespesas = item.TotalOutrasDespesas;
							ProdutoHistorico produtoHistorico = item.ProdutoServico.To<ProdutoHistorico>();
							string text;
							if (produtoHistorico == null)
							{
								text = null;
							}
							else
							{
								Marca marca = produtoHistorico.Marca;
								text = ((marca != null) ? marca.Descricao : null);
							}
							return new
							{
								CodigoInterno = codigoInterno,
								Descricao = descricao,
								Quantidade = quantidade,
								PrecoUnitario = precoUnitario,
								TotalDesconto = totalDesconto,
								Total = total,
								EstoqueMovimentacao = estoqueMovimentacao,
								TotalOutrasDespesas = totalOutrasDespesas,
								Marca = (text ?? "Sem marca definida"),
								ProdutoServicoEmpresaReferencia = item.ProdutoServicoEmpresaReferencia,
								Nome = (agruparPorPessoa ? item.Destinatario.Nome : null),
								CpfCnpjFormatado = (agruparPorPessoa ? item.Destinatario.CpfCnpjFormatado : null)
							};
						});
					}
					list3.AddRange((from g in enumerable3.Select(func2)
						group g by new { g.CodigoInterno, g.Descricao, g.Marca, g.ProdutoServicoEmpresaReferencia, g.Nome, g.CpfCnpjFormatado }).Select(delegate(arg)
					{
						ItemMovimentadoDto itemMovimentadoDto = new ItemMovimentadoDto();
						itemMovimentadoDto.CodigoInterno = arg.Key.CodigoInterno;
						itemMovimentadoDto.Descricao = arg.Key.Descricao;
						itemMovimentadoDto.Preco = arg.Average(x => x.PrecoUnitario);
						itemMovimentadoDto.Quantidade = arg.Sum(x => x.Quantidade);
						itemMovimentadoDto.Desconto = arg.Sum(x => x.TotalDesconto);
						itemMovimentadoDto.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
						itemMovimentadoDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoDto.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
						itemMovimentadoDto.Empresa = empresa.Nome;
						itemMovimentadoDto.CnpjEmpresa = empresa.CpfCnpjFormatado;
						itemMovimentadoDto.Marca = arg.Key.Marca.ToStringEmptyWhenNull();
						itemMovimentadoDto.ProdutoServicoEmpresaReferencia = arg.Key.ProdutoServicoEmpresaReferencia.ToStringEmptyWhenDefault();
						itemMovimentadoDto.NomePessoa = (agruparPorPessoa ? arg.Key.Nome.ToStringEmptyWhenNull() : "");
						itemMovimentadoDto.CpfCnpjPessoa = (agruparPorPessoa ? arg.Key.CpfCnpjFormatado.ToStringEmptyWhenNull() : "");
						return itemMovimentadoDto.ProcessarEstoque();
					}));
				}
			}
			this.PreencherSubgrupos(list2);
			return list2;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00081958 File Offset: 0x0007FB58
		private void PreencherSubgrupos(IEnumerable<ItemMovimentadoDto> result)
		{
			IEnumerable<string> enumerable = result.Select<ItemMovimentadoDto, string>((ItemMovimentadoDto x) => x.ProdutoServicoEmpresaReferencia).Distinct<string>();
			IEnumerable<ProdutoServicoEmpresa> @in = this._produtoServicoEmpresaRepositoryRead.GetIn<ObjectId>((ProdutoServicoEmpresa x) => x.Id, enumerable.Select<string, ObjectId>((string x) => x.TryParseReferencia()));
			IEnumerable<SubGrupo> in2 = this._subGrupoRepositoryRead.GetIn<ObjectId>((SubGrupo x) => x.Id, @in.Select<ProdutoServicoEmpresa, ObjectId>((ProdutoServicoEmpresa p) => p.ProdutoServico.SubGrupoReferencia));
			using (IEnumerator<ItemMovimentadoDto> enumerator = result.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemMovimentadoDto itemMovimentadoDto = enumerator.Current;
					ProdutoServicoEmpresa produtoServicoEmpresa = @in.FirstOrDefault<ProdutoServicoEmpresa>((ProdutoServicoEmpresa x) => x.Id.Equals(itemMovimentadoDto.ProdutoServicoEmpresaReferencia.TryParseReferencia()));
					if (!produtoServicoEmpresa.IsNull())
					{
						ItemMovimentadoDto itemMovimentadoDto3 = itemMovimentadoDto;
						SubGrupo subGrupo = in2.FirstOrDefault<SubGrupo>((SubGrupo x) => x.Id.Equals(produtoServicoEmpresa.ProdutoServico.SubGrupoReferencia));
						itemMovimentadoDto3.Subgrupo = ((subGrupo != null) ? subGrupo.Descricao.ToStringEmptyWhenNull() : null) ?? string.Empty;
						ItemMovimentadoDto itemMovimentadoDto2 = itemMovimentadoDto;
						Produto produto = produtoServicoEmpresa.ProdutoServico.To<Produto>();
						itemMovimentadoDto2.CodigoFabricante = ((produto != null) ? produto.CodigoFabricante.ToStringEmptyWhenNull() : null) ?? string.Empty;
					}
				}
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00081B50 File Offset: 0x0007FD50
		public IEnumerable<ItemMovimentadoDto> ObterItensVendidosPorPromocao(DateTime dataInicial, DateTime dataFinal, string promocaoReferencia, string vendedorReferencia, bool consolidado, int tipoFiltroData, int codigoOrigem, int situacao, IUsuarioIdentity usuarioIdentity)
		{
			Promocao byId = this._promocaoRepositoryRead.GetById(promocaoReferencia.TryParseReferencia());
			IEnumerable<ObjectId> produtos = this._produtoServicoEmpresaRepositoryRead.ObterIdsProdutoServicoEmpresaPorPrecoReferencia(byId.Itens.Select<ItemPromocao, ObjectId>((ItemPromocao x) => x.PrecoReferencia));
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacao(situacao).PorVendedor(vendedorReferencia).PorProdutoServicoEmpresaReferencia(produtos.ToArray<ObjectId>())
				.PorOrigem(codigoOrigem);
			if (tipoFiltroData == 1)
			{
				specification = specification.PorDataHoraPrevisao(dataInicial, dataFinal);
			}
			else
			{
				specification = specification.PorDataHoraEmissao(dataInicial, dataFinal);
			}
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoAuxiliarProduto, bool> <>9__5;
				Func<Movimentacao, IEnumerable<IItemDocumentoAuxiliarProduto>> <>9__4;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					List<ItemMovimentadoDto> list3 = list2;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia));
					Func<Movimentacao, IEnumerable<IItemDocumentoAuxiliarProduto>> func;
					if ((func = <>9__4) == null)
					{
						func = (<>9__4 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoAuxiliarProduto> enumerable3 = m.Itens.OfType<IItemDocumentoAuxiliarProduto>();
							Func<IItemDocumentoAuxiliarProduto, bool> func2;
							if ((func2 = <>9__5) == null)
							{
								func2 = (<>9__5 = (IItemDocumentoAuxiliarProduto i) => !i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro && i.ProdutoServicoEmpresaReferencia.In(produtos.ToArray<ObjectId>()));
							}
							return enumerable3.Where<IItemDocumentoAuxiliarProduto>(func2);
						});
					}
					list3.AddRange((from item in enumerable2.SelectMany<Movimentacao, IItemDocumentoAuxiliarProduto>(func)
						select new
						{
							item.ProdutoServico.CodigoInterno,
							item.ProdutoServico.Descricao,
							item.Quantidade,
							item.PrecoUnitario,
							item.TotalDesconto,
							item.Total,
							item.EstoqueMovimentacao,
							item.TotalOutrasDespesas
						} into g
						group g by new { g.CodigoInterno, g.Descricao }).Select(delegate(arg)
					{
						ItemMovimentadoDto itemMovimentadoDto = new ItemMovimentadoDto();
						itemMovimentadoDto.CodigoInterno = arg.Key.CodigoInterno;
						itemMovimentadoDto.Descricao = arg.Key.Descricao;
						itemMovimentadoDto.Preco = arg.Average(x => x.PrecoUnitario);
						itemMovimentadoDto.Quantidade = arg.Sum(x => x.Quantidade);
						itemMovimentadoDto.Desconto = arg.Sum(x => x.TotalDesconto);
						itemMovimentadoDto.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
						itemMovimentadoDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoDto.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
						itemMovimentadoDto.Empresa = empresa.Nome;
						itemMovimentadoDto.CnpjEmpresa = empresa.CpfCnpjFormatado;
						return itemMovimentadoDto.ProcessarEstoque();
					}));
				}
			}
			return list2;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00081D54 File Offset: 0x0007FF54
		public async Task<ItemAdicionalVendidoCardapioDto> ObterItensVendidosPorCardapio(DateTime dataInicial, DateTime dataFinal, string cardapioReferencia, IUsuarioIdentity usuarioIdentity)
		{
			ISpecification<Cardapio> specification = ((!string.IsNullOrWhiteSpace(cardapioReferencia)) ? EntityEmpresaSpecification<Cardapio>.PorId(cardapioReferencia) : EntityEmpresaSpecification<Cardapio>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa));
			IEnumerable<Cardapio> cardapios = this._cardapioRepositoryRead.Find(specification);
			IEnumerable<ObjectId> produtosEmpresa = cardapios.SelectMany<Cardapio, ObjectId>((Cardapio x) => x.ItensCardapio.Select<ItemCardapio, ObjectId>((ItemCardapio i) => i.ProdutoEmpresaReferencia)).Distinct<ObjectId>();
			IEnumerable<ObjectId> adicionaisCardapio = cardapios.SelectMany<Cardapio, ObjectId>((Cardapio x) => x.ItensCardapio.SelectMany<ItemCardapio, ObjectId>((ItemCardapio y) => y.Adicionais.Select<ItemAdicional, ObjectId>((ItemAdicional g) => g.ProdutoEmpresaReferencia))).Distinct<ObjectId>();
			ISpecification<Movimentacao> specification2 = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorDataHoraEmissao(dataInicial, dataFinal)
				.PorProdutoServicoEmpresaReferencia(produtosEmpresa)
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>()
					.OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>());
			IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindAsync(specification2);
			Func<IItemDocumentoFiscal, bool> <>9__6;
			List<IItemDocumentoFiscal> list = enumerable.SelectMany<Movimentacao, IItemDocumentoFiscal>(delegate(Movimentacao m)
			{
				IEnumerable<IItemDocumentoFiscal> enumerable4 = m.Itens.OfType<IItemDocumentoFiscal>();
				Func<IItemDocumentoFiscal, bool> func;
				if ((func = <>9__6) == null)
				{
					func = (<>9__6 = (IItemDocumentoFiscal i) => !i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro && i.ProdutoServicoEmpresaReferencia.In(produtosEmpresa.ToArray<ObjectId>()));
				}
				return enumerable4.Where<IItemDocumentoFiscal>(func);
			}).ToListEmptyWhenNull<IItemDocumentoFiscal>();
			Func<IItemDocumentoFiscal, bool> <>9__8;
			IEnumerable<AdicionalVendidoCardapioDto> enumerable2 = (from item in enumerable.SelectMany<Movimentacao, IItemDocumentoFiscal>(delegate(Movimentacao m)
				{
					IEnumerable<IItemDocumentoFiscal> enumerable5 = m.Itens.OfType<IItemDocumentoFiscal>();
					Func<IItemDocumentoFiscal, bool> func2;
					if ((func2 = <>9__8) == null)
					{
						func2 = (<>9__8 = (IItemDocumentoFiscal i) => !i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro && i.ProdutoServicoEmpresaReferencia.In(adicionaisCardapio.ToArray<ObjectId>()));
					}
					return enumerable5.Where<IItemDocumentoFiscal>(func2);
				}).ToListEmptyWhenNull<IItemDocumentoFiscal>()
				group item by new { item.ProdutoServicoEmpresaReferencia, item.PrecoUnitario }).Select(delegate(group)
			{
				IItemDocumentoFiscal itemDocumentoFiscal = group.First<IItemDocumentoFiscal>();
				AdicionalVendidoCardapioDto adicionalVendidoCardapioDto = new AdicionalVendidoCardapioDto();
				adicionalVendidoCardapioDto.Descricao = itemDocumentoFiscal.ProdutoServico.Descricao.ToStringEmptyWhenNull();
				adicionalVendidoCardapioDto.CodigoInterno = itemDocumentoFiscal.ProdutoServico.CodigoInterno.ToStringEmptyWhenNull();
				adicionalVendidoCardapioDto.Quantidade = group.Sum<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => x.Quantidade);
				adicionalVendidoCardapioDto.PrecoVenda = group.Key.PrecoUnitario;
				adicionalVendidoCardapioDto.Desconto = group.Sum<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => x.TotalDesconto);
				adicionalVendidoCardapioDto.Total = group.Sum<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => x.Total);
				adicionalVendidoCardapioDto.NumeroItensVendidos = group.Count<IItemDocumentoFiscal>();
				return adicionalVendidoCardapioDto;
			});
			IEnumerable<ItemVendidoCardapioDto> enumerable3 = (from item in list
				group item by new { item.ProdutoServicoEmpresaReferencia, item.PrecoUnitario }).Select(delegate(group)
			{
				IItemDocumentoFiscal itemVendido = group.First<IItemDocumentoFiscal>();
				Func<ItemCardapio, bool> <>9__17;
				Cardapio cardapio = cardapios.FirstOrDefault<Cardapio>(delegate(Cardapio x)
				{
					IEnumerable<ItemCardapio> itensCardapio = x.ItensCardapio;
					Func<ItemCardapio, bool> func3;
					if ((func3 = <>9__17) == null)
					{
						func3 = (<>9__17 = (ItemCardapio i) => i.ProdutoEmpresaReferencia.Equals(itemVendido.ProdutoServicoEmpresaReferencia));
					}
					return itensCardapio.Any<ItemCardapio>(func3);
				});
				ItemVendidoCardapioDto itemVendidoCardapioDto = new ItemVendidoCardapioDto();
				itemVendidoCardapioDto.Cardapio = cardapio.Descricao.ToStringEmptyWhenNull();
				itemVendidoCardapioDto.CardapioReferencia = cardapio.Id.ToStringEmptyWhenDefault();
				itemVendidoCardapioDto.Descricao = itemVendido.ProdutoServico.Descricao.ToStringEmptyWhenNull();
				itemVendidoCardapioDto.CodigoInterno = itemVendido.ProdutoServico.CodigoInterno.ToStringEmptyWhenNull();
				itemVendidoCardapioDto.Quantidade = group.Sum<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => x.Quantidade);
				itemVendidoCardapioDto.PrecoVenda = group.Key.PrecoUnitario;
				itemVendidoCardapioDto.Desconto = group.Sum<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => x.TotalDesconto);
				itemVendidoCardapioDto.Total = group.Sum<IItemDocumentoFiscal>((IItemDocumentoFiscal x) => x.Total);
				itemVendidoCardapioDto.NumeroItensVendidos = group.Count<IItemDocumentoFiscal>();
				itemVendidoCardapioDto.DetalhesMovimentacao = string.Join(Environment.NewLine, from x in @group
					orderby x.TipoMovimentacao, x.NumeroMovimentacao
					select string.Format("{0}    Nº {1:000000000}    {2:G}    Item: {3}", new object[]
					{
						x.TipoMovimentacao.RetornarDescricaoCurta(),
						x.NumeroMovimentacao,
						x.DataHora,
						x.Numero
					}));
				return itemVendidoCardapioDto;
			});
			return new ItemAdicionalVendidoCardapioDto
			{
				ItemVendido = enumerable3,
				Adicionais = enumerable2
			};
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00081DBC File Offset: 0x0007FFBC
		public async Task<IEnumerable<ItemComponenteUtilizadoMontagemDto>> ObterComponentesVendidosPorPeriodo(DateTime dataInicial, DateTime dataFinal, string empresaReferencia)
		{
			IEnumerable<Composicao> composicoes = this._produtoServicoRepositoryRead.ObterComposicoes();
			ObjectId[] idsComposicoes = composicoes.Select<Composicao, ObjectId>((Composicao x) => x.Id).ToArray<ObjectId>();
			ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(empresaReferencia).PorSituacaoConcluido().PorDataEmissao(dataInicial, dataFinal)
				.PorProdutoServicoReferencia(idsComposicoes)
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			Func<IItemDocumentoFiscalProduto, bool> <>9__2;
			List<ItemComponenteUtilizadoMontagemDto> componentesVendidos = (from x in (from x in enumerable.SelectMany<Movimentacao, IItemDocumentoFiscalProduto>(delegate(Movimentacao movimentacao)
					{
						IEnumerable<IItemDocumentoFiscalProduto> enumerable2 = movimentacao.Itens.OfType<IItemDocumentoFiscalProduto>();
						Func<IItemDocumentoFiscalProduto, bool> func;
						if ((func = <>9__2) == null)
						{
							func = (<>9__2 = (IItemDocumentoFiscalProduto item) => !item.NaoEfetivar && !item.Cancelado && idsComposicoes.Contains(item.ProdutoServico.ProdutoServicoReferencia));
						}
						return enumerable2.Where<IItemDocumentoFiscalProduto>(func);
					})
					group x by x.Produto.ProdutoServicoReferencia).SelectMany(delegate(IGrouping<ObjectId, IItemDocumentoFiscalProduto> @group)
				{
					decimal quantidadeComposicao = @group.Sum<IItemDocumentoFiscalProduto>((IItemDocumentoFiscalProduto x) => x.Quantidade);
					return composicoes.FirstOrDefault<Composicao>((Composicao x) => x.Id.Equals(@group.Key)).Componentes.Select((Componente c) => new
					{
						ProdutoReferencia = c.ProdutoReferencia,
						Descricao = c.Produto.Descricao,
						CodigoInterno = c.Produto.CodigoInterno,
						QuantidadeVendida = quantidadeComposicao * c.Quantidade
					});
				})
				group x by x.ProdutoReferencia).Select(delegate(group)
			{
				var anon = group.FirstOrDefault();
				ItemComponenteUtilizadoMontagemDto itemComponenteUtilizadoMontagemDto = new ItemComponenteUtilizadoMontagemDto();
				itemComponenteUtilizadoMontagemDto.CodigoInterno = anon.CodigoInterno;
				itemComponenteUtilizadoMontagemDto.Descricao = anon.Descricao;
				itemComponenteUtilizadoMontagemDto.Quantidade = group.Sum(x => x.QuantidadeVendida);
				return itemComponenteUtilizadoMontagemDto;
			}).ToListEmptyWhenNull<ItemComponenteUtilizadoMontagemDto>();
			return await Task.Factory.StartNew<List<ItemComponenteUtilizadoMontagemDto>>(() => componentesVendidos);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00081E1C File Offset: 0x0008001C
		public async Task<IEnumerable<LucroProdutoVendidoDemonstrativoResultadoDto>> ObterCustoLucroMercadoriaAnual(DateTime dataInicial, DateTime dataFinal, ValorEscriturarInventario valorEscriturarInventario, IUsuarioIdentity usuarioIdentity)
		{
			return await Task.Factory.StartNew<List<LucroProdutoVendidoDemonstrativoResultadoDto>>(delegate
			{
				List<LucroProdutoVendidoDemonstrativoResultadoDto> list = new List<LucroProdutoVendidoDemonstrativoResultadoDto>();
				Dictionary<ObjectId, ProdutoServicoEmpresa> produtosServicos = new Dictionary<ObjectId, ProdutoServicoEmpresa>();
				Dictionary<ObjectId, List<PrecoHistorico>> historicosPrecoCusto = new Dictionary<ObjectId, List<PrecoHistorico>>();
				Func<<>f__AnonymousType138<Movimentacao, IItemDocumentoAuxiliar>, <>f__AnonymousType139<<>f__AnonymousType138<Movimentacao, IItemDocumentoAuxiliar>, ProdutoServicoEmpresa>> <>9__5;
				Func<<>f__AnonymousType139<<>f__AnonymousType141<<>f__AnonymousType140<IItemDocumentoAuxiliar, ProdutoServicoEmpresa>, IItemDocumentoAuxiliar>, ProdutoServicoEmpresa>, <>f__AnonymousType142<<>f__AnonymousType139<<>f__AnonymousType141<<>f__AnonymousType140<IItemDocumentoAuxiliar, ProdutoServicoEmpresa>, IItemDocumentoAuxiliar>, ProdutoServicoEmpresa>, decimal>> <>9__12;
				foreach (Periodo periodo in new GeradorPeriodos().ObterPeriodos(dataInicial, dataFinal, 15))
				{
					ISpecification<Movimentacao> specification = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorData(new Emissao(), periodo.Inicio, periodo.Fim)
						.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
							.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
							.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
							.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
							.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
						.PorNaturezaOperacaoDiferenteDevolucao()
						.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
					foreach (List<Movimentacao> list2 in this._movimentacaoRepositoryRead.FindSemXmlHistoricosEmLotes(specification, 1000))
					{
						this.AdicionarProdutosServicos(produtosServicos, list2.SelectMany<Movimentacao, ObjectId>((Movimentacao x) => x.ItensBase.Select<ItemBase, ObjectId>((ItemBase i) => i.ProdutoServicoEmpresaReferencia)));
						var enumerable = from documentoFiscal in list2
							from item in documentoFiscal.Itens.OfType<IItemDocumentoAuxiliar>()
							select new { documentoFiscal, item };
						var func;
						if ((func = <>9__5) == null)
						{
							func = (<>9__5 = <>h__TransparentIdentifier0 => new
							{
								<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0,
								produtoServico = MovimentacaoSaidaRelatorioAppService.ObterProdutoServico(produtosServicos, <>h__TransparentIdentifier0.item.ProdutoServicoEmpresaReferencia)
							});
						}
						var list3 = (from <>h__TransparentIdentifier1 in enumerable.Select(func)
							where <>h__TransparentIdentifier1.produtoServico != null && !<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.Cancelado && <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.OperacaoFiscal.MovimentaFinanceiro
							select new
							{
								Item = <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item,
								ProdutoServico = <>h__TransparentIdentifier1.produtoServico
							}).ToList();
						if (valorEscriturarInventario == ValorEscriturarInventario.Custo)
						{
							this.AdicionarHistoricosPrecoCusto(historicosPrecoCusto, from x in list3
								where !(x.ProdutoServico is ServicoEmpresa)
								select x.ProdutoServico.Preco.Id);
						}
						List<LucroProdutoVendidoDemonstrativoResultadoDto> list4 = list;
						var enumerable2 = from itemVendido in list3
							let item = itemVendido.Item
							select new
							{
								<>h__TransparentIdentifier0 = <>h__TransparentIdentifier0,
								produtoServico = itemVendido.ProdutoServico
							};
						var func2;
						if ((func2 = <>9__12) == null)
						{
							func2 = (<>9__12 = <>h__TransparentIdentifier1 => new
							{
								<>h__TransparentIdentifier1 = <>h__TransparentIdentifier1,
								precoEntrada = ((<>h__TransparentIdentifier1.produtoServico is ServicoEmpresa) ? <>h__TransparentIdentifier1.produtoServico.ValorCusto : ((valorEscriturarInventario == ValorEscriturarInventario.Custo) ? MovimentacaoSaidaRelatorioAppService.RetornarPrecoCustoAnteriorADataMovimentacao(historicosPrecoCusto, <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.item.DataHora, <>h__TransparentIdentifier1.produtoServico.Preco.Id) : ((valorEscriturarInventario == ValorEscriturarInventario.CustoMedio) ? <>h__TransparentIdentifier1.produtoServico.To<ProdutoEmpresa>().ValorCustoMedio : <>h__TransparentIdentifier1.produtoServico.To<ProdutoEmpresa>().PrecoUltimaCompra)))
							});
						}
						list4.AddRange(from <>h__TransparentIdentifier2 in enumerable2.Select(func2)
							let venda = item.TotalLiquido
							let custo = precoEntrada * item.Quantidade
							let valorLucro = venda - custo
							select new LucroProdutoVendidoDemonstrativoResultadoDto
							{
								DataHora = item.DataHora,
								CustoTotal = custo,
								VendaTotal = venda,
								ValorLucro = valorLucro
							});
					}
				}
				List<LucroProdutoVendidoDemonstrativoResultadoDto> list5 = (from g in list
					group g by new { g.DataHora.Month }).Select(delegate(arg)
				{
					LucroProdutoVendidoDemonstrativoResultadoDto lucroProdutoVendidoDemonstrativoResultadoDto2 = new LucroProdutoVendidoDemonstrativoResultadoDto();
					lucroProdutoVendidoDemonstrativoResultadoDto2.Mes = HelperDateTime.ObterDescricaoMes(arg.Key.Month);
					lucroProdutoVendidoDemonstrativoResultadoDto2.CustoTotal = arg.Sum<LucroProdutoVendidoDemonstrativoResultadoDto>((LucroProdutoVendidoDemonstrativoResultadoDto x) => x.CustoTotal);
					lucroProdutoVendidoDemonstrativoResultadoDto2.VendaTotal = arg.Sum<LucroProdutoVendidoDemonstrativoResultadoDto>((LucroProdutoVendidoDemonstrativoResultadoDto x) => x.VendaTotal);
					lucroProdutoVendidoDemonstrativoResultadoDto2.ValorLucro = arg.Sum<LucroProdutoVendidoDemonstrativoResultadoDto>((LucroProdutoVendidoDemonstrativoResultadoDto x) => x.ValorLucro);
					return lucroProdutoVendidoDemonstrativoResultadoDto2;
				}).ToListEmptyWhenNull<LucroProdutoVendidoDemonstrativoResultadoDto>();
				ISpecification<Movimentacao> specification2 = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorSituacaoConcluido().PorData(new Emissao(), dataInicial, dataFinal)
					.And(MovimentacaoSpecification.PorTipo<NotaFiscalCompra>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Entradas.NotaFiscalEletronicaEntrada>())
					.PorNaturezaOperacao(3);
				IEnumerable<Movimentacao> enumerable3 = this._movimentacaoRepositoryRead.FindSemXmlHistoricos(specification2);
				List<LucroProdutoVendidoDemonstrativoResultadoDto> list6 = new List<LucroProdutoVendidoDemonstrativoResultadoDto>();
				Func<IItemDocumentoAuxiliar, <>f__AnonymousType72<IItemDocumentoAuxiliar, ProdutoServicoEmpresa>> <>9__22;
				foreach (Movimentacao movimentacao in enumerable3)
				{
					List<LucroProdutoVendidoDemonstrativoResultadoDto> list7 = list6;
					IEnumerable<IItemDocumentoAuxiliar> enumerable4 = movimentacao.Itens.OfType<IItemDocumentoAuxiliar>();
					var func3;
					if ((func3 = <>9__22) == null)
					{
						func3 = (<>9__22 = (IItemDocumentoAuxiliar item) => new
						{
							item = item,
							produtoServico = MovimentacaoSaidaRelatorioAppService.ObterProdutoServico(produtosServicos, item.ProdutoServicoEmpresaReferencia)
						});
					}
					list7.AddRange(from <>h__TransparentIdentifier0 in enumerable4.Select(func3)
						where produtoServico != null && !item.Cancelado
						let devolucao = item.TotalLiquido
						select new LucroProdutoVendidoDemonstrativoResultadoDto
						{
							DataHora = item.DataHora,
							Devolucao = devolucao
						});
				}
				List<LucroProdutoVendidoDemonstrativoResultadoDto> list8 = (from g in list6
					group g by new { g.DataHora.Month }).Select(delegate(arg)
				{
					LucroProdutoVendidoDemonstrativoResultadoDto lucroProdutoVendidoDemonstrativoResultadoDto3 = new LucroProdutoVendidoDemonstrativoResultadoDto();
					lucroProdutoVendidoDemonstrativoResultadoDto3.Mes = HelperDateTime.ObterDescricaoMes(arg.Key.Month);
					lucroProdutoVendidoDemonstrativoResultadoDto3.Devolucao = arg.Sum<LucroProdutoVendidoDemonstrativoResultadoDto>((LucroProdutoVendidoDemonstrativoResultadoDto x) => x.Devolucao);
					return lucroProdutoVendidoDemonstrativoResultadoDto3;
				}).ToListEmptyWhenNull<LucroProdutoVendidoDemonstrativoResultadoDto>();
				using (List<LucroProdutoVendidoDemonstrativoResultadoDto>.Enumerator enumerator4 = list5.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						LucroProdutoVendidoDemonstrativoResultadoDto agrupamento = enumerator4.Current;
						LucroProdutoVendidoDemonstrativoResultadoDto agrupamento2 = agrupamento;
						LucroProdutoVendidoDemonstrativoResultadoDto lucroProdutoVendidoDemonstrativoResultadoDto = list8.FirstOrDefault<LucroProdutoVendidoDemonstrativoResultadoDto>((LucroProdutoVendidoDemonstrativoResultadoDto x) => x.Mes.Equals(agrupamento.Mes));
						agrupamento2.Devolucao = ((lucroProdutoVendidoDemonstrativoResultadoDto != null) ? lucroProdutoVendidoDemonstrativoResultadoDto.Devolucao : 0m);
					}
				}
				return list5;
			});
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00081E84 File Offset: 0x00080084
		private void AdicionarProdutosServicos(IDictionary<ObjectId, ProdutoServicoEmpresa> produtosServicos, IEnumerable<ObjectId> produtosServicosReferencia)
		{
			List<ObjectId> list = (from x in produtosServicosReferencia.Distinct<ObjectId>()
				where !produtosServicos.ContainsKey(x)
				select x).ToList<ObjectId>();
			if (!list.Any<ObjectId>())
			{
				return;
			}
			foreach (ProdutoServicoEmpresa produtoServicoEmpresa in this._produtoServicoEmpresaRepositoryRead.GetIn<ObjectId>((ProdutoServicoEmpresa x) => x.Id, list))
			{
				if (!produtosServicos.ContainsKey(produtoServicoEmpresa.Id))
				{
					produtosServicos.Add(produtoServicoEmpresa.Id, produtoServicoEmpresa);
				}
			}
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00081F68 File Offset: 0x00080168
		private static IDictionary<ObjectId, ProdutoServicoEmpresa> PorId(IEnumerable<ProdutoServicoEmpresa> produtosServicos)
		{
			Dictionary<ObjectId, ProdutoServicoEmpresa> dictionary = new Dictionary<ObjectId, ProdutoServicoEmpresa>();
			foreach (ProdutoServicoEmpresa produtoServicoEmpresa in produtosServicos)
			{
				if (!dictionary.ContainsKey(produtoServicoEmpresa.Id))
				{
					dictionary.Add(produtoServicoEmpresa.Id, produtoServicoEmpresa);
				}
			}
			return dictionary;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00081FCC File Offset: 0x000801CC
		private IDictionary<ObjectId, List<PrecoHistorico>> ObterHistoricosPrecoCusto(IEnumerable<ProdutoServicoEmpresa> produtosServicos)
		{
			return this._precoRepositoryRead.ObterPrecosHistorico<ItemPrecoCusto>(from x in produtosServicos
				where !(x is ServicoEmpresa) && x.Preco != null
				select x.Preco.Id);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00082030 File Offset: 0x00080230
		private static ProdutoServicoEmpresa ObterProdutoServico(IDictionary<ObjectId, ProdutoServicoEmpresa> produtosServicos, ObjectId produtoServicoReferencia)
		{
			ProdutoServicoEmpresa produtoServicoEmpresa;
			if (!produtosServicos.TryGetValue(produtoServicoReferencia, out produtoServicoEmpresa))
			{
				return null;
			}
			return produtoServicoEmpresa;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0008204C File Offset: 0x0008024C
		private void AdicionarHistoricosPrecoCusto(IDictionary<ObjectId, List<PrecoHistorico>> historicosPrecoCusto, IEnumerable<ObjectId> precosReferencia)
		{
			List<ObjectId> list = (from x in precosReferencia.Distinct<ObjectId>()
				where !historicosPrecoCusto.ContainsKey(x)
				select x).ToList<ObjectId>();
			if (!list.Any<ObjectId>())
			{
				return;
			}
			foreach (KeyValuePair<ObjectId, List<PrecoHistorico>> keyValuePair in this._precoRepositoryRead.ObterPrecosHistorico<ItemPrecoCusto>(list))
			{
				historicosPrecoCusto.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x000820E4 File Offset: 0x000802E4
		private static decimal RetornarPrecoCustoAnteriorADataMovimentacao(IDictionary<ObjectId, List<PrecoHistorico>> historicosPrecoCusto, DateTime dataHora, ObjectId precoReferencia)
		{
			List<PrecoHistorico> list;
			historicosPrecoCusto.TryGetValue(precoReferencia, out list);
			return (from x in (from x in list.ToListEmptyWhenNull<PrecoHistorico>()
					where x.Data <= dataHora
					select x).ToListEmptyWhenNull<PrecoHistorico>()
				orderby x.Data descending
				select x).FirstOrDefault<PrecoHistorico>().Preco;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00082158 File Offset: 0x00080358
		public async Task<IEnumerable<DadosRelatorioVendasPorTipoSegmentacaoDto>> ObterVendasPorTipoSegmentacao(DateTime dataInicial, DateTime dataFinal, int origem, string tipoSegmentacaoReferencia, string idEmpresa)
		{
			MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass88_0 CS$<>8__locals1 = new MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass88_0();
			Specification<Pessoa> specification = PessoaSpecifications.AtivoTipoCadastroETipoSegmentacao(true, tipoSegmentacaoReferencia, TipoPessoaCadastro.Cliente);
			CS$<>8__locals1.pessoas = this._pessoaRepositoryRead.Find(specification);
			ISpecification<Movimentacao> specification2 = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(idEmpresa).PorDataEmissao(dataInicial, dataFinal).PorOrigem(origem)
				.PorSituacao(2)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = await this._movimentacaoRepositoryRead.FindAsync(specification2);
			List<DadosRelatorioVendasPorTipoSegmentacaoDto> list = new List<DadosRelatorioVendasPorTipoSegmentacaoDto>();
			Func<Movimentacao, bool> func;
			if ((func = CS$<>8__locals1.<>9__0) == null)
			{
				MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass88_0 CS$<>8__locals2 = CS$<>8__locals1;
				Func<Movimentacao, bool> func2 = (Movimentacao x) => CS$<>8__locals1.pessoas.Select<Pessoa, ObjectId>((Pessoa y) => y.Id).Contains(x.Pessoa.PessoaReferencia);
				CS$<>8__locals2.<>9__0 = func2;
				func = func2;
			}
			foreach (Movimentacao movimentacao in enumerable.Where<Movimentacao>(func))
			{
				List<DadosRelatorioVendasPorTipoSegmentacaoDto> list2 = list;
				DadosRelatorioVendasPorTipoSegmentacaoDto dadosRelatorioVendasPorTipoSegmentacaoDto = new DadosRelatorioVendasPorTipoSegmentacaoDto();
				dadosRelatorioVendasPorTipoSegmentacaoDto.Origem = movimentacao.Tipo.RetornarDescricaoCurta();
				dadosRelatorioVendasPorTipoSegmentacaoDto.Numero = movimentacao.Numero;
				DocumentoFiscal documentoFiscal = movimentacao.To<DocumentoFiscal>();
				dadosRelatorioVendasPorTipoSegmentacaoDto.Serie = ((documentoFiscal != null) ? documentoFiscal.Serie.ToStringEmptyWhenNull() : null) ?? string.Empty;
				dadosRelatorioVendasPorTipoSegmentacaoDto.DataEmissao = movimentacao.DataHoraEmissao;
				DocumentoFiscalEletronico documentoFiscalEletronico = movimentacao.To<DocumentoFiscalEletronico>();
				dadosRelatorioVendasPorTipoSegmentacaoDto.ChaveAcesso = ((documentoFiscalEletronico != null) ? documentoFiscalEletronico.ChaveAcesso.ToStringEmptyWhenNull() : null) ?? string.Empty;
				dadosRelatorioVendasPorTipoSegmentacaoDto.PessoaNome = movimentacao.Pessoa.Nome.ToStringEmptyWhenNull();
				dadosRelatorioVendasPorTipoSegmentacaoDto.PessoaDocumento = movimentacao.Pessoa.CpfCnpjFormatado.ToStringEmptyWhenNull();
				dadosRelatorioVendasPorTipoSegmentacaoDto.TotalMovimentacao = movimentacao.Total;
				list2.Add(dadosRelatorioVendasPorTipoSegmentacaoDto);
			}
			return list;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x000821C8 File Offset: 0x000803C8
		public IEnumerable<MovimentacaoDescontoCashbackDto> MovimentacoesComDescontoCashback(DateTime dataInicial, DateTime dataFinal, string pessoaReferencia, string vendedorReferencia, IUsuarioIdentity usuarioIdentity)
		{
			IEnumerable<ItemDebitoCashback> enumerable = (pessoaReferencia.IsNullOrEmpty() ? this._itemDebitoCreditoCashbackRepositoryRead.GetByFields(new Expression<Func<ItemCreditoDebitoCashback, bool>>[]
			{
				(ItemCreditoDebitoCashback x) => x.PessoaReferencia != ObjectId.Parse("59257eeac1546a18c0360ef3") && x.Id >= ObjectId.GenerateNewId(dataInicial.Date) && x.Id <= ObjectId.GenerateNewId(dataFinal.ToFinalDia())
			}).OfType<ItemDebitoCashback>() : this._itemDebitoCreditoCashbackRepositoryRead.GetByFields(new Expression<Func<ItemCreditoDebitoCashback, bool>>[]
			{
				(ItemCreditoDebitoCashback x) => x.PessoaReferencia.Equals(pessoaReferencia.TryParseReferencia()) && x.Id >= ObjectId.GenerateNewId(dataInicial.Date) && x.Id <= ObjectId.GenerateNewId(dataFinal.ToFinalDia())
			}).OfType<ItemDebitoCashback>());
			IEnumerable<Movimentacao> enumerable2 = from x in this._movimentacaoRepositoryRead.GetIn<ObjectId>((Movimentacao x) => x.Id, enumerable.Select<ItemDebitoCashback, ObjectId>((ItemDebitoCashback x) => x.MovimentacaoReferencia))
				where x.Situacao.Codigo.Equals(2)
				select x;
			Func<ItemBase, bool> <>9__3;
			IEnumerable<Movimentacao> enumerable3 = (vendedorReferencia.IsNullOrEmpty() ? enumerable2 : enumerable2.Where<Movimentacao>(delegate(Movimentacao x)
			{
				IEnumerable<ItemBase> itensBase = x.ItensBase;
				Func<ItemBase, bool> func;
				if ((func = <>9__3) == null)
				{
					func = (<>9__3 = (ItemBase a) => a.VendedorReferencia.Equals(vendedorReferencia.TryParseReferencia()));
				}
				return itensBase.Any<ItemBase>(func);
			}));
			List<MovimentacaoDescontoCashbackDto> list = new List<MovimentacaoDescontoCashbackDto>();
			using (IEnumerator<Movimentacao> enumerator = enumerable3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Movimentacao movimentacao = enumerator.Current;
					List<MovimentacaoDescontoCashbackDto> list2 = list;
					MovimentacaoDescontoCashbackDto movimentacaoDescontoCashbackDto = new MovimentacaoDescontoCashbackDto();
					movimentacaoDescontoCashbackDto.Numero = movimentacao.Numero;
					DocumentoFiscal documentoFiscal = movimentacao.To<DocumentoFiscal>();
					movimentacaoDescontoCashbackDto.Serie = ((documentoFiscal != null) ? documentoFiscal.Serie.ToStringEmptyWhenNull() : null) ?? string.Empty;
					movimentacaoDescontoCashbackDto.DataHoraEmissao = movimentacao.DataHoraEmissao;
					movimentacaoDescontoCashbackDto.PessoaNome = movimentacao.Pessoa.Nome.ToStringEmptyWhenNull();
					movimentacaoDescontoCashbackDto.CpfCnpjPessoa = movimentacao.Pessoa.CpfCnpjFormatado.ToStringEmptyWhenNull();
					movimentacaoDescontoCashbackDto.TipoMovimentacao = movimentacao.Tipo.RetornarDescricao();
					DocumentoFiscal documentoFiscal2 = movimentacao.To<DocumentoFiscal>();
					movimentacaoDescontoCashbackDto.Modelo = ((documentoFiscal2 != null) ? documentoFiscal2.Modelo.Modelo.ToStringEmptyWhenNull() : null) ?? string.Empty;
					movimentacaoDescontoCashbackDto.TotalCashback = enumerable.FirstOrDefault<ItemDebitoCashback>((ItemDebitoCashback x) => x.MovimentacaoReferencia.Equals(movimentacao.Id)).Valor;
					movimentacaoDescontoCashbackDto.Vendedor = string.Join(", ", movimentacao.Itens.Select<IItem, string>(delegate(IItem x)
					{
						IItemSaidaProduto itemSaidaProduto = x.To<IItemSaidaProduto>();
						if (itemSaidaProduto == null)
						{
							return null;
						}
						PessoaHistorico vendedor = itemSaidaProduto.Vendedor;
						if (vendedor == null)
						{
							return null;
						}
						return vendedor.Nome.ToStringEmptyWhenNull();
					}).Distinct<string>());
					list2.Add(movimentacaoDescontoCashbackDto);
				}
			}
			return list;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x000827C8 File Offset: 0x000809C8
		public IEnumerable<ItemMovimentadoDto> ProdutosVendidosPorFornecedor(DateTime dataInicial, DateTime dataFinal, string fornecedorReferencia, bool consolidado, IUsuarioIdentity usuarioIdentity)
		{
			List<ItemRelacaoProdutoServicoEmpresaFornecedor> itens = this._itemRelacaoProdutoServicoEmpresaFornecedorRead.Find(EntityBaseSpecification<ItemRelacaoProdutoServicoEmpresaFornecedor>.All().PorFornecedor(fornecedorReferencia)).ToListEmptyWhenNull<ItemRelacaoProdutoServicoEmpresaFornecedor>();
			ISpecification<Movimentacao> specification = ((!consolidado) ? EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa) : EntitySpecification<Movimentacao>.Ativo()).PorSituacaoConcluido().PorDataHoraEmissao(dataInicial, dataFinal).PorProdutoServicoEmpresaReferencia(itens.Select<ItemRelacaoProdutoServicoEmpresaFornecedor, ObjectId>((ItemRelacaoProdutoServicoEmpresaFornecedor x) => x.ProdutoServicoEmpresaReferencia).ToArray<ObjectId>())
				.And(MovimentacaoSpecification.PorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscal>().OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.CupomFiscalEletronico>().OrPorTipo<NotaFiscalManual>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalEletronicaSaida>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalServico>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistroVenda>()
					.OrPorTipo<DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.Saidas.NotaFiscalConsumidorEletronica>())
				.PorNaturezaOperacaoDiferenteDevolucao()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			IEnumerable<Movimentacao> enumerable = this._movimentacaoRepositoryRead.Find(specification);
			var list = (from movimentacao in enumerable
				group movimentacao by new { movimentacao.Empresa.PessoaReferencia } into x
				select new
				{
					x.Last<Movimentacao>().Empresa.CpfCnpjFormatado,
					x.Last<Movimentacao>().Empresa.Nome,
					x.Key.PessoaReferencia
				}).ToListEmptyWhenNull();
			List<ItemMovimentadoDto> list2 = new List<ItemMovimentadoDto>();
			using (var enumerator = list.GetEnumerator())
			{
				Func<IItemDocumentoFiscal, bool> <>9__5;
				Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> <>9__4;
				Func<IItemDocumentoFiscal, <>f__AnonymousType148<ObjectId, string, string, string, string, decimal, decimal, decimal, decimal, EstoqueMovimentacao, decimal>> <>9__7;
				while (enumerator.MoveNext())
				{
					<>f__AnonymousType2<string, string, ObjectId> empresa = enumerator.Current;
					IEnumerable<Movimentacao> enumerable2 = enumerable.Where<Movimentacao>((Movimentacao m) => m.EmpresaReferencia.Equals(empresa.PessoaReferencia)).ToListEmptyWhenNull<Movimentacao>();
					Func<Movimentacao, IEnumerable<IItemDocumentoFiscal>> func;
					if ((func = <>9__4) == null)
					{
						func = (<>9__4 = delegate(Movimentacao m)
						{
							IEnumerable<IItemDocumentoFiscal> enumerable4 = m.Itens.OfType<IItemDocumentoFiscal>();
							Func<IItemDocumentoFiscal, bool> func3;
							if ((func3 = <>9__5) == null)
							{
								func3 = (<>9__5 = delegate(IItemDocumentoFiscal i)
								{
									if (!i.Cancelado && i.OperacaoFiscal.MovimentaEstoque && i.OperacaoFiscal.MovimentaFinanceiro)
									{
										return i.ProdutoServicoEmpresaReferencia.In(itens.Select<ItemRelacaoProdutoServicoEmpresaFornecedor, ObjectId>((ItemRelacaoProdutoServicoEmpresaFornecedor x) => x.ProdutoServicoEmpresaReferencia).ToArray<ObjectId>());
									}
									return false;
								});
							}
							return enumerable4.Where<IItemDocumentoFiscal>(func3);
						});
					}
					List<IItemDocumentoFiscal> list3 = enumerable2.SelectMany<Movimentacao, IItemDocumentoFiscal>(func).ToListEmptyWhenNull<IItemDocumentoFiscal>();
					List<ItemMovimentadoDto> list4 = list2;
					IEnumerable<IItemDocumentoFiscal> enumerable3 = list3;
					var func2;
					if ((func2 = <>9__7) == null)
					{
						func2 = (<>9__7 = delegate(IItemDocumentoFiscal item)
						{
							ObjectId produtoServicoEmpresaReferencia = item.ProdutoServicoEmpresaReferencia;
							string codigoInterno = item.ProdutoServico.CodigoInterno;
							IItemDocumentoFiscalProduto itemDocumentoFiscalProduto = item.To<IItemDocumentoFiscalProduto>();
							string text;
							if (itemDocumentoFiscalProduto == null)
							{
								text = null;
							}
							else
							{
								ProdutoHistorico produto = itemDocumentoFiscalProduto.Produto;
								text = ((produto != null) ? produto.CodigoBarras : null);
							}
							ItemRelacaoProdutoServicoEmpresaFornecedor itemRelacaoProdutoServicoEmpresaFornecedor = itens.FirstOrDefault<ItemRelacaoProdutoServicoEmpresaFornecedor>((ItemRelacaoProdutoServicoEmpresaFornecedor x) => x.ProdutoServicoEmpresaReferencia.Equals(item.ProdutoServicoEmpresaReferencia));
							string text2 = ((itemRelacaoProdutoServicoEmpresaFornecedor != null) ? itemRelacaoProdutoServicoEmpresaFornecedor.CodigoFornecedor : null);
							string descricao = item.ProdutoServico.Descricao;
							decimal quantidade = item.Quantidade;
							decimal precoUnitario = item.PrecoUnitario;
							decimal totalDesconto = item.TotalDesconto;
							decimal total = item.Total;
							IItemDocumentoFiscalProduto itemDocumentoFiscalProduto2 = item.To<IItemDocumentoFiscalProduto>();
							return new
							{
								ProdutoServicoEmpresaReferencia = produtoServicoEmpresaReferencia,
								CodigoInterno = codigoInterno,
								CodigoBarras = text,
								CodigoFornecedor = text2,
								Descricao = descricao,
								Quantidade = quantidade,
								PrecoUnitario = precoUnitario,
								TotalDesconto = totalDesconto,
								Total = total,
								EstoqueMovimentacao = ((itemDocumentoFiscalProduto2 != null) ? itemDocumentoFiscalProduto2.EstoqueMovimentacao : null),
								TotalOutrasDespesas = item.TotalOutrasDespesas
							};
						});
					}
					list4.AddRange((from g in enumerable3.Select(func2)
						group g by new { g.ProdutoServicoEmpresaReferencia, g.PrecoUnitario }).Select(delegate(arg)
					{
						ItemMovimentadoDto itemMovimentadoDto = new ItemMovimentadoDto();
						var anon = arg.FirstOrDefault();
						itemMovimentadoDto.CodigoInterno = ((anon != null) ? anon.CodigoInterno.ToStringEmptyWhenNull() : null);
						var anon2 = arg.FirstOrDefault();
						itemMovimentadoDto.CodigoBarras = ((anon2 != null) ? anon2.CodigoBarras.ToStringEmptyWhenNull() : null);
						var anon3 = arg.FirstOrDefault();
						itemMovimentadoDto.CodigoFornecedor = ((anon3 != null) ? anon3.CodigoFornecedor.ToStringEmptyWhenNull() : null);
						var anon4 = arg.FirstOrDefault();
						itemMovimentadoDto.Descricao = ((anon4 != null) ? anon4.Descricao.ToStringEmptyWhenNull() : null);
						itemMovimentadoDto.Preco = arg.Key.PrecoUnitario;
						itemMovimentadoDto.Quantidade = arg.Sum(x => x.Quantidade);
						itemMovimentadoDto.Desconto = arg.Sum(x => x.TotalDesconto);
						itemMovimentadoDto.OutrasDespesas = arg.Sum(x => x.TotalOutrasDespesas);
						itemMovimentadoDto.Total = arg.Sum(x => x.Total);
						itemMovimentadoDto.Estoque = arg.Select(i => i.EstoqueMovimentacao.ProjectedAsDto()).ToList<EstoqueDto>();
						itemMovimentadoDto.Empresa = empresa.Nome;
						itemMovimentadoDto.CnpjEmpresa = empresa.CpfCnpjFormatado;
						itemMovimentadoDto.QuantidadeMovimentacoes = arg.Count();
						return itemMovimentadoDto.ProcessarEstoque().AgruparEstoque();
					}));
				}
			}
			return list2;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x000829E0 File Offset: 0x00080BE0
		public async Task<IEnumerable<DadosRelatorioTotalMovimentacoesPorMesPorTipoDto>> ObterTotalMovimentacoesPorMesPorTipo(int ano, IUsuarioIdentity usuarioIdentity)
		{
			DateTime dateTime = new DateTime(ano, 1, 1);
			DateTime dateTime2 = new DateTime(ano, 12, 31);
			ISpecification<Movimentacao> neSpec = EntityEmpresaSpecification<Movimentacao>.AtivoPorEmpresa(usuarioIdentity.IdEmpresa).PorDataEmissao(dateTime, dateTime2).PorSituacaoConcluido()
				.PorOrigem(101)
				.PorFinalidadeEmissaoNormal()
				.PorNotaFiscalConsumidorEletronicaDiferenteMovimentacaoItemFinanceiro();
			ConcurrentDictionary<string, decimal[]> acumuladoGlobal = new ConcurrentDictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase);
			int skip = 0;
			List<Movimentacao> list;
			do
			{
				list = (await this._movimentacaoRepositoryRead.FindPagedAsync(neSpec.SatisfiedBy(), skip, 500)).ToList<Movimentacao>();
				skip += 500;
				if (!list.Any<Movimentacao>())
				{
					break;
				}
				IEnumerable<Movimentacao> enumerable = list;
				ParallelOptions parallelOptions = new ParallelOptions();
				parallelOptions.MaxDegreeOfParallelism = Math.Max(2, Environment.ProcessorCount / 2);
				Parallel.ForEach<Movimentacao, Dictionary<string, decimal[]>>(enumerable, parallelOptions, () => new Dictionary<string, decimal[]>(StringComparer.OrdinalIgnoreCase), delegate(Movimentacao movimentacao, ParallelLoopState loopState, Dictionary<string, decimal[]> localDict)
				{
					foreach (IItem item2 in movimentacao.Itens.Where<IItem>((IItem item) => !item.Cancelado))
					{
						string text = movimentacao.Tipo.RetornarDescricaoCurta();
						int month = movimentacao.DataHoraEmissao.Month;
						decimal[] array;
						if (!localDict.TryGetValue(text, out array))
						{
							array = new decimal[12];
							localDict[text] = array;
						}
						array[month - 1] += item2.TotalLiquido;
					}
					return localDict;
				}, delegate(Dictionary<string, decimal[]> localDict)
				{
					using (Dictionary<string, decimal[]>.Enumerator enumerator2 = localDict.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<string, decimal[]> kvp2 = enumerator2.Current;
							acumuladoGlobal.AddOrUpdate(kvp2.Key, kvp2.Value, delegate(string key, decimal[] existing)
							{
								for (int i = 0; i < 12; i++)
								{
									existing[i] += kvp2.Value[i];
								}
								return existing;
							});
						}
					}
				});
			}
			while (list.Any<Movimentacao>());
			IEnumerable<DadosRelatorioTotalMovimentacoesPorMesPorTipoDto> enumerable2;
			if (!acumuladoGlobal.Any<KeyValuePair<string, decimal[]>>())
			{
				enumerable2 = Enumerable.Empty<DadosRelatorioTotalMovimentacoesPorMesPorTipoDto>();
			}
			else
			{
				enumerable2 = acumuladoGlobal.Select<KeyValuePair<string, decimal[]>, DadosRelatorioTotalMovimentacoesPorMesPorTipoDto>(delegate(KeyValuePair<string, decimal[]> kvp)
				{
					string key = kvp.Key;
					decimal[] value = kvp.Value;
					return new DadosRelatorioTotalMovimentacoesPorMesPorTipoDto
					{
						Tipo = key,
						Janeiro = value[0],
						Fevereiro = value[1],
						Marco = value[2],
						Abril = value[3],
						Maio = value[4],
						Junho = value[5],
						Julho = value[6],
						Agosto = value[7],
						Setembro = value[8],
						Outubro = value[9],
						Novembro = value[10],
						Dezembro = value[11]
					};
				});
			}
			return enumerable2;
		}

		// Token: 0x04000844 RID: 2116
		private readonly IMovimentacaoRepositoryRead _movimentacaoRepositoryRead;

		// Token: 0x04000845 RID: 2117
		private readonly IProdutoServicoEmpresaRepositoryRead _produtoServicoEmpresaRepositoryRead;

		// Token: 0x04000846 RID: 2118
		private readonly IProdutoServicoRepositoryRead _produtoServicoRepositoryRead;

		// Token: 0x04000847 RID: 2119
		private readonly IUsuarioRepositoryRead _usuarioRepositoryRead;

		// Token: 0x04000848 RID: 2120
		private readonly IRepositoryRead<RegistroAbastecimento> _abastecimentoRepositoryRead;

		// Token: 0x04000849 RID: 2121
		private readonly IPessoaRepositoryRead _pessoaRepositoryRead;

		// Token: 0x0400084A RID: 2122
		private readonly ICardapioRepositoryRead _cardapioRepositoryRead;

		// Token: 0x0400084B RID: 2123
		private readonly IRepositoryRead<Receita> _receitaRepositoryRead;

		// Token: 0x0400084C RID: 2124
		private readonly IRepositoryRead<Estoque> _estoqueRepositoryRead;

		// Token: 0x0400084D RID: 2125
		private readonly IPrecoRepositoryRead _precoRepositoryRead;

		// Token: 0x0400084E RID: 2126
		private readonly IRepositoryRead<ItemCreditoDebitoCashback> _itemDebitoCreditoCashbackRepositoryRead;

		// Token: 0x0400084F RID: 2127
		private readonly IRepositoryRead<ItemRelacaoProdutoServicoEmpresaFornecedor> _itemRelacaoProdutoServicoEmpresaFornecedorRead;

		// Token: 0x04000850 RID: 2128
		private readonly IRepositoryRead<Promocao> _promocaoRepositoryRead;

		// Token: 0x04000851 RID: 2129
		private readonly IRepositoryRead<SubGrupo> _subGrupoRepositoryRead;

		// Token: 0x04000852 RID: 2130
		private readonly IRepositoryRead<Marca> _marcaRepositoryRead;

		// Token: 0x04000853 RID: 2131
		private readonly IRepositoryRead<Grupo> _grupoRepositoryRead;

		// Token: 0x04000854 RID: 2132
		private const string VendedorNaoInformado = "Não informado";

		// Token: 0x02000A74 RID: 2676
		private class RateioVendedorEspecie
		{
			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x06003038 RID: 12344 RVA: 0x0013CE7D File Offset: 0x0013B07D
			// (set) Token: 0x06003039 RID: 12345 RVA: 0x0013CE85 File Offset: 0x0013B085
			public string Vendedor { get; set; }

			// Token: 0x17000424 RID: 1060
			// (get) Token: 0x0600303A RID: 12346 RVA: 0x0013CE8E File Offset: 0x0013B08E
			// (set) Token: 0x0600303B RID: 12347 RVA: 0x0013CE96 File Offset: 0x0013B096
			public decimal Fator { get; set; }
		}
	}
}
