using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        public static void AddColumn(DataGrid DBGrid, string newName, string PropertyName, bool ReadOnly)
        {

            DBGrid.AutoGenerateColumns = false;
            DBGrid.Columns.Add(new DataGridTextColumn
            {
                Header = newName,
                Binding = new Binding(PropertyName),
                IsReadOnly = ReadOnly
            });

        }

        public static void AddColumn(DataGrid DBGrid, string newName, string PropertyName, bool ReadOnly, string format)
        {

            DBGrid.AutoGenerateColumns = false;
            DBGrid.Columns.Add(new DataGridTextColumn
            {
                Header = newName,
                Binding = new Binding(PropertyName)
                {
                    StringFormat = format//"#.##"
                },
                IsReadOnly = ReadOnly
            });

        }

        public static void DelOllColumn(DataGrid DBGrid)
        {
            DBGrid.Columns.Clear();
            DBGrid.AutoGenerateColumns = true;
        }

        public static void CopyColumnNames(DataGrid dataGrid1, DataGrid dataGrid2)
        {

            List<string> columnNames = new List<string>();

            // Перевірка, чи імена стовпців були вже отримані

            if (columnNames.Count == 0)
            {
                // Отримати імена стовпців з першого DataGrid (dataGrid1)
                foreach (DataGridColumn column in dataGrid1.Columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        columnNames.Add(textColumn.Header.ToString());
                    }
                }
            }

            if (dataGrid2.Items.Count == 0)
            {
                DataTable dataTable = new DataTable();
                foreach (string columnName in columnNames)
                {
                    dataTable.Columns.Add(columnName, typeof(string));
                }

                dataGrid2.ItemsSource = dataTable.DefaultView;
            }
        }

    }
}
