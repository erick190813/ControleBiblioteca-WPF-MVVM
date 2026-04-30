using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleBiblioteca.Wpf.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace ControleBiblioteca.Wpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<EmprestimoModel>? _emprestimos;

        [ObservableProperty]
        private EmprestimoModel? _emprestimoSelecionado;

        [ObservableProperty]
        private int _novoLivroId;

        [ObservableProperty]
        private int _novoUsuarioId;

        public MainViewModel()
        {
            _apiService = new ApiService();
            CarregarDadosCommand.Execute(null);
        }

        [RelayCommand]
        private async Task CarregarDadosAsync()
        {
            try
            {
                Emprestimos = await _apiService.ObterEmprestimosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar com a API: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task RegistrarEmprestimoAsync()
        {
            if (NovoLivroId <= 0 || NovoUsuarioId <= 0)
            {
                MessageBox.Show("Informe IDs válidos para Livro e Usuário.", "Aviso");
                return;
            }

            var sucesso = await _apiService.RegistrarEmprestimoAsync(NovoLivroId, NovoUsuarioId);
            if (sucesso)
            {
                NovoLivroId = 0;
                NovoUsuarioId = 0;
                await CarregarDadosAsync();
                MessageBox.Show("Empréstimo registrado com sucesso!", "Sucesso");
            }
            else
            {
                MessageBox.Show("Falha ao registrar empréstimo. Verifique o estoque do livro.", "Erro");
            }
        }

        [RelayCommand]
        private async Task DevolverLivroAsync()
        {
            if (EmprestimoSelecionado == null)
            {
                MessageBox.Show("Selecione um empréstimo na lista para devolver.", "Aviso");
                return;
            }

            if (EmprestimoSelecionado.StatusEmprestimo == "Devolvido")
            {
                MessageBox.Show("Este livro já foi devolvido.", "Aviso");
                return;
            }

            var sucesso = await _apiService.DevolverLivroAsync(EmprestimoSelecionado.Id);
            if (sucesso)
            {
                await CarregarDadosAsync();
                MessageBox.Show("Livro devolvido com sucesso!", "Sucesso");
            }
        }
    }
}