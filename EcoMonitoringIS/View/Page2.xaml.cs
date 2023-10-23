using EcoMonitoringIS.Classes;
using EcoMonitoringIS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;


namespace EcoMonitoringIS.View
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private EcomonitoringdbContext context;
        DataTableCollection? collection;
        Button CurBtn;
        public Page2()
        {
            InitializeComponent();
            EcomonitoringdbContext _context = new EcomonitoringdbContext();
            context = _context; 

        }

        private void EnterprisesTable_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new EcomonitoringdbContext())
            {

                DBGridControl.DelOllColumn(DBGrid);

                DBGrid.ItemsSource = context.Enterprises
                    .Include(e => e.Belonging)
                    .ToList();

                DBGridControl.AddColumn(DBGrid, "id", "Identerprise");
                DBGridControl.AddColumn(DBGrid, "Назва", "Name");
                DBGridControl.AddColumn(DBGrid, "Вид діяльності", "Activity");
                DBGridControl.AddColumn(DBGrid, "Належність", "Belonging.Name");
                DBGridControl.AddColumn(DBGrid, "Адреса", "Addres");


                ChooseT.Text = ((Button)sender).Content.ToString();
                CurBtn = (Button)sender;
            }
        }

        private void pollution_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGrid.ItemsSource = context.Pollutions
                .Include(p => p.Enterprise)
                .Include(p => p.Pollutant)
                .ToList();
                DBGridControl.DelOllColumn(DBGrid);
                DBGrid.ItemsSource = context.Pollutions.ToList();
                DBGridControl.AddColumn(DBGrid, "id", "Idpollution");
                DBGridControl.AddColumn(DBGrid, "Підприємство", "Enterprise.Name");
                DBGridControl.AddColumn(DBGrid, "Забрудник", "Pollutant.Name");
                DBGridControl.AddColumn(DBGrid, "Викидив(т/рік)", "ValueMfr");
                DBGridControl.AddColumn(DBGrid, "(%)від заг. викидів", "Percent");
                DBGridControl.AddColumn(DBGrid, "Рік", "Year");

            }
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;

        }

        private void pollutant_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGridControl.DelOllColumn(DBGrid);
                DBGrid.ItemsSource = context.Pollutants.ToList();
                DBGridControl.AddColumn(DBGrid, "id", "Idpollutant");
                DBGridControl.AddColumn(DBGrid, "Назва речовини", "Name");
                DBGridControl.AddColumn(DBGrid, "Клас небезпеки", "DangerClass");
                DBGridControl.AddColumn(DBGrid, "Граничнодопустимі\n викиди (мг/м3)", "Gdk");
                DBGridControl.AddColumn(DBGrid, "Величина масової\n витрати (г/год)", "Mfr");
            }
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;
        }

        private void belonging_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGridControl.DelOllColumn(DBGrid);
                DBGrid.ItemsSource = context.Belongings.ToList();
                DBGridControl.AddColumn(DBGrid, "id", "Idbelonging");
                DBGridControl.AddColumn(DBGrid, "Відомча належність", "Name");
            }
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;
        }

        private void results_Click(object sender, RoutedEventArgs e)
        {
            using (EcomonitoringdbContext context = new EcomonitoringdbContext())
            {
                DBGridControl.DelOllColumn(DBGrid);
                DBGrid.ItemsSource = context.Results.ToList();
                DBGridControl.AddColumn(DBGrid, "id", "Idresults");
                DBGridControl.AddColumn(DBGrid, "Перевищення", "Exceeding");
                DBGridControl.AddColumn(DBGrid, "id забрудника", "PollutionId");
            }
            ChooseT.Text = ((Button)sender).Content.ToString();
            CurBtn = (Button)sender;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var path = FileControl.OpenFileNameDialog();
            PathTextBox1.Text = path;
            //DisplayTable(path);

        }

        private void PathTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            DisplayTable(PathTextBox1.Text);
        }

        public void DisplayTable(string path)
        {

            if (File.Exists(path))
            {
                ExcelPageNum.Content = "1";
                collection = ExcelControl.ExelToTableColl(path);
                DataTable table = collection[(Int32.Parse((string)ExcelPageNum.Content))-1];
                DBGrid2.ItemsSource = table.DefaultView;
                navExPage.Visibility = Visibility.Visible;
            }

        }

        private void fillButton_Click(object sender, RoutedEventArgs e)
        {
            int? p = (Int32.Parse(ExcelPageNum.Content.ToString())) - 1;
            if (p != null)
            {
                if (p == 0 && ChooseT.Text == "Enterprises")
                {
                    FillEnterprises(collection[0]);
                }
                else if (p == 1 && ChooseT.Text == "Pollutions")
                {
                    FillPollution(collection[1]);
                }
                else if (p == 2 && ChooseT.Text == "Pollutants")
                {
                    FillPollutant(collection[2]);
                }
            }

        }
        ///-------------------------------------------------FillDBfromExcel---------------------------------------------------------------

        public static void FillEnterprises(DataTable table)
        {
            using (var db = new EcomonitoringdbContext())
            {
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Enterprise e = new Enterprise();
                        e.Initialize(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString());
                        db.Enterprises.Add(e);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка1: " + ex.Message);
                }
            }

        }

        public static void FillPollution(DataTable table)
        {
            using (var db = new EcomonitoringdbContext())
            {
                try
                {
                    string enterpriseName = "";
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if ((table.Rows[i][3]).ToString().Equals(""))
                        {
                            enterpriseName = table.Rows[i][0].ToString();
                        }
                        else
                        {
                            string pollutantName = table.Rows[i][0].ToString();
                            double ValueMFR = (double)table.Rows[i][1];
                            double Percent = (double)table.Rows[i][2];
                            int year = Int32.Parse(table.Rows[i][3].ToString());
                            var p = new Pollution();
                            p.Initialize(enterpriseName, pollutantName, ValueMFR, Percent, year);
                            db.Pollutions.Add(p);
                        }
                        
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка2: " + ex.Message);
                }
            }

        }

        public static void FillPollutant(DataTable table)
        {
            using (var db = new EcomonitoringdbContext())
            {
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                    var s = row[0].ToString();
                    var b = Int32.Parse(row[1].ToString());
                    var c = (double)row[2];
                    var d = (double)row[3];
                        db.Pollutants.Add(new Pollutant(row[0].ToString(), Int32.Parse(row[1].ToString()), (double)row[2], (double)row[3]));
                    }
                    db.SaveChanges();
            }
                catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка3: " + ex.Message);
            }
        }

        }


        ///--------------------------------------------------------------------------------------------------------------------

        private void navBefore_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int p = (Int32.Parse(ExcelPageNum.Content.ToString()))-1;
            if (collection != null && (p - 1 >= 0))
            {
                DBGrid2.ItemsSource = (collection[--p]).DefaultView;
                ExcelPageNum.Content = ++p;
            }
               
        }

        private void navNext_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int p = (Int32.Parse(ExcelPageNum.Content.ToString()))-1;
            if (collection != null && (p + 1 < collection.Count))
            {
                DBGrid2.ItemsSource = (collection[++p]).DefaultView;
                ExcelPageNum.Content = ++p;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurBtn != null)
            {
                CurBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
    }
}
