using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioGastos : ContentPage
{
    public ObservableCollection<GastoCategoria> RelatorioCategorias { get; set; } = new ObservableCollection<GastoCategoria>();

    public RelatorioGastos()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GerarRelatorio();
    }

    private async Task GerarRelatorio()
    {
        RelatorioCategorias.Clear();
        var produtos = await App.Db.GetAll();

        var gastosPorCategoria = produtos
            .GroupBy(p => p.Categoria)
            .Select(g => new GastoCategoria
            {
                Categoria = g.Key,
                TotalGasto = g.Sum(p => p.Total)
            })
            .OrderByDescending(r => r.TotalGasto)
            .ToList();

        gastosPorCategoria.ForEach(r => RelatorioCategorias.Add(r));

        double totalGeral = produtos.Sum(p => p.Total);
        lbl_total_geral.Text = $"Total Geral: {totalGeral:C}";
    }
}

public class GastoCategoria
{
    public string Categoria { get; set; }
    public double TotalGasto { get; set; }
}
