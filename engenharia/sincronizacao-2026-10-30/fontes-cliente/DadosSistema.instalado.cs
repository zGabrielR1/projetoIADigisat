using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DigisatServer.Application.Core.Projections.AuxiliaresProjection;
using DigisatServer.Application.Core.Projections.FinanceirosProjection;
using DigisatServer.Application.Main.Dto.Auxiliares.Configuracoes;
using DigisatServer.Application.Main.Dto.Cadastros.Moedas;
using DigisatServer.Application.Main.Dto.Financeiros.CentrosCusto;
using DigisatServer.Application.Main.Dto.Financeiros.PlanosConta;
using DigisatServer.Distributed.Wcf.Main.Contracts.AuxiliaresModulo;
using DigisatServer.Distributed.Wcf.Main.Contracts.CadastrosModulo;
using DigisatServer.Distributed.Wcf.Main.Contracts.ServidoresModulo;
using DigisatServer.Domain.Core;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Configuracoes;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Configuracoes.Financeiros;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta;
using DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.CentrosCusto;
using DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.Contas;
using DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PlanosConta;
using DigisatServer.Infrastructure.Crosscutting.Config;
using DigisatServer.Infrastructure.Crosscutting.Constant;
using DigisatServer.Infrastructure.Crosscutting.Enum;
using DigisatServer.Infrastructure.Crosscutting.Enum.ProgramasAplicativosFiscais;
using DigisatServer.Infrastructure.Crosscutting.Event;
using DigisatServer.Infrastructure.Crosscutting.Function;
using DigisatServer.Infrastructure.Crosscutting.Habilitacao;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Model;
using DigisatServer.Infrastructure.Crosscutting.Model.RamosAtividade;
using DigisatServer.Infrastructure.Crosscutting.Model.Sistemas;
using DigisatServer.Infrastructure.Crosscutting.Module;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.Right;
using DigisatServer.Infrastructure.Crosscutting.Service.ApiScanntech;
using DigisatServer.Infrastructure.Crosscutting.Util;
using DigisatServer.Infrastructure.Crosscutting.ValueObject.CadastrosVO.UFs;
using DigisatServer.Infrastructure.Crosscutting.Wrapper;
using DigisatServer.Infrastructure.Data.Core.Interfaces;
using Microsoft.Practices.Unity;
using Otus.Infraestrutura.Events;
using Otus.Infraestrutura.Interfaces;
using Otus.Infraestrutura.Models.Sessoes;
using Prism.Modularity;

