   M S B A M L   `   `   `  ����U  Qnmct.project.ui.medewerker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:nmct.project.ui.medewerker.ViewModel$nmct.project.ui.medewerker.ViewModel  5      0    *nmct.project.ui.medewerker.View.Mainscreen   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Avm2clr-namespace:nmct.project.ui.medewerker.ViewModel     5        � DataContext  5	      7  1nmct.project.ui.medewerker.ViewModel.MainscreenVM  5
   
   .��5      �  � RowDefinitions 5   
   �� 5      $��80 �6   �� 5      $��70 �6   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��30 �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $	��300 �6   �� 5      $	��300 �6   .+�5   
   �� $	��150��6   $��3��6!    	  blauw#��[  65   $��2��6S   �� 5   
     mainscreen_logout#��[ 6   $��2��6<   ��6L   �� *DataContext.ChangePageCommand+ �� RelativeSource ��  �� AncestorType" ���6�   �� *DataContext.Pages[2]+ �� " �.��6,  Klant Uitloggen6=  �� 5   
     mainscreen_title#��[ 6   ��6>   �� *Name+�� 5   
     mainscreen_subtitle#��[ 6   $��1��6A   ��6N   �� *)'DataContext.ActiveEmployee.EmployeeName+ �� " ��� 5   
   $��2��6   $��5��6$   $
��Auto=�65   $��10q�6X   .��5       ��   product_template#��[ 6?     paneltemplate#��[ 6p   ��6   �� *AllProducts+6p   5!      �� 5"   
   $��2��6   $��5��6!   $��1��62     donkergrijs#��[ 6B   �� 5#   
   $��
Bestelling��6   $��1��6&   $��2��66     bestelling_style#��[ 6C   �� 5$   
   $��1��6   $��3��6'   $
��Auto=�64   .��5%      ��   product_list_template#��[ 6D   ��6   �� *SelectedProducts+6D   5&      �� 5'   
   $��1��6Z   $��4��6j    	 login_subtitel#��[	 6w   ��6   �� *	TotalCost+ 
 PrijsConverter �� 	Converter# [
 6w   �� 5(   
   $��Bestelling afronden��6   $��1��6/   $��5��6?     button_Style#��[ 6L   ��6r   �� *BestellingCommand+�� 5)   
   $��1��6+   $��6��6;     error_message#��[ 6H   ��6   �� *Error+6H   �� 5*   
   $��Klant��6   $��2��6!   $��2��61   #��[ 6>   � 5+   
   $��3��6   $��2��6   $��4��6,    5,      �� 5-      $	��200 �6    �� 5.      $��40 �6    �� 5/      $��40 �6    �� 50      $��40 �6    �� 51      $��40 �6    .+�53      �� ��6   �� *$"DataContext.ActiveCustomer.Picture+ �� " �  ImageConverter# [ �� 54      $
��Naam��6     customer_info#��[ 6$   $��1��6K   �� 55        customer_info_bold#��[ 6�   $��2��6�   ��6   �� *)'DataContext.ActiveCustomer.CustomerName+ �� " �6�   �� 56      $��Huidig Saldo��6   #��[ 6,   $��3��6S   �� 57      #��[ 6�   $��4��6�   ��6   �� *$"DataContext.ActiveCustomer.Balance+ �� " �  EuroConverter# [ 6�   58      59      5:      