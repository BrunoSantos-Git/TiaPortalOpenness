   M S B A M L   `   `   `  ����   TiaPortalOpennessDemo+    %TiaPortalOpennessDemo.Views.RobotView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Generate Symbolic�� � SizeToContent$ WidthAndHeight=� � 
ResizeMode$ NoResize=� � Icon$; 5/TiaPortalOpennessDemo;component/Images/kuka_icon.ico��.���� ���#3B4461.+��� � $	��650�� � RowDefinitions �� $	��29* ��� $
��101* �.+��� $��0��$��
310,5,30,0q�$	��140��$��Center=�$;��5/TiaPortalOpennessDemo;component/Images/kuka_icon.ico���� 
��� ��� �� Padding$ 3q� �� CornerRadius$ 5��$��311,120,53,0q�$	��Top=�$��Center=�._��� -   $��robNameImage��$��	000000R00�� �� TextAlignment$ Center=�$��Arial Black)�$��14(�$
��Bold"��� -   $��RobName�� �� 	MaxLength$ 9��	 �� TextWrapping$
	 Wrap=�$��	80,18,0,0q�$��14(�$��20��$	��Top=�$
��Left=�$	��195���� $��Name:��$��18(�$��Segoe UI Historic)�$
��Bold"����	#FFF4F4F4$��	10,10,0,0q�$	��Top=�$
��Left=�$��34��$��63���� -   $��StartAddress��$
	 Wrap=�$��14(�$ 9��$��
135,50,0,0q�$��20��$
��Left=�$	��Top=�$	��140���� $��StartAddress:��$��18(�$��Segoe UI Historic)�$
��Bold"����	#FFF4F4F4$��	10,42,0,0q�$	��Top=�$
��Left=�$��34��$	��120���� -   $��	cbRobSafe��$��
105,81,0,0q�$��12(�$��0��$��20��$	��Top=�$
��Left=�$	��170��.@��� $��Range Monitoring���� $��	Operation���� $��	Rob Safe:��$��18(�$��Segoe UI Historic)�$
��Bold"����	#FFF4F4F4$��	10,73,0,0q�$��35��$	��Top=�$
��Left=�$��0��$��87���� -   
 ��Name$
 cbType��$��
70,114,0,0q�$��12(�$��0��$��20��$	��Top=�$
��Left=�$	��205��.@��� $��Basic Slave���� $��Laser Slave���� $��Type:��$��18(�$��Segoe UI Historic)�$
��Bold"����	#FFF4F4F4$��
10,104,0,0q�$��0��$	��Top=�$
��Left=�$��34��$��54���� $��
10,10,10,0q�$	��Top=�$��Stretch=����	#FFDBDCDC$��1��$��1���� $
��Left=�$��	10,17,0,0q�$
��Wrap=�$��Tecnologies:��$	��Top=�$��20(�$
��Bold"�$��Segoe UI Historic)����	#FFF4F4F4$��29��$	��115��$��1�� J� RenderTransformOrigin$ 
0.51,0.177&�&   System.Windows.Controls.DataGrid  -   $��datagrid�� %� IsVirtualizing .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto $
��Auto��$	��370��$��0,55,0,0q�$	��Top=�   AlternatingRowBackground
 ����� �� AlternationCount$ 2��   AutoGenerateColumns .     CanUserResizeRows .  $��1��$��Stretch=�����    Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��   
IsReadOnly .    CellTemplate �� .R��� $��14(�����      Width$ 25    CanUserResize .   �� .R��� $��tecCheck�� d� 	IsChecked ��  �� Mode$ OneWay=��� -    ��Name$ cbImportToTia��$��Import tags to TIA��$��	10,0,0,55q�$��Bottom=�$
��Left=�$��1��$��14(�$��Segoe UI Historic)����	#FFF4F4F4$��18��$	��132��{��� *IsTiaConnected+�� -	   $��CreateRobot��$��
10,0,10,10q�$��37��$��Bottom=�$��1������  �� EndPoint$ 0.5,1&� � MappingMode$ RelativeToBoundingBox=� �� 
StartPoint$ 0.5,0&�.���  � Color$ White��� $ 	#FFDBDCDC�� � Offset$ 1Q�.����   �� Orientation$  
Horizontal=�.+��� $��Create  ��$ Center=�$��16(�$��Segoe UI Emoji)��� $=��7/TiaPortalOpennessDemo;component/Images/Create-icon.png��$��20��