   M S B A M L   `   `   `  ????J  FGoodLineTestWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:GoodLineTestWPFGoodLineTestWPF  5   	        GoodLineTestWPF.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:GoodLineTestWPF     5   	     ? Title$  
MainWindow??5   	   $	??550??6   $	??567??6)   .??5	      ? -   5        $??MainGrid??5	      $??0,0,0,0q?6   .+?5
   
   ?? -   5         ??Name$ ServerNameTextBox??5
      $??Stretch=?6-   $??23??6K   $??10,10,10,10q?6W    ?? TextWrapping$
 Wrap=?6l   $-??'Имя или адрес сервера??6?   $	??Top=?6?   $
??Auto??6?   ?? 5   
   -   5        $ DataBaseTextBox??5      $??Stretch=?6+   $??23??6I   $??10,40,10,10q?6U   $
 Wrap=?6j   $"??Имя базы данных??6~   $	??Top=?6?   $
??Auto??6?   ?? 5   
   -   5        $ UserNameTextBox??5      $??Stretch=?6+   $??23??6I   $??10,70,10,10q?6U   $
 Wrap=?6j   $%??Имя пользователя??6~   $	??Top=?6?   $
??Auto??6?   G? 5   
   -   5         G?Name$ PasswordTextBox??5      $??Stretch=?6/   $??10,100,10,10q?6M   $	??Top=?6c   $
??Auto??6{   $??23??6?    G? Password$ Пароль??6?   ?? 5   
   -   5        $ ??Соединить с БД??5      $??Stretch=?6*   $??10,130,10,10q?6H   $	??Top=?6^   $
??Auto??6v   $??23??6?   6?   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ MerchandiseDataGrid??5      $??Stretch=?60   $??10,160,10,40q?6N   $??Stretch=?6d   $
??Auto??6?   $
??Auto??6?      AutoGenerateColumns .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Наименование??6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 * 6Y   -   5        
nameColumn??1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 5   ;   ??  ?? Path$
 name?6y     5      $ Цена, руб.??6%   $	 * 69   -	   5        priceColumn??
 5   C   ?? $ price? ?? StringFormat$ {0:F2}??6?   4  .System.Windows.Controls.DataGridComboBoxColumn  5      $ Категория??6)   $	 * 6=      SelectedValuePath$ id??6t      DisplayMemberPath$
 name??6?   -
   5        categoryColumn??   SelectedValueBinding 5   G   ?? *id_category+6?   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ Дата??6)   -   5        
dateColumn??   SortMemberPath$ dt??5   K      CellTemplate 5      ?? 5      .R?5      (  "System.Windows.Controls.DatePicker  -   5        $??0q?5   E      SelectedDate 6)   ?? *dt+6Y   5      5         ColumnHeaderStyle 5      ?? 5       ?? 
TargetType=	  7System.Windows.Controls.Primitives.DataGridColumnHeader" 	 6   .?5      ??  ?? Property ? ?? ?? Value$ 60??6/   ?? 5        ? ??$	 Top=?6A   ?? 5!       ? ??$ Center=?6C   5"      5$      ?? 5&   
   $
??Left=?6    ?? Orientation$ 
Horizontal=?60   $??5,0,0,40q?6I   $??Bottom=?6\   .+?5'      ?? -   5         ??Name$ sumlabel??5'      $??Сумма ??6*   $??5,0,0,0q?68   $??22??6?    ?? TextAlignment$ Right=?6?   ??6I   ??  ?? ElementName$ 
nameColumn??$ ActualWidth? ?? Mode$ OneWay=?6?   ?? 5(      $??1q?6   
???   ?$??22??6=   $??0,0,0,0q?6J   
???????._?5)      ?? -   5        $ sumtextblock??5)      $?? ??62   $??5,0,0,0q?6:   $??22??6?   ??6K   ?? $ priceColumn??$ ActualWidth?$ OneWay=?6?   5*      ?? 5+      -   5        $ 
countlabel??5+      $??Количество ??6,   $??0,0,0,0q?6?   $??22??6?   $
 Left=?6?   ??6P   ?? $ categoryColumn??$ ActualWidth?$ OneWay=?6?   ?? 5,      $??1q?6   
???   ?$??22??6=   $??-7,0,0,0q?6J   
???????._?5-      ?? -   5        $ counttextblock??5-      $?? ??64   $??0,0,0,0q?6<   $??22??6?   ??6M   ?? $ 
dateColumn??$ ActualWidth?$ OneWay=?6?   5.      5/      ?? 51   
   -   5         ??Name$ SubmitButton??51      $+??%Применить изменения??6'   $
??Left=?6E   $??	10,0,0,10q?6`   $??Bottom=?6s   $	??158??6?   $??23??6?   6?   52      53      