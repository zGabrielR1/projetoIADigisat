// DigisatServer.Application.Main.AuxiliaresModulo.Services.MovimentacaoSaidaRelatorioAppService.<>c__DisplayClass18_1
// Token: 0x0600328A RID: 12938 RVA: 0x00140F24 File Offset: 0x0013F124
internal decimal[] <ObterTotalMovimentacoesPorMesPorVendedor>b__4(string key, decimal[] existing)
{
	for (int i = 0; i < 12; i++)
	{
		existing[i] += this.kvp.Value[i];
	}
	return existing;
}
