using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using Newtonsoft.Json;


namespace MeContrata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }




        //Métodos para Calcular e mostrar idade

        private void DataSelecionada(object sender, SelectionChangedEventArgs e)
        {

            var DataNascimento = nascimento.SelectedDate.Value;
            var Idade = CalcularIdade(DataNascimento);

            if (nascimento.SelectedDate != null)
            {

                idade.Text = $"Idade: {Idade}";
            }
            if (Idade < 0)
            {
                idade.Text = "Escolha outra data";
                MessageBox.Show("Data de nascimento inválida!");

            }
        }
        private int CalcularIdade(DateTime DataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            if (DataNascimento > hoje.AddYears(-idade)) idade--;
            return idade;

        }


        // Métodos para o form de telefone
        public void TelefoneADD(object sender, RoutedEventArgs e)
        {
            string DDD = ddd.Text, Numero = numerotel.Text, Operadora = operadora.Text;
            string TelefoneCompleto = $"({DDD}){Numero} - {Operadora}";


            if (string.IsNullOrEmpty(ddd.Text) ||
             string.IsNullOrEmpty(numerotel.Text) ||
             string.IsNullOrEmpty(operadora.Text))
            {

                MessageBox.Show("Por favor, preencha todos os campos de telefone");


            }
            else
            {


                if (telefones.Items.Count < 3)
                {

                    telefones.Items.Add(TelefoneCompleto);

                }

                else
                {
                    MessageBox.Show("Limite de telefones atingido!");
                }
            }

        }
        private void ApagarTelefones(object sender, RoutedEventArgs e)
        {
            telefones.Items.Clear();
        }
        private void ApagarTel1(object sender, RoutedEventArgs e)
        {
            telefones.Items.RemoveAt(0);
        }

        private void ApagarTel2(object sender, RoutedEventArgs e)
        {
            telefones.Items.RemoveAt(1);
        }

        private void ApagarTel3(object sender, RoutedEventArgs e)
        {
            telefones.Items.RemoveAt(2);
        }


        //Métodos para o form de endereços
        private void EnderecoADD(object sender, RoutedEventArgs e)
        {
            string Logradouro = logradouro.Text, Numero = numero.Text, Complemento = complemento.Text, Bairro = bairro.Text, Cidade = cidade.Text, CEP = cep.Text;
            string EnderecoCompleto = $"{Logradouro},{Numero} - {Bairro}, {Cidade} - {CEP} (Complemento: {Complemento})";


            if (string.IsNullOrEmpty(logradouro.Text) ||
                string.IsNullOrEmpty(numero.Text) ||
                string.IsNullOrEmpty(complemento.Text) ||
                string.IsNullOrEmpty(bairro.Text) ||
                string.IsNullOrEmpty(cidade.Text) ||
                string.IsNullOrEmpty(cep.Text)
                )
            {
                MessageBox.Show("Por favor, preencha todos os campos de endereço.");

            }
            else
            {

                if (enderecos.Items.Count < 3)
                {
                    enderecos.Items.Add(EnderecoCompleto);
                }
                else
                {
                    MessageBox.Show("Limite de endereços atingido!");
                }

            }
        }


        private void ApagarEnderecos(object sender, RoutedEventArgs e)
        {
            enderecos.Items.Clear();
        }
        private void ApagarEnd1(object sender, RoutedEventArgs e)
        {
            enderecos.Items.RemoveAt(0);
        }

        private void ApagarEnd2(object sender, RoutedEventArgs e)
        {
            enderecos.Items.RemoveAt(1);
        }

        private void ApagarEnd3(object sender, RoutedEventArgs e)
        {
            enderecos.Items.RemoveAt(2);
        }

        //Método de validações genéricas para usar na tela de cadastro

        private bool Flag = false;

        private void IsNumber(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {

                e.Handled = true;
            }
        }

        private void IsNotEmpty(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;


            if (!Flag && string.IsNullOrEmpty(textBox.Text))
            {
                Flag = true;
                textBox.Focus();
                MessageBox.Show("Campo não pode ser vazio");
            }
        }


        // DB  -  Queries






        private void Submit(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=qweasdzxc123#@!;Database=MonsterCadastro;";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();


                    List<string> telefonesItem = new List<string>();
                    foreach (string telefone in telefones.Items)
                    {
                        telefonesItem.Add(telefone);
                    }

                    List<string> enderecosItem = new List<string>();
                    foreach (string endereco in enderecos.Items)
                    {
                        enderecosItem.Add(endereco);
                    }


                    if (string.IsNullOrWhiteSpace(nomecompleto.Text) ||
                          string.IsNullOrWhiteSpace(cpf.Text) ||
                          string.IsNullOrWhiteSpace(rg.Text) ||
                          string.IsNullOrWhiteSpace(sexo.Text) ||
                          string.IsNullOrWhiteSpace(escolaridade.Text) ||
                          string.IsNullOrWhiteSpace(profissao.Text) ||
                          nascimento.SelectedDate == null ||
                          telefonesItem.Count == 0 ||
                          enderecosItem.Count == 0)
                    {
                        MessageBox.Show("Por favor, preencha todos os campos antes de enviar o formulário.");
                    }


                    else
                    {
                        string query =
                        "INSERT INTO cliente_cadastro (nome_completo, rg, cpf, sexo, escolaridade, profissao, data_nascimento,telefone_1, telefone_2, telefone_3,endereco_1,endereco_2,endereco_3)" +
                        " VALUES (@nome_completo, @rg, @cpf, @sexo, @escolaridade, @profissao, @data_nascimento,@telefone_1, @telefone_2, @telefone_3,@endereco_1,@endereco_2,@endereco_3)";

                        NpgsqlCommand command = new NpgsqlCommand(query, connection);


                        command.Parameters.AddWithValue("@nome_completo", nomecompleto.Text);
                        command.Parameters.AddWithValue("@cpf", cpf.Text);
                        command.Parameters.AddWithValue("@rg", rg.Text);
                        command.Parameters.AddWithValue("@sexo", sexo.Text);
                        command.Parameters.AddWithValue("@escolaridade", escolaridade.Text);
                        command.Parameters.AddWithValue("@profissao", profissao.Text);
                        command.Parameters.AddWithValue("@data_nascimento", nascimento.SelectedDate);
                        command.Parameters.AddWithValue("@telefone_1", telefonesItem.Count > 0 ? telefonesItem[0] : "");
                        command.Parameters.AddWithValue("@telefone_2", telefonesItem.Count > 1 ? telefonesItem[1] : "");
                        command.Parameters.AddWithValue("@telefone_3", telefonesItem.Count > 2 ? telefonesItem[2] : "");
                        command.Parameters.AddWithValue("@endereco_1", enderecosItem.Count > 0 ? enderecosItem[0] : "");
                        command.Parameters.AddWithValue("@endereco_2", enderecosItem.Count > 1 ? enderecosItem[1] : "");
                        command.Parameters.AddWithValue("@endereco_3", enderecosItem.Count > 2 ? enderecosItem[2] : "");


                        command.ExecuteNonQuery();


                        MessageBox.Show("Cliente cadastrado com sucesso!");

                        var json = new
                        {
                            nome_completo = nomecompleto.Text,
                            cpf = cpf.Text,
                            rg = rg.Text,
                            sexo = sexo.Text,
                            escolaridade = escolaridade.Text,
                            profissao = profissao.Text,
                            data_nascimento = nascimento.SelectedDate,
                            telefones = telefones.Items.Cast<string>().ToList(),
                            enderecos = enderecos.Items.Cast<string>().ToList()
                        };


                        var jsonWindow = new JsonShow();
                        jsonWindow.JsonData(json);
                        jsonWindow.Show();


                    }
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ClientesCadastrados = new ClientesCadastrados();
            ClientesCadastrados.ShowTableDataGrid();
            ClientesCadastrados.Show();
        }

        private void Cancelar(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       
    }
}


    



