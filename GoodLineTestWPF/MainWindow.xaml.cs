using System;
using System.Text;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;


namespace GoodLineTestWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataSet MainDataSet;
        private SqlDataAdapter DataAdapterMerchandise;

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataAdapterMerchandise.Update(MainDataSet.Tables["Merchandise"]);
                SumUpPrices(String.Empty);

                foreach (var item in MerchandiseDataGrid.Items)
                {
                    DataGridRow row = (DataGridRow)MerchandiseDataGrid.ItemContainerGenerator.ContainerFromItem(item);
                    row.Background = new SolidColorBrush(Colors.White);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка обновления источника данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SumUpPrices(string filter)
        {//вычисляем сумму 
            try
            {
                int sum = Convert.ToInt32(MainDataSet.Tables["Merchandise"].Compute("SUM(Price)", filter));
                int count = Convert.ToInt32(MainDataSet.Tables["Merchandise"].Compute("COUNT(Price)", filter));
                sumtextblock.Text = sum.ToString();
                counttextblock.Text = count.ToString();
            }
            catch
            {
                return;
            }
        }

        private void FillMerchandiseGrid()
        {
            MerchandiseDataGrid.AutoGenerateColumns = false;//выключаем автогенерацию столбцов грида (чтобы потом самостоятельно создать столбец с выпадающим списком)
            MerchandiseDataGrid.CellEditEnding += MerchandiseDataGrid_CellEditEnding;
            MerchandiseDataGrid.RowBackground = new SolidColorBrush(Colors.White);
            StringBuilder sb = new StringBuilder();
            sb.Append("Select * from dbo.Merchandise");

            DataAdapterMerchandise = new SqlDataAdapter(sb.ToString(), Properties.Settings.Default.ConnectionString);//создаём адаптер
            //описываем команды адаптера для связи с источником данных
            //команда обновления
            SqlCommand cmd = new SqlCommand("Update dbo.Merchandise set id_category = @id_category, name = @name, price = @price, dt = @dt where id = @old_id", DataAdapterMerchandise.SelectCommand.Connection);
            cmd.Parameters.Add("@id_category", SqlDbType.Int, 10, "id_category");//задаём параметры
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150, "name");//задаём параметры
            cmd.Parameters.Add("@old_id", SqlDbType.Int, 10, "id");//этот параметр особый
            cmd.Parameters.Add("@price", SqlDbType.Money, 100, "price");//цена
            cmd.Parameters.Add("@dt", SqlDbType.DateTime, 100, "dt");//дата-время
            cmd.Parameters["@old_id"].SourceVersion = DataRowVersion.Original;
            DataAdapterMerchandise.UpdateCommand = cmd;
            //команда вставки
            cmd = new SqlCommand("Insert into dbo.Merchandise (id_category, name, price, dt) values (@id_category, @name, @price, @dt)", DataAdapterMerchandise.SelectCommand.Connection);
            cmd.Parameters.Add("@id_category", SqlDbType.Int, 10, "id_category");//задаём параметры
            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150, "name");//задаём параметры
            cmd.Parameters.Add("@price", SqlDbType.Money, 100, "price");//цена
            cmd.Parameters.Add("@dt", SqlDbType.DateTime, 100, "dt");//дата-время
            DataAdapterMerchandise.InsertCommand = cmd;
            //команда удаления
            cmd = new SqlCommand("Delete from dbo.Merchandise where id = @id", DataAdapterMerchandise.SelectCommand.Connection);
            cmd.Parameters.Add("@id", SqlDbType.Int, 10, "id");//задаём параметры
            DataAdapterMerchandise.DeleteCommand = cmd;
            //привязку таблицы к грида
            MerchandiseDataGrid.ItemsSource = MainDataSet.Tables["Merchandise"].DefaultView;
            //пытаемся создать lookup поле для столбца категорий товаров
            DataGridComboBoxColumn dgvcbc = (DataGridComboBoxColumn)MerchandiseDataGrid.Columns[2];
            dgvcbc.ItemsSource = MainDataSet.Tables["Categories"].DefaultView;//источник данных столбца
            SumUpPrices(String.Empty);
        }

        private void MerchandiseDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            e.Row.Background = new SolidColorBrush(Colors.BlanchedAlmond);
        }

        private void CreateFilters(DataGrid dgv)
        {
            ToolBar tb = new ToolBar();
            ToolBarTray tbt = new ToolBarTray();
            
            ToolBar.SetOverflowMode(tb, OverflowMode.Never);
            
            tb.Background = Brushes.Yellow;
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Top;
            tb.Height = 30;
            tb.Width = MerchandiseDataGrid.Width;
            
            tb.Margin = new Thickness(5, 0, 0, 0);
            tbt.Margin = new Thickness(5, 192, 0, 0);
            tbt.VerticalAlignment = VerticalAlignment.Top;

            tbt.Width = tb.Width;
            tbt.Height = tb.Height;
            tbt.IsLocked = true;
            
            tb.Loaded += ToolBar_Loaded;
            tbt.ToolBars.Add(tb);
    
            MainGrid.Children.Add(tbt);
           
            foreach (DataGridColumn col in dgv.Columns)
            {//создаём поля-фильтры
                TextBox b = new TextBox();
 
                b.Height = 20;
                //создаём привязку свойства ширины столбца грида к свойству ширины текстового поля
                Binding bind = new Binding();
                bind.Source = col;
                bind.Path = new PropertyPath("ActualWidth");
                b.SetBinding(TextBox.WidthProperty, bind);

                //добавляем текстовые поля
                b.TextChanged += FilterApply;
                tb.Items.Add(b);
                col.SetValue(TagProperty, b);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateFilters(MerchandiseDataGrid);
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
     
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private string BuildQueryString(DataGrid dgv, DataTable lookuptable = null, string lookupfield = "")
        {
            StringBuilder QueryString = new StringBuilder("1=1");
            //составляем запрос на основании строковых полей поиска из грида
            //циклимся по столбцам грида
            foreach (DataGridColumn column in dgv.Columns)
            {
                TextBox tstb = (TextBox)column.GetValue(TagProperty);//получаем текстовое поле которое поместили в столбец
                DataView dv = (DataView)dgv.ItemsSource;//получаем представление данных источника грида
                DataTable dt = dv.ToTable();//преобразуем представление в таблицу
                string typeColumn = column.GetType().ToString();//тип столбца грида
                string typeData = String.Empty;//типа данных столбца грида
                string datacolumnname = String.Empty;              

                if (typeColumn == "System.Windows.Controls.DataGridTextColumn")
                {
                    DataGridTextColumn dgtc = (DataGridTextColumn)column;
                    Binding b = (Binding)dgtc.Binding;//получаем объект привязки столбца
                    datacolumnname = b.Path.Path;//получаем путь в привязке (фактически имя столбца таблицы)
                    int columnindex = dt.Columns.IndexOf(datacolumnname);//получаем порядковый номер столбца по имени
                    DataColumn dc = dt.Columns[columnindex];//получаем объект столбца по порядковому номеру
                    typeData = dc.DataType.ToString();//получаем тип данных столбца
                    
                    switch (typeData)
                    {//когда работаем с простым строковым столбцом 
                        case "System.String":
                            QueryString.Insert(QueryString.Length, " and " + datacolumnname + " like '%" + tstb.Text + "%'");
                            break;
                        case "System.Decimal":
                            {
                                if (tstb.Text != String.Empty)
                                {
                                    string firstChar = tstb.Text.Substring(0, 1);//смотрим какой символ первый в строке
                                    string value = tstb.Text.Remove(0, 1);
                                    QueryString.Insert(QueryString.Length, " and " + datacolumnname + " " + firstChar + value);
                                   
                                }
                                break;
                            }
                    }
                
                }
                //если столбец-шаблон
                if (typeColumn == "System.Windows.Controls.DataGridTemplateColumn")
                {
                    DataGridTemplateColumn dgtc = (DataGridTemplateColumn)column;
                    DependencyObject t = dgtc.CellTemplate.LoadContent();//получаем объект который хранится в столбце-шаблоне
                    if (t != null)
                    {//в данном случае мы храним в столбце-шаблоне календарь
                        if (t.DependencyObjectType.Name == "DatePicker")
                        {
                            DatePicker dp = (DatePicker)t;//получаем экземпляр объекта календаря
                            BindingExpression be = dp.GetBindingExpression(DatePicker.SelectedDateProperty);//получаем выражение привязки которое хранит календарь 
                            if (be != null)
                            {
                                Binding b = be.ParentBinding;//получаем саму привязку
                                datacolumnname = b.Path.Path;//получаем путь привязки. В данном случае это имя столбца таблицы с данными
                                int columnindex = dt.Columns.IndexOf(datacolumnname);//получаем порядковый номер столбца по имени
                                DataColumn dc = dt.Columns[columnindex];//получаем объект столбца по порядковому номеру
                                typeData = dc.DataType.ToString();//получаем тип данных столбца
                                switch (typeData)
                                {
                                    case "System.DateTime":
                                        {
                                            if (tstb.Text.Length < 11)//если меньше 11 символов, то возвращаем всё
                                                QueryString.Insert(QueryString.Length, " and " + datacolumnname + ">='01.01.1900'");
                                            else///иначе есть смысл применять фильтр по дате                        
                                            {
                                                QueryString.Insert(QueryString.Length, " and " + datacolumnname + tstb.Text + "'");
                                                QueryString.Insert(QueryString.Length - 11, "'");
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                //если lookup выпадающий список (набор строк)
                if (lookuptable != null & lookupfield != String.Empty & typeColumn == "System.Windows.Controls.DataGridComboBoxColumn")
                {
                    DataGridComboBoxColumn dgvcbc = (DataGridComboBoxColumn)column;
                    Binding b = (Binding)dgvcbc.SelectedValueBinding;
                    datacolumnname = b.Path.Path;
                    lookuptable.DefaultView.RowFilter = lookupfield + " like '%" + tstb.Text + "%'";//фильтруем таблицу чтобы вытащить идентификатор нужного объекта
                    //нужно проциклиться по всем полученным строкам чтобы получить весь список подходящих идентификаторов
                    StringBuilder ids = new StringBuilder(" in (");
                    DataTable filtered = lookuptable.DefaultView.ToTable();
                    foreach (DataRow row in filtered.Rows)
                    {
                        ids.Append(row["id"] + ", ");//накидываем идентификаторы в строку для предложения in
                    }

                    ids.Remove(ids.Length - 2, 2);//удаляем последнюю лишнюю запятую
                    ids.Append(")");//добавляем завершающую скобку предложения in

                    QueryString.Insert(QueryString.Length, " and " + datacolumnname + ids.ToString());//непосрелственно формируем нужный запрос с участием идентификатора
                }
            }
            return QueryString.ToString();
        }

        private void DateTimePickerSelectedDate(object sender, SelectionChangedEventArgs e)
        {     
            MerchandiseDataGrid.BeginEdit();
        }


        private void FilterApply(object sender, RoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            
            if (FilterByValue(MerchandiseDataGrid, BuildQueryString(MerchandiseDataGrid, MainDataSet.Tables["Categories"], "name")) == -1)//если ничего не нашли
            {
                return;
            }

            
        }

        private int FilterByValue(DataGrid dgv, string query)
        {//фильтр грида по указаннному запросу
            try
            {
                MerchandiseDataGrid.CancelEdit();
                if (dgv.ItemsSource.GetType() == typeof(DataView))
                {
                    DataView dv = (DataView)dgv.ItemsSource;
                    dv.RowFilter = query;
                    SumUpPrices(query);
                }

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        private void ConnectDB(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            DB db = new DB();

            Exception ex = db.EstablishConnection(ServerNameTextBox.Text, DataBaseTextBox.Text, UserNameTextBox.Text, PasswordTextBox.Password);
            if (ex != null)
            {
                MessageBox.Show("Не могу соединиться с базой данных", "Ошибка соединения", MessageBoxButton.OK, MessageBoxImage.Error);
                Cursor = Cursors.Arrow;
                return;
            }

            MainDataSet = db.ReturnDataSet();
            FillMerchandiseGrid();
            CreateFilters(MerchandiseDataGrid);
            Cursor = Cursors.Arrow;
        }
    }
}
