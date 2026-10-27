using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Domain.Main.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios;
using DigisatServer.Infrastructure.Crosscutting.Enum;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using eqMJqmRMQJttOr7k5B;

namespace DigisatServer.Domain.Main.Services;

public class GeradorImpressoTurnoService : IGeradorImpressoTurnoService
{
	[CompilerGenerated]
	private sealed class <>c__DisplayClass6_0
	{
		public int espacamento;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public <>c__DisplayClass6_0()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			dqO6oocInm6pZ8RWPqaq();
			tb3oG5cIbDJqP7l5pBA6(this);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string <ObterRelatorioConferenciaFechamentoDeTurno>b__1(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return (string)VpjVqqcIUd37Bq8EN1AI("{0}{1}", D67qjrcIOAQ6NAHCaBLa(U7g2HxcIu6L9QqT7YtTk(x), Direcao.Direita, espacamento, ' '), D67qjrcIOAQ6NAHCaBLa(y20RRWcIYDRs5jxYXRsU(hHXkahcI90FQZGpRPAII(x), 2, 0, false), Direcao.Esquerda, espacamento, ' '));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string <ObterRelatorioConferenciaFechamentoDeTurno>b__3(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return (string)VpjVqqcIUd37Bq8EN1AI("{0}{1}", D67qjrcIOAQ6NAHCaBLa(U7g2HxcIu6L9QqT7YtTk(x), Direcao.Direita, espacamento, ' '), D67qjrcIOAQ6NAHCaBLa(y20RRWcIYDRs5jxYXRsU(hHXkahcI90FQZGpRPAII(x), 2, 0, false), Direcao.Esquerda, espacamento, ' '));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string <ObterRelatorioConferenciaFechamentoDeTurno>b__5(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return (string)VpjVqqcIUd37Bq8EN1AI("{0}{1}", D67qjrcIOAQ6NAHCaBLa(U7g2HxcIu6L9QqT7YtTk(x), Direcao.Direita, espacamento, ' '), D67qjrcIOAQ6NAHCaBLa(y20RRWcIYDRs5jxYXRsU(hHXkahcI90FQZGpRPAII(x), 2, 0, false), Direcao.Esquerda, espacamento, ' '));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string <ObterRelatorioConferenciaFechamentoDeTurno>b__7(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return (string)rJp7k8cIm3VpbjDvokmZ(VpjVqqcIUd37Bq8EN1AI("{0}{1}\r\n", D67qjrcIOAQ6NAHCaBLa(U7g2HxcIu6L9QqT7YtTk(x), Direcao.Direita, espacamento, ' '), D67qjrcIOAQ6NAHCaBLa(y20RRWcIYDRs5jxYXRsU(hHXkahcI90FQZGpRPAII(x), 2, 0, false), Direcao.Esquerda, espacamento, ' ')), MkBS3pcIWHjG0M36J3qx("{0}", D67qjrcIOAQ6NAHCaBLa(NAiw2IcIXXrWV45k63wh(x), Direcao.Direita, espacamento, ' ')));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void dqO6oocInm6pZ8RWPqaq()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			KSYJdK5yiJi5vVi4sK.icVKEWuHl8tvi();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void tb3oG5cIbDJqP7l5pBA6(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			P_0..ctor();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool FkFxSicIVB7iW7mB871J()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool hwCAricIhLw8XQPKHVsm()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object U7g2HxcIu6L9QqT7YtTk(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemDto)P_0).Descricao;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object D67qjrcIOAQ6NAHCaBLa(object P_0, Direcao P_1, int P_2, char P_3)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((string)P_0).PreencherComCaractere(P_1, P_2, P_3);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal hHXkahcI90FQZGpRPAII(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemDto)P_0).ValorCalculado;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object y20RRWcIYDRs5jxYXRsU(decimal P_0, int P_1, int P_2, bool P_3)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return P_0.FormatarCampoValor(P_1, P_2, P_3);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object VpjVqqcIUd37Bq8EN1AI(object P_0, object P_1, object P_2)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return string.Format((string)P_0, P_1, P_2);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object NAiw2IcIXXrWV45k63wh(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemDto)P_0).Observacao;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object MkBS3pcIWHjG0M36J3qx(object P_0, object P_1)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return string.Format((string)P_0, P_1);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object rJp7k8cIm3VpbjDvokmZ(object P_0, object P_1)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return (string)P_0 + (string)P_1;
		}
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class <>c
	{
		public static readonly <>c <>9;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_0;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_2;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_4;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_6;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_8;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, bool> <>9__6_9;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_10;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, bool> <>9__6_11;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_12;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_13;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_14;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, bool> <>9__6_15;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, bool> <>9__6_16;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_17;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_18;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_19;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_20;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__6_21;

		public static Func<DadosRelatorioConferenciaTurnoItemVendidoDto, decimal> <>9__6_22;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, bool> <>9__6_23;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, Tuple<int, string>> <>9__6_24;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, decimal> <>9__6_26;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, decimal> <>9__6_27;

		public static Func<Tuple<int, string>, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto>, <>f__AnonymousType82<int, string, decimal, decimal>> <>9__6_25;

		public static Func<<>f__AnonymousType82<int, string, decimal, decimal>, int> <>9__6_28;

		public static Func<<>f__AnonymousType82<int, string, decimal, decimal>, decimal> <>9__6_29;

		public static Func<<>f__AnonymousType82<int, string, decimal, decimal>, decimal> <>9__6_30;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, bool> <>9__6_31;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, Tuple<string>> <>9__6_32;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, decimal> <>9__6_34;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, decimal> <>9__6_35;

		public static Func<Tuple<string>, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto>, <>f__AnonymousType83<string, decimal, decimal>> <>9__6_33;

		public static Func<<>f__AnonymousType83<string, decimal, decimal>, decimal> <>9__6_36;

		public static Func<<>f__AnonymousType83<string, decimal, decimal>, decimal> <>9__6_37;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, bool> <>9__6_38;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, Tuple<string>> <>9__6_39;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, decimal> <>9__6_41;

		public static Func<DadosRelatorioConferenciaTurnoItemCombustiveDto, decimal> <>9__6_42;

		public static Func<Tuple<string>, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto>, <>f__AnonymousType83<string, decimal, decimal>> <>9__6_40;

		public static Func<<>f__AnonymousType83<string, decimal, decimal>, decimal> <>9__6_43;

		public static Func<<>f__AnonymousType83<string, decimal, decimal>, decimal> <>9__6_44;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__7_1;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__7_2;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__7_3;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__7_5;

		public static Func<DadosRelatorioConferenciaTurnoItemDto, decimal> <>9__7_7;

		public static Func<DadosRelatorioConferenciaTurnoVendaPrazoDto, decimal> <>9__7_8;

		public static Func<DadosRelatorioConferenciaTurnoItemVendidoDto, decimal> <>9__7_9;

		public static Func<DadosRelatorioConferenciaTurnoMovimentoContaDto, decimal> <>9__7_10;

		public static Func<DadosRelatorioConferenciaTurnoMovimentoContaDto, decimal> <>9__7_11;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static <>c()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			TfFMdFcIdj3CLfbiT3U0();
			<>9 = new <>c();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public <>c()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			TfFMdFcIdj3CLfbiT3U0();
			ILbW3PcIoWHOJJWn1frg(this);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_0(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_2(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_4(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_6(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_8(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool <ObterRelatorioConferenciaFechamentoDeTurno>b__6_9(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			if (hPHTgBcI38pwqnPNQfMx(aRW2KEcIaN2P3j8UsIjK(x), "Crédito"))
			{
				return !hPHTgBcI38pwqnPNQfMx(aRW2KEcIaN2P3j8UsIjK(x), "loja");
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_10(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool <ObterRelatorioConferenciaFechamentoDeTurno>b__6_11(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return hPHTgBcI38pwqnPNQfMx(aRW2KEcIaN2P3j8UsIjK(x), "Débito");
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_12(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_13(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_14(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool <ObterRelatorioConferenciaFechamentoDeTurno>b__6_15(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return gLUSLPcIKOA46TiOrS0D(EexWDicIP5d6CjGoIqIb(a), 0m);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool <ObterRelatorioConferenciaFechamentoDeTurno>b__6_16(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return gLUSLPcIKOA46TiOrS0D(EexWDicIP5d6CjGoIqIb(x), 0m);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_17(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return EexWDicIP5d6CjGoIqIb(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_18(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return EexWDicIP5d6CjGoIqIb(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_19(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return EexWDicIP5d6CjGoIqIb(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_20(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return EexWDicIP5d6CjGoIqIb(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_21(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return EexWDicIP5d6CjGoIqIb(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_22(DadosRelatorioConferenciaTurnoItemVendidoDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return t8W3LucI4lhr6gakUox2(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool <ObterRelatorioConferenciaFechamentoDeTurno>b__6_23(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return !LOiFBocIHi2Fd8sNgbF7(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Tuple<int, string> <ObterRelatorioConferenciaFechamentoDeTurno>b__6_24(DadosRelatorioConferenciaTurnoItemCombustiveDto item)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return new Tuple<int, string>(item.NumeroBico, item.Descricao);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal <>f__AnonymousType82<int, string, decimal, decimal> <ObterRelatorioConferenciaFechamentoDeTurno>b__6_25(Tuple<int, string> key, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto> group)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return new
			{
				NumeroBico = key.Item1,
				Descricao = key.Item2,
				Quantidade = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return WnahEfcICZQiC08qj6Zy(x);
				}),
				Valor = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return Na1wU6cIqLPS3QbXfj18(x);
				})
			};
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_26(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return WnahEfcICZQiC08qj6Zy(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_27(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return Na1wU6cIqLPS3QbXfj18(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal int <ObterRelatorioConferenciaFechamentoDeTurno>b__6_28(<>f__AnonymousType82<int, string, decimal, decimal> x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return x.NumeroBico;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_29(<>f__AnonymousType82<int, string, decimal, decimal> x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return x.Quantidade;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_30(<>f__AnonymousType82<int, string, decimal, decimal> x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return x.Valor;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool <ObterRelatorioConferenciaFechamentoDeTurno>b__6_31(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return !LOiFBocIHi2Fd8sNgbF7(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Tuple<string> <ObterRelatorioConferenciaFechamentoDeTurno>b__6_32(DadosRelatorioConferenciaTurnoItemCombustiveDto item)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return new Tuple<string>(item.Descricao);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal <>f__AnonymousType83<string, decimal, decimal> <ObterRelatorioConferenciaFechamentoDeTurno>b__6_33(Tuple<string> key, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto> group)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return new
			{
				Descricao = key.Item1,
				Quantidade = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return WnahEfcICZQiC08qj6Zy(x);
				}),
				Valor = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return Na1wU6cIqLPS3QbXfj18(x);
				})
			};
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_34(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return WnahEfcICZQiC08qj6Zy(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_35(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return Na1wU6cIqLPS3QbXfj18(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_36(<>f__AnonymousType83<string, decimal, decimal> x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return x.Quantidade;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_37(<>f__AnonymousType83<string, decimal, decimal> x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return x.Valor;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal bool <ObterRelatorioConferenciaFechamentoDeTurno>b__6_38(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return LOiFBocIHi2Fd8sNgbF7(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Tuple<string> <ObterRelatorioConferenciaFechamentoDeTurno>b__6_39(DadosRelatorioConferenciaTurnoItemCombustiveDto item)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return new Tuple<string>(item.Descricao);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal <>f__AnonymousType83<string, decimal, decimal> <ObterRelatorioConferenciaFechamentoDeTurno>b__6_40(Tuple<string> key, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto> group)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return new
			{
				Descricao = key.Item1,
				Quantidade = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return WnahEfcICZQiC08qj6Zy(x);
				}),
				Valor = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return Na1wU6cIqLPS3QbXfj18(x);
				})
			};
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_41(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return WnahEfcICZQiC08qj6Zy(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_42(DadosRelatorioConferenciaTurnoItemCombustiveDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return Na1wU6cIqLPS3QbXfj18(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_43(<>f__AnonymousType83<string, decimal, decimal> x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return x.Quantidade;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioConferenciaFechamentoDeTurno>b__6_44(<>f__AnonymousType83<string, decimal, decimal> x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return x.Valor;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_1(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_2(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_3(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_5(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_7(DadosRelatorioConferenciaTurnoItemDto a)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return E1UsYccIlDUUqmtjO0Ya(a);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_8(DadosRelatorioConferenciaTurnoVendaPrazoDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return UqKa8wcIZxX0Irl68BW2(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_9(DadosRelatorioConferenciaTurnoItemVendidoDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return t8W3LucI4lhr6gakUox2(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_10(DadosRelatorioConferenciaTurnoMovimentoContaDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return M8iEU3cIwOjdDCZ3OoYV(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal decimal <ObterRelatorioResumoFechamentoDeTurno>b__7_11(DadosRelatorioConferenciaTurnoMovimentoContaDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return M8iEU3cIwOjdDCZ3OoYV(x);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void TfFMdFcIdj3CLfbiT3U0()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			KSYJdK5yiJi5vVi4sK.icVKEWuHl8tvi();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool gcMZXccIkCpVWv1QUgqA()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool KyiI84cITrkN7OtnSHef()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ILbW3PcIoWHOJJWn1frg(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			P_0..ctor();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal E1UsYccIlDUUqmtjO0Ya(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemDto)P_0).ValorCalculado;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object aRW2KEcIaN2P3j8UsIjK(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemDto)P_0).Descricao;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool hPHTgBcI38pwqnPNQfMx(object P_0, object P_1)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((string)P_0).Contains((string)P_1);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal EexWDicIP5d6CjGoIqIb(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemDto)P_0).Diferenca;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool gLUSLPcIKOA46TiOrS0D(decimal P_0, decimal P_1)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return P_0 != P_1;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal t8W3LucI4lhr6gakUox2(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemVendidoDto)P_0).Valor;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool LOiFBocIHi2Fd8sNgbF7(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemCombustiveDto)P_0).Afericao;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal WnahEfcICZQiC08qj6Zy(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemCombustiveDto)P_0).Quantidade;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal Na1wU6cIqLPS3QbXfj18(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemCombustiveDto)P_0).Valor;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal UqKa8wcIZxX0Irl68BW2(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoVendaPrazoDto)P_0).Total;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal M8iEU3cIwOjdDCZ3OoYV(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoMovimentoContaDto)P_0).Valor;
		}
	}

	[CompilerGenerated]
	private sealed class <>c__DisplayClass7_0
	{
		public int espacamento;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public <>c__DisplayClass7_0()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			ovQlFQcIxtIvhGYXG1F5();
			wXILbicIGChZRbYZmC0s(this);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string <ObterRelatorioResumoFechamentoDeTurno>b__0(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return (string)vmOnD7ctvMsnOit2cBNp("{0}{1}", Irpq7LcIzHn0CNfN4rMI(a8ePnqcI030jNwtbaqgb(x), Direcao.Direita, espacamento, ' '), Irpq7LcIzHn0CNfN4rMI(z9YwDwctcF051DrJNqA3(c1gY3LctjrU67lAMwBgZ(x), 2, 0, false), Direcao.Esquerda, espacamento, ' '));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string <ObterRelatorioResumoFechamentoDeTurno>b__4(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return (string)vmOnD7ctvMsnOit2cBNp("{0}{1}", Irpq7LcIzHn0CNfN4rMI(a8ePnqcI030jNwtbaqgb(x), Direcao.Direita, espacamento, ' '), Irpq7LcIzHn0CNfN4rMI(z9YwDwctcF051DrJNqA3(c1gY3LctjrU67lAMwBgZ(x), 2, 0, false), Direcao.Esquerda, espacamento, ' '));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string <ObterRelatorioResumoFechamentoDeTurno>b__6(DadosRelatorioConferenciaTurnoItemDto x)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return (string)vmOnD7ctvMsnOit2cBNp("{0}{1}", Irpq7LcIzHn0CNfN4rMI(a8ePnqcI030jNwtbaqgb(x), Direcao.Direita, espacamento, ' '), Irpq7LcIzHn0CNfN4rMI(z9YwDwctcF051DrJNqA3(c1gY3LctjrU67lAMwBgZ(x), 2, 0, false), Direcao.Esquerda, espacamento, ' '));
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ovQlFQcIxtIvhGYXG1F5()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			KSYJdK5yiJi5vVi4sK.icVKEWuHl8tvi();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void wXILbicIGChZRbYZmC0s(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			P_0..ctor();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool Am7K5dcIeOYHVnxb5Mxb()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool vWKYudcI7TKqWacMGsid()
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object a8ePnqcI030jNwtbaqgb(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemDto)P_0).Descricao;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object Irpq7LcIzHn0CNfN4rMI(object P_0, Direcao P_1, int P_2, char P_3)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((string)P_0).PreencherComCaractere(P_1, P_2, P_3);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static decimal c1gY3LctjrU67lAMwBgZ(object P_0)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return ((DadosRelatorioConferenciaTurnoItemDto)P_0).ValorCalculado;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object z9YwDwctcF051DrJNqA3(decimal P_0, int P_1, int P_2, bool P_3)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return P_0.FormatarCampoValor(P_1, P_2, P_3);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object vmOnD7ctvMsnOit2cBNp(object P_0, object P_1, object P_2)
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return string.Format((string)P_0, P_1, P_2);
		}
	}

	private readonly bool yC2Cr9oDKH;

	private readonly string GXgCdUJOaC;

	private readonly string WRVC172d8v;

	private readonly string kebCp7pgWL;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GeradorImpressoTurnoService(bool isCinquentaESeisCinquentaESeteCinquentaEOitoMilimetros)
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		SBjqtMcIARV4MurPgyfS();
		int num = ((!NfTiHocIpH5S7JwCQcIC()) ? 1 : 3);
		while (true)
		{
			switch (num)
			{
			case 4:
				AVmqAccIsHwPYia6RPDL(this);
				goto default;
			default:
				yC2Cr9oDKH = isCinquentaESeisCinquentaESeteCinquentaEOitoMilimetros;
				num = 5;
				continue;
			case 0:
			case 3:
				KuPJFQcIJGrS7nmj8ovA();
				num = 4;
				if (0 == 0)
				{
					continue;
				}
				break;
			case 5:
				break;
			}
			break;
		}
		GXgCdUJOaC = (string)(isCinquentaESeisCinquentaESeteCinquentaEOitoMilimetros ? gZB2xocIDEWAQ18Fe3Z0(string.Empty, Direcao.Direita, 32, '-') : gZB2xocIDEWAQ18Fe3Z0(string.Empty, Direcao.Direita, 40, '-'));
		WRVC172d8v = (string)(isCinquentaESeisCinquentaESeteCinquentaEOitoMilimetros ? gZB2xocIDEWAQ18Fe3Z0(string.Empty, Direcao.Direita, 32, ' ') : gZB2xocIDEWAQ18Fe3Z0(string.Empty, Direcao.Direita, 40, ' '));
		kebCp7pgWL = (string)(isCinquentaESeisCinquentaESeteCinquentaEOitoMilimetros ? gZB2xocIDEWAQ18Fe3Z0(string.Empty, Direcao.Direita, 32, '*') : gZB2xocIDEWAQ18Fe3Z0(string.Empty, Direcao.Direita, 40, '*'));
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual IEnumerable<string> ObterRelatorioFechamentoDeTurno(TurnoDto turnoDto)
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		List<string> list = new List<string> { GXgCdUJOaC };
		if (yC2Cr9oDKH)
		{
			list.AddRange(new List<string>
			{
				"RESUMO INFORMADO PELO OPERADOR",
				GXgCdUJOaC,
				$"Usuario: {turnoDto.Usuario.Nome.RemoverAcentos().CopiarTexto(23)}",
				$"Abertura: {turnoDto.DataHoraAbertura:dd/MM/yyyy HH:mm:ss}",
				$"Fechamento: {turnoDto.Fechamento.DataHoraFechamento:dd/MM/yyyy HH:mm:ss}",
				GXgCdUJOaC,
				string.Format("{0}{1}", "Especie".PreencherComCaractere(Direcao.Direita, 16, ' '), "Valor(  R$)".PreencherComCaractere(Direcao.Esquerda, 16, ' ')),
				GXgCdUJOaC
			});
			if (turnoDto.Fechamento.TotalEspecie > 0m)
			{
				list.Add(string.Format("{0}{1}", "Dinheiro Caixa".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalEspecie.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalCartao > 0m)
			{
				list.Add(string.Format("{0}{1}", "Cartao".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalCartao.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalCheque > 0m)
			{
				list.Add(string.Format("{0}{1}", "Cheque".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalCheque.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalVale > 0m)
			{
				list.Add(string.Format("{0}{1}", "Vale".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalVale.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalValePresente > 0m)
			{
				list.Add(string.Format("{0}{1}", "Vale Presente".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalValePresente.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalCarteirasDigitais > 0m)
			{
				list.Add(string.Format("{0}{1}", "Carteiras Digitais".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalCarteirasDigitais.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalPrazo > 0m)
			{
				list.Add(string.Format("{0}{1}", "Prazo".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalPrazo.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalOutrasEspecies > 0m)
			{
				list.Add(string.Format("{0}{1}", "Outras espécies".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalOutrasEspecies.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalDepositoBancario > 0m)
			{
				list.Add(string.Format("{0}{1}", "Dep. bancário".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalDepositoBancario.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalTed > 0m)
			{
				list.Add(string.Format("{0}{1}", "TED".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalTed.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			if (turnoDto.Fechamento.TotalDoc > 0m)
			{
				list.Add(string.Format("{0}{1}", "DOC".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.TotalDoc.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, 16, ' '), turnoDto.Fechamento.Total.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')));
			int num = turnoDto.Fechamento.Observacao.Length;
			if (num > 0)
			{
				list.Add(GXgCdUJOaC);
				list.Add("OBSERVACAO");
				list.Add(GXgCdUJOaC);
				while (num > 0)
				{
					list.Add(turnoDto.Fechamento.Observacao.CopiarTexto(turnoDto.Fechamento.Observacao.Length - num, 32));
					num -= 32;
				}
			}
		}
		else
		{
			list.AddRange(new List<string>
			{
				"RESUMO DO TURNO INFORMADO PELO OPERADOR",
				GXgCdUJOaC,
				$"Usuario: {turnoDto.Usuario.Nome.RemoverAcentos().CopiarTexto(31)}",
				$"Abertura: {turnoDto.DataHoraAbertura:dd/MM/yyyy HH:mm:ss}",
				$"Fechamento: {turnoDto.Fechamento.DataHoraFechamento:dd/MM/yyyy HH:mm:ss}",
				GXgCdUJOaC,
				string.Format("{0}{1}", "Especie".PreencherComCaractere(Direcao.Direita, 20, ' '), "Valor(  R$)".PreencherComCaractere(Direcao.Esquerda, 20, ' ')),
				GXgCdUJOaC
			});
			if (turnoDto.Fechamento.TotalEspecie > 0m)
			{
				list.Add(string.Format("{0}{1}", "Dinheiro em Caixa".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalEspecie.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalCartao > 0m)
			{
				list.Add(string.Format("{0}{1}", "Cartao".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalCartao.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalCheque > 0m)
			{
				list.Add(string.Format("{0}{1}", "Cheque".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalCheque.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalVale > 0m)
			{
				list.Add(string.Format("{0}{1}", "Vale".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalVale.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalValePresente > 0m)
			{
				list.Add(string.Format("{0}{1}", "Vale Presente".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalValePresente.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalCarteirasDigitais > 0m)
			{
				list.Add(string.Format("{0}{1}", "Carteiras Digitais".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalCarteirasDigitais.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalPrazo > 0m)
			{
				list.Add(string.Format("{0}{1}", "Prazo".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalPrazo.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalOutrasEspecies > 0m)
			{
				list.Add(string.Format("{0}{1}", "Outras espécies".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalOutrasEspecies.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalDepositoBancario > 0m)
			{
				list.Add(string.Format("{0}{1}", "Depósito bancário".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalDepositoBancario.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalTed > 0m)
			{
				list.Add(string.Format("{0}{1}", "TED".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalTed.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			if (turnoDto.Fechamento.TotalDoc > 0m)
			{
				list.Add(string.Format("{0}{1}", "DOC".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.TotalDoc.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, 20, ' '), turnoDto.Fechamento.Total.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')));
			int num2 = turnoDto.Fechamento.Observacao.Length;
			if (num2 > 0)
			{
				list.Add(GXgCdUJOaC);
				list.Add("OBSERVACAO");
				list.Add(GXgCdUJOaC);
				while (num2 > 0)
				{
					list.Add(turnoDto.Fechamento.Observacao.CopiarTexto(turnoDto.Fechamento.Observacao.Length - num2, 40));
					num2 -= 40;
				}
			}
		}
		return list;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual IEnumerable<string> ObterRelatorioConferenciaFechamentoDeTurno(ConferenciaTurnoDto conferenciaTurno, TotalConvenioTurnoDto totalConvenioTurno, TotalCanceladasTurnoDto totalCanceladasTurno, bool imprimirItensVendidosFechamentoTurno)
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		<>c__DisplayClass6_0 CS$<>8__locals73 = new <>c__DisplayClass6_0();
		CS$<>8__locals73.espacamento = (yC2Cr9oDKH ? 16 : 20);
		List<string> list = (yC2Cr9oDKH ? new List<string> { kebCp7pgWL, "RESUMO PROCESSADO PELO SISTEMA", GXgCdUJOaC, "Suprimentos", GXgCdUJOaC } : new List<string> { kebCp7pgWL, "RESUMO DO CAIXA PROCESSADO PELO SISTEMA", GXgCdUJOaC, "Suprimentos", GXgCdUJOaC });
		if (conferenciaTurno.Suprimentos.Any())
		{
			foreach (DadosRelatorioConferenciaTurnoItemDto suprimento in conferenciaTurno.Suprimentos)
			{
				string arg = (suprimento.Observacao.IsNullOrEmpty() ? suprimento.Descricao.PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' ') : $"{suprimento.Descricao} ({suprimento.Observacao})".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '));
				list.Add($"{arg}" + $"{suprimento.ValorCalculado.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')}");
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.Suprimentos.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(a);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.Recebimentos.Any())
		{
			list.Add("Recebimentos realizados");
			list.Add(GXgCdUJOaC);
			list.AddRange(conferenciaTurno.Recebimentos.Select([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return (string)<>c__DisplayClass6_0.VpjVqqcIUd37Bq8EN1AI("{0}{1}", <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.U7g2HxcIu6L9QqT7YtTk(x), Direcao.Direita, CS$<>8__locals73.espacamento, ' '), <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.y20RRWcIYDRs5jxYXRsU(<>c__DisplayClass6_0.hHXkahcI90FQZGpRPAII(x), 2, 0, false), Direcao.Esquerda, CS$<>8__locals73.espacamento, ' '));
			}));
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.Recebimentos.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(a);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.Pagamentos.Any())
		{
			list.Add("Pagamentos realizados (Livro caixa)");
			list.Add(GXgCdUJOaC);
			list.AddRange(conferenciaTurno.Pagamentos.Select([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return (string)<>c__DisplayClass6_0.VpjVqqcIUd37Bq8EN1AI("{0}{1}", <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.U7g2HxcIu6L9QqT7YtTk(x), Direcao.Direita, CS$<>8__locals73.espacamento, ' '), <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.y20RRWcIYDRs5jxYXRsU(<>c__DisplayClass6_0.hHXkahcI90FQZGpRPAII(x), 2, 0, false), Direcao.Esquerda, CS$<>8__locals73.espacamento, ' '));
			}));
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.Pagamentos.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(a);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.PagamentosOutrasContas.Any())
		{
			list.Add("Pagamentos realizados (Outras contas)");
			list.Add(GXgCdUJOaC);
			list.AddRange(conferenciaTurno.PagamentosOutrasContas.Select([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return (string)<>c__DisplayClass6_0.VpjVqqcIUd37Bq8EN1AI("{0}{1}", <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.U7g2HxcIu6L9QqT7YtTk(x), Direcao.Direita, CS$<>8__locals73.espacamento, ' '), <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.y20RRWcIYDRs5jxYXRsU(<>c__DisplayClass6_0.hHXkahcI90FQZGpRPAII(x), 2, 0, false), Direcao.Esquerda, CS$<>8__locals73.espacamento, ' '));
			}));
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.PagamentosOutrasContas.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(a);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.Sangrias.Any())
		{
			list.Add("Sangrias");
			list.Add(GXgCdUJOaC);
			list.AddRange(conferenciaTurno.Sangrias.Select([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return (string)<>c__DisplayClass6_0.rJp7k8cIm3VpbjDvokmZ(<>c__DisplayClass6_0.VpjVqqcIUd37Bq8EN1AI("{0}{1}\r\n", <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.U7g2HxcIu6L9QqT7YtTk(x), Direcao.Direita, CS$<>8__locals73.espacamento, ' '), <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.y20RRWcIYDRs5jxYXRsU(<>c__DisplayClass6_0.hHXkahcI90FQZGpRPAII(x), 2, 0, false), Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')), <>c__DisplayClass6_0.MkBS3pcIWHjG0M36J3qx("{0}", <>c__DisplayClass6_0.D67qjrcIOAQ6NAHCaBLa(<>c__DisplayClass6_0.NAiw2IcIXXrWV45k63wh(x), Direcao.Direita, CS$<>8__locals73.espacamento, ' ')));
			}));
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.Sangrias.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(a);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (totalConvenioTurno.TotalDescontoConvenio > 0m)
		{
			list.Add("Total com Convênio");
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total Bruto".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), totalConvenioTurno.TotalBruto.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(string.Format("{0}{1}", "Total Desconto".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), totalConvenioTurno.TotalDescontoConvenio.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(string.Format("{0}{1}", "Total Líquido".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), totalConvenioTurno.TotalLiquido.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (totalCanceladasTurno.Quantidade > 0)
		{
			list.Add("Total de cancelamentos");
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Quantidade".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), totalCanceladasTurno.Quantidade.ToString().PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(string.Format("{0}{1}", "Total Bruto".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), totalCanceladasTurno.TotalBruto.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(string.Format("{0}{1}", "Total Líquido".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), totalCanceladasTurno.TotalLiquido.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.RecebimentosDeMovimentacoesAVista.Any())
		{
			list.Add("Vendido à vista por espécie");
			list.Add(GXgCdUJOaC);
			foreach (DadosRelatorioConferenciaTurnoItemDto recebimentosDeMovimentacoesAVistum in conferenciaTurno.RecebimentosDeMovimentacoesAVista)
			{
				list.Add($"{recebimentosDeMovimentacoesAVistum.Descricao.PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' ')}{recebimentosDeMovimentacoesAVistum.ValorCalculado.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')}");
			}
			list.Add(WRVC172d8v);
			list.Add(string.Format("{0}{1})", "(Subtotal Cartão Crédito".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.RecebimentosDeMovimentacoesAVista.Where([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.hPHTgBcI38pwqnPNQfMx(<>c.aRW2KEcIaN2P3j8UsIjK(x), "Crédito") && !<>c.hPHTgBcI38pwqnPNQfMx(<>c.aRW2KEcIaN2P3j8UsIjK(x), "loja");
			}).Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(x);
			}).FormatarCampoValor(2)
				.PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(string.Format("{0}{1})", "(Subtotal Cartão Débito".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.RecebimentosDeMovimentacoesAVista.Where([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.hPHTgBcI38pwqnPNQfMx(<>c.aRW2KEcIaN2P3j8UsIjK(x), "Débito");
			}).Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(x);
			}).FormatarCampoValor(2)
				.PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			if (conferenciaTurno.CarteiraDigitalPorTipo.Any())
			{
				foreach (DadosRelatorioConferenciaTurnoCarteiraDigitalDto item in conferenciaTurno.CarteiraDigitalPorTipo)
				{
					list.Add($"(Subtotal {item.Tipo.PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' ')}{item.Valor.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')})");
				}
			}
			list.Add(WRVC172d8v);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.RecebimentosDeMovimentacoesAVista.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(x);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.ParcelamentosReceber.Any())
		{
			list.Add("Prazo por espécie");
			list.Add(GXgCdUJOaC);
			foreach (DadosRelatorioConferenciaTurnoItemDto item2 in conferenciaTurno.ParcelamentosReceber)
			{
				list.Add($"{item2.Descricao.PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' ')}{item2.ValorCalculado.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')}");
			}
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.ParcelamentosReceber.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(x);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		list.Add(string.Format("{0}{1}", "TOTAL EM CAIXA".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.TotalEmCaixa.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
		list.Add(WRVC172d8v);
		if (conferenciaTurno.EspeciesRecebimento.Any([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return <>c.gLUSLPcIKOA46TiOrS0D(<>c.EexWDicIP5d6CjGoIqIb(a), 0m);
		}))
		{
			list.Add("(Informado - Calculado) = Resultado".PreencherComCaractere(Direcao.Esquerda, 32, ' '));
			foreach (DadosRelatorioConferenciaTurnoItemDto item3 in conferenciaTurno.EspeciesRecebimento.Where([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.gLUSLPcIKOA46TiOrS0D(<>c.EexWDicIP5d6CjGoIqIb(x), 0m);
			}))
			{
				list.Add($"{item3.Descricao.PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' ')}");
				list.Add($"{item3.DiferencaImpressoToText.PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' ')}");
				list.Add(GXgCdUJOaC);
			}
			list.Add(kebCp7pgWL);
			if (conferenciaTurno.EspeciesRecebimento.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.EexWDicIP5d6CjGoIqIb(a);
			}) > 0m)
			{
				list.Add(string.Format("{0}{1}", "Sobra".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.EspeciesRecebimento.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return <>c.EexWDicIP5d6CjGoIqIb(a);
				}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			}
			else if (conferenciaTurno.EspeciesRecebimento.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.EexWDicIP5d6CjGoIqIb(a);
			}) < 0m)
			{
				list.Add(string.Format("{0}{1}", "Falta".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), (conferenciaTurno.EspeciesRecebimento.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return <>c.EexWDicIP5d6CjGoIqIb(a);
				}) * -1m).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			}
			else
			{
				list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.EspeciesRecebimento.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return <>c.EexWDicIP5d6CjGoIqIb(a);
				}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			}
		}
		else
		{
			list.Add("Sem diferenca entre total informado e calculado");
		}
		if (conferenciaTurno.ItensVendidos.Any() & imprimirItensVendidosFechamentoTurno)
		{
			list.Add(WRVC172d8v);
			list.Add(kebCp7pgWL);
			list.Add("PRODUTOS VENDIDOS");
			list.Add(GXgCdUJOaC);
			list.Add(yC2Cr9oDKH ? "Cód.   Descricao    Qtde   Valor" : "Cód.   Descrição            Qtde   Valor");
			foreach (DadosRelatorioConferenciaTurnoItemVendidoDto itensVendido in conferenciaTurno.ItensVendidos)
			{
				if (yC2Cr9oDKH)
				{
					list.Add($"{itensVendido.Codigo.CopiarTextoDaDireitaParaEsquerda(6).PreencherComCaractere(Direcao.Direita, 6, ' ')} " + $"{itensVendido.Descricao.CopiarTexto(0, 9).PadRight(9)}" + $"{itensVendido.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{itensVendido.Valor.FormatarCampoValor(2).PadLeft(8)}");
					continue;
				}
				IEnumerable<string> enumerable = HelperString.QuebrarStringEmPartes(itensVendido.Descricao, 17);
				int num = 0;
				foreach (string item4 in enumerable)
				{
					if (num == 0)
					{
						list.Add($"{itensVendido.Codigo.CopiarTextoDaDireitaParaEsquerda(6).PreencherComCaractere(Direcao.Direita, 6, ' ')} " + $"{item4.PadRight(17)}" + $"{itensVendido.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{itensVendido.Valor.FormatarCampoValor(2).PadLeft(8)}");
					}
					else
					{
						list.Add(string.Format("{0}", "".PreencherComCaractere(Direcao.Direita, 7, ' ')) + $"{item4.PadRight(17)}");
					}
					num++;
				}
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), conferenciaTurno.ItensVendidos.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemVendidoDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.t8W3LucI4lhr6gakUox2(x);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
		}
		if (conferenciaTurno.Abastecimentos.Any())
		{
			list.Add(WRVC172d8v);
			list.Add(kebCp7pgWL);
			list.Add("VENDAS DE COMBUSTÍVEIS POR BICO");
			list.Add(GXgCdUJOaC);
			list.Add(yC2Cr9oDKH ? "Bico Combust.   Litragem   Valor" : "Bico Combustível        Litragem   Valor");
			var list2 = conferenciaTurno.Abastecimentos.Where([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return !<>c.LOiFBocIHi2Fd8sNgbF7(x);
			}).GroupBy([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto item) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return new Tuple<int, string>(item.NumeroBico, item.Descricao);
			}, [MethodImpl(MethodImplOptions.NoInlining)] (Tuple<int, string> key, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto> group) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return new
				{
					NumeroBico = key.Item1,
					Descricao = key.Item2,
					Quantidade = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
					{
						while (false)
						{
							_ = ((object[])null)[0];
						}
						return <>c.WnahEfcICZQiC08qj6Zy(x);
					}),
					Valor = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
					{
						while (false)
						{
							_ = ((object[])null)[0];
						}
						return <>c.Na1wU6cIqLPS3QbXfj18(x);
					})
				};
			}).OrderBy([MethodImpl(MethodImplOptions.NoInlining)] (x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return x.NumeroBico;
			})
				.ToList();
			foreach (var item5 in list2)
			{
				if (yC2Cr9oDKH)
				{
					list.Add($"{item5.NumeroBico:000} " + $"{item5.Descricao.CopiarTexto(0, 11).PadRight(12)}" + $"{item5.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{item5.Valor.FormatarCampoValor(2).PadLeft(8)}");
				}
				else
				{
					list.Add($"{item5.NumeroBico:000} " + $"{item5.Descricao.CopiarTexto(0, 19).PadRight(20)}" + $"{item5.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{item5.Valor.FormatarCampoValor(2).PadLeft(8)}");
				}
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total Litros".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), list2.Sum([MethodImpl(MethodImplOptions.NoInlining)] (x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return x.Quantidade;
			}).FormatarCampoValor(3).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(string.Format("{0}{1}", "Total Valores".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), list2.Sum([MethodImpl(MethodImplOptions.NoInlining)] (x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return x.Valor;
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
			list.Add(kebCp7pgWL);
			list.Add("VENDAS DE COMBUSTÍVEIS POR TIPO");
			list.Add(GXgCdUJOaC);
			list.Add(yC2Cr9oDKH ? "Combustível     Litragem   Valor" : "Combustível             Litragem   Valor");
			var list3 = conferenciaTurno.Abastecimentos.Where([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return !<>c.LOiFBocIHi2Fd8sNgbF7(x);
			}).GroupBy([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto item) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return new Tuple<string>(item.Descricao);
			}, [MethodImpl(MethodImplOptions.NoInlining)] (Tuple<string> key, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto> group) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return new
				{
					Descricao = key.Item1,
					Quantidade = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
					{
						while (false)
						{
							_ = ((object[])null)[0];
						}
						return <>c.WnahEfcICZQiC08qj6Zy(x);
					}),
					Valor = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
					{
						while (false)
						{
							_ = ((object[])null)[0];
						}
						return <>c.Na1wU6cIqLPS3QbXfj18(x);
					})
				};
			}).ToList();
			foreach (var item6 in list3)
			{
				if (yC2Cr9oDKH)
				{
					list.Add($"{item6.Descricao.CopiarTexto(0, 15).PadRight(16)}" + $"{item6.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{item6.Valor.FormatarCampoValor(2).PadLeft(8)}");
				}
				else
				{
					list.Add($"{item6.Descricao.CopiarTexto(0, 23).PadRight(24)}" + $"{item6.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{item6.Valor.FormatarCampoValor(2).PadLeft(8)}");
				}
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total Litros".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), list3.Sum([MethodImpl(MethodImplOptions.NoInlining)] (x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return x.Quantidade;
			}).FormatarCampoValor(3).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(string.Format("{0}{1}", "Total Valores".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), list3.Sum([MethodImpl(MethodImplOptions.NoInlining)] (x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return x.Valor;
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			list.Add(WRVC172d8v);
			list.Add(kebCp7pgWL);
			list.Add("AFERIÇÕES");
			list.Add(GXgCdUJOaC);
			list.Add(yC2Cr9oDKH ? "Combustível     Litragem   Valor" : "Combustível             Litragem   Valor");
			var list4 = conferenciaTurno.Abastecimentos.Where([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.LOiFBocIHi2Fd8sNgbF7(x);
			}).GroupBy([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto item) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return new Tuple<string>(item.Descricao);
			}, [MethodImpl(MethodImplOptions.NoInlining)] (Tuple<string> key, IEnumerable<DadosRelatorioConferenciaTurnoItemCombustiveDto> group) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return new
				{
					Descricao = key.Item1,
					Quantidade = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
					{
						while (false)
						{
							_ = ((object[])null)[0];
						}
						return <>c.WnahEfcICZQiC08qj6Zy(x);
					}),
					Valor = group.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemCombustiveDto x) =>
					{
						while (false)
						{
							_ = ((object[])null)[0];
						}
						return <>c.Na1wU6cIqLPS3QbXfj18(x);
					})
				};
			}).ToList();
			foreach (var item7 in list4)
			{
				if (yC2Cr9oDKH)
				{
					list.Add($"{item7.Descricao.CopiarTexto(0, 15).PadRight(16)}" + $"{item7.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{item7.Valor.FormatarCampoValor(2).PadLeft(8)}");
				}
				else
				{
					list.Add($"{item7.Descricao.CopiarTexto(0, 23).PadRight(24)}" + $"{item7.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{item7.Valor.FormatarCampoValor(2).PadLeft(8)}");
				}
				list.Add(GXgCdUJOaC);
				list.Add(string.Format("{0}{1}", "Total Litros".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), list4.Sum([MethodImpl(MethodImplOptions.NoInlining)] (x) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return x.Quantidade;
				}).FormatarCampoValor(3).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
				list.Add(string.Format("{0}{1}", "Total Valores".PreencherComCaractere(Direcao.Direita, CS$<>8__locals73.espacamento, ' '), list4.Sum([MethodImpl(MethodImplOptions.NoInlining)] (x) =>
				{
					while (false)
					{
						_ = ((object[])null)[0];
					}
					return x.Valor;
				}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals73.espacamento, ' ')));
			}
		}
		if (conferenciaTurno.EncerrantesToText.Any())
		{
			list.Add(WRVC172d8v);
			list.Add(kebCp7pgWL);
			list.AddRange(conferenciaTurno.EncerrantesToText);
		}
		return list;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual IEnumerable<string> ObterRelatorioResumoFechamentoDeTurno(TurnoDto turnoDto, ConferenciaTurnoSinteticoDto conferenciaTurno, bool imprimirItens, bool imprimirMovimentacoesPrazo, bool imprimirLancamentosManuais)
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		<>c__DisplayClass7_0 CS$<>8__locals42 = new <>c__DisplayClass7_0();
		CS$<>8__locals42.espacamento = (yC2Cr9oDKH ? 16 : 20);
		List<string> list = new List<string> { kebCp7pgWL, "RESUMO DE TURNO", GXgCdUJOaC };
		list.AddRange(new List<string>
		{
			$"Usuario: {turnoDto.Usuario.Nome.RemoverAcentos().CopiarTexto(23)}",
			$"Abertura: {turnoDto.DataHoraAbertura:dd/MM/yyyy HH:mm:ss}",
			$"Fechamento: {turnoDto.Fechamento.DataHoraFechamento:dd/MM/yyyy HH:mm:ss}",
			GXgCdUJOaC
		});
		if (conferenciaTurno.TotalCanceladasTurno.Quantidade > 0)
		{
			list.Add("Total de cancelamentos");
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Quantidade".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.TotalCanceladasTurno.Quantidade.ToString().PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(string.Format("{0}{1}", "Total Bruto".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.TotalCanceladasTurno.TotalBruto.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(string.Format("{0}{1}", "Total Líquido".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.TotalCanceladasTurno.TotalLiquido.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.TotalDescontoAplicado.TotalDesconto > 0m)
		{
			list.Add("Total de descontos");
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.TotalDescontoAplicado.TotalDesconto.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.TotalOutrasDespesasAplicado.TotalOutrasDespesas > 0m)
		{
			list.Add("Total de outras despesas (acréscimos)");
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.TotalOutrasDespesasAplicado.TotalOutrasDespesas.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.MovimentacoesPorTipo.Any())
		{
			list.Add("Total das vendas por origem");
			list.Add(GXgCdUJOaC);
			list.AddRange(conferenciaTurno.MovimentacoesPorTipo.Select([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return (string)<>c__DisplayClass7_0.vmOnD7ctvMsnOit2cBNp("{0}{1}", <>c__DisplayClass7_0.Irpq7LcIzHn0CNfN4rMI(<>c__DisplayClass7_0.a8ePnqcI030jNwtbaqgb(x), Direcao.Direita, CS$<>8__locals42.espacamento, ' '), <>c__DisplayClass7_0.Irpq7LcIzHn0CNfN4rMI(<>c__DisplayClass7_0.z9YwDwctcF051DrJNqA3(<>c__DisplayClass7_0.c1gY3LctjrU67lAMwBgZ(x), 2, 0, false), Direcao.Esquerda, CS$<>8__locals42.espacamento, ' '));
			}));
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.MovimentacoesPorTipo.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(a);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.RecebimentosDeMovimentacoesAVista.Any())
		{
			list.Add("Vendido à vista");
			list.Add(GXgCdUJOaC);
			foreach (DadosRelatorioConferenciaTurnoItemDto recebimentosDeMovimentacoesAVistum in conferenciaTurno.RecebimentosDeMovimentacoesAVista)
			{
				list.Add($"{recebimentosDeMovimentacoesAVistum.Descricao.PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' ')}{recebimentosDeMovimentacoesAVistum.ValorCalculado.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')}");
			}
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.RecebimentosDeMovimentacoesAVista.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(x);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.ParcelamentosReceber.Any())
		{
			list.Add("Vendido a prazo");
			list.Add(GXgCdUJOaC);
			foreach (DadosRelatorioConferenciaTurnoItemDto item in conferenciaTurno.ParcelamentosReceber)
			{
				list.Add($"{item.Descricao.PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' ')}{item.ValorCalculado.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')}");
			}
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.ParcelamentosReceber.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(x);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.Recebimentos.Any())
		{
			list.Add("Contas recebidas");
			list.Add(GXgCdUJOaC);
			list.AddRange(conferenciaTurno.Recebimentos.Select([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return (string)<>c__DisplayClass7_0.vmOnD7ctvMsnOit2cBNp("{0}{1}", <>c__DisplayClass7_0.Irpq7LcIzHn0CNfN4rMI(<>c__DisplayClass7_0.a8ePnqcI030jNwtbaqgb(x), Direcao.Direita, CS$<>8__locals42.espacamento, ' '), <>c__DisplayClass7_0.Irpq7LcIzHn0CNfN4rMI(<>c__DisplayClass7_0.z9YwDwctcF051DrJNqA3(<>c__DisplayClass7_0.c1gY3LctjrU67lAMwBgZ(x), 2, 0, false), Direcao.Esquerda, CS$<>8__locals42.espacamento, ' '));
			}));
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.Recebimentos.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(a);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.Pagamentos.Any())
		{
			list.Add("Contas pagas");
			list.Add(GXgCdUJOaC);
			list.AddRange(conferenciaTurno.Pagamentos.Select([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return (string)<>c__DisplayClass7_0.vmOnD7ctvMsnOit2cBNp("{0}{1}", <>c__DisplayClass7_0.Irpq7LcIzHn0CNfN4rMI(<>c__DisplayClass7_0.a8ePnqcI030jNwtbaqgb(x), Direcao.Direita, CS$<>8__locals42.espacamento, ' '), <>c__DisplayClass7_0.Irpq7LcIzHn0CNfN4rMI(<>c__DisplayClass7_0.z9YwDwctcF051DrJNqA3(<>c__DisplayClass7_0.c1gY3LctjrU67lAMwBgZ(x), 2, 0, false), Direcao.Esquerda, CS$<>8__locals42.espacamento, ' '));
			}));
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.Pagamentos.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemDto a) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.E1UsYccIlDUUqmtjO0Ya(a);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
			list.Add(WRVC172d8v);
		}
		if (conferenciaTurno.MovimentacoesAPrazo.Any() & imprimirMovimentacoesPrazo)
		{
			list.Add(WRVC172d8v);
			list.Add(kebCp7pgWL);
			list.Add("CLIENTES QUE COMPRARAM A PRAZO");
			list.Add(GXgCdUJOaC);
			list.Add(yC2Cr9oDKH ? "Número Pessoa              Parc." : "Número Pessoa                      Parc.");
			foreach (DadosRelatorioConferenciaTurnoVendaPrazoDto item2 in conferenciaTurno.MovimentacoesAPrazo)
			{
				if (yC2Cr9oDKH)
				{
					list.Add($"{item2.Numero:000000} " + $"{item2.Pessoa.CopiarTexto(0, 19).PadRight(19)}" + $"{item2.NumeroParcelas.ToString().PreencherComCaractere(Direcao.Esquerda, 4, ' ')}");
				}
				else
				{
					list.Add($"{item2.Numero:000000} " + $"{item2.Pessoa.CopiarTexto(0, 27).PadRight(27)}" + $"{item2.NumeroParcelas.ToString().PreencherComCaractere(Direcao.Esquerda, 4, ' ')}");
				}
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.MovimentacoesAPrazo.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoVendaPrazoDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.UqKa8wcIZxX0Irl68BW2(x);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
		}
		if (conferenciaTurno.ItensVendidos.Any() & imprimirItens)
		{
			list.Add(WRVC172d8v);
			list.Add(kebCp7pgWL);
			list.Add("PRODUTOS VENDIDOS");
			list.Add(GXgCdUJOaC);
			list.Add(yC2Cr9oDKH ? "Cód.   Descricao    Qtde   Valor" : "Cód.   Descrição            Qtde   Valor");
			foreach (DadosRelatorioConferenciaTurnoItemVendidoDto itensVendido in conferenciaTurno.ItensVendidos)
			{
				if (yC2Cr9oDKH)
				{
					list.Add($"{itensVendido.Codigo.CopiarTextoDaDireitaParaEsquerda(6).PreencherComCaractere(Direcao.Direita, 6, ' ')} " + $"{itensVendido.Descricao.CopiarTexto(0, 9).PadRight(9)}" + $"{itensVendido.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{itensVendido.Valor.FormatarCampoValor(2).PadLeft(8)}");
					continue;
				}
				IEnumerable<string> enumerable = HelperString.QuebrarStringEmPartes(itensVendido.Descricao, 17);
				int num = 0;
				foreach (string item3 in enumerable)
				{
					if (num == 0)
					{
						list.Add($"{itensVendido.Codigo.CopiarTextoDaDireitaParaEsquerda(6).PreencherComCaractere(Direcao.Direita, 6, ' ')} " + $"{item3.PadRight(17)}" + $"{itensVendido.Quantidade.FormatarCampoValor(3).PadLeft(8)}" + $"{itensVendido.Valor.FormatarCampoValor(2).PadLeft(8)}");
					}
					else
					{
						list.Add(string.Format("{0}", "".PreencherComCaractere(Direcao.Direita, 7, ' ')) + $"{item3.PadRight(17)}");
					}
					num++;
				}
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.ItensVendidos.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoItemVendidoDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.t8W3LucI4lhr6gakUox2(x);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
		}
		if (conferenciaTurno.LancamentosManuais.Any() & imprimirLancamentosManuais)
		{
			list.Add(WRVC172d8v);
			list.Add(kebCp7pgWL);
			list.Add("LANC. MANUAIS NO CAIXA");
			list.Add(GXgCdUJOaC);
			list.Add(yC2Cr9oDKH ? "Hora  Descricao            Valor" : "Hora  Descricao                    Valor");
			foreach (DadosRelatorioConferenciaTurnoMovimentoContaDto lancamentosManuai in conferenciaTurno.LancamentosManuais)
			{
				if (yC2Cr9oDKH)
				{
					list.Add($"{lancamentosManuai.DataHoraCompetencia:HH:mm} " + $"{lancamentosManuai.Descricao.CopiarTexto(0, 16).PadRight(17)}" + $"{lancamentosManuai.Valor.FormatarCampoValor(2).PadLeft(6)}");
				}
				else
				{
					list.Add($"{lancamentosManuai.DataHoraCompetencia:HH:mm} " + $"{lancamentosManuai.Descricao.CopiarTexto(0, 24).PadRight(25)}" + $"{lancamentosManuai.Valor.FormatarCampoValor(2).PadLeft(6)}");
				}
			}
			list.Add(GXgCdUJOaC);
			list.Add(string.Format("{0}{1}", "Total".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' '), conferenciaTurno.LancamentosManuais.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoMovimentoContaDto x) =>
			{
				while (false)
				{
					_ = ((object[])null)[0];
				}
				return <>c.M8iEU3cIwOjdDCZ3OoYV(x);
			}).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')));
		}
		list.Add(WRVC172d8v);
		list.Add(kebCp7pgWL);
		list.Add(string.Format("{0}", "TOTAL*".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' ')) + (imprimirLancamentosManuais ? $"{conferenciaTurno.TotalEmCaixa.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')}" : $"{(conferenciaTurno.TotalEmCaixa - conferenciaTurno.LancamentosManuais.Sum([MethodImpl(MethodImplOptions.NoInlining)] (DadosRelatorioConferenciaTurnoMovimentoContaDto x) =>
		{
			while (false)
			{
				_ = ((object[])null)[0];
			}
			return <>c.M8iEU3cIwOjdDCZ3OoYV(x);
		})).FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, CS$<>8__locals42.espacamento, ' ')}"));
		list.Add(imprimirLancamentosManuais ? string.Format("{0}", "*A vista+Manual+Recebidas-Pagas".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' ')) : string.Format("{0}", "*A vista+Recebidas-Pagas".PreencherComCaractere(Direcao.Direita, CS$<>8__locals42.espacamento, ' ')));
		list.Add(WRVC172d8v);
		return list;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual IEnumerable<string> RetornarImpressoSuprimentoSangria(decimal valor, string titulo, string observacao, string nomeUsuario)
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		List<string> list = new List<string> { kebCp7pgWL };
		if (yC2Cr9oDKH)
		{
			list.AddRange(new List<string>
			{
				titulo,
				GXgCdUJOaC,
				$"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
				$"Usuario: {nomeUsuario.RemoverAcentos().CopiarTexto(23)}",
				GXgCdUJOaC,
				string.Format("{0}{1}", "VALOR".PreencherComCaractere(Direcao.Direita, 16, ' '), valor.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 16, ' ')),
				GXgCdUJOaC,
				(!string.IsNullOrEmpty(observacao)) ? $"Observacao: {observacao.RemoverAcentos()}" : string.Empty,
				kebCp7pgWL
			});
		}
		else
		{
			list.AddRange(new List<string>
			{
				titulo,
				GXgCdUJOaC,
				$"Data/Hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
				$"Usuario: {nomeUsuario.RemoverAcentos().CopiarTexto(31)}",
				GXgCdUJOaC,
				string.Format("{0}{1}", "VALOR".PreencherComCaractere(Direcao.Direita, 20, ' '), valor.FormatarCampoValor(2).PreencherComCaractere(Direcao.Esquerda, 20, ' ')),
				GXgCdUJOaC,
				(!string.IsNullOrEmpty(observacao)) ? $"Observacao: {observacao.RemoverAcentos()}" : string.Empty,
				kebCp7pgWL
			});
		}
		return list;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GeradorImpressoTurnoService()
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		KuPJFQcIJGrS7nmj8ovA();
		AVmqAccIsHwPYia6RPDL(this);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void KuPJFQcIJGrS7nmj8ovA()
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		KSYJdK5yiJi5vVi4sK.icVKEWuHl8tvi();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void AVmqAccIsHwPYia6RPDL(object P_0)
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		P_0..ctor();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static object gZB2xocIDEWAQ18Fe3Z0(object P_0, Direcao P_1, int P_2, char P_3)
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		return ((string)P_0).PreencherComCaractere(P_1, P_2, P_3);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool NfTiHocIpH5S7JwCQcIC()
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool SBjqtMcIARV4MurPgyfS()
	{
		while (false)
		{
			_ = ((object[])null)[0];
		}
		return false;
	}
}
