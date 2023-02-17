using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeContrata
{
    /// <summary>
    /// Lógica interna para Window1.xaml
    /// </summary>
    public partial class ClientesCadastrados : Window
    {
        public ClientesCadastrados()
        {
            InitializeComponent();

        }

        public void ShowTableDataGrid()
        {
            string connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=qweasdzxc123#@!;Database=MonsterCadastro;";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM cliente_cadastro", connection);

                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);

                DataTable dataTable = new DataTable();

                dataAdapter.Fill(dataTable);

                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }


    }
}