namespace Otus.Infraestrutura.Models
{
	// Token: 0x02000392 RID: 914
	public static class DadosSistema
	{
		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001951 RID: 6481 RVA: 0x0017CB7F File Offset: 0x0017AD7F
		public static bool Bloqueado
		{
			get
			{
				return DadosSistema._bloqueado;
			}
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0017CB86 File Offset: 0x0017AD86
		public static void SetarBloqueado(bool bloqueado)
		{
			DadosSistema._bloqueado = bloqueado;
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001953 RID: 6483 RVA: 0x0017CB8E File Offset: 0x0017AD8E
		public static Otus.Infraestrutura.Models.Sessoes.Pessoa EmpresaLogada
		{
			get
			{
				ISessao sessao = DadosSistema._sessao;
				if (sessao == null)
				{
					return null;
				}
				Lista<PerfilEmpresaSessao> perfis = sessao.Perfis;
				if (perfis == null)
				{
					return null;
				}
				PerfilEmpresaSessao itemSelecionado = perfis.ItemSelecionado;
				if (itemSelecionado == null)
				{
					return null;
				}
				return itemSelecionado.Empresa;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x0017CBB6 File Offset: 0x0017ADB6
		public static IEnumerable<PerfilEmpresaSessao> Empresas
		{
			get
			{
				ISessao sessao = DadosSistema._sessao;
				if (sessao == null)
				{
					return null;
				}
				Lista<PerfilEmpresaSessao> perfis = sessao.Perfis;
				if (perfis == null)
				{
					return null;
				}
				return perfis.Itens;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001955 RID: 6485 RVA: 0x0017CBD3 File Offset: 0x0017ADD3
		public static string NomeUsuario
		{
			get
			{
				ISessao sessao = DadosSistema._sessao;
				if (sessao == null)
				{
					return null;
				}
				return sessao.NomeUsuario;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x0017CBE5 File Offset: 0x0017ADE5
		public static string IdUsuario
		{
			get
			{
				ISessao sessao = DadosSistema._sessao;
				if (sessao == null)
				{
					return null;
				}
				return sessao.IdUsuario;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001957 RID: 6487 RVA: 0x0017CBF7 File Offset: 0x0017ADF7
		public static decimal LimiteDescontoUsuario
		{
			get
			{
				return DadosSistema._sessao.LimiteDesconto;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x0017CC03 File Offset: 0x0017AE03
		public static string IdClienteConsumidor
		{
			get
			{
				return "59257eeac1546a18c0360ef3";
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x0017CC0A File Offset: 0x0017AE0A
		public static RamoDeAtividade Ramo
		{
			get
			{
				return DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x0017CC2A File Offset: 0x0017AE2A
		public static bool IsPostoCombustivel
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo == RamoDeAtividade.PostoCombustivel;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x0017CC56 File Offset: 0x0017AE56
		public static bool IsFarmacia
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo == RamoDeAtividade.Farmacia;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x0017CC82 File Offset: 0x0017AE82
		public static bool IsOtica
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo == RamoDeAtividade.Otica;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600195D RID: 6493 RVA: 0x0017CCAE File Offset: 0x0017AEAE
		public static bool IsFarmaciaOuOtica
		{
			get
			{
				return DadosSistema.IsFarmacia || DadosSistema.IsOtica;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x0017CCC0 File Offset: 0x0017AEC0
		public static bool IsAdministrador
		{
			get
			{
				return DadosSistema._sessao != null && (DadosSistema._sessao.Perfis.ItemSelecionado.Sistema.Is<Administrador>() || DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Sistema.Is<AdministradorIndustria>());
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x0017CD11 File Offset: 0x0017AF11
		public static bool IsGerencial
		{
			get
			{
				return (DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Sistema.Is<Gerencial>()) || DadosSistema.IsAdministrador;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x0017CD3C File Offset: 0x0017AF3C
		public static bool IsHotel
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo == RamoDeAtividade.Hotel;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x0017CD68 File Offset: 0x0017AF68
		public static bool IsRestaurante
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo == RamoDeAtividade.Restaurante;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x0017CD94 File Offset: 0x0017AF94
		public static bool IsRestauranteOuHotel
		{
			get
			{
				return DadosSistema.IsRestaurante || DadosSistema.IsHotel;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x0017CDA4 File Offset: 0x0017AFA4
		public static bool IsComercio
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo == RamoDeAtividade.Comercio;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001964 RID: 6500 RVA: 0x0017CDD0 File Offset: 0x0017AFD0
		public static bool IsComercioOuPostoCombustivel
		{
			get
			{
				return DadosSistema.IsComercio || DadosSistema.IsPostoCombustivel;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x0017CDE0 File Offset: 0x0017AFE0
		public static bool IsPet
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo == RamoDeAtividade.Pet;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x0017CE0C File Offset: 0x0017B00C
		public static bool IsIndustria
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Sistema.Is<AdministradorIndustria>();
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x0017CE35 File Offset: 0x0017B035
		public static bool IsFarmaciaOrPet
		{
			get
			{
				return DadosSistema.IsFarmacia || DadosSistema.IsPet;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x0017CE45 File Offset: 0x0017B045
		public static bool IsFarmaciaOrPetOrOtica
		{
			get
			{
				return DadosSistema.IsFarmacia || DadosSistema.IsPet || DadosSistema.IsOtica;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x0017CE5C File Offset: 0x0017B05C
		public static bool IsMatrizFilialIndustria
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.Existe((PerfilEmpresaSessao x) => x.Empresa.Sistema.Is<AdministradorIndustria>());
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x0017CEEC File Offset: 0x0017B0EC
		public static bool IsSistemaDigiSatMEI
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Sistema.Is<DigiSatMEI>();
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x0017CF10 File Offset: 0x0017B110
		public static bool PodeConfigurarPdvOffline
		{
			get
			{
				return DadosSistema.IsAdministrador && !DadosSistema.IsPostoCombustivel && !DadosSistema.IsSistemaDigiSatMEI;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600196C RID: 6508 RVA: 0x0017CF2A File Offset: 0x0017B12A
		public static bool HabilitadoMEI
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.HabilitadoMei;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x0017CF4E File Offset: 0x0017B14E
		public static bool IsMEI
		{
			get
			{
				return DadosSistema.IsSistemaDigiSatMEI || DadosSistema.HabilitadoMEI;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x0600196E RID: 6510 RVA: 0x0017CF5E File Offset: 0x0017B15E
		public static DigisatServer.Infrastructure.Crosscutting.Model.Sistemas.Sistema Sistema
		{
			get
			{
				ISessao sessao = DadosSistema._sessao;
				if (sessao == null)
				{
					return null;
				}
				return sessao.Perfis.ItemSelecionado.Sistema;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x0017CF7A File Offset: 0x0017B17A
		// (set) Token: 0x06001970 RID: 6512 RVA: 0x0017CF81 File Offset: 0x0017B181
		public static bool IsMatrizFilial { get; private set; }

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x0017CF89 File Offset: 0x0017B189
		public static bool IsMatriz
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.IsMatriz;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x0017CFB0 File Offset: 0x0017B1B0
		public static bool IsPaf
		{
			get
			{
				if (DadosSistema._sessao != null)
				{
					Lista<PerfilEmpresaSessao> perfis = DadosSistema._sessao.Perfis;
					bool? flag;
					if (perfis == null)
					{
						flag = null;
					}
					else
					{
						PerfilEmpresaSessao itemSelecionado = perfis.ItemSelecionado;
						if (itemSelecionado == null)
						{
							flag = null;
						}
						else
						{
							Otus.Infraestrutura.Models.Sessoes.Pessoa empresa = itemSelecionado.Empresa;
							if (empresa == null)
							{
								flag = null;
							}
							else
							{
								DigisatServer.Infrastructure.Crosscutting.Model.PontoDeVenda pontoDeVenda = empresa.PontoDeVenda;
								if (pontoDeVenda == null)
								{
									flag = null;
								}
								else
								{
									IEnumerable<Ecf> ecfs = pontoDeVenda.Ecfs;
									flag = ((ecfs != null) ? new bool?(ecfs.Any<Ecf>()) : null);
								}
							}
						}
					}
					if (flag ?? false)
					{
						return true;
					}
				}
				return DadosSistema.IsPafNfce;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x0017D057 File Offset: 0x0017B257
		// (set) Token: 0x06001974 RID: 6516 RVA: 0x0017D05E File Offset: 0x0017B25E
		public static bool IsPafNfce { get; private set; }

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x0017D066 File Offset: 0x0017B266
		public static bool IsPafEcf
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.PontoDeVenda.Ecfs.Any<Ecf>();
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x0017D094 File Offset: 0x0017B294
		public static IEnumerable<Ecf> Ecfs
		{
			get
			{
				if (DadosSistema._sessao == null)
				{
					return Enumerable.Empty<Ecf>();
				}
				return DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.PontoDeVenda.Ecfs;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001977 RID: 6519 RVA: 0x0017D0C1 File Offset: 0x0017B2C1
		public static bool PermitirVisualizarFichaTecnica
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<ProdutoServico, PermitirVisualizarFichaTecnica>();
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001978 RID: 6520 RVA: 0x0017D0DB File Offset: 0x0017B2DB
		public static bool PodeVisualizarRelatorioAgendamento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Agendamento, VisualizarRelatorio>();
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001979 RID: 6521 RVA: 0x0017D0F5 File Offset: 0x0017B2F5
		public static bool PodeAlterarDataRecebimento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Recebimento, PermitirAlterarDataRecebimentoPagamento>();
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x0600197A RID: 6522 RVA: 0x0017D10F File Offset: 0x0017B30F
		public static bool PodeVisualizarInformacoesGerenciaisRecebimento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Recebimento, PermitirVisualizarInformacoesGerenciaisFinanceiras>();
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x0600197B RID: 6523 RVA: 0x0017D129 File Offset: 0x0017B329
		public static bool PodeVisualizarInformacoesGerenciaisPagamento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Pagamento, PermitirVisualizarInformacoesGerenciaisFinanceiras>();
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x0600197C RID: 6524 RVA: 0x0017D143 File Offset: 0x0017B343
		public static bool PodeVisualizarInformacoesGerenciaisMovimentoConta
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<MovimentoConta, PermitirVisualizarInformacoesGerenciaisFinanceiras>();
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600197D RID: 6525 RVA: 0x0017D15D File Offset: 0x0017B35D
		public static bool PodeVisualizarTodasMovimentacoes
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<MovimentacaoSaida, PermitirVisualizarTodasMovimentacoes>();
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x0017D177 File Offset: 0x0017B377
		public static bool PodeAlterarDataPagamento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Pagamento, PermitirAlterarDataRecebimentoPagamento>();
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x0017D191 File Offset: 0x0017B391
		public static bool PodeAlterarDataEmissaoRecebimento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Recebimento, PermitirAlterarDataEmissaoRecebimentoPagamento>();
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0017D1AB File Offset: 0x0017B3AB
		public static bool PodeAlterarDataEmissaoPagamento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Pagamento, PermitirAlterarDataEmissaoRecebimentoPagamento>();
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001981 RID: 6529 RVA: 0x0017D1C5 File Offset: 0x0017B3C5
		public static bool PodeAplicarDescontoAcrescimoRecebimento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Recebimento, PermitirDescontoAcrescimoRecebimentoPagamento>();
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x0017D1DF File Offset: 0x0017B3DF
		public static bool PodeAplicarDescontoAcimaLimite
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<MovimentacaoSaida, PermitirVenderAcimaDoLimiteDeDesconto>();
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001983 RID: 6531 RVA: 0x0017D1F9 File Offset: 0x0017B3F9
		public static bool PodeAplicarDescontoAcrescimoPagamento
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<Pagamento, PermitirDescontoAcrescimoRecebimentoPagamento>();
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001984 RID: 6532 RVA: 0x0017D213 File Offset: 0x0017B413
		public static bool PermitirDesconto
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<MovimentacaoSaida, PermitirDesconto>();
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x0017D22D File Offset: 0x0017B42D
		public static bool VisualizarPrecoVendaOrdemServico
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<OrdemServico, VisualizarPrecoVenda>();
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x0017D247 File Offset: 0x0017B447
		public static bool PermitirDocumentoDuplicado
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<DigisatServer.Infrastructure.Crosscutting.Module.Pessoa, PermitirDocumentoDuplicado>();
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x0017D261 File Offset: 0x0017B461
		public static bool PermitirAlterarLimiteCredito
		{
			get
			{
				return DadosSistema._sessao.IsNotNull() && DadosSistema._sessao.VerificarPermissao<DigisatServer.Infrastructure.Crosscutting.Module.Pessoa, PermitirAlterarLimiteCredito>();
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x0017D27B File Offset: 0x0017B47B
		public static bool PodeImprimirDavNaRetaguarda
		{
			get
			{
				return !DadosSistema.IsPaf || (DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.PodeImprimirDavNaRetaguardaRequisitoIVItem3());
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x0017D2B2 File Offset: 0x0017B4B2
		public static bool PodeGerarArquivosBinarios
		{
			get
			{
				return DadosSistema.IsPaf && DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.PodeGerarArquivosBinariosRequisitoXIIItem234e5();
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600198A RID: 6538 RVA: 0x0017D2E7 File Offset: 0x0017B4E7
		public static bool PodeEmitirNFe
		{
			get
			{
				return !DadosSistema.IsPaf || (DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.PodeEmitirNFeRequisitoXXVIIIItem2a());
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x0017D31E File Offset: 0x0017B51E
		public static bool PodeEmitirNFCePreVendaDav
		{
			get
			{
				return DadosSistema.IsPaf && DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.PodeEmitirNFCeRequisitoXXVIIIItem2b();
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600198C RID: 6540 RVA: 0x0017D353 File Offset: 0x0017B553
		public static UF UFEmpresaLogada
		{
			get
			{
				ISessao sessao = DadosSistema._sessao;
				if (sessao == null)
				{
					return null;
				}
				return sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600198D RID: 6541 RVA: 0x0017D37C File Offset: 0x0017B57C
		public static bool PodeEmitirNFCe
		{
			get
			{
				return DadosSistema._sessao != null && (DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.PodeEmitirNFCeRequisitoXXVIIIItem2b() || DadosSistema._configuracaoClientService.ObterConfiguracao().ModoFuncionamentoPaf == ModoFuncionamento.PafNfce);
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x0017D3CC File Offset: 0x0017B5CC
		public static bool PodeEmitirNotaManualRetaguarda
		{
			get
			{
				return (DadosSistema._sessao != null && !DadosSistema.IsPaf && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.CodigoIbge != 42) || (DadosSistema.IsPaf && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.PodeEmitirNotaManualRequisitoXXVIIIItem7());
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x0017D43C File Offset: 0x0017B63C
		public static bool PodeEmitirNotaManualPontoDeVenda
		{
			get
			{
				return !DadosSistema.IsPostoCombustivel && DadosSistema.IsPaf && DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.PodeEmitirNotaManualRequisitoXXVIIIItem8() && !DadosSistema.IsPafNfce;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001990 RID: 6544 RVA: 0x0017D48D File Offset: 0x0017B68D
		public static ConfiguracaoDto ConfiguracaoDto
		{
			get
			{
				return DadosSistema._configuracaoDto;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001991 RID: 6545 RVA: 0x0017D494 File Offset: 0x0017B694
		public static ConfiguracaoServidorDto ConfiguracaoServidorDto
		{
			get
			{
				return DadosSistema._configuracaoServidorDto;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001992 RID: 6546 RVA: 0x0017D49B File Offset: 0x0017B69B
		public static bool NaoPodeBloquearPontoDeVendaBlocoX
		{
			get
			{
				return DadosSistema.IsPaf && DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.NaoPodeBloquearPontoDeVendaRequisitoLVIIIItem423E43();
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x0017D4D0 File Offset: 0x0017B6D0
		public static bool DeveGerarBlocoXRequisitoLVIII
		{
			get
			{
				return DadosSistema.IsPaf && DadosSistema._sessao != null && DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Municipio.Uf.DeveGerarBlocoXRequisitoLVIII() && DadosSistema._webServiceEnvioPafEcfServiceAtivo;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001994 RID: 6548 RVA: 0x0017D50C File Offset: 0x0017B70C
		public static int CodigoIbgeMunicipioEmitente
		{
			get
			{
				ISessao sessao = DadosSistema._sessao;
				if (sessao == null)
				{
					return 0;
				}
				return sessao.Perfis.ItemSelecionado.Empresa.Municipio.CodigoIbge;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x0017D532 File Offset: 0x0017B732
		public static bool UsuarioSuporteLogado
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.UsuarioSuporte;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x0017D547 File Offset: 0x0017B747
		public static bool UsuarioRevendaLogado
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.UsuarioRevenda;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001997 RID: 6551 RVA: 0x0017D55C File Offset: 0x0017B75C
		public static bool UsuarioPedirMenuLogado
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.UsuarioPedirMenu;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001998 RID: 6552 RVA: 0x0017D571 File Offset: 0x0017B771
		public static bool UsuarioAdministradorLogado
		{
			get
			{
				if (DadosSistema._sessao != null)
				{
					string nomeUsuario = DadosSistema._sessao.NomeUsuario;
					return nomeUsuario != null && nomeUsuario.Equals("Usuário Administrador");
				}
				return false;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x0017D596 File Offset: 0x0017B796
		public static bool UsuarioNaoPrecisaAbrirTurno
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.UsuarioNaoPrecisaAbrirTurno;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600199A RID: 6554 RVA: 0x0017D5AB File Offset: 0x0017B7AB
		public static bool ManterItemRemovidoDav
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ManterItemRemovidoDav.Valor;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600199B RID: 6555 RVA: 0x0017D5CA File Offset: 0x0017B7CA
		public static bool DeveSolicitarGarcomAoAbrirMesaConta
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.SolicitarGarcomAoAbrirMesaConta.Valor;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600199C RID: 6556 RVA: 0x0017D5E9 File Offset: 0x0017B7E9
		public static bool DeveSolicitarPessoaAoAbrirMesaConta
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.SolicitarPessoaAoAbrirMesaConta.Valor;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x0600199D RID: 6557 RVA: 0x0017D608 File Offset: 0x0017B808
		public static bool DeveSolicitarNumeroMesaAoAbrirConta
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.SolicitarNumeroMesaAoAbrirConta.Valor && DadosSistema.TipoMesaConta == TipoMesaContaCliente.ContaCliente;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x0600199E RID: 6558 RVA: 0x0017D631 File Offset: 0x0017B831
		public static bool AgruparItensConferenciaMesaContaCliente
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.AgruparItensConferenciaMesaContaCliente.Valor;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x0017D650 File Offset: 0x0017B850
		public static bool AgruparItensImportacaoPontoDeVendas
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.AgruparItensImportacaoPontoDeVendas.Valor;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060019A0 RID: 6560 RVA: 0x0017D66F File Offset: 0x0017B86F
		public static bool SolicitarReplicacaoUltimoDelivey
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.SolicitarReplicacaoUltimoDelivey.Valor;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060019A1 RID: 6561 RVA: 0x0017D68E File Offset: 0x0017B88E
		public static bool SolicitarOndeFaturarContrato
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Financeiro.SolicitarOndeFaturarContrato.Valor;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060019A2 RID: 6562 RVA: 0x0017D6B0 File Offset: 0x0017B8B0
		public static string TipoRestaurante
		{
			get
			{
				string text = string.Empty;
				if (DadosSistema._sessao != null && DadosSistema.IsRestauranteOuHotel)
				{
					text = (DadosSistema.IsRestaurante ? DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.To<DigisatServer.Infrastructure.Crosscutting.Model.RamosAtividade.Restaurante>().TipoMesaContaCliente.RetornarDescricao() : DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.To<Hotel>().TipoMesaContaCliente.RetornarDescricao());
					if (text.Equals("Indefinido"))
					{
						text = "Comanda";
					}
				}
				return text;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060019A3 RID: 6563 RVA: 0x0017D74C File Offset: 0x0017B94C
		public static TipoMesaContaCliente TipoMesaConta
		{
			get
			{
				if (DadosSistema._sessao == null || !DadosSistema.IsRestauranteOuHotel)
				{
					return TipoMesaContaCliente.Indefinido;
				}
				if (!DadosSistema.IsRestaurante)
				{
					return DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.To<Hotel>().TipoMesaContaCliente;
				}
				return DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.To<DigisatServer.Infrastructure.Crosscutting.Model.RamosAtividade.Restaurante>().TipoMesaContaCliente;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060019A4 RID: 6564 RVA: 0x0017D7B7 File Offset: 0x0017B9B7
		public static string TipoMesaContaPlural
		{
			get
			{
				if (DadosSistema.TipoMesaConta == TipoMesaContaCliente.ContaCliente)
				{
					return "Contas de cliente";
				}
				if (DadosSistema.TipoMesaConta != TipoMesaContaCliente.Mesa)
				{
					return "Indefinido";
				}
				return "Mesas";
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060019A5 RID: 6565 RVA: 0x0017D7DA File Offset: 0x0017B9DA
		public static bool BancoDadosConfigurado
		{
			get
			{
				return DadosSistema._bancoDadosConfigurado;
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0017D7E1 File Offset: 0x0017B9E1
		public static bool PodeExecutarFuncao(params Type[] sistema)
		{
			ISessao sessao = DadosSistema._sessao;
			Type type;
			if (sessao == null)
			{
				type = null;
			}
			else
			{
				Lista<PerfilEmpresaSessao> perfis = sessao.Perfis;
				type = ((perfis != null) ? perfis.ItemSelecionado.Sistema.GetType() : null);
			}
			return sistema.Contains(type);
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060019A7 RID: 6567 RVA: 0x0017D810 File Offset: 0x0017BA10
		public static bool PodeEmitirDocumentoFiscalCeara
		{
			get
			{
				return DadosSistema.EmpresaLogada.Municipio.Uf.CodigoIbge.Equals(23);
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060019A8 RID: 6568 RVA: 0x0017D83B File Offset: 0x0017BA3B
		public static bool CalcularPesoPrecoAutomaticoMdfe
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.CalcularPesoPrecoAutomaticoMdfe.Valor;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060019A9 RID: 6569 RVA: 0x0017D85A File Offset: 0x0017BA5A
		public static bool PodeEmitirMfe
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema.EmpresaLogada.PontoDeVenda.Sats.Any<Sat>() && DadosSistema.PodeEmitirDocumentoFiscalCeara;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x0017D880 File Offset: 0x0017BA80
		public static bool PodeAlterarUsuarioAgendamento
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.VerificarPermissao<Agendamento, PermitirAlterarUsuarioAgendamento>();
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060019AB RID: 6571 RVA: 0x0017D895 File Offset: 0x0017BA95
		public static bool PodeGerarEtiqueta
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._sessao.VerificarPermissao<Etiqueta, Adicionar>();
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060019AC RID: 6572 RVA: 0x0017D8AA File Offset: 0x0017BAAA
		// (set) Token: 0x060019AD RID: 6573 RVA: 0x0017D8B1 File Offset: 0x0017BAB1
		public static bool TurnoAberto { get; set; }

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x0017D8B9 File Offset: 0x0017BAB9
		public static bool ImprimirDetalheVendaNfce
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirDetalheDaVendaNfce.Valor;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060019AF RID: 6575 RVA: 0x0017D8D8 File Offset: 0x0017BAD8
		public static bool ImprimirDanfeResumidoNfce
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirDanfeNfceResumido.Valor;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060019B0 RID: 6576 RVA: 0x0017D8F7 File Offset: 0x0017BAF7
		public static bool ImprimirItemResumidoDanfeNfce
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirItemResumidoDanfeNfce.Valor;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0017D916 File Offset: 0x0017BB16
		public static bool ImprimirItemResumidoDanfeNfe
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirItemResumidoDanfeNfe.Valor;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0017D935 File Offset: 0x0017BB35
		public static bool ImprimirCodigoBarrasItemDanfeNfe
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirCodigoBarrasItemDanfeNfe.Valor;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0017D954 File Offset: 0x0017BB54
		public static bool ImprimirStItemDanfeNfe
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirStItemDanfeNfe.Valor;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060019B4 RID: 6580 RVA: 0x0017D973 File Offset: 0x0017BB73
		public static bool ImprimirFcpItemDanfeNfe
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirFcpItemDanfeNfe.Valor;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x0017D992 File Offset: 0x0017BB92
		public static bool ImprimirUnidadeQuantidadeTributariaItemDanfeNfe
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirUnidadeQuantidadeTributariaItemDanfeNfe.Valor;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060019B6 RID: 6582 RVA: 0x0017D9B1 File Offset: 0x0017BBB1
		public static bool ImprimirNumeroEItemPedidoCompraItemDanfeNfe
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirNumeroEItemPedidoCompraItemDanfeNfe.Valor;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060019B7 RID: 6583 RVA: 0x0017D9D0 File Offset: 0x0017BBD0
		public static bool SuprimirTotaisReformaTributariaDanfeNfe
		{
			get
			{
				if (DadosSistema._sessao != null && !DadosSistema._configuracaoDto.Saida.SuprimirTotaisReformaTributariaDanfeNfe.Valor)
				{
					ConfiguracaoServidorDto configuracaoServidorDto = DadosSistema._configuracaoServidorDto;
					int? num;
					if (configuracaoServidorDto == null)
					{
						num = null;
					}
					else
					{
						NFeServidorDto nfe = configuracaoServidorDto.NFe;
						num = ((nfe != null) ? new bool?(nfe.GerarCamposReferenteReformaTributariaConsumo) : null);
					}
					return (num ?? 0) == 0;
				}
				return true;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060019B8 RID: 6584 RVA: 0x0017DA45 File Offset: 0x0017BC45
		public static bool ImprimirItemResumidoEm40Colunas
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirItemResumidoEm40Colunas.Valor;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x0017DA64 File Offset: 0x0017BC64
		public static bool ImprimirDadosDoEncerranteItemDanfeNfce
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.ImprimirDadosDoEncerranteItemDanfeNfce.Valor;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060019BA RID: 6586 RVA: 0x0017DA84 File Offset: 0x0017BC84
		public static int MargemEsquerdaImpressoraNaoFiscal
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				int? num;
				if (configuracaoClientService == null)
				{
					num = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						num = null;
					}
					else
					{
						ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscal = configuracaoClient.ImpressoraNaoFiscal;
						num = ((impressoraNaoFiscal != null) ? new int?(impressoraNaoFiscal.RetornarMargemEsquerda()) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 50;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x0017DAEC File Offset: 0x0017BCEC
		public static bool RollPaper
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				bool? flag;
				if (configuracaoClientService == null)
				{
					flag = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						flag = null;
					}
					else
					{
						ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscal = configuracaoClient.ImpressoraNaoFiscal;
						flag = ((impressoraNaoFiscal != null) ? new bool?(impressoraNaoFiscal.RollPaper) : null);
					}
				}
				return flag ?? false;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060019BC RID: 6588 RVA: 0x0017DB54 File Offset: 0x0017BD54
		public static bool EnviarComandoCortarPapel
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				bool? flag;
				if (configuracaoClientService == null)
				{
					flag = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						flag = null;
					}
					else
					{
						ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscal = configuracaoClient.ImpressoraNaoFiscal;
						flag = ((impressoraNaoFiscal != null) ? new bool?(impressoraNaoFiscal.EnviarComandoCortarPapel) : null);
					}
				}
				return flag ?? false;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060019BD RID: 6589 RVA: 0x0017DBBC File Offset: 0x0017BDBC
		public static bool PossuiGavetaConectada
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				bool? flag;
				if (configuracaoClientService == null)
				{
					flag = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						flag = null;
					}
					else
					{
						ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscal = configuracaoClient.ImpressoraNaoFiscal;
						flag = ((impressoraNaoFiscal != null) ? impressoraNaoFiscal.PossuiGavetaConectada : null);
					}
				}
				return flag ?? true;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x060019BE RID: 6590 RVA: 0x0017DC1D File Offset: 0x0017BE1D
		public static string ImpressoraNaoFiscalDanfeNfe
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				if (configuracaoClientService == null)
				{
					return null;
				}
				ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
				if (configuracaoClient == null)
				{
					return null;
				}
				ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscalDanfeNfe = configuracaoClient.ImpressoraNaoFiscalDanfeNfe;
				if (impressoraNaoFiscalDanfeNfe == null)
				{
					return null;
				}
				return impressoraNaoFiscalDanfeNfe.Descricao;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060019BF RID: 6591 RVA: 0x0017DC45 File Offset: 0x0017BE45
		public static string ImpressoraNaoFiscalEtiqueta
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				if (configuracaoClientService == null)
				{
					return null;
				}
				ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
				if (configuracaoClient == null)
				{
					return null;
				}
				ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscalEtiqueta = configuracaoClient.ImpressoraNaoFiscalEtiqueta;
				if (impressoraNaoFiscalEtiqueta == null)
				{
					return null;
				}
				return impressoraNaoFiscalEtiqueta.Descricao;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x0017DC6D File Offset: 0x0017BE6D
		public static string ImpressoraNaoFiscalOutrosImpressos
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				if (configuracaoClientService == null)
				{
					return null;
				}
				ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
				if (configuracaoClient == null)
				{
					return null;
				}
				ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscalOutrosImpressos = configuracaoClient.ImpressoraNaoFiscalOutrosImpressos;
				if (impressoraNaoFiscalOutrosImpressos == null)
				{
					return null;
				}
				return impressoraNaoFiscalOutrosImpressos.Descricao;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x0017DC98 File Offset: 0x0017BE98
		public static int MargemEsquerdaImpressoraNaoFiscalDanfeNfe
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				int? num;
				if (configuracaoClientService == null)
				{
					num = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						num = null;
					}
					else
					{
						ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscalDanfeNfe = configuracaoClient.ImpressoraNaoFiscalDanfeNfe;
						num = ((impressoraNaoFiscalDanfeNfe != null) ? new int?(impressoraNaoFiscalDanfeNfe.RetornarMargemEsquerda()) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 50;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060019C2 RID: 6594 RVA: 0x0017DD00 File Offset: 0x0017BF00
		public static int MargemSuperiorImpressoraNaoFiscalDanfeNfe
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				int? num;
				if (configuracaoClientService == null)
				{
					num = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						num = null;
					}
					else
					{
						ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscalDanfeNfe = configuracaoClient.ImpressoraNaoFiscalDanfeNfe;
						num = ((impressoraNaoFiscalDanfeNfe != null) ? new int?(impressoraNaoFiscalDanfeNfe.RetornarMargemSuperior()) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 50;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x0017DD68 File Offset: 0x0017BF68
		public static TamanhoBobina TamanhoBobina
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				TamanhoBobina? tamanhoBobina;
				if (configuracaoClientService == null)
				{
					tamanhoBobina = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						tamanhoBobina = null;
					}
					else
					{
						ConfiguracaoTamanhoBobina tamanhoBobina2 = configuracaoClient.TamanhoBobina;
						tamanhoBobina = ((tamanhoBobina2 != null) ? new TamanhoBobina?(tamanhoBobina2.TamanhoBobina) : null);
					}
				}
				TamanhoBobina? tamanhoBobina3 = tamanhoBobina;
				if (tamanhoBobina3 == null)
				{
					return TamanhoBobina.SetentaENoveOitentaMilimetros;
				}
				return tamanhoBobina3.GetValueOrDefault();
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060019C4 RID: 6596 RVA: 0x0017DDD0 File Offset: 0x0017BFD0
		public static int NumeroCopiasDanfeNfe
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				int? num;
				if (configuracaoClientService == null)
				{
					num = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						num = null;
					}
					else
					{
						ConfiguracaoImpressoraNaoFiscal impressoraNaoFiscalDanfeNfe = configuracaoClient.ImpressoraNaoFiscalDanfeNfe;
						num = ((impressoraNaoFiscalDanfeNfe != null) ? new int?(impressoraNaoFiscalDanfeNfe.NumeroCopias) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 1;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x0017DE38 File Offset: 0x0017C038
		public static int RegistrosPorPaginaProdutoServico
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				int? num;
				if (configuracaoDto == null)
				{
					num = null;
				}
				else
				{
					GeralDto geral = configuracaoDto.Geral;
					if (geral == null)
					{
						num = null;
					}
					else
					{
						InteiroDto registrosPorPaginaProdutoServico = geral.RegistrosPorPaginaProdutoServico;
						num = ((registrosPorPaginaProdutoServico != null) ? new int?(registrosPorPaginaProdutoServico.Valor) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 60;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060019C6 RID: 6598 RVA: 0x0017DEA0 File Offset: 0x0017C0A0
		public static int RegistrosPorPaginaPessoa
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				int? num;
				if (configuracaoDto == null)
				{
					num = null;
				}
				else
				{
					GeralDto geral = configuracaoDto.Geral;
					if (geral == null)
					{
						num = null;
					}
					else
					{
						InteiroDto registrosPorPaginaPessoa = geral.RegistrosPorPaginaPessoa;
						num = ((registrosPorPaginaPessoa != null) ? new int?(registrosPorPaginaPessoa.Valor) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 60;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x0017DF08 File Offset: 0x0017C108
		public static int QuantidadeQuartosExibirMapaQuartos
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				int? num;
				if (configuracaoDto == null)
				{
					num = null;
				}
				else
				{
					GeralDto geral = configuracaoDto.Geral;
					if (geral == null)
					{
						num = null;
					}
					else
					{
						InteiroDto quantidadeQuartosExibirMapaQuartos = geral.QuantidadeQuartosExibirMapaQuartos;
						num = ((quantidadeQuartosExibirMapaQuartos != null) ? new int?(quantidadeQuartosExibirMapaQuartos.Valor) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 10;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060019C8 RID: 6600 RVA: 0x0017DF70 File Offset: 0x0017C170
		public static int QuantidadeDiasExibirMapaQuartos
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				int? num;
				if (configuracaoDto == null)
				{
					num = null;
				}
				else
				{
					GeralDto geral = configuracaoDto.Geral;
					if (geral == null)
					{
						num = null;
					}
					else
					{
						InteiroDto quantidadeDiasExibirMapaQuartos = geral.QuantidadeDiasExibirMapaQuartos;
						num = ((quantidadeDiasExibirMapaQuartos != null) ? new int?(quantidadeDiasExibirMapaQuartos.Valor) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 10;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0017DFD7 File Offset: 0x0017C1D7
		public static bool UtilizarPrePagoComoDescontoHospedagem
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Geral.UtilizarPrePagoComoDescontoHospedagem.Valor;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x0017DFF8 File Offset: 0x0017C1F8
		public static DateTime HorarioPadraoCheckOut
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				DateTime? dateTime;
				if (configuracaoDto == null)
				{
					dateTime = null;
				}
				else
				{
					GeralDto geral = configuracaoDto.Geral;
					if (geral == null)
					{
						dateTime = null;
					}
					else
					{
						DataHoraDto horarioPadraoCheckOut = geral.HorarioPadraoCheckOut;
						dateTime = ((horarioPadraoCheckOut != null) ? new DateTime?(horarioPadraoCheckOut.Valor) : null);
					}
				}
				DateTime? dateTime2 = dateTime;
				if (dateTime2 == null)
				{
					return DateTime.Now;
				}
				return dateTime2.GetValueOrDefault();
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x0017E064 File Offset: 0x0017C264
		public static DateTime HorarioPadraoCheckInHunit
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				DateTime? dateTime;
				if (configuracaoDto == null)
				{
					dateTime = null;
				}
				else
				{
					GeralDto geral = configuracaoDto.Geral;
					if (geral == null)
					{
						dateTime = null;
					}
					else
					{
						DataHoraDto horarioPadraoCheckInHunit = geral.HorarioPadraoCheckInHunit;
						dateTime = ((horarioPadraoCheckInHunit != null) ? new DateTime?(horarioPadraoCheckInHunit.Valor) : null);
					}
				}
				DateTime? dateTime2 = dateTime;
				if (dateTime2 == null)
				{
					return DateTime.Now;
				}
				return dateTime2.GetValueOrDefault();
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060019CC RID: 6604 RVA: 0x0017E0CE File Offset: 0x0017C2CE
		public static bool CarregarQuantidadeRegistrosBuscaMovimentacao
		{
			get
			{
				return DadosSistema._sessao != null && DadosSistema._configuracaoDto.Saida.CarregarQuantidadeRegistrosBuscaMovimentacao.Valor;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0017E0F0 File Offset: 0x0017C2F0
		public static int QuantidadeRegistrosCarregarBuscaMovimentacoes
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				int? num;
				if (configuracaoDto == null)
				{
					num = null;
				}
				else
				{
					SaidaDto saida = configuracaoDto.Saida;
					if (saida == null)
					{
						num = null;
					}
					else
					{
						InteiroDto quantidadeRegistrosCarregarBuscaMovimentacoes = saida.QuantidadeRegistrosCarregarBuscaMovimentacoes;
						num = ((quantidadeRegistrosCarregarBuscaMovimentacoes != null) ? new int?(quantidadeRegistrosCarregarBuscaMovimentacoes.Valor) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 0;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x0017E158 File Offset: 0x0017C358
		public static int QuantidadeObjetosExibirMapaLocacoes
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				int? num;
				if (configuracaoDto == null)
				{
					num = null;
				}
				else
				{
					GeralDto geral = configuracaoDto.Geral;
					if (geral == null)
					{
						num = null;
					}
					else
					{
						InteiroDto quantidadeObjetosExibirMapaLocacoes = geral.QuantidadeObjetosExibirMapaLocacoes;
						num = ((quantidadeObjetosExibirMapaLocacoes != null) ? new int?(quantidadeObjetosExibirMapaLocacoes.Valor) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 10;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x0017E1C0 File Offset: 0x0017C3C0
		public static int QuantidadeDiasExibirMapaLocacoes
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema._configuracaoDto;
				int? num;
				if (configuracaoDto == null)
				{
					num = null;
				}
				else
				{
					GeralDto geral = configuracaoDto.Geral;
					if (geral == null)
					{
						num = null;
					}
					else
					{
						InteiroDto quantidadeDiasExibirMapaLocacoes = geral.QuantidadeDiasExibirMapaLocacoes;
						num = ((quantidadeDiasExibirMapaLocacoes != null) ? new int?(quantidadeDiasExibirMapaLocacoes.Valor) : null);
					}
				}
				int? num2 = num;
				if (num2 == null)
				{
					return 10;
				}
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x0017E228 File Offset: 0x0017C428
		public static bool PodeAtualizarPrecoProduto
		{
			get
			{
				return (DadosSistema._sessao != null && (DadosSistema._sessao.Perfis.ItemSelecionado.Sistema.Is<DigiSatMEI>() || DadosSistema._sessao.Perfis.ItemSelecionado.Sistema.Is<Comercial>())) || DadosSistema._sessao.VerificarPermissao<ProdutoServico, PermitirAtualizarPreco>();
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x0017E27E File Offset: 0x0017C47E
		public static string EnderecoServidorWhatsApp
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				string text;
				if (configuracaoClientService == null)
				{
					text = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					if (configuracaoClient == null)
					{
						text = null;
					}
					else
					{
						ConfiguracaoWhatsApp whatsApp = configuracaoClient.WhatsApp;
						text = ((whatsApp != null) ? whatsApp.Ip : null);
					}
				}
				return text ?? string.Empty;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x0017E2B2 File Offset: 0x0017C4B2
		public static bool WhatsAppConfigurado
		{
			get
			{
				return !string.IsNullOrEmpty(DadosSistema.EnderecoServidorWhatsApp);
			}
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0017E2C4 File Offset: 0x0017C4C4
		public static bool RetornarPermitirEstoqueNegativoPorTipoMovimentacao(TipoMovimentacao tipoMovimentacao)
		{
			if (tipoMovimentacao <= TipoMovimentacao.DavOs)
			{
				if (tipoMovimentacao != TipoMovimentacao.Dav && tipoMovimentacao != TipoMovimentacao.DavOs)
				{
					goto IL_002C;
				}
			}
			else if (tipoMovimentacao != TipoMovimentacao.PreVenda && tipoMovimentacao != TipoMovimentacao.Delivery)
			{
				goto IL_002C;
			}
			return DadosSistema.ConfiguracaoDto.Saida.PermitirEstoqueNegativoDocumentoAuxiliarVenda.Valor;
			IL_002C:
			return DadosSistema.ConfiguracaoDto.Saida.PermitirEstoqueNegativo.Valor;
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x0017E314 File Offset: 0x0017C514
		public static int NumeroPdvSat
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				ConfiguracaoSat configuracaoSat;
				if (configuracaoClientService == null)
				{
					configuracaoSat = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					configuracaoSat = ((configuracaoClient != null) ? configuracaoClient.Sat : null);
				}
				ConfiguracaoSat configuracaoSat2 = configuracaoSat;
				if (configuracaoSat2 == null || !configuracaoSat2.SatConfigurado)
				{
					return 0;
				}
				return configuracaoSat2.NumeroPdv;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x0017E354 File Offset: 0x0017C554
		public static int NumeroPdvScanntech
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				ConfiguracaoScanntech configuracaoScanntech;
				if (configuracaoClientService == null)
				{
					configuracaoScanntech = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					configuracaoScanntech = ((configuracaoClient != null) ? configuracaoClient.Scanntech : null);
				}
				ConfiguracaoScanntech configuracaoScanntech2 = configuracaoScanntech;
				if (configuracaoScanntech2 == null || !configuracaoScanntech2.ScanntechConfigurado)
				{
					return 0;
				}
				return configuracaoScanntech2.NumeroPdv;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x0017E394 File Offset: 0x0017C594
		public static int NumeroPdvTerminalPos
		{
			get
			{
				IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
				ConfiguracaoTerminalPos configuracaoTerminalPos;
				if (configuracaoClientService == null)
				{
					configuracaoTerminalPos = null;
				}
				else
				{
					ConfiguracaoClient configuracaoClient = configuracaoClientService.ObterConfiguracao();
					configuracaoTerminalPos = ((configuracaoClient != null) ? configuracaoClient.TerminalPos : null);
				}
				ConfiguracaoTerminalPos configuracaoTerminalPos2 = configuracaoTerminalPos;
				if (configuracaoTerminalPos2 == null || !configuracaoTerminalPos2.TerminalPosConfigurado)
				{
					return 0;
				}
				return configuracaoTerminalPos2.NumeroPdv;
			}
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x0017E3D2 File Offset: 0x0017C5D2
		public static bool VerificarPermissao<TModulo, TDireito>()
		{
			return DadosSistema._sessao != null && DadosSistema._sessao.VerificarPermissao<TModulo, TDireito>();
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0017E3E7 File Offset: 0x0017C5E7
		public static void SetarAutenticacaoService(IAutenticacaoService autenticacaoService)
		{
			DadosSistema.AutenticacaoService = autenticacaoService;
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0017E3EF File Offset: 0x0017C5EF
		public static void CarregarModulo(string nomeModulo)
		{
			DadosSistema._container.Resolve<IModuleManager>(Array.Empty<ResolverOverride>()).LoadModule(nomeModulo);
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0017E406 File Offset: 0x0017C606
		public static IRelatorioCatalogoService ObterCatalogoRelatorios()
		{
			DadosSistema._container.Resolve<IModuleManager>(Array.Empty<ResolverOverride>()).LoadModule("RelatorioModulo");
			return DadosSistema._container.Resolve<IRelatorioCatalogoService>(Array.Empty<ResolverOverride>());
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0017E430 File Offset: 0x0017C630
		public static IArquivoFiscalCatalogoService ObterCatalogoArquivosFiscais()
		{
			return DadosSistema._container.Resolve<IArquivoFiscalCatalogoService>(Array.Empty<ResolverOverride>());
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0017E444 File Offset: 0x0017C644
		public static void ExecutarGeracoesAutomaticasSeNecessario()
		{
			if (!DadosSistema.IsGerencial)
			{
				return;
			}
			IConfiguracaoClientService configuracaoClientService = DadosSistema._configuracaoClientService;
			ConfiguracaoClient configuracaoClient = ((configuracaoClientService != null) ? configuracaoClientService.ObterConfiguracao() : null);
			if (configuracaoClient == null)
			{
				return;
			}
			AutomacaoLog.Init();
			if (configuracaoClient.GerarRelatoriosAutomaticamenteNesteTerminal)
			{
				try
				{
					DadosSistema._container.Resolve<IModuleManager>(Array.Empty<ResolverOverride>()).LoadModule("RelatorioModulo");
					DadosSistema._container.Resolve<IGeracaoAutomaticaRelatoriosService>(Array.Empty<ResolverOverride>()).Executar();
				}
				catch (Exception ex)
				{
					AutomacaoLog.CriarLog(string.Format("Erro ao executar rotina de geração de relatórios automaticamente {0}", ex.GetLastException().Message), false);
				}
			}
			if (configuracaoClient.GerarArquivosFiscaisAutomaticamenteNesteTerminal)
			{
				try
				{
					DadosSistema._container.Resolve<IGeracaoAutomaticaArquivosFiscaisService>(Array.Empty<ResolverOverride>()).Executar();
				}
				catch (Exception ex2)
				{
					AutomacaoLog.CriarLog(string.Format("Erro ao executar rotina de geração de arquivos fiscais automaticamente {0}", ex2.GetLastException().Message), false);
				}
			}
			try
			{
				DadosSistema._container.Resolve<IRegeneracaoAutomaticaTimerService>(Array.Empty<ResolverOverride>()).Iniciar();
			}
			catch (Exception ex3)
			{
				AutomacaoLog.CriarLog(string.Format("Erro ao iniciar timer de geração automática {0}", ex3.GetLastException().Message), false);
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x0017E564 File Offset: 0x0017C764
		public static void SetarContainer(IUnityContainer container)
		{
			DadosSistema._container = container;
			DadosSistema._impressoraNaoFiscalService = container.Resolve<IImpressoraNaoFiscalService>(Array.Empty<ResolverOverride>());
			DadosSistema._impressaoGraficoService = container.Resolve<IImpressaoGraficoService>(Array.Empty<ResolverOverride>());
			DadosSistema._impressaoImagemItensMovimentacaoService = container.Resolve<IImpressaoImagemItensMovimentacaoService>(Array.Empty<ResolverOverride>());
			DadosSistema._impressaoImagemAnexaDavOsService = container.Resolve<IImpressaoImagemAnexaDavOsService>(Array.Empty<ResolverOverride>());
			DadosSistema._balancaService = container.Resolve<IBalancaService>(Array.Empty<ResolverOverride>());
			DadosSistema._eventPublisher = container.Resolve<IEventPublisher>(Array.Empty<ResolverOverride>());
			DadosSistema._boletoProxyService = container.Resolve<IBoletoProxyService>(Array.Empty<ResolverOverride>());
			DadosSistema._mobileService = container.Resolve<IMobileService>(Array.Empty<ResolverOverride>());
			DadosSistema._notificacao = container.Resolve<INotificacao>(Array.Empty<ResolverOverride>());
			DadosSistema._fileIoWrapper = container.Resolve<IFileIoWrapper>(Array.Empty<ResolverOverride>());
			DadosSistema._assinaturaWrapper = container.Resolve<IAssinaturaWrapper>(Array.Empty<ResolverOverride>());
			DadosSistema._eventPublisher.GetEvent<MongoDbConfiguradoEvent>().Subscribe<MongoDbConfiguradoEvent>(delegate(MongoDbConfiguradoEvent e)
			{
				DadosSistema._bancoDadosConfigurado = true;
			});
			DadosSistema._eventPublisher.GetEvent<SessaoAtualizadaEvent>().Subscribe<SessaoAtualizadaEvent>(delegate(SessaoAtualizadaEvent e)
			{
				DadosSistema._sessao = e.Sessao;
				if (DadosSistema._sessao.IsNull())
				{
					return;
				}
				DadosSistema._service = container.Resolve<IWService>(Array.Empty<ResolverOverride>());
				DadosArquivoHabilitacao dadosArquivoHabilitacao;
				try
				{
					dadosArquivoHabilitacao = DadosSistema._service.Canal<IInformacaoModuleService, DadosArquivoHabilitacao>((IInformacaoModuleService x) => x.ObterDadosArquivoHabilitacao());
					DadosSistema.AtualizarArquivoHabilitacaoLocal();
				}
				catch (Exception)
				{
					dadosArquivoHabilitacao = DadosSistema.ObterDadosArquivoHabilitacaoValidado();
				}
				DadosHabilitacao.DadosArquivoHabilitacao = dadosArquivoHabilitacao;
				if (dadosArquivoHabilitacao.IsNull() || !dadosArquivoHabilitacao.ContemCnpj(DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.CpfCnpj))
				{
					DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.SetarHabilitadoMei(false);
					return;
				}
				DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.SetarHabilitadoMei(dadosArquivoHabilitacao.IsMeiPorCnpj(DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.CpfCnpj));
				List<int> list = new List<int>(dadosArquivoHabilitacao.Modulos.Select<DigisatServer.Infrastructure.Crosscutting.Habilitacao.Modulo, int>((DigisatServer.Infrastructure.Crosscutting.Habilitacao.Modulo x) => x.Codigo));
				DadosSistema.AdicionarModulosDependentes(list);
				DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.SetarCodigosModulosAdicionais(list);
				if (!DadosSistema._sessao.Perfis.ItemSelecionado.Sistema.Tipo.Equals(dadosArquivoHabilitacao.SistemaPorCnpj(DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.CpfCnpj)))
				{
					DialogoFactory.MostrarDialogoConfirma("O sistema identificou inconsistências relacionadas aos direitos de uso e da licença, conforme previsto na Cláusula Segunda, item “c”, e/ou Cláusula Quinta do contrato. Para regularizar, reinicie o Servidor e o Sistema através dos executáveis originais e autenticados. Caso a inconsistência persista, contate o suporte autorizado.", false, HorizontalAlignment.Center);
					Application.Current.Shutdown();
				}
			});
			DadosSistema._eventPublisher.GetEvent<ExecutouLogonEvent>().Subscribe<ExecutouLogonEvent>(delegate(ExecutouLogonEvent e)
			{
				DadosSistema._service = container.Resolve<IWService>(Array.Empty<ResolverOverride>());
				DadosSistema._service.Canal<IConfiguracaoServidorModuleService, Task<bool>>((IConfiguracaoServidorModuleService service) => service.IsPafNfce()).ContinueWith(delegate(Task<bool> task)
				{
					if (task.IsFaulted)
					{
						return;
					}
					DadosSistema.IsPafNfce = task.Result;
				});
				DadosSistema.PreencherConfiguracaoDto(DadosSistema._service);
				DadosSistema._shipayService = container.Resolve<IShipayService>(Array.Empty<ResolverOverride>());
				DadosSistema._shopeeApiService = container.Resolve<IShopeeApiService>(Array.Empty<ResolverOverride>());
			});
			DadosSistema._eventPublisher.GetEvent<ConfiguracaoDto>().Subscribe<ConfiguracaoDto>(delegate(ConfiguracaoDto e)
			{
				DadosSistema._configuracaoDto = e;
			});
			DadosSistema._configuracaoRepositoryRead = container.Resolve<IConfiguracaoRepositoryRead>(Array.Empty<ResolverOverride>());
			DadosSistema._contaRepositoryRead = container.Resolve<IContaRepositoryRead>(Array.Empty<ResolverOverride>());
			DadosSistema._relacaoEspecieContaRepositoryRead = container.Resolve<IRepositoryEmpresaRead<DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta>>(Array.Empty<ResolverOverride>());
			DadosSistema._configuracaoClientService = container.Resolve<IConfiguracaoClientService>(Array.Empty<ResolverOverride>());
			DadosSistema._planoContaRepositoryRead = container.Resolve<IRepositoryTreeRead<DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PlanosConta.PlanoConta>>(Array.Empty<ResolverOverride>());
			DadosSistema._centroCustoRepositoryRead = container.Resolve<IRepositoryTreeRead<DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.CentrosCusto.CentroCusto>>(Array.Empty<ResolverOverride>());
			DadosSistema._tefService = container.Resolve<ITefService>(Array.Empty<ResolverOverride>());
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0017E799 File Offset: 0x0017C999
		public static void AdicionarModulosDependentes(IList<int> modulosAdicionais)
		{
			if (modulosAdicionais.Contains(162))
			{
				modulosAdicionais.Add(163);
				modulosAdicionais.Add(170);
			}
			if (modulosAdicionais.Contains(84))
			{
				modulosAdicionais.Add(175);
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x0017E7D3 File Offset: 0x0017C9D3
		// (set) Token: 0x060019E0 RID: 6624 RVA: 0x0017E7DA File Offset: 0x0017C9DA
		public static IAutenticacaoService AutenticacaoService { get; set; }

		// Token: 0x060019E1 RID: 6625 RVA: 0x0017E7E4 File Offset: 0x0017C9E4
		private static void PreencherConfiguracaoDto(IWService service)
		{
			try
			{
				DadosSistema._configuracaoDto = service.Canal<IConfiguracaoModuleService, ConfiguracaoDto>((IConfiguracaoModuleService x) => x.Obter());
				DadosSistema.IsMatrizFilial = service.Canal<IPessoaModuleService, bool>((IPessoaModuleService x) => x.IsMatrizFilial());
			}
			catch (Exception)
			{
				DadosSistema.PreencherConfiguracaoDtoBaseLocal();
			}
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0017E860 File Offset: 0x0017CA60
		private static void PreencherConfiguracaoDtoBaseLocal()
		{
			if (DadosSistema._sessao.IsNull())
			{
				return;
			}
			DadosSistema._configuracaoDto = DadosSistema._configuracaoRepositoryRead.Get(DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Id).ProjectedAsDto();
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0017E89C File Offset: 0x0017CA9C
		public static TResult ObterConfiguracao<T, TResult>() where TResult : class
		{
			return DadosSistema._configuracaoRepositoryRead.ObterConfiguracao<T, TResult>(DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Id);
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0017E8C1 File Offset: 0x0017CAC1
		public static DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.Contas.Conta ObterContaLivroCaixa()
		{
			return DadosSistema._contaRepositoryRead.ObterContaLivroCaixa();
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0017E8CD File Offset: 0x0017CACD
		public static IEnumerable<DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta> ObterRelacaoEspecieConta()
		{
			return DadosSistema._relacaoEspecieContaRepositoryRead.Find(EntityEmpresaSpecification<DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta>.AtivoPorEmpresa(DadosSistema.EmpresaLogada.Id));
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0017E8E8 File Offset: 0x0017CAE8
		public static IEnumerable<DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta> ValidarRelacaoEspecieConta(IEnumerable<DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta> relacoes)
		{
			IEnumerable<DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.Contas.Conta> contas = DadosSistema._contaRepositoryRead.GetByFields(new Expression<Func<DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.Contas.Conta, bool>>[]
			{
				(DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.Contas.Conta x) => x.Ativo
			});
			return relacoes.Where<DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta>((DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta relacaoEspecieConta) => contas.Any<DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.Contas.Conta>((DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.Contas.Conta x) => x.Id.Equals(relacaoEspecieConta.Conta.Id))).ToList<DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta>();
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x0017E964 File Offset: 0x0017CB64
		public static DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PlanosConta.PlanoConta ObterPlanoContaPontoDeVenda()
		{
			ConfiguracaoPlanoContaLocal configuracaoPlanoContaPontoDeVenda = DadosSistema._configuracaoClientService.ObterConfiguracao().ConfiguracaoPlanoContaPontoDeVenda;
			if (configuracaoPlanoContaPontoDeVenda.IsNotNull() && !string.IsNullOrEmpty(configuracaoPlanoContaPontoDeVenda.PlanoContaReferencia))
			{
				return DadosSistema._planoContaRepositoryRead.GetByCodigoUnico(configuracaoPlanoContaPontoDeVenda.PlanoContaReferencia).Obter(configuracaoPlanoContaPontoDeVenda.PlanoContaReferencia);
			}
			return DadosSistema.ObterConfiguracao<PlanoContaSaidaPadrao, DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PlanosConta.PlanoConta>();
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0017E9B8 File Offset: 0x0017CBB8
		public static DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.CentrosCusto.CentroCusto ObterCentroCustoPontoDeVenda()
		{
			ConfiguracaoCentroCustoLocal configuracaoCentroCustoPontoDeVenda = DadosSistema._configuracaoClientService.ObterConfiguracao().ConfiguracaoCentroCustoPontoDeVenda;
			if (configuracaoCentroCustoPontoDeVenda.IsNotNull() && !string.IsNullOrEmpty(configuracaoCentroCustoPontoDeVenda.CentroCustoReferencia))
			{
				return DadosSistema._centroCustoRepositoryRead.GetByCodigoUnico(configuracaoCentroCustoPontoDeVenda.CentroCustoReferencia).Obter(configuracaoCentroCustoPontoDeVenda.CentroCustoReferencia);
			}
			return DadosSistema.ObterConfiguracao<CentroCustoSaidaPadrao, DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.CentrosCusto.CentroCusto>();
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0017EA0C File Offset: 0x0017CC0C
		public static PlanoContaDto ObterPlanoContaSaidaLocal()
		{
			ConfiguracaoPlanoContaLocal configuracaoPlanoContaSaidaLocal = DadosSistema._configuracaoClientService.ObterConfiguracao().ConfiguracaoPlanoContaSaidaLocal;
			if (!configuracaoPlanoContaSaidaLocal.IsNotNull() || string.IsNullOrEmpty(configuracaoPlanoContaSaidaLocal.PlanoContaReferencia))
			{
				return DadosSistema.ObterConfiguracao<PlanoContaSaidaPadrao, DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PlanosConta.PlanoConta>().ProjectedAsDto();
			}
			DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PlanosConta.PlanoConta byCodigoUnico = DadosSistema._planoContaRepositoryRead.GetByCodigoUnico(configuracaoPlanoContaSaidaLocal.PlanoContaReferencia);
			return ((byCodigoUnico != null) ? byCodigoUnico.Obter(configuracaoPlanoContaSaidaLocal.PlanoContaReferencia).ProjectedAsDto() : null) ?? DadosSistema.ObterConfiguracao<PlanoContaSaidaPadrao, DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PlanosConta.PlanoConta>().ProjectedAsDto();
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x0017EA80 File Offset: 0x0017CC80
		public static CentroCustoDto ObterCentroCustoSaidaLocal()
		{
			ConfiguracaoCentroCustoLocal configuracaoCentroCustoSaidaLocal = DadosSistema._configuracaoClientService.ObterConfiguracao().ConfiguracaoCentroCustoSaidaLocal;
			if (!configuracaoCentroCustoSaidaLocal.IsNotNull() || string.IsNullOrEmpty(configuracaoCentroCustoSaidaLocal.CentroCustoReferencia))
			{
				return DadosSistema.ObterConfiguracao<CentroCustoSaidaPadrao, DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.CentrosCusto.CentroCusto>().ProjectedAsDto();
			}
			DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.CentrosCusto.CentroCusto byCodigoUnico = DadosSistema._centroCustoRepositoryRead.GetByCodigoUnico(configuracaoCentroCustoSaidaLocal.CentroCustoReferencia);
			return ((byCodigoUnico != null) ? byCodigoUnico.Obter(configuracaoCentroCustoSaidaLocal.CentroCustoReferencia).ProjectedAsDto() : null) ?? DadosSistema.ObterConfiguracao<CentroCustoSaidaPadrao, DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.CentrosCusto.CentroCusto>().ProjectedAsDto();
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0017EAF2 File Offset: 0x0017CCF2
		public static ConfiguracaoDepartamentoVenda ObterDepartamentoVenda()
		{
			return DadosSistema._configuracaoClientService.ObterConfiguracao().ConfiguracaoDepartamentoVenda ?? ConfiguracaoDepartamentoVenda.Criar();
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0017EB0C File Offset: 0x0017CD0C
		public static bool PodeAbrirPontoDeVenda()
		{
			return (DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.PontoDeVenda.Ativo || DadosSistema.PodeEmitirNFCe || DadosSistema.IsMEI) && DadosSistema._configuracaoClientService.ObterConfiguracao().IdEmpresaLocal == DadosSistema._sessao.Perfis.ItemSelecionado.Empresa.Id && DadosSistema._sessao.VerificarPermissao<DigisatServer.Infrastructure.Crosscutting.Module.PontoDeVenda, Visualizar>();
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x0017EB82 File Offset: 0x0017CD82
		public static void SetarWebServiceEnvioPafEcfServiceAtivo(bool webServiceAtivo)
		{
			DadosSistema._webServiceEnvioPafEcfServiceAtivo = webServiceAtivo;
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x0017EB8A File Offset: 0x0017CD8A
		public static void SetarConfiguracaoServidor(ConfiguracaoServidorDto configuracaoServidorDto)
		{
			DadosSistema._configuracaoServidorDto = configuracaoServidorDto;
			DadosSistema._eventPublisher.Publish<CarregouConfiguracaoServidorEvent>(new CarregouConfiguracaoServidorEvent());
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0017EBA1 File Offset: 0x0017CDA1
		public static string ObterVersaoSistema()
		{
			return InformacoesSistema.ObterVersaoSistema();
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0017EBA8 File Offset: 0x0017CDA8
		public static string ObterReleaseSistema()
		{
			return InformacoesSistema.ObterReleaseSistema();
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x0017EBAF File Offset: 0x0017CDAF
		public static bool PodeAbrirImportacaoMesaContaAoFinalizarDocumentoFiscal()
		{
			return DadosSistema.IsRestauranteOuHotel && DadosSistema._configuracaoClientService.ObterConfiguracao().AbrirImportacaoMesaContaAoFinalizarDocumentoFiscal;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0017EBC9 File Offset: 0x0017CDC9
		public static bool DeveSolicitarRestauranteDelivery()
		{
			return DadosSistema.IsRestauranteOuHotel && DadosSistema._configuracaoClientService.ObterConfiguracao().RestauranteSolicitaDelivery;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0017EBE3 File Offset: 0x0017CDE3
		public static bool DeveImprimirDemonstrativoConsumo()
		{
			return !DadosSistema.IsPaf && DadosSistema.IsRestauranteOuHotel && DadosSistema._configuracaoClientService.ObterConfiguracao().RestauranteImprimirDemonstrativoConsumo;
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x0017EC04 File Offset: 0x0017CE04
		public static bool DeveImprimirAmbienteProducaoPainelPedido
		{
			get
			{
				return DadosSistema.IsRestauranteOuHotel && (DadosSistema._configuracaoClientService.ObterConfiguracao().RestauranteImprimirAmbienteProducaoPainelPedido ?? true);
			}
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0017EC3C File Offset: 0x0017CE3C
		public static string DiretorioBackup()
		{
			return DadosSistema._configuracaoClientService.ObterConfiguracao().ObterDiretorioBackup();
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0017EC4D File Offset: 0x0017CE4D
		public static bool AbrirPontoDeVendaAoLogar()
		{
			return DadosSistema._configuracaoClientService.ObterConfiguracao().AbrirPontoDeVendaAoLogar;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0017EC5E File Offset: 0x0017CE5E
		public static bool AbrirAutoatendimentoBalancaAoLogar()
		{
			return DadosSistema._configuracaoClientService.ObterConfiguracao().AbrirAutoatendimentoBalancaAoLogar;
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x0017EC6F File Offset: 0x0017CE6F
		public static bool EsconderTecladoTouchScreen
		{
			get
			{
				return DadosSistema._configuracaoClientService.ObterConfiguracao().EsconderTecladoTouchScreen;
			}
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0017EC80 File Offset: 0x0017CE80
		public static ConfiguracaoFarmacia ObterConfiguracaoFarmacia()
		{
			return DadosSistema._configuracaoClientService.ObterConfiguracao().Farmacia ?? ConfiguracaoFarmacia.Criar();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0017EC9A File Offset: 0x0017CE9A
		public static IConfiguracaoClientService ObterConfiguracaoClient()
		{
			return DadosSistema._configuracaoClientService;
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0017ECA1 File Offset: 0x0017CEA1
		public static ITefService ObterTefService()
		{
			return DadosSistema._tefService;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0017ECA8 File Offset: 0x0017CEA8
		public static IShipayService ObterShipayService()
		{
			return DadosSistema._shipayService;
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x0017ECAF File Offset: 0x0017CEAF
		public static bool UtilizarShipayDePagar
		{
			get
			{
				return DadosSistema.ConfiguracaoDto.IsNotNull() && DadosSistema.ConfiguracaoDto.Geral.UtilizarShipayDePagar.Valor;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x0017ECD4 File Offset: 0x0017CED4
		public static bool ShipayConfigurado
		{
			get
			{
				ConfiguracaoDto configuracaoDto = DadosSistema.ConfiguracaoDto;
				GeralDto geralDto = ((configuracaoDto != null) ? configuracaoDto.Geral : null);
				if (geralDto.IsNull())
				{
					return false;
				}
				if (!DadosSistema.UtilizarShipayDePagar)
				{
					return !string.IsNullOrEmpty(geralDto.AccessKeyShipay.Valor) && !string.IsNullOrEmpty(geralDto.SecretKeyShipay.Valor);
				}
				return !string.IsNullOrEmpty(geralDto.EmailDePagar.Valor) && !string.IsNullOrEmpty(geralDto.TokenDePagar.Valor);
			}
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0017ED53 File Offset: 0x0017CF53
		public static void SetarTurnoAbertoDoUsuario(bool turnoAberto)
		{
			if (DadosSistema._configuracaoDto == null)
			{
				DadosSistema.PreencherConfiguracaoDtoBaseLocal();
			}
			DadosSistema.TurnoAberto = !DadosSistema._configuracaoDto.Geral.PermitirAcessoMovimentacaoFinanceiroSomenteComTurnoAberto.Valor || turnoAberto || DadosSistema.UsuarioNaoPrecisaAbrirTurno;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0017ED8C File Offset: 0x0017CF8C
		public static IWebProxy ObterProxy()
		{
			ConfiguracaoServidorDto configuracaoServidorDto = DadosSistema.ConfiguracaoServidorDto;
			if (configuracaoServidorDto.IsNull())
			{
				return null;
			}
			return Proxy.Criar().ComUsuario(configuracaoServidorDto.Geral.UsuarioProxy.Valor).ComSenha(configuracaoServidorDto.Geral.SenhaProxy.Valor)
				.ComHost(configuracaoServidorDto.Geral.EnderecoProxy.Valor)
				.ComPorta(configuracaoServidorDto.Geral.PortaProxy.Valor)
				.ObterProxy();
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0017EE07 File Offset: 0x0017D007
		public static bool ContemModuloAdicional(int moduloCodigo)
		{
			return DadosSistema.EmpresaLogada != null && DadosSistema.EmpresaLogada.ModulosAdicionais.Contains(Modulos.ObterPorCodigo(moduloCodigo).DescricaoDetalhada);
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0017EE2C File Offset: 0x0017D02C
		public static bool ModuloDisponivel(int moduloCodigo)
		{
			DigisatServer.Infrastructure.Crosscutting.Model.Sistemas.Sistema sistema = DadosSistema.Sistema;
			bool? flag;
			if (sistema == null)
			{
				flag = null;
			}
			else
			{
				IEnumerable<Restricao> restricoes = sistema.Restricoes;
				flag = ((restricoes != null) ? new bool?(restricoes.Any<Restricao>((Restricao x) => x.ModuloReferencia == moduloCodigo)) : null);
			}
			return (flag ?? false) || DadosSistema.ContemModuloAdicional(moduloCodigo);
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0017EEA7 File Offset: 0x0017D0A7
		public static bool IfoodDisponivel
		{
			get
			{
				return DadosSistema.ModuloDisponivel(113) && (DadosSistema.ModuloDisponivel(72) || DadosSistema.ModuloDisponivel(162));
			}
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0017EEC9 File Offset: 0x0017D0C9
		public static IWService GetService()
		{
			if (DadosSistema._service.IsNull())
			{
				throw new Exception("IWService indisponível. É necessário efetuar logon no servidor.");
			}
			return DadosSistema._service;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0017EEE7 File Offset: 0x0017D0E7
		public static IEventPublisher GetEventPublisher()
		{
			return DadosSistema._eventPublisher;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0017EEEE File Offset: 0x0017D0EE
		public static IBoletoProxyService GetBoletoProxyService()
		{
			return DadosSistema._boletoProxyService;
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0017EEF5 File Offset: 0x0017D0F5
		public static IMobileService GetMobileService()
		{
			return DadosSistema._mobileService;
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0017EEFC File Offset: 0x0017D0FC
		public static INotificacao GetNotificacao()
		{
			return DadosSistema._notificacao;
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0017EF03 File Offset: 0x0017D103
		public static IBalancaService GetBalancaService()
		{
			return DadosSistema._balancaService;
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0017EF0A File Offset: 0x0017D10A
		public static IShopeeApiService GetShopeeApiService()
		{
			return DadosSistema._shopeeApiService;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0017EF11 File Offset: 0x0017D111
		public static IImpressoraNaoFiscalService GetImpressoraNaoFiscalService()
		{
			return DadosSistema._impressoraNaoFiscalService;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0017EF18 File Offset: 0x0017D118
		public static IImpressaoGraficoService GetImpressaoGraficoService()
		{
			return DadosSistema._impressaoGraficoService;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0017EF1F File Offset: 0x0017D11F
		public static IImpressaoImagemItensMovimentacaoService GetImpressaoImagemItensMovimentacaoService()
		{
			return DadosSistema._impressaoImagemItensMovimentacaoService;
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0017EF26 File Offset: 0x0017D126
		public static IImpressaoImagemAnexaDavOsService GetImpressaoImagemAnexaDavOsService()
		{
			return DadosSistema._impressaoImagemAnexaDavOsService;
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x0017EF2D File Offset: 0x0017D12D
		public static IEnumerable<MoedaDto> Moedas
		{
			get
			{
				if (DadosSistema._moedas.IsNull())
				{
					DadosSistema.PreencherMoedas();
				}
				return DadosSistema._moedas;
			}
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0017EF48 File Offset: 0x0017D148
		public static bool PreencherMoedas()
		{
			bool flag;
			try
			{
				DadosSistema._moedas = DadosSistema._service.Canal<IMoedaModuleService, IEnumerable<MoedaDto>>((IMoedaModuleService x) => x.ObterAtivos());
				flag = true;
			}
			catch (Exception)
			{
				DadosSistema._moedas = Enumerable.Empty<MoedaDto>();
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0017EFA8 File Offset: 0x0017D1A8
		public static void PreencherMoedas(IEnumerable<MoedaDto> moedas)
		{
			DadosSistema._moedas = moedas;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0017EFB0 File Offset: 0x0017D1B0
		public static bool HabilitacaoLocalContemModuloPdvOffline(bool inicializacao = false)
		{
			DadosArquivoHabilitacao dadosArquivoHabilitacao = DadosSistema.ObterDadosArquivoHabilitacaoValidado();
			if (dadosArquivoHabilitacao == null)
			{
				return false;
			}
			bool flag = dadosArquivoHabilitacao.Modulos.Any<DigisatServer.Infrastructure.Crosscutting.Habilitacao.Modulo>((DigisatServer.Infrastructure.Crosscutting.Habilitacao.Modulo x) => x.Nome == "Ponto de venda Offline");
			if (!inicializacao)
			{
				List<string> list = new List<string>();
				list.AddRange(dadosArquivoHabilitacao.Filiais.Select<FilialXml, string>((FilialXml x) => x.Empresa.Cnpj));
				list.Add(dadosArquivoHabilitacao.Matriz.Cnpj);
				if (!list.Any<string>((string x) => x.Contains(DadosSistema.EmpresaLogada.CpfCnpjFormatado)))
				{
					return false;
				}
			}
			return flag;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0017F065 File Offset: 0x0017D265
		private static DadosArquivoHabilitacao ObterDadosArquivoHabilitacaoValidado()
		{
			return DadosSistema.ObterDadosArquivoHabilitacaoValidado(Path.Combine(ConstantesDeNomes.DiretorioSistema, "info.dat"));
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0017F07C File Offset: 0x0017D27C
		private static void AtualizarArquivoHabilitacaoLocal()
		{
			try
			{
				string text = DadosSistema._service.Canal<IInformacaoModuleService, string>((IInformacaoModuleService x) => x.ObterArquivoHabilitacao());
				if (!string.IsNullOrWhiteSpace(text))
				{
					string text2 = Path.Combine(ConstantesDeNomes.DiretorioSistema, "info.dat");
					if (!File.Exists(text2) || !File.ReadAllText(text2, Encoding.UTF8).Equals(text))
					{
						string text3 = text2 + ".novo";
						DadosSistema._fileIoWrapper.Save(text3, text);
						if (DadosSistema.ObterDadosArquivoHabilitacaoValidado(text3).IsNull())
						{
							DadosSistema._fileIoWrapper.Delete(text3);
						}
						else
						{
							File.Copy(text3, text2, true);
							DadosSistema._fileIoWrapper.Delete(text3);
						}
					}
				}
			}
			catch (Exception ex)
			{
				new GerarLog().GravarLog(string.Format("Erro ao atualizar o arquivo de habilitação local: {0}", ex.GetLastException().Message));
			}
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0017F168 File Offset: 0x0017D368
		private static DadosArquivoHabilitacao ObterDadosArquivoHabilitacaoValidado(string pathArquivoHabilitacao)
		{
			if (!File.Exists(pathArquivoHabilitacao))
			{
				return null;
			}
			IList<string> list = DadosSistema._fileIoWrapper.ReadTxt(pathArquivoHabilitacao, Encoding.UTF8);
			if (!AmbienteTeste.IsValid)
			{
				string text = DadosSistema.RemoverAssinatura(list);
				if (!DadosSistema._assinaturaWrapper.ValidarAssinaturaComChavePublica("-----BEGIN PUBLIC KEY-----\r\nMIGdMA0GCSqGSIb3DQEBAQUAA4GLADCBhwKBgQCrNawRNV3kiJ5ivnEEFOyMIiUO\r\nv+he2GtjRY8p3m7UepNwNeslK8RCfkcozGmyWV/RouD10vOjq0kDOmOQs01yLLHE\r\n9rcGmAPB0YgzKjgLSYtk/S0Hb76mgyBNodbm6Jqs2YVC2pqSudyngP2hVjyU127X\r\n8x3SKwpbMxbDPS9wmwIBAw==\r\n-----END PUBLIC KEY-----\r\n", text, list.ToArray<string>().LinhasParaTexto()))
				{
					return null;
				}
			}
			return DadosSistema._fileIoWrapper.ReadXml(pathArquivoHabilitacao, typeof(DadosArquivoHabilitacao)) as DadosArquivoHabilitacao;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0017F1D8 File Offset: 0x0017D3D8
		private static string RemoverAssinatura(ICollection<string> arquivo)
		{
			string text = arquivo.LastOrDefault<string>().ToStringOrEmpty();
			if (string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			arquivo.Remove(text);
			return text.Substring(4, text.Length - 7).Trim();
		}

		// Token: 0x04001B8C RID: 7052
		private static ISessao _sessao;

		// Token: 0x04001B8D RID: 7053
		private static ConfiguracaoDto _configuracaoDto;

		// Token: 0x04001B8E RID: 7054
		private static ConfiguracaoServidorDto _configuracaoServidorDto;

		// Token: 0x04001B8F RID: 7055
		private static IConfiguracaoRepositoryRead _configuracaoRepositoryRead;

		// Token: 0x04001B90 RID: 7056
		private static IContaRepositoryRead _contaRepositoryRead;

		// Token: 0x04001B91 RID: 7057
		private static IRepositoryEmpresaRead<DigisatServer.Domain.Main.Aggregates.CadastrosAgg.RelacoesEspecieConta.RelacaoEspecieConta> _relacaoEspecieContaRepositoryRead;

		// Token: 0x04001B92 RID: 7058
		private static IConfiguracaoClientService _configuracaoClientService;

		// Token: 0x04001B93 RID: 7059
		private static IUnityContainer _container;

		// Token: 0x04001B94 RID: 7060
		private static bool _webServiceEnvioPafEcfServiceAtivo;

		// Token: 0x04001B95 RID: 7061
		private static bool _bancoDadosConfigurado;

		// Token: 0x04001B96 RID: 7062
		private static bool _bloqueado;

		// Token: 0x04001B97 RID: 7063
		private static IWService _service;

		// Token: 0x04001B98 RID: 7064
		private static IEventPublisher _eventPublisher;

		// Token: 0x04001B99 RID: 7065
		private static IBalancaService _balancaService;

		// Token: 0x04001B9A RID: 7066
		private static IEnumerable<MoedaDto> _moedas;

		// Token: 0x04001B9B RID: 7067
		private static IRepositoryTreeRead<DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.PlanosConta.PlanoConta> _planoContaRepositoryRead;

		// Token: 0x04001B9C RID: 7068
		private static IRepositoryTreeRead<DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.CentrosCusto.CentroCusto> _centroCustoRepositoryRead;

		// Token: 0x04001B9D RID: 7069
		private static ITefService _tefService;

		// Token: 0x04001B9E RID: 7070
		private static IMobileService _mobileService;

		// Token: 0x04001B9F RID: 7071
		private static INotificacao _notificacao;

		// Token: 0x04001BA0 RID: 7072
		private static IFileIoWrapper _fileIoWrapper;

		// Token: 0x04001BA1 RID: 7073
		private static IAssinaturaWrapper _assinaturaWrapper;

		// Token: 0x04001BA2 RID: 7074
		private static IImpressoraNaoFiscalService _impressoraNaoFiscalService;

		// Token: 0x04001BA3 RID: 7075
		private static IImpressaoGraficoService _impressaoGraficoService;

		// Token: 0x04001BA4 RID: 7076
		private static IImpressaoImagemItensMovimentacaoService _impressaoImagemItensMovimentacaoService;

		// Token: 0x04001BA5 RID: 7077
		private static IImpressaoImagemAnexaDavOsService _impressaoImagemAnexaDavOsService;

		// Token: 0x04001BA6 RID: 7078
		private static IShipayService _shipayService;

		// Token: 0x04001BA7 RID: 7079
		private static IBoletoProxyService _boletoProxyService;

		// Token: 0x04001BA8 RID: 7080
		private static IShopeeApiService _shopeeApiService;

		// Token: 0x04001BAC RID: 7084
		public const string MensagemWhatsAppNaoConfigurado = "Informe o IP ou nome do servidor WhatsApp (WD Connect) em Configurações > Auxiliares para enviar mensagens pelo WhatsApp.";
	}
}
