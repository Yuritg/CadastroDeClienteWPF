using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Lógica interna para JsonShow.xaml
    /// </summary>
    public partial class JsonShow : Window
    {
        public JsonShow()
        {
            InitializeComponent();
        }
        public void JsonData(object jsonData)
        {

            var json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);


            JsonContent.Text = json;
        }


    }
}
