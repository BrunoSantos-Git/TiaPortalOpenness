   M S B A M L   `   `   `  ����  TiaPortalOpennessDemo+    %TiaPortalOpennessDemo.Views.RobotView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Generate Symbolic��5   	    � SizeToContent$ WidthAndHeight=�6#    � 
ResizeMode$ NoResize=�6B    � Icon$; 5/TiaPortalOpennessDemo;component/Images/kuka_icon.ico��5   	   .��5      �� ���#3B4461.+�5   
   �� � 5	   
   $	��650��6    � RowDefinitions 5
      �� 5      $	��29* �6    �� 5      $
��101* �6    .+�5      �� $��0��6   $��
310,5,30,0q�6!   $	��140��65   $��Center=�6A   $;��5/TiaPortalOpennessDemo;component/Images/kuka_icon.ico��6^   �� 5      
��� ��� �� Padding$ 3q�6)    �� CornerRadius$ 5��65   $��311,120,53,0q�6F   $	��Top=�6\   $��Center=�6t   ._�5      �� -   5        $��robNameImage��5      $��	000000R00��60    �� TextAlignment$ Center=�6A   $��Arial Black)�6X   $��14(�6q   $
��Bold"�6   5      �� 5      -   5        $��RobName��5       �� 	MaxLength$ 9��6%   	 �� TextWrapping$
	 Wrap=�63   $��	80,18,0,0q�6G   $��14(�6Z   $��20��6h   $	��Top=�6t   $
��Left=�6�   $	��195��6�   �� 5      $��Name:��6   $��18(�6$   $��Segoe UI Historic)�62   $
��Bold"�6Q   ���	#FFF4F4F4$��	10,10,0,0q�6z   $	��Top=�6�   $
��Left=�6�   $��34��6�   $��63��6�   �� 5      -   5        $��StartAddress��5      $
	 Wrap=�6*   $��14(�6>   $ 9��6L   $��
135,50,0,0q�6Z   $��20��6n   $
��Left=�6z   $	��Top=�6�   $	��140��6  �� 5      $��StartAddress:��6   $��18(�6,   $��Segoe UI Historic)�6:   $
��Bold"�6Y   ���	#FFF4F4F4$��	10,42,0,0q�6�   $	��Top=�6�   $
��Left=�6�   $��34��6�   $	��120��6�   �� 5      -   5        $��	cbRobSafe��5      $��
105,81,0,0q�6(   $��12(�6<   $��0��6J   $��20��6W   $	��Top=�6c   $
��Left=�6{   $	��170��6�   .@�5      �� $��Range Monitoring��6   �� 5      $��	Operation��6   5      �� 5      $��	Rob Safe:��6   $��18(�6(   $��Segoe UI Historic)�66   $
��Bold"�6U   ���	#FFF4F4F4$��	10,73,0,0q�6~   $��35��6�   $	��Top=�6�   $
��Left=�6�   $��0��6�   $��87��6�   �� 5      -   5        
 ��Name$
 cbType��5      $��
70,114,0,0q�6'   $��12(�6;   $��0��6I   $��20��6�   $	��Top=�6�   $
��Left=�6�   $	��205��6�   .@�5      �� $��Basic Slave��6   �� 5      $��Laser Slave��6   5      �� 5       $��Type:��6   $��18(�6$   $��Segoe UI Historic)�62   $
��Bold"�6Q   ���	#FFF4F4F4$��
10,104,0,0q�6z   $��0��6�   $	��Top=�6�   $
��Left=�6�   $��34��6�   $��54��6�   �� 5!      $��
10,10,10,0q�6   $	��Top=�6,   $��Stretch=�6D   ���	#FFDBDCDC$��1��6u   $��1��6�   �� 5"      $
��Left=�6   $��	10,17,0,0q�63   $
��Wrap=�6F   $��Tecnologies:��6Z   $	��Top=�6n   $��20(�6�   $
��Bold"�6�   $��Segoe UI Historic)�6�   ���	#FFF4F4F4$��29��6�   $	��115��6�   $��1��6�    J� RenderTransformOrigin$ 
0.51,0.177&�6  &   System.Windows.Controls.DataGrid  5#      -   5        $��datagrid��5#       %� IsVirtualizing .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6v   $
��Auto��6�   $	��370��6�   $��0,55,0,0q�6�   $	��Top=�6�      AlternatingRowBackground
 ����� �� AlternationCount$ 2��6
     AutoGenerateColumns .     CanUserResizeRows .  $��1��6U  $��Stretch=�6b  ��6�   ��    Columns 5$      4  .System.Windows.Controls.DataGridTemplateColumn  5%      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6-      
IsReadOnly .    CellTemplate 5&      �� 5'      .R�5(   "   �� $��14(�6=   ��6,   �� 6=   5)      5+        5-         Width$ 25 6-      CanUserResize .   5.      �� 5/      .R�50   "   �� $��tecCheck��6M    d� 	IsChecked 6+   ��  �� Mode$ OneWay=�6M   51      53      55      �� 56      -   5         ��Name$ cbImportToTia��56      $��Import tags to TIA��6.   $��	10,0,0,55q�6K   $��Bottom=�6�   $
��Left=�6�   $��1��6�   $��14(�6�   $��Segoe UI Historic)�6�   ���	#FFF4F4F4$��18��6
  $	��132��6  {�6^   �� *IsTiaConnected+6  �� 57      -	   5        $��CreateRobot��57      $��
10,0,10,10q�6(   $��37��6V   $��Bottom=�6b   $��1��6}   ��58      �� 59       �� EndPoint$ 0.5,1&�6*    � MappingMode$ RelativeToBoundingBox=�6;    �� 
StartPoint$ 0.5,0&�6_   .��5:      �  � Color$ White��6'   � 5;      $ 	#FFDBDCDC��6'    � Offset$ 1Q�69   5<      .��5>      ��   �� Orientation$  
Horizontal=�6   .+�5?      �� $��Create  ��6    $ Center=�60   $��16(�6G   $��Segoe UI Emoji)�6U   �� 5@      $=��7/TiaPortalOpennessDemo;component/Images/Create-icon.png��6   $��20��6]   5A      5B      5C      5D      5E      