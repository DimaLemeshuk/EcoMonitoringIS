using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace EcoMonitoringIS.Classes
{
    internal class DBGridControl
    {
        public static void AddColumn(DataGrid DBGrid, string newName, string PropertyName)
        {

            DBGrid.AutoGenerateColumns = false;
            DBGrid.Columns.Add(new DataGridTextColumn
            {
                Header = newName,
                Binding = new Binding(PropertyName)
            });

        }

        public static void DelOllColumn(DataGrid DBGrid)
        {
            DBGrid.Columns.Clear();
            DBGrid.AutoGenerateColumns = true;
        }

    }
}
